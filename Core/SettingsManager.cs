using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using Microsoft.Data.Sqlite;
using SiobhanDev;

namespace FocalPointer
{
    class SettingsManager
    {
        public static readonly IReadOnlyDictionary<string, string> EmptySettings;
        static SettingsManager()
        {
            EmptySettings = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
        }

        SqliteConnection _db;
        Dictionary<string, string> _lastVersions;

        public SettingsManager()
        {
            _lastVersions = new Dictionary<string, string>();
        }

        private string[] getTableNames()
        {
            List<string> results = new List<string>();

            var cmd = new SqliteCommand("SELECT name FROM sqlite_master WHERE type = 'table';", _db);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                results.Add(reader[0].ToString());
            }

            return results.ToArray();
        }

        private void dumpSchema()
        {
            var cmd = new SqliteCommand("SELECT * FROM sqlite_master WHERE type = 'table';", _db);
            var reader = cmd.ExecuteReader();

            bool first = true;
            while (reader.Read())
            {
                if (first)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (i > 0)
                            Console.Write("\t");
                        Console.Write(reader.GetName(i));
                    }

                    first = false;
                    Console.WriteLine();
                }

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (i > 0)
                        Console.Write("\t");
                    Console.Write(reader[i].ToString());
                }
                Console.WriteLine();
            }
        }

        private class namespaceFields
        {
            public string LastVersion;
            public IDictionary<string, string> Settings;
            public IDictionary<string, string> Blobs;
        }

        private class settingsSeed
        {
            public IDictionary<string, namespaceFields> Namespaces;
        }

        private void seedTables(string path, HashSet<string> needed)
        {
            if (!File.Exists(path))
                return;

            string json = File.ReadAllText(path);
            var seedData = JsonConvert.DeserializeObject<settingsSeed>(json);

            foreach (var space in seedData.Namespaces.Keys)
            {
                var seed = seedData.Namespaces[space];
                bool storedKvps = false;

                if (needed.Contains(settingsTable) && seed.Settings != null && seed.Settings.Count > 0)
                {
                    doSqlUpsertForNamespace(settingsTable, space, seed.Settings);
                    storedKvps = true;
                }

                if (needed.Contains(blobTable) && seed.Blobs != null && seed.Settings.Count > 0)
                {
                    doSqlUpsertForNamespace(blobTable, space, seed.Settings);
                    storedKvps = true;
                }

                if (storedKvps || needed.Contains(versionTable))
                    storeLastVersion(space, seed.LastVersion);
            }
        }

        private const string versionTable = "last_version";
        private const string settingsTable = "setting";
        private const string blobTable = "blob";
        private const string namespaceColumn = "namespace";
        private const string versionColumn = "version";
        private void verifySchema(string seedPath)
        {
            var seedsNeeded = new HashSet<string>();
            var requiredTables = new HashSet<string>(new string[] { versionTable, settingsTable, blobTable } );

            foreach (string table in getTableNames())
            {
                if (requiredTables.Contains(table))
                    requiredTables.Remove(table);
            }

            if (requiredTables.Count > 0)
            {
                var cmd = new SqliteCommand("", _db);

                foreach (string table in requiredTables)
                {
                    cmd.CommandText = "";

                    switch (table)
                    {
                        case versionTable:
                            cmd.CommandText = $"CREATE TABLE {table} ({namespaceColumn} TEXT PRIMARY KEY, {versionColumn} TEXT) WITHOUT ROWID;";
                            break;

                        case settingsTable:
                        case blobTable:
                            cmd.CommandText = $"CREATE TABLE {table} ({namespaceColumn} TEXT, key TEXT, value TEXT, PRIMARY KEY({namespaceColumn}, key)) WITHOUT ROWID;";
                            break;

                        default:
                            Console.WriteLine($"Don't know how to create table {table}");
                            break;
                    }

                    if (cmd.CommandText != "")
                    {
                        var created = cmd.ExecuteNonQuery();
                        if (created == 0)
                        {
                            Console.WriteLine($"Failed to create table {table}");
                        }

                        seedsNeeded.Add(table);
                    }
                }
            }

            if (seedsNeeded.Count > 0)
            {
                seedTables(seedPath, seedsNeeded);
            }
        }

        public void InitializeFromSqlitePath(string path, string seedPath)
        {
            var builder = new SqliteConnectionStringBuilder();
            builder.DataSource = path;
            builder.Mode = SqliteOpenMode.ReadWriteCreate;
            builder.Cache = SqliteCacheMode.Private;

            _db = new SqliteConnection(builder.ToString());
            _db.Open();

            verifySchema(seedPath);
        }

        private string doSqlQueryValue(string table, string space, string key, bool isColumn)
        {
            var cmd = new SqliteCommand("", _db);

            cmd.Parameters.AddWithValue($"@space", space);

            if (isColumn)
            {
                cmd.CommandText = $"SELECT {key} FROM {table} WHERE {namespaceColumn} = @space;";
            }
            else
            {
                cmd.CommandText = $"SELECT value FROM {table} WHERE {namespaceColumn} = @space AND key = @key;";
                cmd.Parameters.AddWithValue($"@key", key);

            }
            var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;
            else
                return reader[0].ToString();
        }

        private IDictionary<string, string> doSqlQueryDictionary(string table, string space)
        {
            var cmd = new SqliteCommand("", _db);

            cmd.Parameters.AddWithValue($"@space", space);
            cmd.CommandText = $"SELECT key, value FROM {table} WHERE {namespaceColumn} = @space;";

            var reader = cmd.ExecuteReader();

            Dictionary<string, string> results = null;

            while (reader.Read())
            {
                if (results == null)
                    results = new Dictionary<string, string>();
                results.Add(reader[0].ToString(), reader[1].ToString());
            }

            return results;
        }

        private void doSqlUpsert(string table, Dictionary<string, string> columnValuePairs)
        {
            doSqlUpsert(table, columnValuePairs.Unfold());
        }

        private void doSqlUpsert(string table, params string[] columnValuePairs)
        {
            var cmd = new SqliteCommand("", _db);

            var columns = new StringBuilder();
            var parameters = new StringBuilder();

            for (int i = 0; i < columnValuePairs.Length - 1; i += 2)
            {
                if (i > 0)
                {
                    columns.Append(", ");
                    parameters.Append(", ");
                }

                string column = columnValuePairs[i];
                columns.Append(column);
                parameters.Append($"@{column}");
                cmd.Parameters.AddWithValue($"@{column}", columnValuePairs[i + 1]);
            }

            cmd.CommandText = $"REPLACE INTO {table} ({columns}) VALUES ({parameters});";
            cmd.ExecuteNonQuery();
        }

        private void doSqlUpsertForNamespace(string table, string space, IDictionary<string, string> keyValuePairs)
        {
            foreach (var kvp in keyValuePairs)
            {
                doSqlUpsert(table, namespaceColumn, space, "key", kvp.Key, "value", kvp.Value);
            }
        }

        private void doSqlUpsertForNamespace(string table, string space, params string[] keyValuePairs)
        {
            for (int i = 0; i < keyValuePairs.Length - 1; i += 2)
            {
                doSqlUpsert(table, namespaceColumn, space, "key", keyValuePairs[i], "value", keyValuePairs[i + 1]);
            }
        }

        private void storeLastVersion(string space, string version)
        {
            if (_lastVersions.ContainsKey(space))
            {
                if (_lastVersions[space] == version)
                    return;
            }

            _lastVersions[space] = version;
            doSqlUpsert(versionTable, namespaceColumn, space, versionColumn, version);
        }

        private void storeSetting(string space, string key, string value)
        {
            doSqlUpsertForNamespace(settingsTable, space, key, value);
        }

        private void storeBlob(string space, string key, string blob, bool secure)
        {
            // todo secure :(
            doSqlUpsertForNamespace(settingsTable, space, key, blob);
        }

        public IReadOnlyDictionary<string, string> GetSettings(PluginManager.Plugin source)
        {
            var kvps = doSqlQueryDictionary(settingsTable, source.Source);
            if (kvps == null)
                return EmptySettings;
            else
                return new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(kvps);
        }

        public string LastVersion(PluginManager.Plugin source)
        {
            if (_lastVersions.ContainsKey(source.Source))
                return _lastVersions[source.Source];

            string result = doSqlQueryValue(versionTable, source.Source, versionColumn, true);
            if (result != null)
                _lastVersions[source.Source] = result;

            return result;
        }

        public void StoreSetting(PluginManager.Plugin source, string key, string value)
        {
            storeLastVersion(source.Source, source.Version);
            storeSetting(source.Source, key, value);
        }

        public void StoreBlob(PluginManager.Plugin source, string key, string blob, bool secure)
        {
            storeLastVersion(source.Source, source.Version);
            storeBlob(source.Source, key, blob, secure);
        }

        public string RetrieveBlob(PluginManager.Plugin source, string key)
        {
            return doSqlQueryValue(blobTable, source.Source, key, false);
        }
    }
}
