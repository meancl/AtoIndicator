using static AtoIndicator.Utils.Comparer;

namespace AtoIndicator.Utils
{
    internal static class Protractor
    {

        /// <summary>
        /// 기울기 변수 fN과 fM 간 사이각도를 ArcTangent를 통해 받아온다.
        /// 기울기는 2차원 좌표 내의 직선 기울기를 기준으로 함
        /// fN기울기가 fM기울기로 이동하는데 몇도 이동이 필요한 지 확인하기 위한 함수로써
        /// 값이 양수일경우 각도가 반시계방향만큼 이동이 필요하게 차이가 나고
        /// 값이 음수일경우 각도가 시계방향만큼 이동이 필요하게 차이가 난다.
        /// 수식 : ArcTangent((fM - fN) / ( fN + fM + 1 )) * ( 180 / PI )
        /// 기울기가 음수냐 양수냐에 따라 ArcTangent값이 달라져 처리가 필요했다.
        /// </summary>
        /// <param name="fN"></param>
        /// <param name="fM"></param>
        /// <returns></returns>
        internal static double GetAngleBetween(double fN, double fM)
        {
            double fAngleDirection;

            if (isEqualBetweenDouble(fN, fM)) // same
            {
                fAngleDirection = 0;
            }
            else if (isEqualBetweenDouble(fN * fM, -1)) // -1
            {
                if (fN > 0)
                {
                    fAngleDirection = -90;
                }
                else
                {
                    fAngleDirection = 90;
                }
            }
            else
            {
                // 각도가 반시계 방향이면 + 
                //        시계방향이면 -
                if (fN < fM) // 분자는 양수
                {

                    if (fN * fM < 0) // 둘 중 하나가 음수면
                    {
                        if (fN * fM < -1)
                        {
                            fAngleDirection = 180 + System.Math.Atan((fM - fN) / (1 + fN * fM)) * (180 / System.Math.PI);
                        }
                        else
                        {
                            fAngleDirection = System.Math.Atan((fM - fN) / (1 + fN * fM)) * (180 / System.Math.PI);
                        }


                    }
                    else // 둘 다 음수거나 양수면
                    {
                        fAngleDirection = System.Math.Atan((fM - fN) / (1 + fN * fM)) * (180 / System.Math.PI);
                    }

                }
                else // 분자가 음수
                {
                    if (fN * fM < 0) // 둘 중 하나가 음수면
                    {
                        if (fN * fM < -1) // 둘의 곱이 -1 을 넘으면
                        {
                            fAngleDirection = -180 + System.Math.Atan((fM - fN) / (1 + fN * fM)) * (180 / System.Math.PI);
                        }
                        else // 둘의 곱이 -1 ~ 0 사이면
                        {
                            fAngleDirection = System.Math.Atan((fM - fN) / (1 + fN * fM)) * (180 / System.Math.PI);
                        }

                    }
                    else // 둘 다 음수거나 양수면
                    {
                        fAngleDirection = System.Math.Atan((fM - fN) / (1 + fN * fM)) * (180 / System.Math.PI);
                    }

                }

            }
            return fAngleDirection;
        }

    }
}
