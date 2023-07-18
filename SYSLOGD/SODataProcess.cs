using System;
using System.Runtime.InteropServices;

namespace SYSLOGD
{
	/// <summary>
	/// ��ȯ ���� �ڵ��Դϴ�.
	/// </summary>
	public enum E_ConvertError
	{
		/// <summary>
		/// ���������� ��ȯ�Ǿ����ϴ�.
		/// </summary>
		NoError,
		/// <summary>
		/// �ڷḦ ������ Ÿ������ ��ȯ �� �� �����ϴ�.
		/// </summary>
		CantConvert
	}

	/// <summary>
	/// ��ȯ ó���� ����ϴ� Ŭ���� �Դϴ�.
	/// </summary>
	public class TMSConvert
	{		
		/// <summary>
		/// ������ ���ڿ��� Integer������ ��ȯ �մϴ�.
		/// </summary>
		/// <param name="vString">Integer������ ��ȯ�� ���ڿ��Դϴ�.</param>
		/// <param name="vInt32">��ȯ�� Integer���Դϴ�.</param>
		/// <returns>��ȯ ���� �Դϴ�.</returns>
		public static E_ConvertError ToInteger32(string vString, ref int vInt32)
		{			
			try
			{
				vInt32 = int.Parse(vString);
				return E_ConvertError.NoError;
			}
			catch(Exception ex)
			{
				ex=ex;
				return E_ConvertError.CantConvert;
			}
		}

		/// <summary>
		/// ������ ���ڿ��� UnsignedInteger32������ ��ȯ �մϴ�.
		/// </summary>
		/// <param name="vString">UnsignedInteger32������ ��ȯ�� ���ڿ��Դϴ�.</param>
		/// <param name="vUInt32">��ȯ�� UnsignedInteger32���Դϴ�.</param>
		/// <returns>��ȯ ���� �Դϴ�.</returns>
		public static E_ConvertError ToUnsignedInteger32(string vString, ref UInt32 vUInt32)
		{
			try
			{
				vUInt32 = UInt32.Parse(vString);
				return E_ConvertError.NoError;
			}
			catch(Exception ex)
			{
				ex=ex;
				return E_ConvertError.CantConvert;
			}
		}

		/// <summary>
		/// ������ ���ڿ��� UnsignedInteger64������ ��ȯ �մϴ�.
		/// </summary>
		/// <param name="vString">UnsignedInteger64������ ��ȯ�� ���ڿ��Դϴ�.</param>
		/// <param name="vUInt64">��ȯ�� UnsignedInteger64���Դϴ�.</param>
		/// <returns>��ȯ ���� �Դϴ�.</returns>
		public static E_ConvertError ToUnsignedInteger64(string vString, ref UInt64 vUInt64)
		{
			try
			{
				vUInt64 = UInt64.Parse(vString);
				return E_ConvertError.NoError;
			}
			catch(Exception ex)
			{
				ex=ex;
				return E_ConvertError.CantConvert;
			}
		}

		/// <summary>
		/// 16���� ���ڿ� �迭�� Byte�迭�� �����մϴ�.
		/// </summary>
		/// <param name="vStr">Byte�迭�� ������ 16���� ���ڿ� �迭�Դϴ�.</param>
		/// <returns>��ȯ�� Byte�迭 �Դϴ�.</returns>
		public static byte [] HexaStringToBytes(string [] vStr)
		{
			byte [] m_DD = new byte [vStr.Length];
			
			int tM = 0;
			for(int i = 0; i < vStr.Length; i++)
			{
				try
				{
					tM = 0;
					tM = Uri.FromHex(vStr[i][0]);
					tM *= 16;
					tM += Uri.FromHex(vStr[i][1]);
				}
				catch(Exception ex)
				{
					ex = ex;
				}
				finally
				{
					m_DD[i] = (byte)tM;
				}
			}
			return m_DD;
		}

		/// <summary>
		/// 16���� ���ڿ� �迭�� Byte�迭�� �����մϴ�.
		/// </summary>
		/// <param name="vStr">Byte�迭�� ������ 16���� ���ڿ� �迭�Դϴ�.</param>
		/// <returns>��ȯ�� Byte�迭 �Դϴ�.</returns>
		public static byte [] HexaStringToBytes(string vStr)
		{
			byte [] m_DD = new byte [(int)Math.Round(vStr.Length / 2.0)];		
			int tIndex = 0;
			int tM = 0;
			for(int i = 0; i < vStr.Length; i+=2)
			{
				try
				{
					tM = 0;
					tM = Uri.FromHex(vStr[i]);
					tM *= 16;
					tM += Uri.FromHex(vStr[i+1]);
				}
				catch(Exception ex)
				{
					ex=ex;
				}
				finally
				{
					m_DD[tIndex] = (byte)tM;
					tIndex++;
				}
			}
			return m_DD;
		}
		
		/// <summary>
		/// ����ü�� Byte�迭�� �����մϴ�.
		/// </summary>
		/// <param name="vStruct">Byte�迭�� ������ ����ü�Դϴ�.</param>
		/// <returns>������ ����ü�� �������� Byte�迭�Դϴ�.</returns>
		public static byte[] StructToBytes(Object vStruct)
		{
			int tSize = Marshal.SizeOf(vStruct.GetType());
			byte [] tBytes = new byte [tSize];
			IntPtr tPtr = Marshal.AllocCoTaskMem(tSize);
			Marshal.StructureToPtr(vStruct, tPtr, false);
			Marshal.Copy(tPtr, tBytes, 0, tSize);
			return tBytes;
		}

		/// <summary>
		/// Byte�迭�� ����ü�� �����մϴ�.
		/// </summary>
		/// <param name="vBytes">����ü�� ������ �����Ͱ� ����ִ� Byte�迭�Դϴ�.</param>
		/// <param name="vStruct">������ ����ü�� �����Ͱ� �� �迭�Դϴ�.</param>
		public static void BytesToStruct(Byte[] vBytes, ref Object vStruct)
		{
			int tSize = vBytes.Length;
			IntPtr tPtr = Marshal.AllocCoTaskMem(tSize);
			Marshal.Copy(vBytes, 0, tPtr, tSize);
			vStruct = Marshal.PtrToStructure(tPtr, vStruct.GetType());
		}

		
	}
}
