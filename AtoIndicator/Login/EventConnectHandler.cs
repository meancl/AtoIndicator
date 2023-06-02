using Microsoft.EntityFrameworkCore;
using AtoIndicator.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtoIndicator
{
    public partial class MainForm
    {


        public bool isLoginSucced;
        public string sAccountNum; // 계좌번호 // eventconnect


        // ============================================
        // 로그인 이벤트발생시 핸들러 메소드
        // ============================================
        private void OnEventConnectHandler(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {

            if (e.nErrCode == 0) // 로그인 성공
            {
                PrintLog("로그인 성공");
                string sMyName = axKHOpenAPI1.GetLoginInfo("USER_NAME").Trim();
                string sAccList = axKHOpenAPI1.GetLoginInfo("ACCLIST").Trim(); // 로그인 사용자 계좌번호 리스트 요청


                using (var db = new myDbContext())
                {
                    db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    var data = db.locationUserDict.FirstOrDefault(x => x.sUserName.Equals(sMyName));
                    if (data != null) // 현재는 등록된 사원만 함수를 사용할 수 있게
                    {
                        COMPUTER_LOCATION = data.nUserLocationComp; 
                    }
                }
                
                if (axKHOpenAPI1.GetLoginInfo("GetServerGubun").Trim().Equals("1")) // 모의투자
                    marketGubunLabel.Text = "모의투자";
                else // 실거래
                    marketGubunLabel.Text = "실거래";


                string[] accountArray = sAccList.Split(';');

                sAccountNum = accountArray[0]; // 처음계좌가 main계좌
                accountComboBox.Text = sAccountNum;
                this.ActiveControl = logTxtBx;
                RequestHoldings(0);
                SubscribeRealData(); // 실시간 구독 
                RequestDeposit(); // 예수금상세현황요청 


                foreach (string sAccount in accountArray)
                {
                    if (sAccount.Length > 0)
                        accountComboBox.Items.Add(sAccount);
                }
                myNameLabel.Text = sMyName;
                isLoginSucced = true;
            }
            else
            {
                MessageBox.Show("로그인 실패");
            }
        } // END ---- 로그인 이벤트 핸들러


    }
}
