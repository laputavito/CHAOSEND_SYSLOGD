using System;
using System.Reflection;

namespace SYSLOGD
{
	#region Syslog관련 열거형입니다 --------------------------------------------------------
	/// <summary>
	/// Syslog Facility 열거형입니다.
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
	/// Syslog Severity 열거형입니다.
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
	/// Syslog 달 열거형입니다.
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
	/// Syslog Error 열거형입니다.
	/// </summary>
	[Serializable]
	public enum E_SyslogError
	{
		NoError,
		UnKnownLevel
	}
	#endregion //Syslog관련 열거형입니다 --------------------------------------------------------

	#region Syslog Timestamp 클래스 입니다 -------------------------------------------------
	/// <summary>
	/// Syslog Timestamp 클래스 입니다.
	/// </summary>
	public class TMSSyslogTimestamp
	{
		/// <summary>
		/// Syslog월 입니다.
		/// </summary>
		private E_SyslogMonth m_Month = E_SyslogMonth.Jan;
		/// <summary>
		/// Syslog 일 입니다.
		/// </summary>
		private int m_Day = 0;
		/// <summary>
		/// Syslog 시 입니다.
		/// </summary>
		private int m_Hour = 0;
		/// <summary>
		/// Syslog 분 입니다.
		/// </summary>
		private int m_Minute = 0;
		/// <summary>
		/// Syslog 초 입니다.
		/// </summary>
		private int m_Second = 0;

		/// <summary>
		/// Syslog Timestamp기본 생성자 입니다.
		/// </summary>
		public TMSSyslogTimestamp() { }

		/// <summary>
		/// Syslog Timestamp 생성자 입니다.
		/// </summary>
		/// <param name="vMonth">Syslog 월 입니다.</param>
		/// <param name="vDay">Syslog 일 입니다.</param>
		/// <param name="vHour">Syslog 시 입니다.</param>
		/// <param name="vMinute">Syslog 분 입니다.</param>
		/// <param name="vSecond">Syslog 초 입니다.</param>
		public TMSSyslogTimestamp(E_SyslogMonth vMonth, int vDay, int vHour, int vMinute, int vSecond)
		{
			m_Month = vMonth;
			m_Day = vDay;
			m_Hour = vHour;
			m_Minute = vMinute;
			m_Second = vSecond;
		}

		/// <summary>
		/// Syslog Timestamp 생성자 입니다.
		/// </summary>
		/// <param name="vDate">날짜 및 시간이 포함된 DateTime입니다.</param>
		public TMSSyslogTimestamp(DateTime vDate)
		{
			m_Month = (E_SyslogMonth)vDate.Month;
			m_Day = vDate.Day;
			m_Hour = vDate.Hour;
			m_Minute = vDate.Minute;
			m_Second = vDate.Second;
		}

		/// <summary>
		/// Syslog 월을 가져오거나 설정합니다.
		/// </summary>
		public E_SyslogMonth Month
		{
			get { return m_Month; }
			set { m_Month = value; }				
		}

		/// <summary>
		/// Syslog 일을 가져오거나 설정합니다.
		/// </summary>
		public int Day
		{
			get { return m_Day; }
			set { m_Day = value; }
		}

		/// <summary>
		/// Syslog 시를 가져오거나 설정합니다.
		/// </summary>
		public int Hour
		{
			get { return m_Hour; }
			set { m_Hour = value; }
		}

		/// <summary>
		/// Syslog 분을 가져오거나 설정합니다.
		/// </summary>
		public int Minute
		{
			get { return m_Minute; }
			set { m_Minute = value; }
		}

		/// <summary>
		/// Syslog 초를 가져오거나 설정합니다.
		/// </summary>
		public int Second
		{
			get { return m_Second; }
			set { m_Second = value; }
		}

		/// <summary>
		/// Syslog Timestamp 문자열을 가져오거나 설정합니다.
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
		/// Timestamp 문자열을 반환 합니다.
		/// </summary>
		/// <returns>Timestamp 문자열입니다.</returns>
		public override string ToString()
		{
			return MakeTimestamp();
		}

		/// <summary>
		/// 지정한 Timestamp값을 Timestamp문자열로 만듭니다.
		/// </summary>
		/// <param name="vMonth">달 입니다.</param>
		/// <param name="vDay">일자 입니다.</param>
		/// <param name="vHour">시 입니다.</param>
		/// <param name="vMinute">분 입니다.</param>
		/// <param name="vSecond">초 입니다.</param>
		/// <returns>문자열로 변경된 Syslog Timestamp입니다.</returns>
		public static string MakeTimestamp(E_SyslogMonth vMonth, int vDay, int vHour, int vMinute, int vSecond)
		{
			return vMonth.ToString() + " " + String.Format("00", vDay) + " " + String.Format("00", vHour) + ":" + String.Format("00", vMinute) + ":" + String.Format("00", vSecond);
		}
		/// <summary>
		/// 지정한 Timestamp값을 Timestamp문자열로 만듭니다.
		/// </summary>
		/// <returns>문자열로 변경된 Syslog Timestamp입니다.</returns>
		public string MakeTimestamp()
		{
			return m_Month.ToString() + " " + String.Format("00", m_Day) + " " + String.Format("00", m_Hour) + ":" + String.Format("00", m_Minute) + ":" + String.Format("00", m_Second);
		}

		/// <summary>
		/// Timestamp 문자열을 분석하여 Timestamp객체를 반환 합니다.
		/// </summary>
		/// <param name="vTimestamp">Timestamp 문자열 입니다.</param>
		/// <returns>Timestamp 객체 입니다.</returns>
		/// <param name="vIndex">분석을 시작할 위치 입니다.</param>
		public static TMSSyslogTimestamp ParseTimestamp(string vTimestamp, ref int vIndex)
		{
			TMSSyslogTimestamp tTimestamp = new TMSSyslogTimestamp();
			string tValue = "";
			int sPos = vIndex;
			int tPos = 0;
			bool tIsMatch = false;
			E_SyslogMonth tMonth = E_SyslogMonth.Jan;
			
			//달을 분석 합니다.			
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

			//일을 분석합니다.
			sPos = tPos + 1;
			tPos = vTimestamp.IndexOf(" ", sPos);
			if(tPos == -1) return null;
			tValue = vTimestamp.Substring(sPos, tPos);
			if(tValue.Length != 3) return null;			
			if(TMSConvert.ToInteger32(tValue, ref tTimestamp.m_Day) != E_ConvertError.NoError) return null;

			//시, 분, 초를 분석합니다.
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
	#endregion //Syslog Timestamp 클래스 입니다 -------------------------------------------------

	#region Syslog Message 클래스 입니다 ---------------------------------------------------
	/// <summary>
	/// Syslog Message 클래스 입니다
	/// </summary>
	public class TMSSyslogMessage
	{
		/// <summary>
		/// Syslog Facility 열거형입니다.
		/// </summary>
		private E_Facility m_Facility = E_Facility.LocalUse0;
		/// <summary>
		/// Syslog Severity 열거형입니다.
		/// </summary>
		private E_Severity m_Severity = E_Severity.Error;
		/// <summary>
		/// Syslog Timestamp 열거형입니다.
		/// </summary>
		private TMSSyslogTimestamp m_Timestamp = null;
		/// <summary>
		/// Syslog 발생 장비 IP주소 또는 장비 이름입니다.
		/// </summary>
		private string m_HostName = "";
		/// <summary>
		/// Syslog 메시지 입니다.
		/// </summary>
		private string m_Message = "";
		/// <summary>
		/// Syslog Level입니다.
		/// </summary>
		internal int m_Level;
		/// <summary>
		/// Syslog발생 장비 IP주소 입니다.
		/// </summary>
		private string m_IPAddress = "";
		/// <summary>
		/// Syslog처리 오류 코드 입니다.
		/// </summary>
		internal E_SyslogError m_Error = E_SyslogError.NoError;

		/// <summary>
		/// Syslog메시지 기본 생성자 입니다.
		/// </summary>
		public TMSSyslogMessage() {}

		/// <summary>
		/// Facility값을 가져오거나 설정합니다.
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
		/// Severity값을 가져오거나 설정합니다.
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
		/// 레벨을 가져오거나 설정합니다.
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
		/// 시간 값을 가져오거나 설정합니다.
		/// </summary>
		public TMSSyslogTimestamp Timestamp
		{
			get { return m_Timestamp; }
			set { m_Timestamp = value; }
		}	

		/// <summary>
		/// 호스트명을 가져오거나 설정합니다.
		/// </summary>
		public string HostName
		{
			get { return m_HostName; }
			set { m_HostName = value; }
		}

		/// <summary>
		/// Syslog메시지를 가져오거나 설정합니다.
		/// </summary>
		public string Message
		{
			get { return m_Message; }
			set { m_Message = value; }
		}

		/// <summary>
		/// 메시지를 보낸 시스템의 IP주소 입니다.
		/// </summary>
		public string IPAddress
		{
			get { return m_IPAddress; }
			set { m_IPAddress = value; }
		}

		/// <summary>
		/// Syslog 처리 오류를 가져옵니다. 
		/// </summary>
		public E_SyslogError Error
		{
			get { return m_Error; }
		}

		/// <summary>
		/// Level을 분석하여 Facility 와 Severity를 찾습니다.
		/// </summary>
		private void ParseLevel()
		{
			ParseLevel(m_Level, ref m_Facility, ref m_Severity);
		}

		/// <summary>
		/// Facility 와 Severity를 조합하여 Level을 만듭니다.
		/// </summary>
		private void MakeLevel()
		{
			m_Level = MakeLevel(m_Facility, m_Severity);
		}

		/// <summary>
		/// Level을 분석하여 Facility 와 Severity를 찾습니다.
		/// </summary>
		/// <param name="vLevel">분석할 Level입니다.</param>
		/// <param name="vFacility">분석된 Facility값 입니다.</param>
		/// <param name="vSeverity">분석된 Severity값 입니다.</param>
		public static void ParseLevel(int vLevel, ref E_Facility vFacility, ref E_Severity vSeverity)
		{
			vFacility = (E_Facility)((vLevel & 0xF8) >> 3);
			vSeverity = (E_Severity)(vLevel & 0x07);
		}

		/// <summary>
		/// Facility 와 Severity를 조합하여 Level을 만듭니다.
		/// </summary>
		/// <param name="vFacility">Facility값 입니다.</param>
		/// <param name="vSeverity">Severity값 입니다.</param>
		/// <returns>Level입니다.</returns>
		public static int MakeLevel(E_Facility vFacility, E_Severity vSeverity)
		{
			return (((int)vFacility) << 3) + (int)vSeverity;
		}
	}
	#endregion //Syslog Message 클래스 입니다 ---------------------------------------------------

	#region Syslog 클래스 입니다 -----------------------------------------------------------
	/// <summary>
	/// TMSSyslogClass에 대한 요약 설명입니다.
	/// </summary>
	public class SOSyslogClass
	{
		/// <summary>
		/// 지정한 Syslog문자열을 패킷으로 Encode합니다.
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
		/// 지정한 Syslog 패킷을 Syslog 객체로 Decode합니다.
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

			//Syslog Level을 처리합니다.
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

			//레벨을 얻기합니다			
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
	#endregion //Syslog 클래스 입니다 -----------------------------------------------------------
}