using System;
using System.Reflection;

namespace SYSLOGD
{
	#region Syslog���� �������Դϴ� --------------------------------------------------------
	/// <summary>
	/// Syslog Facility �������Դϴ�.
	/// </summary>
	[Serializable]
	public enum E_Facility
	{
		Kernel = 0,
		UserLevel,
		MailSystem,
		SystemDaemons,
		SecurityOrAuthorization1,
		BySyslogDaemons,
		LinePrinterSubsystem,
		NetworkSubsystem,
		UUCPSubsystem,
		ClockDaemon1,
		SecurityOrAuthorization2,
		FTP,
		NTP,
		LogAudit,
		LogAlert,
		ClockDaemon2,
		LocalUse0,
		LocalUse1,
		LocalUse2,
		LocalUse3,
		LocalUse4,
		LocalUse5,
		LocalUse6,
		LocalUse7
	}

	/// <summary>
	/// Syslog Severity �������Դϴ�.
	/// </summary>
	[Serializable]
	public enum E_Severity
	{
		Emergency = 0,
		Alert,
		Critical,
		Error,
		Warning,
		Notice,
		Informational,
		Debug
	}
	
	/// <summary>
	/// Syslog �� �������Դϴ�.
	/// </summary>
	[Serializable]
	public enum E_SyslogMonth
	{
		Jan = 1, 
		Feb, 
		Mar, 
		Apr, 
		May, 
		Jun, 
		Jul, 
		Aug, 
		Sep, 
		Oct, 
		Nov, 
		Dec
	}

	/// <summary>
	/// Syslog Error �������Դϴ�.
	/// </summary>
	[Serializable]
	public enum E_SyslogError
	{
		NoError,
		UnKnownLevel
	}
	#endregion //Syslog���� �������Դϴ� --------------------------------------------------------

	#region Syslog Timestamp Ŭ���� �Դϴ� -------------------------------------------------
	/// <summary>
	/// Syslog Timestamp Ŭ���� �Դϴ�.
	/// </summary>
	public class TMSSyslogTimestamp
	{
		/// <summary>
		/// Syslog�� �Դϴ�.
		/// </summary>
		private E_SyslogMonth m_Month = E_SyslogMonth.Jan;
		/// <summary>
		/// Syslog �� �Դϴ�.
		/// </summary>
		private int m_Day = 0;
		/// <summary>
		/// Syslog �� �Դϴ�.
		/// </summary>
		private int m_Hour = 0;
		/// <summary>
		/// Syslog �� �Դϴ�.
		/// </summary>
		private int m_Minute = 0;
		/// <summary>
		/// Syslog �� �Դϴ�.
		/// </summary>
		private int m_Second = 0;

		/// <summary>
		/// Syslog Timestamp�⺻ ������ �Դϴ�.
		/// </summary>
		public TMSSyslogTimestamp() { }

		/// <summary>
		/// Syslog Timestamp ������ �Դϴ�.
		/// </summary>
		/// <param name="vMonth">Syslog �� �Դϴ�.</param>
		/// <param name="vDay">Syslog �� �Դϴ�.</param>
		/// <param name="vHour">Syslog �� �Դϴ�.</param>
		/// <param name="vMinute">Syslog �� �Դϴ�.</param>
		/// <param name="vSecond">Syslog �� �Դϴ�.</param>
		public TMSSyslogTimestamp(E_SyslogMonth vMonth, int vDay, int vHour, int vMinute, int vSecond)
		{
			m_Month = vMonth;
			m_Day = vDay;
			m_Hour = vHour;
			m_Minute = vMinute;
			m_Second = vSecond;
		}

		/// <summary>
		/// Syslog Timestamp ������ �Դϴ�.
		/// </summary>
		/// <param name="vDate">��¥ �� �ð��� ���Ե� DateTime�Դϴ�.</param>
		public TMSSyslogTimestamp(DateTime vDate)
		{
			m_Month = (E_SyslogMonth)vDate.Month;
			m_Day = vDate.Day;
			m_Hour = vDate.Hour;
			m_Minute = vDate.Minute;
			m_Second = vDate.Second;
		}

		/// <summary>
		/// Syslog ���� �������ų� �����մϴ�.
		/// </summary>
		public E_SyslogMonth Month
		{
			get { return m_Month; }
			set { m_Month = value; }				
		}

		/// <summary>
		/// Syslog ���� �������ų� �����մϴ�.
		/// </summary>
		public int Day
		{
			get { return m_Day; }
			set { m_Day = value; }
		}

		/// <summary>
		/// Syslog �ø� �������ų� �����մϴ�.
		/// </summary>
		public int Hour
		{
			get { return m_Hour; }
			set { m_Hour = value; }
		}

		/// <summary>
		/// Syslog ���� �������ų� �����մϴ�.
		/// </summary>
		public int Minute
		{
			get { return m_Minute; }
			set { m_Minute = value; }
		}

		/// <summary>
		/// Syslog �ʸ� �������ų� �����մϴ�.
		/// </summary>
		public int Second
		{
			get { return m_Second; }
			set { m_Second = value; }
		}

		/// <summary>
		/// Syslog Timestamp ���ڿ��� �������ų� �����մϴ�.
		/// </summary>
		public string TimeString
		{
			get { return MakeTimestamp(); }
			set
			{
				int tPos = 0;
				TMSSyslogTimestamp tST = ParseTimestamp(value, ref tPos);
				if(tST != null)
				{
					this.m_Month = tST.m_Month;
					this.m_Day = tST.m_Day;
					this.m_Hour = tST.m_Hour;
					this.m_Minute = tST.m_Minute;
					this.m_Second = tST.Second;
				}
			}
		}

		/// <summary>
		/// Timestamp ���ڿ��� ��ȯ �մϴ�.
		/// </summary>
		/// <returns>Timestamp ���ڿ��Դϴ�.</returns>
		public override string ToString()
		{
			return MakeTimestamp();
		}

		/// <summary>
		/// ������ Timestamp���� Timestamp���ڿ��� ����ϴ�.
		/// </summary>
		/// <param name="vMonth">�� �Դϴ�.</param>
		/// <param name="vDay">���� �Դϴ�.</param>
		/// <param name="vHour">�� �Դϴ�.</param>
		/// <param name="vMinute">�� �Դϴ�.</param>
		/// <param name="vSecond">�� �Դϴ�.</param>
		/// <returns>���ڿ��� ����� Syslog Timestamp�Դϴ�.</returns>
		public static string MakeTimestamp(E_SyslogMonth vMonth, int vDay, int vHour, int vMinute, int vSecond)
		{
			return vMonth.ToString() + " " + String.Format("00", vDay) + " " + String.Format("00", vHour) + ":" + String.Format("00", vMinute) + ":" + String.Format("00", vSecond);
		}
		/// <summary>
		/// ������ Timestamp���� Timestamp���ڿ��� ����ϴ�.
		/// </summary>
		/// <returns>���ڿ��� ����� Syslog Timestamp�Դϴ�.</returns>
		public string MakeTimestamp()
		{
			return m_Month.ToString() + " " + String.Format("00", m_Day) + " " + String.Format("00", m_Hour) + ":" + String.Format("00", m_Minute) + ":" + String.Format("00", m_Second);
		}

		/// <summary>
		/// Timestamp ���ڿ��� �м��Ͽ� Timestamp��ü�� ��ȯ �մϴ�.
		/// </summary>
		/// <param name="vTimestamp">Timestamp ���ڿ� �Դϴ�.</param>
		/// <returns>Timestamp ��ü �Դϴ�.</returns>
		/// <param name="vIndex">�м��� ������ ��ġ �Դϴ�.</param>
		public static TMSSyslogTimestamp ParseTimestamp(string vTimestamp, ref int vIndex)
		{
			TMSSyslogTimestamp tTimestamp = new TMSSyslogTimestamp();
			string tValue = "";
			int sPos = vIndex;
			int tPos = 0;
			bool tIsMatch = false;
			E_SyslogMonth tMonth = E_SyslogMonth.Jan;
			
			//���� �м� �մϴ�.			
			tPos = vTimestamp.IndexOf(" ", sPos);
			if(tPos == -1) return null;
			tValue = vTimestamp.Substring(sPos, tPos - sPos);

			if(tValue.Length != 3) return null;
			FieldInfo [] tFI = typeof(E_SyslogMonth).GetFields();

			for(int i = 1; i < tFI.Length; i++)
			{
				if(tValue == tFI[i].Name.ToString())
				{
					tIsMatch = true;
					tMonth = (E_SyslogMonth)(i-1);
					break;
				}
			}

			if(!tIsMatch) return null;			
			tTimestamp.Month = tMonth;

			//���� �м��մϴ�.
			sPos = tPos + 1;
			tPos = vTimestamp.IndexOf(" ", sPos);
			if(tPos == -1) return null;
			tValue = vTimestamp.Substring(sPos, tPos);
			if(tValue.Length != 3) return null;			
			if(TMSConvert.ToInteger32(tValue, ref tTimestamp.m_Day) != E_ConvertError.NoError) return null;

			//��, ��, �ʸ� �м��մϴ�.
			sPos = tPos + 1;
			tPos = vTimestamp.IndexOf(" ", sPos);
			if(tPos == -1) return null;
			tValue = vTimestamp.Substring(sPos, tPos);
			string [] tTime = tValue.Split(':');

			if(tTime.Length != 3) return null;

			if(TMSConvert.ToInteger32(tTime[0], ref tTimestamp.m_Hour) != E_ConvertError.NoError) return null;
			if(TMSConvert.ToInteger32(tTime[1], ref tTimestamp.m_Minute) != E_ConvertError.NoError) return null;
			if(TMSConvert.ToInteger32(tTime[2], ref tTimestamp.m_Second) != E_ConvertError.NoError) return null;

			vIndex = tPos + 1;
			return tTimestamp;
		}
	}
	#endregion //Syslog Timestamp Ŭ���� �Դϴ� -------------------------------------------------

	#region Syslog Message Ŭ���� �Դϴ� ---------------------------------------------------
	/// <summary>
	/// Syslog Message Ŭ���� �Դϴ�
	/// </summary>
	public class TMSSyslogMessage
	{
		/// <summary>
		/// Syslog Facility �������Դϴ�.
		/// </summary>
		private E_Facility m_Facility = E_Facility.LocalUse0;
		/// <summary>
		/// Syslog Severity �������Դϴ�.
		/// </summary>
		private E_Severity m_Severity = E_Severity.Error;
		/// <summary>
		/// Syslog Timestamp �������Դϴ�.
		/// </summary>
		private TMSSyslogTimestamp m_Timestamp = null;
		/// <summary>
		/// Syslog �߻� ��� IP�ּ� �Ǵ� ��� �̸��Դϴ�.
		/// </summary>
		private string m_HostName = "";
		/// <summary>
		/// Syslog �޽��� �Դϴ�.
		/// </summary>
		private string m_Message = "";
		/// <summary>
		/// Syslog Level�Դϴ�.
		/// </summary>
		internal int m_Level;
		/// <summary>
		/// Syslog�߻� ��� IP�ּ� �Դϴ�.
		/// </summary>
		private string m_IPAddress = "";
		/// <summary>
		/// Syslogó�� ���� �ڵ� �Դϴ�.
		/// </summary>
		internal E_SyslogError m_Error = E_SyslogError.NoError;

		/// <summary>
		/// Syslog�޽��� �⺻ ������ �Դϴ�.
		/// </summary>
		public TMSSyslogMessage() {}

		/// <summary>
		/// Facility���� �������ų� �����մϴ�.
		/// </summary>
		public E_Facility Facility
		{
			get { return m_Facility; }
			set 
			{ 
				m_Facility = value;
				MakeLevel();
			}
		}

		/// <summary>
		/// Severity���� �������ų� �����մϴ�.
		/// </summary>
		public E_Severity Severity
		{
			get { return m_Severity; }
			set 
			{ 
				m_Severity = value; 
				MakeLevel();
			}
		}

		/// <summary>
		/// ������ �������ų� �����մϴ�.
		/// </summary>
		public int Level
		{
			get { return m_Level; }
			set 
			{ 
				m_Level = value; 
				ParseLevel();
			}
		}

		/// <summary>
		/// �ð� ���� �������ų� �����մϴ�.
		/// </summary>
		public TMSSyslogTimestamp Timestamp
		{
			get { return m_Timestamp; }
			set { m_Timestamp = value; }
		}	

		/// <summary>
		/// ȣ��Ʈ���� �������ų� �����մϴ�.
		/// </summary>
		public string HostName
		{
			get { return m_HostName; }
			set { m_HostName = value; }
		}

		/// <summary>
		/// Syslog�޽����� �������ų� �����մϴ�.
		/// </summary>
		public string Message
		{
			get { return m_Message; }
			set { m_Message = value; }
		}

		/// <summary>
		/// �޽����� ���� �ý����� IP�ּ� �Դϴ�.
		/// </summary>
		public string IPAddress
		{
			get { return m_IPAddress; }
			set { m_IPAddress = value; }
		}

		/// <summary>
		/// Syslog ó�� ������ �����ɴϴ�. 
		/// </summary>
		public E_SyslogError Error
		{
			get { return m_Error; }
		}

		/// <summary>
		/// Level�� �м��Ͽ� Facility �� Severity�� ã���ϴ�.
		/// </summary>
		private void ParseLevel()
		{
			ParseLevel(m_Level, ref m_Facility, ref m_Severity);
		}

		/// <summary>
		/// Facility �� Severity�� �����Ͽ� Level�� ����ϴ�.
		/// </summary>
		private void MakeLevel()
		{
			m_Level = MakeLevel(m_Facility, m_Severity);
		}

		/// <summary>
		/// Level�� �м��Ͽ� Facility �� Severity�� ã���ϴ�.
		/// </summary>
		/// <param name="vLevel">�м��� Level�Դϴ�.</param>
		/// <param name="vFacility">�м��� Facility�� �Դϴ�.</param>
		/// <param name="vSeverity">�м��� Severity�� �Դϴ�.</param>
		public static void ParseLevel(int vLevel, ref E_Facility vFacility, ref E_Severity vSeverity)
		{
			vFacility = (E_Facility)((vLevel & 0xF8) >> 3);
			vSeverity = (E_Severity)(vLevel & 0x07);
		}

		/// <summary>
		/// Facility �� Severity�� �����Ͽ� Level�� ����ϴ�.
		/// </summary>
		/// <param name="vFacility">Facility�� �Դϴ�.</param>
		/// <param name="vSeverity">Severity�� �Դϴ�.</param>
		/// <returns>Level�Դϴ�.</returns>
		public static int MakeLevel(E_Facility vFacility, E_Severity vSeverity)
		{
			return (((int)vFacility) << 3) + (int)vSeverity;
		}
	}
	#endregion //Syslog Message Ŭ���� �Դϴ� ---------------------------------------------------

	#region Syslog Ŭ���� �Դϴ� -----------------------------------------------------------
	/// <summary>
	/// TMSSyslogClass�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SOSyslogClass
	{
		/// <summary>
		/// ������ Syslog���ڿ��� ��Ŷ���� Encode�մϴ�.
		/// </summary>
		/// <param name="vSyslog"></param>
		/// <returns></returns>
		public static byte [] EncodeSyslogPacket(TMSSyslogMessage vSyslog)
		{
			string tVal = "<" + vSyslog.Level.ToString() + ">";
			if(vSyslog.Timestamp != null) tVal += vSyslog.Timestamp.MakeTimestamp() + " ";
			if(vSyslog.HostName.Trim() != "") tVal += vSyslog.HostName + " ";
			tVal += vSyslog.Message;
			return System.Text.Encoding.Default.GetBytes(tVal);
		}

		/// <summary>
		/// ������ Syslog ��Ŷ�� Syslog ��ü�� Decode�մϴ�.
		/// </summary>
		/// <param name="vData"></param>
		/// <returns></returns>
		public static TMSSyslogMessage DecodeSyslogPacket(byte [] vData)
		{
			int sPos = 1, tPos = 0;
			string tVal = "";

			if(vData == null) return null;
			
			//System.Text.Encoding tEncoder = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

			TMSSyslogMessage tSyslog = new TMSSyslogMessage();
			string tData = System.Text.Encoding.Default.GetString(vData);
			//string tData = tEncoder.GetString(vData);

			//Syslog Level�� ó���մϴ�.
			if(tData[0] != '<') 
			{
				tSyslog.m_Error = E_SyslogError.UnKnownLevel;
				tSyslog.Message = tData;
				return tSyslog;
			}
			
			tPos = tData.IndexOf('>', sPos);
			if(tPos == -1)
			{
				tSyslog.m_Error = E_SyslogError.UnKnownLevel;
				tSyslog.Message = tData;
				return tSyslog;
			}			

			//������ ����մϴ�			
			tVal = tData.Substring(sPos, tPos - sPos);
			int tInt = 0;
			if(TMSConvert.ToInteger32(tVal, ref tInt) != E_ConvertError.NoError)
			{
				tSyslog.m_Error = E_SyslogError.UnKnownLevel;
				tSyslog.Message = tData;
				return tSyslog;
			}
			tSyslog.Level = tInt;

			tPos++;
			tSyslog.Timestamp = TMSSyslogTimestamp.ParseTimestamp(tData, ref tPos);
//			if(tSyslog.Timestamp == null)
//			{
//				if(tData.Length - tPos > 0)
//				{
//					tSyslog.Message = tData.Substring(tPos, tData.Length - tPos);
//				}
//				else
//				{
//					tSyslog.Message = tData;
//				}
//				return tSyslog;
//			}

			//tPos++;
			sPos = tPos;			
			tPos = tData.IndexOf(' ', sPos);
			if(tPos == -1)
			{				
				tSyslog.Message = tData;
				return tSyslog;
			}

			tSyslog.HostName = tData.Substring(sPos, tPos - sPos);
			
			tSyslog.Message = tData;
//			tPos++;
//			sPos = tPos;
//			if(tData.Length - sPos > 0)
//			{
//				tSyslog.Message = tData.Substring(sPos, tData.Length - sPos);
//			}
			return tSyslog;
		}
	}
	#endregion //Syslog Ŭ���� �Դϴ� -----------------------------------------------------------
}