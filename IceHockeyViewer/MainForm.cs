using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using NataInfo.Nibus;
using NataInfo.Nibus.Nms;
using NataInfo.Nibus.Sport;

namespace IceHockeyViewer
{
    public partial class MainForm : Form
    {
        private NibusStack _stack;
        private IceHockeyProtocol _iceHockeyProtocol;

        public MainForm()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            InitNibus();
        }

        private void OnClosed(object sender, FormClosedEventArgs e)
        {
            using (_stack)
            {
                _stack = null;
            }
        }

        /// <summary>
        /// Создаем стек NiBUS и подписываемся на события.
        /// </summary>
        /// TODO: Имя COM-порта зашито в код, измените на свой.
        private void InitNibus()
        {
            try
            {
                _stack = NibusStack.CreateSerialNmsStack("COM14");
                var nmsCodec = _stack.GetCodec<NmsCodec>();
                var nmsProtocol = nmsCodec.Protocol;
                _iceHockeyProtocol = new IceHockeyProtocol(nmsProtocol);
                _iceHockeyProtocol.HomeScoreChanged += OnHomeScoreChanged;
                _iceHockeyProtocol.VisitorScoreChanged += OnVisitorScoreChanged;
                _iceHockeyProtocol.PenaltyStatChanged += OnPenaltyStatChanged;
                _iceHockeyProtocol.TimerChanged += OnTimerChanged;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка создания стека NiBUS.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buttonLoad.Enabled = false;
                buttonSynchronize.Enabled = false;
            }
        }

        private void OnHomeScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            labelHomeScore.Text = e.Score.ToString(CultureInfo.InvariantCulture);
        }

        private void OnVisitorScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            labelVisitorScore.Text = e.Score.ToString(CultureInfo.InvariantCulture);
        }

        private void OnTimerChanged(object sender, TimerChangedEventArgs e)
        {
            var timer = e.Timer;
            if (timer.IsHidden) return;
            var label = timer.IsSecondary ? labelSecondaryTimer : labelPrimaryTimer;
            var timerAttributes = _iceHockeyProtocol.Provider.Timers.SingleOrDefault(ta => ta.Id == timer.TimerId);
            label.Text = timer.ToString(timerAttributes);
        }

        private void OnPenaltyStatChanged(object sender, PenaltyStatChangedEventArgs e)
        {
            listBoxHomePenalty.Items.Clear();
            listBoxVisitorPenalty.Items.Clear();
            listBoxHomePenalty.Items.AddRange(
                e.PenaltyStat.HomePenalties.Where(pi => pi.IsActive).Cast<object>().ToArray());
            listBoxVisitorPenalty.Items.AddRange(
                e.PenaltyStat.VisitorPenalties.Where(pi => pi.IsActive).Cast<object>().ToArray());
        }

        private void OnLoadProviderClick(object sender, EventArgs e)
        {
            _iceHockeyProtocol.LoadProvider();
        }

        private void OnSynchronizeClick(object sender, EventArgs e)
        {
            _iceHockeyProtocol.RequestGameInfo();
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}