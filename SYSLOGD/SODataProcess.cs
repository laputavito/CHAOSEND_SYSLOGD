using System;
using System.Runtime.InteropServices;

namespace SYSLOGD
{
	/// <summary>
	/// 변환 오류 코드입니다.
	/// </summary>
	public enum E_ConvertError
	{
		/// <summary>
		/// 정상적으로 변환되었습니다.
		/// </summary>
		NoError,
		/// <summary>
		/// 자료를 지정한 타입으로 변환 할 수 없습니다.
		/// </summary>
		CantConvert
	}

	/// <summary>
	/// 변환 처리를 담당하는 클래스 입니다.
	/// </summary>
	public class TMSConvert
	{		
		/// <summary>
		/// 지정한 문자열을 Integer형으로 변환 합니다.
		/// </summary>
		/// <param name="vString">Integer형으로 변환할 문자열입니다.</param>
		/// <param name="vInt32">변환된 Integer형입니다.</param>
		/// <returns>변환 오류 입니다.</returns>
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
		/// 지정한 문자열을 UnsignedInteger32형으로 변환 합니다.
		/// </summary>
		/// <param name="vString">UnsignedInteger32형으로 변환할 문자열입니다.</param>
		/// <param name="vUInt32">변환된 UnsignedInteger32형입니다.</param>
		/// <returns>변환 오류 입니다.</returns>
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
		/// 지정한 문자열을 UnsignedInteger64형으로 변환 합니다.
		/// </summary>
		/// <param name="vString">UnsignedInteger64형으로 변환할 문자열입니다.</param>
		/// <param name="vUInt64">변환된 UnsignedInteger64형입니다.</param>
		/// <returns>변환 오류 입니다.</returns>
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
		/// 16진수 문자열 배열을 Byte배열로 변경합니다.
		/// </summary>
		/// <param name="vStr">Byte배열로 변경할 16진수 문자열 배열입니다.</param>
		/// <returns>변환된 Byte배열 입니다.</returns>
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
		/// 16진수 문자열 배열을 Byte배열로 변경합니다.
		/// </summary>
		/// <param name="vStr">Byte배열로 변경할 16진수 문자열 배열입니다.</param>
		/// <returns>변환된 Byte배열 입니다.</returns>
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
		/// 구조체를 Byte배열로 변경합니다.
		/// </summary>
		/// <param name="vStruct">Byte배열로 변경할 구조체입니다.</param>
		/// <returns>변경한 구조체의 데이터의 Byte배열입니다.</returns>
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
		/// Byte배열을 구조체로 변경합니다.
		/// </summary>
		/// <param name="vBytes">구조체로 변경할 데이터가 들어있는 Byte배열입니다.</param>
		/// <param name="vStruct">변경한 구조체의 데이터가 들어갈 배열입니다.</param>
		public static void BytesToStruct(Byte[] vBytes, ref Object vStruct)
		{
			int tSize = vBytes.Length;
			IntPtr tPtr = Marshal.AllocCoTaskMem(tSize);
			Marshal.Copy(vBytes, 0, tPtr, tSize);
			vStruct = Marshal.PtrToStructure(tPtr, vStruct.GetType());
		}

		
	}
}
