using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace SYSLOGD
{
	#region SNMP ���� ������ �Դϴ� --------------------------------------------------------
	/// <summary>
	/// SNMP ���� ������ �Դϴ�.
	/// </summary>
	[Serializable]
	public enum E_SnmpVersion
	{
		/// <summary>
		/// Snmp Version 1 �Դϴ�.
		/// </summary>
		Version1 = 0x00,
		/// <summary>
		/// Snmp Version 2c �Դϴ�.
		/// </summary>
		Version2c = 0x01,
		/// <summary>
		/// Snmp Version 3 �Դϴ�.
		/// </summary>
		Version3 = 0x03
	}

	/// <summary>
	/// SNMP PDU Type ������ �Դϴ�.
	/// </summary>
	[Serializable]
	public enum E_SnmpPDUType
	{
		/// <summary>
		/// Snmp GetRequest �Դϴ�.
		/// </summary>
		GetRequest = 0xA0,
		/// <summary>
		/// Snmp GetNextRequest �Դϴ�.
		/// </summary>
		GetNextRequest = 0xA1,
		/// <summary>
		/// Snmp GetResponse �Դϴ�.
		/// </summary>
		GetResponse = 0xA2,
		/// <summary>
		/// Snmp SetRequest �Դϴ�.
		/// </summary>
		SetRequest = 0xA3,
		/// <summary>
		/// Snmp Trap Version 1 �Դϴ�.
		/// </summary>
		TrapV1 = 0xA4,
		/// <summary>
		/// Snmp GetBulkRequest �Դϴ�.
		/// </summary>
		GetBulkRequest = 0xA5,
		/// <summary>
		/// Snmp InformRequest �Դϴ�.
		/// </summary>
		InformRequest = 0xA6,
		/// <summary>
		/// Snmp Trap Version 2 �Դϴ�.
		/// </summary>
		TrapV2 = 0xA7,
		/// <summary>
		/// Snmp Report �Դϴ�.
		/// </summary>
		Report = 0xA8
	}

	/// <summary>
	/// SNMP���� ������ Ÿ�� ������ ��Ÿ���ϴ�.
	/// </summary>
	[Serializable]
	public enum E_SnmpDataType
	{		
		/// <summary>
		/// 4Byte Integer���Դϴ�.
		/// </summary>
		Integer = 0x02,
		/// <summary>
		/// 4Byte Integer���Դϴ�.
		/// </summary>
		Integer32 = 0x02,
		/// <summary>
		/// ���ڿ� ���Դϴ�.
		/// </summary>
		OctetString = 0x04,
		/// <summary>
		/// Null ���Դϴ�.
		/// </summary>
		Null = 0x05,
		/// <summary>
		/// ObjectIdentifier ���Դϴ�.
		/// </summary>
		ObjectIdentifier = 0x06,
		/// <summary>
		/// Sequence ���Դϴ�.
		/// </summary>
		Sequence = 0x30,
		/// <summary>
		/// Sequence Of ���Դϴ�.
		/// </summary>
		SequenceOf = 0x30,
		/// <summary>
		/// IP�ּ� ���Դϴ�.
		/// </summary>
		IPAddress = 0x40,
		/// <summary>
		/// 4Byte Integer���Դϴ�.
		/// </summary>
		Counter = 0x41,
		/// <summary>
		/// 4Byte Integer���Դϴ�.
		/// </summary>
		Counter32 = 0x41,
		/// <summary>
		/// 4Byte Integer���Դϴ�.
		/// </summary>
		Gauge = 0x42,
		/// <summary>
		/// 4Byte Integer���Դϴ�.
		/// </summary>
		Gauge32 = 0x42,
		/// <summary>
		/// TimeTicks���Դϴ�.
		/// </summary>
		TimeTicks = 0x43,
		/// <summary>
		/// Opaque���Դϴ�.
		/// </summary>
		Opaque = 0x44,
		/// <summary>
		/// 8Byte Integer �迭 ���Դϴ�.
		/// </summary>
		Counter64 = 0x46,			//Snmp ver 2	
	
		//������ Library���� �߰��� �����ϴ� ���Դϴ�.
		/// <summary>
		/// MAC Address�� �Դϴ�.(AA:BB:CC:20:2C:AC);
		/// </summary>
		MACAddress = 0xFF01,
		/// <summary>
		/// ��¥ �� �ð� ���Դϴ�.
		/// </summary>
		DateAndTime = 0xFF02
	}

	/// <summary>
	/// SNMP ���� ���� �ڵ� ������ �Դϴ�.
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
	/// Standard TrapŸ�� ������ �Դϴ�.
	/// </summary>
	[Serializable]
	public enum E_GenericTrapType
	{
		/// <summary>
		/// ����� ������ ������ ������ ��� �߻��մϴ�.
		/// </summary>
		ColdStart,
		/// <summary>
		/// ��� ������Ͽ������ �߻��մϴ�.
		/// </summary>
		WarmStart,
		/// <summary>
		/// ����� ��Ʈ�� �ٿ�Ǿ������ �߻��մϴ�.
		/// </summary>
		LinkDown,
		/// <summary>
		/// ����� ��Ʈ�� ���Ǿ������ �߻��մϴ�.
		/// </summary>
		LinkUp,
		/// <summary>
		/// ��� ���ӽ� ������ ������� �߻��մϴ�.
		/// </summary>
		AuthenticationFailure,
		/// <summary>
		/// BGP�� ���õ� Ʈ���Դϴ�.
		/// </summary>
		EgpNeighborLoss,
		/// <summary>
		/// ����ȸ�翡�� ���� Ʈ���Դϴ�.
		/// </summary>
		EnterpriseSpecific,		
	}		

	/// <summary>
	/// �м� Error ������ �Դϴ�.
	/// </summary>
	[Serializable]
	public enum E_ParseError
	{
		/// <summary>
		/// �м��� ������ �߻����� �ʾҽ��ϴ�.
		/// </summary>
		NoError = 0,
		/// <summary>
		/// SNMP ��Ŷ�� �߸��Ǿ����ϴ�.
		/// </summary>
		IncorrectPacket
	}

	/// <summary>
	/// SNMP V3 ���� �������� �������Դϴ�.
	/// </summary>
	[Serializable]
	public enum E_SnmpV3AuthenticationProtocol
	{
		/// <summary>
		/// ���� ���������� ������� �ʽ��ϴ�.
		/// </summary>
		None,
		/// <summary>
		/// ���� �������ݷ� HMAC_MD5�� ����մϴ�.
		/// </summary>
		HMAC_MD5,
		/// <summary>
		/// ���� �������ݷ� HMAC_SHA�� ����մϴ�.
		/// </summary>
		HMAC_SHA
	}

	/// <summary>
	/// SNMP V3 �����̹��� �������� �������Դϴ�.
	/// </summary>
	[Serializable]
	public enum E_SnmpV3PrivacyProtocol
	{
		/// <summary>
		/// �����̹��� ���������� ������� �ʽ��ϴ�.
		/// </summary>
		None,
		/// <summary>
		/// �����̹��� �������� CBC_DES�� ����մϴ�.
		/// </summary>
		CBC_DES,
		/// <summary>
		/// �����̹��� �������� CFB_AES_128�� ����մϴ�.
		/// </summary>
		CFB_AES_128
	}
	#endregion //SNMP ���� ������ �Դϴ� --------------------------------------------------------

	/// <summary>
	/// SNMP ������ �м� Ŭ���� �Դϴ�.
	/// </summary>
	public class SOSnmpClass
	{
		#region SNMP ���� �⺻ ���� �Դϴ� -------------------------------------------------
		/// <summary>
		/// Snmp Version2 Trap�� ������ ��Ÿ���� OID�Դϴ�.
		/// </summary>
		public static string [] A_SnmpV2TrapOID = new string []
		{
			"1.3.6.1.6.3.1.1.4.1.0",	//GenericTrap
			"1.3.6.1.6.3.1.1.4.3.0"		//EnterpriseTrap
		};

		/// <summary>
		/// Snmp Version2 TrapType�� ��Ÿ���� OID�Դϴ�. 
		/// </summary>
		public static string [] A_SnmpV2GenericTrapOID = new string []
		{
			"1.3.6.1.6.3.1.1.5.1",	//ColdStart
			"1.3.6.1.6.3.1.1.5.2",	//WarmStart
			"1.3.6.1.6.3.1.1.5.3",	//LinkDown
			"1.3.6.1.6.3.1.1.5.4",	//LinkUp
			"1.3.6.1.6.3.1.1.5.5"	//AuthticationFailure
		};
		#endregion //SNMP ���� �⺻ ���� �Դϴ� -------------------------------------------------

		#region SNMP Decode �κ� �Դϴ� ----------------------------------------------------
		/// <summary>
		/// SNMP PDU�� �м��մϴ�.
		/// </summary>
		/// <param name="vData">�м��� SNMP������ �Դϴ�.</param>
		/// <returns>�м��� ����� ��ȯ �մϴ�.</returns>
		public static ITMSSnmpPDU DecodeSnmpPDU(byte [] vData)
		{
			ITMSSnmpPDU tPDU = null;

			E_SnmpVersion tVersion = E_SnmpVersion.Version1;

			//���� Byte�� Sequence���� Ȯ�� �մϴ�.
			if((E_SnmpDataType)vData[0] != E_SnmpDataType.Sequence)
			{
				return null;
			}

			int tIndex = 1;
			int tPDULength = DecodeLength(vData, ref tIndex);
			
			//SNMP Version�� Ȯ���մϴ�.
			if((E_SnmpDataType)vData[tIndex] != E_SnmpDataType.Integer)
			{
				return null;
			}
			tIndex++;
			tVersion = (E_SnmpVersion)DecodeInteger32(vData, ref tIndex);

			switch(tVersion)
			{
				case E_SnmpVersion.Version1:	//PDU�� SNMP V1�ΰ��
				case E_SnmpVersion.Version2c:	//PDU�� SNmp V2c�� ���					
					tPDU = DecodeSnmp_PDU(tVersion, vData, tIndex);
					break;

				case E_SnmpVersion.Version3:	//PDU�� SNmp V3�� ���					
					break;
			}
			return tPDU;
		}
		
		/// <summary>
		/// SNMP Version1, 2 PDU�� �м��մϴ�.
		/// </summary>
		/// <param name="vVersion">Snmp Version�Դϴ�.</param>
		/// <param name="vData">Decode�� ������ �Դϴ�.</param>
		/// <param name="vIndex">Deocde�� ������ ��ġ�Դϴ�.</param>
		/// <returns>Decode�� PDU��ü �Դϴ�.</returns>
		private static ITMSSnmpPDU DecodeSnmp_PDU(E_SnmpVersion vVersion, byte [] vData, int vIndex)
		{
			ITMSSnmpPDU tPDU = null;

			string tCommunity = "";
			E_SnmpPDUType tPDUType = E_SnmpPDUType.TrapV1;

			//SNMP Community�� Ȯ���մϴ�.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.OctetString)
			{
				return null;
			}
			vIndex++;

			byte [] tRD = null;
			tCommunity = DecodeOctetString(vData, ref vIndex, ref tRD);

			//SNMP PDU Type�� ����ϴ�.
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

					//SnmpV2 Generic Trap�ΰ��
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
						//SnmpV2 Enterprise Trap�ΰ��
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
		/// SNMP Version1, 2 Trap�� �м��մϴ�.
		/// </summary>
		/// <param name="vData">Decode�� ������ �Դϴ�.</param>
		/// <param name="vIndex">Deocde�� ������ ��ġ�Դϴ�.</param>
		/// <param name="vValueObj">Decode����� ����� Trap PDU��ü �Դϴ�.</param>
		internal static void DecodeSnmp_Trap(byte [] vData, int vIndex, ref TMSSnmp_TrapPDU vValueObj)
		{
			//Trap PDU�� ���̸� ����ϴ�.
			int tLength = DecodeLength(vData, ref vIndex);

			//OID���� Ȯ���ϰ� Enterprise OID�� �б� �մϴ�.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.ObjectIdentifier)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.Enterprise = DecodeObjectID(vData, ref vIndex);

			//Agent�ּҸ� ����ϴ�.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.IPAddress)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.AgentIPAddress = DecodeIPAddress(vData, ref vIndex);

			//Trap Type�� ����ϴ�.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Integer)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.TrapType = (E_GenericTrapType)DecodeInteger32(vData, ref vIndex);

			//Specific Code�� ����ϴ�.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Integer)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.SpecificCode = DecodeInteger32(vData, ref vIndex);

			//TimeTicks���� ����ϴ�.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.TimeTicks)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.TimeTick = DecodeInteger32(vData, ref vIndex);

			//Sequence���� Ȯ�� �մϴ�.
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
		/// SNMP V1, V2 GetResponse PDU�� �м��մϴ�.
		/// </summary>
		/// <param name="vData">�м��� SNMP�����Ͱ� ����ִ� Byte�迭�Դϴ�.</param>
		/// <param name="vIndex">�м��� �������� ������ġ �Դϴ�.</param>
		/// <param name="vValueObj">�м��� ���� ����� TMSSnmpPDUDataŬ���� ��ü �Դϴ�.</param>
		internal static void DecodeSnmp_GetResponse(byte [] vData, int vIndex, ref TMSSnmp_GetResponsePDU vValueObj)
		{
			//Trap PDU�� ���̸� ����ϴ�.
			int tLength = DecodeLength(vData, ref vIndex);

			//Get ��û ID�� ����ϴ�.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Integer)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.RequestID = DecodeInteger32(vData, ref vIndex);

			//Error Status�� ����ϴ�.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Integer)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.ErrorStatus = (E_SnmpErrorStatus)DecodeInteger32(vData, ref vIndex);

			//Error Index�� ����ϴ�.
			if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Integer)
			{
				vValueObj.ParseError = E_ParseError.IncorrectPacket;
				return;
			}
			vIndex++;
			vValueObj.ErrorIndex = DecodeInteger32(vData, ref vIndex);
			
			//Sequence���� Ȯ�� �մϴ�.
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
		/// SNMP Variable Bind�κ��� �м��մϴ�.
		/// </summary>
		/// <param name="vData">�м��� SNMP�����Ͱ� ����ִ� Byte�迭�Դϴ�.</param>
		/// <param name="vIndex">�м��� �������� ������ġ �Դϴ�.</param>
		/// <param name="vLength">�м��� �������� ��ü ���� �Դϴ�.</param>
		/// <param name="vValueObj">�м��� ���� ����� TMSSnmpVariableCollectionŬ���� ��ü �Դϴ�.</param>
		/// <returns>�м��� �������θ� ��ȯ �մϴ�.</returns>
		internal static E_ParseError DecodeVariables(byte [] vData, int vIndex, int vLength, ref TMSSnmpVariableCollection vValueObj)
		{
			vLength += vIndex;

			while(vIndex < vLength)
			{
				TMSSnmpVariable tVariable = new TMSSnmpVariable();

				//Sequence���� Ȯ�� �մϴ�.
				if((E_SnmpDataType)vData[vIndex] != E_SnmpDataType.Sequence)
				{
					return E_ParseError.IncorrectPacket;
				}
				vIndex++;
				DecodeLength(vData, ref vIndex);

				//OID���� Ȯ���ϰ� Enterprise OID�� �б� �մϴ�.
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

		#region SNMP DataType Decode �κ� �Դϴ� -------------------------------------------
		/// <summary>
		/// Length�� �м��Ͽ� Payload�� ���̸� �����ɴϴ�.
		/// </summary>
		/// <param name="vData">SNMP �����Ͱ� ����ִ� Byte�迭�Դϴ�.</param>
		/// <param name="vIndex">Length�� ���� ��ġ �Դϴ�. ��� ��ȯ�� �ε����� Length�� ���� ��ġ �Դϴ�.</param>
		/// <returns>Payload�� ���� �Դϴ�.</returns>
		public static int DecodeLength(byte [] vData, ref int vIndex)
		{			
			int tLengthCount = 0;
			int tLength = 0;

			//Decode Rule ------------------------------------------------------------------
			// Length�� Payload������ Decode�� Byte�� �ֻ��� Bit�� 1�ϰ�� ���� Byte��
			// �ֻ��� Bit�� ������ ������ �κ��� Length�� ��Ÿ���� Byte�� ���̰� �˴ϴ�.
			// ���� �ֻ��� ��Ʈ�� 1�̰� �������κ��� ���� 3�ϰ�� ���� 3���� Byte�� ���̸�
			// ��Ÿ���� Byte���˴ϴ�.
			// �ֻ��� Bit�� 0�� ���� Byte��ü�� Payload�� ���̸� ��Ÿ���� �˴ϴ�.

			//Length�� ���̰� 1Byte�̻��� ���
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
			else //Length�� ���̰� 1Byte�� ���
			{
				tLengthCount = (int)vData[vIndex];
				vIndex++;
				return tLengthCount;
			}
		}		

		/// <summary>
		/// �������� Decode�մϴ�.
		/// </summary>
		/// <param name="vData">SNMP �����Ͱ� ����ִ� Byte�迭�Դϴ�.</param>
		/// <param name="vIndex">Integer �������� ������ġ �Դϴ�. �޼ҵ� ó���� �ε����� Integer �������� ��ġ �Դϴ�.</param>
		/// <returns>Decode�� Integer������ �Դϴ�.</returns>
		public static int DecodeInteger32(byte [] vData, ref int vIndex)
		{
			//Decode Rule ------------------------------------------------------------------
			// ���� Byte�� ������ ���� Byte ���̸� ��Ÿ���ϴ�.
			// ���� Byte�� ���� Byte���� ���̸�ŭ�� ������ �Դϴ�.				

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
		/// �������� Decode�մϴ�.
		/// </summary>
		/// <param name="vData">SNMP �����Ͱ� ����ִ� Byte�迭�Դϴ�.</param>
		/// <param name="vIndex">Integer �������� ������ġ �Դϴ�. �޼ҵ� ó���� �ε����� Integer �������� ��ġ �Դϴ�.</param>
		/// <returns>Decode�� Integer������ �Դϴ�.</returns>
		public static UInt32 DecodeUnsignedInteger32(byte [] vData, ref int vIndex)
		{
			//Decode Rule ------------------------------------------------------------------
			// ���� Byte�� ������ ���� Byte ���̸� ��Ÿ���ϴ�.
			// ���� Byte�� ���� Byte���� ���̸�ŭ�� ������ �Դϴ�.				

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
		/// �������� Decode�մϴ�.
		/// </summary>
		/// <param name="vData">SNMP �����Ͱ� ����ִ� Byte�迭�Դϴ�.</param>
		/// <param name="vIndex">Integer �������� ������ġ �Դϴ�. �޼ҵ� ó���� �ε����� Integer �������� ��ġ �Դϴ�.</param>
		/// <returns>Decode�� Integer������ �Դϴ�.</returns>
		public static long DecodeInteger64(byte [] vData, ref int vIndex)
		{
			//Decode Rule ------------------------------------------------------------------
			// ���� Byte�� ������ ���� Byte ���̸� ��Ÿ���ϴ�.
			// ���� Byte�� ���� Byte���� ���̸�ŭ�� ������ �Դϴ�.

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
		/// �������� Decode�մϴ�.
		/// </summary>
		/// <param name="vData">SNMP �����Ͱ� ����ִ� Byte�迭�Դϴ�.</param>
		/// <param name="vIndex">Integer �������� ������ġ �Դϴ�. �޼ҵ� ó���� �ε����� Integer �������� ��ġ �Դϴ�.</param>
		/// <returns>Decode�� Integer������ �Դϴ�.</returns>
		public static UInt64 DecodeUnsignedInteger64(byte [] vData, ref int vIndex)
		{
			//Decode Rule ------------------------------------------------------------------
			// ���� Byte�� ������ ���� Byte ���̸� ��Ÿ���ϴ�.
			// ���� Byte�� ���� Byte���� ���̸�ŭ�� ������ �Դϴ�.

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
		/// ���ڿ��� Decode�մϴ�.
		/// </summary>
		/// <param name="vData">SNMP �����Ͱ� ����ִ� Byte�迭�Դϴ�.</param>
		/// <param name="vIndex">String �������� ������ġ �Դϴ�. �޼ҵ� ó���� �ε����� String �������� ���� ��ġ �Դϴ�.</param>
		/// <param name="vRawData">Decode���� ���� ���� �迭�Դϴ�.</param>
		/// <returns>Decode�� String������ �Դϴ�.</returns>
		public static string DecodeOctetString(byte [] vData, ref int vIndex, ref byte [] vRawData)
		{
			//			//Encode Rule ------------------------------------------------------------------
			//			// DecodeOctetString�� Decode Rule�� ���� �Ͻʽÿ�.
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
			// DecodeOctetString�� Decode Rule�� ���� �Ͻʽÿ�.
			
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
					//���� 16�������� Ȯ�� �մϴ�.
					//if(vData[vIndex+i] < 16)
					if(((int)vData[vIndex+i] < 32 || (int)vData[vIndex+i] > 125))
					{
						//������ �����Ͱ� 16������ �ƴϾ�����
						if(hCount == 0)
						{
							//���������� �����͸� ���ڿ��� �����մϴ�.
							tValue += System.Text.Encoding.Default.GetString(vData, sIdx, (vIndex + i) - sIdx);
							sIdx = vIndex+i;
						}
						//16���� ������ ������ŵ�ϴ�.
						hCount++;
					}
					else //�Ϲ� ���ڿ��ΰ���� ó���Դϴ�.
					{
						//������ 16������ ������ ������
						if(hCount > 0)
						{							
							if(hCount == 6) //MAC�ּ����� Ȯ���մϴ�.
							{
								//16������ ���ڿ��� �����մϴ�.
								tStr = BitConverter.ToString(vData, sIdx, (int)hCount);
								//MAC�ּ��� �������� �����մϴ�.
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
									//16������ ���ڿ��� �����մϴ�.
									tStr = BitConverter.ToString(vData, sIdx, (int)hCount);
									tValue += System.Text.RegularExpressions.Regex.Replace(tStr, "-", "");
								}								
							}
							//16������ ������ �ʱ�ȭ �մϴ�.
							hCount = 0;
							sIdx = vIndex+i;
						}
					}
				}

				//������ �����Ͱ� 16������ �ƴϾ�����
				if(hCount == 0)
				{
					//���������� �����͸� ���ڿ��� �����մϴ�.
					tValue += System.Text.Encoding.Default.GetString(vData, sIdx, (vIndex + i) - sIdx);
				}
				else
				{					
					if(hCount == 6) //MAC�ּ����� Ȯ���մϴ�.
					{
						//16������ ���ڿ��� �����մϴ�.
						tStr = BitConverter.ToString(vData, sIdx, (int)hCount);
						//MAC�ּ��� �������� �����մϴ�.
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
							//16������ ���ڿ��� �����մϴ�.
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
		/// ObjectIdentifier�� Decode�մϴ�.
		/// </summary>
		/// <param name="vData">SNMP �����Ͱ� ����ִ� Byte�迭�Դϴ�.</param>
		/// <param name="vIndex">ObjectID �������� ������ġ �Դϴ�. �޼ҵ� ó���� �ε����� ObjectID �������� ���� ��ġ �Դϴ�.</param>
		/// <returns>Decode�� ObjectID������ �Դϴ�.</returns>
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
		/// IP�ּҸ� Decode�մϴ�.
		/// </summary>
		/// <param name="vData">SNMP �����Ͱ� ����ִ� Byte�迭�Դϴ�.</param>
		/// <param name="vIndex">IP�ּ� �������� ������ġ �Դϴ�. �޼ҵ� ó���� �ε����� IP�ּ� �������� ���� ��ġ �Դϴ�.</param>
		/// <returns>Decode�� IP�ּ� ������ �Դϴ�.</returns>
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
		/// Null�����͸� Decode�մϴ�.
		/// </summary>
		/// <param name="vData">SNMP �����Ͱ� ����ִ� Byte�迭�Դϴ�.</param>
		/// <param name="vIndex">Null �������� ������ġ �Դϴ�. �޼ҵ� ó���� �ε����� Null�������� ���� ��ġ �Դϴ�.</param>
		/// <returns></returns>
		public static string DecodeNull(byte [] vData, ref int vIndex)
		{
			vIndex++;
			return "";
		}
		#endregion //SNMP DataType Decode �κ� �Դϴ� -------------------------------------------
		#endregion //SNMP Decode �κ� �Դϴ� ----------------------------------------------------

		#region SNMP Encode �κ� �Դϴ� ----------------------------------------------------
		/// <summary>
		/// SetRequest PDU�� Encode�մϴ�.
		/// </summary>
		/// <param name="vVersion">Snmp Version�Դϴ�.</param>
		/// <param name="vCommunity">Snmp Community�Դϴ�.</param>
		/// <param name="vRequestID">GetRequest�� RequestID�Դϴ�.</param>
		/// <param name="vVariables">Variable�迭�Դϴ�.</param>
		/// <returns>Encode�� ����� ��ȯ �մϴ�.</returns>
		public static byte [] EncodeSetRequest(E_SnmpVersion vVersion, string vCommunity, int vRequestID, TMSSnmpVariableCollection vVariables)
		{
			return EncodeSnmpGetSetPDU(vVersion, vCommunity, E_SnmpPDUType.SetRequest, vRequestID, 0, 0, vVariables);
		}

		/// <summary>
		/// GetRequest PDU�� Encode�մϴ�.
		/// </summary>
		/// <param name="vVersion">Snmp Version�Դϴ�.</param>
		/// <param name="vCommunity">Snmp Community�Դϴ�.</param>
		/// <param name="vRequestID">GetRequest�� RequestID�Դϴ�.</param>
		/// <param name="vVariables">Variable�迭�Դϴ�.</param>
		/// <returns>Encode�� ����� ��ȯ �մϴ�.</returns>
		public static byte [] EncodeGetRequest(E_SnmpVersion vVersion, string vCommunity, int vRequestID, TMSSnmpVariableCollection vVariables)
		{
			return EncodeSnmpGetSetPDU(vVersion, vCommunity, E_SnmpPDUType.GetRequest, vRequestID, 0, 0, vVariables);
		}

		/// <summary>
		/// GetNextRequest PDU�� Encode�մϴ�.
		/// </summary>
		/// <param name="vVersion">Snmp Version�Դϴ�.</param>
		/// <param name="vCommunity">Snmp Community�Դϴ�.</param>
		/// <param name="vRequestID">GetRequest�� RequestID�Դϴ�.</param>		
		/// <param name="vVariables">Variable�迭�Դϴ�.</param>
		/// <returns>Encode�� ����� ��ȯ �մϴ�.</returns>
		public static byte [] EncodeGetNextRequest(E_SnmpVersion vVersion, string vCommunity, int vRequestID, TMSSnmpVariableCollection vVariables)
		{	
			return EncodeSnmpGetSetPDU(vVersion, vCommunity, E_SnmpPDUType.GetNextRequest, vRequestID, 0, 0, vVariables);			
		}

		/// <summary>
		/// SetRequest PDU�� Encode�մϴ�.
		/// </summary>
		/// <param name="vVersion">Snmp Version�Դϴ�.</param>
		/// <param name="vCommunity">Snmp Community�Դϴ�.</param>
		/// <param name="vRequestID">GetRequest�� RequestID�Դϴ�.</param>
		/// <param name="vNon_Repeaters"></param>
		/// <param name="vMax_Repetition"></param>
		/// <param name="vVariables">Variable�迭�Դϴ�.</param>
		/// <returns>Encode�� ����� ��ȯ �մϴ�.</returns>
		public static byte [] EncodeGetBulkRequest(E_SnmpVersion vVersion, string vCommunity, int vRequestID, int vNon_Repeaters, int vMax_Repetition, TMSSnmpVariableCollection vVariables)
		{
			return EncodeSnmpGetSetPDU(vVersion, vCommunity, E_SnmpPDUType.GetBulkRequest, vRequestID, vNon_Repeaters, vMax_Repetition, vVariables);
		}

		/// <summary>
		/// Snmp�� Get, GetNext, GetBulk, Set PDU�� Encode�մϴ�.
		/// </summary>
		/// <param name="vVersion">Snmp Version�Դϴ�.</param>
		/// <param name="vCommunity">Snmp Version1, 2�� ���� ��ȣ �Դϴ�.</param>
		/// <param name="vPDUType">Snmp PDU Ÿ�� �Դϴ�.</param>
		/// <param name="vRequestID">Snmp ��û ID�Դϴ�.</param>
		/// <param name="vNon_Repeaters"></param>
		/// <param name="vMax_Repetition"></param>
		/// <param name="vVariables">Snmp ��û ������ ����ִ� ���� �迭�Դϴ�.</param>
		/// <returns>Encode�� Snmp PDU���� �迭�Դϴ�.</returns>
		private static byte [] EncodeSnmpGetSetPDU(E_SnmpVersion vVersion, string vCommunity, E_SnmpPDUType vPDUType, int vRequestID, int vNon_Repeaters, int vMax_Repetition, TMSSnmpVariableCollection vVariables)
		{
			ArrayList tData = new ArrayList();
			
			//������ Encode�մϴ�.
			tData.Add((byte)E_SnmpDataType.Integer);
			tData.AddRange(EncodeInteger32((Int32)vVersion));

			//Community�� Encode�մϴ�.
			tData.Add((byte)E_SnmpDataType.OctetString);
			tData.AddRange(EncodeOctetString(vCommunity));

			//PDU Type�� Encode�մϴ�.
			tData.Add((byte)vPDUType);
			
			int tIndex = tData.Count;

			//RequestID�� Encode�մϴ�.
			tData.Add((byte)E_SnmpDataType.Integer);
			tData.AddRange(EncodeInteger32((Int32)vRequestID));

			//ErrorStatus�� Encode�մϴ�.
			tData.Add((byte)E_SnmpDataType.Integer);
			tData.AddRange(EncodeInteger32(vNon_Repeaters));

			//ErrorIndex�� Encode�մϴ�.
			tData.Add((byte)E_SnmpDataType.Integer);
			tData.AddRange(EncodeInteger32(vMax_Repetition));
			
			//Variable Bind�� Encode�մϴ�.
			ArrayList tVariables = EncodeVariables(vVariables);

			//Variable Bind Sequence�� Encode�մϴ�.
			tData.Add((byte)E_SnmpDataType.Sequence);
			tData.AddRange(EncodeLength(tVariables.Count));

			//Variable Bind�� �߰��մϴ�.
			tData.AddRange(tVariables);

			//PDU�� ���̸� Encode�մϴ�.
			int tCount  = tData.Count - tIndex;
			tData.InsertRange(tIndex, EncodeLength(tCount));

			//SNMP Sequence�� Encode�մϴ�.
			tCount = tData.Count;
			tData.Insert(0, (byte)E_SnmpDataType.Sequence);
			tData.InsertRange(1, EncodeLength(tCount));
			
			byte [] tBytes = new byte[tData.Count];
			Array.Copy(tData.ToArray(), tBytes, tData.Count);
	
			return tBytes;
		}

		/// <summary>
		/// SNMP Version1 PDU�� ���� �մϴ�.
		/// </summary>
		/// <param name="vPDU">Encode�� PDU������ ����ִ� ��ü �Դϴ�.</param>
		/// <returns>������ PDU �������� Byte�迭 �Դϴ�.</returns>
		public static byte [] EncodeSnmpPDU(ITMSSnmpPDU vPDU)
		{		
			if(vPDU == null) return null;

			ArrayList tData = new ArrayList();	

			//������ Encode�մϴ�.
			tData.Add((byte)E_SnmpDataType.Integer);
			tData.AddRange(EncodeInteger32((Int32)vPDU.Version));

			switch(vPDU.Version)
			{					
				case E_SnmpVersion.Version1:
				case E_SnmpVersion.Version2c:
					//PDU�� SNMP V1�ΰ��
					EncodeSnmp_PDU(vPDU, ref tData);
					break;

				case E_SnmpVersion.Version3:
					//PDU�� SNmp V3�� ���
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
		/// SNMP Version1 PDU�� ���� �մϴ�.
		/// </summary>
		/// <param name="vPDU">Encode�� PDU������ ����ִ� ��ü �Դϴ�.</param>
		/// <param name="vData">Encode�� ������ ����� �迭�Դϴ�.</param>
		/// <returns>������ PDU �������� Byte�迭 �Դϴ�.</returns>		
		internal static void EncodeSnmp_PDU(ITMSSnmpPDU vPDU, ref ArrayList vData)
		{
			switch(vPDU.PDUType)
			{
				case E_SnmpPDUType.GetResponse:
					//Community�� Encode�մϴ�.
					vData.Add((byte)E_SnmpDataType.OctetString);
					vData.AddRange(EncodeOctetString(((TMSSnmp_GetResponsePDU)vPDU).Community));

					//PDU Type�� Encode�մϴ�.
					vData.Add((byte)vPDU.PDUType);					

					EncodeSnmp_GetResponse((TMSSnmp_GetResponsePDU)vPDU, ref vData);
					break;

				case E_SnmpPDUType.TrapV1:
					//Community�� Encode�մϴ�.
					vData.Add((byte)E_SnmpDataType.OctetString);
					vData.AddRange(EncodeOctetString(((TMSSnmp_TrapPDU)vPDU).Community));

					//PDU Type�� Encode�մϴ�.
					vData.Add((byte)vPDU.PDUType);

					EncodeSnmp_Trap((TMSSnmp_TrapPDU)vPDU, ref vData);
					break;

				case E_SnmpPDUType.TrapV2:
					//Community�� Encode�մϴ�.
					vData.Add((byte)E_SnmpDataType.OctetString);
					vData.AddRange(EncodeOctetString(((TMSSnmpV2_TrapPDU)vPDU).Community));

					//PDU Type�� Encode�մϴ�.
					vData.Add((byte)vPDU.PDUType);

					EncodeSnmp_GetResponse((TMSSnmp_GetResponsePDU)vPDU, ref vData);
					break;
			}
		}

		/// <summary>
		/// SNMP Version1 Trap�� �м��մϴ�.
		/// </summary>
		/// <param name="vPDU">Encode�� PDU������ ����ִ� ��ü �Դϴ�.</param>
		/// <param name="vData">Encode�� ������ ����� �迭�Դϴ�.</param>
		public static void EncodeSnmp_Trap(TMSSnmp_TrapPDU vPDU, ref ArrayList vData)
		{
			int tIndex = vData.Count;
			ArrayList tVariableData = null;
				
			//OID�� Encode�մϴ�.
			vData.Add((byte)E_SnmpDataType.ObjectIdentifier);
			vData.AddRange(EncodeObjectID(vPDU.Enterprise));

			//Agent�ּҸ� Encode�մϴ�.
			vData.Add((byte)E_SnmpDataType.IPAddress);
			vData.AddRange(EncodeIPAddress(vPDU.AgentIPAddress));

			//Trap Type�� Encode�մϴ�.				
			vData.Add((byte)E_SnmpDataType.Integer);
			vData.AddRange(EncodeInteger32((Int32)vPDU.TrapType));

			//Specific Code�� Encode�մϴ�.
			vData.Add((byte)E_SnmpDataType.Integer);
			vData.AddRange(EncodeInteger32((Int32)vPDU.SpecificCode));

			//Time Ticks�� Encode�մϴ�.
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
				
			//Variable������ Sequence�� �����մϴ�.
			vData.Add((byte)E_SnmpDataType.Sequence);
			vData.AddRange(EncodeLength(tVariableData.Count));

			//Variable�� ���� �մϴ�.
			vData.AddRange(tVariableData);
				
			//PDU��ü ���̸� Encode�մϴ�.
			int tCount = vData.Count - tIndex;
			vData.InsertRange(tIndex, EncodeLength(tCount));
		}

		/// <summary>
		/// SNMP V1, V2 GetResponse PDU�� Enocde�մϴ�.
		/// </summary>
		/// <param name="vPDU">Encode�� PDU������ ����ִ� ��ü �Դϴ�.</param>
		/// <param name="vData">Encode�� ������ ����� �迭�Դϴ�.</param>
		internal static void EncodeSnmp_GetResponse(TMSSnmp_GetResponsePDU vPDU, ref ArrayList vData)
		{
			int tIndex = vData.Count;
			ArrayList tVariableData = null;

			//OID�� Encode�մϴ�.
			vData.Add((byte)E_SnmpDataType.Integer);
			vData.AddRange(EncodeInteger32((Int32)vPDU.RequestID));

			//Error Status�� Encode�մϴ�.
			vData.Add((byte)E_SnmpDataType.Integer);
			vData.AddRange(EncodeInteger32((Int32)vPDU.ErrorStatus));

			//Error Index�� Encode�մϴ�.
			vData.Add((byte)E_SnmpDataType.Integer);
			vData.AddRange(EncodeInteger32((Int32)vPDU.ErrorIndex));
			
			if(vPDU.Variables.Count > 0)
			{
				tVariableData = EncodeVariables(vPDU.Variables);
			}
				
			//Variable������ Sequence�� �����մϴ�.
			vData.Add((byte)E_SnmpDataType.Sequence);
			vData.AddRange(EncodeLength(tVariableData.Count));

			//Variable�� ���� �մϴ�.
			vData.AddRange(tVariableData);
				
			//PDU��ü ���̸� Encode�մϴ�.
			int tCount = vData.Count - tIndex;
			vData.InsertRange(tIndex, EncodeLength(tCount));
		}

		/// <summary>
		/// SNMP Variable Bind�κ��� �����մϴ�.
		/// </summary>
		/// <param name="vValueObj">������ ���� ����� TMSSnmpVariableCollectionŬ���� ��ü �Դϴ�.</param>
		/// <returns>������ Varaible Bind�� Byte�迭 �Դϴ�.</returns>
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
								throw new ArgumentException("������ �� " + (string)vValueObj[i].Value + "��(��) ������ ���� " + vValueObj[i].Type.ToString() + "�� ��ȯ �� �� �����ϴ�.");
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
								throw new ArgumentException("������ �� " + (string)vValueObj[i].Value + "��(��) ������ ���� " + vValueObj[i].Type.ToString() + "�� ��ȯ �� �� �����ϴ�.");
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
								throw new ArgumentException("������ �� " + (string)vValueObj[i].Value + "��(��) ������ ���� " + vValueObj[i].Type.ToString() + "�� ��ȯ �� �� �����ϴ�.");
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

		#region SNMP DataType Encode �κ� �Դϴ� -------------------------------------------
		/// <summary>
		/// ������ Payload�� ���̸� Encode�մϴ�.
		/// </summary>
		/// <param name="vLength">Encode�� Payload�� ���� �Դϴ�.</param>
		/// <returns>Encode�� Payload�� ���� �Դϴ�.</returns>
		public static byte [] EncodeLength(int vLength)
		{
			byte [] tLength = null;			

			//Encode Rule ------------------------------------------------------------------
			// DecodeLength�� Decode Rule�� ���� �Ͻʽÿ�.

			//���̸� ��Byte�� ��Ÿ���� ���� ���
			if(vLength < 127)
			{
				tLength = new byte [] { (byte)vLength };
			}
			else //���̸� 1Byte�� ��Ÿ�� �� �������
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
		/// ������ �������� Encode�մϴ�.
		/// </summary>
		/// <param name="vValue">Encode�� ������ ������ �Դϴ�.</param>
		/// <returns>Encode�� �������� Byte�迭�Դϴ�.</returns>
		public static byte [] EncodeInteger32(Int32 vValue)
		{
			//Encode Rule ------------------------------------------------------------------
			// DecodeInteger�� Decode Rule�� ���� �Ͻʽÿ�.
			
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
		/// ������ �������� Encode�մϴ�.
		/// </summary>
		/// <param name="vValue">Encode�� ������ ������ �Դϴ�.</param>
		/// <returns>Encode�� �������� Byte�迭�Դϴ�.</returns>
		public static byte [] EncodeUnsignedInteger32(UInt32 vValue)
		{
			//Encode Rule ------------------------------------------------------------------
			// DecodeInteger�� Decode Rule�� ���� �Ͻʽÿ�.
			
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
		/// ������ �������� Encode�մϴ�.
		/// </summary>
		/// <param name="vValue">Encode�� ������ ������ �Դϴ�.</param>
		/// <returns>Encode�� �������� Byte�迭�Դϴ�.</returns>
		public static byte [] EncodeInteger64(Int64 vValue)
		{
			//Encode Rule ------------------------------------------------------------------
			// DecodeInteger�� Decode Rule�� ���� �Ͻʽÿ�.
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
		/// ������ �������� Encode�մϴ�.
		/// </summary>
		/// <param name="vValue">Encode�� ������ ������ �Դϴ�.</param>
		/// <returns>Encode�� �������� Byte�迭�Դϴ�.</returns>
		public static byte [] EncodeUnsignedInteger64(UInt64 vValue)
		{
			//Encode Rule ------------------------------------------------------------------
			// DecodeInteger�� Decode Rule�� ���� �Ͻʽÿ�.
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
		/// ObjectID�� Encode�մϴ�.
		/// </summary>
		/// <param name="vValue">Encode�� ObjectID�Դϴ�.</param>
		/// <returns>Encode�� ObjectID�Դϴ�.</returns>
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
		/// ������ ���ڿ��� Encode�մϴ�.
		/// </summary>
		/// <param name="vValue">Encode�� ���ڿ� ������ �Դϴ�.</param>
		/// <returns>Encode�� �������� Byte�迭�Դϴ�.</returns>
		public static byte [] EncodeOctetString(string vValue)
		{
			//Decode Rule ------------------------------------------------------------------
			// ���� Byte�� ���ڿ� ���� Byte ���̸� ��Ÿ���ϴ�.
			// ���� Byte�� ���� Byte���� ���̸�ŭ�� ������ �Դϴ�.

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
		/// ������ ���ڿ��� Encode�մϴ�.
		/// </summary>
		/// <param name="vValue">Encode�� ���ڿ� ������ �Դϴ�.</param>
		/// <returns>Encode�� �������� Byte�迭�Դϴ�.</returns>
		public static byte [] EncodeOctetString(byte [] vValue)
		{
			byte [] tLength = EncodeLength(vValue.Length);
			byte [] tBytes = new byte [vValue.Length + tLength.Length];
			Array.Copy(tLength, 0, tBytes, 0, tLength.Length);
			Array.Copy(vValue, 0, tBytes, tLength.Length, vValue.Length);

			return tBytes;
		}

		/// <summary>
		/// IPAddress�� Encode�մϴ�.
		/// </summary>
		/// <param name="vIPAddress">Encode�� IP�ּ� �Դϴ�.</param>
		/// <returns>Encode�� IP�ּ��� Byte�迭�Դϴ�.</returns>
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
		/// Null�� Encode�մϴ�.
		/// </summary>
		/// <returns>Encode�� Null�� Byte�迭�� ��ȯ �մϴ�.</returns>
		public static byte [] EncodeNull()
		{
			byte [] tDatas = new byte[1];
			return tDatas;
		}
		#endregion //SNMP DataType Encode �κ� �Դϴ� -------------------------------------------
		#endregion //SNMP Encode �κ� �Դϴ� ----------------------------------------------------

		#region Snmp DateType��ȯ �κ� �Դϴ� ----------------------------------------------
		/// <summary>
		/// ������ MAC�ּҸ� Byte�迭�� �����մϴ�.
		/// </summary>
		/// <param name="vMacAddress">Byte�迭�� ��ȯ�� MAC�ּ� �Դϴ�.</param>
		/// <returns>��ȯ�� MAC�ּ��� Byte�迭�Դϴ�. ��ȯ������ ��� null�Դϴ�.</returns>
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
		#endregion //Snmp DateType��ȯ �κ� �Դϴ� ----------------------------------------------
	}	

	#region SNMP Data Type Ŭ���� �Դϴ� ---------------------------------------------------
	/// <summary>
	/// �ð� �� ���� �߰� �����Դϴ�.
	/// </summary>
	public enum E_UTCDirection
	{
		/// <summary>
		/// UTC���� �Դϴ�.
		/// </summary>
		None = 0,
		/// <summary>
		/// UTC�� ���ϱ� �մϴ�.
		/// </summary>
		Plus = 43,
		/// <summary>
		/// UTC�� ���� �մϴ�.
		/// </summary>
		Minus = 45
	}
	/// <summary>
	/// SNMP DateAndType Ŭ���� �Դϴ�.
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
		/// �⵵ �Դϴ�.
		/// </summary>
		private int m_Year = 1999;
		/// <summary>
		/// �� �Դϴ�.
		/// </summary>
		private int m_Month = 1;
		/// <summary>
		/// �� �Դϴ�.
		/// </summary>
		private int m_Day = 1;
		/// <summary>
		/// �ð� �Դϴ�.
		/// </summary>
		private int m_Hour = 0;
		/// <summary>
		/// �� �Դϴ�.
		/// </summary>
		private int m_Minute = 0;
		/// <summary>
		/// �� �Դϴ�.
		/// </summary>
		private int m_Second = 0;
		/// <summary>
		/// 1/10 �� �Դϴ�.
		/// </summary>
		private int m_DeciSecond = 0;
		/// <summary>
		/// UTC�� ���ϱ� �����Դϴ�.
		/// </summary>
		private E_UTCDirection m_UTCDirection = E_UTCDirection.None;
		/// <summary>
		/// ���ϰų� �� �ð� �Դϴ�.
		/// </summary>
		private int m_UTCHour = 0;
		/// <summary>
		/// ���ϰų� �� �� �Դϴ�.
		/// </summary>
		private int m_UTCMinute = 0;

		/// <summary>
		/// TMSDateAndTimeŬ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vYear">�⵵ �Դϴ�.</param>
		/// <param name="vMonth">�� �Դϴ�.</param>
		/// <param name="vDay">�� �Դϴ�.</param>
		/// <param name="vHour">�ð� �Դϴ�.</param>
		/// <param name="vMinute">�� �Դϴ�.</param>
		/// <param name="vSecond">�� �Դϴ�.</param>
		/// <param name="vDeciSecond">1/10 �� �Դϴ�.</param>
		/// <param name="vUTCDirection">UTC�� ���ϱ� �����Դϴ�.</param>
		/// <param name="vUTCHour">���ϰų� �� �ð� �Դϴ�.</param>
		/// <param name="vUTCMinute">���ϰų� �� �� �Դϴ�.</param>
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
		/// TMSDateAndTimeŬ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vDateTime">��¥�� �ð��� ������ DateTime��ü �Դϴ�.</param>
		/// <param name="vUTCDirection">UTC�� ���ϱ� �����Դϴ�.</param>
		/// <param name="vUTCHour">���ϰų� �� �ð� �Դϴ�.</param>
		/// <param name="vUTCMinute">���ϰų� �� �� �Դϴ�.</param>
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
		/// TMSDateAndTimeŬ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vDateTime">��¥�� �ð��� ���Ե� ���ڿ� �Դϴ�(��: "2005-10-10,13:30:30.3,-2:20").</param>
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
		/// TMSDateAndTimeŬ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vDateTime">��¥�� �ð��� ����� Byte�迭 �Դϴ�.</param>
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
		/// TMSDateAndTime�� ��¥�� �ð��� Byte�迭�� ��ȯ�մϴ�.
		/// </summary>
		/// <returns>Byte�迭�� ��ȯ�� ��¥�� �ð��Դϴ�.</returns>
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
		/// TMSDateAndTime�� ��¥�� �ð��� ���ڿ��� ��ȯ�մϴ�.
		/// </summary>
		/// <returns>���ڿ��� ��ȯ�� ��¥�� �ð��Դϴ�.</returns>
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
		/// ������ ��¥�� �ð��� Byte�迭�� ��ȯ �մϴ�.
		/// </summary>
		/// <param name="vYear">�⵵ �Դϴ�.</param>
		/// <param name="vMonth">�� �Դϴ�.</param>
		/// <param name="vDay">�� �Դϴ�.</param>
		/// <param name="vHour">�ð� �Դϴ�.</param>
		/// <param name="vMinute">�� �Դϴ�.</param>
		/// <param name="vSecond">�� �Դϴ�.</param>
		/// <param name="vDeciSecond">1/10 �� �Դϴ�.</param>
		/// <param name="vUTCDirection">UTC�� ���ϱ� �����Դϴ�.</param>
		/// <param name="vUTCHour">���ϰų� �� �ð� �Դϴ�.</param>
		/// <param name="vUTCMinute">���ϰų� �� �� �Դϴ�.</param>
		/// <returns>Byte�迭�� ��ȯ�� ��¥�� �ð��Դϴ�.</returns>
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
		/// ������ ��¥�� �ð� ���ڿ��� Byte�迭�� ��ȯ �մϴ�.
		/// </summary>
		/// <param name="vDateTime">��¥�� �ð��� ���Ե� ���ڿ� �Դϴ�.</param>
		/// <returns>Byte�迭�� ��ȯ�� ��¥�� �ð��Դϴ�.</returns>
		public static byte [] ToBytes(string vDateTime)
		{
			TMSDateAndTime tDateTime = new TMSDateAndTime(vDateTime);
			return tDateTime.GetBytes();
		}

		/// <summary>
		/// ������ ��¥�� �ð� Byte�迭�� TMSDateAndTime�������� ��ȯ �մϴ�.
		/// </summary>
		/// <param name="vDateTime">��¥�� �ð��� ����� Byte�迭 �Դϴ�.</param>
		/// <returns>��¥�� �ð��� ��ȯ�� TMSDateAndTime��ü �Դϴ�.</returns>
		public static TMSDateAndTime ToDateAndTime(byte [] vDateTime)
		{
			return new TMSDateAndTime(vDateTime);
		}
	}
	#endregion SNMP Data Type Ŭ���� �Դϴ� ---------------------------------------------------

	#region SNMP PDU Ŭ���� �Դϴ� ---------------------------------------------------------
	#region SNMP PDU Interface�Դϴ� -------------------------------------------------------
	/// <summary>
	/// SNMP PDU�� �������̽� �Դϴ�.
	/// </summary>
	public interface ITMSSnmpPDU
	{
		/// <summary>
		/// SNMP Version�� �������ų� �����մϴ�.
		/// </summary>
		E_SnmpVersion Version {get; set;}
		/// <summary>
		/// SNMP PDU Type�� �������ų� �����մϴ�.
		/// </summary>
		E_SnmpPDUType PDUType {get; set;}			
	}
	#endregion //SNMP PDU Interface�Դϴ� -------------------------------------------------------

	#region SNMP V1, V2 TrapPDU Ŭ���� �Դϴ� ----------------------------------------------
	/// <summary>
	/// Snmp Version1�� Ʈ�� PDUŬ���� �Դϴ�.
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
		/// Snmp Version1 Ʈ�� PDUŬ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vVersion">Snmp Version�Դϴ�.</param>
		public TMSSnmp_TrapPDU(E_SnmpVersion vVersion)
		{
			m_Version = vVersion;
		}

		/// <summary>
		/// Snmp Version�� �������ų� �����մϴ�.
		/// </summary>
		public E_SnmpVersion Version
		{
			get { return m_Version; }
			set { m_Version = value; }
		}

		/// <summary>
		/// Snmp ���� ��ȣ�� �������ų� �����մϴ�.
		/// </summary>
		public string Community
		{
			get { return m_Community; }
			set { m_Community = value; }
		}

		/// <summary>
		/// Snmp PDU Ÿ���� �������ų� �����մϴ�.
		/// </summary>
		public E_SnmpPDUType PDUType
		{
			get { return m_PDUType; }
			set { m_PDUType = value; }
		}

		/// <summary>
		/// ����� Enterprise ObjectIdentifier�� �������ų� �����մϴ�.
		/// </summary>
		public string Enterprise
		{
			get { return m_Enterprise; }
			set { m_Enterprise = value; }
		}
		
		/// <summary>
		/// Snmp Agent�� IP�ּҸ� �������ų� �����մϴ�.
		/// </summary>
		public string AgentIPAddress
		{
			get { return m_AgentIPAddress; }
			set { m_AgentIPAddress = value; }
		}

		/// <summary>
		/// Snmp Trap Type�� �������ų� �����մϴ�.
		/// </summary>
		public E_GenericTrapType TrapType
		{
			get { return m_GenericTrapType; }
			set { m_GenericTrapType = value; }
		}

		/// <summary>
		/// SpecificCode�� �������ų� �����մϴ�.
		/// </summary>
		public long SpecificCode
		{
			get { return m_SpecificCode; }
			set { m_SpecificCode = value; }
		}

		/// <summary>
		/// TimeTick���� �������ų� �����մϴ�.
		/// </summary>
		public long TimeTick
		{
			get { return m_TimeTick; }
			set { m_TimeTick = value; }
		}

		/// <summary>
		/// Snmp Trap�� Binding ������ ����� �迭�� �������ų� �����մϴ�.
		/// </summary>
		public TMSSnmpVariableCollection Variables
		{
			get { return m_Variables; }
			set { m_Variables = value; }
		}

		/// <summary>
		/// �м������� �������ų� �����մϴ�.
		/// </summary>
		public  E_ParseError ParseError
		{
			get { return m_ParseError; }
			set { m_ParseError = value; }
		}		
	}

	/// <summary>
	/// Snmp Version2�� Trap PDU Ŭ���� �Դϴ�.
	/// </summary>
	public class TMSSnmpV2_TrapPDU : TMSSnmp_GetResponsePDU
	{
		/// <summary>
		/// Snmp Trap Type�Դϴ�.
		/// </summary>
		private E_GenericTrapType m_GenericTrapType = E_GenericTrapType.ColdStart;

		/// <summary>
		/// Snmp Version2�� Trap PDU Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vVersion">Snmp Version�Դϴ�.</param>
		public TMSSnmpV2_TrapPDU(E_SnmpVersion vVersion) : base(vVersion)
		{
			this.PDUType = E_SnmpPDUType.TrapV2;
		}

		/// <summary>
		/// Ʈ�� Ÿ���� �������ų� �����մϴ�.
		/// </summary>
		public E_GenericTrapType TrapType
		{
			get { return m_GenericTrapType; }
			set { m_GenericTrapType = value; }
		}
	}
	#endregion //SNMP V1, V2 TrapPDU Ŭ���� �Դϴ� ----------------------------------------------

	#region SNMP V1, V2 GetResponsePDU Ŭ���� �Դϴ� ---------------------------------------
	/// <summary>
	/// Snmp Version1, 2�� GetResponse PDU Ŭ���� �Դϴ�.
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
		/// Snmp Version1, 2�� GetResponse PDU Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vVersion">SnmpVersion �Դϴ�.</param>
		public TMSSnmp_GetResponsePDU(E_SnmpVersion vVersion)
		{
			m_Version = vVersion;
		}

		/// <summary>
		/// Snmp Version�� �������ų� �����մϴ�.
		/// </summary>
		public E_SnmpVersion Version
		{
			get { return m_Version; }
			set { m_Version = value; }
		}

		/// <summary>
		/// Snmp ���� ��ȣ�� �������ų� �����մϴ�.
		/// </summary>
		public string Community
		{
			get { return m_Community; }
			set { m_Community = value; }
		}

		/// <summary>
		/// Snmp PDU Type�� �������ų� �����մϴ�.
		/// </summary>
		public E_SnmpPDUType PDUType
		{
			get { return m_PDUType; }
			set { m_PDUType = value; }
		}

		/// <summary>
		/// Snmp ��û ID�� �������ų� �����մϴ�.
		/// </summary>
		public long RequestID
		{
			get { return m_RequestID; }
			set { m_RequestID = value; }
		}

		/// <summary>
		/// Snmp Error ���¸� �������ų� �����մϴ�.
		/// </summary>
		public E_SnmpErrorStatus ErrorStatus
		{
			get { return m_ErrorStatus; }
			set { m_ErrorStatus = value; }
		}

		/// <summary>
		/// Snmp Error Index�� �������ų� �����մϴ�.
		/// </summary>
		public int ErrorIndex
		{
			get { return m_ErrorIndex; }
			set { m_ErrorIndex = value; }
		}

		/// <summary>
		/// Snmp GetResponse�� Binding������ ����� �迭�� �������ų� �����մϴ�.
		/// </summary>
		public TMSSnmpVariableCollection Variables
		{
			get { return m_Variables; }
			set { m_Variables = value; }
		}

		/// <summary>
		/// �м� ������ �������ų� �����մϴ�.
		/// </summary>
		public  E_ParseError ParseError
		{
			get { return m_ParseError; }
			set { m_ParseError = value; }
		}
	}
	#endregion //SNMP V1, V2 GetResponsePDU Ŭ���� �Դϴ� ---------------------------------------

	#region SNMP Variable Ŭ���� �Դϴ� ----------------------------------------------------
	/// <summary>
	/// Snmp Binding���� ��� Ŭ���� �Դϴ�.
	/// </summary>
	[Serializable]
	public class TMSSnmpVariableCollection : MarshalByRefObject
	{
		private ArrayList m_Variables = new ArrayList();		

		/// <summary>
		/// Snmp Binding���� ��� Ŭ������ �⺻ ������ �Դϴ�.
		/// </summary>
		public TMSSnmpVariableCollection() {}

		/// <summary>
		/// Snmp Binding���� ��� Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vVariableCollection">������ Snmp Binding���� ��� ��ü �Դϴ�.</param>
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
		/// Snmp Binding������ ������ �����ɴϴ�.
		/// </summary>
		public int Count
		{
			get { return m_Variables.Count; }
		}
		
		/// <summary>
		/// Snmp Binding������ �߰��մϴ�.
		/// </summary>
		/// <param name="vVariable">�߰��� Binding���� ��ü �Դϴ�.</param>
		/// <returns>Binding������ ����� ��ġ �Դϴ�.</returns>
		public int Add(TMSSnmpVariable vVariable)
		{				
			return m_Variables.Add(vVariable);
		}

		/// <summary>
		/// Snmp Binding������ �߰��մϴ�.
		/// </summary>
		/// <param name="vObjectID">�߰��� OID �Դϴ�.</param>
		/// <returns>Binding������ ����� ��ġ �Դϴ�.</returns>
		public int Add(string vObjectID)
		{
			return m_Variables.Add(new TMSSnmpVariable(vObjectID));
		}

		/// <summary>
		/// Snmp Binding������ �߰��մϴ�.
		/// </summary>
		/// <param name="vObjectID">�߰��� OID �Դϴ�.</param>
		/// <param name="vDataType">Snmp ������ Ÿ�� �Դϴ�.</param>
		/// <param name="vValue">�߰��� Binding �� �Դϴ�.</param>
		/// <returns>Binding������ ����� ��ġ �Դϴ�.</returns>
		public int Add(string vObjectID, E_SnmpDataType vDataType, object vValue)
		{
			return m_Variables.Add(new TMSSnmpVariable(vObjectID, vDataType, vValue));
		}

		/// <summary>
		/// Snmp Binding������ �߰��մϴ�.
		/// </summary>
		/// <param name="vObjectID">�߰��� OID �Դϴ�.</param>
		/// <param name="vDataType">Snmp ������ Ÿ�� �Դϴ�.</param>
		/// <param name="vValue">�߰��� Binding �� �Դϴ�.</param>
		/// <returns>Binding������ ����� ��ġ �Դϴ�.</returns>
		public int Add(string vObjectID, E_SnmpDataType vDataType, string vValue)
		{
			return m_Variables.Add(new TMSSnmpVariable(vObjectID, vDataType, vValue));
		}

		/// <summary>
		/// Snmp Binding������ �߰��մϴ�.
		/// </summary>
		/// <param name="vVariables">�߰��� Snmp Binding���� Ŭ���� �Դϴ�.</param>
		public void AddRange(TMSSnmpVariableCollection vVariables)
		{
			m_Variables.AddRange(vVariables.m_Variables);
		}
	
		/// <summary>
		/// ������ �迭�� �߰��մϴ�.
		/// </summary>
		/// <param name="vVariables">�߰��� �迭�Դϴ�.</param>
		internal void AddRange(ArrayList vVariables)
		{
			m_Variables.AddRange(vVariables);
		}

		/// <summary>
		/// ������ ��ġ���� ������ ������ �迭�� �����ɴϴ�.
		/// </summary>
		/// <param name="vIndex">�迭�� ������ ���� ��ġ �Դϴ�.</param>
		/// <param name="vCount">������ �迭�� ���� �Դϴ�.</param>
		/// <returns>�迭 �Դϴ�.</returns>
		internal ArrayList GetRange(int vIndex, int vCount)
		{
			return m_Variables.GetRange(vIndex, vCount);
		}

		/// <summary>
		/// Snmp Binding������ �������ų� �����մϴ�.
		/// </summary>
		public TMSSnmpVariable this[int index]
		{
			get { return (TMSSnmpVariable)m_Variables[index]; }
			set { m_Variables[index] = value; }
		}

		/// <summary>
		/// Snmp Binding���� ����� ���ڿ��� ��ȯ �մϴ�.
		/// </summary>
		/// <returns>Snmp Binding�������ڿ� �Դϴ�.</returns>
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
		/// ���ڿ��� Snmp Binding���� ������� �����մϴ�.
		/// </summary>
		/// <param name="vVariableString">Snmp Binding���� ��� ���ڿ� �Դϴ�.</param>
		/// <param name="vSeperator">�� ������ ������ �Դϴ�.</param>
		/// <returns>Snmp Binding���� ��� Ŭ���� �Դϴ�.</returns>
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
	/// SNMP Binding �����͸� ������ Ŭ���� �Դϴ�.
	/// </summary>
	[Serializable]
	public class TMSSnmpVariable : MarshalByRefObject
	{
		/// <summary>
		/// Object Identifier�Դϴ�.
		/// </summary>
		private string m_ObjectID = "";
		/// <summary>
		/// Snmp DataType�Դϴ�.
		/// </summary>
		private E_SnmpDataType m_Type = E_SnmpDataType.Null;
		/// <summary>
		/// �� �Դϴ�.
		/// </summary>
		private object m_Value = null;
		/// <summary>
		/// ���� ������ �Դϴ�.
		/// </summary>
		private byte [] m_RawData = null;

		/// <summary>
		/// SNMP Binding �����͸� ������ Ŭ������ �⺻ ������ �Դϴ�.
		/// </summary>
		public TMSSnmpVariable() {}		

		/// <summary>
		/// SNMP Binding �����͸� ������ Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vVariable">������ SNMP Binding ������ ��ü �Դϴ�.</param>
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
		/// SNMP Binding �����͸� ������ Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vObjectID">Snmp Binding������ ObjectIdentifier�Դϴ�.</param>
		public TMSSnmpVariable(string vObjectID)
		{
			m_ObjectID = vObjectID;
		}

		/// <summary>
		/// SNMP Binding ������ ���� Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vObjectID">���� Object Identifier�Դϴ�.</param>
		/// <param name="vType">���� ������ Ÿ�� �Դϴ�.</param>
		/// <param name="vValue">SNMP������ �� �Դϴ�.</param>
		public TMSSnmpVariable(string vObjectID, E_SnmpDataType vType, object vValue)
		{
			m_ObjectID = vObjectID;
			m_Type = vType;
			m_Value = vValue;
		}

		/// <summary>
		/// SNMP Binding ������ ���� Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vObjectID">���� Object Identifier�Դϴ�.</param>
		/// <param name="vType">���� ������ Ÿ�� �Դϴ�.</param>
		/// <param name="vValue">SNMP������ �� �Դϴ�.</param>
		public TMSSnmpVariable(string vObjectID, E_SnmpDataType vType, string vValue)
		{
			m_ObjectID = vObjectID;
			m_Type = vType;
			m_Value = vValue;
		}

		/// <summary>
		/// SNMP Binding ������ ���� Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vObjectID">���� Object Identifier�Դϴ�.</param>
		/// <param name="vValue">��¥�� �ð��� ����� TMSDateAndTime��ü �Դϴ�.</param>
		public TMSSnmpVariable(string vObjectID, TMSDateAndTime vValue)
		{
			m_ObjectID = vObjectID;
			m_Type = E_SnmpDataType.DateAndTime;
			m_Value = vValue.GetBytes();
		}

		/// <summary>
		/// Object Identifier�� �����ɴϴ�.
		/// </summary>
		public string ObjectID
		{
			get { return m_ObjectID; }
			set { m_ObjectID = value; }
		}

		/// <summary>
		/// SNMP ������ Ÿ���� �����ɴϴ�.
		/// </summary>
		public E_SnmpDataType Type
		{			
			get { return m_Type;}
			set { m_Type = value; }
		}

		/// <summary>
		/// SNMP�����͸� �������ų� �����մϴ�. �����ʹ� �������� Ÿ�Կ� �°� �Է��� �Ͽ� �ֽʽÿ�. OctetStringŸ���� ��� ���� Byte�迭�� �Է��ϽǼ� �ֽ��ϴ�.
		/// </summary>
		public object Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}

		/// <summary>
		/// ���� �����͸� �������ų� �����մϴ�.
		/// </summary>
		public byte [] RawData
		{
			get { return m_RawData; }
			set { m_RawData = value; }
		}
	}
	#endregion //SNMP Variable Ŭ���� �Դϴ� ----------------------------------------------------
	#endregion //SNMP PDU Ŭ���� �Դϴ� ---------------------------------------------------------
}