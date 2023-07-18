using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Threading;

namespace SYSLOGD
{
	#region ������ ���� --------------------------------------------------------------------
	/// <summary>
	/// ������ �����͸� Encoding�� Ÿ�� ������ �Դϴ�.
	/// </summary>
	public enum E_EncodingType
	{
		/// <summary>
		/// �����͸� ASCII�ڵ�� Encoding�մϴ�.
		/// </summary>
		ASCII,
		/// <summary>
		/// �����͸� Unicode�� Encoding�մϴ�.
		/// </summary>
		Unicode
	}

	/// <summary>
	/// ������ ���� ������ �Դϴ�.
	/// </summary>
	public enum E_SocketStatus
	{
		/// <summary>
		/// �������Դϴ�.
		/// </summary>
		Connecting,
		/// <summary>
		/// ����Ǿ��ֽ��ϴ�.
		/// </summary>
		Connected,
		/// <summary>
		/// ����Ǿ����� �ʽ��ϴ�.
		/// </summary>
		DisConnected
	}

	/// <summary>
	/// ���� ���� ������ �Դϴ�.
	/// </summary>
	public enum E_SocketError
	{
		/// <summary>
		/// ������ �����ϴ�.
		/// </summary>
		NoError,
		/// <summary>
		/// ������ ���� �Ǿ����ϴ�.
		/// </summary>
		Disconnected,
		/// <summary>
		/// ������ Ÿ�Ӿƿ��� �߻��Ͽ����ϴ�.
		/// </summary>
		ConnectTimeout,
		/// <summary>
		/// ��� �߿� Ÿ�Ӿƿ��� �߻��Ͽ����ϴ�.
		/// </summary>
		Timeout,
		/// <summary>
		/// ���� ȣ��Ʈ�� ���� ������ ����Ǿ����ϴ�.
		/// </summary>
		ConnectionResetByPeer,
		/// <summary>
		/// �˼� ���� �����Դϴ�.
		/// </summary>
		UnknownError
	}

    /// <summary>
    /// Ÿ�Ӿƿ� �������Դϴ�.
    /// </summary>
    internal enum E_TimeoutType
    {
        None,
        Connect,
        Receive,
        Send
    }
	#endregion //������ ���� --------------------------------------------------------------------

	#region ���� ������ �� Ÿ�Ӿƿ� ���� Ŭ���� -------------------------------------------------------------
	/// <summary>
	/// ������Ž� ����� ������ ��ü �Դϴ�.
	/// </summary>
	public class TMSSocketData
	{
		/// <summary>
		/// �۾� ���� �Դϴ�.
		/// </summary>
		private Socket m_WorkSocket;
		/// <summary>
		/// ���� ���� ũ�� �Դϴ�.
		/// </summary>
		private int m_BufferSize;
		/// <summary>
		/// ���� ���� �迭 �Դϴ�.
		/// </summary>
		private byte [] m_Buffers;

		/// <summary>
		/// ���� �����Ͱ�ü �⺻ ������ �Դϴ�.
		/// </summary>
		public TMSSocketData()
		{
			m_WorkSocket = null;
			m_BufferSize = 4096;
			Variable_Init();
		}			

		/// <summary>
		/// ���� ������ ��ü ������ �Դϴ�.
		/// </summary>
		/// <param name="vWorkSocket">�۾� ���� �Դϴ�.</param>
		public TMSSocketData(Socket vWorkSocket)
		{
			m_WorkSocket = vWorkSocket;
            m_BufferSize = 4096;
			Variable_Init();
		}

		/// <summary>
		/// ���� ������ ��ü ������ �Դϴ�.
		/// </summary>
		/// <param name="vWorkSocket">�۾� ���� �Դϴ�.</param>
		/// <param name="vBufferSize">���� ���� ũ�� �Դϴ�.</param>
		public TMSSocketData(Socket vWorkSocket, int vBufferSize)
		{
			m_WorkSocket = vWorkSocket;
			m_BufferSize = vBufferSize;
			Variable_Init();
		}

		/// <summary>
		/// ���� ���۸� �ʱ�ȭ �մϴ�.
		/// </summary>
		private void Variable_Init()
		{
			m_Buffers = new byte [m_BufferSize];
		}

		/// <summary>
		/// �۾� ������ �������ų� �����մϴ�.
		/// </summary>
		public Socket WorkSocket
		{
			get { return m_WorkSocket; }
			set { m_WorkSocket = value; }
		}

		/// <summary>
		/// ���� ������ ũ�⸦ �������ų� �����մϴ�.
		/// </summary>
		public int BufferSize
		{
			get { return m_BufferSize; }
			set 
			{
				if(m_BufferSize != value)
				{	
					m_BufferSize = value;
					m_Buffers = null;
					m_Buffers = new byte[value];						
				}
			}
		}

		/// <summary>
		/// ���� ���۸� �����ɴϴ�.
		/// </summary>
		public byte [] Buffers
		{
			get { return m_Buffers; }
		}
	}



    /// <summary>
    /// Ÿ�� �ƿ� ���� Ŭ���� �Դϴ�.
    /// </summary>
    internal class TimeoutInfo
    {
        /// <summary>
        /// Ÿ�� �ƿ� Ÿ�� �Դϴ�.
        /// </summary>
        private E_TimeoutType m_TimeoutType = E_TimeoutType.None;
        /// <summary>
        /// Ÿ�Ӿƿ� �ð��Դϴ�(���� ��)
        /// </summary>
        private int m_Timeout = 0;

        /// <summary>
        /// Ÿ�� �ƿ� ������ �⺻ ������ �Դϴ�.
        /// </summary>
        public TimeoutInfo() { }

        /// <summary>
        /// Ÿ�� �ƿ� ������ �⺻ ������ �Դϴ�.
        /// </summary>
        /// <param name="vTimeoutType">Ÿ�� �ƿ� Ÿ�� �Դϴ�.</param>
        /// <param name="vTimeout">Ÿ�Ӿƿ� �ð��Դϴ�(���� ��)</param>
        public TimeoutInfo(E_TimeoutType vTimeoutType, int vTimeout)
        {
            m_TimeoutType = vTimeoutType;
            m_Timeout = vTimeout;
        }

        /// <summary>
        /// Ÿ�� �ƿ� Ÿ���� �������ų� �����մϴ�.
        /// </summary>
        public E_TimeoutType TimeoutType
        {
            get { return m_TimeoutType; }
            set { m_TimeoutType = value; }
        }

        /// <summary>
        /// Ÿ�� �ƿ� �ð��� �������ų� �����մϴ�.
        /// </summary>
        public int Timeout
        {
            get { return m_Timeout; }
            set { m_Timeout = value; }
        }
    }
	#endregion //���� ������ �� Ÿ�Ӿƿ� ���� Ŭ���� -------------------------------------------------------------

	#region ���� ��� ���� Ŭ���� �Դϴ�. --------------------------------------------------
	/// <summary>
	/// ���� ��� �۾��� ������ ����Ǵ� Ŭ���� �Դϴ�.
	/// </summary>
	public class TMSSocketError
	{
		/// <summary>
		/// ���� �ڵ� �Դϴ�.
		/// </summary>
		internal E_SocketError m_Error = E_SocketError.NoError;
		/// <summary>
		/// ���� �޽��� �Դϴ�.
		/// </summary>
		internal string m_ErrorMessage = "";
		/// <summary>
		/// ���� ���� Ŭ������ �⺻ ������ �Դϴ�.
		/// </summary>
		public TMSSocketError() {}
		/// <summary>
		/// ���� ���� Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vError">���� �ڵ� �Դϴ�.</param>
		/// <param name="vErrorMessage">���� �޽��� �Դϴ�.</param>
		public TMSSocketError(E_SocketError vError, string vErrorMessage)
		{
			m_Error = vError;
			m_ErrorMessage = vErrorMessage;
		}

		/// <summary>
		/// ���� ��� ���� �ڵ带 ���� �ɴϴ�.
		/// </summary>
		public E_SocketError Error
		{
			get { return m_Error; }
		}

		/// <summary>
		/// ���� ��� ���� �޽����� �����ɴϴ�.
		/// </summary>
		public string ErrorMessage
		{
			get { return m_ErrorMessage; }
		}
	}



	#endregion //���� ��� ���� Ŭ���� �Դϴ�. --------------------------------------------------

	#region �̺�Ʈ ��Ը�Ʈ Ŭ���� ---------------------------------------------------------
	/// <summary>
	/// ���� Accepted �̺�Ʈ ��Ը�Ʈ Ŭ���� �Դϴ�.
	/// </summary>
	public class TMSSocketAcceptedEventArgs : EventArgs
	{
		/// <summary>
		/// ���� ��ü �Դϴ�.
		/// </summary>
        private TMSUdpSocket m_Socket;

		/// <summary>
		/// ���� Accepted �̺�Ʈ ��Ը�Ʈ ������ �Դϴ�.
		/// </summary>
		/// <param name="vSocket">���� ��ü �Դϴ�.</param>
		public TMSSocketAcceptedEventArgs(Socket vSocket)
		{
            m_Socket = new TMSUdpSocket(vSocket);
            //vSocket.Available = 3000;
		}

		/// <summary>
		/// ������ �����ɴϴ�.
		/// </summary>
        public TMSUdpSocket Socket
		{
			get { return m_Socket; }
		}
	}
	/// <summary>
	/// ���� Accept�̺�Ʈ �ڵ鷯 �Դϴ�.
	/// </summary>
	public delegate void TMSSocketAcceptedEventHandler(object sender, TMSSocketAcceptedEventArgs e);

	/// <summary>
	/// ���� ������ ���� �̺�Ʈ ��Ը�Ʈ �Դϴ�.
	/// </summary>
	public class TMSDataReceivedEventArgs : EventArgs
	{
		/// <summary>
		/// ���� ������ �Դϴ�.
		/// </summary>
		private TMSSocketData m_SocketData;
		/// <summary>
		/// �������� ���� ������ Byte���� �Դϴ�.
		/// </summary>
		private int m_ReadCount;
		/// <summary>
		/// �����͸� ������ IP�ּ� �Դϴ�.
		/// </summary>
		private string m_SenderIPAddress = "";

		/// <summary>
		/// ���� ������ ���� �̺�Ʈ ��Ը�Ʈ ������ �Դϴ�.
		/// </summary>
		/// <param name="vSocketData">���� ������ �Դϴ�.</param>
		/// <param name="vReadCount">���� �������� Byte���� �Դϴ�.</param>
		public TMSDataReceivedEventArgs(TMSSocketData vSocketData, int vReadCount)
		{
			m_SocketData = vSocketData;
			m_ReadCount = vReadCount;
		}

		/// <summary>
		/// ���� ������ ���� �̺�Ʈ ��Ը�Ʈ ������ �Դϴ�.
		/// </summary>
		/// <param name="vSenderIPAddress">�����͸� ������ IP�ּ� �Դϴ�.</param>
		/// <param name="vSocketData">���� ������ �Դϴ�.</param>
		/// <param name="vReadCount">���� �������� Byte���� �Դϴ�.</param>
		public TMSDataReceivedEventArgs(string vSenderIPAddress, TMSSocketData vSocketData, int vReadCount)
		{
			m_SocketData = vSocketData;
			m_ReadCount = vReadCount;
			m_SenderIPAddress = vSenderIPAddress;
		}

		/// <summary>
		/// ���� ������ ��ü�� ���� �ɴϴ�.
		/// </summary>
		public TMSSocketData SocketData
		{
			get { return m_SocketData; }
		}

		/// <summary>
		/// ���� ������ ������ �����ɴϴ�.
		/// </summary>
		public int ReadCount
		{
			get { return m_ReadCount; }
		}

		/// <summary>
		/// �����͸� ������ IP�ּҸ� �����ɴϴ�.
		/// </summary>
		public string SenderIPAddress
		{
			get { return m_SenderIPAddress; }
		}
	}
	/// <summary>
	/// ���� �����͹��� �̺�Ʈ �ڵ鷯 �Դϴ�.
	/// </summary>
	public delegate void TMSDataReceivedEventHandler(object sender, TMSDataReceivedEventArgs e);

    /// <summary>
    /// ���� ���� �̺�Ʈ ��Ը�Ʈ �Դϴ�.
    /// </summary>
    public class TMSSocketErrorEventArgs : EventArgs
    {
        /// <summary>
        /// ���� ���� �������Դϴ�.
        /// </summary>
        private E_SocketError m_SocketError = E_SocketError.NoError;
        /// <summary>
        /// ���� �޽��� �Դϴ�.
        /// </summary>
        private string m_Message = "";

        /// <summary>
        /// ���� ���� �̺�Ʈ ��Ը�Ʈ ������ �Դϴ�.
        /// </summary>
        /// <param name="vSocketError">���� ���� �Դϴ�.</param>
        public TMSSocketErrorEventArgs(E_SocketError vSocketError)
        {
            m_SocketError = vSocketError;
        }

        /// <summary>
        /// ���� ���� �̺�Ʈ ��Ը�Ʈ ������ �Դϴ�.
        /// </summary>
        /// <param name="vSocketError">���� ���� �Դϴ�.</param>
        /// <param name="vMessage">���� ���� �޽��� �Դϴ�.</param>
        public TMSSocketErrorEventArgs(E_SocketError vSocketError, string vMessage)
        {
            m_SocketError = vSocketError;
            m_Message = vMessage;
        }

        /// <summary>
        /// ���� ���� �������� �����ɴϴ�.
        /// </summary>
        public E_SocketError SocketError
        {
            get { return m_SocketError; }
        }

        /// <summary>
        /// ���� ���� �޽����� �����ɴϴ�.
        /// </summary>
        public string Message
        {
            get { return m_Message; }
        }
    }

    /// <summary>
    /// ���� ���� �̺�Ʈ �ڵ鷯 �Դϴ�.
    /// </summary>
    public delegate void TMSSocketErrorEventHandler(object sender, TMSSocketErrorEventArgs e);

	#endregion //�̺�Ʈ ��Ը�Ʈ Ŭ���� ---------------------------------------------------------

	#region TMSUdpSocket �Դϴ� -------------------------------------------------------------
    /// <summary>
	/// ��Ʈ��ũ�� ���Ͽ� ������ ������ ó���� �񵿱� ���� Ŭ���� �Դϴ�.
	/// </summary>
	public class TMSUdpSocket : IDisposable
	{
		#region ���� ����� ������ ---------------------------------------------------------
		/// <summary>
		/// �ͱ���� �����Դϴ�.
		/// </summary>
		private Socket m_Listener;
		/// <summary>
		/// �۾� ���� ��ü �Դϴ�.
		/// </summary>
		private Socket m_Socket;
		/// <summary>
		/// Ÿ�� �ƿ� �ð� �Դϴ�.(���� : ��)
		/// </summary>
		private int	m_Timeout = 15;
		/// <summary>
		/// ��ü�� �Ҹ��������� ���� �Դϴ�.
		/// </summary>
		private bool m_Disposing = false;
		/// <summary>
		/// Ÿ�Ӿƿ� üũ ������ �Դϴ�.
		/// </summary>
		private Thread m_TimeoutThread = null;
		/// <summary>
		/// Ÿ�Ӿƿ� �ð� ��� �Դϴ�.
		/// </summary>
		private ArrayList m_TimeoutList = new ArrayList();
		
		private TimeoutInfo m_ConnectTimeout = new TimeoutInfo(E_TimeoutType.Connect, 15);
		private TimeoutInfo m_ReceiveTimeout = new TimeoutInfo(E_TimeoutType.Receive, 15);
		private TimeoutInfo m_SendTimeout = new TimeoutInfo(E_TimeoutType.Send, 15);

		/// <summary>
		/// ���� �޾Ƶ��� �̺�Ʈ �Դϴ�.
		/// </summary>
		public event TMSSocketAcceptedEventHandler SocketAccepted;
		/// <summary>
		/// ���� ����� �̺�Ʈ �Դϴ�.
		/// </summary>
		public event EventHandler SocketConnected;
		/// <summary>
		/// ������ ���� �̺�Ʈ �Դϴ�.
		/// </summary>
		public event TMSDataReceivedEventHandler DataReceived;
		/// <summary>
		/// ������ ���۵� �̺�Ʈ �Դϴ�.
		/// </summary>
		public event EventHandler DataSended;
		/// <summary>
		/// ���� ���� �̺�Ʈ �Դϴ�.
		/// </summary>
		public event TMSSocketErrorEventHandler SocketError;

		/// <summary>
		/// Network Socket Ŭ������ �⺻ ������ �Դϴ�.
		/// </summary>
		public TMSUdpSocket()
		{
			m_TimeoutThread = new Thread(new ThreadStart(CheckTimeout));
			m_TimeoutThread.Start();
		}
	
		/// <summary>
		/// Network Socket Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vSocket"> Windows Socket ��ü �Դϴ�.</param>
		public TMSUdpSocket(Socket vSocket)
		{			
			m_Socket = vSocket;
			m_TimeoutThread = new Thread(new ThreadStart(CheckTimeout));
			m_TimeoutThread.Start();
		}		

		/// <summary>
		/// Ŭ������ �Ҹ� ��ŵ�ϴ�.
		/// </summary>
		public void Dispose()
		{
			m_Disposing = true;

			try
			{
				lock(m_TimeoutList.SyncRoot)
				{
					m_TimeoutList.Clear();
				}

				if(m_TimeoutThread != null)
				{
					m_TimeoutThread.Join(100);
					if(m_TimeoutThread.IsAlive)
					{
						try
						{
							m_TimeoutThread.Abort();
						}
						catch {}
					}
				}

				//��� ������ �ݴ´�
				if(m_Socket != m_Listener) 
				{	
					if(m_Socket != null)
					{
						m_Socket.Shutdown(SocketShutdown.Both);
						m_Socket.Close();
						m_Socket = null;
					}
				}

				//���� ������ �ݴ´�
				if(m_Listener != null) 
				{	
					m_Listener.Shutdown(SocketShutdown.Both);
					m_Listener.Close();
					m_Listener = null;
				}
			}
			catch(Exception ex)
			{
				ex=ex;
			}
		}


		#endregion //���� ����� ������ ---------------------------------------------------------		

        #region Ÿ�Ӿƿ� ���� �ż����Դϴ� -----------------------------------------------------
        /// <summary>
        /// Ÿ�� �ƿ��� Ȯ���մϴ�.
        /// </summary>
        private void CheckTimeout()
        {
            TimeoutInfo tTI = null;

            while (!m_Disposing)
            {
                lock (m_TimeoutList.SyncRoot)
                {
                    for (int i = m_TimeoutList.Count - 1; i > -1; i--)
                    {
                        tTI = (TimeoutInfo)m_TimeoutList[i];
                        tTI.Timeout--;
                        if (tTI.Timeout < 1)
                        {
                            m_TimeoutList.RemoveAt(i);

                            switch (tTI.TimeoutType)
                            {
                                case E_TimeoutType.Connect:
                                    if (SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.ConnectTimeout));
                                    break;
                                case E_TimeoutType.Send:
                                    if (SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.Timeout));
                                    break;
                                case E_TimeoutType.Receive:
                                    if (SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.Timeout));
                                    break;
                            }
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private void RemoveTimeout(E_TimeoutType vTimeoutType)
        {
            lock (m_TimeoutList.SyncRoot)
            {
                for (int i = m_TimeoutList.Count - 1; i > -1; i--)
                {
                    if (((TimeoutInfo)m_TimeoutList[i]).TimeoutType == vTimeoutType)
                    {
                        m_TimeoutList.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void AddTimeout(TimeoutInfo vTimeoutInfo)
        {
            bool tisExists = false;

            lock (m_TimeoutList.SyncRoot)
            {
                for (int i = 0; i < m_TimeoutList.Count; i++)
                {
                    if (((TimeoutInfo)m_TimeoutList[i]).TimeoutType == vTimeoutInfo.TimeoutType)
                    {
                        ((TimeoutInfo)m_TimeoutList[i]).Timeout = m_Timeout;
                        tisExists = true;
                        break;
                    }
                }

                if (!tisExists)
                {
                    vTimeoutInfo.Timeout = m_Timeout;
                    m_TimeoutList.Add(vTimeoutInfo);
                }
            }
        }
        #endregion //Ÿ�Ӿƿ� ���� �ż����Դϴ� -----------------------------------------------------

 		#region ���� �� ����� ó�� --------------------------------------------------------
		/// <summary>
		/// ���� ��⸦ �����մϴ�.
		/// </summary>
		/// <param name="vPortNumber">����� ��Ʈ��ȣ�Դϴ�.</param>
		public void StartListening(int vPortNumber)
		{		
			StartListen(IPAddress.Any, vPortNumber, 10);
		}

		/// <summary>
		/// ���� ��⸦ �����մϴ�.
		/// </summary>
		/// <param name="vPortNumber">����� ��Ʈ��ȣ�Դϴ�.</param>
		/// <param name="vMaxListenCount">�ִ� ���� ��� �����Դϴ�.</param>
		public void StartListening(int vPortNumber, int vMaxListenCount)
		{
			StartListen(IPAddress.Any, vPortNumber, vMaxListenCount);
		}		

		/// <summary>
		/// ���� ��⸦ �����մϴ�.
		/// </summary>
		/// <param name="vIPAddress">��⿡ ����� IP�ּ� �Դϴ�.</param>
		/// <param name="vPortNumber">����� ��Ʈ��ȣ�Դϴ�.</param>
		public void StartListening(string vIPAddress, int vPortNumber)
		{
			StartListen(vIPAddress, vPortNumber, 10);
		}

		/// <summary>
		/// ���� ��⸦ �����մϴ�.
		/// </summary>
		/// <param name="vIPAddress">��⿡ ����� IP�ּ� �Դϴ�.</param>
		/// <param name="vPortNumber">����� ��Ʈ��ȣ�Դϴ�.</param>
		/// <param name="vMaxListenCount">�ִ� ���� ��� �����Դϴ�.</param>
		public void StartListening(string vIPAddress, int vPortNumber, int vMaxListenCount)
		{
			StartListen(vIPAddress, vPortNumber, vMaxListenCount);
		}		

		/// <summary>
		/// ���� ��⸦ �����մϴ�.
		/// </summary>
		/// <param name="vIPAddress">��⿡ ����� IP�ּ� �Դϴ�.</param>
		/// <param name="vPortNumber">����� ��Ʈ��ȣ�Դϴ�.</param>
		/// <param name="vMaxListenCount">�ִ� ���� ��� �����Դϴ�.</param>
		private void StartListen(string vIPAddress, int vPortNumber, int vMaxListenCount)
		{
			IPAddress tIP = IPAddress.Parse(vIPAddress);
			StartListen(tIP, vPortNumber, vMaxListenCount);
		}

		/// <summary>
		/// ���� ��⸦ �����մϴ�.
		/// </summary>
		/// <param name="vIPAddress">��⿡ ����� IP�ּ� ��ü �Դϴ�.</param>
		/// <param name="vPortNumber">����� ��Ʈ��ȣ�Դϴ�.</param>
		/// <param name="vMaxListenCount">�ִ� ���� ��� �����Դϴ�.</param>
		private void StartListen(IPAddress vIPAddress, int vPortNumber, int vMaxListenCount)
		{
			try
			{				
				IPEndPoint tLocalEndPoint = new IPEndPoint(vIPAddress, vPortNumber);
				m_Listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);				

				m_Listener.Bind(tLocalEndPoint);
                //m_Listener.NoDelay = true;
                m_Socket = m_Listener;				
				StartReceive();
			}
			catch(SocketException sx)
			{
				if(sx.ErrorCode == 10048)
				{
					Console.WriteLine("�̹̻������ ��Ʈ��ȣ �Դϴ�");
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("�ͱ���� ���� sx : " + sx.ToString());
				}
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("�ͱ���� ���� ex : " + ex.ToString());
			}
		}

		/// <summary>
		/// ������ �޾Ƶ��Դϴ�.
		/// </summary>
		/// <param name="ar">�񵿱� ó�� ��� �Դϴ�.</param>
		private void AcceptCallback(IAsyncResult ar)
		{
			try
			{
				Socket tListener = (Socket)ar.AsyncState;
				Socket tConnectedSocket = tListener.EndAccept(ar);

				//���� �Ǿ����� �˸��� �̺�Ʈ
				if(SocketAccepted != null) SocketAccepted(this, new TMSSocketAcceptedEventArgs(tConnectedSocket));

				//Ŭ���̾�Ʈ�� �ٽ� �޾Ƶ��δ�.
				tListener.BeginAccept(new System.AsyncCallback(AcceptCallback), tListener);
			}
			catch(SocketException se)
			{
				System.Diagnostics.Debug.WriteLine("���� �޾Ƶ��� se : " + se.Message);
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("���� �޾Ƶ��� ex : " + ex.Message);
			}
		}
		#endregion //���� �� ����� ó�� --------------------------------------------------------

		#region ���� ���� ó�� -------------------------------------------------------------
		/// <summary>
		/// ���� ���Ͽ� ������ �õ��մϴ�.
		/// </summary>
		/// <param name="vRemoteIPAddress">������ ���������� IP�ּ� �Դϴ�.</param>
		/// <param name="vRemotePortNumber">������ ���������� ��Ʈ��ȣ �Դϴ�.</param>
		public void StartConnecting(string vRemoteIPAddress, int vRemotePortNumber)
		{
			StartConnect(vRemoteIPAddress, vRemotePortNumber, ProtocolType.Tcp, SocketType.Stream);
		}

		/// <summary>
		/// ���� ���Ͽ� ������ �õ��մϴ�.
		/// </summary>
		/// <param name="vLocalIPAddress">���� IP�ּ� �Դϴ�.</param>
		/// <param name="vLocalPortNumber">���� ��Ʈ��ȣ �Դϴ�.</param>
		/// <param name="vRemoteIPAddress">������ ���������� IP�ּ� �Դϴ�.</param>
		/// <param name="vRemotePortNumber">������ ���������� ��Ʈ��ȣ �Դϴ�.</param>
		public void StartConnecting(string vLocalIPAddress, int vLocalPortNumber, string vRemoteIPAddress, int vRemotePortNumber)
		{
			StartConnect(vRemoteIPAddress, vRemotePortNumber, ProtocolType.Tcp, SocketType.Stream);
		}

		/// <summary>
		/// ���� ���Ͽ� ������ �õ��մϴ�.
		/// </summary>
		/// <param name="vRemoteIPAddress">������ ���������� IP�ּ� �Դϴ�.</param>
		/// <param name="vRemotePortNumber">������ ���������� ��Ʈ��ȣ �Դϴ�.</param>
		/// <param name="vProtocolType">������ ���������� �������� Ÿ�� �Դϴ�.</param>
		public void StartConnecting(string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType)
		{
			StartConnect(vRemoteIPAddress, vRemotePortNumber, vProtocolType, SocketType.Stream);
		}

		/// <summary>
		/// ���� ���Ͽ� ������ �õ��մϴ�.
		/// </summary>
		/// <param name="vRemoteIPAddress">������ ���������� IP�ּ� �Դϴ�.</param>
		/// <param name="vRemotePortNumber">������ ���������� ��Ʈ��ȣ �Դϴ�.</param>
		/// <param name="vProtocolType">������ ���������� �������� Ÿ�� �Դϴ�.</param>
		/// <param name="vSocketType">������ ���������� ���� Ÿ�� �Դϴ�.</param>
		public void StartConnecting(string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType, SocketType vSocketType)
		{
			StartConnect(vRemoteIPAddress, vRemotePortNumber, vProtocolType, vSocketType);
		}

		/// <summary>
		/// ���� ���Ͽ� ������ �õ��մϴ�.
		/// </summary>
		/// <param name="vRemoteIPAddress">������ ���������� IP�ּ� �Դϴ�.</param>
		/// <param name="vRemotePortNumber">������ ���������� ��Ʈ��ȣ �Դϴ�.</param>
		/// <param name="vProtocolType">������ ���������� �������� Ÿ�� �Դϴ�.</param>
		/// <param name="vSocketType">������ ���������� ���� Ÿ�� �Դϴ�.</param>
		private void StartConnect(string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType, SocketType vSocketType)
		{
			try
			{
				IPAddress tRemoteIP = IPAddress.Parse(vRemoteIPAddress);
				IPEndPoint tRemoteEndPoint = new IPEndPoint(tRemoteIP, vRemotePortNumber);
				AddTimeout(m_ConnectTimeout);
				m_Listener = new Socket(AddressFamily.InterNetwork, vSocketType, vProtocolType);				
				m_Listener.BeginConnect(tRemoteEndPoint, new System.AsyncCallback(ConnectCallback), m_Listener);
			}
			catch(SocketException sx)
			{
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "���� ����õ� ���� : " + sx.Message + " " + sx.ErrorCode));
			}
			catch(Exception ex)
			{
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "���� ����õ� ���� : " + ex.Message));
			}
		}

		/// <summary>
		/// ���� ���Ͽ� ������ �õ��մϴ�.
		/// </summary>
		/// <param name="vLocalIPAddress">���� IP�ּ� �Դϴ�.</param>
		/// <param name="vLocalPortNumber">���� ��Ʈ��ȣ �Դϴ�.</param>
		/// <param name="vRemoteIPAddress">������ ���������� IP�ּ� �Դϴ�.</param>
		/// <param name="vRemotePortNumber">������ ���������� ��Ʈ��ȣ �Դϴ�.</param>
		/// <param name="vProtocolType">������ ���������� �������� Ÿ�� �Դϴ�.</param>
		/// <param name="vSocketType">������ ���������� ���� Ÿ�� �Դϴ�.</param>
		private void StartConnect(string vLocalIPAddress, int vLocalPortNumber, string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType, SocketType vSocketType)
		{
			try
			{
				IPAddress tRemoteIP = IPAddress.Parse(vRemoteIPAddress);
				IPEndPoint tRemoteEndPoint = new IPEndPoint(tRemoteIP, vRemotePortNumber);
				AddTimeout(m_ConnectTimeout);
				m_Listener = new Socket(AddressFamily.InterNetwork, vSocketType, vProtocolType);
				IPEndPoint tLocalEndPoint = new IPEndPoint(IPAddress.Parse(vLocalIPAddress), vLocalPortNumber);
				m_Listener.Bind(tLocalEndPoint);	
				m_Listener.BeginConnect(tRemoteEndPoint, new System.AsyncCallback(ConnectCallback), m_Listener);
			}
			catch(SocketException sx)
			{
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "���� ����õ� ���� : " + sx.Message + " " + sx.ErrorCode));
			}
			catch(Exception ex)
			{
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "���� ����õ� ���� : " + ex.Message));
			}
		}

		/// <summary>
		/// ���� ������ ó���մϴ�.
		/// </summary>
		/// <param name="ar">�񵿱� ó�� ��� �Դϴ�.</param>
		private void ConnectCallback(System.IAsyncResult ar)
		{
			try
			{	
				//m_ConnectTimeoutTimer.Enabled = false;
				RemoveTimeout(E_TimeoutType.Connect);

				Socket tClient = (Socket)ar.AsyncState;	
			
				//������ ����Ǿ����� Ȯ���մϴ�.
				if(tClient.Connected)
				{
					tClient.EndConnect(ar);

					m_Socket = tClient;
					m_Listener = null;
				
					if(SocketConnected != null) SocketConnected(this, EventArgs.Empty);
				}
				else
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.ConnectTimeout));
				}
			}
			catch(SocketException sx)
			{
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "���� ����ó�� ���� : " + sx.Message + " " + sx.ErrorCode));
			}
			catch(Exception ex)
			{
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "���� ����ó�� ���� : " + ex.Message));
			}
		}
		#endregion //���� ���� ó�� -------------------------------------------------------------

		#region ���� ������ �ޱ� ó�� ------------------------------------------------------
		/// <summary>
		/// ������ �ޱ⸦ ����մϴ�.
		/// </summary>
		public void StartReceive()
		{
			TMSSocketData tSocketData = new TMSSocketData(m_Socket);
			StartReceive(tSocketData);
		}

		/// <summary>
		/// �����͹ޱ⸦ ����մϴ�.
		/// </summary>
		/// <param name="vSocketData">���������� ����ִ� TMSSocketDataŬ���� �Դϴ�.</param>
		public void StartReceive(TMSSocketData vSocketData)
		{
			try
			{	
				IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
				EndPoint tempRemoteEP = (EndPoint)sender;

				m_Socket.BeginReceiveFrom(vSocketData.Buffers, 0, vSocketData.BufferSize, SocketFlags.None, ref tempRemoteEP, new AsyncCallback(ReceivedCallBack), vSocketData);
			}
			catch(SocketException sx)
			{
				if(sx.ErrorCode == 10054)
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
				}
				else
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ �ޱ� ���� ���� : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ �ޱ� ���� ���� : " + ex.Message));
			}
		}

		/// <summary>
		/// �����͹ޱ⸦ ó���մϴ�.
		/// </summary>
		/// <param name="ar">�����͸� ���� �񵿱� ��� ��ü �Դϴ�.</param>
		private void ReceivedCallBack(IAsyncResult ar)
		{		
			try
			{
				if(m_Disposing) return;
				RemoveTimeout(E_TimeoutType.Receive);

				TMSSocketData tSocketData = (TMSSocketData)ar.AsyncState;				

				IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
				EndPoint tempRemoteEP = (EndPoint)sender;

				int nByteRead = tSocketData.WorkSocket.EndReceiveFrom(ar, ref tempRemoteEP);
				
				if(nByteRead != 0)
				{						
					if(DataReceived != null) DataReceived(this, new TMSDataReceivedEventArgs(((IPEndPoint)tempRemoteEP).Address.ToString(), tSocketData, nByteRead)); //������ ���� �̺�Ʈ
				}
				else
				{			
					bool tIsReadable = m_Socket.Poll(1, SelectMode.SelectRead);
					if(tIsReadable && m_Socket.Available == 0)
					{
						if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.Disconnected));
					}
					else
					{
						m_Socket.BeginReceiveFrom(tSocketData.Buffers, 0, tSocketData.BufferSize, SocketFlags.None, ref tempRemoteEP, new AsyncCallback(ReceivedCallBack), tSocketData);
					}
				}
			}
			catch(SocketException sx)
			{
				if(sx.ErrorCode == 10054) 
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
				}
				else
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ �ޱ� ó�� ���� : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{				
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ �ޱ� ó�� ���� : " + ex.Message));
			}
		}
		#endregion //���� ������ �ޱ� ó�� ------------------------------------------------------

		#region ���� ������ ���� ó�� ------------------------------------------------------
		/// <summary>
		/// ������ �����͸� �����մϴ�.
		/// </summary>
		/// <param name="vSendData">������ �������Դϴ�</param>
		/// <param name="vEncodingType">�����͸� Encoding�� Encode����Դϴ�.</param>
		public void SendData(string vSendData, E_EncodingType vEncodingType)
		{
			byte [] tData = null;
			Encoding tEncoding;

			try
			{
				switch(vEncodingType)
				{
					case E_EncodingType.ASCII:
						tEncoding = Encoding.ASCII;
						tData = tEncoding.GetBytes(vSendData);
						AddTimeout(m_SendTimeout);
						m_Socket.BeginSend(tData, 0, tData.GetLength(0), SocketFlags.None, new AsyncCallback(SendedCallBack), m_Socket);
						break;

					case E_EncodingType.Unicode:
						tEncoding = Encoding.Unicode;
						tData = tEncoding.GetBytes(vSendData);
						AddTimeout(m_SendTimeout);
						m_Socket.BeginSend(tData, 0, tData.GetLength(0), SocketFlags.None, new AsyncCallback(SendedCallBack), m_Socket);
						break;
				}
			}
			catch(SocketException sx)
			{
				if(sx.ErrorCode == 10054) 
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
				}
				else
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ ���۽��� ���� : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{				
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ ���۽��� ���� : " + ex.Message));
			}			
		}
		

		/// <summary>
		/// ������ �����͸� �����մϴ�.
		/// </summary>
		/// <param name="vSendData">������ �������Դϴ�</param>
		public void SendData(string vSendData)
		{
			char [] tChars = vSendData.ToCharArray();
			byte [] tBytes = new byte[tChars.Length];

			for(int i=0; i < tChars.Length; i++)
			{
				tBytes[i] = Convert.ToByte(tChars[i]);
			}

			try
			{
				AddTimeout(m_SendTimeout);
				m_Socket.BeginSend(tBytes, 0, tBytes.Length, SocketFlags.None, new AsyncCallback(SendedCallBack), m_Socket);
			}
			catch(SocketException sx)
			{
				if(sx.ErrorCode == 10054) 
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
				}
				else
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ ���۽��� ���� : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{				
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ ���۽��� ���� : " + ex.Message));
			}
		}

		/// <summary>
		/// ������ �����͸� �����մϴ�.
		/// </summary>
		/// <param name="vSendData">������ �������Դϴ�.</param>
		public void SendData(byte [] vSendData)
		{	
			try
			{
				AddTimeout(m_SendTimeout);
				m_Socket.BeginSend(vSendData, 0, vSendData.GetLength(0), SocketFlags.None, new AsyncCallback(SendedCallBack), m_Socket);
			}
			catch(SocketException sx)
			{
				if(sx.ErrorCode == 10054) 
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
				}
				else
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ ���۽��� ���� : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{				
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ ���۽��� ���� : " + ex.Message));
			}		
		}

		/// <summary>
		/// ������ �����͸� �����մϴ�.
		/// </summary>
		/// <param name="vSendData">������ �������Դϴ�.</param>
		public void SendDataTo(byte [] vSendData, EndPoint vRemoteEndPoint)
		{	
			try
			{
				AddTimeout(m_SendTimeout);
				m_Socket.BeginSendTo(vSendData, 0, vSendData.Length, SocketFlags.None, vRemoteEndPoint, new AsyncCallback(SendedCallBack), m_Socket);
			}
			catch(SocketException sx)
			{
				if(sx.ErrorCode == 10054) 
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
				}
				else
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ ���۽��� ���� : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{				
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ ���۽��� ���� : " + ex.Message));
			}		
		}

		/// <summary>
		/// ������ ���� �ϷḦ ó���մϴ�.
		/// </summary>
		/// <param name="ar">�񵿱� ó�� ��� �Դϴ�.</param>
		private void SendedCallBack(IAsyncResult ar)
		{
			RemoveTimeout(E_TimeoutType.Send);
			try
			{				
				int tSendCount = m_Socket.EndSend(ar);
				if(DataSended != null) DataSended(this, EventArgs.Empty); //������ ���� �̺�Ʈ
				if(tSendCount > 0)
				{					
					RemoveTimeout(E_TimeoutType.Send);
				}
			}
			catch(SocketException sx)
			{
				if(sx.ErrorCode == 10054) 
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
				}
				else
				{
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ ����ó�� ���� : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{				
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "������ ����ó�� ���� : " + ex.Message));
			}
		}
		#endregion //���� ������ ���� ó�� ------------------------------------------------------		
	}
	#endregion //TMSUdpSocket �Դϴ� -------------------------------------------------------------
}
