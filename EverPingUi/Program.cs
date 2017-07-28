using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace EverPingUi
{

    static class Program
    {
        private static string ExceptionLogPath = "exception.log";
        private static Form1 MainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Catch all unhandled program exceptions
            Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);

            MainForm = new Form1();
            Application.Run(MainForm);
        }

        /// <summary>
        /// Event handler for thread exceptions
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">A ThreadExceptionEventArgs that contains the event data</param>
        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string msg = null;
            try
            {
                if (!File.Exists(ExceptionLogPath))
                {
                    File.Create(ExceptionLogPath);
                    Thread.Sleep(100);
                }
                File.AppendAllText(ExceptionLogPath, string.Format(
                    "[{0}] {1}{2}{3}{2}--------------------------------------------------------------------------------{2}",
                    DateTime.Now.ToString("MM-dd-yyyyTHH:mm:ss"), e.Exception.Message, Environment.NewLine, e.Exception.ToString()));
                string absoluteLogPath = Path.GetFullPath(ExceptionLogPath);
                msg = string.Format("The following exception occurred and has been logged to {0}:", absoluteLogPath);
            }
            catch
            {
                msg = "The following exception occurred and could not be logged:";
            }

            MessageBox.Show(string.Format("{0}{1}{2}", msg, Environment.NewLine, e.Exception.ToString()), "Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            MainForm.Recover();
        }

        /// <summary>
        /// Event handler for unhandled exceptions
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">An UnhandledExceptionEventArgs that contains the event data</param>
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            OnThreadException(sender, new ThreadExceptionEventArgs((Exception)e.ExceptionObject));
        }
    }

}
