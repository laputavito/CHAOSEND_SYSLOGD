using System;
using System.Threading;						// Mutex, 
using System.Text;							// Encoding, 
using System.IO;							// FileStream, StreamWriter, Exception, 
using System.Diagnostics;
using System.Collections;

namespace SYSLOGD
{
	/// <summary>
	/// FileLog 관련 클래스입니다.
	/// </summary>
	public class SOFileLog : IDisposable
	{
		#region 변수선언및 생성자 부분 입니다 -----------------------------------------------------

		/// <summary>
		/// 파일 경로입니다.
		/// </summary>
		private string m_FilePath = null;

		/// <summary>
		/// 파일명입니다.
		/// </summary>
		private string m_FileName = null;

		/// <summary>
		/// 확장자입니다.
		/// </summary>
		private string m_Extension = null;

		/// <summary>
		/// 파일명을 시간 패턴으로 기록할지 여부입니다.
		/// </summary>
		private bool m_UseDateTimeFileName = false;

		/// <summary>
		/// 기록 시간의 표시 여부입니다.
		/// </summary>
		private bool m_UseDateTimeLogMessage = false;

		/// <summary>
		/// 파일 기록 여부입니다.
		/// </summary>
		private bool m_UseFileLog = false;

		/// <summary>
		/// 파일 열기 유지 상태입니다.
		/// </summary>
		private bool m_KeepFileOpen = false;

		/// <summary>
		/// 파일의 재설정 모드입니다.
		/// </summary>
		private int m_ResetType = 0;

		/// <summary>
		/// 인코딩입니다.
		/// </summary>
		private Encoding m_Encoding = Encoding.Default;

		/// <summary>
		/// 인코딩에 대한 코드 페이지입니다.
		/// </summary>
		private int m_CodePage = -1;
		
		/// <summary>
		/// 파일을 여는 시간입니다.
		/// </summary>
		private DateTime m_OpenDateTime;

		/// <summary>
		/// 파일을 재설정하는 시간입니다.
		/// </summary>
		private DateTime m_ResetDateTime;

		private Queue m_LogQueue = new Queue();
		private Thread m_LogProcessThread = null;
		private bool m_isShutdown = false;

		/// <summary>
		/// 스트림쓰기 객체입니다.
		/// </summary>
		private StreamWriter m_StreamWriter = null;

		/// <summary>
		/// 기본 생성자 입니다.
		/// </summary>
		public SOFileLog()
		{
			Initialize();
		}

		/// <summary>
		/// 확장 생성자 입니다.
		/// </summary>
		/// <param name="vFilePath">파일경로입니다.</param>
		/// <param name="vFileName">파일명입니다.</param>
		/// <param name="vUseDateTimeFileName">파일명을 시간 패턴으로 기록할지 여부입니다.</param>
		/// <param name="vUseDateTimeLogMessage">기록 시간의 표시 여부입니다.</param>
		public SOFileLog(string vFilePath, string vFileName, 
			bool vUseDateTimeFileName, bool vUseDateTimeLogMessage)
		{
			Initialize();

			FilePath = vFilePath;
			FileName = vFileName;
			UseDateTimeFileName = vUseDateTimeFileName;
			UseDateTimeLogMessage = vUseDateTimeLogMessage;

			
		}

		/// <summary>
		/// 사용하지 않으면 명시적으로 호출합니다.
		/// </summary>
		public void Dispose()
		{
			try
			{
				m_isShutdown = true;				

				m_LogProcessThread.Join(1000);
				int tCount = 0;

				while(m_LogProcessThread.IsAlive)
				{
					tCount++;
//					if(tCount >= 30)
//					{
//						try
//						{
//							m_LogProcessThread.Abort();
//						}
//						catch(Exception ex)
//						{
//							Console.WriteLine(ex.ToString());
//						}
//						break;
//					}
//					else
//					{
						Thread.Sleep(100);
//					}
				}
				
				lock(m_LogQueue.SyncRoot)
				{
					m_LogQueue.Clear();
				}

				Close();
			}
			catch(Exception ex1)
			{
				Console.WriteLine(ex1.ToString());
			}

			GC.SuppressFinalize(this);
		}
		#endregion 변수선언및 생성자 부분 입니다 -----------------------------------------------------

		#region 클래스 속성 입니다 ----------------------------------------------------------------

		/// <summary>
		/// 파일의 경로를 가져오거나 설정합니다.
		/// </summary>
		public string FilePath
		{
			get { return m_FilePath; }
			set { m_FilePath = value; }
		}

		/// <summary>
		/// 파일명을 가져오거나 설정합니다.
		/// </summary>
		public string FileName
		{
			get { return m_FileName; }
			set { m_FileName = value; }
		}

		/// <summary>
		/// 파일의 확장자를 가져오거나 설정합니다.
		/// </summary>
		public string Extension
		{
			get { return m_Extension; }
			set { m_Extension = value; }
		}

		/// <summary>
		/// 파일명을 시간 패턴으로 기록할지 여부를 가져오거나 설정합니다.
		/// </summary>
		public bool UseDateTimeFileName
		{
			get { return m_UseDateTimeFileName; }
			set { m_UseDateTimeFileName = value; }
		}

		/// <summary>
		/// 기록 시간의 표시 여부를 가져오거나 설정합니다.
		/// </summary>
		public bool UseDateTimeLogMessage
		{
			get { return m_UseDateTimeLogMessage; }
			set { m_UseDateTimeLogMessage = value; }
		}

		/// <summary>
		/// 파일 기록 여부를 가져오거나 설정합니다.
		/// </summary>
		public bool UseFileLog
		{
			get { return m_UseFileLog; }
			set { m_UseFileLog = value; }
		}

		/// <summary>
		/// 파일 열기 유지 상태를 가져오거나 설정합니다.
		/// </summary>
		public bool KeepFileOpen
		{
			get { return m_KeepFileOpen; }
			set { m_KeepFileOpen = value; }
		}

		/// <summary>
		/// 파일의 재설정 모드를 가져오거나 설정합니다.
		/// </summary>
		public int ResetType
		{
			get { return m_ResetType; }
			set { m_ResetType = value; }
		}

		/// <summary>
		/// 인코딩에 대한 코드 페이지를 가져오거나 설정합니다.
		/// </summary>
		public int CodePage
		{
			get { return m_CodePage; }
			set 
			{ 
				m_CodePage = value;
				m_Encoding = Encoding.GetEncoding(value);
			}
		}
		#endregion 클래스 속성 입니다 ----------------------------------------------------------------

		#region 공용 메소드 입니다 ----------------------------------------------------------------
		/// <summary>
		/// 문자열에 엔터를 포함하여 기록합니다.
		/// </summary>
		/// <param name="vLog">문자열입니다.</param>
		public void PrintLogEnter(string vLog)
		{
			vLog += "\r\n";
			PrintLog(vLog);
		}

		/// <summary>
		/// 바이트 블럭을 기록합니다.
		/// </summary>
		public void PrintLog(string vLog)
		{
			lock(m_LogQueue.SyncRoot)
			{
				m_LogQueue.Enqueue(vLog);
			}
		}
		#endregion 공용 메소드 입니다 ----------------------------------------------------------------

		#region 지역 메소드 입니다 ----------------------------------------------------------------
		/// <summary>
		/// 초기화를 수행합니다.
		/// </summary>
		private void Initialize()
		{
			try
			{
				//Debug.Write("[P] Initialize : Start");

				FilePath = "";
				FileName = "";
				Extension = "log";

				UseDateTimeFileName = true;
				UseDateTimeLogMessage = true;
				UseFileLog = true;

				KeepFileOpen = false;

				m_OpenDateTime = DateTime.Now;

				ResetType = 0;

				CalculateResetTime();

				m_isShutdown = false;
				m_LogProcessThread = new Thread(new ThreadStart(LogProcess));
				m_LogProcessThread.Start();
			}
			catch {}
		}

		/// <summary>
		/// 파일의 재설정 시간을 계산합니다.
		/// </summary>
		private void CalculateResetTime()
		{
			try
			{
				//Debug.Write("[P] CalculateResetTime : Start");

				DateTime tCurrentDateTime = DateTime.Now;

				switch(ResetType)
				{
						// 하루
					case 0 :
						tCurrentDateTime.AddDays(1);
						m_ResetDateTime = new DateTime(
							tCurrentDateTime.Year, tCurrentDateTime.Month, tCurrentDateTime.Day, 
							0, 0, 0, 0);

						break;
				
						// 한시간
					case 1 : 
						tCurrentDateTime.AddHours(1);
						m_ResetDateTime = new DateTime(
							tCurrentDateTime.Year, tCurrentDateTime.Month, tCurrentDateTime.Day, 
							tCurrentDateTime.Hour, 0, 0, 0);

						break;

						// 한건
					case 2 :
						m_ResetDateTime = tCurrentDateTime;

						break;

						// 해당사항 없음
					default :
						break;
				}

				//Debug.Write("[P] CalculateResetTime : m_ResetDateTime : " + m_ResetDateTime.ToString());

			}
			catch {}
		}		

		/// <summary>
		/// 파일의 경로를 만듭니다.
		/// </summary>
		/// <param name="vFilePath">파일경로입니다.</param>
		/// <returns>만들어진 파일경로입니다.</returns>
		private string MakeStringFilePath(string vFilePath)
		{
			try
			{
				//Debug.Write("[P] MakeStringFilePath : Start");

				int tFilePathLength = vFilePath.Length;
				int tCurrentLength = tFilePathLength - 1;

				char tAttributer = (char)0;
				char tSeparator = '\\';

				for( ; tCurrentLength > 0; tCurrentLength--)
				{
					tAttributer = (char)vFilePath[tCurrentLength];
					if(tAttributer == tSeparator)
					{
						return vFilePath;
					}
					else if( (tAttributer == ' ') || (tAttributer == '\t') || (tAttributer == '\r') || 
						(tAttributer == '\n') || (tAttributer == '\b') || (tAttributer == '\f') )
					{
						continue;
					}
					else
					{
						break;
					}
				}

				return (vFilePath += tSeparator).ToString();
			}
			catch 
			{
				return "";
			}
		}

		/// <summary>
		/// 파일을 엽니다.
		/// </summary>
		/// <returns>성공, 실패입니다.</returns>
		private bool Open()
		{
			try
			{
				string tDateTime = "";

				//Debug.Write("[P] Open : Start");

				bool tReturnValue = false;

				if(UseFileLog == false)
				{
					return tReturnValue;
				}

				string tFilePath = FilePath;
				tFilePath = MakeStringFilePath(tFilePath);				

				try
				{
					if(UseDateTimeFileName == true)
					{
						switch(ResetType)
						{
								// 하루
							case 0 : 
								tDateTime = DateTime.Now.ToString("yyyyMMdd");
								break;
								// 한시간
							case 1 :
								tDateTime = DateTime.Now.ToString("yyyyMMddHH");
								break;
								// 한건
							case 2 : 
								tDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
								break;
								// 해당사항 없음
							default :
								break;
						}
						tFilePath += tDateTime + "-" + FileName + "." + Extension;
					}
					else
					{
						tFilePath += FileName + "." + Extension;
					}

					//Debug.Write("[P] Open : " + tFilePath);

					//cklee 2005-12-8 추가 시작 ------------------------------------------------
					if(!MakeDirectory(tFilePath, true))
					{
						return false;
					}
					//cklee 2005-12-8 추가 끝 ------------------------------------------------

					m_StreamWriter = new StreamWriter(tFilePath, true, Encoding.Default);
					m_StreamWriter.AutoFlush = true;

					CalculateResetTime();

					tReturnValue = true;

				}
				catch(Exception ioe)
				{
					Debug.WriteLine("[E] Open : ioe : " + ioe.ToString());

					tReturnValue = false;
				}					

				return tReturnValue;
			}
			catch 
			{					
				return false; 
			}
		}
		
		//cklee 2005-12-8 추가 시작 ------------------------------------------------
		/// <summary>
		/// 지정한 폴더를 생성합니다.
		/// </summary>
		/// <param name="vPath">폴더 위치 입니다.</param>
		/// <param name="isIncludeFileName">위치가 파일 이름을 포함 하고있는지의 여부 입니다.</param>
		/// <returns>작업 결과 입니다.</returns>
		private bool MakeDirectory(string vPath, bool isIncludeFileName)
		{
			try
			{
				if(vPath.Trim() == "") return true;

				string tPath = "";
				int tPos = 0; 
				if(isIncludeFileName)
				{					
					tPos = vPath.LastIndexOf('\\');
					if(tPos < 0) return true;
					tPath = vPath.Substring(0, tPos);
				}
				else
				{
					tPath = vPath;
				}
			
			
				if(!Directory.Exists(tPath))
				{
					Directory.CreateDirectory(tPath);				
				}
				return true;
			}
			catch(Exception ex)
			{
				ex=ex;
				return false;
			}			
		}
		//cklee 2005-12-8 추가 끝 ------------------------------------------------

		/// <summary>
		/// 파일을 닫습니다.
		/// </summary>
		/// <returns>성공, 실패입니다.</returns>
		private bool Close()
		{
			//Debug.Write("[P] Close : Start");

			bool tReturnValue = false;

			if(UseFileLog == false)
			{
				return tReturnValue;
			}

			try
			{
				if(m_StreamWriter != null)
				{
					m_StreamWriter.Close();
					m_StreamWriter = null;
				}				
			}
			catch(Exception ioe1)
			{
				Debug.WriteLine("[E] Close : ioe1 : " + ioe1.ToString());
			}
			finally
			{
				m_StreamWriter = null;
			}
			return tReturnValue;
		}

		/// <summary>
		/// 파일이 열려있는지 여부를 확인합니다.
		/// </summary>
		/// <returns>파일이 열려있는지 여부입니다.</returns>
		private bool IsOpen()
		{
			try
			{
				//Debug.Write("[P] IsOpen : Start");

				bool tReturnValue = false;

				if(m_StreamWriter == null)
				{
					return tReturnValue;
				}				

				if(m_ResetDateTime.Millisecond < DateTime.Now.Millisecond)
				{
					Close();
				}
				else
				{
					tReturnValue = true;
				}
				return tReturnValue;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 바이트 블럭을 기록합니다.
		/// </summary>
		/// <returns></returns>
		private int WriteByteData(string vLog)
		{
			//Debug.Write("[P] WriteByteData : Start");

			int tReturnValue = 0;

			if(UseFileLog == false)
			{
				return -1;
			}

			try
			{
				if(!IsOpen())
				{
					if(!Open()) return -1;
				}				

				m_StreamWriter.Write(vLog);
				tReturnValue = vLog.Length;

				if(KeepFileOpen == false)
				{
					Close();
				}				
			}
			catch(Exception ioe)
			{
				Debug.WriteLine("[E] WriteTo : ioe : " + ioe.ToString());
				tReturnValue = -1;
			}

			return tReturnValue;
		}

		/// <summary>
		/// 바이트 블럭을 기록합니다.
		/// </summary>
		private void LogProcess()
		{
			//Debug.Write("[P] PrintLog : Start");
			string tLog = "";

			while(!m_isShutdown)
			{
				if(UseFileLog == false)	return;
				tLog = "";

				try
				{
					lock(m_LogQueue.SyncRoot)
					{
						if(m_LogQueue.Count > 0)
						{
							tLog = (string)m_LogQueue.Dequeue();
						}
					}

					if(tLog != "")
					{
						if(UseDateTimeLogMessage == true) tLog = DateTime.Now.ToString("[HH:mm:ss] ") + tLog;				
						Debug.Write(tLog);
						WriteByteData(tLog);
					}
				}
				catch(Exception ioe)
				{
					Debug.WriteLine("[E] PrintLog : ioe : " + ioe.ToString());
				}

				Thread.Sleep(1);
			}
		}
		#endregion 지역 메소드 입니다 ----------------------------------------------------------------
	}
}