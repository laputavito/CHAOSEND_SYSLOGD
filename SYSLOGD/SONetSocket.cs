using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Threading;

namespace SYSLOGD
{
	#region 열거형 선언 --------------------------------------------------------------------
	/// <summary>
	/// 전송할 데이터를 Encoding할 타입 열거형 입니다.
	/// </summary>
	public enum E_EncodingType
	{
		/// <summary>
		/// 데이터를 ASCII코드로 Encoding합니다.
		/// </summary>
		ASCII,
		/// <summary>
		/// 데이터를 Unicode로 Encoding합니다.
		/// </summary>
		Unicode
	}

	/// <summary>
	/// 소켓의 상태 열거형 입니다.
	/// </summary>
	public enum E_SocketStatus
	{
		/// <summary>
		/// 연결중입니다.
		/// </summary>
		Connecting,
		/// <summary>
		/// 연결되어있습니다.
		/// </summary>
		Connected,
		/// <summary>
		/// 연결되어있지 않습니다.
		/// </summary>
		DisConnected
	}

	/// <summary>
	/// 소켓 오류 열거형 입니다.
	/// </summary>
	public enum E_SocketError
	{
		/// <summary>
		/// 오류가 없습니다.
		/// </summary>
		NoError,
		/// <summary>
		/// 연결이 종료 되었습니다.
		/// </summary>
		Disconnected,
		/// <summary>
		/// 연결중 타임아웃이 발생하였습니다.
		/// </summary>
		ConnectTimeout,
		/// <summary>
		/// 통신 중에 타임아웃이 발생하였습니다.
		/// </summary>
		Timeout,
		/// <summary>
		/// 원격 호스트에 의해 연결이 종료되었습니다.
		/// </summary>
		ConnectionResetByPeer,
		/// <summary>
		/// 알수 없는 오류입니다.
		/// </summary>
		UnknownError
	}

    /// <summary>
    /// 타임아웃 열거형입니다.
    /// </summary>
    internal enum E_TimeoutType
    {
        None,
        Connect,
        Receive,
        Send
    }
	#endregion //열거형 선언 --------------------------------------------------------------------

	#region 소켓 데이터 및 타임아웃 정보 클래스 -------------------------------------------------------------
	/// <summary>
	/// 소켓통신시 사용할 데이터 객체 입니다.
	/// </summary>
	public class TMSSocketData
	{
		/// <summary>
		/// 작업 소켓 입니다.
		/// </summary>
		private Socket m_WorkSocket;
		/// <summary>
		/// 소켓 버퍼 크기 입니다.
		/// </summary>
		private int m_BufferSize;
		/// <summary>
		/// 소켓 버퍼 배열 입니다.
		/// </summary>
		private byte [] m_Buffers;

		/// <summary>
		/// 소켓 데이터객체 기본 생성자 입니다.
		/// </summary>
		public TMSSocketData()
		{
			m_WorkSocket = null;
			m_BufferSize = 4096;
			Variable_Init();
		}			

		/// <summary>
		/// 소켓 데이터 객체 생성자 입니다.
		/// </summary>
		/// <param name="vWorkSocket">작업 소켓 입니다.</param>
		public TMSSocketData(Socket vWorkSocket)
		{
			m_WorkSocket = vWorkSocket;
            m_BufferSize = 4096;
			Variable_Init();
		}

		/// <summary>
		/// 소켓 데이터 객체 생성자 입니다.
		/// </summary>
		/// <param name="vWorkSocket">작업 소켓 입니다.</param>
		/// <param name="vBufferSize">소켓 버퍼 크기 입니다.</param>
		public TMSSocketData(Socket vWorkSocket, int vBufferSize)
		{
			m_WorkSocket = vWorkSocket;
			m_BufferSize = vBufferSize;
			Variable_Init();
		}

		/// <summary>
		/// 소켓 버퍼를 초기화 합니다.
		/// </summary>
		private void Variable_Init()
		{
			m_Buffers = new byte [m_BufferSize];
		}

		/// <summary>
		/// 작업 소켓을 가져오거나 설정합니다.
		/// </summary>
		public Socket WorkSocket
		{
			get { return m_WorkSocket; }
			set { m_WorkSocket = value; }
		}

		/// <summary>
		/// 소켓 버퍼의 크기를 가져오거나 설정합니다.
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
		/// 소켓 버퍼를 가져옵니다.
		/// </summary>
		public byte [] Buffers
		{
			get { return m_Buffers; }
		}
	}



    /// <summary>
    /// 타임 아웃 정보 클래스 입니다.
    /// </summary>
    internal class TimeoutInfo
    {
        /// <summary>
        /// 타임 아웃 타입 입니다.
        /// </summary>
        private E_TimeoutType m_TimeoutType = E_TimeoutType.None;
        /// <summary>
        /// 타임아웃 시간입니다(단위 초)
        /// </summary>
        private int m_Timeout = 0;

        /// <summary>
        /// 타임 아웃 정보의 기본 생성자 입니다.
        /// </summary>
        public TimeoutInfo() { }

        /// <summary>
        /// 타임 아웃 정보의 기본 생성자 입니다.
        /// </summary>
        /// <param name="vTimeoutType">타임 아웃 타입 입니다.</param>
        /// <param name="vTimeout">타임아웃 시간입니다(단위 초)</param>
        public TimeoutInfo(E_TimeoutType vTimeoutType, int vTimeout)
        {
            m_TimeoutType = vTimeoutType;
            m_Timeout = vTimeout;
        }

        /// <summary>
        /// 타임 아웃 타입을 가져오거나 설정합니다.
        /// </summary>
        public E_TimeoutType TimeoutType
        {
            get { return m_TimeoutType; }
            set { m_TimeoutType = value; }
        }

        /// <summary>
        /// 타임 아웃 시간을 가져오거나 설정합니다.
        /// </summary>
        public int Timeout
        {
            get { return m_Timeout; }
            set { m_Timeout = value; }
        }
    }
	#endregion //소켓 데이터 및 타임아웃 정보 클래스 -------------------------------------------------------------

	#region 소켓 통신 오류 클래스 입니다. --------------------------------------------------
	/// <summary>
	/// 소켓 통신 작업의 오류가 저장되는 클래스 입니다.
	/// </summary>
	public class TMSSocketError
	{
		/// <summary>
		/// 오류 코드 입니다.
		/// </summary>
		internal E_SocketError m_Error = E_SocketError.NoError;
		/// <summary>
		/// 오류 메시지 입니다.
		/// </summary>
		internal string m_ErrorMessage = "";
		/// <summary>
		/// 소켓 오류 클래스의 기본 생성자 입니다.
		/// </summary>
		public TMSSocketError() {}
		/// <summary>
		/// 소켓 오류 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vError">오류 코드 입니다.</param>
		/// <param name="vErrorMessage">오류 메시지 입니다.</param>
		public TMSSocketError(E_SocketError vError, string vErrorMessage)
		{
			m_Error = vError;
			m_ErrorMessage = vErrorMessage;
		}

		/// <summary>
		/// 소켓 통신 오류 코드를 가져 옵니다.
		/// </summary>
		public E_SocketError Error
		{
			get { return m_Error; }
		}

		/// <summary>
		/// 소켓 통신 오류 메시지를 가져옵니다.
		/// </summary>
		public string ErrorMessage
		{
			get { return m_ErrorMessage; }
		}
	}



	#endregion //소켓 통신 오류 클래스 입니다. --------------------------------------------------

	#region 이벤트 어규먼트 클래스 ---------------------------------------------------------
	/// <summary>
	/// 소켓 Accepted 이벤트 어규먼트 클래스 입니다.
	/// </summary>
	public class TMSSocketAcceptedEventArgs : EventArgs
	{
		/// <summary>
		/// 소켓 객체 입니다.
		/// </summary>
        private TMSUdpSocket m_Socket;

		/// <summary>
		/// 소켓 Accepted 이벤트 어규먼트 생성자 입니다.
		/// </summary>
		/// <param name="vSocket">소켓 객체 입니다.</param>
		public TMSSocketAcceptedEventArgs(Socket vSocket)
		{
            m_Socket = new TMSUdpSocket(vSocket);
            //vSocket.Available = 3000;
		}

		/// <summary>
		/// 소켓을 가져옵니다.
		/// </summary>
        public TMSUdpSocket Socket
		{
			get { return m_Socket; }
		}
	}
	/// <summary>
	/// 소켓 Accept이벤트 핸들러 입니다.
	/// </summary>
	public delegate void TMSSocketAcceptedEventHandler(object sender, TMSSocketAcceptedEventArgs e);

	/// <summary>
	/// 소켓 데이터 받음 이벤트 어규먼트 입니다.
	/// </summary>
	public class TMSDataReceivedEventArgs : EventArgs
	{
		/// <summary>
		/// 소켓 데이터 입니다.
		/// </summary>
		private TMSSocketData m_SocketData;
		/// <summary>
		/// 소켓으로 받은 데이터 Byte개수 입니다.
		/// </summary>
		private int m_ReadCount;
		/// <summary>
		/// 데이터를 전송한 IP주소 입니다.
		/// </summary>
		private string m_SenderIPAddress = "";

		/// <summary>
		/// 소켓 데이터 받음 이벤트 어규먼트 생성자 입니다.
		/// </summary>
		/// <param name="vSocketData">소켓 데이터 입니다.</param>
		/// <param name="vReadCount">받은 데이터의 Byte개수 입니다.</param>
		public TMSDataReceivedEventArgs(TMSSocketData vSocketData, int vReadCount)
		{
			m_SocketData = vSocketData;
			m_ReadCount = vReadCount;
		}

		/// <summary>
		/// 소켓 데이터 받음 이벤트 어규먼트 생성자 입니다.
		/// </summary>
		/// <param name="vSenderIPAddress">데이터를 전송한 IP주소 입니다.</param>
		/// <param name="vSocketData">소켓 데이터 입니다.</param>
		/// <param name="vReadCount">받은 데이터의 Byte개수 입니다.</param>
		public TMSDataReceivedEventArgs(string vSenderIPAddress, TMSSocketData vSocketData, int vReadCount)
		{
			m_SocketData = vSocketData;
			m_ReadCount = vReadCount;
			m_SenderIPAddress = vSenderIPAddress;
		}

		/// <summary>
		/// 소켓 데이터 객체를 가져 옵니다.
		/// </summary>
		public TMSSocketData SocketData
		{
			get { return m_SocketData; }
		}

		/// <summary>
		/// 받은 데이터 개수를 가져옵니다.
		/// </summary>
		public int ReadCount
		{
			get { return m_ReadCount; }
		}

		/// <summary>
		/// 데이터를 전송한 IP주소를 가져옵니다.
		/// </summary>
		public string SenderIPAddress
		{
			get { return m_SenderIPAddress; }
		}
	}
	/// <summary>
	/// 소켓 데이터받음 이벤트 핸들러 입니다.
	/// </summary>
	public delegate void TMSDataReceivedEventHandler(object sender, TMSDataReceivedEventArgs e);

    /// <summary>
    /// 소켓 오류 이벤트 어규먼트 입니다.
    /// </summary>
    public class TMSSocketErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 소켓 오류 열거형입니다.
        /// </summary>
        private E_SocketError m_SocketError = E_SocketError.NoError;
        /// <summary>
        /// 오류 메시지 입니다.
        /// </summary>
        private string m_Message = "";

        /// <summary>
        /// 소켓 오류 이벤트 어규먼트 생성자 입니다.
        /// </summary>
        /// <param name="vSocketError">소켓 오류 입니다.</param>
        public TMSSocketErrorEventArgs(E_SocketError vSocketError)
        {
            m_SocketError = vSocketError;
        }

        /// <summary>
        /// 소켓 오류 이벤트 어규먼트 생성자 입니다.
        /// </summary>
        /// <param name="vSocketError">소켓 오류 입니다.</param>
        /// <param name="vMessage">소켓 오류 메시지 입니다.</param>
        public TMSSocketErrorEventArgs(E_SocketError vSocketError, string vMessage)
        {
            m_SocketError = vSocketError;
            m_Message = vMessage;
        }

        /// <summary>
        /// 소켓 오류 열거형을 가져옵니다.
        /// </summary>
        public E_SocketError SocketError
        {
            get { return m_SocketError; }
        }

        /// <summary>
        /// 소켓 오류 메시지를 가져옵니다.
        /// </summary>
        public string Message
        {
            get { return m_Message; }
        }
    }

    /// <summary>
    /// 소켓 오류 이벤트 핸들러 입니다.
    /// </summary>
    public delegate void TMSSocketErrorEventHandler(object sender, TMSSocketErrorEventArgs e);

	#endregion //이벤트 어규먼트 클래스 ---------------------------------------------------------

	#region TMSUdpSocket 입니다 -------------------------------------------------------------
    /// <summary>
	/// 네트워크를 통하여 데이터 전송을 처리할 비동기 소켓 클래스 입니다.
	/// </summary>
	public class TMSUdpSocket : IDisposable
	{
		#region 변수 선언및 생성자 ---------------------------------------------------------
		/// <summary>
		/// 귀기울임 소켓입니다.
		/// </summary>
		private Socket m_Listener;
		/// <summary>
		/// 작업 소켓 객체 입니다.
		/// </summary>
		private Socket m_Socket;
		/// <summary>
		/// 타임 아웃 시간 입니다.(단위 : 초)
		/// </summary>
		private int	m_Timeout = 15;
		/// <summary>
		/// 객체가 소멸중인지의 여부 입니다.
		/// </summary>
		private bool m_Disposing = false;
		/// <summary>
		/// 타임아웃 체크 스래드 입니다.
		/// </summary>
		private Thread m_TimeoutThread = null;
		/// <summary>
		/// 타임아웃 시간 목록 입니다.
		/// </summary>
		private ArrayList m_TimeoutList = new ArrayList();
		
		private TimeoutInfo m_ConnectTimeout = new TimeoutInfo(E_TimeoutType.Connect, 15);
		private TimeoutInfo m_ReceiveTimeout = new TimeoutInfo(E_TimeoutType.Receive, 15);
		private TimeoutInfo m_SendTimeout = new TimeoutInfo(E_TimeoutType.Send, 15);

		/// <summary>
		/// 접속 받아들임 이벤트 입니다.
		/// </summary>
		public event TMSSocketAcceptedEventHandler SocketAccepted;
		/// <summary>
		/// 접속 연결됨 이벤트 입니다.
		/// </summary>
		public event EventHandler SocketConnected;
		/// <summary>
		/// 데이터 받음 이벤트 입니다.
		/// </summary>
		public event TMSDataReceivedEventHandler DataReceived;
		/// <summary>
		/// 데이터 전송됨 이벤트 입니다.
		/// </summary>
		public event EventHandler DataSended;
		/// <summary>
		/// 소켓 오류 이벤트 입니다.
		/// </summary>
		public event TMSSocketErrorEventHandler SocketError;

		/// <summary>
		/// Network Socket 클래스의 기본 생성자 입니다.
		/// </summary>
		public TMSUdpSocket()
		{
			m_TimeoutThread = new Thread(new ThreadStart(CheckTimeout));
			m_TimeoutThread.Start();
		}
	
		/// <summary>
		/// Network Socket 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vSocket"> Windows Socket 객체 입니다.</param>
		public TMSUdpSocket(Socket vSocket)
		{			
			m_Socket = vSocket;
			m_TimeoutThread = new Thread(new ThreadStart(CheckTimeout));
			m_TimeoutThread.Start();
		}		

		/// <summary>
		/// 클래스를 소멸 시킵니다.
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

				//통신 소켓을 닫는다
				if(m_Socket != m_Listener) 
				{	
					if(m_Socket != null)
					{
						m_Socket.Shutdown(SocketShutdown.Both);
						m_Socket.Close();
						m_Socket = null;
					}
				}

				//연결 소켓을 닫는다
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


		#endregion //변수 선언및 생성자 ---------------------------------------------------------		

        #region 타임아웃 관련 매서드입니다 -----------------------------------------------------
        /// <summary>
        /// 타임 아웃을 확인합니다.
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
        #endregion //타임아웃 관련 매서드입니다 -----------------------------------------------------

 		#region 소켓 귀 기울임 처리 --------------------------------------------------------
		/// <summary>
		/// 연결 대기를 시작합니다.
		/// </summary>
		/// <param name="vPortNumber">대기할 포트번호입니다.</param>
		public void StartListening(int vPortNumber)
		{		
			StartListen(IPAddress.Any, vPortNumber, 10);
		}

		/// <summary>
		/// 연결 대기를 시작합니다.
		/// </summary>
		/// <param name="vPortNumber">대기할 포트번호입니다.</param>
		/// <param name="vMaxListenCount">최대 연결 대기 개수입니다.</param>
		public void StartListening(int vPortNumber, int vMaxListenCount)
		{
			StartListen(IPAddress.Any, vPortNumber, vMaxListenCount);
		}		

		/// <summary>
		/// 연결 대기를 시작합니다.
		/// </summary>
		/// <param name="vIPAddress">대기에 사용할 IP주소 입니다.</param>
		/// <param name="vPortNumber">대기할 포트번호입니다.</param>
		public void StartListening(string vIPAddress, int vPortNumber)
		{
			StartListen(vIPAddress, vPortNumber, 10);
		}

		/// <summary>
		/// 연결 대기를 시작합니다.
		/// </summary>
		/// <param name="vIPAddress">대기에 사용할 IP주소 입니다.</param>
		/// <param name="vPortNumber">대기할 포트번호입니다.</param>
		/// <param name="vMaxListenCount">최대 연결 대기 개수입니다.</param>
		public void StartListening(string vIPAddress, int vPortNumber, int vMaxListenCount)
		{
			StartListen(vIPAddress, vPortNumber, vMaxListenCount);
		}		

		/// <summary>
		/// 연결 대기를 시작합니다.
		/// </summary>
		/// <param name="vIPAddress">대기에 사용할 IP주소 입니다.</param>
		/// <param name="vPortNumber">대기할 포트번호입니다.</param>
		/// <param name="vMaxListenCount">최대 연결 대기 개수입니다.</param>
		private void StartListen(string vIPAddress, int vPortNumber, int vMaxListenCount)
		{
			IPAddress tIP = IPAddress.Parse(vIPAddress);
			StartListen(tIP, vPortNumber, vMaxListenCount);
		}

		/// <summary>
		/// 연결 대기를 시작합니다.
		/// </summary>
		/// <param name="vIPAddress">대기에 사용할 IP주소 객체 입니다.</param>
		/// <param name="vPortNumber">대기할 포트번호입니다.</param>
		/// <param name="vMaxListenCount">최대 연결 대기 개수입니다.</param>
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
					Console.WriteLine("이미사용중인 포트번호 입니다");
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("귀기울임 시작 sx : " + sx.ToString());
				}
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("귀기울임 시작 ex : " + ex.ToString());
			}
		}

		/// <summary>
		/// 연결을 받아들입니다.
		/// </summary>
		/// <param name="ar">비동기 처리 결과 입니다.</param>
		private void AcceptCallback(IAsyncResult ar)
		{
			try
			{
				Socket tListener = (Socket)ar.AsyncState;
				Socket tConnectedSocket = tListener.EndAccept(ar);

				//연결 되었음을 알리는 이벤트
				if(SocketAccepted != null) SocketAccepted(this, new TMSSocketAcceptedEventArgs(tConnectedSocket));

				//클라이언트를 다시 받아들인다.
				tListener.BeginAccept(new System.AsyncCallback(AcceptCallback), tListener);
			}
			catch(SocketException se)
			{
				System.Diagnostics.Debug.WriteLine("접속 받아들임 se : " + se.Message);
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("접속 받아들임 ex : " + ex.Message);
			}
		}
		#endregion //소켓 귀 기울임 처리 --------------------------------------------------------

		#region 소켓 연결 처리 -------------------------------------------------------------
		/// <summary>
		/// 서버 소켓에 연결을 시도합니다.
		/// </summary>
		/// <param name="vRemoteIPAddress">연결할 서버소켓의 IP주소 입니다.</param>
		/// <param name="vRemotePortNumber">연결할 서버소켓의 포트번호 입니다.</param>
		public void StartConnecting(string vRemoteIPAddress, int vRemotePortNumber)
		{
			StartConnect(vRemoteIPAddress, vRemotePortNumber, ProtocolType.Tcp, SocketType.Stream);
		}

		/// <summary>
		/// 서버 소켓에 연결을 시도합니다.
		/// </summary>
		/// <param name="vLocalIPAddress">로컬 IP주소 입니다.</param>
		/// <param name="vLocalPortNumber">로컬 포트번호 입니다.</param>
		/// <param name="vRemoteIPAddress">연결할 서버소켓의 IP주소 입니다.</param>
		/// <param name="vRemotePortNumber">연결할 서버소켓의 포트번호 입니다.</param>
		public void StartConnecting(string vLocalIPAddress, int vLocalPortNumber, string vRemoteIPAddress, int vRemotePortNumber)
		{
			StartConnect(vRemoteIPAddress, vRemotePortNumber, ProtocolType.Tcp, SocketType.Stream);
		}

		/// <summary>
		/// 서버 소켓에 연결을 시도합니다.
		/// </summary>
		/// <param name="vRemoteIPAddress">연결할 서버소켓의 IP주소 입니다.</param>
		/// <param name="vRemotePortNumber">연결할 서버소켓의 포트번호 입니다.</param>
		/// <param name="vProtocolType">연결할 서버소켓의 프로토콜 타입 입니다.</param>
		public void StartConnecting(string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType)
		{
			StartConnect(vRemoteIPAddress, vRemotePortNumber, vProtocolType, SocketType.Stream);
		}

		/// <summary>
		/// 서버 소켓에 연결을 시도합니다.
		/// </summary>
		/// <param name="vRemoteIPAddress">연결할 서버소켓의 IP주소 입니다.</param>
		/// <param name="vRemotePortNumber">연결할 서버소켓의 포트번호 입니다.</param>
		/// <param name="vProtocolType">연결할 서버소켓의 프로토콜 타입 입니다.</param>
		/// <param name="vSocketType">연결할 서버소켓의 소켓 타입 입니다.</param>
		public void StartConnecting(string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType, SocketType vSocketType)
		{
			StartConnect(vRemoteIPAddress, vRemotePortNumber, vProtocolType, vSocketType);
		}

		/// <summary>
		/// 서버 소켓에 연결을 시도합니다.
		/// </summary>
		/// <param name="vRemoteIPAddress">연결할 서버소켓의 IP주소 입니다.</param>
		/// <param name="vRemotePortNumber">연결할 서버소켓의 포트번호 입니다.</param>
		/// <param name="vProtocolType">연결할 서버소켓의 프로토콜 타입 입니다.</param>
		/// <param name="vSocketType">연결할 서버소켓의 소켓 타입 입니다.</param>
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
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "소켓 연결시도 오류 : " + sx.Message + " " + sx.ErrorCode));
			}
			catch(Exception ex)
			{
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "소켓 연결시도 오류 : " + ex.Message));
			}
		}

		/// <summary>
		/// 서버 소켓에 연결을 시도합니다.
		/// </summary>
		/// <param name="vLocalIPAddress">로컬 IP주소 입니다.</param>
		/// <param name="vLocalPortNumber">로컬 포트번호 입니다.</param>
		/// <param name="vRemoteIPAddress">연결할 서버소켓의 IP주소 입니다.</param>
		/// <param name="vRemotePortNumber">연결할 서버소켓의 포트번호 입니다.</param>
		/// <param name="vProtocolType">연결할 서버소켓의 프로토콜 타입 입니다.</param>
		/// <param name="vSocketType">연결할 서버소켓의 소켓 타입 입니다.</param>
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
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "소켓 연결시도 오류 : " + sx.Message + " " + sx.ErrorCode));
			}
			catch(Exception ex)
			{
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "소켓 연결시도 오류 : " + ex.Message));
			}
		}

		/// <summary>
		/// 소켓 연결을 처리합니다.
		/// </summary>
		/// <param name="ar">비동기 처리 결과 입니다.</param>
		private void ConnectCallback(System.IAsyncResult ar)
		{
			try
			{	
				//m_ConnectTimeoutTimer.Enabled = false;
				RemoveTimeout(E_TimeoutType.Connect);

				Socket tClient = (Socket)ar.AsyncState;	
			
				//소켓이 연결되었는지 확인합니다.
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
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "소켓 연결처리 오류 : " + sx.Message + " " + sx.ErrorCode));
			}
			catch(Exception ex)
			{
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "소켓 연결처리 오류 : " + ex.Message));
			}
		}
		#endregion //소켓 연결 처리 -------------------------------------------------------------

		#region 소켓 데이터 받기 처리 ------------------------------------------------------
		/// <summary>
		/// 데이터 받기를 대기합니다.
		/// </summary>
		public void StartReceive()
		{
			TMSSocketData tSocketData = new TMSSocketData(m_Socket);
			StartReceive(tSocketData);
		}

		/// <summary>
		/// 데이터받기를 대기합니다.
		/// </summary>
		/// <param name="vSocketData">소켓정보가 들어있는 TMSSocketData클래스 입니다.</param>
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
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 받기 시작 오류 : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 받기 시작 오류 : " + ex.Message));
			}
		}

		/// <summary>
		/// 데이터받기를 처리합니다.
		/// </summary>
		/// <param name="ar">데이터를 받을 비동기 결과 객체 입니다.</param>
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
					if(DataReceived != null) DataReceived(this, new TMSDataReceivedEventArgs(((IPEndPoint)tempRemoteEP).Address.ToString(), tSocketData, nByteRead)); //데이터 받음 이벤트
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
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 받기 처리 오류 : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{				
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 받기 처리 오류 : " + ex.Message));
			}
		}
		#endregion //소켓 데이터 받기 처리 ------------------------------------------------------

		#region 소켓 데이터 전송 처리 ------------------------------------------------------
		/// <summary>
		/// 지정한 데이터를 전송합니다.
		/// </summary>
		/// <param name="vSendData">전송할 데이터입니다</param>
		/// <param name="vEncodingType">데이터를 Encoding할 Encode방식입니다.</param>
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
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 전송시작 오류 : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{				
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 전송시작 오류 : " + ex.Message));
			}			
		}
		

		/// <summary>
		/// 지정한 데이터를 전송합니다.
		/// </summary>
		/// <param name="vSendData">전송할 데이터입니다</param>
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
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 전송시작 오류 : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{				
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 전송시작 오류 : " + ex.Message));
			}
		}

		/// <summary>
		/// 지정한 데이터를 전송합니다.
		/// </summary>
		/// <param name="vSendData">전송할 데이터입니다.</param>
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
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 전송시작 오류 : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{				
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 전송시작 오류 : " + ex.Message));
			}		
		}

		/// <summary>
		/// 지정한 데이터를 전송합니다.
		/// </summary>
		/// <param name="vSendData">전송할 데이터입니다.</param>
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
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 전송시작 오류 : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{				
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 전송시작 오류 : " + ex.Message));
			}		
		}

		/// <summary>
		/// 데이터 전송 완료를 처리합니다.
		/// </summary>
		/// <param name="ar">비동기 처리 결과 입니다.</param>
		private void SendedCallBack(IAsyncResult ar)
		{
			RemoveTimeout(E_TimeoutType.Send);
			try
			{				
				int tSendCount = m_Socket.EndSend(ar);
				if(DataSended != null) DataSended(this, EventArgs.Empty); //데이터 보냄 이벤트
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
					if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 전송처리 오류 : " + sx.Message + " " + sx.ErrorCode));
				}
			}
			catch(Exception ex)
			{				
				if(SocketError != null) SocketError(this, new TMSSocketErrorEventArgs(E_SocketError.UnknownError, "데이터 전송처리 오류 : " + ex.Message));
			}
		}
		#endregion //소켓 데이터 전송 처리 ------------------------------------------------------		
	}
	#endregion //TMSUdpSocket 입니다 -------------------------------------------------------------
}
