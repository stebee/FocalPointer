using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.IO;

namespace SiobhanDev
{ 
    public static class AssemblyAttributes
    {
        private static Assembly _assembly;
        static AssemblyAttributes()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _cache = new Dictionary<string, string>();
        }

        #region Attribute cache
        // The reflection operations are potentially expensive(ish),
        //  so we cache their results. No need for cache invalidation
        //  because these are compile-time constants.
        // [CallerMemberName] is a "magic" attribute that, at compile
        //  time, automatically replaces the default value of the
        //  argument with the name of the calling method.
        private static Dictionary<string, string> _cache;
        private static bool isCached([CallerMemberName] string key = null)
        {
            return _cache.ContainsKey(key);
        }

        private static string getFromCache([CallerMemberName] string key = null)
        {
            if (isCached(key))
            {
                return _cache[key];
            }
            else
            {
                return null;
            }
        }

        private static void storeInCache(string value, [CallerMemberName] string key = null)
        {
            _cache[key] = value;
        }
        #endregion

        public static string Title
        {
            get
            {
                if (!isCached())
                {
                    object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                    if (attributes.Length > 0)
                    {
                        AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                        if (titleAttribute.Title != "")
                        {
                            return titleAttribute.Title;
                        }
                    }

                    storeInCache(System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase));
                }

                return getFromCache();
            }
        }

        public static string Version
        {
            get
            {
                if (!isCached())
                    storeInCache(_assembly.GetName().Version.ToString());

                return getFromCache();
            }
        }

        public static string Description
        {
            get
            {
                if (!isCached())
                {
                    object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                    if (attributes.Length == 0)
                        storeInCache("");
                    else
                        storeInCache(((AssemblyDescriptionAttribute)attributes[0]).Description);
                }

                return getFromCache();
            }
        }

        public static string Product
        {
            get
            {
                if (!isCached())
                {
                    object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                    if (attributes.Length == 0)
                        storeInCache("");
                    else
                        storeInCache(((AssemblyProductAttribute)attributes[0]).Product);
                }

                return getFromCache();
            }
        }
        
        public static string Copyright
        {
            get
            {
                if (!isCached())
                {
                    object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                    if (attributes.Length == 0)
                        storeInCache("");
                    else
                        storeInCache(((AssemblyCopyrightAttribute)attributes[0]).Copyright);
                }

                return getFromCache();
            }
        }
        
        public static string Company
        {
            get
            {
                if (!isCached())
                {
                    object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                    if (attributes.Length == 0)
                        storeInCache("");
                    else
                        storeInCache(((AssemblyCompanyAttribute)attributes[0]).Company);
                }

                return getFromCache();
            }
        }

        public static string EmbeddedFile(string filename)
        {
            using (var stream = _assembly.GetManifestResourceStream(_assembly.GetName().Name + ".Properties." + filename))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}