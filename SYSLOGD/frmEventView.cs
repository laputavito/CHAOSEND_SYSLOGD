using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using NT50.CommonClass;
using C1.Win.C1FlexGrid;

namespace tmcMain
{
	/// <summary>
	/// frmLogView에 대한 요약 설명입니다.
	/// </summary>
	public class frmEventView : System.Windows.Forms.Form
	{
		private MKLibrary.Controls.MKButton butCancel;
		private MKLibrary.Controls.MKButton btnSOP;
		private MKLibrary.Controls.MKPanel pnlInfo;
		private MKLibrary.Controls.MKLabel lblMessage;
		private System.Windows.Forms.Panel pnlSyslog;
		private System.Windows.Forms.Label lblSFaultStatus;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblSEventLevel;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label lblSEventName;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label lblSDeviceName;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label lblSDeviceIP;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label lblSDevicePath;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label lblSEventTime;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label lblSSeverity;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label lblSLevel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblSFacility;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Panel pnlTrap;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label lblTEventLevel;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label lblTEventName;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label lblTDeviceName;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label lblTOID;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label lblTDeviceIP;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lblTDevicePath;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblTEventTime;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblTTrapType;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label lblTSpecCode;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Panel pnlThreshold;
		private System.Windows.Forms.Label lblHFaultStatus;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblHEventLevel;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label lblHEventName;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label lblHDeviceName;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label lblHDeviceIP;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label lblHDevicePath;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label lblHEventTime;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label lblTFaultStatus;
		private MKLibrary.Controls.MKButton btnComplete;
		private MKLibrary.Controls.MKButton btnSee;
		private MKLibrary.Controls.MKPanel pnlCheck;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label lblCheckTime;
		private System.Windows.Forms.Label lblCheckUser;
		private MKLibrary.Controls.MKPanel pnlManage;
		private System.Windows.Forms.Label lblManageTime;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label lblManageUser;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Panel pnlManageView;
		private System.Windows.Forms.Panel pnlManageInput;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label37;
		private MKLibrary.Controls.MKButton btnTelnetStart;
		private System.Windows.Forms.RichTextBox txtManageContent;
		private System.Windows.Forms.RichTextBox txtManageContentInput;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label lblHThresholdValue;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label lblValue;
		private System.ComponentModel.IContainer components;

		public frmEventView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.butCancel = new MKLibrary.Controls.MKButton(this.components);
			this.btnSOP = new MKLibrary.Controls.MKButton(this.components);
			this.pnlInfo = new MKLibrary.Controls.MKPanel(this.components);
			this.lblMessage = new MKLibrary.Controls.MKLabel();
			this.pnlThreshold = new System.Windows.Forms.Panel();
			this.lblHThresholdValue = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.lblHFaultStatus = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblHEventLevel = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.lblHEventName = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.lblHDeviceName = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.lblHDeviceIP = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.lblHDevicePath = new System.Windows.Forms.Label();
			this.label32 = new System.Windows.Forms.Label();
			this.lblHEventTime = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.pnlSyslog = new System.Windows.Forms.Panel();
			this.lblSFaultStatus = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lblSEventLevel = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.lblSEventName = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.lblSDeviceName = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.lblSDeviceIP = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.lblSDevicePath = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.lblSEventTime = new System.Windows.Forms.Label();
			this.label36 = new System.Windows.Forms.Label();
			this.lblSSeverity = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.lblSLevel = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblSFacility = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.pnlTrap = new System.Windows.Forms.Panel();
			this.lblTFaultStatus = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.lblTEventLevel = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.lblTEventName = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.lblTDeviceName = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.lblTOID = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.lblTDeviceIP = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lblTDevicePath = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblTEventTime = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblTTrapType = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.lblTSpecCode = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.btnComplete = new MKLibrary.Controls.MKButton(this.components);
			this.btnSee = new MKLibrary.Controls.MKButton(this.components);
			this.pnlCheck = new MKLibrary.Controls.MKPanel(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblCheckTime = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.lblCheckUser = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.pnlManage = new MKLibrary.Controls.MKPanel(this.components);
			this.pnlManageView = new System.Windows.Forms.Panel();
			this.label31 = new System.Windows.Forms.Label();
			this.lblManageTime = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.lblManageUser = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.txtManageContent = new System.Windows.Forms.RichTextBox();
			this.pnlManageInput = new System.Windows.Forms.Panel();
			this.txtManageContentInput = new System.Windows.Forms.RichTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label37 = new System.Windows.Forms.Label();
			this.btnTelnetStart = new MKLibrary.Controls.MKButton(this.components);
			this.label25 = new System.Windows.Forms.Label();
			this.lblValue = new System.Windows.Forms.Label();
			this.pnlInfo.SuspendLayout();
			this.pnlThreshold.SuspendLayout();
			this.pnlSyslog.SuspendLayout();
			this.pnlTrap.SuspendLayout();
			this.pnlCheck.SuspendLayout();
			this.panel1.SuspendLayout();
			this.pnlManage.SuspendLayout();
			this.pnlManageView.SuspendLayout();
			this.pnlManageInput.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.BackColor = System.Drawing.Color.Transparent;
			this.butCancel.BorderColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(207)), ((System.Byte)(208)));
			this.butCancel.BorderEdgeRadius = 5;
			this.butCancel.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Round;
			this.butCancel.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
			this.butCancel.ColorDepthFocus = 2;
			this.butCancel.ColorDepthHover = 2;
			this.butCancel.ColorDepthShadow = 2;
			this.butCancel.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
			this.butCancel.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
			this.butCancel.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.LeftTopToRightBottom;
			this.butCancel.Image = null;
			this.butCancel.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
			this.butCancel.ImageIndent = 0;
			this.butCancel.ImageIndexDisable = -1;
			this.butCancel.ImageIndexHover = -1;
			this.butCancel.ImageIndexNormal = -1;
			this.butCancel.ImageIndexSelect = -1;
			this.butCancel.ImageList = null;
			this.butCancel.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.butCancel.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
			this.butCancel.Location = new System.Drawing.Point(486, 426);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(93, 30);
			this.butCancel.TabIndex = 16;
			this.butCancel.Text = "닫기";
			this.butCancel.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
			this.butCancel.TextIndent = 0;
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// btnSOP
			// 
			this.btnSOP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSOP.BackColor = System.Drawing.Color.Transparent;
			this.btnSOP.BorderColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(207)), ((System.Byte)(208)));
			this.btnSOP.BorderEdgeRadius = 5;
			this.btnSOP.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Round;
			this.btnSOP.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
			this.btnSOP.ColorDepthFocus = 2;
			this.btnSOP.ColorDepthHover = 2;
			this.btnSOP.ColorDepthShadow = 2;
			this.btnSOP.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
			this.btnSOP.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
			this.btnSOP.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.LeftTopToRightBottom;
			this.btnSOP.Image = null;
			this.btnSOP.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
			this.btnSOP.ImageIndent = 0;
			this.btnSOP.ImageIndexDisable = -1;
			this.btnSOP.ImageIndexHover = -1;
			this.btnSOP.ImageIndexNormal = -1;
			this.btnSOP.ImageIndexSelect = -1;
			this.btnSOP.ImageList = null;
			this.btnSOP.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.btnSOP.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
			this.btnSOP.Location = new System.Drawing.Point(387, 426);
			this.btnSOP.Name = "btnSOP";
			this.btnSOP.Size = new System.Drawing.Size(93, 30);
			this.btnSOP.TabIndex = 17;
			this.btnSOP.Text = "조치방법";
			this.btnSOP.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
			this.btnSOP.TextIndent = 0;
			this.btnSOP.Click += new System.EventHandler(this.btnSOP_Click);
			// 
			// pnlInfo
			// 
			this.pnlInfo.BackColorNormal = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(247)));
			this.pnlInfo.BackColorPattern = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(247)));
			this.pnlInfo.BorderColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(207)), ((System.Byte)(208)));
			this.pnlInfo.BorderEdgeRadius = 7;
			this.pnlInfo.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Round;
			this.pnlInfo.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
			this.pnlInfo.CaptionLabel = false;
			this.pnlInfo.Controls.Add(this.lblMessage);
			this.pnlInfo.Controls.Add(this.pnlThreshold);
			this.pnlInfo.Controls.Add(this.pnlSyslog);
			this.pnlInfo.Controls.Add(this.pnlTrap);
			this.pnlInfo.DockPadding.Bottom = 4;
			this.pnlInfo.DockPadding.Left = 4;
			this.pnlInfo.DockPadding.Right = 4;
			this.pnlInfo.DockPadding.Top = 4;
			this.pnlInfo.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.LeftTopToRightBottom;
			this.pnlInfo.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
			this.pnlInfo.Location = new System.Drawing.Point(12, 9);
			this.pnlInfo.Name = "pnlInfo";
			this.pnlInfo.Size = new System.Drawing.Size(561, 174);
			this.pnlInfo.TabIndex = 18;
			// 
			// lblMessage
			// 
			this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
			this.lblMessage.BackColorPattern = System.Drawing.Color.White;
			this.lblMessage.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
			this.lblMessage.BorderColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(207)), ((System.Byte)(208)));
			this.lblMessage.BorderEdgeRadius = 5;
			this.lblMessage.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Round;
			this.lblMessage.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
			this.lblMessage.CaptionLabel = false;
			this.lblMessage.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblMessage.ForeColor = System.Drawing.Color.White;
			this.lblMessage.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.HorzontalRound;
			this.lblMessage.Image = null;
			this.lblMessage.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
			this.lblMessage.ImageIndent = 0;
			this.lblMessage.ImageIndex = -1;
			this.lblMessage.ImageList = null;
			this.lblMessage.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.lblMessage.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
			this.lblMessage.Location = new System.Drawing.Point(24, 15);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(516, 27);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "mkLabel1";
			this.lblMessage.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
			this.lblMessage.TextIndent = 0;
			// 
			// pnlThreshold
			// 
			this.pnlThreshold.BackColor = System.Drawing.Color.Transparent;
			this.pnlThreshold.Controls.Add(this.lblHThresholdValue);
			this.pnlThreshold.Controls.Add(this.label18);
			this.pnlThreshold.Controls.Add(this.lblHFaultStatus);
			this.pnlThreshold.Controls.Add(this.label5);
			this.pnlThreshold.Controls.Add(this.lblHEventLevel);
			this.pnlThreshold.Controls.Add(this.label19);
			this.pnlThreshold.Controls.Add(this.lblHEventName);
			this.pnlThreshold.Controls.Add(this.label23);
			this.pnlThreshold.Controls.Add(this.lblHDeviceName);
			this.pnlThreshold.Controls.Add(this.label27);
			this.pnlThreshold.Controls.Add(this.lblHDeviceIP);
			this.pnlThreshold.Controls.Add(this.label30);
			this.pnlThreshold.Controls.Add(this.lblHDevicePath);
			this.pnlThreshold.Controls.Add(this.label32);
			this.pnlThreshold.Controls.Add(this.lblHEventTime);
			this.pnlThreshold.Controls.Add(this.label34);
			this.pnlThreshold.Location = new System.Drawing.Point(24, 54);
			this.pnlThreshold.Name = "pnlThreshold";
			this.pnlThreshold.Size = new System.Drawing.Size(516, 108);
			this.pnlThreshold.TabIndex = 3;
			this.pnlThreshold.Visible = false;
			// 
			// lblHThresholdValue
			// 
			this.lblHThresholdValue.Location = new System.Drawing.Point(75, 75);
			this.lblHThresholdValue.Name = "lblHThresholdValue";
			this.lblHThresholdValue.Size = new System.Drawing.Size(429, 27);
			this.lblHThresholdValue.TabIndex = 35;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(3, 75);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(69, 12);
			this.label18.TabIndex = 34;
			this.label18.Text = "임계치 값 :";
			// 
			// lblHFaultStatus
			// 
			this.lblHFaultStatus.Location = new System.Drawing.Point(75, 21);
			this.lblHFaultStatus.Name = "lblHFaultStatus";
			this.lblHFaultStatus.Size = new System.Drawing.Size(177, 12);
			this.lblHFaultStatus.TabIndex = 33;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(3, 21);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(69, 12);
			this.label5.TabIndex = 32;
			this.label5.Text = "장애 상태 :";
			// 
			// lblHEventLevel
			// 
			this.lblHEventLevel.Location = new System.Drawing.Point(327, 3);
			this.lblHEventLevel.Name = "lblHEventLevel";
			this.lblHEventLevel.Size = new System.Drawing.Size(177, 12);
			this.lblHEventLevel.TabIndex = 31;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(255, 3);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(69, 12);
			this.label19.TabIndex = 30;
			this.label19.Text = "등       급 :";
			// 
			// lblHEventName
			// 
			this.lblHEventName.Location = new System.Drawing.Point(75, 3);
			this.lblHEventName.Name = "lblHEventName";
			this.lblHEventName.Size = new System.Drawing.Size(177, 12);
			this.lblHEventName.TabIndex = 29;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(3, 3);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(69, 12);
			this.label23.TabIndex = 28;
			this.label23.Text = "이벤트 명 :";
			// 
			// lblHDeviceName
			// 
			this.lblHDeviceName.Location = new System.Drawing.Point(75, 57);
			this.lblHDeviceName.Name = "lblHDeviceName";
			this.lblHDeviceName.Size = new System.Drawing.Size(177, 12);
			this.lblHDeviceName.TabIndex = 27;
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(3, 57);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(69, 12);
			this.label27.TabIndex = 26;
			this.label27.Text = "장  비  명 :";
			// 
			// lblHDeviceIP
			// 
			this.lblHDeviceIP.Location = new System.Drawing.Point(327, 57);
			this.lblHDeviceIP.Name = "lblHDeviceIP";
			this.lblHDeviceIP.Size = new System.Drawing.Size(177, 12);
			this.lblHDeviceIP.TabIndex = 25;
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(255, 57);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(69, 12);
			this.label30.TabIndex = 24;
			this.label30.Text = "장   비 IP :";
			// 
			// lblHDevicePath
			// 
			this.lblHDevicePath.Location = new System.Drawing.Point(75, 39);
			this.lblHDevicePath.Name = "lblHDevicePath";
			this.lblHDevicePath.Size = new System.Drawing.Size(429, 12);
			this.lblHDevicePath.TabIndex = 23;
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(3, 39);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(69, 12);
			this.label32.TabIndex = 22;
			this.label32.Text = "장비 경로 :";
			// 
			// lblHEventTime
			// 
			this.lblHEventTime.Location = new System.Drawing.Point(327, 21);
			this.lblHEventTime.Name = "lblHEventTime";
			this.lblHEventTime.Size = new System.Drawing.Size(177, 12);
			this.lblHEventTime.TabIndex = 21;
			// 
			// label34
			// 
			this.label34.Location = new System.Drawing.Point(255, 21);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(69, 12);
			this.label34.TabIndex = 20;
			this.label34.Text = "발생 시간 :";
			// 
			// pnlSyslog
			// 
			this.pnlSyslog.BackColor = System.Drawing.Color.Transparent;
			this.pnlSyslog.Controls.Add(this.lblSFaultStatus);
			this.pnlSyslog.Controls.Add(this.label7);
			this.pnlSyslog.Controls.Add(this.lblSEventLevel);
			this.pnlSyslog.Controls.Add(this.label17);
			this.pnlSyslog.Controls.Add(this.lblSEventName);
			this.pnlSyslog.Controls.Add(this.label20);
			this.pnlSyslog.Controls.Add(this.lblSDeviceName);
			this.pnlSyslog.Controls.Add(this.label22);
			this.pnlSyslog.Controls.Add(this.lblSDeviceIP);
			this.pnlSyslog.Controls.Add(this.label29);
			this.pnlSyslog.Controls.Add(this.lblSDevicePath);
			this.pnlSyslog.Controls.Add(this.label33);
			this.pnlSyslog.Controls.Add(this.lblSEventTime);
			this.pnlSyslog.Controls.Add(this.label36);
			this.pnlSyslog.Controls.Add(this.lblSSeverity);
			this.pnlSyslog.Controls.Add(this.label14);
			this.pnlSyslog.Controls.Add(this.lblSLevel);
			this.pnlSyslog.Controls.Add(this.label3);
			this.pnlSyslog.Controls.Add(this.lblSFacility);
			this.pnlSyslog.Controls.Add(this.label11);
			this.pnlSyslog.Location = new System.Drawing.Point(24, 54);
			this.pnlSyslog.Name = "pnlSyslog";
			this.pnlSyslog.Size = new System.Drawing.Size(516, 108);
			this.pnlSyslog.TabIndex = 2;
			this.pnlSyslog.Visible = false;
			// 
			// lblSFaultStatus
			// 
			this.lblSFaultStatus.Location = new System.Drawing.Point(75, 21);
			this.lblSFaultStatus.Name = "lblSFaultStatus";
			this.lblSFaultStatus.Size = new System.Drawing.Size(177, 12);
			this.lblSFaultStatus.TabIndex = 33;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(3, 21);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(69, 12);
			this.label7.TabIndex = 32;
			this.label7.Text = "장애 상태 :";
			// 
			// lblSEventLevel
			// 
			this.lblSEventLevel.Location = new System.Drawing.Point(327, 3);
			this.lblSEventLevel.Name = "lblSEventLevel";
			this.lblSEventLevel.Size = new System.Drawing.Size(177, 12);
			this.lblSEventLevel.TabIndex = 31;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(255, 3);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(69, 12);
			this.label17.TabIndex = 30;
			this.label17.Text = "등급        :";
			// 
			// lblSEventName
			// 
			this.lblSEventName.Location = new System.Drawing.Point(75, 3);
			this.lblSEventName.Name = "lblSEventName";
			this.lblSEventName.Size = new System.Drawing.Size(177, 12);
			this.lblSEventName.TabIndex = 29;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(3, 3);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(69, 12);
			this.label20.TabIndex = 28;
			this.label20.Text = "이벤트 명 :";
			// 
			// lblSDeviceName
			// 
			this.lblSDeviceName.Location = new System.Drawing.Point(75, 57);
			this.lblSDeviceName.Name = "lblSDeviceName";
			this.lblSDeviceName.Size = new System.Drawing.Size(177, 12);
			this.lblSDeviceName.TabIndex = 27;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(3, 57);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(69, 12);
			this.label22.TabIndex = 26;
			this.label22.Text = "장비 명    :";
			// 
			// lblSDeviceIP
			// 
			this.lblSDeviceIP.Location = new System.Drawing.Point(327, 57);
			this.lblSDeviceIP.Name = "lblSDeviceIP";
			this.lblSDeviceIP.Size = new System.Drawing.Size(177, 12);
			this.lblSDeviceIP.TabIndex = 25;
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(255, 57);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(69, 12);
			this.label29.TabIndex = 24;
			this.label29.Text = "장비 IP  :";
			// 
			// lblSDevicePath
			// 
			this.lblSDevicePath.Location = new System.Drawing.Point(75, 39);
			this.lblSDevicePath.Name = "lblSDevicePath";
			this.lblSDevicePath.Size = new System.Drawing.Size(429, 12);
			this.lblSDevicePath.TabIndex = 23;
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(3, 39);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(69, 12);
			this.label33.TabIndex = 22;
			this.label33.Text = "장비 경로 :";
			// 
			// lblSEventTime
			// 
			this.lblSEventTime.Location = new System.Drawing.Point(327, 21);
			this.lblSEventTime.Name = "lblSEventTime";
			this.lblSEventTime.Size = new System.Drawing.Size(177, 12);
			this.lblSEventTime.TabIndex = 21;
			// 
			// label36
			// 
			this.label36.Location = new System.Drawing.Point(255, 21);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(69, 12);
			this.label36.TabIndex = 20;
			this.label36.Text = "발생 시간 :";
			// 
			// lblSSeverity
			// 
			this.lblSSeverity.Location = new System.Drawing.Point(318, 75);
			this.lblSSeverity.Name = "lblSSeverity";
			this.lblSSeverity.Size = new System.Drawing.Size(186, 12);
			this.lblSSeverity.TabIndex = 15;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(255, 75);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(60, 12);
			this.label14.TabIndex = 14;
			this.label14.Text = "Severity :";
			// 
			// lblSLevel
			// 
			this.lblSLevel.Location = new System.Drawing.Point(96, 93);
			this.lblSLevel.Name = "lblSLevel";
			this.lblSLevel.Size = new System.Drawing.Size(408, 12);
			this.lblSLevel.TabIndex = 13;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 93);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 12);
			this.label3.TabIndex = 12;
			this.label3.Text = "Syslog Level :";
			// 
			// lblSFacility
			// 
			this.lblSFacility.Location = new System.Drawing.Point(75, 75);
			this.lblSFacility.Name = "lblSFacility";
			this.lblSFacility.Size = new System.Drawing.Size(177, 12);
			this.lblSFacility.TabIndex = 9;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(3, 75);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(72, 12);
			this.label11.TabIndex = 8;
			this.label11.Text = "Facility    :";
			// 
			// pnlTrap
			// 
			this.pnlTrap.BackColor = System.Drawing.Color.Transparent;
			this.pnlTrap.Controls.Add(this.lblTFaultStatus);
			this.pnlTrap.Controls.Add(this.label28);
			this.pnlTrap.Controls.Add(this.lblTEventLevel);
			this.pnlTrap.Controls.Add(this.label24);
			this.pnlTrap.Controls.Add(this.lblTEventName);
			this.pnlTrap.Controls.Add(this.label16);
			this.pnlTrap.Controls.Add(this.lblTDeviceName);
			this.pnlTrap.Controls.Add(this.label12);
			this.pnlTrap.Controls.Add(this.lblTOID);
			this.pnlTrap.Controls.Add(this.label10);
			this.pnlTrap.Controls.Add(this.lblTDeviceIP);
			this.pnlTrap.Controls.Add(this.label6);
			this.pnlTrap.Controls.Add(this.lblTDevicePath);
			this.pnlTrap.Controls.Add(this.label4);
			this.pnlTrap.Controls.Add(this.lblTEventTime);
			this.pnlTrap.Controls.Add(this.label1);
			this.pnlTrap.Controls.Add(this.lblTTrapType);
			this.pnlTrap.Controls.Add(this.label8);
			this.pnlTrap.Controls.Add(this.lblTSpecCode);
			this.pnlTrap.Controls.Add(this.label15);
			this.pnlTrap.Controls.Add(this.label25);
			this.pnlTrap.Controls.Add(this.lblValue);
			this.pnlTrap.Location = new System.Drawing.Point(24, 54);
			this.pnlTrap.Name = "pnlTrap";
			this.pnlTrap.Size = new System.Drawing.Size(516, 108);
			this.pnlTrap.TabIndex = 1;
			this.pnlTrap.Visible = false;
			// 
			// lblTFaultStatus
			// 
			this.lblTFaultStatus.Location = new System.Drawing.Point(75, 21);
			this.lblTFaultStatus.Name = "lblTFaultStatus";
			this.lblTFaultStatus.Size = new System.Drawing.Size(177, 12);
			this.lblTFaultStatus.TabIndex = 19;
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(3, 21);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(69, 12);
			this.label28.TabIndex = 18;
			this.label28.Text = "장애 상태 :";
			// 
			// lblTEventLevel
			// 
			this.lblTEventLevel.Location = new System.Drawing.Point(327, 3);
			this.lblTEventLevel.Name = "lblTEventLevel";
			this.lblTEventLevel.Size = new System.Drawing.Size(177, 12);
			this.lblTEventLevel.TabIndex = 17;
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(255, 3);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(69, 12);
			this.label24.TabIndex = 16;
			this.label24.Text = "등급 :";
			// 
			// lblTEventName
			// 
			this.lblTEventName.Location = new System.Drawing.Point(75, 3);
			this.lblTEventName.Name = "lblTEventName";
			this.lblTEventName.Size = new System.Drawing.Size(177, 12);
			this.lblTEventName.TabIndex = 15;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(3, 3);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(69, 12);
			this.label16.TabIndex = 14;
			this.label16.Text = "이벤트 명 :";
			// 
			// lblTDeviceName
			// 
			this.lblTDeviceName.Location = new System.Drawing.Point(75, 57);
			this.lblTDeviceName.Name = "lblTDeviceName";
			this.lblTDeviceName.Size = new System.Drawing.Size(177, 12);
			this.lblTDeviceName.TabIndex = 11;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(3, 57);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(69, 12);
			this.label12.TabIndex = 10;
			this.label12.Text = "장  비  명 :";
			// 
			// lblTOID
			// 
			this.lblTOID.Location = new System.Drawing.Point(75, 93);
			this.lblTOID.Name = "lblTOID";
			this.lblTOID.Size = new System.Drawing.Size(177, 12);
			this.lblTOID.TabIndex = 9;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(3, 93);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(69, 12);
			this.label10.TabIndex = 8;
			this.label10.Text = "OID :";
			// 
			// lblTDeviceIP
			// 
			this.lblTDeviceIP.Location = new System.Drawing.Point(327, 57);
			this.lblTDeviceIP.Name = "lblTDeviceIP";
			this.lblTDeviceIP.Size = new System.Drawing.Size(177, 12);
			this.lblTDeviceIP.TabIndex = 5;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(255, 57);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(69, 12);
			this.label6.TabIndex = 4;
			this.label6.Text = "장비 IP :";
			// 
			// lblTDevicePath
			// 
			this.lblTDevicePath.Location = new System.Drawing.Point(75, 39);
			this.lblTDevicePath.Name = "lblTDevicePath";
			this.lblTDevicePath.Size = new System.Drawing.Size(429, 12);
			this.lblTDevicePath.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(3, 39);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(69, 12);
			this.label4.TabIndex = 2;
			this.label4.Text = "장비 경로 :";
			// 
			// lblTEventTime
			// 
			this.lblTEventTime.Location = new System.Drawing.Point(327, 21);
			this.lblTEventTime.Name = "lblTEventTime";
			this.lblTEventTime.Size = new System.Drawing.Size(177, 12);
			this.lblTEventTime.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(255, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "발생 시간 :";
			// 
			// lblTTrapType
			// 
			this.lblTTrapType.Location = new System.Drawing.Point(75, 75);
			this.lblTTrapType.Name = "lblTTrapType";
			this.lblTTrapType.Size = new System.Drawing.Size(177, 12);
			this.lblTTrapType.TabIndex = 7;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(3, 75);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(69, 12);
			this.label8.TabIndex = 6;
			this.label8.Text = "Trap 종류 :";
			// 
			// lblTSpecCode
			// 
			this.lblTSpecCode.Location = new System.Drawing.Point(357, 75);
			this.lblTSpecCode.Name = "lblTSpecCode";
			this.lblTSpecCode.Size = new System.Drawing.Size(147, 12);
			this.lblTSpecCode.TabIndex = 13;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(255, 75);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(99, 12);
			this.label15.TabIndex = 12;
			this.label15.Text = "SpecificCode :";
			// 
			// btnComplete
			// 
			this.btnComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnComplete.BackColor = System.Drawing.Color.Transparent;
			this.btnComplete.BorderColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(207)), ((System.Byte)(208)));
			this.btnComplete.BorderEdgeRadius = 5;
			this.btnComplete.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Round;
			this.btnComplete.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
			this.btnComplete.ColorDepthFocus = 2;
			this.btnComplete.ColorDepthHover = 2;
			this.btnComplete.ColorDepthShadow = 2;
			this.btnComplete.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
			this.btnComplete.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
			this.btnComplete.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.LeftTopToRightBottom;
			this.btnComplete.Image = null;
			this.btnComplete.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
			this.btnComplete.ImageIndent = 0;
			this.btnComplete.ImageIndexDisable = -1;
			this.btnComplete.ImageIndexHover = -1;
			this.btnComplete.ImageIndexNormal = -1;
			this.btnComplete.ImageIndexSelect = -1;
			this.btnComplete.ImageList = null;
			this.btnComplete.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.btnComplete.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
			this.btnComplete.Location = new System.Drawing.Point(288, 426);
			this.btnComplete.Name = "btnComplete";
			this.btnComplete.Size = new System.Drawing.Size(93, 30);
			this.btnComplete.TabIndex = 21;
			this.btnComplete.Text = "조치완료";
			this.btnComplete.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
			this.btnComplete.TextIndent = 0;
			this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
			// 
			// btnSee
			// 
			this.btnSee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSee.BackColor = System.Drawing.Color.Transparent;
			this.btnSee.BorderColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(207)), ((System.Byte)(208)));
			this.btnSee.BorderEdgeRadius = 5;
			this.btnSee.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Round;
			this.btnSee.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
			this.btnSee.ColorDepthFocus = 2;
			this.btnSee.ColorDepthHover = 2;
			this.btnSee.ColorDepthShadow = 2;
			this.btnSee.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
			this.btnSee.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
			this.btnSee.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.LeftTopToRightBottom;
			this.btnSee.Image = null;
			this.btnSee.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
			this.btnSee.ImageIndent = 0;
			this.btnSee.ImageIndexDisable = -1;
			this.btnSee.ImageIndexHover = -1;
			this.btnSee.ImageIndexNormal = -1;
			this.btnSee.ImageIndexSelect = -1;
			this.btnSee.ImageList = null;
			this.btnSee.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.btnSee.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
			this.btnSee.Location = new System.Drawing.Point(189, 426);
			this.btnSee.Name = "btnSee";
			this.btnSee.Size = new System.Drawing.Size(93, 30);
			this.btnSee.TabIndex = 20;
			this.btnSee.Text = "인지";
			this.btnSee.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
			this.btnSee.TextIndent = 0;
			this.btnSee.Click += new System.EventHandler(this.btnSee_Click);
			// 
			// pnlCheck
			// 
			this.pnlCheck.BackColorNormal = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(247)));
			this.pnlCheck.BackColorPattern = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(247)));
			this.pnlCheck.BorderColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(207)), ((System.Byte)(208)));
			this.pnlCheck.BorderEdgeRadius = 7;
			this.pnlCheck.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Round;
			this.pnlCheck.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
			this.pnlCheck.CaptionLabel = false;
			this.pnlCheck.Controls.Add(this.panel1);
			this.pnlCheck.DockPadding.Bottom = 4;
			this.pnlCheck.DockPadding.Left = 4;
			this.pnlCheck.DockPadding.Right = 4;
			this.pnlCheck.DockPadding.Top = 4;
			this.pnlCheck.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.LeftTopToRightBottom;
			this.pnlCheck.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
			this.pnlCheck.Location = new System.Drawing.Point(13, 192);
			this.pnlCheck.Name = "pnlCheck";
			this.pnlCheck.Size = new System.Drawing.Size(561, 39);
			this.pnlCheck.TabIndex = 22;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.Add(this.lblCheckTime);
			this.panel1.Controls.Add(this.label9);
			this.panel1.Controls.Add(this.lblCheckUser);
			this.panel1.Controls.Add(this.label26);
			this.panel1.Location = new System.Drawing.Point(22, 9);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(516, 18);
			this.panel1.TabIndex = 2;
			// 
			// lblCheckTime
			// 
			this.lblCheckTime.BackColor = System.Drawing.Color.Transparent;
			this.lblCheckTime.Location = new System.Drawing.Point(327, 3);
			this.lblCheckTime.Name = "lblCheckTime";
			this.lblCheckTime.Size = new System.Drawing.Size(177, 12);
			this.lblCheckTime.TabIndex = 19;
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Location = new System.Drawing.Point(255, 3);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(69, 12);
			this.label9.TabIndex = 18;
			this.label9.Text = "인지 시간 :";
			// 
			// lblCheckUser
			// 
			this.lblCheckUser.Location = new System.Drawing.Point(87, 3);
			this.lblCheckUser.Name = "lblCheckUser";
			this.lblCheckUser.Size = new System.Drawing.Size(165, 12);
			this.lblCheckUser.TabIndex = 15;
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(3, 3);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(81, 12);
			this.label26.TabIndex = 14;
			this.label26.Text = "인지 사용자 :";
			// 
			// pnlManage
			// 
			this.pnlManage.BackColorNormal = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(247)));
			this.pnlManage.BackColorPattern = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(247)));
			this.pnlManage.BorderColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(207)), ((System.Byte)(208)));
			this.pnlManage.BorderEdgeRadius = 7;
			this.pnlManage.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Round;
			this.pnlManage.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
			this.pnlManage.CaptionLabel = false;
			this.pnlManage.Controls.Add(this.pnlManageView);
			this.pnlManage.Controls.Add(this.pnlManageInput);
			this.pnlManage.DockPadding.Bottom = 4;
			this.pnlManage.DockPadding.Left = 4;
			this.pnlManage.DockPadding.Right = 4;
			this.pnlManage.DockPadding.Top = 4;
			this.pnlManage.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.LeftTopToRightBottom;
			this.pnlManage.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
			this.pnlManage.Location = new System.Drawing.Point(13, 240);
			this.pnlManage.Name = "pnlManage";
			this.pnlManage.Size = new System.Drawing.Size(561, 174);
			this.pnlManage.TabIndex = 23;
			// 
			// pnlManageView
			// 
			this.pnlManageView.BackColor = System.Drawing.Color.Transparent;
			this.pnlManageView.Controls.Add(this.label31);
			this.pnlManageView.Controls.Add(this.lblManageTime);
			this.pnlManageView.Controls.Add(this.label13);
			this.pnlManageView.Controls.Add(this.lblManageUser);
			this.pnlManageView.Controls.Add(this.label21);
			this.pnlManageView.Controls.Add(this.txtManageContent);
			this.pnlManageView.Location = new System.Drawing.Point(22, 9);
			this.pnlManageView.Name = "pnlManageView";
			this.pnlManageView.Size = new System.Drawing.Size(516, 153);
			this.pnlManageView.TabIndex = 2;
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(3, 18);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(81, 12);
			this.label31.TabIndex = 20;
			this.label31.Text = "조치 내용    :";
			// 
			// lblManageTime
			// 
			this.lblManageTime.BackColor = System.Drawing.Color.Transparent;
			this.lblManageTime.Location = new System.Drawing.Point(327, 3);
			this.lblManageTime.Name = "lblManageTime";
			this.lblManageTime.Size = new System.Drawing.Size(177, 12);
			this.lblManageTime.TabIndex = 19;
			// 
			// label13
			// 
			this.label13.BackColor = System.Drawing.Color.Transparent;
			this.label13.Location = new System.Drawing.Point(255, 3);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(69, 12);
			this.label13.TabIndex = 18;
			this.label13.Text = "조치 시간 :";
			// 
			// lblManageUser
			// 
			this.lblManageUser.Location = new System.Drawing.Point(87, 3);
			this.lblManageUser.Name = "lblManageUser";
			this.lblManageUser.Size = new System.Drawing.Size(165, 12);
			this.lblManageUser.TabIndex = 15;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(3, 3);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(81, 12);
			this.label21.TabIndex = 14;
			this.label21.Text = "조치 사용자 :";
			// 
			// txtManageContent
			// 
			this.txtManageContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtManageContent.Location = new System.Drawing.Point(87, 18);
			this.txtManageContent.Name = "txtManageContent";
			this.txtManageContent.ReadOnly = true;
			this.txtManageContent.Size = new System.Drawing.Size(420, 126);
			this.txtManageContent.TabIndex = 22;
			this.txtManageContent.Text = "";
			// 
			// pnlManageInput
			// 
			this.pnlManageInput.BackColor = System.Drawing.Color.Transparent;
			this.pnlManageInput.Controls.Add(this.txtManageContentInput);
			this.pnlManageInput.Controls.Add(this.label2);
			this.pnlManageInput.Controls.Add(this.label37);
			this.pnlManageInput.Location = new System.Drawing.Point(22, 9);
			this.pnlManageInput.Name = "pnlManageInput";
			this.pnlManageInput.Size = new System.Drawing.Size(516, 153);
			this.pnlManageInput.TabIndex = 3;
			// 
			// txtManageContentInput
			// 
			this.txtManageContentInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtManageContentInput.Location = new System.Drawing.Point(87, 18);
			this.txtManageContentInput.Name = "txtManageContentInput";
			this.txtManageContentInput.Size = new System.Drawing.Size(417, 126);
			this.txtManageContentInput.TabIndex = 21;
			this.txtManageContentInput.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(81, 12);
			this.label2.TabIndex = 20;
			this.label2.Text = "조치 내용 :";
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(3, 3);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(414, 12);
			this.label37.TabIndex = 14;
			this.label37.Text = "조치 완료 처리를 위해 조치 내용을 입력해 주세요.";
			// 
			// btnTelnetStart
			// 
			this.btnTelnetStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTelnetStart.BackColor = System.Drawing.Color.Transparent;
			this.btnTelnetStart.BorderColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(207)), ((System.Byte)(208)));
			this.btnTelnetStart.BorderEdgeRadius = 5;
			this.btnTelnetStart.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Round;
			this.btnTelnetStart.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
			this.btnTelnetStart.ColorDepthFocus = 2;
			this.btnTelnetStart.ColorDepthHover = 2;
			this.btnTelnetStart.ColorDepthShadow = 2;
			this.btnTelnetStart.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
			this.btnTelnetStart.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
			this.btnTelnetStart.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.LeftTopToRightBottom;
			this.btnTelnetStart.Image = null;
			this.btnTelnetStart.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
			this.btnTelnetStart.ImageIndent = 0;
			this.btnTelnetStart.ImageIndexDisable = -1;
			this.btnTelnetStart.ImageIndexHover = -1;
			this.btnTelnetStart.ImageIndexNormal = -1;
			this.btnTelnetStart.ImageIndexSelect = -1;
			this.btnTelnetStart.ImageList = null;
			this.btnTelnetStart.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.btnTelnetStart.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
			this.btnTelnetStart.Location = new System.Drawing.Point(84, 426);
			this.btnTelnetStart.Name = "btnTelnetStart";
			this.btnTelnetStart.Size = new System.Drawing.Size(93, 30);
			this.btnTelnetStart.TabIndex = 20;
			this.btnTelnetStart.Text = "텔넷 접속";
			this.btnTelnetStart.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
			this.btnTelnetStart.TextIndent = 0;
			this.btnTelnetStart.Click += new System.EventHandler(this.btnTelnetStart_Click);
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(255, 93);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(66, 12);
			this.label25.TabIndex = 12;
			this.label25.Text = "추가정보 :";
			// 
			// lblValue
			// 
			this.lblValue.Location = new System.Drawing.Point(324, 93);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(180, 12);
			this.lblValue.TabIndex = 13;
			// 
			// frmEventView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(232)), ((System.Byte)(232)), ((System.Byte)(224)));
			this.ClientSize = new System.Drawing.Size(586, 464);
			this.Controls.Add(this.btnComplete);
			this.Controls.Add(this.btnSee);
			this.Controls.Add(this.btnSOP);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.btnTelnetStart);
			this.Controls.Add(this.pnlManage);
			this.Controls.Add(this.pnlCheck);
			this.Controls.Add(this.pnlInfo);
			this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmEventView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "이벤트 정보";
			this.pnlInfo.ResumeLayout(false);
			this.pnlThreshold.ResumeLayout(false);
			this.pnlSyslog.ResumeLayout(false);
			this.pnlTrap.ResumeLayout(false);
			this.pnlCheck.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.pnlManage.ResumeLayout(false);
			this.pnlManageView.ResumeLayout(false);
			this.pnlManageInput.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		#region 내부 필드 --------------------------------------------------------------------

		private Panel m_Panel               = null;
		private int m_SopId                 = 0;
		public long m_JobId                 = 0;
		public int m_EventRaiseServerID     = 0;
		private E_EventType m_Type          = E_EventType.Trap;
		private E_FaultStatus m_FaultStatus = E_FaultStatus.Normal;
		private E_EventLevel m_EventLevel   = E_EventLevel.UnKnown;
		
		private string m_SelectModelIP		= "";
		
		#endregion //내부 필드 ---------------------------------------------------------------

		/// <summary>
		/// 이벤트 뷰 창에 이벤트 뷰을 설정합니다.
		/// </summary>
		//public void SetEventView(E_EventType vEventType, long vJobID)
		public void SetEventView(E_EventType vEventType, EventLogInfo vEventLogInfo)
		{

			m_Type = vEventType;
			//m_JobId = vJobID;
			m_JobId = vEventLogInfo.JobID;
			m_EventRaiseServerID = vEventLogInfo.EventRaiseServerID;
			C1.Win.C1FlexGrid.Row tRow = null;
			lock(EventGlobal.m_ReceivedEvents.SyncRoot)
			{
				tRow = (C1.Win.C1FlexGrid.Row)EventGlobal.m_ReceivedEvents[(int)vEventType][m_JobId];
			}
			if(m_Panel != null) m_Panel.Visible = false;

			switch(vEventType)
			{
				case E_EventType.Trap:
					m_Panel = pnlTrap;
					TrapEvent tTE = EventGlobal.GetTrapEvent((int)tRow[1]);
					SetTrapEventView(m_JobId, tRow, tTE);
					break;
				case E_EventType.Syslog:
					m_Panel = pnlSyslog;
					SyslogEvent tSE = EventGlobal.GetSyslogEvent((int)tRow[1]);
					SetSyslogEventView(m_JobId, tRow, tSE);
					break;
				case E_EventType.Threshold:
					m_Panel = pnlThreshold;
					AutoThreshold tAT = EventGlobal.GetThresholdEvent((int)tRow[1]);
					SetThresholdEventView(m_JobId, tRow, tAT);
					break;
			}

			m_Panel.Visible = true;

			//SOP, 인지, 조치완료 필요여부
			if( m_FaultStatus == E_FaultStatus.Release || m_EventLevel == E_EventLevel.Clear ||  m_EventLevel == E_EventLevel.Info )
			{
				btnSee.Enabled = false;
				btnComplete.Enabled = false;
				//				btnSOP.Enabled = false;
				pnlCheck.Visible = false;
				pnlManage.Visible = false;
				this.Size = new System.Drawing.Size(592, 258);

			}
			else
			{
				if( tRow[3] == null && tRow[4] == null ) //인지가 안 된 경우
				{
					btnSee.Enabled = true;
					btnComplete.Enabled = false;
					pnlCheck.Visible = false;
					pnlManage.Visible = false;
					this.Size = new System.Drawing.Size(592, 258);
				}
				else if( tRow[3] != null && tRow[4] == null ) //인지가 된 경우
				{
					btnSee.Enabled = false;
					btnComplete.Enabled = true;
					pnlCheck.Visible = true;
					pnlManage.Visible = true;
					pnlManageInput.Visible = true;
					pnlManageView.Visible = false;
					this.Size = new System.Drawing.Size(592, 489);
					lblCheckTime.Text = tRow[16].ToString();		//인지 시간
					lblCheckUser.Text = tRow[17].ToString();		//인지 사용자
				}
				else if( tRow[3] != null && tRow[4] != null ) //인지, 조치완료 된 경우
				{
					btnSee.Enabled = false;
					btnComplete.Enabled = false;
					pnlCheck.Visible = true;
					pnlManage.Visible = true;
					pnlManageInput.Visible = false;
					pnlManageView.Visible = true;
					this.Size = new System.Drawing.Size(592, 489);
					lblCheckTime.Text = tRow[16].ToString();		//인지 시간
					lblCheckUser.Text = tRow[16].ToString();		//인지 사용자
					lblManageTime.Text = tRow[18].ToString();		//조치 시간
					lblManageUser.Text = tRow[19].ToString();		//조치 사용자
					txtManageContent.Text = tRow[20].ToString();	//조치 내용
				}
			}
		}

		//Trap
		public void SetTrapEventView(long vJobID, C1.Win.C1FlexGrid.Row vRow, TrapEvent vTE)
		{
			Color tColor = Color.Black;

			tColor = EventGlobal.m_EventLevelColor[(int)vTE.EventLevel];

			lblMessage.BackColor   = tColor;
			lblMessage.Text        = vTE.Description.ToString();
			lblTEventName.Text     = vTE.EventName.ToString();
			lblTEventLevel.Text    = vTE.EventLevel.ToString();
			lblTFaultStatus.Text   = EventGlobal.GetFaultStatusDesc(vTE.FaultStatus);

			lblTEventTime.Text     = vRow[11].ToString();		//발생 시간
			lblTDevicePath.Text    = vRow[8].ToString();		//장비 경로
			lblTDeviceName.Text    = vRow[9].ToString();		//장비 이름
			lblTDeviceIP.Text      = vRow[10].ToString();		//장비 IP
			lblValue.Text          = vRow[21].ToString();		//추가정보	

			lblTTrapType.Text      = vTE.GenericTrapType.ToString();
			lblTSpecCode.Text      = vTE.SpecificCode.ToString();
			lblTOID.Text           = vTE.TrapOID.ToString();

			//			vTE.SnmpVersion.ToString();
			//			m_Message.ToString();

			m_FaultStatus = vTE.FaultStatus;
			m_EventLevel  = vTE.EventLevel;
			m_SopId       = vTE.SopID;

			m_SelectModelIP = lblTDeviceIP.Text.Trim();
		}

		//Syslog
		public void SetSyslogEventView(long vJobID, C1.Win.C1FlexGrid.Row vRow, SyslogEvent vSE)
		{
			Color tColor = Color.Black;

			tColor = EventGlobal.m_EventLevelColor[(int)vSE.EventLevel];

			lblMessage.BackColor = tColor;

			lblMessage.Text      = vSE.Description.ToString();
			lblSEventName.Text   = vSE.EventName.ToString();
			lblSEventLevel.Text  = vSE.EventLevel.ToString();
			lblSFaultStatus.Text = EventGlobal.GetFaultStatusDesc(vSE.FaultStatus);

			lblSEventTime.Text   = vRow[11].ToString();	//발생 시간
			lblSDevicePath.Text  = vRow[8].ToString();	//장비 경로
			lblSDeviceName.Text  = vRow[9].ToString();	//장비 이름
			lblSDeviceIP.Text    = vRow[10].ToString();		//장비 IP

			lblSFacility.Text    = vRow[13].ToString();	//Facility
			lblSSeverity.Text    = vRow[14].ToString();	//Serverity
			lblSLevel.Text       = vRow[15].ToString();		//Syslog Level

			//			vSE.CheckString.ToString();
			//			m_Message.ToString();

			m_FaultStatus        = vSE.FaultStatus;
			m_EventLevel         = vSE.EventLevel;
			m_SopId              = vSE.SopID;

			m_SelectModelIP = lblSDeviceIP.Text.Trim();
		}

		//Threshold
		public void SetThresholdEventView(long vJobID, C1.Win.C1FlexGrid.Row vRow, AutoThreshold vAT)
		{
			Color tColor = Color.Black;

			tColor = EventGlobal.m_EventLevelColor[(int)vAT.EventLevel];

			lblMessage.BackColor = tColor;

			lblMessage.Text = vAT.Description.ToString();
			lblHEventName.Text = vAT.Name.ToString();
			lblHEventLevel.Text = vAT.EventLevel.ToString();
			lblHFaultStatus.Text = vRow[7].ToString();		//장애 상태

			lblHEventTime.Text = vRow[11].ToString();		//발생 시간
			lblHDevicePath.Text = vRow[8].ToString();		//장비 경로
			lblHDeviceName.Text = vRow[9].ToString();		//장비 이름
			lblHDeviceIP.Text = vRow[10].ToString();		//장비 IP
			lblHThresholdValue.Text = vRow[21].ToString();  //임계치 값

			//			m_Message.ToString();

			m_FaultStatus = vAT.FaultStatus;
			m_EventLevel = vAT.EventLevel;
			m_SopId = vAT.SopID;

            m_SelectModelIP = lblHDeviceIP.Text.Trim();
		}

		private void butCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnSOP_Click(object sender, System.EventArgs e)
		{
			frmSOP tfSOP = new frmSOP();
			if(tfSOP.SetSOP(m_SopId))
			{
				tfSOP.ShowDialog(this);
			}
			else
			{
				tfSOP.Close();
			}
		}

		/// <summary>
		/// 이벤트에 대하여 인지 처리 합니다.
		/// </summary>
		private void btnSee_Click(object sender, System.EventArgs e)
		{
			if( EventGlobal.EventSyncCheck(m_Type, m_JobId, m_EventRaiseServerID) ) 
			{
				btnSee.Enabled = false;
				btnComplete.Enabled = true;
			}
			else
			{
//				this.Close();
			}
			this.Close();
		}

		/// <summary>
		/// 이벤트에 대하여 조치완료 처리 합니다.
		/// </summary>
		private void btnComplete_Click(object sender, System.EventArgs e)
		{
			if( EventGlobal.EventSyncManage(m_Type, m_JobId, m_EventRaiseServerID,  txtManageContentInput.Text.ToString()) ) 
			//if( EventGlobal.EventSyncManage(m_Type, m_JobId, m_EventRaiseServerID,  txtManageContent.Text.ToString()) ) 
			{
				btnComplete.Enabled = false;
			}
			else
			{
//				this.Close();
			}
			this.Close();
		}
		
		/// <summary>
		/// 텔넷 실행을 하는 부분입니다.
		/// </summary>
		private void btnTelnetStart_Click(object sender, System.EventArgs e)
		{
			//텔넷 실행
			System.Diagnostics.Process m_Process = new System.Diagnostics.Process();
			string m_Telnet						 = "telnet.exe";
			string m_Argument					 = m_SelectModelIP;
			
			m_Process.StartInfo.FileName		 = m_Telnet;
			m_Process.StartInfo.Arguments		 = m_Argument;
			m_Process.Start();
		}
	}
}