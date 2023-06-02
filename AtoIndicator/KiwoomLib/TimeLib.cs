namespace AtoIndicator.KiwoomLib
{
    internal static class TimeLib
    {
        
        /// <summary>
        /// kiwoom time형 데이터 간 시간차이를 kiwoom time형으로 반환해줌
        /// retTime = KiwoomTime(timeToBeSub - timeToSub)
        /// </summary>
        /// <param name="timeToBeSub"></param>
        /// <param name="timeToSub"></param>
        /// <returns></returns>
        public static int SubTimeToTime(int timeToBeSub, int timeToSub)
        {
            if (timeToBeSub <= timeToSub)
                return 0;

            int secToBeSub = (int)(timeToBeSub / 10000) * 3600 + (int)(timeToBeSub / 100) % 100 * 60 + timeToBeSub % 100;
            int secToSub = (int)(timeToSub / 10000) * 3600 + (int)(timeToSub / 100) % 100 * 60 + timeToSub % 100;
            int diffTime = secToBeSub - secToSub;
            int hour = diffTime / 3600;
            int minute = (diffTime % 3600) / 60;
            int second = diffTime % 60;

            return hour * 10000 + minute * 100 + second;
        }
        /// <summary>
        /// kiwoom time형 데이터 간 시간차이를 int형 sec로 반환해줌
        /// retSec = Seconds(timeToBeSub - timeToSub)
        /// </summary>
        /// <param name="timeToBeSub"></param>
        /// <param name="timeToSub"></param>
        /// <returns></returns>
        public static int SubTimeToTimeAndSec(int timeToBeSub, int timeToSub)
        {
            if (timeToBeSub <= timeToSub)
                return 0;

            int secToBeSub = (int)(timeToBeSub / 10000) * 3600 + (int)(timeToBeSub / 100) % 100 * 60 + timeToBeSub % 100;
            int secToSub = (int)(timeToSub / 10000) * 3600 + (int)(timeToSub / 100) % 100 * 60 + timeToSub % 100;

            return secToBeSub - secToSub;
        }

        /// <summary>
        /// kiwoom time형 데이터를 int형  sec만큼 감소하여 반환해줌
        /// retTime = KiwoomTime(Seconds(timeToBeSub) - subSec)
        /// </summary>
        /// <param name="timeToBeSub"></param>
        /// <param name="subSec"></param>
        /// <returns></returns>
        public static int SubTimeBySec(int timeToBeSub, int subSec)
        {
            int secToBeSub = (int)(timeToBeSub / 10000) * 3600 + (int)(timeToBeSub / 100) % 100 * 60 + timeToBeSub % 100;
            if (subSec <= 0)
                return timeToBeSub;

            if (secToBeSub <= subSec)
                return 0;
            secToBeSub -= subSec;
            int hour = secToBeSub / 3600;
            int minute = (secToBeSub % 3600) / 60;
            int second = secToBeSub % 60;

            return hour * 10000 + minute * 100 + second;
        }

        /// <summary>
        /// kiwoom time형 데이터를 int형 sec만큼 증가시켜 반환해줌
        /// retTime = KiwoomTime(Seconds(timeToBeAdd) + addSec)
        /// </summary>
        /// <param name="timeToBeAdd"></param>
        /// <param name="addSec"></param>
        /// <returns></returns>
        public static int AddTimeBySec(int timeToBeAdd, int addSec)
        {
            int secToBeAdd = (int)(timeToBeAdd / 10000) * 3600 + (int)(timeToBeAdd / 100) % 100 * 60 + timeToBeAdd % 100;
            secToBeAdd += addSec;
            int hour = secToBeAdd / 3600;
            int minute = (secToBeAdd % 3600) / 60;
            int second = secToBeAdd % 60;

            return hour * 10000 + minute * 100 + second;
        }

        /// <summary>
        /// kiwoom time형 데이터 간 시간을 합해 int형 sec로 반환해줌
        /// retSec = Seconds(timeToBeAdd + timeToAdd)
        /// </summary>
        /// <param name="timeToBeAdd"></param>
        /// <param name="timeToAdd"></param>
        /// <returns></returns>
        public static int AddTimeToTimeAndSec(int timeToBeAdd, int timeToAdd)
        {
            int secToBeAdd = (int)(timeToBeAdd / 10000) * 3600 + (int)(timeToBeAdd / 100) % 100 * 60 + timeToBeAdd % 100;
            int secToAdd = (int)(timeToAdd / 10000) * 3600 + (int)(timeToAdd / 100) % 100 * 60 + timeToAdd % 100;
            secToBeAdd += secToAdd;

            return secToBeAdd;
        }

        /// <summary>
        /// int형 sec을 kiwoom time형으로 변환하여 반환해줌
        /// retTime = KiwoomTime(timeSec)
        /// </summary>
        /// <param name="timeSec"></param>
        /// <returns></returns>
        public static int GetKiwoomTime(int timeSec)
        {
            return (int)(timeSec / 3600) * 10000 + (int)(timeSec % 3600 / 60) * 100 + timeSec % 60;
        }

        /// <summary>
        /// kiwoom time형 데이터를 int형 sec로 변환하여 반환해줌
        /// retSec = Seconds(kiwoomTime)
        /// </summary>
        /// <param name="kiwoomTime"></param>
        /// <returns></returns>
        public static int GetSec(int kiwoomTime)
        {
            return (int)(kiwoomTime / 10000) * 3600 + (int)(kiwoomTime / 100) % 100 * 60 + kiwoomTime % 100;
        }

    }
}
