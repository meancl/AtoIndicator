using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtoIndicator
{
    static class Program
    {
        // Event handler for the AppDomain.UnhandledException event
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                // Get the exception object from the event arguments
                Exception ex = e.ExceptionObject as Exception;
                DateTime curTime = DateTime.Now;

                // Write the program log to a file using a file stream or a logging library
                // ...

                string logFilePath = "errorLog.txt"; // Set the path to the log file


                // Open the log file in append mode using a StreamWriter
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    // Write the log message to the file
                    writer.WriteLine($"{DateTime.Now.ToString()} ## Msg : {ex.Message}");
                    writer.WriteLine($"======================================================");
                    writer.WriteLine($"{ex.StackTrace}{Environment.NewLine}");
                }
                mf.StoreLog();

                //using (StreamWriter writer = new StreamWriter(logMsgFilePath, true))
                //{

                //    // Write the log message to the file
                //    writer.WriteLine($"{mf.sbLogTxtBx.ToString()}");
                //}


                // Display an error message to the user
                // MessageBox.Show("An unhandled exception has occurred. The program will now terminate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //// Exit the application
                //Environment.Exit(1);
            }
            catch
            {

            }
        }

        public static MainForm mf;
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Subscribe to the AppDomain.UnhandledException event in your application startup code
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mf = new MainForm();
            Application.Run(mf);
        }

        
    }
}
