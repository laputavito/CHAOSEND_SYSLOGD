using System;
using System.Diagnostics;

namespace SYSLOGD
{
	/// <summary>
	/// 데이터 검사 클래스 입니다.
	/// </summary>
	public class TMSVerify
	{
		/// <summary>
		/// 지정한 문자열이 e-MailAddress인지 검사합니다.
		/// </summary>
		/// <param name="vEMailAddress">검사할 e-MailAddress문자열 입니다.</param>
		/// <returns>문자열이 e-MailAddress인지의 여부 입니다.</returns>
		public static bool isEMailAddress(string vEMailAddress)
		{
			try
			{
				if(vEMailAddress.IndexOf('@') < 0) return false;
				if(vEMailAddress.IndexOf('.') < 0) return false;
				return true;
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Error - isEMailAddress : " + ex.ToString());
				return false;
			}
		}

		/// <summary>
		/// 지정한 문자열이 숫자인지 검사합니다.
		/// </summary>
		/// <param name="tString">숫자인지 검사 할 문자열입니다.</param>
		/// <returns>문자열이 숫자이면 true, 문자열이 숫자가 아니면 false를 반환합니다.</returns>
		public static bool isNumeric(string tString)
		{
			try
			{
				decimal.Parse(tString);
				return true;
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Error - isNumeric : " + ex.ToString());
				return false;
			}
		}
	}
}