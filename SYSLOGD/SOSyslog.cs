using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Collections;

namespace SYSLOGD
{
	/// <summary>
	/// TMSSyslog�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SOSyslog : System.ComponentModel.Component
	{
		#region ����, �̺�Ʈ, ������ ó���κ� �Դϴ� ---------------------------------------
		/// <summary> 
		/// �ʼ� �����̳� �����Դϴ�.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// Syslog ������ �˸��� �̺�Ʈ �Դϴ�.
		/// </summary>
		public event SyslogReceivedEventHandler SyslogReceived = null;
		/// <summary>
		/// Syslog�ޱ� ���� �Դϴ�.
		/// </summary>
		private TMSUdpSocket m_Socket = null;
		/// <summary>
		/// ���� IP�ּ� �Դϴ�.
		/// </summary>
		private string m_LocalIPAddress = "";
		/// <summary>
		/// Syslog ������ ��Ʈ��ȣ �Դϴ�.
		/// </summary>
		private int m_PortNumber = 514;
		/// <summary>
		/// Syslog �������� �۵��������� �����Դϴ�.
		/// </summary>
		private bool m_isRun = false;
		/// <summary>
		/// Syslog������ ���� ���� �Դϴ�.
		/// </summary>
		private bool m_isShutDown = false;
		/// <summary>
		/// �����尡 �۵��������� ���� �Դϴ�.
		/// </summary>
		private bool m_isThreadRun = false;
		/// <summary>
		/// Syslog ó���� ���� ����� ���� �̺�Ʈ �Դϴ�.
		/// </summary>
		private ManualResetEvent m_SyslogMRE = null;
        /// <summary>
        /// Syslog�� ó�� �� ������ Ǯ�Դϴ�.
        /// </summary>
        private SECUONELibrary.SOThreadPool m_SyslogWorkThreadPool;

		/// <summary>
		/// Syslog ��Ŷ�� �ӽ÷� ������ ť �Դϴ�.
		/// </summary>
		private Queue m_SyslogQueue = null;
		/// <summary>
        /// Syslog ť�� �ִ� �����Դϴ�.
		/// </summary>
		private long m_MaxQueueCount = 65535;
		/// <summary>
        /// Syslog ó������ ����� ���� ���� �̺�Ʈ �Դϴ�.
		/// </summary>
		private ManualResetEvent m_SyslogProcessMRE = null;
		/// <summary>
        /// Syslog ó�� ������ �Դϴ�.
		/// </summary>
		private Thread m_TrapProcessThread = null;

        /// <summary>
        /// �α������� ���� ��ġ �Դϴ�.
        /// </summary>
        private string m_LogPath = "";
        /// <summary>
        /// Log�׸� ������ ���Ϸα� ��ü �Դϴ�.
        /// </summary>
        private SOFileLog m_Log = null;

        /// <summary>
        /// Syslog������ ���۽�Ű�� �����ϱ� ���� �������Դϴ�. 
        /// </summary>
        private Thread tThread = null;

		/// <summary>
		/// Syslog��Ʈ���� ������ �Դϴ�.
		/// </summary>
		/// <param name="container">��Ʈ���� �߰��� Container��ü �Դϴ�.</param>
		public SOSyslog(System.ComponentModel.IContainer container)
		{
			container.Add(this);
			InitializeComponent();
		}

		/// <summary>
		/// Syslog��Ʈ���� �⺻ ������ �Դϴ�.
		/// </summary>
		public SOSyslog()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Syslog��Ʈ���� ������ �Դϴ�.
		/// </summary>
		/// <param name="vMaxQueueCount">�ִ� Syslog Queue���� �Դϴ�.</param>
		public SOSyslog(long vMaxQueueCount)
		{
			m_MaxQueueCount = vMaxQueueCount;
			InitializeComponent();
		}

		/// <summary> 
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			//Syslog ������ �����մϴ�.
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
		/// Syslog������ ���� IP�ּҸ� �������ų� �����մϴ�.
		/// </summary>
		public string LocalIPAddress
		{
			get { return m_LocalIPAddress; }
			set { m_LocalIPAddress = value; }
		}

		/// <summary>
		/// Syslog������ ��Ʈ��ȣ�� �����ɴϴ�.
		/// </summary>
		public int PortNumber
		{
			get { return m_PortNumber; }
			set { m_PortNumber = value; }
		}

		/// <summary>
		/// Syslog������ �۵����θ� �������ų� �����մϴ�.
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
		/// Syslog������ �����մϴ�.
		/// </summary>
		public void StartSyslogServer(int vPort )
		{
            //------------------------------------------------------------------------------
            //���� �ΰŸ� �ʱ�ȭ �մϴ�.
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
                //�۾��� ó���� ������ Ǯ�� �����մϴ�.
                //m_SyslogWorkThreadPool = new SECUONELibrary.SOThreadPool(10);
                //m_SyslogWorkThreadPool.StartThreadPool();
			}
		}

		/// <summary>
		/// Syslog ������ �����մϴ�.
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
                //�����带 ���� �մϴ�.
                if (tThread.IsAlive)
                {
                    try
                    {
                        //Syslog ó�� �����带 ���� ���� �մϴ�.
                        tThread.Abort();
                    }
                    catch (Exception ex)
                    {
                        ex = ex;
                    }
                }
                m_TrapProcessThread.Join(10);

                //m_Log.PrintLogEnter("tmsSyslogDemon - StopSyslogDemon : Step 2");
                //�����带 ���� �մϴ�.
                if (m_TrapProcessThread.IsAlive)
                {
                    try
                    {
                        //Syslog ó�� �����带 ���� ���� �մϴ�.
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
		#endregion //��Ʈ�� ���� �޼ҵ� �κ� �Դϴ� ---------------------------------------------

		#region ��Ʈ�� ���� �޼ҵ� �κ� �Դϴ� ---------------------------------------------
		/// <summary>
		/// Syslog������ ���۽�Ű�� �����մϴ�.
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
				m_SyslogMRE.WaitOne();	//������ �����ɶ����� ����մϴ�.
			}
			m_isThreadRun = false;
		}		

		//�����͹����� ó���մϴ�.
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
				Console.WriteLine("���� ������ ó�� ����: " + ex.ToString());
				m_Socket.Dispose();
				m_Socket = null;
				m_isShutDown = true;
				m_SyslogMRE.Set();
			}
		}

		/// <summary>
		/// Queue�� �׿��� Syslog��Ŷ�� ó���մϴ�.
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
		#endregion //��Ʈ�� ���� �޼ҵ� �κ� �Դϴ� ---------------------------------------------
	}

	#region Syslog ��Ŷ �ӽ� ���� Ŭ���� �Դϴ� --------------------------------------------
	/// <summary>
	/// Syslog ��Ŷ�� �����ϴ� Ŭ���� �Դϴ�.
	/// </summary>
	public class TMSSyslogPacket
	{
		/// <summary>
		/// Syslog ��Ŷ ������ �Դϴ�.
		/// </summary>
		private byte [] m_Packet = null;
		/// <summary>
		/// Syslog�� ���� �ð��Դϴ�.
		/// </summary>
		private DateTime m_Time = DateTime.Now;
		/// <summary>
		/// Syslog �߽� ��� IP�ּ� �Դϴ�.
		/// </summary>
		private string m_SenderAddress = "";

		/// <summary>
		/// Syslog ��Ŷ Ŭ������ ������ �Դϴ�;
		/// </summary>
		/// <param name="vPacket">Syslog ��Ŷ ������ �Դϴ�.</param>
		/// <param name="vSenderAddress">Syslog �߽� ��� IP�ּ� �Դϴ�.</param>
		public TMSSyslogPacket(byte [] vPacket, string vSenderAddress)
		{
			m_Packet = vPacket;
			m_SenderAddress = vSenderAddress;
		}

		/// <summary>
		/// Syslog ��Ŷ Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vPacket">Syslog ��Ŷ ������ �Դϴ�.</param>
		/// <param name="vTime">Syslog�� �߻��� �ð��Դϴ�.</param>
		/// <param name="vSenderAddress">Syslog �߽� ��� IP�ּ� �Դϴ�.</param>
		public TMSSyslogPacket(byte [] vPacket, DateTime vTime, string vSenderAddress)
		{
			m_Packet = vPacket;
			m_Time = vTime;
			m_SenderAddress = vSenderAddress;
		}

		/// <summary>
		/// Syslog ��Ŷ �����͸� �����ɴϴ�.
		/// </summary>
		public byte [] Packet
		{
			get { return m_Packet; }
		}

		/// <summary>
		/// Syslog �߻��ð��� �����ɴϴ�.
		/// </summary>
		public DateTime Time
		{
			get { return m_Time; }
		}

		/// <summary>
		/// Syslog �߽� ����� IP�ּҸ� �����ɴϴ�.
		/// </summary>
		public string SenderAddress
		{
			get { return m_SenderAddress; }
		}
	}
	#endregion //Syslog ��Ŷ �ӽ� ���� Ŭ���� �Դϴ� --------------------------------------------

	#region Syslog �̺�Ʈ ��Ը�Ʈ Ŭ���� �Դϴ� ----------------------------------------
	/// <summary>
	/// Syslog �̺�Ʈ ��Ը�Ʈ Ŭ���� �Դϴ�. 
	/// </summary>
	public class SyslogEventArgs : EventArgs
	{
		/// <summary>
		/// Syslog ������ ����� TMSSyslogMessage��ü �Դϴ�.
		/// </summary>
		private TMSSyslogMessage m_SyslogMessage = null;
		/// <summary>
		/// Syslog �߻� �ð��Դϴ�.
		/// </summary>
		private DateTime m_Time = DateTime.Now;
		/// <summary>
		/// Syslog �߽� ��� IP�ּ� �Դϴ�.
		/// </summary>
		private string m_SenderAddress = "";

		/// <summary>
		/// Syslog �̺�Ʈ ��Ը�Ʈ Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vSyslog">Syslog Message ��ü�Դϴ�.</param>
		/// <param name="vSenderAddress">Syslog �߽� ��� IP�ּ� �Դϴ�.</param>
		public SyslogEventArgs(TMSSyslogMessage vSyslogMessage, string vSenderAddress)
		{
			m_SyslogMessage = vSyslogMessage;
			m_SenderAddress = vSenderAddress;
		}

		/// <summary>
		/// Syslog �̺�Ʈ ��Ը�Ʈ Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vSyslog">Syslog Message ��ü�Դϴ�.</param>
		/// <param name="vTime">Ʈ�� �߻� �ð��Դϴ�.</param>
		/// <param name="vSenderAddress">Ʈ�� �߽� ��� IP�ּ� �Դϴ�.</param>
		public SyslogEventArgs(TMSSyslogMessage vSyslogMessage, DateTime vTime, string vSenderAddress)
		{
			m_SyslogMessage = vSyslogMessage;
			m_SenderAddress = vSenderAddress;
			m_Time = vTime;
		}

		/// <summary>
		/// Syslog Message��ü�� �����ɴϴ�.
		/// </summary>
		public TMSSyslogMessage SyslogMessage
		{
			get { return m_SyslogMessage; }
		}

		/// <summary>
		/// Syslog �߻� �ð��� �����ɴϴ�.
		/// </summary>
		public DateTime Time
		{
			get { return m_Time; }
		}

		/// <summary>
		/// Syslog �߽� ����� IP�ּҸ� �����ɴϴ�.
		/// </summary>
		public string SenderAddress
		{
			get { return m_SenderAddress; }
		}
	}
	/// <summary>
	/// Syslog������ �˸� �̺�Ʈ �ڵ鷯 �Դϴ�.
	/// </summary>
	public delegate void SyslogReceivedEventHandler(object sender, SyslogEventArgs e);	
	#endregion //Syslog �̺�Ʈ ��Ը�Ʈ Ŭ���� �Դϴ� ----------------------------------------
}
