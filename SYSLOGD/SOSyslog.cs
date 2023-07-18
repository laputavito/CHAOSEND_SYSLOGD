using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Collections;

namespace SYSLOGD
{
	/// <summary>
	/// TMSSyslog에 대한 요약 설명입니다.
	/// </summary>
	public class SOSyslog : System.ComponentModel.Component
	{
		#region 변수, 이벤트, 생성자 처리부분 입니다 ---------------------------------------
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// Syslog 받음을 알리는 이벤트 입니다.
		/// </summary>
		public event SyslogReceivedEventHandler SyslogReceived = null;
		/// <summary>
		/// Syslog받기 소켓 입니다.
		/// </summary>
		private TMSUdpSocket m_Socket = null;
		/// <summary>
		/// 로컬 IP주소 입니다.
		/// </summary>
		private string m_LocalIPAddress = "";
		/// <summary>
		/// Syslog 서버의 포트번호 입니다.
		/// </summary>
		private int m_PortNumber = 514;
		/// <summary>
		/// Syslog 스래드의 작동중인지의 여부입니다.
		/// </summary>
		private bool m_isRun = false;
		/// <summary>
		/// Syslog서버의 중지 여부 입니다.
		/// </summary>
		private bool m_isShutDown = false;
		/// <summary>
		/// 스래드가 작동중인지의 여부 입니다.
		/// </summary>
		private bool m_isThreadRun = false;
		/// <summary>
		/// Syslog 처리를 위한 사용자 설정 이벤트 입니다.
		/// </summary>
		private ManualResetEvent m_SyslogMRE = null;
        /// <summary>
        /// Syslog를 처리 할 스래드 풀입니다.
        /// </summary>
        private SECUONELibrary.SOThreadPool m_SyslogWorkThreadPool;

		/// <summary>
		/// Syslog 패킷을 임시로 저장할 큐 입니다.
		/// </summary>
		private Queue m_SyslogQueue = null;
		/// <summary>
        /// Syslog 큐의 최대 개수입니다.
		/// </summary>
		private long m_MaxQueueCount = 65535;
		/// <summary>
        /// Syslog 처리에서 사용할 수동 리셋 이벤트 입니다.
		/// </summary>
		private ManualResetEvent m_SyslogProcessMRE = null;
		/// <summary>
        /// Syslog 처리 스래드 입니다.
		/// </summary>
		private Thread m_TrapProcessThread = null;

        /// <summary>
        /// 로그파일의 저장 위치 입니다.
        /// </summary>
        private string m_LogPath = "";
        /// <summary>
        /// Log그를 저장할 파일로그 객체 입니다.
        /// </summary>
        private SOFileLog m_Log = null;

        /// <summary>
        /// Syslog서버를 시작시키고 감시하기 위한 스래드입니다. 
        /// </summary>
        private Thread tThread = null;

		/// <summary>
		/// Syslog컨트롤의 생성자 입니다.
		/// </summary>
		/// <param name="container">컨트롤이 추가될 Container객체 입니다.</param>
		public SOSyslog(System.ComponentModel.IContainer container)
		{
			container.Add(this);
			InitializeComponent();
		}

		/// <summary>
		/// Syslog컨트롤의 기본 생성자 입니다.
		/// </summary>
		public SOSyslog()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Syslog컨트롤의 생성자 입니다.
		/// </summary>
		/// <param name="vMaxQueueCount">최대 Syslog Queue개수 입니다.</param>
		public SOSyslog(long vMaxQueueCount)
		{
			m_MaxQueueCount = vMaxQueueCount;
			InitializeComponent();
		}

		/// <summary> 
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			//Syslog 서버를 중지합니다.
			StopSyslogServer();

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		
		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion //변수, 이벤트, 생성자 처리부분 입니다 ---------------------------------------

		#region 컨트롤 속성 부분 입니다 ----------------------------------------------------
		/// <summary>
		/// Syslog서버의 로컬 IP주소를 가져오거나 설정합니다.
		/// </summary>
		public string LocalIPAddress
		{
			get { return m_LocalIPAddress; }
			set { m_LocalIPAddress = value; }
		}

		/// <summary>
		/// Syslog서버의 포트번호를 가져옵니다.
		/// </summary>
		public int PortNumber
		{
			get { return m_PortNumber; }
			set { m_PortNumber = value; }
		}

		/// <summary>
		/// Syslog서버의 작동여부를 가져오거나 설정합니다.
		/// </summary>
		[ToolboxItem(false)]
		public bool isRunning
		{
			get 
			{
				lock(this)
				{
					return m_isRun; 
				}
			}
		}
		#endregion //컨트롤 속성 부분 입니다 ----------------------------------------------------

		#region 컨트롤 공용 메소드 부분 입니다 ---------------------------------------------
		/// <summary>
		/// Syslog서버를 시작합니다.
		/// </summary>
		public void StartSyslogServer(int vPort )
		{
            //------------------------------------------------------------------------------
            //파일 로거를 초기화 합니다.
            m_LogPath = System.Windows.Forms.Application.StartupPath + "\\log\\"; //
            m_Log = new SOFileLog(m_LogPath, "SOSyslog", true, true);
            m_Log.Extension = "log";
            //------------------------------------------------------------------------------
            m_PortNumber = vPort;

			if(m_isThreadRun == false)
			{
				m_SyslogMRE = new ManualResetEvent(false);
				m_SyslogProcessMRE = new ManualResetEvent(false);
				m_SyslogQueue = new Queue();

				m_isShutDown = false;
				tThread = new Thread(new ThreadStart(RunSyslogServer));
				tThread.Start();
				m_TrapProcessThread = new Thread(new ThreadStart(SyslogProcess));
				m_TrapProcessThread.Start();
                //작업을 처리할 스래드 풀을 생성합니다.
                //m_SyslogWorkThreadPool = new SECUONELibrary.SOThreadPool(10);
                //m_SyslogWorkThreadPool.StartThreadPool();
			}
		}

		/// <summary>
		/// Syslog 서버를 중지합니다.
		/// </summary>
		public void StopSyslogServer()
		{
			try
			{
				m_isShutDown = true;
				m_isThreadRun = false;

				if(m_SyslogMRE != null) m_SyslogMRE.Set();
				if(m_SyslogProcessMRE != null) m_SyslogProcessMRE.Set();

                m_Socket.DataReceived -= new TMSDataReceivedEventHandler(m_Socket_DataReceived);

				if(m_Socket != null)
				{
					m_Socket.Dispose();
					m_Socket = null;
				}

				if(m_SyslogMRE != null)
				{
					m_SyslogMRE.Close();
					m_SyslogMRE = null;
				}

				if(m_SyslogProcessMRE != null)
				{
					m_SyslogProcessMRE.Close();
					m_SyslogProcessMRE = null;
				}

				if(m_SyslogQueue != null)
				{
					m_SyslogQueue.Clear();
					m_SyslogQueue = null;
				}
                tThread.Join(10);

                //m_Log.PrintLogEnter("tmsSyslogDemon - StopSyslogDemon : Step 2");
                //스래드를 종료 합니다.
                if (tThread.IsAlive)
                {
                    try
                    {
                        //Syslog 처리 스래드를 강제 종료 합니다.
                        tThread.Abort();
                    }
                    catch (Exception ex)
                    {
                        ex = ex;
                    }
                }
                m_TrapProcessThread.Join(10);

                //m_Log.PrintLogEnter("tmsSyslogDemon - StopSyslogDemon : Step 2");
                //스래드를 종료 합니다.
                if (m_TrapProcessThread.IsAlive)
                {
                    try
                    {
                        //Syslog 처리 스래드를 강제 종료 합니다.
                        m_TrapProcessThread.Abort();
                    }
                    catch (Exception ex)
                    {
                        ex = ex;
                    }
                }
                m_Log.Dispose();
			}
			catch(Exception ex)
			{
                Console.WriteLine(ex.Message);
				ex=ex;
			}
		}
		#endregion //컨트롤 공용 메소드 부분 입니다 ---------------------------------------------

		#region 컨트롤 지역 메소드 부분 입니다 ---------------------------------------------
		/// <summary>
		/// Syslog서버를 시작시키고 감시합니다.
		/// </summary>
		private void RunSyslogServer()
		{
			m_isThreadRun = true;
			while(!m_isShutDown)
			{
				if(m_Socket == null)
				{
					m_Socket = new TMSUdpSocket();
					m_Socket.DataReceived += new TMSDataReceivedEventHandler(m_Socket_DataReceived);
					if(m_LocalIPAddress == "")
					{
						m_Socket.StartListening(m_PortNumber, 10);
					}
					else
					{
						m_Socket.StartListening(m_LocalIPAddress, m_PortNumber, 10);
					}
				}
				m_SyslogMRE.Reset();
				m_SyslogMRE.WaitOne();	//소켓이 중지될때까지 대기합니다.
			}
			m_isThreadRun = false;
		}		

		//데이터받음을 처리합니다.
		private void m_Socket_DataReceived(object sender, TMSDataReceivedEventArgs e)
		{
			try
			{
				byte [] tPacket = null;
				int tCount = e.ReadCount;
                int i = 0;
                for (i = tCount - 1; i >= 0; i--)
                {
                    if ((int)e.SocketData.Buffers[i] > 15)
                    {
                        break;
                    }
                }
                tCount = i + 1;
				if(tCount > 0)
				{				
					tPacket = new byte[tCount];
					Array.Copy(e.SocketData.Buffers, tPacket, tCount);

                    byte[] tData = new byte[tCount];
                    TMSSyslogPacket tPkt = null;
                    tPkt = new TMSSyslogPacket(tPacket, e.SenderIPAddress);
                    Array.Copy(tPkt.Packet, tData, tCount);
                    TMSSyslogMessage tSyslogMessage = SOSyslogClass.DecodeSyslogPacket(tData);
                    //m_Log.PrintLogEnter(tSyslogMessage.Message);
                    lock (m_SyslogQueue.SyncRoot)
                    {
                        if (m_SyslogQueue.Count < m_MaxQueueCount)
                        {
                            m_SyslogQueue.Enqueue(new TMSSyslogPacket(tPacket, e.SenderIPAddress));
                            m_SyslogProcessMRE.Set();
                        }
                    }
				}
				m_Socket.StartReceive(e.SocketData);
			}
			catch(Exception ex)
			{
				Console.WriteLine("소켓 데이터 처리 오류: " + ex.ToString());
				m_Socket.Dispose();
				m_Socket = null;
				m_isShutDown = true;
				m_SyslogMRE.Set();
			}
		}

		/// <summary>
		/// Queue에 쌓여진 Syslog패킷을 처리합니다.
		/// </summary>
		private void SyslogProcess()
		{
			TMSSyslogPacket tPacket = null;

			while(!m_isShutDown)
			{
				tPacket = null;
				
				lock(m_SyslogQueue.SyncRoot)
				{
					if(m_SyslogQueue.Count > 0)
					{
						tPacket = (TMSSyslogPacket)m_SyslogQueue.Dequeue();
					}
				}

                if (tPacket != null)
                {
                    int tCount = tPacket.Packet.Length;
                    int i = 0;
                    for (i = tCount - 1; i >= 0; i--)
                    {
                        if ((int)tPacket.Packet[i] > 15)
                        {
                            break;
                        }
                    }
                    tCount = i + 1;

                    byte[] tData = new byte[tCount];
                    Array.Copy(tPacket.Packet, tData, tCount);
                    TMSSyslogMessage tSyslogMessage = SOSyslogClass.DecodeSyslogPacket(tData);
                    //m_Log.PrintLogEnter(tSyslogMessage.Message);
                    if (SyslogReceived != null) SyslogReceived(this, new SyslogEventArgs(tSyslogMessage, tPacket.Time, tPacket.SenderAddress));
                    Thread.Sleep(1);
                }
                else
                {
                    m_SyslogProcessMRE.Reset();
                    m_SyslogProcessMRE.WaitOne();
                }
			}
		}
		#endregion //컨트롤 지역 메소드 부분 입니다 ---------------------------------------------
	}

	#region Syslog 패킷 임시 저장 클래스 입니다 --------------------------------------------
	/// <summary>
	/// Syslog 패킷을 저장하는 클래스 입니다.
	/// </summary>
	public class TMSSyslogPacket
	{
		/// <summary>
		/// Syslog 패킷 데이터 입니다.
		/// </summary>
		private byte [] m_Packet = null;
		/// <summary>
		/// Syslog을 받은 시간입니다.
		/// </summary>
		private DateTime m_Time = DateTime.Now;
		/// <summary>
		/// Syslog 발신 장비 IP주소 입니다.
		/// </summary>
		private string m_SenderAddress = "";

		/// <summary>
		/// Syslog 패킷 클래스의 생성자 입니다;
		/// </summary>
		/// <param name="vPacket">Syslog 패킷 데이터 입니다.</param>
		/// <param name="vSenderAddress">Syslog 발신 장비 IP주소 입니다.</param>
		public TMSSyslogPacket(byte [] vPacket, string vSenderAddress)
		{
			m_Packet = vPacket;
			m_SenderAddress = vSenderAddress;
		}

		/// <summary>
		/// Syslog 패킷 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vPacket">Syslog 패킷 데이터 입니다.</param>
		/// <param name="vTime">Syslog이 발생한 시간입니다.</param>
		/// <param name="vSenderAddress">Syslog 발신 장비 IP주소 입니다.</param>
		public TMSSyslogPacket(byte [] vPacket, DateTime vTime, string vSenderAddress)
		{
			m_Packet = vPacket;
			m_Time = vTime;
			m_SenderAddress = vSenderAddress;
		}

		/// <summary>
		/// Syslog 패킷 데이터를 가져옵니다.
		/// </summary>
		public byte [] Packet
		{
			get { return m_Packet; }
		}

		/// <summary>
		/// Syslog 발생시간을 가져옵니다.
		/// </summary>
		public DateTime Time
		{
			get { return m_Time; }
		}

		/// <summary>
		/// Syslog 발신 장비의 IP주소를 가져옵니다.
		/// </summary>
		public string SenderAddress
		{
			get { return m_SenderAddress; }
		}
	}
	#endregion //Syslog 패킷 임시 저장 클래스 입니다 --------------------------------------------

	#region Syslog 이벤트 어규먼트 클래스 입니다 ----------------------------------------
	/// <summary>
	/// Syslog 이벤트 어규먼트 클래스 입니다. 
	/// </summary>
	public class SyslogEventArgs : EventArgs
	{
		/// <summary>
		/// Syslog 정보가 저장될 TMSSyslogMessage객체 입니다.
		/// </summary>
		private TMSSyslogMessage m_SyslogMessage = null;
		/// <summary>
		/// Syslog 발생 시간입니다.
		/// </summary>
		private DateTime m_Time = DateTime.Now;
		/// <summary>
		/// Syslog 발신 장비 IP주소 입니다.
		/// </summary>
		private string m_SenderAddress = "";

		/// <summary>
		/// Syslog 이벤트 어규먼트 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vSyslog">Syslog Message 객체입니다.</param>
		/// <param name="vSenderAddress">Syslog 발신 장비 IP주소 입니다.</param>
		public SyslogEventArgs(TMSSyslogMessage vSyslogMessage, string vSenderAddress)
		{
			m_SyslogMessage = vSyslogMessage;
			m_SenderAddress = vSenderAddress;
		}

		/// <summary>
		/// Syslog 이벤트 어규먼트 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vSyslog">Syslog Message 객체입니다.</param>
		/// <param name="vTime">트랩 발생 시간입니다.</param>
		/// <param name="vSenderAddress">트랩 발신 장비 IP주소 입니다.</param>
		public SyslogEventArgs(TMSSyslogMessage vSyslogMessage, DateTime vTime, string vSenderAddress)
		{
			m_SyslogMessage = vSyslogMessage;
			m_SenderAddress = vSenderAddress;
			m_Time = vTime;
		}

		/// <summary>
		/// Syslog Message객체를 가져옵니다.
		/// </summary>
		public TMSSyslogMessage SyslogMessage
		{
			get { return m_SyslogMessage; }
		}

		/// <summary>
		/// Syslog 발생 시간을 가져옵니다.
		/// </summary>
		public DateTime Time
		{
			get { return m_Time; }
		}

		/// <summary>
		/// Syslog 발신 장비의 IP주소를 가져옵니다.
		/// </summary>
		public string SenderAddress
		{
			get { return m_SenderAddress; }
		}
	}
	/// <summary>
	/// Syslog받음을 알릴 이벤트 핸들러 입니다.
	/// </summary>
	public delegate void SyslogReceivedEventHandler(object sender, SyslogEventArgs e);	
	#endregion //Syslog 이벤트 어규먼트 클래스 입니다 ----------------------------------------
}
