using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FocalPointer
{
    public partial class ManageIntervalWnd : Form, IPlugin
    {
        private System.Timers.Timer _timer;
        private ICoreApi _api;

        public ManageIntervalWnd()
        {
            InitializeComponent();
            Show(false);

            this.LostFocus += ManageIntervalWnd_LostFocus;
            this.Deactivate += ManageIntervalWnd_Deactivate;

            _timer = new System.Timers.Timer();
            _timer.AutoReset = false;
            _timer.Interval = 500;
            _timer.Elapsed += timer_Elapsed;
        }

        public new void Show()
        {
            this.Show(true);
        }

        public new void Hide()
        {
            this.Show(false);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Show(false);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool _wasShown;
        private delegate void SafeShow(bool show);
        public void Show(bool show)
        {
            if (this.InvokeRequired)
            {
                var func = new SafeShow(Show);
                Invoke(func, new object[] { show });
                return;
            }

            if (show && _wasShown)
            {
                _timer.Enabled = false;
            }
            else if (show)
            {
                // Request task list populate?

                WindowState = FormWindowState.Normal;
                base.Visible = true;
                this.Activate();
                _nameText.Focus();
                BringToFront();
            }
            else
            {
                base.Visible = false;
            }

            _wasShown = show;
        }

        public IRegistration GetRegistration()
        {
            var registration = new SimpleRegistration();
            registration.Title = "Interval Control Panel";
            registration.Version = "1";
            return registration;
        }

        public void OnTasksPopulated(string mostRecent, string[] list)
        {

        }

        public bool OnInitialize(ICoreApi api, string lastVersion, IReadOnlyDictionary<string, string> settings)
        {
            _api = api;
            return true;
        }

        public void OnClockTick(string id, PluginBase.StateChangeType lastStateChange, DateTime timestamp, TimeSpan elapsed)
        {
            throw new NotImplementedException();
        }

        public void OnIntervalCreated(string id, PluginBase.IntervalType mode, string intervalName = null, string taskName = null)
        {
            throw new NotImplementedException();
        }

        public void OnIntervalStateChange(string id, PluginBase.StateChangeType change, DateTime timestamp, TimeSpan elapsed)
        {
            throw new NotImplementedException();
        }

        public string[] OnPopulateTasks(string mostRecent)
        {
            throw new NotImplementedException();
        }

        public string OnSettingEdited(string key, string value, IReadOnlyDictionary<string, string> context)
        {
            throw new NotImplementedException();
        }

        public void OnSettingsAccepted(IReadOnlyDictionary<string, string> settings)
        {
            throw new NotImplementedException();
        }

        public void OnTaskSelected(string taskName)
        {
            throw new NotImplementedException();
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.InvokeRequired)
            Show(false);
        }

        private void ManageIntervalWnd_LostFocus(object sender, EventArgs e)
        {
            _timer.Start();
        }

        private void ManageIntervalWnd_Deactivate(object sender, EventArgs e)
        {
            _timer.Start();
        }
    }
}
