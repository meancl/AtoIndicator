using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtoIndicator.KiwoomLib
{
    internal static class Errors
    {
        public static int OP_ERR_NONE = 0; // 정상처리
        public static int OP_ERR_FAIL = -10; // 실패
        public static int OP_ERR_LOGIN = -100; // 사용자정보교환실패
        public static int OP_ERR_CONNECT = -101; // 서버접속실패
        public static int OP_ERR_VERSION = -102; // 버전처리실패
        public static int OP_ERR_FIREWALL = -103; // 개인방화벽실패
        public static int OP_ERR_MEMORY = -104; // 메모리보호실패
        public static int OP_ERR_INPUT = -105; // 함수입력값오류
        public static int OP_ERR_SOCKET_CLOSED = -106; // 통신연결종료
        public static int OP_ERR_OVERFLOW1 = -200; // 시세조회과부하
        public static int OP_ERR_OVERFLOW2 = -209; // 시세조회과부하
        public static int OP_ERR_OVERFLOW3 = -211; // 시세조회과부하

        public static int OP_ERR_RQ_STRUCT_FAIL = -201; // 전문작성초기화실패
        public static int OP_ERR_RQ_STRING_FAIL = -202; // 전문작성입력값오류
        public static int OP_ERR_NO_DATA = -203; // 데이터없음
        public static int OP_ERR_OVER_MAX_DATA = -204; // 조회가능한종목수초과
        public static int OP_ERR_DATA_RCV_FAIL = -205; // 데이터수신실패
        public static int OP_ERR_OVER_MAX_FID = -206; // 조회가능한FID수초과
        public static int OP_ERR_REAL_CANCEL = -207; // 실시간해제오류
        public static int OP_ERR_ORD_WRONG_INPUT = -300; // 입력값오류
        public static int OP_ERR_ORD_WRONG_ACCTNO = -301; // 계좌비밀번호없음
        public static int OP_ERR_OTHER_ACC_USE = -302; // 타인계좌사용오류
        public static int OP_ERR_MIS_2BILL_EXC = -303; // 주문가격이20억원을초과
        public static int OP_ERR_MIS_5BILL_EXC = -304; // 주문가격이50억원을초과
        public static int OP_ERR_MIS_1PER_EXC = -305; // 주문수량이총발행주수의1%초과오류
        public static int OP_ERR_MIS_3PER_EXC = -306; // 주문수량이총발생주수의3%초과오류
        public static int OP_ERR_SEND_FAIL = -307; // 주문전송실패
        public static int OP_ERR_ORD_OVERFLOW = -308; // 주문전송과부하
        public static int OP_ERR_MIS_300CNT_EXC = -309; // 주문수량300계약초과
        public static int OP_ERR_MIS_500CNT_EXC = -310; // 주문수량500계약초과
        public static int OP_ERR_ORD_WRONG_ACCTINFO = -340; // 계좌정보없음
        public static int OP_ERR_ORD_SYMCODE_EMPTY = -500; // 종목코드없음
    }
}
