using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoServer.Shared_Memory
{
    public class MMF
    {
        public const string sMemoryName = "AtoMemory";


        public const int SERVICE_POINTER_LOC = 0; // 서비스쪽 포인터 위치
        public const int USER_POINTER_LOC = 4;    // 우리쪽 포인터 위치

        public const int nStepPtrSize = 8;         // 포인터 이후 OffSet
        public const int nStructStepNum = 8192;    // 실제 데이터 OffSet

        public bool[] checkingRequestArray;   // 데이터 요청 유무
        public bool[] checkingComeArray;      // 서비스 도착 유무

        public double[] checkingRatioArray; // 정답율
        public bool[] checkingBookArray;    // 정답 확정

        public int nStructSize = Marshal.SizeOf<SharedAIBlock>();
        public int nTotalMemorySize;   

        public MemoryMappedFile mappedMemory;

        public int nPtrOffSet;
        public int nQueueOffSet;
        public int nBlockOffSet;

        
        public int nCurOtherPtr;                 // 서비스쪽 위치
        public int nCurMyClientPtr;              // 처리안된 우리 위치
        public int nCurMyProcessedPtr = 0;       // 처리된 우리 위치

        public SharedAIBlock curBlock;

        public MMF()
        {
            nTotalMemorySize = nStepPtrSize + nStructStepNum * sizeof(int) + nStructSize * nStructStepNum;

            nPtrOffSet = 0;
            nQueueOffSet = nPtrOffSet + nStepPtrSize;
            nBlockOffSet = nQueueOffSet + nStructStepNum * sizeof(int);

            checkingRequestArray = new bool[nStructStepNum]; // AI Service 요청상황
            checkingComeArray = new bool[nStructStepNum]; // AI service 도착유무
            checkingRatioArray = new double[nStructStepNum]; // AI service target 값
            checkingBookArray = new bool[nStructStepNum]; // AI service target 값

            OpenSharedMemory();
        }

        // 생성된 공유메모리 접근하기
        public void OpenSharedMemory()
        {
#if AI
            CallEvent();
            while (true)
            {
                try
                {
                    mappedMemory = MemoryMappedFile.OpenExisting(sMemoryName);
                    break;
                }
                catch
                {
                    Thread.Sleep(5000);
                }
            }
            using (var accessor = mappedMemory.CreateViewAccessor())
            {
                accessor.Read(nPtrOffSet + USER_POINTER_LOC, out nCurMyProcessedPtr); // 우리 메모리 초기화하기
                nCurMyClientPtr = nCurMyProcessedPtr;
            }
#endif
        }

        // 처리된 데이터는 가져오기
        public void FetchTargets()
        {
            using (var accessor = mappedMemory.CreateViewAccessor())
            {
                accessor.Read(nPtrOffSet + SERVICE_POINTER_LOC, out nCurOtherPtr); // 서버 메모리 받아오기

                while (CheckIsDataToFetch()) // 추론이 끝난 데이터가 있다
                {
                    accessor.Read(nQueueOffSet + nCurMyProcessedPtr * sizeof(int), out int nPtr); // 처리된 데이터의 인덱스번호 가져오기

                    accessor.Read(nBlockOffSet + nPtr * nStructSize, out curBlock); // 해당 데이터 가져오기

                    checkingRatioArray[nPtr] = curBlock.fRatio;
                    checkingBookArray[nPtr] = curBlock.isTarget; // 해당 데이터 값 집어넣기
                    checkingComeArray[nPtr] = true; // 데이터 가져왔다고 체크하기
                    nCurMyProcessedPtr = (nCurMyProcessedPtr + 1) % nStructStepNum; // 처리된 데이터 한칸 증가시키기
                }
            }
        }

        // 처리됐는지 체크하기
        bool CheckIsDataToFetch()
        {
            return (nCurMyProcessedPtr <= nCurOtherPtr) ? nCurMyProcessedPtr < nCurOtherPtr : nCurOtherPtr != nCurMyProcessedPtr;
        }

        // 데이터 슬롯이 남아있는 지 체크하기
        bool CheckPtrRemained()
        {
            return ((nCurOtherPtr - 1 + nStructStepNum) % nStructStepNum) != nCurMyClientPtr;
        }

        // sCode : 종목명
        // nRqTime : 요청시간
        // nRqType : 요청타입
        // inputData : 요청데이터
        // nSellReqNum : 이벤트 콜 인덱스
        public int RequestAIService(string sCode, int nRqTime, int nRqType, double[] inputData)
        {
            /*
             * 1. 데이터 중에 사용가능한 메모리를 찾는다.
             * 2. 해당 메모리에 데이터를 집어넣는다.
             * 3. 해당 메모리 행 번째 데이터를 기록한다.
             */
            int nCheckToUse = -1;

            using (var accessor = mappedMemory.CreateViewAccessor())
            {
                accessor.Read(nPtrOffSet + SERVICE_POINTER_LOC, out nCurOtherPtr); // 서버메모리 현재 인덱스 가져오기

                for (int i = 0; i < nStructStepNum; i++)
                {
                    if (!checkingRequestArray[i]) // 할당 가능한 AI슬롯 찾기
                    {
                        nCheckToUse = i;  //i번째 사용가능
                        checkingRequestArray[i] = true;
                        break;
                    }
                }

                if (nCheckToUse != -1) // 할당 가능한 AI 슬롯이 있다면
                {
                    if (CheckPtrRemained()) // 한칸은 문제 없기에 냄겨둠.
                    {
                        SharedAIBlock curBlock = new SharedAIBlock // 데이터 만들기
                        {
                            nRequestTime = nRqTime,
                            nFeatureLen = inputData.Length,
                            nRequestType = nRqType,                      
                        };
                        unsafe // 값 삽입하기
                        {
                            for (int i = 0; i < inputData.Length; i++)
                                curBlock.fArr[i] = inputData[i];

                            for (int i = 0; i < sCode.Length; i++)
                            {
                                curBlock.cCodeArr[i] = sCode[i];
                            }
                        }
                        accessor.Write(nBlockOffSet + nCheckToUse * nStructSize, ref curBlock); // 할당한 슬롯 인덱스위치에 데이터 저장하기
                     
                        accessor.Write(nQueueOffSet + nCurMyClientPtr * sizeof(int), ref nCheckToUse); // 할당한 슬롯 위치 인덱스 집어넣기
                        nCurMyClientPtr = (nCurMyClientPtr + 1) % nStructStepNum; // 클라이언트 ptr 한칸 올려놓기
                        accessor.Write(nPtrOffSet + USER_POINTER_LOC, ref nCurMyClientPtr); // 클라이언트  ptr 저장하기

                        CallEvent();
                    }
                    else
                    {
                        nCheckToUse = -1;
                    }
                }
                else // 할당 가능한 AI 슬롯이 없다면
                {
                }
            }

            return nCheckToUse;
        }

        public void CallEvent(string sEventName= "MySharedMemoryEvent")
        {
            EventWaitHandle eventHandle = new EventWaitHandle(false, EventResetMode.AutoReset, sEventName);
            eventHandle.Set();
        }
    }
}
