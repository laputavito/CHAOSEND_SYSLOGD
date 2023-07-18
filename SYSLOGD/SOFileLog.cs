using System;
using System.Threading;						// Mutex, 
using System.Text;							// Encoding, 
using System.IO;							// FileStream, StreamWriter, Exception, 
using System.Diagnostics;
using System.Collections;

namespace SYSLOGD
{
	/// <summary>
	/// FileLog ���� Ŭ�����Դϴ�.
	/// </summary>
	public class SOFileLog : IDisposable
	{
		#region ��������� ������ �κ� �Դϴ� -----------------------------------------------------

		/// <summary>
		/// ���� ����Դϴ�.
		/// </summary>
		private string m_FilePath = null;

		/// <summary>
		/// ���ϸ��Դϴ�.
		/// </summary>
		private string m_FileName = null;

		/// <summary>
		/// Ȯ�����Դϴ�.
		/// </summary>
		private string m_Extension = null;

		/// <summary>
		/// ���ϸ��� �ð� �������� ������� �����Դϴ�.
		/// </summary>
		private bool m_UseDateTimeFileName = false;

		/// <summary>
		/// ��� �ð��� ǥ�� �����Դϴ�.
		/// </summary>
		private bool m_UseDateTimeLogMessage = false;

		/// <summary>
		/// ���� ��� �����Դϴ�.
		/// </summary>
		private bool m_UseFileLog = false;

		/// <summary>
		/// ���� ���� ���� �����Դϴ�.
		/// </summary>
		private bool m_KeepFileOpen = false;

		/// <summary>
		/// ������ �缳�� ����Դϴ�.
		/// </summary>
		private int m_ResetType = 0;

		/// <summary>
		/// ���ڵ��Դϴ�.
		/// </summary>
		private Encoding m_Encoding = Encoding.Default;

		/// <summary>
		/// ���ڵ��� ���� �ڵ� �������Դϴ�.
		/// </summary>
		private int m_CodePage = -1;
		
		/// <summary>
		/// ������ ���� �ð��Դϴ�.
		/// </summary>
		private DateTime m_OpenDateTime;

		/// <summary>
		/// ������ �缳���ϴ� �ð��Դϴ�.
		/// </summary>
		private DateTime m_ResetDateTime;

		private Queue m_LogQueue = new Queue();
		private Thread m_LogProcessThread = null;
		private bool m_isShutdown = false;

		/// <summary>
		/// ��Ʈ������ ��ü�Դϴ�.
		/// </summary>
		private StreamWriter m_StreamWriter = null;

		/// <summary>
		/// �⺻ ������ �Դϴ�.
		/// </summary>
		public SOFileLog()
		{
			Initialize();
		}

		/// <summary>
		/// Ȯ�� ������ �Դϴ�.
		/// </summary>
		/// <param name="vFilePath">���ϰ���Դϴ�.</param>
		/// <param name="vFileName">���ϸ��Դϴ�.</param>
		/// <param name="vUseDateTimeFileName">���ϸ��� �ð� �������� ������� �����Դϴ�.</param>
		/// <param name="vUseDateTimeLogMessage">��� �ð��� ǥ�� �����Դϴ�.</param>
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
		/// ������� ������ ��������� ȣ���մϴ�.
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
		#endregion ��������� ������ �κ� �Դϴ� -----------------------------------------------------

		#region Ŭ���� �Ӽ� �Դϴ� ----------------------------------------------------------------

		/// <summary>
		/// ������ ��θ� �������ų� �����մϴ�.
		/// </summary>
		public string FilePath
		{
			get { return m_FilePath; }
			set { m_FilePath = value; }
		}

		/// <summary>
		/// ���ϸ��� �������ų� �����մϴ�.
		/// </summary>
		public string FileName
		{
			get { return m_FileName; }
			set { m_FileName = value; }
		}

		/// <summary>
		/// ������ Ȯ���ڸ� �������ų� �����մϴ�.
		/// </summary>
		public string Extension
		{
			get { return m_Extension; }
			set { m_Extension = value; }
		}

		/// <summary>
		/// ���ϸ��� �ð� �������� ������� ���θ� �������ų� �����մϴ�.
		/// </summary>
		public bool UseDateTimeFileName
		{
			get { return m_UseDateTimeFileName; }
			set { m_UseDateTimeFileName = value; }
		}

		/// <summary>
		/// ��� �ð��� ǥ�� ���θ� �������ų� �����մϴ�.
		/// </summary>
		public bool UseDateTimeLogMessage
		{
			get { return m_UseDateTimeLogMessage; }
			set { m_UseDateTimeLogMessage = value; }
		}

		/// <summary>
		/// ���� ��� ���θ� �������ų� �����մϴ�.
		/// </summary>
		public bool UseFileLog
		{
			get { return m_UseFileLog; }
			set { m_UseFileLog = value; }
		}

		/// <summary>
		/// ���� ���� ���� ���¸� �������ų� �����մϴ�.
		/// </summary>
		public bool KeepFileOpen
		{
			get { return m_KeepFileOpen; }
			set { m_KeepFileOpen = value; }
		}

		/// <summary>
		/// ������ �缳�� ��带 �������ų� �����մϴ�.
		/// </summary>
		public int ResetType
		{
			get { return m_ResetType; }
			set { m_ResetType = value; }
		}

		/// <summary>
		/// ���ڵ��� ���� �ڵ� �������� �������ų� �����մϴ�.
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
		#endregion Ŭ���� �Ӽ� �Դϴ� ----------------------------------------------------------------

		#region ���� �޼ҵ� �Դϴ� ----------------------------------------------------------------
		/// <summary>
		/// ���ڿ��� ���͸� �����Ͽ� ����մϴ�.
		/// </summary>
		/// <param name="vLog">���ڿ��Դϴ�.</param>
		public void PrintLogEnter(string vLog)
		{
			vLog += "\r\n";
			PrintLog(vLog);
		}

		/// <summary>
		/// ����Ʈ ���� ����մϴ�.
		/// </summary>
		public void PrintLog(string vLog)
		{
			lock(m_LogQueue.SyncRoot)
			{
				m_LogQueue.Enqueue(vLog);
			}
		}
		#endregion ���� �޼ҵ� �Դϴ� ----------------------------------------------------------------

		#region ���� �޼ҵ� �Դϴ� ----------------------------------------------------------------
		/// <summary>
		/// �ʱ�ȭ�� �����մϴ�.
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
		/// ������ �缳�� �ð��� ����մϴ�.
		/// </summary>
		private void CalculateResetTime()
		{
			try
			{
				//Debug.Write("[P] CalculateResetTime : Start");

				DateTime tCurrentDateTime = DateTime.Now;

				switch(ResetType)
				{
						// �Ϸ�
					case 0 :
						tCurrentDateTime.AddDays(1);
						m_ResetDateTime = new DateTime(
							tCurrentDateTime.Year, tCurrentDateTime.Month, tCurrentDateTime.Day, 
							0, 0, 0, 0);

						break;
				
						// �ѽð�
					case 1 : 
						tCurrentDateTime.AddHours(1);
						m_ResetDateTime = new DateTime(
							tCurrentDateTime.Year, tCurrentDateTime.Month, tCurrentDateTime.Day, 
							tCurrentDateTime.Hour, 0, 0, 0);

						break;

						// �Ѱ�
					case 2 :
						m_ResetDateTime = tCurrentDateTime;

						break;

						// �ش���� ����
					default :
						break;
				}

				//Debug.Write("[P] CalculateResetTime : m_ResetDateTime : " + m_ResetDateTime.ToString());

			}
			catch {}
		}		

		/// <summary>
		/// ������ ��θ� ����ϴ�.
		/// </summary>
		/// <param name="vFilePath">���ϰ���Դϴ�.</param>
		/// <returns>������� ���ϰ���Դϴ�.</returns>
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
		/// ������ ���ϴ�.
		/// </summary>
		/// <returns>����, �����Դϴ�.</returns>
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
								// �Ϸ�
							case 0 : 
								tDateTime = DateTime.Now.ToString("yyyyMMdd");
								break;
								// �ѽð�
							case 1 :
								tDateTime = DateTime.Now.ToString("yyyyMMddHH");
								break;
								// �Ѱ�
							case 2 : 
								tDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
								break;
								// �ش���� ����
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

					//cklee 2005-12-8 �߰� ���� ------------------------------------------------
					if(!MakeDirectory(tFilePath, true))
					{
						return false;
					}
					//cklee 2005-12-8 �߰� �� ------------------------------------------------

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
		
		//cklee 2005-12-8 �߰� ���� ------------------------------------------------
		/// <summary>
		/// ������ ������ �����մϴ�.
		/// </summary>
		/// <param name="vPath">���� ��ġ �Դϴ�.</param>
		/// <param name="isIncludeFileName">��ġ�� ���� �̸��� ���� �ϰ��ִ����� ���� �Դϴ�.</param>
		/// <returns>�۾� ��� �Դϴ�.</returns>
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
		//cklee 2005-12-8 �߰� �� ------------------------------------------------

		/// <summary>
		/// ������ �ݽ��ϴ�.
		/// </summary>
		/// <returns>����, �����Դϴ�.</returns>
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
		/// ������ �����ִ��� ���θ� Ȯ���մϴ�.
		/// </summary>
		/// <returns>������ �����ִ��� �����Դϴ�.</returns>
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
		/// ����Ʈ ���� ����մϴ�.
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
		/// ����Ʈ ���� ����մϴ�.
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
		#endregion ���� �޼ҵ� �Դϴ� ----------------------------------------------------------------
	}
}