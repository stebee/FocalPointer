using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        TrayIconWnd _tray;

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

            _tray = new TrayIconWnd(_trayIcon);
            _clock = new IntervalManager();

            _plugins = new PluginManager();
            if (!_plugins.Initialize(_clock, _tray))
                Application.Exit();
            else
                _initializationComplete = true; 
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
