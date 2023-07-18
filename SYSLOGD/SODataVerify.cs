using System;
using System.Diagnostics;

namespace SYSLOGD
{
	/// <summary>
	/// ������ �˻� Ŭ���� �Դϴ�.
	/// </summary>
	public class TMSVerify
	{
		/// <summary>
		/// ������ ���ڿ��� e-MailAddress���� �˻��մϴ�.
		/// </summary>
		/// <param name="vEMailAddress">�˻��� e-MailAddress���ڿ� �Դϴ�.</param>
		/// <returns>���ڿ��� e-MailAddress������ ���� �Դϴ�.</returns>
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
		/// ������ ���ڿ��� �������� �˻��մϴ�.
		/// </summary>
		/// <param name="tString">�������� �˻� �� ���ڿ��Դϴ�.</param>
		/// <returns>���ڿ��� �����̸� true, ���ڿ��� ���ڰ� �ƴϸ� false�� ��ȯ�մϴ�.</returns>
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