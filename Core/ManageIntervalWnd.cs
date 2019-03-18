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
        private Timer _timer;
        private ICoreApi _api;

        private List<string> _tasksSeen = new List<string>();
        private HashSet<string> _idsSeen = new HashSet<string>();

        private readonly Interval _knownState = new Interval();
        bool _heartbeat = false;

        public ManageIntervalWnd()
        {
            InitializeComponent();
            Show(false);

            this.LostFocus += ManageIntervalWnd_LostFocus;
            this.Deactivate += ManageIntervalWnd_Deactivate;

            _timer = new Timer();
            _timer.Interval = 250;
            _timer.Tick += timer_Tick;
            _timer.Start();

            refreshButtonsForState();
            refreshIconForState();
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
                _hidePending = false;
            }
            else if (show)
            {
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
            // TODO implement this system later
        }

        public bool OnInitialize(ICoreApi api, string lastVersion, IReadOnlyDictionary<string, string> settings)
        {
            _api = api;
            return true;
        }

        public void OnClockTick(string id, PluginBase.StateChangeType lastStateChange, DateTime timestamp, TimeSpan elapsed)
        {
            _heartbeat = !_heartbeat;
            refreshIconForState();
        }

        private void noteTaskName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return;

            if (_tasksSeen.Contains(name))
            {
                _tasksSeen.Remove(name);
            }
            _tasksSeen.Insert(0, name);

            _taskCombo.BeginUpdate();
            _taskCombo.Items.Clear();
            _taskCombo.Items.AddRange(_tasksSeen.ToArray());
            _taskCombo.EndUpdate();
            _taskCombo.SelectedIndex = 0;
        }

        public void OnIntervalPropertyChange(string id, PluginBase.IntervalType mode, string intervalName = null, string taskName = null)
        {
            if (_knownState.Id != id)
            {
                if (_idsSeen.Contains(id))
                {
                    // This is a modification to an old interval. Ignore it.
                    return;
                }
                else
                {
                    // This is a new interval. Capture it.
                    _idsSeen.Add(id);
                    _knownState.Id = id;
                    _knownState.Reset();
                }
            }

            _knownState.Type = mode;

            if (mode == PluginBase.IntervalType.Focus)
            {
                if (!String.IsNullOrEmpty(intervalName))
                {
                    _nameText.Text = intervalName;
                    _knownState.Name = intervalName;
                }

                if (!String.IsNullOrEmpty(taskName))
                {
                    noteTaskName(taskName);
                    _knownState.Task = taskName;
                }
            }
        }

        public void OnIntervalStateChange(string id, PluginBase.StateChangeType change, DateTime timestamp, TimeSpan elapsed)
        {
            if (_knownState.Id != id)
            {
                if (_idsSeen.Contains(id))
                {
                    // This is a modification to an old interval. Ignore it.
                    return;
                }
                else
                {
                    // This is a new interval. Capture it.
                    _idsSeen.Add(id);
                    _knownState.Id = id;
                    _knownState.Reset();

                    if (change == PluginBase.StateChangeType.Start)
                        _knownState.Begun = timestamp;
                }
            }

            if (change == PluginBase.StateChangeType.Unpause)
                _knownState.State = PluginBase.StateChangeType.Start;
            else
                _knownState.State = change;

            _knownState.Elapsed = elapsed;

            refreshButtonsForState();
            refreshIconForState();
        }

        public string[] OnPopulateTasks(string mostRecent)
        {
            //TODO
            return null;
        }

        public string OnSettingEdited(string key, string value, IReadOnlyDictionary<string, string> context)
        {
            // TODO
            return null;
        }

        public void OnSettingsAccepted(IReadOnlyDictionary<string, string> settings)
        {
            // TODO
        }

        public void OnTaskSelected(string taskName)
        {
            // TODO
        }

        private bool _hidePending = false;
        private void timer_Tick(object sender, EventArgs e)
        {
            if (_hidePending)
            {
                _hidePending = false;
                Show(false);
            }
        }

        private void ManageIntervalWnd_LostFocus(object sender, EventArgs e)
        {
            _hidePending = true;
        }

        private void ManageIntervalWnd_Deactivate(object sender, EventArgs e)
        {
            _hidePending = true;
        }

        private void refreshButtonsForState()
        {
            switch (_knownState.State)
            {
                case PluginBase.StateChangeType.Start:
                    // Interval has started
                    _leftButton.Image = Properties.Resources.update;
                    _rightButton.Image = Properties.Resources.cancel;
                    break;

                case PluginBase.StateChangeType.Pause:
                    // Interval has started
                    _leftButton.Image = Properties.Resources.update;
                    _rightButton.Image = Properties.Resources.cancel;
                    break;

                case PluginBase.StateChangeType.End:
                default:
                    // Sitting idle
                    _leftButton.Image = Properties.Resources.update;
                    _rightButton.Image = Properties.Resources.start;
                    break;
            }
        }

        private void refreshIconForState()
        {
            _stateIcon.Image = IconBank.GetBitmapForState(_stateIcon.Size.Width, _knownState.Type, _knownState.State, _heartbeat);
        }

        private void leftButton_Click(object sender, EventArgs e)
        {
            string toUpdate = null;
            switch (_knownState.State)
            {
                case PluginBase.StateChangeType.Start:
                case PluginBase.StateChangeType.Pause:
                    // In the middle of an interval. Is it focusing?
                    if (_knownState.Type == PluginBase.IntervalType.Focus)
                        toUpdate = _knownState.Id;
                    break;

                case PluginBase.StateChangeType.End:
                default:
                    break;
            }

            if (_nameText.Text != _knownState.Name)
            {
                _api.RequestSetIntervalName(_nameText.Text, toUpdate);
            }

            if (_taskCombo.Text != _knownState.Task)
            {
                _api.RequestSetTaskName(_taskCombo.Text, toUpdate);
            }
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            switch (_knownState.State)
            {
                case PluginBase.StateChangeType.Start:
                case PluginBase.StateChangeType.Pause:
                    if (!String.IsNullOrEmpty(_knownState.Id))
                        _api.RequestModifyIntervalState(_knownState.Id, PluginBase.StateChangeType.End);
                    break;

                case PluginBase.StateChangeType.End:
                    _api.RequestSetIntervalName(_nameText.Text);
                    _api.RequestSetTaskName(_taskCombo.Text);
                    _api.RequestStartInterval(PluginBase.IntervalType.Focus);
                    break;

                default:
                    break;
            }
        }

        private void stateIcon_Click(object sender, EventArgs e)
        {
            switch (_knownState.State)
            {
                case PluginBase.StateChangeType.Start:
                    if (!String.IsNullOrEmpty(_knownState.Id))
                        _api.RequestModifyIntervalState(_knownState.Id, PluginBase.StateChangeType.Pause);
                    break;

                case PluginBase.StateChangeType.Pause:
                    if (!String.IsNullOrEmpty(_knownState.Id))
                        _api.RequestModifyIntervalState(_knownState.Id, PluginBase.StateChangeType.Unpause);
                    break;

                case PluginBase.StateChangeType.End:
                    _api.RequestSetIntervalName(_nameText.Text);
                    _api.RequestSetTaskName(_taskCombo.Text);
                    _api.RequestStartInterval(PluginBase.IntervalType.Focus);
                    break;

                default:
                    break;
            }
        }
    }
}
