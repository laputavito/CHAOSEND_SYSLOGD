using System;
using System.ComponentModel;
using System.Threading;
using System.Net;
using System.Collections;


namespace SYSLOGD
{
	/// <summary>
	/// TMSSnmpTrap에 대한 요약 설명입니다.
	/// </summary>
	public class SOSnmpTrap : System.ComponentModel.Component
	{
		#region 변수, 이벤트, 생성자 처리부분 입니다 ---------------------------------------
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// Snmp Version1 트랩 받음을 알리는 이벤트 입니다.
		/// </summary>
		public event SnmpV1TrapReceivedEventHandler TrapV1Received = null;
		/// <summary>
		/// Snmp Version2 트랩 받음을 알리는 이벤트 입니다.
		/// </summary>
		public event SnmpV2TrapReceivedEventHandler TrapV2Received = null;
		/// <summary>
		/// 트랩을 받을 소캣입니다.
		/// </summary>
		private TMSUdpSocket m_Socket = null;
		/// <summary>
		/// 로컬 IP주소 입니다.
		/// </summary>
		private string m_LocalIPAddress = "";
		/// <summary>
		/// Trap 서버의 포트번호 입니다.
		/// </summary>
		private int m_PortNumber = 162;
		/// <summary>
		/// 트랩 스래드의 작동중인지의 여부입니다.
		/// </summary>
		private bool m_isRun = false;
		/// <summary>
		/// 트랩 스래드를 사용자가 중지 시켰는지의 여부 입니다.
		/// </summary>
		private bool m_isShutDown = false;
		/// <summary>
		/// 트랩 스래드가 작동중인지의 여부 입니다.
		/// </summary>
		private bool m_isThreadRun = false;
		/// <summary>
		/// 트랩 처리를 위한 수동 리셋 이벤트 입니다.
		/// </summary>
		private ManualResetEvent m_TrapMRE = null;
		/// <summary>
		/// 트랩 패킷을 임시로 저장할 큐 입니다.
		/// </summary>
		private Queue m_TrapQueue = null;
		/// <summary>
		/// 트랩 큐의 최대 개수입니다.
		/// </summary>
		private long m_MaxQueueCount = 65535;
		/// <summary>
		/// 트랩 처리에서 사용할 수동 리셋 이벤트 입니다.
		/// </summary>
		private ManualResetEvent m_TrapProcessMRE = null;
		/// <summary>
		/// 트랩 처리 스래드 입니다.
		/// </summary>
		private Thread m_TrapProcessThread = null;
		/// <summary>
		/// Snmp Trap컨트롤의 생성자 입니다.
		/// </summary>
		/// <param name="container">컨트롤이 추가될 Container객체 입니다.</param>
		public SOSnmpTrap(System.ComponentModel.IContainer container)
		{
			container.Add(this);
			InitializeComponent();
		}

		/// <summary>
		/// Snmp Trap컨트롤의 기본 생성자 입니다.
		/// </summary>
		public SOSnmpTrap()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Snmp Trap컨트롤의 기본 생성자 입니다.
		/// </summary>
		/// <param name="vMaxQueueCount">최대 Trap Queue개수 입니다.</param>
		public SOSnmpTrap(long vMaxQueueCount)
		{
			m_MaxQueueCount = vMaxQueueCount;
			InitializeComponent();
		}

		/// <summary> 
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			//Trap 서버를 중지합니다.
			StopTrapServer();

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
		/// 트랩서버의 로컬 IP주소를 가져오거나 설정합니다.
		/// </summary>
		public string LocalIPAddress
		{
			get { return m_LocalIPAddress; }
			set { m_LocalIPAddress = value; }
		}

		/// <summary>
		/// 트랩서버의 포트번호를 가져옵니다.
		/// </summary>
		public int PortNumber
		{
			get { return m_PortNumber; }
			set { m_PortNumber = value; }
		}

		/// <summary>
		/// 트랩서버의 작동여부를 가져오거나 설정합니다.
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
		/// 트랩서버를 시작합니다.
		/// </summary>
		public void StartTrapServer()
		{
			if(m_isThreadRun == false)
			{
				m_TrapMRE = new ManualResetEvent(false);
				m_TrapProcessMRE = new ManualResetEvent(false);
				m_TrapQueue = new Queue();

				m_isShutDown = false;
				Thread tThread = new Thread(new ThreadStart(RunTrapServer));
				tThread.Start();
				m_TrapProcessThread = new Thread(new ThreadStart(TrapProcess));
				m_TrapProcessThread.Start();
			}
		}

		/// <summary>
		/// 트랩 서버를 중지합니다.
		/// </summary>
		public void StopTrapServer()
		{
			try
			{
				m_isShutDown = true;
				m_isThreadRun = false;

				if(m_TrapMRE != null) m_TrapMRE.Set();
				if(m_TrapProcessMRE != null) m_TrapProcessMRE.Set();

				if(m_Socket != null)
				{				
					m_Socket.Dispose();
					m_Socket = null;
				}

				if(m_TrapMRE != null)
				{
					m_TrapMRE.Close();
					m_TrapMRE = null;
				}

				if(m_TrapProcessMRE != null)
				{
					m_TrapProcessMRE.Close();
					m_TrapProcessMRE = null;
				}

				if(m_TrapQueue != null)
				{
					m_TrapQueue.Clear();
					m_TrapQueue = null;
				}
			}
			catch(Exception ex)
			{
				ex=ex;
			}
		}
        /// <summary>
        /// 트랩서버를 시작합니다.
        /// </summary>
        public void SendTrap(byte[] vSendData, EndPoint vEP )
        {
            m_Socket.SendDataTo(vSendData, vEP);
        }
		#endregion //컨트롤 공용 메소드 부분 입니다 ---------------------------------------------

		#region 컨트롤 지역 메소드 부분 입니다 ---------------------------------------------
		/// <summary>
		/// 트랩서버를 시작시키고 감시합니다.
		/// </summary>
		private void RunTrapServer()
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
						m_Socket.StartListening(m_PortNumber, 1);
					}
					else
					{
						m_Socket.StartListening(m_LocalIPAddress, m_PortNumber, 1);
					}
				}
				m_TrapMRE.Reset();
				m_TrapMRE.WaitOne();	//소켓이 중지될때까지 대기합니다.
			}
			m_isThreadRun = false;
		}		

		//데이터받음을 처리합니다.
		private void m_Socket_DataReceived(object sender, TMSDataReceivedEventArgs e)
		{
//			try
//			{	
//				ITMSSnmpPDU tPDU = TMSSnmpClass.DecodeSnmpPDU(e.SocketData.Buffers);
//
//				switch(tPDU.PDUType)
//				{
//					case E_SnmpPDUType.TrapV1:
//						if(TrapV1Received != null) TrapV1Received(this, new SnmpV1TrapEventArgs(tPDU));
//						break;
//
//					case E_SnmpPDUType.TrapV2:
//						if(TrapV2Received != null) TrapV2Received(this, new SnmpV2TrapEventArgs(tPDU));
//						break;
//				}
//
//				m_Socket.StartReceive(e.SocketData);
//			}
//			catch(Exception ex)
//			{
//				Console.WriteLine("소켓 데이터 처리 오류: " + ex.ToString());
//				m_Socket.Dispose();
//				m_Socket = null;
//				m_TrapMRE.Set();
//			}

			try
			{	
//				ITMSSnmpPDU tPDU = TMSSnmpClass.DecodeSnmpPDU(e.SocketData.Buffers);
//
//				switch(tPDU.PDUType)
//				{
//					case E_SnmpPDUType.TrapV1:
//						if(TrapV1Received != null) TrapV1Received(this, new SnmpV1TrapEventArgs(tPDU));
//						break;
//
//					case E_SnmpPDUType.TrapV2:
//						if(TrapV2Received != null) TrapV2Received(this, new SnmpV2TrapEventArgs(tPDU));
//						break;
//				}

				byte [] tPacket = new byte[e.ReadCount];
				Array.Copy(e.SocketData.Buffers, tPacket, e.ReadCount);

				lock(m_TrapQueue.SyncRoot)
				{
					if(m_TrapQueue.Count < m_MaxQueueCount)
					{						
						m_TrapQueue.Enqueue(new TMSSnmpTrapPacket(tPacket, e.SenderIPAddress));
						m_TrapProcessMRE.Set();
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
				m_TrapMRE.Set();
			}
		}

		/// <summary>
		/// Queue에 쌓여진 Trap패킷을 처리합니다.
		/// </summary>
		private void TrapProcess()
		{
			TMSSnmpTrapPacket tPacket = null;

			while(!m_isShutDown)
			{
				tPacket = null;
				
				lock(m_TrapQueue.SyncRoot)
				{
					if(m_TrapQueue.Count > 0)
					{
						tPacket = (TMSSnmpTrapPacket)m_TrapQueue.Dequeue();
					}
				}
				
				if(tPacket != null)
				{
					ITMSSnmpPDU tPDU = SOSnmpClass.DecodeSnmpPDU(tPacket.Packet);
	
					switch(tPDU.PDUType)
					{
						case E_SnmpPDUType.TrapV1:
							if(TrapV1Received != null) TrapV1Received(this, new SnmpV1TrapEventArgs(tPDU, tPacket.Time));
							break;
	
						case E_SnmpPDUType.TrapV2:
							if(TrapV2Received != null) TrapV2Received(this, new SnmpV2TrapEventArgs(tPDU, tPacket.Time, tPacket.SenderAddress));
							break;
					}
					Thread.Sleep(1);
				}
				else
				{
					m_TrapProcessMRE.Reset();
					m_TrapProcessMRE.WaitOne();
				}
			}
		}
		#endregion //컨트롤 지역 메소드 부분 입니다 ---------------------------------------------
	}

	#region 트랩 패킷 클래스 입니다 --------------------------------------------------------
	/// <summary>
	/// Snmp트랩 패킷을 저장하는 클래스 입니다.
	/// </summary>
	public class TMSSnmpTrapPacket
	{
		/// <summary>
		/// 트랩 패킷 데이터 입니다.
		/// </summary>
		private byte [] m_Packet = null;
		/// <summary>
		/// 트랩을 받은 시간입니다.
		/// </summary>
		private DateTime m_Time = DateTime.Now;
		/// <summary>
		/// 트랩 발신 장비 IP주소 입니다.
		/// </summary>
		private string m_SenderAddress = "";

		/// <summary>
		/// 트랩 패킷 클래스의 생성자 입니다;
		/// </summary>
		/// <param name="vPacket">트랩 패킷 데이터 입니다.</param>
		/// <param name="vSenderAddress">트랩 발신 장비 IP주소 입니다.</param>
		public TMSSnmpTrapPacket(byte [] vPacket, string vSenderAddress)
		{
			m_Packet = vPacket;
			m_SenderAddress = vSenderAddress;
		}

		/// <summary>
		/// 트랩 패킷 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vPacket">트랩 패킷 데이터 입니다.</param>
		/// <param name="vTime">트랩이 발생한 시간입니다.</param>
		/// <param name="vSenderAddress">트랩 발신 장비 IP주소 입니다.</param>
		public TMSSnmpTrapPacket(byte [] vPacket, DateTime vTime, string vSenderAddress)
		{
			m_Packet = vPacket;
			m_Time = vTime;
			m_SenderAddress = vSenderAddress;
		}

		/// <summary>
		/// 트랩 패킷 데이터를 가져옵니다.
		/// </summary>
		public byte [] Packet
		{
			get { return m_Packet; }
		}

		/// <summary>
		/// 트랩 발생시간을 가져옵니다.
		/// </summary>
		public DateTime Time
		{
			get { return m_Time; }
		}

		/// <summary>
		/// 트랩 발신 장비의 IP주소를 가져옵니다.
		/// </summary>
		public string SenderAddress
		{
			get { return m_SenderAddress; }
		}
	}
	#endregion //트랩 패킷 클래스 입니다 --------------------------------------------------------

	#region Snmp Trap 이벤트 어규먼트 클래스 입니다 ----------------------------------------
	/// <summary>
	/// Snmp Version 1 트랩 이벤트 어규먼트 클래스 입니다. 
	/// </summary>
	public class SnmpV1TrapEventArgs : EventArgs
	{
		/// <summary>
		/// 트랩 정보가 저장될 PDU객체 입니다.
		/// </summary>
		private TMSSnmp_TrapPDU m_TrapPDU = null;
		/// <summary>
		/// 트랩 발생 시간입니다.
		/// </summary>
		private DateTime m_Time = DateTime.Now;

		/// <summary>
		/// Snmp Version 1 트랩 이벤트 어규먼트 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vPDU">Snmp PDU 인터페이스 객체입니다.</param>
		public SnmpV1TrapEventArgs(ITMSSnmpPDU vPDU)
		{
			m_TrapPDU = (TMSSnmp_TrapPDU)vPDU;
		}

		/// <summary>
		/// Snmp Version 1 트랩 이벤트 어규먼트 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vPDU">Snmp PDU 인터페이스 객체입니다.</param>
		/// <param name="vTime">트랩 발생 시간입니다.</param>
		public SnmpV1TrapEventArgs(ITMSSnmpPDU vPDU, DateTime vTime)
		{
			m_Time = vTime;
			m_TrapPDU = (TMSSnmp_TrapPDU)vPDU;
		}

		/// <summary>
		/// Snmp Version 1 트랩 PDU객체를 가져옵니다.
		/// </summary>
		public TMSSnmp_TrapPDU TrapPDU
		{
			get { return m_TrapPDU; }
		}

		/// <summary>
		/// 트랩 발생 시간을 가져옵니다.
		/// </summary>
		public DateTime Time
		{
			get { return m_Time; }
		}
	}
	/// <summary>
	/// Snmp Version 1 트랩받음을 알릴 이벤트 핸들러 입니다.
	/// </summary>
	public delegate void SnmpV1TrapReceivedEventHandler(object sender, SnmpV1TrapEventArgs e);

	/// <summary>
	/// Snmp Version 2 트랩 이벤트 어규먼트 클래스 입니다. 
	/// </summary>
	public class SnmpV2TrapEventArgs : EventArgs
	{
		/// <summary>
		/// 트랩 정보가 저장될 PDU객체 입니다.
		/// </summary>
		private TMSSnmpV2_TrapPDU m_TrapPDU = null;
		/// <summary>
		/// 트랩 발생 시간입니다.
		/// </summary>
		private DateTime m_Time = DateTime.Now;
		/// <summary>
		/// 트랩 발신 장비 IP주소 입니다.
		/// </summary>
		private string m_SenderAddress = "";

		/// <summary>
		/// Snmp Version 2 트랩 이벤트 어규먼트 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vPDU">Snmp PDU 인터페이스 객체입니다.</param>
		/// <param name="vSenderAddress">트랩 발신 장비 IP주소 입니다.</param>
		public SnmpV2TrapEventArgs(ITMSSnmpPDU vPDU, string vSenderAddress)
		{
			m_TrapPDU = (TMSSnmpV2_TrapPDU)vPDU;
			m_SenderAddress = vSenderAddress;
		}

		/// <summary>
		/// Snmp Version 2 트랩 이벤트 어규먼트 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vPDU">Snmp PDU 인터페이스 객체입니다.</param>
		/// <param name="vTime">트랩 발생 시간입니다.</param>
		/// <param name="vSenderAddress">트랩 발신 장비 IP주소 입니다.</param>
		public SnmpV2TrapEventArgs(ITMSSnmpPDU vPDU, DateTime vTime, string vSenderAddress)
		{
			m_TrapPDU = (TMSSnmpV2_TrapPDU)vPDU;
			m_SenderAddress = vSenderAddress;
			m_Time = vTime;
		}

		/// <summary>
		/// Snmp Version 2 트랩 PDU객체를 가져옵니다.
		/// </summary>
		public TMSSnmpV2_TrapPDU TrapPDU
		{
			get { return m_TrapPDU; }
		}

		/// <summary>
		/// 트랩 발생 시간을 가져옵니다.
		/// </summary>
		public DateTime Time
		{
			get { return m_Time; }
		}

		/// <summary>
		/// 트랩 발신 장비의 IP주소를 가져옵니다.
		/// </summary>
		public string SenderAddress
		{
			get { return m_SenderAddress; }
		}
	}
	/// <summary>
	/// Snmp Version 2 트랩받음을 알릴 이벤트 핸들러 입니다.
	/// </summary>
	public delegate void SnmpV2TrapReceivedEventHandler(object sender, SnmpV2TrapEventArgs e);
	#endregion //Snmp Trap 이벤트 어규먼트 클래스 입니다 ----------------------------------------
}
