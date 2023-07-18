using System;
using System.ComponentModel;
using System.Threading;
using System.Net;
using System.Collections;


namespace SYSLOGD
{
	/// <summary>
	/// TMSSnmpTrap�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SOSnmpTrap : System.ComponentModel.Component
	{
		#region ����, �̺�Ʈ, ������ ó���κ� �Դϴ� ---------------------------------------
		/// <summary> 
		/// �ʼ� �����̳� �����Դϴ�.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// Snmp Version1 Ʈ�� ������ �˸��� �̺�Ʈ �Դϴ�.
		/// </summary>
		public event SnmpV1TrapReceivedEventHandler TrapV1Received = null;
		/// <summary>
		/// Snmp Version2 Ʈ�� ������ �˸��� �̺�Ʈ �Դϴ�.
		/// </summary>
		public event SnmpV2TrapReceivedEventHandler TrapV2Received = null;
		/// <summary>
		/// Ʈ���� ���� ��Ĺ�Դϴ�.
		/// </summary>
		private TMSUdpSocket m_Socket = null;
		/// <summary>
		/// ���� IP�ּ� �Դϴ�.
		/// </summary>
		private string m_LocalIPAddress = "";
		/// <summary>
		/// Trap ������ ��Ʈ��ȣ �Դϴ�.
		/// </summary>
		private int m_PortNumber = 162;
		/// <summary>
		/// Ʈ�� �������� �۵��������� �����Դϴ�.
		/// </summary>
		private bool m_isRun = false;
		/// <summary>
		/// Ʈ�� �����带 ����ڰ� ���� ���״����� ���� �Դϴ�.
		/// </summary>
		private bool m_isShutDown = false;
		/// <summary>
		/// Ʈ�� �����尡 �۵��������� ���� �Դϴ�.
		/// </summary>
		private bool m_isThreadRun = false;
		/// <summary>
		/// Ʈ�� ó���� ���� ���� ���� �̺�Ʈ �Դϴ�.
		/// </summary>
		private ManualResetEvent m_TrapMRE = null;
		/// <summary>
		/// Ʈ�� ��Ŷ�� �ӽ÷� ������ ť �Դϴ�.
		/// </summary>
		private Queue m_TrapQueue = null;
		/// <summary>
		/// Ʈ�� ť�� �ִ� �����Դϴ�.
		/// </summary>
		private long m_MaxQueueCount = 65535;
		/// <summary>
		/// Ʈ�� ó������ ����� ���� ���� �̺�Ʈ �Դϴ�.
		/// </summary>
		private ManualResetEvent m_TrapProcessMRE = null;
		/// <summary>
		/// Ʈ�� ó�� ������ �Դϴ�.
		/// </summary>
		private Thread m_TrapProcessThread = null;
		/// <summary>
		/// Snmp Trap��Ʈ���� ������ �Դϴ�.
		/// </summary>
		/// <param name="container">��Ʈ���� �߰��� Container��ü �Դϴ�.</param>
		public SOSnmpTrap(System.ComponentModel.IContainer container)
		{
			container.Add(this);
			InitializeComponent();
		}

		/// <summary>
		/// Snmp Trap��Ʈ���� �⺻ ������ �Դϴ�.
		/// </summary>
		public SOSnmpTrap()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Snmp Trap��Ʈ���� �⺻ ������ �Դϴ�.
		/// </summary>
		/// <param name="vMaxQueueCount">�ִ� Trap Queue���� �Դϴ�.</param>
		public SOSnmpTrap(long vMaxQueueCount)
		{
			m_MaxQueueCount = vMaxQueueCount;
			InitializeComponent();
		}

		/// <summary> 
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			//Trap ������ �����մϴ�.
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
		/// �����̳� ������ �ʿ��� �޼����Դϴ�. 
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion //����, �̺�Ʈ, ������ ó���κ� �Դϴ� ---------------------------------------

		#region ��Ʈ�� �Ӽ� �κ� �Դϴ� ----------------------------------------------------
		/// <summary>
		/// Ʈ�������� ���� IP�ּҸ� �������ų� �����մϴ�.
		/// </summary>
		public string LocalIPAddress
		{
			get { return m_LocalIPAddress; }
			set { m_LocalIPAddress = value; }
		}

		/// <summary>
		/// Ʈ�������� ��Ʈ��ȣ�� �����ɴϴ�.
		/// </summary>
		public int PortNumber
		{
			get { return m_PortNumber; }
			set { m_PortNumber = value; }
		}

		/// <summary>
		/// Ʈ�������� �۵����θ� �������ų� �����մϴ�.
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
		#endregion //��Ʈ�� �Ӽ� �κ� �Դϴ� ----------------------------------------------------

		#region ��Ʈ�� ���� �޼ҵ� �κ� �Դϴ� ---------------------------------------------
		/// <summary>
		/// Ʈ�������� �����մϴ�.
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
		/// Ʈ�� ������ �����մϴ�.
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
        /// Ʈ�������� �����մϴ�.
        /// </summary>
        public void SendTrap(byte[] vSendData, EndPoint vEP )
        {
            m_Socket.SendDataTo(vSendData, vEP);
        }
		#endregion //��Ʈ�� ���� �޼ҵ� �κ� �Դϴ� ---------------------------------------------

		#region ��Ʈ�� ���� �޼ҵ� �κ� �Դϴ� ---------------------------------------------
		/// <summary>
		/// Ʈ�������� ���۽�Ű�� �����մϴ�.
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
				m_TrapMRE.WaitOne();	//������ �����ɶ����� ����մϴ�.
			}
			m_isThreadRun = false;
		}		

		//�����͹����� ó���մϴ�.
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
//				Console.WriteLine("���� ������ ó�� ����: " + ex.ToString());
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
				Console.WriteLine("���� ������ ó�� ����: " + ex.ToString());
				m_Socket.Dispose();
				m_Socket = null;
				m_isShutDown = true;
				m_TrapMRE.Set();
			}
		}

		/// <summary>
		/// Queue�� �׿��� Trap��Ŷ�� ó���մϴ�.
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
		#endregion //��Ʈ�� ���� �޼ҵ� �κ� �Դϴ� ---------------------------------------------
	}

	#region Ʈ�� ��Ŷ Ŭ���� �Դϴ� --------------------------------------------------------
	/// <summary>
	/// SnmpƮ�� ��Ŷ�� �����ϴ� Ŭ���� �Դϴ�.
	/// </summary>
	public class TMSSnmpTrapPacket
	{
		/// <summary>
		/// Ʈ�� ��Ŷ ������ �Դϴ�.
		/// </summary>
		private byte [] m_Packet = null;
		/// <summary>
		/// Ʈ���� ���� �ð��Դϴ�.
		/// </summary>
		private DateTime m_Time = DateTime.Now;
		/// <summary>
		/// Ʈ�� �߽� ��� IP�ּ� �Դϴ�.
		/// </summary>
		private string m_SenderAddress = "";

		/// <summary>
		/// Ʈ�� ��Ŷ Ŭ������ ������ �Դϴ�;
		/// </summary>
		/// <param name="vPacket">Ʈ�� ��Ŷ ������ �Դϴ�.</param>
		/// <param name="vSenderAddress">Ʈ�� �߽� ��� IP�ּ� �Դϴ�.</param>
		public TMSSnmpTrapPacket(byte [] vPacket, string vSenderAddress)
		{
			m_Packet = vPacket;
			m_SenderAddress = vSenderAddress;
		}

		/// <summary>
		/// Ʈ�� ��Ŷ Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vPacket">Ʈ�� ��Ŷ ������ �Դϴ�.</param>
		/// <param name="vTime">Ʈ���� �߻��� �ð��Դϴ�.</param>
		/// <param name="vSenderAddress">Ʈ�� �߽� ��� IP�ּ� �Դϴ�.</param>
		public TMSSnmpTrapPacket(byte [] vPacket, DateTime vTime, string vSenderAddress)
		{
			m_Packet = vPacket;
			m_Time = vTime;
			m_SenderAddress = vSenderAddress;
		}

		/// <summary>
		/// Ʈ�� ��Ŷ �����͸� �����ɴϴ�.
		/// </summary>
		public byte [] Packet
		{
			get { return m_Packet; }
		}

		/// <summary>
		/// Ʈ�� �߻��ð��� �����ɴϴ�.
		/// </summary>
		public DateTime Time
		{
			get { return m_Time; }
		}

		/// <summary>
		/// Ʈ�� �߽� ����� IP�ּҸ� �����ɴϴ�.
		/// </summary>
		public string SenderAddress
		{
			get { return m_SenderAddress; }
		}
	}
	#endregion //Ʈ�� ��Ŷ Ŭ���� �Դϴ� --------------------------------------------------------

	#region Snmp Trap �̺�Ʈ ��Ը�Ʈ Ŭ���� �Դϴ� ----------------------------------------
	/// <summary>
	/// Snmp Version 1 Ʈ�� �̺�Ʈ ��Ը�Ʈ Ŭ���� �Դϴ�. 
	/// </summary>
	public class SnmpV1TrapEventArgs : EventArgs
	{
		/// <summary>
		/// Ʈ�� ������ ����� PDU��ü �Դϴ�.
		/// </summary>
		private TMSSnmp_TrapPDU m_TrapPDU = null;
		/// <summary>
		/// Ʈ�� �߻� �ð��Դϴ�.
		/// </summary>
		private DateTime m_Time = DateTime.Now;

		/// <summary>
		/// Snmp Version 1 Ʈ�� �̺�Ʈ ��Ը�Ʈ Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vPDU">Snmp PDU �������̽� ��ü�Դϴ�.</param>
		public SnmpV1TrapEventArgs(ITMSSnmpPDU vPDU)
		{
			m_TrapPDU = (TMSSnmp_TrapPDU)vPDU;
		}

		/// <summary>
		/// Snmp Version 1 Ʈ�� �̺�Ʈ ��Ը�Ʈ Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vPDU">Snmp PDU �������̽� ��ü�Դϴ�.</param>
		/// <param name="vTime">Ʈ�� �߻� �ð��Դϴ�.</param>
		public SnmpV1TrapEventArgs(ITMSSnmpPDU vPDU, DateTime vTime)
		{
			m_Time = vTime;
			m_TrapPDU = (TMSSnmp_TrapPDU)vPDU;
		}

		/// <summary>
		/// Snmp Version 1 Ʈ�� PDU��ü�� �����ɴϴ�.
		/// </summary>
		public TMSSnmp_TrapPDU TrapPDU
		{
			get { return m_TrapPDU; }
		}

		/// <summary>
		/// Ʈ�� �߻� �ð��� �����ɴϴ�.
		/// </summary>
		public DateTime Time
		{
			get { return m_Time; }
		}
	}
	/// <summary>
	/// Snmp Version 1 Ʈ�������� �˸� �̺�Ʈ �ڵ鷯 �Դϴ�.
	/// </summary>
	public delegate void SnmpV1TrapReceivedEventHandler(object sender, SnmpV1TrapEventArgs e);

	/// <summary>
	/// Snmp Version 2 Ʈ�� �̺�Ʈ ��Ը�Ʈ Ŭ���� �Դϴ�. 
	/// </summary>
	public class SnmpV2TrapEventArgs : EventArgs
	{
		/// <summary>
		/// Ʈ�� ������ ����� PDU��ü �Դϴ�.
		/// </summary>
		private TMSSnmpV2_TrapPDU m_TrapPDU = null;
		/// <summary>
		/// Ʈ�� �߻� �ð��Դϴ�.
		/// </summary>
		private DateTime m_Time = DateTime.Now;
		/// <summary>
		/// Ʈ�� �߽� ��� IP�ּ� �Դϴ�.
		/// </summary>
		private string m_SenderAddress = "";

		/// <summary>
		/// Snmp Version 2 Ʈ�� �̺�Ʈ ��Ը�Ʈ Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vPDU">Snmp PDU �������̽� ��ü�Դϴ�.</param>
		/// <param name="vSenderAddress">Ʈ�� �߽� ��� IP�ּ� �Դϴ�.</param>
		public SnmpV2TrapEventArgs(ITMSSnmpPDU vPDU, string vSenderAddress)
		{
			m_TrapPDU = (TMSSnmpV2_TrapPDU)vPDU;
			m_SenderAddress = vSenderAddress;
		}

		/// <summary>
		/// Snmp Version 2 Ʈ�� �̺�Ʈ ��Ը�Ʈ Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vPDU">Snmp PDU �������̽� ��ü�Դϴ�.</param>
		/// <param name="vTime">Ʈ�� �߻� �ð��Դϴ�.</param>
		/// <param name="vSenderAddress">Ʈ�� �߽� ��� IP�ּ� �Դϴ�.</param>
		public SnmpV2TrapEventArgs(ITMSSnmpPDU vPDU, DateTime vTime, string vSenderAddress)
		{
			m_TrapPDU = (TMSSnmpV2_TrapPDU)vPDU;
			m_SenderAddress = vSenderAddress;
			m_Time = vTime;
		}

		/// <summary>
		/// Snmp Version 2 Ʈ�� PDU��ü�� �����ɴϴ�.
		/// </summary>
		public TMSSnmpV2_TrapPDU TrapPDU
		{
			get { return m_TrapPDU; }
		}

		/// <summary>
		/// Ʈ�� �߻� �ð��� �����ɴϴ�.
		/// </summary>
		public DateTime Time
		{
			get { return m_Time; }
		}

		/// <summary>
		/// Ʈ�� �߽� ����� IP�ּҸ� �����ɴϴ�.
		/// </summary>
		public string SenderAddress
		{
			get { return m_SenderAddress; }
		}
	}
	/// <summary>
	/// Snmp Version 2 Ʈ�������� �˸� �̺�Ʈ �ڵ鷯 �Դϴ�.
	/// </summary>
	public delegate void SnmpV2TrapReceivedEventHandler(object sender, SnmpV2TrapEventArgs e);
	#endregion //Snmp Trap �̺�Ʈ ��Ը�Ʈ Ŭ���� �Դϴ� ----------------------------------------
}
