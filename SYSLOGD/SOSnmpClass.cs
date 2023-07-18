using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace SYSLOGD
{
	#region SNMP 관련 열거형 입니다 --------------------------------------------------------
	/// <summary>
	/// SNMP 버젼 열거형 입니다.
	/// </summary>
	[Serializable]
	public enum E_SnmpVersion
	{
		/// <summary>
		/// Snmp Version 1 입니다.
		/// </summary>
		Version1 = 0x00,
		/// <summary>
		/// Snmp Version 2c 입니다.
		/// </summary>
		Version2c = 0x01,
		/// <summary>
		/// Snmp Version 3 입니다.
		/// </summary>
		Version3 = 0x03
	}

	/// <summary>
	/// SNMP PDU Type 열거형 입니다.
	/// </summary>
	[Serializable]
	public enum E_SnmpPDUType
	{
		/// <summary>
		/// Snmp GetRequest 입니다.
		/// </summary>
		GetRequest = 0xA0,
		/// <summary>
		/// Snmp GetNextRequest 입니다.
		/// </summary>
		GetNextRequest = 0xA1,
		/// <summary>
		/// Snmp GetResponse 입니다.
		/// </summary>
		GetResponse = 0xA2,
		/// <summary>
		/// Snmp SetRequest 입니다.
		/// </summary>
		SetRequest = 0xA3,
		/// <summary>
		/// Snmp Trap Version 1 입니다.
		/// </summary>
		TrapV1 = 0xA4,
		/// <summary>
		/// Snmp GetBulkRequest 입니다.
		/// </summary>
		GetBulkRequest = 0xA5,
		/// <summary>
		/// Snmp InformRequest 입니다.
		/// </summary>
		InformRequest = 0xA6,
		/// <summary>
		/// Snmp Trap Version 2 입니다.
		/// </summary>
		TrapV2 = 0xA7,
		/// <summary>
		/// Snmp Report 입니다.
		/// </summary>
		Report = 0xA8
	}

	/// <summary>
	/// SNMP정보 데이터 타입 열거형 나타냅니다.
	/// </summary>
	[Serializable]
	public enum E_SnmpDataType
	{		
		/// <summary>
		/// 4Byte Integer형입니다.
		/// </summary>
		Integer = 0x02,
		/// <summary>
		/// 4Byte Integer형입니다.
		/// </summary>
		Integer32 = 0x02,
		/// <summary>
		/// 문자열 형입니다.
		/// </summary>
		OctetString = 0x04,
		/// <summary>
		/// Null 형입니다.
		/// </summary>
		Null = 0x05,
		/// <summary>
		/// ObjectIdentifier 형입니다.
		/// </summary>
		ObjectIdentifier = 0x06,
		/// <summary>
		/// Sequence 형입니다.
		/// </summary>
		Sequence = 0x30,
		/// <summary>
		/// Sequence Of 형입니다.
		/// </summary>
		SequenceOf = 0x30,
		/// <summary>
		/// IP주소 형입니다.
		/// </summary>
		IPAddress = 0x40,
		/// <summary>
		/// 4Byte Integer형입니다.
		/// </summary>
		Counter = 0x41,
		/// <summary>
		/// 4Byte Integer형입니다.
		/// </summary>
		Counter32 = 0x41,
		/// <summary>
		/// 4Byte Integer형입니다.
		/// </summary>
		Gauge = 0x42,
		/// <summary>
		/// 4Byte Integer형입니다.
		/// </summary>
		Gauge32 = 0x42,
		/// <summary>
		/// TimeTicks형입니다.
		/// </summary>
		TimeTicks = 0x43,
		/// <summary>
		/// Opaque형입니다.
		/// </summary>
		Opaque = 0x44,
		/// <summary>
		/// 8Byte Integer 배열 형입니다.
		/// </summary>
		Counter64 = 0x46,			//Snmp ver 2	
	
		//다음은 Library에서 추가로 지원하는 형입니다.
		/// <summary>
		/// MAC Address형 입니다.(AA:BB:CC:20:2C:AC);
		/// </summary>
		MACAddress = 0xFF01,
		/// <summary>
		/// 날짜 및 시간 형입니다.
		/// </summary>
		DateAndTime = 0xFF02
	}

	/// <summary>
	/// SNMP 오류 상태 코드 열거형 입니다.
	/// </summary>
	[Serializable]
	public enum E_SnmpErrorStatus
	{
		/// <summary>
		/// No error occurred. This code is also used in all request PDUs, since they have no error status to report.
		/// </summary>
		noError	= 0,
		/// <summary>
		/// The size of the Response-PDU would be too large to transport.
		/// </summary>
		tooBig,
		/// <summary>
		/// The name of a requested object was not found.
		/// </summary>
		noSuchName,
		/// <summary>
		/// A value in the request didn't match the structure that the recipient of the request had for the object. For example, an object in the request was specified with an incorrect length or type.
		/// </summary>
		badValue,
		/// <summary>
		/// An attempt was made to set a variable that has an Access value indicating that it is read-only.
		/// </summary>
		readOnly,
		/// <summary>
		/// An error occurred other than one indicated by a more specific error code in this table.
		/// </summary>
		genErr,
		/// <summary>
		/// Access was denied to the object for security reasons.
		/// </summary>
		noAccess,
		/// <summary>
		/// The object type in a variable binding is incorrect for the object.
		/// </summary>
		wrongType,
		/// <summary>
		/// A variable binding specifies a length incorrect for the object.
		/// </summary>
		wrongLength,
		/// <summary>
		/// A variable binding specifies an encoding incorrect for the object.
		/// </summary>
		wrongEncoding,
		/// <summary>
		/// The value given in a variable binding is not possible for the object.
		/// </summary>
		wrongValue,
		/// <summary>
		/// A specified variable does not exist and cannot be created.
		/// </summary>
		noCreation,
		/// <summary>
		/// A variable binding specifies a value that could be held by the variable but cannot be assigned to it at this time.
		/// </summary>
		inconsistentValue,
		/// <summary>
		/// An attempt to set a variable required a resource that is not available.
		/// </summary>
		resourceUnavailable,
		/// <summary>
		/// An attempt to set a particular variable failed.
		/// </summary>
		commitFailed,
		/// <summary>
		/// An attempt to set a particular variable as part of a group of variables failed, and the attempt to then undo the setting of other variables was not successful.
		/// </summary>
		undoFailed,
		/// <summary>
		/// A problem occurred in authorization.
		/// </summary>
		authorizationError,
		/// <summary>
		/// The variable cannot be written or created.
		/// </summary>
		notWritable,
		/// <summary>
		/// inconsistentName
		/// </summary>
		inconsistentName
	}

	/// <summary>
	/// Standard Trap타입 열거형 입니다.
	/// </summary>
	[Serializable]
	public enum E_GenericTrapType
	{
		/// <summary>
		/// 장비의 전원이 꺼졌다 켜졌을 경우 발생합니다.
		/// </summary>
		ColdStart,
		/// <summary>
		/// 장비를 재시작하였을경우 발생합니다.
		/// </summary>
		WarmStart,
		/// <summary>
		/// 장비의 포트가 다운되었을경우 발생합니다.
		/// </summary>
		LinkDown,
		/// <summary>
		/// 장비의 포트가 업되었을경우 발생합니다.
		/// </summary>
		LinkUp,
		/// <summary>
		/// 장비에 접속시 권한이 없을경우 발생합니다.
		/// </summary>
		AuthenticationFailure,
		/// <summary>
		/// BGP와 관련된 트랩입니다.
		/// </summary>
		EgpNeighborLoss,
		/// <summary>
		/// 제조회사에서 만든 트랩입니다.
		/// </summary>
		EnterpriseSpecific,		
	}		

	/// <summary>
	/// 분석 Error 열거형 입니다.
	/// </summary>
	[Serializable]
	public enum E_ParseError
	{
		/// <summary>
		/// 분석중 에러가 발생하지 않았습니다.
		/// </summary>
		NoError = 0,
		/// <summary>
		/// SNMP 패킷이 잘못되었습니다.
		/// </summary>
		IncorrectPacket
	}

	/// <summary>
	/// SNMP V3 인증 프로토콜 열거형입니다.
	/// </summary>
	[Serializable]
	public enum E_SnmpV3AuthenticationProtocol
	{
		/// <summary>
		/// 인증 프로토콜을 사용하지 않습니다.
		/// </summary>
		None,
		/// <summary>
		/// 인증 프로토콜로 HMAC_MD5를 사용합니다.
		/// </summary>
		HMAC_MD5,
		/// <summary>
		/// 인증 프로토콜로 HMAC_SHA를 사용합니다.
		/// </summary>
		HMAC_SHA
	}

	/// <summary>
	/// SNMP V3 프라이버시 프로토콜 열거형입니다.
	/// </summary>
	[Serializable]
	public enum E_SnmpV3PrivacyProtocol
	{
		/// <summary>
		/// 프라이버시 프로토콜을 사용하지 않습니다.
		/// </summary>
		None,
		/// <summary>
		/// 프라이버시 프로토콜 CBC_DES를 사용합니다.
		/// </summary>
		CBC_DES,
		/// <summary>
		/// 프라이버시 프로토콜 CFB_AES_128을 사용합니다.
		/// </summary>
		CFB_AES_128
	}
	#endregion //SNMP 관련 열거형 입니다 --------------------------------------------------------

	/// <summary>
	/// SNMP 데이터 분석 클래스 입니다.
	/// </summary>
	public class SOSnmpClass
	{
		#region SNMP 관련 기본 정의 입니다 -------------------------------------------------
		/// <summary>
		/// Snmp Version2 Trap의 종류를 나타내는 OID입니다.
		/// </summary>
		public static string [] A_SnmpV2TrapOID = new string []
		{
			"1.3.6.1.6.3.1.1.4.1.0",	//GenericTrap
			"1.3.6.1.6.3.1.1.4.3.0"		//EnterpriseTrap
		};

		/// <summary>
		/// Snmp Version2 TrapType을 나타내는 OID입니다. 
		/// </summary>
		public static string [] A_SnmpV2GenericTrapOID = new string []
		{
			"1.3.6.1.6.3.1.1.5.1",	//ColdStart
			"1.3.6.1.6.3.1.1.5.2",	//WarmStart
			"1.3.6.1.6.3.1.1.5.3",	//LinkDown
			"1.3.6.1.6.3.1.1.5.4",	//LinkUp
			"1.3.6.1.6.3.1.1.5.5"	//AuthticationFailure
		};
		#endregion //SNMP 관련 기본 정의 입니다 -------------------------------------------------

		#region SNMP Decode 부분 입니다 ----------------------------------------------------
		/// <summary>
		/// SNMP PDU를 분석합니다.
		/// </summary>
		/// <param name="vData">분석할 SNMP데이터 입니다.</param>
		/// <returns>분석된 결과를 반환 합니다.</returns>
		public static ITMSSnmpPDU DecodeSnmpPDU(byte [] vData)
		{
			ITMSSnmpPDU tPDU = null;

			E_SnmpVersion tVersion = E_SnmpVersion.Version1;

			//최초 Byte가 Sequence인지 확인 합니다.
			if((E_SnmpDataType)vData[0] != E_SnmpDataType.Sequence)
			{
				return null;
			}

			int tIndex = 1;
			int tPDULength = DecodeLength(vData, ref tIndex);
			
			//SNMP Version을 확인합니다.
			if((E_SnmpDataType)vData[tIndex] != E_SnmpDataType.Integer)
			{
				return null;
			}
			tIndex++;
			tVersion = (E_SnmpVersion)DecodeInteger32(vData, ref tIndex);

			switch(tVersion)
			{
				case E_SnmpVersion.Version1:	//PDU가 SNMP V1인경우
				case E_SnmpVersion.Version2c:	//PDU가 SNmp V2c인 경우					
					tPDU = DecodeSnmp_PDU(tVersion, vData, tIndex);
					break;

				case E_SnmpVersion.Version3:	//PDU가 SNmp V3인 경우					
					break;
			}
			return tPDU;
		}
		
		/// <summary>
		/// SNMP Version1, 2 PDU를 분석합니다.
		/// </summary>
		/// <param name="vVersion">Snmp Version입니다.</param>
		/// <param name="vData">Decode할 데이터 입니다.</param>
		/// <param name="vIndex">Deocde를 시작할 위치입니다.</param>
		/// <returns>Decode된 PDU객체 입니다.</returns>
		private static ITMSSnmpPDU DecodeSnmp_PDU(E_SnmpVersion vVersion, byte [] vData, int vIndex)
		{
			ITMSSnmpPDU tPDU = null;

			string tCommunity = "";
			E_SnmpPDUType tPDUType = E_SnmpPDUType.TrapV1;

			//SNMP Community를 확인합니다.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.OctetString)
			{
				return null;
			}
			vIndex++;

			byte [] tRD = null;
			tCommunity = DecodeOctetString(vData, ref vIndex, ref tRD);

			//SNMP PDU Type을 얻습니다.
			tPDUType = (E_SnmpPDUType)vData[vIndex];
			vIndex++;

			switch(tPDUType)
			{				
				case E_SnmpPDUType.GetResponse:
					TMSSnmp_GetResponsePDU tGetResponsePDU = new TMSSnmp_GetResponsePDU(vVersion);
					tGetResponsePDU.Community = tCommunity;
					
					DecodeSnmp_GetResponse(vData, vIndex, ref tGetResponsePDU);

					tPDU = tGetResponsePDU;
					break;

				case E_SnmpPDUType.TrapV1:				
					TMSSnmp_TrapPDU tTrapPDU = new TMSSnmp_TrapPDU(vVersion);
					tTrapPDU.Community = tCommunity;

					DecodeSnmp_Trap(vData, vIndex, ref tTrapPDU);

					tPDU = tTrapPDU;
					break;

				case E_SnmpPDUType.TrapV2:
					TMSSnmpV2_TrapPDU tV2TrapPDU = new TMSSnmpV2_TrapPDU(vVersion);
					tV2TrapPDU.Community = tCommunity;
					
					TMSSnmp_GetResponsePDU tV = (TMSSnmp_GetResponsePDU)tV2TrapPDU;
					DecodeSnmp_GetResponse(vData, vIndex, ref tV);

					//SnmpV2 Generic Trap인경우
					if(tV2TrapPDU.Variables[1].ObjectID.IndexOf(A_SnmpV2TrapOID[0]) > -1)
					{
						int i = 0;
						for(i = 0; i < A_SnmpV2GenericTrapOID.Length; i++)
						{
							if(tV2TrapPDU.Variables[1].Value.ToString().IndexOf(A_SnmpV2GenericTrapOID[i]) > -1)
							{
								tV2TrapPDU.TrapType = (E_GenericTrapType)i;
								break;
							}
						}
						if(i == A_SnmpV2GenericTrapOID.Length)
						{
							tV2TrapPDU.TrapType = E_GenericTrapType.EnterpriseSpecific;
						}
					}
					else
					{
						//SnmpV2 Enterprise Trap인경우
						if(tV2TrapPDU.Variables[1].ObjectID.IndexOf(A_SnmpV2TrapOID[1]) > -1)
						{
							tV2TrapPDU.TrapType = E_GenericTrapType.EnterpriseSpecific;
						}
					}

					tPDU = tV2TrapPDU;
					break;
			}
			return tPDU;
		}

		/// <summary>
		/// SNMP Version1, 2 Trap을 분석합니다.
		/// </summary>
		/// <param name="vData">Decode할 데이터 입니다.</param>
		/// <param name="vIndex">Deocde를 시작할 위치입니다.</param>
		/// <param name="vValueObj">Decode결과가 저장될 Trap PDU객체 입니다.</param>
		internal static void DecodeSnmp_Trap(byte [] vData, int vIndex, ref TMSSnmp_TrapPDU vValueObj)
		{
			//Trap PDU의 길이를 얻습니다.
			int tLength = DecodeLength(vData, ref vIndex);

			//OID인지 확인하고 Enterprise OID를 읽기 합니다.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.ObjectIdentifier)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.Enterprise = DecodeObjectID(vData, ref vIndex);

			//Agent주소를 얻습니다.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.IPAddress)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.AgentIPAddress = DecodeIPAddress(vData, ref vIndex);

			//Trap Type을 얻습니다.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Integer)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.TrapType = (E_GenericTrapType)DecodeInteger32(vData, ref vIndex);

			//Specific Code를 얻습니다.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Integer)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.SpecificCode = DecodeInteger32(vData, ref vIndex);

			//TimeTicks값을 얻습니다.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.TimeTicks)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.TimeTick = DecodeInteger32(vData, ref vIndex);

			//Sequence인지 확인 합니다.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Sequence)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			tLength = DecodeLength(vData, ref vIndex);
			
			if(tLength > 0)
			{
				DecodeVariables(vData, vIndex, tLength,  ref vValueObj.m_Variables);
			}				
		}

		/// <summary>
		/// SNMP V1, V2 GetResponse PDU를 분석합니다.
		/// </summary>
		/// <param name="vData">분석할 SNMP데이터가 들어있는 Byte배열입니다.</param>
		/// <param name="vIndex">분석할 데이터의 시작위치 입니다.</param>
		/// <param name="vValueObj">분석한 값이 저장된 TMSSnmpPDUData클래스 객체 입니다.</param>
		internal static void DecodeSnmp_GetResponse(byte [] vData, int vIndex, ref TMSSnmp_GetResponsePDU vValueObj)
		{
			//Trap PDU의 길이를 얻습니다.
			int tLength = DecodeLength(vData, ref vIndex);

			//Get 요청 ID를 얻습니다.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Integer)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.RequestID = DecodeInteger32(vData, ref vIndex);

			//Error Status를 얻습니다.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Integer)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.ErrorStatus = (E_SnmpErrorStatus)DecodeInteger32(vData, ref vIndex);

			//Error Index를 얻습니다.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Integer)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.ErrorIndex = DecodeInteger32(vData, ref vIndex);
			
			//Sequence인지 확인 합니다.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Sequence)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;

			tLength = DecodeLength(vData, ref vIndex);
			
			if(tLength > 0)
			{
				DecodeVariables(vData, vIndex, tLength, ref vValueObj.m_Variables);
			}
		}

		/// <summary>
		/// SNMP Variable Bind부분은 분석합니다.
		/// </summary>
		/// <param name="vData">분석할 SNMP데이터가 들어있는 Byte배열입니다.</param>
		/// <param name="vIndex">분석할 데이터의 시작위치 입니다.</param>
		/// <param name="vLength">분석할 데이터의 전체 길이 입니다.</param>
		/// <param name="vValueObj">분석한 값이 저장된 TMSSnmpVariableCollection클래스 객체 입니다.</param>
		/// <returns>분석중 오류여부를 반환 합니다.</returns>
		internal static E_ParseError DecodeVariables(byte [] vData, int vIndex, int vLength, ref TMSSnmpVariableCollection vValueObj)
		{
			vLength += vIndex;

			while(vIndex < vLength)
			{
				TMSSnmpVariable tVariable = new TMSSnmpVariable();

				//Sequence인지 확인 합니다.
				if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Sequence)
				{
					return E_ParseError.IncorrectPacket;
				}
				vIndex++;
				DecodeLength(vData, ref vIndex);

				//OID인지 확인하고 Enterprise OID를 읽기 합니다.
				if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.ObjectIdentifier)
				{
					return E_ParseError.IncorrectPacket;
				}
				vIndex++;
				tVariable.ObjectID = DecodeObjectID(vData, ref vIndex);

				tVariable.Type = (E_SnmpDataType)vData[vIndex];
				vIndex++;
				
				switch(tVariable.Type)
				{
					case E_SnmpDataType.Counter:
					case E_SnmpDataType.Gauge:						
					case E_SnmpDataType.TimeTicks:
						tVariable.Value = DecodeUnsignedInteger32(vData, ref vIndex);
						break;

					case E_SnmpDataType.Integer:					
						tVariable.Value = DecodeInteger32(vData, ref vIndex);
						break;
	
					case E_SnmpDataType.Counter64:
						tVariable.Value = DecodeUnsignedInteger64(vData, ref vIndex);
						break;

					case E_SnmpDataType.IPAddress:
						tVariable.Value = DecodeIPAddress(vData, ref vIndex);
						break;	

					case E_SnmpDataType.OctetString:
						byte [] tRD = null;
						tVariable.Value = DecodeOctetString(vData, ref vIndex, ref tRD);
						tVariable.RawData = tRD;
						break;

					case E_SnmpDataType.ObjectIdentifier:
						tVariable.Value = DecodeObjectID(vData, ref vIndex);
						break;

					case E_SnmpDataType.Null:
						tVariable.Value = DecodeNull(vData, ref vIndex);
						break;
				}

				vValueObj.Add(tVariable);
			}
			return E_ParseError.NoError;
		}

		#region SNMP DataType Decode 부분 입니다 -------------------------------------------
		/// <summary>
		/// Length를 분석하여 Payload의 길이를 가져옵니다.
		/// </summary>
		/// <param name="vData">SNMP 데이터가 들어있는 Byte배열입니다.</param>
		/// <param name="vIndex">Length의 시작 위치 입니다. 결과 반환시 인덱스는 Length의 다음 위치 입니다.</param>
		/// <returns>Payload의 길이 입니다.</returns>
		public static int DecodeLength(byte [] vData, ref int vIndex)
		{			
			int tLengthCount = 0;
			int tLength = 0;

			//Decode Rule ------------------------------------------------------------------
			// Length의 Payload길이의 Decode는 Byte의 최상위 Bit가 1일경우 현재 Byte는
			// 최상위 Bit를 제외한 나머지 부분이 Length를 나타내는 Byte의 길이가 됩니다.
			// 만약 최상위 비트가 1이고 나머지부분의 값이 3일경우 다음 3개의 Byte는 길이를
			// 나타내는 Byte가됩니다.
			// 최상위 Bit가 0일 경우는 Byte자체가 Payload의 길이를 나타내게 됩니다.

			//Length의 길이가 1Byte이상인 경우
			if((vData[vIndex] & 0x80) == 0x80)
			{
				tLengthCount = (int)(vData[vIndex] ^ 0x80);
				tLength = (int)vData[vIndex + 1];
				for(int i = 2; i <= tLengthCount; i++)
				{
					tLength = (tLength << 8) + (int)vData[vIndex + i];
				}
				vIndex += tLengthCount + 1;
				return tLength;
			}
			else //Length의 길이가 1Byte인 경우
			{
				tLengthCount = (int)vData[vIndex];
				vIndex++;
				return tLengthCount;
			}
		}		

		/// <summary>
		/// 정수형을 Decode합니다.
		/// </summary>
		/// <param name="vData">SNMP 데이터가 들어있는 Byte배열입니다.</param>
		/// <param name="vIndex">Integer 데이터의 시작위치 입니다. 메소드 처리후 인덱스는 Integer 데이터의 위치 입니다.</param>
		/// <returns>Decode된 Integer데이터 입니다.</returns>
		public static int DecodeInteger32(byte [] vData, ref int vIndex)
		{
			//Decode Rule ------------------------------------------------------------------
			// 현재 Byte는 정수형 값의 Byte 길이를 나타냅니다.
			// 현재 Byte의 이후 Byte에서 길이만큼이 데이터 입니다.				

			int tLengthCount = (int)(vData[vIndex]);
			vIndex++;

			int tValue = 0;
			byte [] tB = new byte [8];

			if((vData[vIndex] & 0x80) == 0x80)
			{
				for(int i = 0; i < 8-tLengthCount; i++)
				{
					tB[i] = 0xFF;
				}					
			}
			
			try
			{	
				Array.Copy(vData, vIndex, tB, 8-tLengthCount, tLengthCount);
				Array.Reverse(tB, 0, 8);
				tValue = BitConverter.ToInt32(tB, 0);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			vIndex += tLengthCount;			
			return tValue;			
		}
		
		/// <summary>
		/// 정수형을 Decode합니다.
		/// </summary>
		/// <param name="vData">SNMP 데이터가 들어있는 Byte배열입니다.</param>
		/// <param name="vIndex">Integer 데이터의 시작위치 입니다. 메소드 처리후 인덱스는 Integer 데이터의 위치 입니다.</param>
		/// <returns>Decode된 Integer데이터 입니다.</returns>
		public static UInt32 DecodeUnsignedInteger32(byte [] vData, ref int vIndex)
		{
			//Decode Rule ------------------------------------------------------------------
			// 현재 Byte는 정수형 값의 Byte 길이를 나타냅니다.
			// 현재 Byte의 이후 Byte에서 길이만큼이 데이터 입니다.				

			byte [] tB = null;
			int tLengthCount = (int)(vData[vIndex]);
			vIndex++;

			UInt32 tValue = 0;
			tB = new byte [8];
			
			try
			{	
				Array.Copy(vData, vIndex, tB, 8-tLengthCount, tLengthCount);
				Array.Reverse(tB, 0, 8);
				tValue = BitConverter.ToUInt32(tB, 0);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			vIndex += tLengthCount;			
			return tValue;
		}

		/// <summary>
		/// 정수형을 Decode합니다.
		/// </summary>
		/// <param name="vData">SNMP 데이터가 들어있는 Byte배열입니다.</param>
		/// <param name="vIndex">Integer 데이터의 시작위치 입니다. 메소드 처리후 인덱스는 Integer 데이터의 위치 입니다.</param>
		/// <returns>Decode된 Integer데이터 입니다.</returns>
		public static long DecodeInteger64(byte [] vData, ref int vIndex)
		{
			//Decode Rule ------------------------------------------------------------------
			// 현재 Byte는 정수형 값의 Byte 길이를 나타냅니다.
			// 현재 Byte의 이후 Byte에서 길이만큼이 데이터 입니다.

			int tLengthCount = (int)(vData[vIndex]);
			vIndex++;

			long tValue = 0;
			byte [] tB = new byte [8];

			if((vData[vIndex] & 0x80) == 0x80)
			{
				for(int i = 0; i < 8-tLengthCount; i++)
				{
					tB[i] = 0xFF;
				}					
			}	
			
			Array.Copy(vData, vIndex, tB, 8-tLengthCount, tLengthCount);
			Array.Reverse(tB, 0, 8);
			tValue = BitConverter.ToInt64(tB, 0);

			vIndex += tLengthCount;
			return tValue;
		}
		
		/// <summary>
		/// 정수형을 Decode합니다.
		/// </summary>
		/// <param name="vData">SNMP 데이터가 들어있는 Byte배열입니다.</param>
		/// <param name="vIndex">Integer 데이터의 시작위치 입니다. 메소드 처리후 인덱스는 Integer 데이터의 위치 입니다.</param>
		/// <returns>Decode된 Integer데이터 입니다.</returns>
		public static UInt64 DecodeUnsignedInteger64(byte [] vData, ref int vIndex)
		{
			//Decode Rule ------------------------------------------------------------------
			// 현재 Byte는 정수형 값의 Byte 길이를 나타냅니다.
			// 현재 Byte의 이후 Byte에서 길이만큼이 데이터 입니다.

			int tLengthCount = (int)(vData[vIndex]);
			vIndex++;

			UInt64 tValue = 0;
			byte [] tB = new byte [8];
				
			Array.Copy(vData, vIndex, tB, 8-tLengthCount, tLengthCount);
			Array.Reverse(tB, 0, 8);
			tValue = BitConverter.ToUInt64(tB, 0);

			vIndex += tLengthCount;
			return tValue;
		}

		/// <summary>
		/// 문자열을 Decode합니다.
		/// </summary>
		/// <param name="vData">SNMP 데이터가 들어있는 Byte배열입니다.</param>
		/// <param name="vIndex">String 데이터의 시작위치 입니다. 메소드 처리후 인덱스는 String 데이터의 다음 위치 입니다.</param>
		/// <param name="vRawData">Decode되지 않은 값의 배열입니다.</param>
		/// <returns>Decode된 String데이터 입니다.</returns>
		public static string DecodeOctetString(byte [] vData, ref int vIndex, ref byte [] vRawData)
		{
			//			//Encode Rule ------------------------------------------------------------------
			//			// DecodeOctetString의 Decode Rule을 참고 하십시오.
			//			
			//			string tValue = "";
			//			int tLengthCount = DecodeLength(vData, ref vIndex);
			//			int i = 0;
			//
			//			if(tLengthCount > 0)
			//			{	
			//				bool isNull = false;
			//				for(i = 0; i < tLengthCount; i++)
			//				{
			//					if(vData[vIndex+i] == 0)
			//					{
			//						isNull = true;
			//						break;
			//					}
			//				}
			//
			//				if(isNull)
			//				{
			//					tValue = BitConverter.ToString(vData, vIndex, (int)tLengthCount);
			//					if(tLengthCount == 6)
			//					{
			//						tValue = System.Text.RegularExpressions.Regex.Replace(tValue, "-", ":");
			//					}
			//					else
			//					{
			//						tValue = System.Text.RegularExpressions.Regex.Replace(tValue, "-", "");
			//					}
			//				}
			//				else
			//				{
			//					tValue = System.Text.Encoding.ASCII.GetString(vData, vIndex, (int)tLengthCount);
			//				}				
			//			}			
			//			vIndex += tLengthCount;
			//			
			//			//tValue = System.Text.RegularExpressions.Regex.Replace(tValue, "\0", " ");
			//
			//			return tValue;



			//Encode Rule ------------------------------------------------------------------
			// DecodeOctetString의 Decode Rule을 참고 하십시오.
			
			string tStr = "";
			string tValue = "";
			int i = 0;
			int tLengthCount = DecodeLength(vData, ref vIndex);
			int sIdx = vIndex;
			int hCount = 0;
			byte [] tByteData = null;

			if(tLengthCount > 0)
			{	
				//-------------------------------
				vRawData = new byte [tLengthCount];
				Array.Copy(vData, vIndex, vRawData, 0, tLengthCount);
				//-------------------------------

				for(i = 0; i < tLengthCount; i++)
				{					
					//값이 16진수인지 확인 합니다.
					//if(vData[vIndex+i] < 16)
					if(((int)vData[vIndex+i] < 32 || (int)vData[vIndex+i] > 125))
					{
						//이전의 데이터가 16진수가 아니었으면
						if(hCount == 0)
						{
							//이전까지의 데이터를 문자열로 변경합니다.
							tValue += System.Text.Encoding.Default.GetString(vData, sIdx, (vIndex + i) - sIdx);
							sIdx = vIndex+i;
						}
						//16진수 개수를 증가시킵니다.
						hCount++;
					}
					else //일반 문자열인경우의 처리입니다.
					{
						//이전에 16진수의 개수가 있으면
						if(hCount > 0)
						{							
							if(hCount == 6) //MAC주소인지 확인합니다.
							{
								//16진수를 문자열로 변경합니다.
								tStr = BitConverter.ToString(vData, sIdx, (int)hCount);
								//MAC주소의 형식으로 변경합니다.
								tValue += System.Text.RegularExpressions.Regex.Replace(tStr, "-", ":");
							}
							else
							{
								if(hCount == 8)
								{
									tByteData = new byte [8];
									Array.Copy(vData, sIdx, tByteData, 0, 8);
									tValue += TMSDateAndTime.ToDateAndTime(tByteData).ToString();
								}
								else
								{
									//16진수를 문자열로 변경합니다.
									tStr = BitConverter.ToString(vData, sIdx, (int)hCount);
									tValue += System.Text.RegularExpressions.Regex.Replace(tStr, "-", "");
								}								
							}
							//16진수의 개수를 초기화 합니다.
							hCount = 0;
							sIdx = vIndex+i;
						}
					}
				}

				//이전의 데이터가 16진수가 아니었으면
				if(hCount == 0)
				{
					//이전까지의 데이터를 문자열로 변경합니다.
					tValue += System.Text.Encoding.Default.GetString(vData, sIdx, (vIndex + i) - sIdx);
				}
				else
				{					
					if(hCount == 6) //MAC주소인지 확인합니다.
					{
						//16진수를 문자열로 변경합니다.
						tStr = BitConverter.ToString(vData, sIdx, (int)hCount);
						//MAC주소의 형식으로 변경합니다.
						tValue += System.Text.RegularExpressions.Regex.Replace(tStr, "-", ":");
					}
					else
					{
						if(hCount == 8)
						{							
							tByteData = new byte [8];
							Array.Copy(vData, sIdx, tByteData, 0, 8);
							tValue += TMSDateAndTime.ToDateAndTime(tByteData).ToString();
						}
						else
						{
							//16진수를 문자열로 변경합니다.
							tStr = BitConverter.ToString(vData, sIdx, (int)hCount);
							tValue += System.Text.RegularExpressions.Regex.Replace(tStr, "-", "");
						}
					}
					
				}
			}
			vIndex += tLengthCount;
			
			//tValue = System.Text.RegularExpressions.Regex.Replace(tValue, "\0", " ");

			return tValue;
		}		

		/// <summary>
		/// ObjectIdentifier를 Decode합니다.
		/// </summary>
		/// <param name="vData">SNMP 데이터가 들어있는 Byte배열입니다.</param>
		/// <param name="vIndex">ObjectID 데이터의 시작위치 입니다. 메소드 처리후 인덱스는 ObjectID 데이터의 다음 위치 입니다.</param>
		/// <returns>Decode된 ObjectID데이터 입니다.</returns>
		public static string DecodeObjectID(byte [] vData, ref int vIndex)
		{
			string tStr = "";
			int tLengthCount = DecodeLength(vData, ref vIndex);

			if(tLengthCount > 0)
			{
				int tValue = ((int)vData[vIndex] % 40);
				tStr = ((int)(((int)vData[vIndex] - tValue) / 40)).ToString() + "." + tValue.ToString();

				tValue = 0;
				for(int i = 1; i < tLengthCount; i++)
				{
					if((vData[vIndex + i] & 0x80) == 0x80)
					{
						tValue = (tValue << 7) + (int)(vData[vIndex + i] & 0x7F);
					}
					else
					{
						if(tValue == 0)
						{
							tStr += "." + ((int)vData[vIndex + i]).ToString();
						}
						else
						{
							tValue = (tValue << 7) + (int)vData[vIndex + i];
							tStr += "." + tValue.ToString();
							tValue = 0;
						}					
					}
				}
			}
			vIndex += tLengthCount;
			return tStr;			
		}		

		/// <summary>
		/// IP주소를 Decode합니다.
		/// </summary>
		/// <param name="vData">SNMP 데이터가 들어있는 Byte배열입니다.</param>
		/// <param name="vIndex">IP주소 데이터의 시작위치 입니다. 메소드 처리후 인덱스는 IP주소 데이터의 다음 위치 입니다.</param>
		/// <returns>Decode된 IP주소 데이터 입니다.</returns>
		public static string DecodeIPAddress(byte [] vData, ref int vIndex)
		{
			string tStr = "";
			int tLengthCount = (int)vData[vIndex];
			vIndex++;

			for(int i = 0; i < tLengthCount; i++)
			{				
				tStr += ((int)vData[vIndex + i]).ToString() + ".";
			}

			vIndex += tLengthCount;
			return tStr.Substring(0, tStr.Length-1);
		}		

		/// <summary>
		/// Null데이터를 Decode합니다.
		/// </summary>
		/// <param name="vData">SNMP 데이터가 들어있는 Byte배열입니다.</param>
		/// <param name="vIndex">Null 데이터의 시작위치 입니다. 메소드 처리후 인덱스는 Null데이터의 다음 위치 입니다.</param>
		/// <returns></returns>
		public static string DecodeNull(byte [] vData, ref int vIndex)
		{
			vIndex++;
			return "";
		}
		#endregion //SNMP DataType Decode 부분 입니다 -------------------------------------------
		#endregion //SNMP Decode 부분 입니다 ----------------------------------------------------

		#region SNMP Encode 부분 입니다 ----------------------------------------------------
		/// <summary>
		/// SetRequest PDU를 Encode합니다.
		/// </summary>
		/// <param name="vVersion">Snmp Version입니다.</param>
		/// <param name="vCommunity">Snmp Community입니다.</param>
		/// <param name="vRequestID">GetRequest의 RequestID입니다.</param>
		/// <param name="vVariables">Variable배열입니다.</param>
		/// <returns>Encode된 결과를 반환 합니다.</returns>
		public static byte [] EncodeSetRequest(E_SnmpVersion vVersion, string vCommunity, int vRequestID, TMSSnmpVariableCollection vVariables)
		{
			return EncodeSnmpGetSetPDU(vVersion, vCommunity, E_SnmpPDUType.SetRequest, vRequestID, 0, 0, vVariables);
		}

		/// <summary>
		/// GetRequest PDU를 Encode합니다.
		/// </summary>
		/// <param name="vVersion">Snmp Version입니다.</param>
		/// <param name="vCommunity">Snmp Community입니다.</param>
		/// <param name="vRequestID">GetRequest의 RequestID입니다.</param>
		/// <param name="vVariables">Variable배열입니다.</param>
		/// <returns>Encode된 결과를 반환 합니다.</returns>
		public static byte [] EncodeGetRequest(E_SnmpVersion vVersion, string vCommunity, int vRequestID, TMSSnmpVariableCollection vVariables)
		{
			return EncodeSnmpGetSetPDU(vVersion, vCommunity, E_SnmpPDUType.GetRequest, vRequestID, 0, 0, vVariables);
		}

		/// <summary>
		/// GetNextRequest PDU를 Encode합니다.
		/// </summary>
		/// <param name="vVersion">Snmp Version입니다.</param>
		/// <param name="vCommunity">Snmp Community입니다.</param>
		/// <param name="vRequestID">GetRequest의 RequestID입니다.</param>		
		/// <param name="vVariables">Variable배열입니다.</param>
		/// <returns>Encode된 결과를 반환 합니다.</returns>
		public static byte [] EncodeGetNextRequest(E_SnmpVersion vVersion, string vCommunity, int vRequestID, TMSSnmpVariableCollection vVariables)
		{	
			return EncodeSnmpGetSetPDU(vVersion, vCommunity, E_SnmpPDUType.GetNextRequest, vRequestID, 0, 0, vVariables);			
		}

		/// <summary>
		/// SetRequest PDU를 Encode합니다.
		/// </summary>
		/// <param name="vVersion">Snmp Version입니다.</param>
		/// <param name="vCommunity">Snmp Community입니다.</param>
		/// <param name="vRequestID">GetRequest의 RequestID입니다.</param>
		/// <param name="vNon_Repeaters"></param>
		/// <param name="vMax_Repetition"></param>
		/// <param name="vVariables">Variable배열입니다.</param>
		/// <returns>Encode된 결과를 반환 합니다.</returns>
		public static byte [] EncodeGetBulkRequest(E_SnmpVersion vVersion, string vCommunity, int vRequestID, int vNon_Repeaters, int vMax_Repetition, TMSSnmpVariableCollection vVariables)
		{
			return EncodeSnmpGetSetPDU(vVersion, vCommunity, E_SnmpPDUType.GetBulkRequest, vRequestID, vNon_Repeaters, vMax_Repetition, vVariables);
		}

		/// <summary>
		/// Snmp의 Get, GetNext, GetBulk, Set PDU를 Encode합니다.
		/// </summary>
		/// <param name="vVersion">Snmp Version입니다.</param>
		/// <param name="vCommunity">Snmp Version1, 2의 접근 암호 입니다.</param>
		/// <param name="vPDUType">Snmp PDU 타입 입니다.</param>
		/// <param name="vRequestID">Snmp 요청 ID입니다.</param>
		/// <param name="vNon_Repeaters"></param>
		/// <param name="vMax_Repetition"></param>
		/// <param name="vVariables">Snmp 요청 정보가 들어있는 변수 배열입니다.</param>
		/// <returns>Encode된 Snmp PDU정보 배열입니다.</returns>
		private static byte [] EncodeSnmpGetSetPDU(E_SnmpVersion vVersion, string vCommunity, E_SnmpPDUType vPDUType, int vRequestID, int vNon_Repeaters, int vMax_Repetition, TMSSnmpVariableCollection vVariables)
		{
			ArrayList tData = new ArrayList();
			
			//버젼을 Encode합니다.
			tData.Add((byte)E_SnmpDataType.Integer);
			tData.AddRange(EncodeInteger32((Int32)vVersion));

			//Community를 Encode합니다.
			tData.Add((byte)E_SnmpDataType.OctetString);
			tData.AddRange(EncodeOctetString(vCommunity));

			//PDU Type를 Encode합니다.
			tData.Add((byte)vPDUType);
			
			int tIndex = tData.Count;

			//RequestID를 Encode합니다.
			tData.Add((byte)E_SnmpDataType.Integer);
			tData.AddRange(EncodeInteger32((Int32)vRequestID));

			//ErrorStatus를 Encode합니다.
			tData.Add((byte)E_SnmpDataType.Integer);
			tData.AddRange(EncodeInteger32(vNon_Repeaters));

			//ErrorIndex를 Encode합니다.
			tData.Add((byte)E_SnmpDataType.Integer);
			tData.AddRange(EncodeInteger32(vMax_Repetition));
			
			//Variable Bind를 Encode합니다.
			ArrayList tVariables = EncodeVariables(vVariables);

			//Variable Bind Sequence를 Encode합니다.
			tData.Add((byte)E_SnmpDataType.Sequence);
			tData.AddRange(EncodeLength(tVariables.Count));

			//Variable Bind를 추가합니다.
			tData.AddRange(tVariables);

			//PDU의 길이를 Encode합니다.
			int tCount  = tData.Count - tIndex;
			tData.InsertRange(tIndex, EncodeLength(tCount));

			//SNMP Sequence를 Encode합니다.
			tCount = tData.Count;
			tData.Insert(0, (byte)E_SnmpDataType.Sequence);
			tData.InsertRange(1, EncodeLength(tCount));
			
			byte [] tBytes = new byte[tData.Count];
			Array.Copy(tData.ToArray(), tBytes, tData.Count);
	
			return tBytes;
		}

		/// <summary>
		/// SNMP Version1 PDU를 생성 합니다.
		/// </summary>
		/// <param name="vPDU">Encode할 PDU정보가 들어있는 객체 입니다.</param>
		/// <returns>생성된 PDU 데이터의 Byte배열 입니다.</returns>
		public static byte [] EncodeSnmpPDU(ITMSSnmpPDU vPDU)
		{		
			if(vPDU == null) return null;

			ArrayList tData = new ArrayList();	

			//버젼을 Encode합니다.
			tData.Add((byte)E_SnmpDataType.Integer);
			tData.AddRange(EncodeInteger32((Int32)vPDU.Version));

			switch(vPDU.Version)
			{					
				case E_SnmpVersion.Version1:
				case E_SnmpVersion.Version2c:
					//PDU가 SNMP V1인경우
					EncodeSnmp_PDU(vPDU, ref tData);
					break;

				case E_SnmpVersion.Version3:
					//PDU가 SNmp V3인 경우
					break;
			}
			
			int tCount = tData.Count;
			tData.Insert(0, (byte)E_SnmpDataType.Sequence);
			tData.InsertRange(1, EncodeLength(tCount));
			byte [] tBytes = new byte [tData.Count];
			Array.Copy(tData.ToArray(), tBytes, tData.Count);
			return tBytes;
		}

		/// <summary>
		/// SNMP Version1 PDU를 생성 합니다.
		/// </summary>
		/// <param name="vPDU">Encode할 PDU정보가 들어있는 객체 입니다.</param>
		/// <param name="vData">Encode된 정보가 저장될 배열입니다.</param>
		/// <returns>생성된 PDU 데이터의 Byte배열 입니다.</returns>		
		internal static void EncodeSnmp_PDU(ITMSSnmpPDU vPDU, ref ArrayList vData)
		{
			switch(vPDU.PDUType)
			{
				case E_SnmpPDUType.GetResponse:
					//Community를 Encode합니다.
					vData.Add((byte)E_SnmpDataType.OctetString);
					vData.AddRange(EncodeOctetString(((TMSSnmp_GetResponsePDU)vPDU).Community));

					//PDU Type를 Encode합니다.
					vData.Add((byte)vPDU.PDUType);					

					EncodeSnmp_GetResponse((TMSSnmp_GetResponsePDU)vPDU, ref vData);
					break;

				case E_SnmpPDUType.TrapV1:
					//Community를 Encode합니다.
					vData.Add((byte)E_SnmpDataType.OctetString);
					vData.AddRange(EncodeOctetString(((TMSSnmp_TrapPDU)vPDU).Community));

					//PDU Type를 Encode합니다.
					vData.Add((byte)vPDU.PDUType);

					EncodeSnmp_Trap((TMSSnmp_TrapPDU)vPDU, ref vData);
					break;

				case E_SnmpPDUType.TrapV2:
					//Community를 Encode합니다.
					vData.Add((byte)E_SnmpDataType.OctetString);
					vData.AddRange(EncodeOctetString(((TMSSnmpV2_TrapPDU)vPDU).Community));

					//PDU Type를 Encode합니다.
					vData.Add((byte)vPDU.PDUType);

					EncodeSnmp_GetResponse((TMSSnmp_GetResponsePDU)vPDU, ref vData);
					break;
			}
		}

		/// <summary>
		/// SNMP Version1 Trap을 분석합니다.
		/// </summary>
		/// <param name="vPDU">Encode할 PDU정보가 들어있는 객체 입니다.</param>
		/// <param name="vData">Encode된 정보가 저장될 배열입니다.</param>
		public static void EncodeSnmp_Trap(TMSSnmp_TrapPDU vPDU, ref ArrayList vData)
		{
			int tIndex = vData.Count;
			ArrayList tVariableData = null;
				
			//OID를 Encode합니다.
			vData.Add((byte)E_SnmpDataType.ObjectIdentifier);
			vData.AddRange(EncodeObjectID(vPDU.Enterprise));

			//Agent주소를 Encode합니다.
			vData.Add((byte)E_SnmpDataType.IPAddress);
			vData.AddRange(EncodeIPAddress(vPDU.AgentIPAddress));

			//Trap Type을 Encode합니다.				
			vData.Add((byte)E_SnmpDataType.Integer);
			vData.AddRange(EncodeInteger32((Int32)vPDU.TrapType));

			//Specific Code를 Encode합니다.
			vData.Add((byte)E_SnmpDataType.Integer);
			vData.AddRange(EncodeInteger32((Int32)vPDU.SpecificCode));

			//Time Ticks를 Encode합니다.
			vData.Add((byte)E_SnmpDataType.TimeTicks);
			vData.AddRange(EncodeInteger32((Int32)vPDU.TimeTick));

			if(vPDU.Variables.Count > 0)
			{
				tVariableData = EncodeVariables(vPDU.Variables);
			}
			else
			{
				tVariableData = new ArrayList();
			}
				
			//Variable에대한 Sequence를 생성합니다.
			vData.Add((byte)E_SnmpDataType.Sequence);
			vData.AddRange(EncodeLength(tVariableData.Count));

			//Variable을 복사 합니다.
			vData.AddRange(tVariableData);
				
			//PDU전체 길이를 Encode합니다.
			int tCount = vData.Count - tIndex;
			vData.InsertRange(tIndex, EncodeLength(tCount));
		}

		/// <summary>
		/// SNMP V1, V2 GetResponse PDU를 Enocde합니다.
		/// </summary>
		/// <param name="vPDU">Encode할 PDU정보가 들어있는 객체 입니다.</param>
		/// <param name="vData">Encode된 정보가 저장될 배열입니다.</param>
		internal static void EncodeSnmp_GetResponse(TMSSnmp_GetResponsePDU vPDU, ref ArrayList vData)
		{
			int tIndex = vData.Count;
			ArrayList tVariableData = null;

			//OID를 Encode합니다.
			vData.Add((byte)E_SnmpDataType.Integer);
			vData.AddRange(EncodeInteger32((Int32)vPDU.RequestID));

			//Error Status를 Encode합니다.
			vData.Add((byte)E_SnmpDataType.Integer);
			vData.AddRange(EncodeInteger32((Int32)vPDU.ErrorStatus));

			//Error Index를 Encode합니다.
			vData.Add((byte)E_SnmpDataType.Integer);
			vData.AddRange(EncodeInteger32((Int32)vPDU.ErrorIndex));
			
			if(vPDU.Variables.Count > 0)
			{
				tVariableData = EncodeVariables(vPDU.Variables);
			}
				
			//Variable에대한 Sequence를 생성합니다.
			vData.Add((byte)E_SnmpDataType.Sequence);
			vData.AddRange(EncodeLength(tVariableData.Count));

			//Variable을 복사 합니다.
			vData.AddRange(tVariableData);
				
			//PDU전체 길이를 Encode합니다.
			int tCount = vData.Count - tIndex;
			vData.InsertRange(tIndex, EncodeLength(tCount));
		}

		/// <summary>
		/// SNMP Variable Bind부분을 생성합니다.
		/// </summary>
		/// <param name="vValueObj">생성할 값이 저장된 TMSSnmpVariableCollection클래스 객체 입니다.</param>
		/// <returns>생성한 Varaible Bind의 Byte배열 입니다.</returns>
		public static ArrayList EncodeVariables(TMSSnmpVariableCollection vValueObj)
		{	
			ArrayList tData = new ArrayList();
			ArrayList tTmp = new ArrayList();

			for(int i = 0; i < vValueObj.Count; i++)
			{
				tTmp.Clear();

				tTmp.Add((byte)E_SnmpDataType.ObjectIdentifier);
				tTmp.AddRange(EncodeObjectID(vValueObj[i].ObjectID));				

				switch(vValueObj[i].Type)
				{
					case E_SnmpDataType.Integer:
						tTmp.Add((byte)vValueObj[i].Type);
						if(vValueObj[i].Value.GetType().Equals(typeof(string)))
						{								
							int tInt32 = 0;
							if(TMSConvert.ToInteger32((string)vValueObj[i].Value, ref tInt32) == E_ConvertError.CantConvert)
							{
								throw new ArgumentException("지정한 값 " + (string)vValueObj[i].Value + "을(를) 지정한 형식 " + vValueObj[i].Type.ToString() + "로 변환 할 수 없습니다.");
							}
							tTmp.AddRange(EncodeInteger32((Int32)tInt32));
						}
						else
						{	
							tTmp.AddRange(EncodeInteger32((Int32)vValueObj[i].Value));
						}
						break;

					case E_SnmpDataType.Counter:
					case E_SnmpDataType.Gauge:
					case E_SnmpDataType.TimeTicks:
					case E_SnmpDataType.Sequence:
						

						tTmp.Add((byte)vValueObj[i].Type);
						if(vValueObj[i].Value.GetType().Equals(typeof(string)))
						{
							UInt32 tUInt32 = 0;

							if(TMSConvert.ToUnsignedInteger32((string)vValueObj[i].Value, ref tUInt32) == E_ConvertError.CantConvert)
							{
								throw new ArgumentException("지정한 값 " + (string)vValueObj[i].Value + "을(를) 지정한 형식 " + vValueObj[i].Type.ToString() + "로 변환 할 수 없습니다.");
							}
							tTmp.AddRange(EncodeUnsignedInteger32((UInt32)tUInt32));
						}
						else
						{	
							tTmp.AddRange(EncodeUnsignedInteger32((UInt32)vValueObj[i].Value));
						}
						break;
	
					case E_SnmpDataType.Counter64:
						tTmp.Add((byte)vValueObj[i].Type);
						if(vValueObj[i].Value.GetType().Equals(typeof(string)))
						{
							UInt64 tUInt64 = 0;

							if(TMSConvert.ToUnsignedInteger64((string)vValueObj[i].Value, ref tUInt64) == E_ConvertError.CantConvert)
							{
								throw new ArgumentException("지정한 값 " + (string)vValueObj[i].Value + "을(를) 지정한 형식 " + vValueObj[i].Type.ToString() + "로 변환 할 수 없습니다.");
							}
							tTmp.AddRange(EncodeUnsignedInteger64((UInt64)tUInt64));
						}
						else
						{	
							tTmp.AddRange(EncodeUnsignedInteger64((UInt64)vValueObj[i].Value));
						}
						break;

					case E_SnmpDataType.IPAddress:
						tTmp.Add((byte)vValueObj[i].Type);
						tTmp.AddRange(EncodeIPAddress((string)vValueObj[i].Value));
						break;	

					case E_SnmpDataType.OctetString:
						tTmp.Add((byte)vValueObj[i].Type);
						if(vValueObj[i].Value.GetType().Equals(typeof(string)))
						{
							tTmp.AddRange(EncodeOctetString((string)vValueObj[i].Value));
						}
						else
						{
							tTmp.AddRange(EncodeOctetString((byte [])vValueObj[i].Value));
						}
						break;

					case E_SnmpDataType.ObjectIdentifier:
						tTmp.Add((byte)vValueObj[i].Type);
						tTmp.AddRange(EncodeObjectID((string)vValueObj[i].Value));
						break;

					case E_SnmpDataType.Opaque:
						tTmp.Add((byte)vValueObj[i].Type);
						if(vValueObj[i].Value.GetType().Equals(typeof(string)))
						{
							tTmp.AddRange(EncodeOctetString((string)vValueObj[i].Value));
						}
						else
						{
							tTmp.AddRange(EncodeOctetString((byte [])vValueObj[i].Value));
						}
						break;

					case E_SnmpDataType.DateAndTime:
						tTmp.Add((byte)E_SnmpDataType.OctetString);
						if(vValueObj[i].Value.GetType().Equals(typeof(string)))
						{
							tTmp.AddRange(EncodeOctetString(TMSDateAndTime.ToBytes((string)vValueObj[i].Value)));
						}
						else
						{
							if(vValueObj[i].Value.GetType().Equals(typeof(TMSDateAndTime)))
							{
								tTmp.AddRange(EncodeOctetString(((TMSDateAndTime)vValueObj[i].Value).GetBytes()));
							}
							else
							{
								tTmp.AddRange(EncodeOctetString((byte [])vValueObj[i].Value));
							}
						}
						break;
					
					case E_SnmpDataType.MACAddress:
						tTmp.Add((byte)E_SnmpDataType.OctetString);
						if(vValueObj[i].Value.GetType().Equals(typeof(string)))
						{
							tTmp.AddRange(EncodeOctetString(SOSnmpClass.MacToBytes((string)vValueObj[i].Value)));
						}
						else
						{
							tTmp.AddRange(EncodeOctetString((byte [])vValueObj[i].Value));
						}
						break;
					
					default:
						tTmp.Add((byte)vValueObj[i].Type);
						tTmp.AddRange(EncodeNull());
						break;
				}

				tData.Add((byte)E_SnmpDataType.Sequence);
				tData.AddRange(EncodeLength(tTmp.Count));
				tData.AddRange(tTmp);
			}				
			return tData;
		}

		#region SNMP DataType Encode 부분 입니다 -------------------------------------------
		/// <summary>
		/// 지정한 Payload의 길이를 Encode합니다.
		/// </summary>
		/// <param name="vLength">Encode할 Payload의 길이 입니다.</param>
		/// <returns>Encode된 Payload의 길이 입니다.</returns>
		public static byte [] EncodeLength(int vLength)
		{
			byte [] tLength = null;			

			//Encode Rule ------------------------------------------------------------------
			// DecodeLength의 Decode Rule를 참고 하십시오.

			//길이를 한Byte에 나타낼수 있을 경우
			if(vLength < 127)
			{
				tLength = new byte [] { (byte)vLength };
			}
			else //길이를 1Byte에 나타낼 수 없을경우
			{
				int i = 0, tCnt = 0;
				
				vLength = System.Net.IPAddress.HostToNetworkOrder(vLength);
				byte [] tLen = BitConverter.GetBytes(vLength);
				for(i = 0; i < tLen.Length; i++)
				{
					if(tLen[i] > 0) break;
				}
				tCnt = tLen.Length - i;

				tLength = new byte [tCnt + 1];
				tLength[0] = (byte)(0x80 + tCnt);
				Array.Copy(tLen, i, tLength, 1, tCnt);				
			}		
			return tLength;
		}

		/// <summary>
		/// 지정한 정수형을 Encode합니다.
		/// </summary>
		/// <param name="vValue">Encode할 정수형 데이터 입니다.</param>
		/// <returns>Encode된 데이터의 Byte배열입니다.</returns>
		public static byte [] EncodeInteger32(Int32 vValue)
		{
			//Encode Rule ------------------------------------------------------------------
			// DecodeInteger의 Decode Rule을 참고 하십시오.
			
			byte [] tLength = null;
			int i = 0, tCnt = 0;				
				
			if(vValue == 0)
			{
				tLength = new byte [] { 1, 0 };
			}
			else
			{
				byte [] tLen = BitConverter.GetBytes(vValue);
				Array.Reverse(tLen, 0, tLen.Length);

				if(vValue > 0)
				{
					for(i = 0; i < tLen.Length; i++)
					{
						if(tLen[i] > 0)
						{
							if((tLen[i] & 0x80) == 0x80) i--;
							break;
						}
					}
					tCnt = tLen.Length - i;					

					tLength = new byte [tCnt + 1];
					tLength[0] = (byte)tCnt;

					Array.Copy(tLen, i, tLength, 1, tCnt);
				}
				else
				{
					for(i = 0; i < tLen.Length; i++)
					{
						if(tLen[i] != 0xFF) break;
					}
					i--;
					tCnt = tLen.Length - i;

					tLength = new byte [tCnt + 1];
					tLength[0] = (byte)tCnt;

					Array.Copy(tLen, i, tLength, 1, tCnt);
				}
			}
			return tLength;
		}
		
		/// <summary>
		/// 지정한 정수형을 Encode합니다.
		/// </summary>
		/// <param name="vValue">Encode할 정수형 데이터 입니다.</param>
		/// <returns>Encode된 데이터의 Byte배열입니다.</returns>
		public static byte [] EncodeUnsignedInteger32(UInt32 vValue)
		{
			//Encode Rule ------------------------------------------------------------------
			// DecodeInteger의 Decode Rule을 참고 하십시오.
			
			byte [] tLength = null;
			int i = 0, tCnt = 0;				
				
			if(vValue == 0)
			{
				tLength = new byte [] { 1, 0 };
			}
			else
			{
				byte [] tLen = BitConverter.GetBytes(vValue);
				Array.Reverse(tLen, 0, tLen.Length);

				for(i = 0; i < tLen.Length; i++)
				{
					if(tLen[i] > 0) break;
				}
				tCnt = tLen.Length - i;

				tLength = new byte [tCnt + 1];
				tLength[0] = (byte)tCnt;

				Array.Copy(tLen, i, tLength, 1, tCnt);
			}
			return tLength;
		}


		/// <summary>
		/// 지정한 정수형을 Encode합니다.
		/// </summary>
		/// <param name="vValue">Encode할 정수형 데이터 입니다.</param>
		/// <returns>Encode된 데이터의 Byte배열입니다.</returns>
		public static byte [] EncodeInteger64(Int64 vValue)
		{
			//Encode Rule ------------------------------------------------------------------
			// DecodeInteger의 Decode Rule을 참고 하십시오.
			byte [] tLength = null;
			int i = 0, tCnt = 0;				
				
			if(vValue == 0)
			{
				tLength = new byte [] { 1, 0 };
			}
			else
			{
				byte [] tLen = BitConverter.GetBytes(vValue);
				Array.Reverse(tLen, 0, tLen.Length);

				if(vValue > 0)
				{
					for(i = 0; i < tLen.Length; i++)
					{
						if(tLen[i] > 0) break;
					}
					tCnt = tLen.Length - i;

					tLength = new byte [tCnt + 1];
					tLength[0] = (byte)tCnt;

					Array.Copy(tLen, i, tLength, 1, tCnt);
				}
				else
				{
					for(i = 0; i < tLen.Length; i++)
					{
						if(tLen[i] != 0xFF) break;
					}
					i--;
					tCnt = tLen.Length - i;

					tLength = new byte [tCnt + 1];
					tLength[0] = (byte)tCnt;

					Array.Copy(tLen, i, tLength, 1, tCnt);
				}
			}
			return tLength;
		}

		/// <summary>
		/// 지정한 정수형을 Encode합니다.
		/// </summary>
		/// <param name="vValue">Encode할 정수형 데이터 입니다.</param>
		/// <returns>Encode된 데이터의 Byte배열입니다.</returns>
		public static byte [] EncodeUnsignedInteger64(UInt64 vValue)
		{
			//Encode Rule ------------------------------------------------------------------
			// DecodeInteger의 Decode Rule을 참고 하십시오.
			byte [] tLength = null;
			int i = 0, tCnt = 0;				
				
			if(vValue == 0)
			{
				tLength = new byte [] { 1, 0 };
			}
			else
			{
				byte [] tLen = BitConverter.GetBytes(vValue);
				Array.Reverse(tLen, 0, tLen.Length);

				for(i = 0; i < tLen.Length; i++)
				{
					if(tLen[i] > 0) break;
				}
				tCnt = tLen.Length - i;

				tLength = new byte [tCnt + 1];
				tLength[0] = (byte)tCnt;

				Array.Copy(tLen, i, tLength, 1, tCnt);				
			}
			return tLength;
		}

		/// <summary>
		/// ObjectID를 Encode합니다.
		/// </summary>
		/// <param name="vValue">Encode할 ObjectID입니다.</param>
		/// <returns>Encode된 ObjectID입니다.</returns>
		public static byte [] EncodeObjectID(string vValue)
		{
			int tIdx = 0;
			int tValue = 0, tVal1 = 0;

			string [] tString = vValue.Split('.');
			byte [] tDatas = new byte [tString.Length * 2];			
			
			if(tString.Length > 1)
			{
				TMSConvert.ToInteger32(tString[0], ref tValue);
				TMSConvert.ToInteger32(tString[1], ref tVal1);
				tDatas[tIdx] = (byte)(tValue * 40 + tVal1);
			}			
			
			tIdx = 1;
			for(int i = 2; i < tString.Length; i++)
			{
				if(tString[i] == "") continue;

				tValue = 0;				
				TMSConvert.ToInteger32(tString[i], ref tValue);
				if(tValue < 127)
				{
					tDatas[tIdx] = (byte)tValue;
					tIdx++;
				}
				else
				{
					int cIdx = 0;
					byte [] tVal = new byte [8];
					
					while(true)
					{	
						tVal[cIdx] = (byte)((tValue) & 0x7F);
						cIdx++;
						tValue >>= 7;
						if(tValue <= 0) break;
					}

					for(int j = cIdx - 1; j > 0; j--)
					{
						tDatas[tIdx] = (byte)(tVal[j] | 0x80);
						tIdx++;
					}
					tDatas[tIdx] = (byte)tVal[0];
					tIdx++;
				}
			}
			byte [] tLength = EncodeLength(tIdx);
			byte [] tBytes = new byte [tLength.Length + tIdx];
			Array.Copy(tLength, 0, tBytes, 0, tLength.Length);
			Array.Copy(tDatas, 0, tBytes, tLength.Length, tIdx);

			return tBytes;
		}

		/// <summary>
		/// 지정한 문자열을 Encode합니다.
		/// </summary>
		/// <param name="vValue">Encode할 문자열 데이터 입니다.</param>
		/// <returns>Encode된 데이터의 Byte배열입니다.</returns>
		public static byte [] EncodeOctetString(string vValue)
		{
			//Decode Rule ------------------------------------------------------------------
			// 현재 Byte는 문자열 값의 Byte 길이를 나타냅니다.
			// 현재 Byte의 이후 Byte에서 길이만큼이 데이터 입니다.

			byte [] tData = System.Text.Encoding.Default.GetBytes(vValue);
			return EncodeOctetString(tData);
			//			byte [] tLength = EncodeLength(tData.Length);
			//			byte [] tBytes = new byte [tData.Length + tLength.Length];
			//			Array.Copy(tLength, 0, tBytes, 0, tLength.Length);
			//			Array.Copy(tData, 0, tBytes, tLength.Length, tData.Length);
			//
			//			return tBytes;
		}

		/// <summary>
		/// 지정한 문자열을 Encode합니다.
		/// </summary>
		/// <param name="vValue">Encode할 문자열 데이터 입니다.</param>
		/// <returns>Encode된 데이터의 Byte배열입니다.</returns>
		public static byte [] EncodeOctetString(byte [] vValue)
		{
			byte [] tLength = EncodeLength(vValue.Length);
			byte [] tBytes = new byte [vValue.Length + tLength.Length];
			Array.Copy(tLength, 0, tBytes, 0, tLength.Length);
			Array.Copy(vValue, 0, tBytes, tLength.Length, vValue.Length);

			return tBytes;
		}

		/// <summary>
		/// IPAddress를 Encode합니다.
		/// </summary>
		/// <param name="vIPAddress">Encode할 IP주소 입니다.</param>
		/// <returns>Encode된 IP주소의 Byte배열입니다.</returns>
		public static byte [] EncodeIPAddress(string vIPAddress)
		{
			string [] tString = vIPAddress.Split('.');
			byte [] tDatas = new byte [tString.Length + 1];
			int tIdx = 1;
			int tValue = 0;

			tDatas[0] = (byte)tString.Length;
			for(int i = 0; i < tString.Length; i++)
			{
				tValue = 0;
				TMSConvert.ToInteger32(tString[i], ref tValue);
				tDatas[tIdx + i] = (byte)tValue;
			}

			return tDatas;
		}
		/// <summary>
		/// Null을 Encode합니다.
		/// </summary>
		/// <returns>Encode된 Null의 Byte배열을 반환 합니다.</returns>
		public static byte [] EncodeNull()
		{
			byte [] tDatas = new byte[1];
			return tDatas;
		}
		#endregion //SNMP DataType Encode 부분 입니다 -------------------------------------------
		#endregion //SNMP Encode 부분 입니다 ----------------------------------------------------

		#region Snmp DateType변환 부분 입니다 ----------------------------------------------
		/// <summary>
		/// 지정한 MAC주소를 Byte배열로 변경합니다.
		/// </summary>
		/// <param name="vMacAddress">Byte배열로 변환할 MAC주소 입니다.</param>
		/// <returns>변환된 MAC주소의 Byte배열입니다. 변환실패의 경우 null입니다.</returns>
		public static byte [] MacToBytes(string vMacAddress)
		{
			int tM = 0;
			byte [] bMacs = new byte [6];
			string [] tMacs = vMacAddress.Split(':');

			if(tMacs.Length != 6) return null;

			for(int i = 0; i < tMacs.Length; i++)
			{	
				tM = 0;
				try
				{
					if(tMacs[i].Length < 3)
					{					
						tM = Uri.FromHex(tMacs[i][0]);
						tM *= 16;
						tM += Uri.FromHex(tMacs[i][1]);						
					}
				}
				catch(Exception ex)
				{
					ex=ex;
				}
				finally
				{
					bMacs[i] = (byte)tM;
				}
			}
			return bMacs;
		}		
		#endregion //Snmp DateType변환 부분 입니다 ----------------------------------------------
	}	

	#region SNMP Data Type 클래스 입니다 ---------------------------------------------------
	/// <summary>
	/// 시간 및 분의 추가 방향입니다.
	/// </summary>
	public enum E_UTCDirection
	{
		/// <summary>
		/// UTC없음 입니다.
		/// </summary>
		None = 0,
		/// <summary>
		/// UTC를 더하기 합니다.
		/// </summary>
		Plus = 43,
		/// <summary>
		/// UTC를 빼기 합니다.
		/// </summary>
		Minus = 45
	}
	/// <summary>
	/// SNMP DateAndType 클래스 입니다.
	/// Display Format : YYYY-MM-dd,HH:mm:ss.ds,UTC Direction UTCHour:UTCMinute (ex: 2005-10-01,12:35:20.2,-4:0)
	/// DateAndType Byte Formate : 
	/// Year-2Byte (0-65535)
	/// Month-1Byte (1-12)
	/// Day-1Byte (1-30)
	/// Hour-1Byte (0-23)
	/// Minute-1Byte (0-59)
	/// Second-1Byte (0-60)
	/// DeciSecond-1Byte (0-9)
	/// UTC Direction-1Byte String [ + | - ]
	/// UTC Hour-1Byte (0-13)
	/// UTC Minute-1Byte (0-59)
	/// </summary>
	public class TMSDateAndTime
	{
		/// <summary>
		/// 년도 입니다.
		/// </summary>
		private int m_Year = 1999;
		/// <summary>
		/// 월 입니다.
		/// </summary>
		private int m_Month = 1;
		/// <summary>
		/// 일 입니다.
		/// </summary>
		private int m_Day = 1;
		/// <summary>
		/// 시간 입니다.
		/// </summary>
		private int m_Hour = 0;
		/// <summary>
		/// 분 입니다.
		/// </summary>
		private int m_Minute = 0;
		/// <summary>
		/// 초 입니다.
		/// </summary>
		private int m_Second = 0;
		/// <summary>
		/// 1/10 초 입니다.
		/// </summary>
		private int m_DeciSecond = 0;
		/// <summary>
		/// UTC의 더하기 방향입니다.
		/// </summary>
		private E_UTCDirection m_UTCDirection = E_UTCDirection.None;
		/// <summary>
		/// 더하거나 뺄 시간 입니다.
		/// </summary>
		private int m_UTCHour = 0;
		/// <summary>
		/// 더하거나 뺄 분 입니다.
		/// </summary>
		private int m_UTCMinute = 0;

		/// <summary>
		/// TMSDateAndTime클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vYear">년도 입니다.</param>
		/// <param name="vMonth">월 입니다.</param>
		/// <param name="vDay">일 입니다.</param>
		/// <param name="vHour">시간 입니다.</param>
		/// <param name="vMinute">분 입니다.</param>
		/// <param name="vSecond">초 입니다.</param>
		/// <param name="vDeciSecond">1/10 초 입니다.</param>
		/// <param name="vUTCDirection">UTC의 더하기 방향입니다.</param>
		/// <param name="vUTCHour">더하거나 뺄 시간 입니다.</param>
		/// <param name="vUTCMinute">더하거나 뺄 분 입니다.</param>
		public TMSDateAndTime(int vYear, int vMonth, int vDay, int vHour, int vMinute, int vSecond, int vDeciSecond, E_UTCDirection vUTCDirection, int vUTCHour, int vUTCMinute)
		{
			m_Year = vYear;
			m_Month = vMonth;
			m_Day = vDay;
			m_Hour = vHour;
			m_Minute = vMinute;
			m_Second = vSecond;
			m_DeciSecond = vDeciSecond;
			
			m_UTCDirection = vUTCDirection;
			m_UTCHour = vUTCHour;
			m_UTCMinute = vUTCMinute;
		}

		/// <summary>
		/// TMSDateAndTime클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vDateTime">날짜와 시간을 포함한 DateTime객체 입니다.</param>
		/// <param name="vUTCDirection">UTC의 더하기 방향입니다.</param>
		/// <param name="vUTCHour">더하거나 뺄 시간 입니다.</param>
		/// <param name="vUTCMinute">더하거나 뺄 분 입니다.</param>
		public TMSDateAndTime(DateTime vDateTime, E_UTCDirection vUTCDirection, int vUTCHour, int vUTCMinute)
		{
			m_Year = vDateTime.Year;
			m_Month = vDateTime.Month;
			m_Day = vDateTime.Day;
			m_Hour = vDateTime.Hour;
			m_Minute = vDateTime.Minute;
			m_Second = vDateTime.Second;
			m_DeciSecond = vDateTime.Millisecond / 100;
			
			m_UTCDirection = vUTCDirection;
			m_UTCHour = vUTCHour;
			m_UTCMinute = vUTCMinute;
		}

		/// <summary>
		/// TMSDateAndTime클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vDateTime">날짜와 시간이 포함된 문자열 입니다(예: "2005-10-10,13:30:30.3,-2:20").</param>
		public TMSDateAndTime(string vDateTime)
		{
			try
			{
				int iPos = 0;
				int sPos = 0;
				int tPos = vDateTime.IndexOf(",");
				if(tPos == -1) return;
				string tStr = vDateTime.Substring(0, tPos);
				string [] tDate = tStr.Split('-');
				if(tDate.Length != 3) return;

				TMSConvert.ToInteger32(tDate[0], ref m_Year);
				TMSConvert.ToInteger32(tDate[1], ref m_Month);
				TMSConvert.ToInteger32(tDate[2], ref m_Day);

				sPos = tPos+1;
				tPos = vDateTime.IndexOf(",", sPos);
				if(tPos == -1)
				{
					if(vDateTime.Length - sPos < 1) return;
					tStr = vDateTime.Substring(sPos, vDateTime.Length - sPos);
					tDate = tStr.Split(':');
					if(tDate.Length != 3) return;
					TMSConvert.ToInteger32(tDate[0], ref m_Hour);
					TMSConvert.ToInteger32(tDate[1], ref m_Minute);
					iPos = tDate[2].IndexOf('.');
					if(iPos > 0)
					{
						tDate = tDate[2].Split('.');
						TMSConvert.ToInteger32(tDate[0], ref m_Second);
						TMSConvert.ToInteger32(tDate[1], ref m_DeciSecond);
					}
					else
					{
						TMSConvert.ToInteger32(tDate[2], ref m_Second);
					}
					return;
				}
				else
				{
					tStr = vDateTime.Substring(sPos, tPos - sPos);
					tDate = tStr.Split(':');
					if(tDate.Length != 3) return;
					TMSConvert.ToInteger32(tDate[0], ref m_Hour);
					TMSConvert.ToInteger32(tDate[1], ref m_Minute);
					iPos = tDate[2].IndexOf('.');
					if(iPos > 0)
					{
						tDate = tDate[2].Split('.');
						TMSConvert.ToInteger32(tDate[0], ref m_Second);
						TMSConvert.ToInteger32(tDate[1], ref m_DeciSecond);
					}
					else
					{
						TMSConvert.ToInteger32(tDate[2], ref m_Second);
					}
				}
				sPos = tPos+1;
				if(vDateTime[sPos] == '+')
				{
					m_UTCDirection = E_UTCDirection.Plus;
				}
				else 
				{
					if(vDateTime[sPos] == '-')
					{
						m_UTCDirection = E_UTCDirection.Minus;
					}
					else
					{
						return;
					}
				}
 
				sPos++;
				if(vDateTime.Length - sPos < 1) return;
				tStr = vDateTime.Substring(sPos, vDateTime.Length - sPos);
				tDate = tStr.Split(':');
				if(tDate.Length != 2) return;

				TMSConvert.ToInteger32(tDate[0], ref m_UTCHour);
				TMSConvert.ToInteger32(tDate[1], ref m_UTCMinute);
			}
			catch(Exception ex)
			{
				ex=ex;
			}
		}

		/// <summary>
		/// TMSDateAndTime클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vDateTime">날짜와 시간이 저장된 Byte배열 입니다.</param>
		public TMSDateAndTime(byte [] vDateTime)
		{
			try
			{
				m_Year = ((int)vDateTime[0] << 8) + (int)vDateTime[1];
				m_Month = (int)vDateTime[2];
				m_Day = (int)vDateTime[3];
				m_Hour = (int)vDateTime[4];
				m_Minute = (int)vDateTime[5];
				m_Second = (int)vDateTime[6];
				m_DeciSecond = (int)vDateTime[7];
				m_UTCDirection = (E_UTCDirection)vDateTime[8];
				m_UTCHour = (int)vDateTime[9];
				m_UTCMinute = (int)vDateTime[10];
			}
			catch(Exception ex)
			{
				ex=ex;
			}
		}

		/// <summary>
		/// TMSDateAndTime의 날짜와 시간을 Byte배열로 변환합니다.
		/// </summary>
		/// <returns>Byte배열로 변환된 날짜와 시간입니다.</returns>
		public byte [] GetBytes()
		{
			byte [] tDate;

			if(m_UTCDirection == E_UTCDirection.None)
			{
				tDate = new byte[8];				
			}
			else
			{
				tDate = new byte[11];
				tDate[8] = (byte)m_UTCDirection;
				tDate[9] = (byte)m_UTCHour;
				tDate[10] = (byte)m_UTCMinute;
			}

			tDate[0] = (byte)((m_Year & 0xff00) >> 8);
			tDate[1] = (byte)(m_Year & 0xff);
			tDate[2] = (byte)m_Month;
			tDate[3] = (byte)m_Day;
			tDate[4] = (byte)m_Hour;
			tDate[5] = (byte)m_Minute;
			tDate[6] = (byte)m_Second;
			tDate[7] = (byte)m_DeciSecond;
			return tDate;
		}
		
		/// <summary>
		/// TMSDateAndTime의 날짜와 시간을 문자열로 변환합니다.
		/// </summary>
		/// <returns>문자열로 변환된 날짜와 시간입니다.</returns>
		public new string ToString()
		{
			string tStr = m_Year.ToString() + "-" + m_Month.ToString() + "-" + m_Day.ToString() + "," + m_Hour.ToString() + ":" + m_Minute.ToString() + ":" + m_Second.ToString() + "." + m_DeciSecond.ToString();
			if(m_UTCDirection != E_UTCDirection.None)
			{
				tStr += "," + (char)m_UTCDirection + m_UTCHour.ToString() + ":" + m_UTCMinute.ToString();
			}
			return tStr;
		}

		/// <summary>
		/// 지정한 날짜와 시간을 Byte배열로 변환 합니다.
		/// </summary>
		/// <param name="vYear">년도 입니다.</param>
		/// <param name="vMonth">월 입니다.</param>
		/// <param name="vDay">일 입니다.</param>
		/// <param name="vHour">시간 입니다.</param>
		/// <param name="vMinute">분 입니다.</param>
		/// <param name="vSecond">초 입니다.</param>
		/// <param name="vDeciSecond">1/10 초 입니다.</param>
		/// <param name="vUTCDirection">UTC의 더하기 방향입니다.</param>
		/// <param name="vUTCHour">더하거나 뺄 시간 입니다.</param>
		/// <param name="vUTCMinute">더하거나 뺄 분 입니다.</param>
		/// <returns>Byte배열로 변환된 날짜와 시간입니다.</returns>
		public static byte [] ToBytes(int vYear, int vMonth, int vDay, int vHour, int vMinute, int vSecond, int vDeciSecond, E_UTCDirection vUTCDirection, int vUTCHour, int vUTCMinute)
		{
			byte [] tDate;

			if(vUTCDirection == E_UTCDirection.None)
			{
				tDate = new byte[8];				
			}
			else
			{
				tDate = new byte[11];
				tDate[8] = (byte)vUTCDirection;
				tDate[9] = (byte)vUTCHour;
				tDate[10] = (byte)vUTCMinute;
			}

			tDate[0] = (byte)((vYear & 0xff00) >> 8);
			tDate[1] = (byte)(vYear & 0xff);
			tDate[2] = (byte)vMonth;
			tDate[3] = (byte)vDay;
			tDate[4] = (byte)vHour;
			tDate[5] = (byte)vMinute;
			tDate[6] = (byte)vSecond;
			tDate[7] = (byte)vDeciSecond;
			return tDate;
		}

		/// <summary>
		/// 지정한 날짜와 시간 문자열을 Byte배열로 변환 합니다.
		/// </summary>
		/// <param name="vDateTime">날짜와 시간이 포함된 문자열 입니다.</param>
		/// <returns>Byte배열로 변환된 날짜와 시간입니다.</returns>
		public static byte [] ToBytes(string vDateTime)
		{
			TMSDateAndTime tDateTime = new TMSDateAndTime(vDateTime);
			return tDateTime.GetBytes();
		}

		/// <summary>
		/// 지정한 날짜와 시간 Byte배열을 TMSDateAndTime형식으로 변환 합니다.
		/// </summary>
		/// <param name="vDateTime">날짜와 시간이 저장된 Byte배열 입니다.</param>
		/// <returns>날짜와 시간이 변환된 TMSDateAndTime객체 입니다.</returns>
		public static TMSDateAndTime ToDateAndTime(byte [] vDateTime)
		{
			return new TMSDateAndTime(vDateTime);
		}
	}
	#endregion SNMP Data Type 클래스 입니다 ---------------------------------------------------

	#region SNMP PDU 클래스 입니다 ---------------------------------------------------------
	#region SNMP PDU Interface입니다 -------------------------------------------------------
	/// <summary>
	/// SNMP PDU의 인터페이스 입니다.
	/// </summary>
	public interface ITMSSnmpPDU
	{
		/// <summary>
		/// SNMP Version을 가져오거나 설정합니다.
		/// </summary>
		E_SnmpVersion Version {get; set;}
		/// <summary>
		/// SNMP PDU Type을 가져오거나 설정합니다.
		/// </summary>
		E_SnmpPDUType PDUType {get; set;}			
	}
	#endregion //SNMP PDU Interface입니다 -------------------------------------------------------

	#region SNMP V1, V2 TrapPDU 클래스 입니다 ----------------------------------------------
	/// <summary>
	/// Snmp Version1의 트랩 PDU클래스 입니다.
	/// </summary>
	public class TMSSnmp_TrapPDU : ITMSSnmpPDU
	{
		private E_SnmpVersion m_Version = E_SnmpVersion.Version1;
		private string m_Community = "";
		private E_SnmpPDUType m_PDUType = E_SnmpPDUType.TrapV1;
		private string m_Enterprise = "";
		private string m_AgentIPAddress = "";
		private E_GenericTrapType m_GenericTrapType = E_GenericTrapType.ColdStart;
		private long m_SpecificCode = 0;
		private long m_TimeTick = 0;

		internal TMSSnmpVariableCollection m_Variables = new TMSSnmpVariableCollection();

		private E_ParseError m_ParseError = E_ParseError.NoError;

		/// <summary>
		/// Snmp Version1 트랩 PDU클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vVersion">Snmp Version입니다.</param>
		public TMSSnmp_TrapPDU(E_SnmpVersion vVersion)
		{
			m_Version = vVersion;
		}

		/// <summary>
		/// Snmp Version을 가져오거나 설정합니다.
		/// </summary>
		public E_SnmpVersion Version
		{
			get { return m_Version; }
			set { m_Version = value; }
		}

		/// <summary>
		/// Snmp 접근 암호를 가져오거나 설정합니다.
		/// </summary>
		public string Community
		{
			get { return m_Community; }
			set { m_Community = value; }
		}

		/// <summary>
		/// Snmp PDU 타입을 가져오거나 설정합니다.
		/// </summary>
		public E_SnmpPDUType PDUType
		{
			get { return m_PDUType; }
			set { m_PDUType = value; }
		}

		/// <summary>
		/// 장비의 Enterprise ObjectIdentifier를 가져오거나 설정합니다.
		/// </summary>
		public string Enterprise
		{
			get { return m_Enterprise; }
			set { m_Enterprise = value; }
		}
		
		/// <summary>
		/// Snmp Agent의 IP주소를 가져오거나 설정합니다.
		/// </summary>
		public string AgentIPAddress
		{
			get { return m_AgentIPAddress; }
			set { m_AgentIPAddress = value; }
		}

		/// <summary>
		/// Snmp Trap Type를 가져오거나 설정합니다.
		/// </summary>
		public E_GenericTrapType TrapType
		{
			get { return m_GenericTrapType; }
			set { m_GenericTrapType = value; }
		}

		/// <summary>
		/// SpecificCode를 가져오거나 설정합니다.
		/// </summary>
		public long SpecificCode
		{
			get { return m_SpecificCode; }
			set { m_SpecificCode = value; }
		}

		/// <summary>
		/// TimeTick값을 가져오거나 설정합니다.
		/// </summary>
		public long TimeTick
		{
			get { return m_TimeTick; }
			set { m_TimeTick = value; }
		}

		/// <summary>
		/// Snmp Trap의 Binding 정보가 저장될 배열을 가져오거나 설정합니다.
		/// </summary>
		public TMSSnmpVariableCollection Variables
		{
			get { return m_Variables; }
			set { m_Variables = value; }
		}

		/// <summary>
		/// 분석오류를 가져오거나 설정합니다.
		/// </summary>
		public  E_ParseError ParseError
		{
			get { return m_ParseError; }
			set { m_ParseError = value; }
		}		
	}

	/// <summary>
	/// Snmp Version2의 Trap PDU 클래스 입니다.
	/// </summary>
	public class TMSSnmpV2_TrapPDU : TMSSnmp_GetResponsePDU
	{
		/// <summary>
		/// Snmp Trap Type입니다.
		/// </summary>
		private E_GenericTrapType m_GenericTrapType = E_GenericTrapType.ColdStart;

		/// <summary>
		/// Snmp Version2의 Trap PDU 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vVersion">Snmp Version입니다.</param>
		public TMSSnmpV2_TrapPDU(E_SnmpVersion vVersion) : base(vVersion)
		{
			this.PDUType = E_SnmpPDUType.TrapV2;
		}

		/// <summary>
		/// 트랩 타입을 가져오거나 설정합니다.
		/// </summary>
		public E_GenericTrapType TrapType
		{
			get { return m_GenericTrapType; }
			set { m_GenericTrapType = value; }
		}
	}
	#endregion //SNMP V1, V2 TrapPDU 클래스 입니다 ----------------------------------------------

	#region SNMP V1, V2 GetResponsePDU 클래스 입니다 ---------------------------------------
	/// <summary>
	/// Snmp Version1, 2의 GetResponse PDU 클래스 입니다.
	/// </summary>
	public class TMSSnmp_GetResponsePDU : ITMSSnmpPDU
	{
		private E_SnmpVersion m_Version = E_SnmpVersion.Version1;
		private string m_Community = "";
		private E_SnmpPDUType m_PDUType = E_SnmpPDUType.GetResponse;
		private long m_RequestID = 0;
		private E_SnmpErrorStatus m_ErrorStatus = E_SnmpErrorStatus.noError;
		private int m_ErrorIndex = 0;

		internal TMSSnmpVariableCollection m_Variables = new TMSSnmpVariableCollection();

		private E_ParseError m_ParseError = E_ParseError.NoError;

		/// <summary>
		/// Snmp Version1, 2의 GetResponse PDU 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vVersion">SnmpVersion 입니다.</param>
		public TMSSnmp_GetResponsePDU(E_SnmpVersion vVersion)
		{
			m_Version = vVersion;
		}

		/// <summary>
		/// Snmp Version을 가져오거나 설정합니다.
		/// </summary>
		public E_SnmpVersion Version
		{
			get { return m_Version; }
			set { m_Version = value; }
		}

		/// <summary>
		/// Snmp 접근 암호를 가져오거나 설정합니다.
		/// </summary>
		public string Community
		{
			get { return m_Community; }
			set { m_Community = value; }
		}

		/// <summary>
		/// Snmp PDU Type을 가져오거나 설정합니다.
		/// </summary>
		public E_SnmpPDUType PDUType
		{
			get { return m_PDUType; }
			set { m_PDUType = value; }
		}

		/// <summary>
		/// Snmp 요청 ID를 가져오거나 설정합니다.
		/// </summary>
		public long RequestID
		{
			get { return m_RequestID; }
			set { m_RequestID = value; }
		}

		/// <summary>
		/// Snmp Error 상태를 가져오거나 설정합니다.
		/// </summary>
		public E_SnmpErrorStatus ErrorStatus
		{
			get { return m_ErrorStatus; }
			set { m_ErrorStatus = value; }
		}

		/// <summary>
		/// Snmp Error Index를 가져오거나 설정합니다.
		/// </summary>
		public int ErrorIndex
		{
			get { return m_ErrorIndex; }
			set { m_ErrorIndex = value; }
		}

		/// <summary>
		/// Snmp GetResponse의 Binding정보가 저장될 배열을 가져오거나 설정합니다.
		/// </summary>
		public TMSSnmpVariableCollection Variables
		{
			get { return m_Variables; }
			set { m_Variables = value; }
		}

		/// <summary>
		/// 분석 오류를 가져오거나 설정합니다.
		/// </summary>
		public  E_ParseError ParseError
		{
			get { return m_ParseError; }
			set { m_ParseError = value; }
		}
	}
	#endregion //SNMP V1, V2 GetResponsePDU 클래스 입니다 ---------------------------------------

	#region SNMP Variable 클래스 입니다 ----------------------------------------------------
	/// <summary>
	/// Snmp Binding정보 목록 클래스 입니다.
	/// </summary>
	[Serializable]
	public class TMSSnmpVariableCollection : MarshalByRefObject
	{
		private ArrayList m_Variables = new ArrayList();		

		/// <summary>
		/// Snmp Binding정보 목록 클래스의 기본 생성자 입니다.
		/// </summary>
		public TMSSnmpVariableCollection() {}

		/// <summary>
		/// Snmp Binding정보 목록 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vVariableCollection">복사할 Snmp Binding정보 목록 객체 입니다.</param>
		public TMSSnmpVariableCollection(TMSSnmpVariableCollection vVariableCollection) 
		{
			if(vVariableCollection != null)
			{
				for(int i = 0; i < vVariableCollection.Count; i++)
				{				
					m_Variables.Add(new TMSSnmpVariable(vVariableCollection[i]));
				}
			}
		}

		/// <summary>
		/// Snmp Binding정보의 개수를 가져옵니다.
		/// </summary>
		public int Count
		{
			get { return m_Variables.Count; }
		}
		
		/// <summary>
		/// Snmp Binding정보를 추가합니다.
		/// </summary>
		/// <param name="vVariable">추가할 Binding정보 객체 입니다.</param>
		/// <returns>Binding정보가 저장된 위치 입니다.</returns>
		public int Add(TMSSnmpVariable vVariable)
		{				
			return m_Variables.Add(vVariable);
		}

		/// <summary>
		/// Snmp Binding정보를 추가합니다.
		/// </summary>
		/// <param name="vObjectID">추가할 OID 입니다.</param>
		/// <returns>Binding정보가 저장된 위치 입니다.</returns>
		public int Add(string vObjectID)
		{
			return m_Variables.Add(new TMSSnmpVariable(vObjectID));
		}

		/// <summary>
		/// Snmp Binding정보를 추가합니다.
		/// </summary>
		/// <param name="vObjectID">추가할 OID 입니다.</param>
		/// <param name="vDataType">Snmp 데이터 타입 입니다.</param>
		/// <param name="vValue">추가할 Binding 값 입니다.</param>
		/// <returns>Binding정보가 저장된 위치 입니다.</returns>
		public int Add(string vObjectID, E_SnmpDataType vDataType, object vValue)
		{
			return m_Variables.Add(new TMSSnmpVariable(vObjectID, vDataType, vValue));
		}

		/// <summary>
		/// Snmp Binding정보를 추가합니다.
		/// </summary>
		/// <param name="vObjectID">추가할 OID 입니다.</param>
		/// <param name="vDataType">Snmp 데이터 타입 입니다.</param>
		/// <param name="vValue">추가할 Binding 값 입니다.</param>
		/// <returns>Binding정보가 저장된 위치 입니다.</returns>
		public int Add(string vObjectID, E_SnmpDataType vDataType, string vValue)
		{
			return m_Variables.Add(new TMSSnmpVariable(vObjectID, vDataType, vValue));
		}

		/// <summary>
		/// Snmp Binding정보를 추가합니다.
		/// </summary>
		/// <param name="vVariables">추가할 Snmp Binding모음 클래스 입니다.</param>
		public void AddRange(TMSSnmpVariableCollection vVariables)
		{
			m_Variables.AddRange(vVariables.m_Variables);
		}
	
		/// <summary>
		/// 지정한 배열을 추가합니다.
		/// </summary>
		/// <param name="vVariables">추가할 배열입니다.</param>
		internal void AddRange(ArrayList vVariables)
		{
			m_Variables.AddRange(vVariables);
		}

		/// <summary>
		/// 지정한 위치에서 지정한 개수의 배열을 가져옵니다.
		/// </summary>
		/// <param name="vIndex">배열을 가져올 시작 위치 입니다.</param>
		/// <param name="vCount">가져올 배열의 개수 입니다.</param>
		/// <returns>배열 입니다.</returns>
		internal ArrayList GetRange(int vIndex, int vCount)
		{
			return m_Variables.GetRange(vIndex, vCount);
		}

		/// <summary>
		/// Snmp Binding정보를 가져오거나 설정합니다.
		/// </summary>
		public TMSSnmpVariable this[int index]
		{
			get { return (TMSSnmpVariable)m_Variables[index]; }
			set { m_Variables[index] = value; }
		}

		/// <summary>
		/// Snmp Binding정보 목록을 문자열로 변환 합니다.
		/// </summary>
		/// <returns>Snmp Binding정보문자열 입니다.</returns>
		public string ToString(string vSeperator)
		{
			string tStr = "";
			
			for(int i = 0; i < m_Variables.Count; i++)
			{
				tStr += ((TMSSnmpVariable)m_Variables[i]).ObjectID + vSeperator;
				tStr += ((int)((TMSSnmpVariable)m_Variables[i]).Type).ToString() + vSeperator;
				if(((TMSSnmpVariable)m_Variables[i]).Value != null)
				{
					tStr += ((TMSSnmpVariable)m_Variables[i]).Value.ToString() + vSeperator;
				}
				else
				{
					tStr += " " + vSeperator;
				}
			}

			if(tStr != "") tStr.Remove(tStr.Length - vSeperator.Length, vSeperator.Length);
			return tStr;
		}

		/// <summary>
		/// 문자열을 Snmp Binding정보 목록으로 변경합니다.
		/// </summary>
		/// <param name="vVariableString">Snmp Binding정보 목록 문자열 입니다.</param>
		/// <param name="vSeperator">각 정보의 구분자 입니다.</param>
		/// <returns>Snmp Binding정보 목록 클래스 입니다.</returns>
		public static TMSSnmpVariableCollection Parse(string vVariableString, string vSeperator)
		{
			string [] tStr = vVariableString.Split(vSeperator.ToCharArray());
			if(tStr.Length % 3 > 0 || tStr.Length == 0) return null;

			try
			{
				TMSSnmpVariableCollection tVariables = new TMSSnmpVariableCollection();
				E_SnmpDataType tType = E_SnmpDataType.Null;
				object tObj = null;

				for(int i = 0; i < tStr.Length; i += 3)
				{
					if(tStr[i + 2].Trim() == "")
					{
						tVariables.Add(new TMSSnmpVariable(tStr[i], (E_SnmpDataType)int.Parse(tStr[i+1]), null));
					}
					else
					{
						tType = (E_SnmpDataType)int.Parse(tStr[i+1]);
						switch(tType)
						{
							case E_SnmpDataType.Integer:
								tObj = Int32.Parse(tStr[i + 2].Trim());
								break;

							case E_SnmpDataType.Counter:	
							case E_SnmpDataType.Gauge:
							case E_SnmpDataType.Sequence:
							case E_SnmpDataType.TimeTicks:
								tObj = UInt32.Parse(tStr[i + 2].Trim());
								break;

							case E_SnmpDataType.Counter64:
								tObj = UInt64.Parse(tStr[i + 2].Trim());
								break;

							case E_SnmpDataType.Null:
								break;
							default:
								tObj = tStr[i + 2].Trim();
								break;
						}

						tVariables.Add(new TMSSnmpVariable(tStr[i], (E_SnmpDataType)int.Parse(tStr[i+1]), tObj));
					}
				}
				return tVariables;
			}
			catch(Exception ex)
			{
				ex = ex;
				return null;
			}
		}
	}

	/// <summary>
	/// SNMP Binding 데이터를 저장할 클래스 입니다.
	/// </summary>
	[Serializable]
	public class TMSSnmpVariable : MarshalByRefObject
	{
		/// <summary>
		/// Object Identifier입니다.
		/// </summary>
		private string m_ObjectID = "";
		/// <summary>
		/// Snmp DataType입니다.
		/// </summary>
		private E_SnmpDataType m_Type = E_SnmpDataType.Null;
		/// <summary>
		/// 값 입니다.
		/// </summary>
		private object m_Value = null;
		/// <summary>
		/// 원본 데이터 입니다.
		/// </summary>
		private byte [] m_RawData = null;

		/// <summary>
		/// SNMP Binding 데이터를 저장할 클래스의 기본 생성자 입니다.
		/// </summary>
		public TMSSnmpVariable() {}		

		/// <summary>
		/// SNMP Binding 데이터를 저장할 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vVariable">복사할 SNMP Binding 데이터 객체 입니다.</param>
		public TMSSnmpVariable(TMSSnmpVariable vVariable)
		{
			m_ObjectID = vVariable.ObjectID;
			m_Type = vVariable.Type;
			m_Value = vVariable.Value;
			if(vVariable.RawData != null)
			{
				m_RawData = new byte [vVariable.RawData.Length];
				Array.Copy(vVariable.RawData, m_RawData, m_RawData.Length);
			}
		}

		/// <summary>
		/// SNMP Binding 데이터를 저장할 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vObjectID">Snmp Binding정보의 ObjectIdentifier입니다.</param>
		public TMSSnmpVariable(string vObjectID)
		{
			m_ObjectID = vObjectID;
		}

		/// <summary>
		/// SNMP Binding 데이터 저장 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vObjectID">값의 Object Identifier입니다.</param>
		/// <param name="vType">값의 데이터 타입 입니다.</param>
		/// <param name="vValue">SNMP데이터 값 입니다.</param>
		public TMSSnmpVariable(string vObjectID, E_SnmpDataType vType, object vValue)
		{
			m_ObjectID = vObjectID;
			m_Type = vType;
			m_Value = vValue;
		}

		/// <summary>
		/// SNMP Binding 데이터 저장 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vObjectID">값의 Object Identifier입니다.</param>
		/// <param name="vType">값의 데이터 타입 입니다.</param>
		/// <param name="vValue">SNMP데이터 값 입니다.</param>
		public TMSSnmpVariable(string vObjectID, E_SnmpDataType vType, string vValue)
		{
			m_ObjectID = vObjectID;
			m_Type = vType;
			m_Value = vValue;
		}

		/// <summary>
		/// SNMP Binding 데이터 저장 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vObjectID">값의 Object Identifier입니다.</param>
		/// <param name="vValue">날짜와 시간이 저장된 TMSDateAndTime객체 입니다.</param>
		public TMSSnmpVariable(string vObjectID, TMSDateAndTime vValue)
		{
			m_ObjectID = vObjectID;
			m_Type = E_SnmpDataType.DateAndTime;
			m_Value = vValue.GetBytes();
		}

		/// <summary>
		/// Object Identifier를 가져옵니다.
		/// </summary>
		public string ObjectID
		{
			get { return m_ObjectID; }
			set { m_ObjectID = value; }
		}

		/// <summary>
		/// SNMP 데이터 타입을 가져옵니다.
		/// </summary>
		public E_SnmpDataType Type
		{			
			get { return m_Type;}
			set { m_Type = value; }
		}

		/// <summary>
		/// SNMP데이터를 가져오거나 설정합니다. 데이터는 각데이터 타입에 맞게 입력을 하여 주십시오. OctetString타입의 경우 값을 Byte배열로 입력하실수 있습니다.
		/// </summary>
		public object Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}

		/// <summary>
		/// 원본 데이터를 가져오거나 설정합니다.
		/// </summary>
		public byte [] RawData
		{
			get { return m_RawData; }
			set { m_RawData = value; }
		}
	}
	#endregion //SNMP Variable 클래스 입니다 ----------------------------------------------------
	#endregion //SNMP PDU 클래스 입니다 ---------------------------------------------------------
}