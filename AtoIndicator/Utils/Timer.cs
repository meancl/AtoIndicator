using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtoIndicator.Utils
{
    internal static class Timer
    {
        public static void Delay(int ms)
        {
            DateTime dateTimeNow = DateTime.UtcNow;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime dateTimeAdd = dateTimeNow.Add(duration);
            while (dateTimeAdd >= dateTimeNow)
            {
                System.Windows.Forms.Application.DoEvents();
                dateTimeNow = DateTime.UtcNow;
            }
            return;
        }
    }
}
