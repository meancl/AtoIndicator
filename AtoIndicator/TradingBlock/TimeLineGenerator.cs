using static  AtoIndicator.KiwoomLib.TimeLib;
using static AtoIndicator.MainForm;

namespace AtoIndicator.TradingBlock
{
    internal static class TimeLineGenerator
    {
        public static void GenerateFrontLine(ref TimeLineManager lineManager, int nBirthTime, int nYesterdayPrice, int nBirthPrice, int nIter = BRUSH)
        {
            try
            {
                int nTimeDegree = lineManager.nTimeDegree;
                if (lineManager.arrTimeLine == null)
                    lineManager.arrTimeLine = new TimeLine[BRUSH + SubTimeToTimeAndSec(MARKET_END_TIME, nBirthTime) / nTimeDegree];

                for (int i = 0; i < nIter; i++) // 원래 안해도 되는데 사고나서 아예 데이터가 없을경우 확인이 안되기 때문에 미리 해놓는것
                {
                    lineManager.nRealDataIdx = lineManager.nPrevTimeLineIdx; // 지금은 nRealDataIdx인것
                    lineManager.nPrevTimeLineIdx++; // 다음 페이즈로 넘어간다는 느낌
                    lineManager.arrTimeLine[i].nTimeIdx = lineManager.nRealDataIdx; // 배열원소에 현재 타임라인 인덱스 삽입

                    lineManager.arrTimeLine[i].nTime = AddTimeBySec(nBirthTime, (lineManager.nRealDataIdx - BRUSH) * nTimeDegree); // PADDING의 경우 장시작시간보다 아래로 설정
                    lineManager.arrTimeLine[i].nStartFs = nBirthPrice;
                    lineManager.arrTimeLine[i].nLastFs = nBirthPrice;
                    lineManager.arrTimeLine[i].nMaxFs = nBirthPrice;
                    lineManager.arrTimeLine[i].nMinFs = nBirthPrice;
                    lineManager.arrTimeLine[i].nUpFs = nBirthPrice;
                    lineManager.arrTimeLine[i].nDownFs = nBirthPrice;
                    lineManager.arrTimeLine[i].fOverMa0 = nBirthPrice;
                    lineManager.arrTimeLine[i].fOverMa1 = nBirthPrice;
                    lineManager.arrTimeLine[i].fOverMa2 = nBirthPrice;
                    lineManager.arrTimeLine[i].fOverMaGap0 = nYesterdayPrice;
                    lineManager.arrTimeLine[i].fOverMaGap1 = nYesterdayPrice;
                    lineManager.arrTimeLine[i].fOverMaGap2 = nYesterdayPrice;
                }
            }
            catch
            {

            }
        }
    }
}
