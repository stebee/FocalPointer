using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using SiobhanDev;

namespace FocalPointer
{
    public partial class SplashWnd : Form
    {
        bool _initializationComplete = false;
        Timer _timer;
        int _startingTick;

        PluginManager _plugins;
        IntervalManager _clock;
        SettingsManager _settings;
        TrayIconWnd _tray;
        ManageIntervalWnd _controls;

        public SplashWnd()
        {
            _initializationComplete = false;
            _timer = new Timer();
            _timer.Interval = 500;
            _startingTick = Environment.TickCount;
            _timer.Tick += timer_Tick;
            _timer.Enabled = true;

            InitializeComponent();

            _titleLabel.Text = AssemblyAttributes.Title;
            _authorLabel.Text = $"by {AssemblyAttributes.Company}";
            _versionLabel.Text = AssemblyAttributes.Version;
            _copyrightLabel.Text = AssemblyAttributes.Copyright;

            _controls = new ManageIntervalWnd();

            _tray = new TrayIconWnd(_trayIcon, _controls);
            _settings = new SettingsManager();

            _settings.InitializeFromSqlitePath(getStoragePath(), "debug.private");
            _clock = new IntervalManager(_settings);

            _plugins = new PluginManager();
            if (!_plugins.Initialize(_clock, _tray, _controls))
                Application.Exit();
            else
                _initializationComplete = true; 
        }

        private string getStoragePath()
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FocalPointer");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return Path.Combine(dir, "storage.sqlite3");
        }

        private void showSelf(bool show)
        {
            if (show)
            {
                Show();
                _tray.Show(false);
            }
            else
            {
                Hide();
                _tray.Show(true);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int elapsed = Environment.TickCount - _startingTick;

            if (_initializationComplete && elapsed > 1000)
            {
                _timer.Enabled = false;
                showSelf(false);
            }
        }
    }
}
