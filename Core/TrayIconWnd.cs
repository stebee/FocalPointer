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
    class TrayIconWnd : PluginBase
    {
        private NotifyIcon _icon;
        private MenuItem _stateMenuItem;
        private IntervalType _currentIntervalType;
        private string _currentIntervalId;
        private StateChangeType _lastStateChange;
        private TimeSpan _currentIntervalElapsedTime;

        private ICoreApi _api;

        public TrayIconWnd(NotifyIcon icon)
        {
            _icon = icon;

            _stateMenuItem = new MenuItem("[error]", new EventHandler(handleStateMenu));
            MenuItem configMenuItem = new MenuItem("Configuration", new EventHandler(handleConfigMenu));
            MenuItem helpMenuItem = new MenuItem("Help", new EventHandler(handleHelpMenu));
            MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(handleExitMenu));

            _icon.ContextMenu = new ContextMenu(new MenuItem[]
            {
                _stateMenuItem,
                new MenuItem("-"),
                helpMenuItem,
                configMenuItem,
                exitMenuItem
            });

            _icon.BalloonTipClicked += icon_OnLeftClick;
            _icon.MouseClick += icon_OnLeftClick;
            _icon.BalloonTipClosed += icon_OnLeftClick;

            RefreshForState(null);
        }

        public void Show(bool show)
        {
            _icon.Visible = show;
        }

        public void RefreshForState(IntervalManager clock = null)
        {
            if (clock == null)
            {
                _icon.Icon = Properties.Resources.idle;
                return;
            }
        }

        private void icon_OnLeftClick(object sender, EventArgs e)
        {
        }

        private void handleConfigMenu(object sender, EventArgs e)
        {
        }

        private void handleStateMenu(object sender, EventArgs e)
        {
        }

        private void handleHelpMenu(object sender, EventArgs e)
        {
        }

        private void handleExitMenu(object sender, EventArgs e)
        {
            // TODO End interval cleanly before exiting
            Application.Exit();
        }

        public override IRegistration GetRegistration()
        {
            var registration = new SimpleRegistration();
            registration.Title = "System Tray Icon";
            registration.Version = "1";
            return registration;
        }

        public override bool OnInitialize(ICoreApi api, string lastVersion, IReadOnlyDictionary<string, string> settings)
        {
            _api = api;
            return true;
        }
    }
}
