using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtoIndicator
{
    public partial class MainForm
    {
        public bool isPrintLogBox = false;
        public StringBuilder sbLogTxtBx = new StringBuilder();

        public void PrintLog(string sMessage, int? nEaIdx = null, int? nSlotIdx = null, bool isTxtBx = true)
        {
            try
            {
                string sMsg = $"{sMessage}{NEW_LINE}";

                if (isTxtBx)
                {
                    if (isPrintLogBox)
                    {
                        if (logTxtBx.InvokeRequired)
                            logTxtBx.Invoke(new MethodInvoker(delegate { logTxtBx.AppendText(sMsg); }));
                        else
                            logTxtBx.AppendText(sMsg);
                    }

                    sbLogTxtBx.Append(sMsg);
                }

                if (nEaIdx != null)
                {
                    ea[(int)nEaIdx].myTradeManager.sTotalLog.Append(sMsg);
                    if (nSlotIdx != null)
                        ea[(int)nEaIdx].myTradeManager.arrBuyedSlots[(int)nSlotIdx].sEachLog.Append(sMsg);
                }
            }
            catch
            {

            }
        }

        public void StoreLog()
        {
            string logMsgFilePath = "fullMsgLog.txt";
            using (StreamWriter writer = new StreamWriter(logMsgFilePath, false))
            {
                // Write the log message to the file
                writer.WriteLine($"{sbLogTxtBx.ToString()}");
            }
        }
    }
}
