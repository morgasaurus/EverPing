using Pinger;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EverPingUi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetSettings(AppMethods.ReadConfig());
            SetButtons(false);
        }
        
        private int CodeOrange = 500;
        private int CodeRed = 1000;

        private string Host;
        private int Timeout;
        private int Bytes;

        private PingWorker PingWorker;
        private PingReport PingReport;

        private string CurrentReport;
        private DateTimeOffset Timestamp;
        
        public void Recover()
        {
            if (PingWorker != null)
            {
                Task task = Task.Run(() => { PingWorker.Stop(); });
                task.Wait();
            }
            PingWorker = null;
            PingReport = null;
            CurrentReport = null;
            SetButtons(false);
        }
        
        private void SetSettings(Settings settings)
        {
            Host = settings.Host;
            TextBox_Host.Text = settings.Host;

            Timeout = settings.Timeout;
            TextBox_Timeout.Text = settings.Timeout.ToString();

            Bytes = settings.Bytes;
            TextBox_Bytes.Text = settings.Bytes.ToString();            
        }

        private void SetButtons(bool started)
        {
            Button_Start.Enabled = !started;
            Button_Stop.Enabled = started;
            Button_Save.Enabled = !string.IsNullOrEmpty(CurrentReport);
            Button_Defaults.Enabled = !started;

            TextBox_Host.ReadOnly = started;
            TextBox_Timeout.ReadOnly = started;
            TextBox_Bytes.ReadOnly = started;
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            SetButtons(true);
            ValidateInputAndSetFields();
            AppMethods.SaveConfig(new Settings() { Host = Host, Timeout = Timeout, Bytes = Bytes });
            PingWorker = new PingWorker(Host, Timeout, Bytes);
            PingWorker.PingComplete += OnPingComplete;
            PingWorker.Start();
        }

        private async void Button_Stop_Click(object sender, EventArgs e)
        {
            await Task.Run(() => { EndWorker(); }); // Must run in separate thread to prevent UI thread deadlock
            ReportOnResults();
            PingWorker = null;
            PingReport = null;
            SetButtons(false);
        }
        
        private void Button_Defaults_Click(object sender, EventArgs e)
        {
            SetSettings(AppMethods.DefaultSettings);
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog_SaveResult.FileName = string.Format("PingResult_{0}.txt", Timestamp.ToString("yyyy-MM-dd_HHmm"));

            if (string.IsNullOrEmpty(CurrentReport))
            {
                ShowError("I don't know how you did it, but you managed to click 'Save' when there is nothing to save.  Good job!");
            }

            DialogResult result = SaveFileDialog_SaveResult.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            string file = SaveFileDialog_SaveResult.FileName;
            try
            {
                File.WriteAllText(file, CurrentReport);
            }
            catch (Exception ex)
            {
                ShowError(string.Format("The report could not be saved.{0}{0}{1}",
                    Environment.NewLine,
                    ex.Message));
            }
        }

        private void OnTextBoxFocusEnter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void EndWorker()
        {
            if (PingWorker != null)
            {
                PingWorker.Stop();
                PingReport = PingWorker.GetReport();
            }
        }
        
        private void ValidateInputAndSetFields()
        {
            Host = TextBox_Host.Text;
            if (string.IsNullOrEmpty(Host))
            {
                Host = AppMethods.DefaultSettings.Host;
                TextBox_Host.Text = Host;
            }

            string timeoutString = TextBox_Timeout.Text;
            int n;
            if (!int.TryParse(timeoutString, out n) || n < 500)
            {
                Timeout = AppMethods.DefaultSettings.Timeout;
                TextBox_Timeout.Text = Timeout.ToString();
            }
            else
            {
                Timeout = n;
            }

            string bytesString = TextBox_Bytes.Text;
            if (!int.TryParse(bytesString, out n) || n < 0)
            {
                Bytes = AppMethods.DefaultSettings.Bytes;
                TextBox_Bytes.Text = Bytes.ToString();
            }
            else
            {
                Bytes = n;
            }
        }

        private void OnPingComplete(object sender, PingCompleteEventArgs e)
        {
            string text = string.Format("{0}{1}", FormatResult(e.Result), Environment.NewLine);
            RichTextBox_Log.Invoke(new Action(() => {
                RichTextBox_Log.SelectionStart = RichTextBox_Log.TextLength;
                RichTextBox_Log.SelectionLength = 0;

                Color color = RichTextBox_Log.ForeColor;
                if (!string.IsNullOrEmpty(e.Result.ErrorMessage) || e.Result.Milliseconds >= CodeRed)
                {
                    color = Color.Red;
                }
                else if (e.Result.Milliseconds >= CodeOrange)
                {
                    color = Color.Orange;
                }

                RichTextBox_Log.SelectionColor = color;
                RichTextBox_Log.AppendText(text);
                RichTextBox_Log.SelectionColor = RichTextBox_Log.ForeColor;
                RichTextBox_Log.ScrollToCaret();
            }));
        }

        private string FormatResult(PingResult result)
        {
            if (string.IsNullOrEmpty(result.ErrorMessage))
            {
                return string.Format("Reply from {0} ({1}): Bytes={2} Time={3}ms", result.Host, result.IpAddress, result.Bytes, result.Milliseconds);
            }
            else
            {
                return string.Format("Error from {0}: {1}", result.Host, result.ErrorMessage);
            }
        }
        
        private void ReportOnResults()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Environment.NewLine);

            builder.AppendFormat("Ping statistics for {0} from {1} to {2} ({3} minutes):{4}",
                PingReport.Host,
                PingReport.Start.ToString("h:mm tt"),
                PingReport.End.ToString("h:mm tt"),
                PingReport.Duration.TotalMinutes.ToString("0.0"),
                Environment.NewLine);

            builder.AppendFormat("    Packets: Sent = {0}, Received = {1}, Lost = {2} ({3}% loss){4}",
                PingReport.Sent,
                PingReport.Received,
                PingReport.Lost,
                (PingReport.PacketLoss * 100).ToString("0.00000"),
                Environment.NewLine);

            builder.AppendLine("Approximate round trip times in milliseconds:");

            builder.AppendFormat("    Minimum = {0}ms, Maximum = {1}ms, Average = {2}ms{3}",
                PingReport.Min,
                PingReport.Max,
                PingReport.Average.ToString("0.00"),
                Environment.NewLine);

            builder.AppendLine("Video game health analysis:");

            builder.AppendFormat("    High ping events = {0} ({1} per hour), Potential disconnects = {2} ({3} per hour){4}{4}",
                PingReport.HighPingEvents,
                PingReport.HighPingEventsPerHour.ToString("0.00"),
                PingReport.PotentialDisconnects,
                PingReport.PotentialDisconnectsPerHour.ToString("0.00"),
                Environment.NewLine);

            string text = builder.ToString();
            RichTextBox_Log.AppendText(text);
            CurrentReport = text.Trim();
            Timestamp = PingReport.Start;
        }
    }
}
