using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using SECUONELibrary;
using System.Diagnostics;

namespace SYSLOGD
{
    public partial class SyslogD : Form
    {
        private SOSyslog sSyslog = null;
        private string sMultiSyslog = "";
        /// <summary>
        /// 스래드 종료 요청 여부 입니다.
        /// </summary>
        private bool m_isShutDown = false;
        /// <summary>
        /// 스래드 종료 요청 여부 입니다.
        /// </summary>
        private bool m_isProcShutDown = false;
        /// <summary>
        /// 내부 Syslog큐 처리시 사용할 사용자 알림 이벤트 입니다.
        /// </summary>
        private ManualResetEvent m_SyslogQueueMRE = null;
        /// <summary>
        /// Syslog 패킷을 임시로 저장할 큐 입니다.
        /// </summary>
        private Queue m_SyslogQueue = null;
        /// <summary>
        /// Message 패킷을 임시로 저장할 큐 입니다.
        /// </summary>
        private Queue m_MessageQueue = null;
        private bool m_SyslogPrint = false;
        /// <summary>
        /// Syslog를 처리 할 스래드 풀입니다.
        /// </summary>
        private SECUONELibrary.SOThreadPool m_SyslogWorkThreadPool;
        /// <summary>
        /// Syslog를 처리할 스래드 입니다.
        /// </summary>
        private Thread m_SyslogProcessThread = null;
        /// <summary>
        /// Syslog를 처리할 스래드 입니다.
        /// </summary>
        private Thread m_ProcessThread = null;
        /// <summary>
        /// Syslog를 처리할 스래드 입니다.
        /// </summary>
        private Thread m_HashProcessThread = null;
        /// <summary>
        /// Syslog를 처리 할 스래드 풀입니다.
        /// </summary>
        private SECUONELibrary.SOThreadPool m_LogWorkThreadPool;
        private Process m_Proc = null;

        /// <summary>
        /// Log그를 저장할 파일로그 객체 입니다.
        /// </summary>
        private SOFileLog m_Log = null;
        /// <summary>
        /// 로그파일의 저장 위치 입니다.
        /// </summary>
        private string m_LogPath = "";
        private int LogSysCnt = 0;
        private int SysCnt = 0;
        private delegate void ListRefresh(string strMsg);
        private bool m_isThread = false;
        private string m_Filter = "";
        /// <summary>
        /// Syslog 받음을 알리는 이벤트 입니다.
        /// </summary>
        public event DataReceivedEventHandler DataReceived;

        public SyslogD()
        {
            InitializeComponent();

            sSyslog = new SOSyslog(131070);
            m_LogPath = System.Windows.Forms.Application.StartupPath + "\\log\\"; //
            m_Log = new SOFileLog(m_LogPath, "CHAOSEND_SOSyslog", true, true);
            m_Log.Extension = "log";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            chkLog.Enabled = false;
            txtFilter.Enabled = false;
            txtPort.Enabled = false;
            m_LogPath = System.Windows.Forms.Application.StartupPath + "\\log\\"; //
            m_Log = new SOFileLog(m_LogPath, "CHAOSEND_SOSyslog", true, true);
            m_Log.Extension = "log";
            m_Filter = txtFilter.Text;

            m_SyslogQueue = new Queue();
            m_MessageQueue = new Queue();

            //사용자 사용 이벤트를 생성합니다.
            m_SyslogQueueMRE = new ManualResetEvent(false);
            sSyslog.SyslogReceived += new SyslogReceivedEventHandler(m_SyslogServer_SyslogReceived);
            int iPort = 514;
            try
            {
                iPort = int.Parse(txtPort.Text);
            }
            catch
            {
                MessageBox.Show("포트 오류");
                return;
            }
            sSyslog.StartSyslogServer(iPort);
            m_isShutDown = false;
            //m_isShutDown = false;
            m_Log.PrintLogEnter("CHAOSEND SYSLOG 서버가 시작되었습니다.");

            //Syslog 처리 스래드를 시작합니다.
            m_SyslogProcessThread = new Thread(new ThreadStart(SyslogCheckProcess));
            m_SyslogProcessThread.Start();
            //m_ProcessThread = new Thread(new ThreadStart(CheckProcess));
            //m_ProcessThread.Start();
            m_HashProcessThread = new Thread(new ThreadStart(HashTableProcess));
            m_HashProcessThread.Start();
            //작업을 처리할 스래드 풀을 생성합니다.
            m_SyslogWorkThreadPool = new SOThreadPool(10);
            m_SyslogWorkThreadPool.StartThreadPool();

            //작업을 처리할 스래드 풀을 생성합니다.
            m_LogWorkThreadPool = new SOThreadPool(10);
            m_LogWorkThreadPool.StartThreadPool();

            m_isThread = true;

        }

        private void btnStop_Click(object sender, EventArgs e)
        {

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            chkLog.Enabled = true;
            txtFilter.Enabled = true;
            txtPort.Enabled = true;
            sSyslog.SyslogReceived -= new SyslogReceivedEventHandler(m_SyslogServer_SyslogReceived);

            //rtbSyslog.Text = sMultiSyslog;
            m_isShutDown = true;
            m_SyslogQueue = null;
            m_MessageQueue = null;
            //사용자 설정 이벤트를 닫기 합니다.
            if (m_SyslogQueue != null)
            {
                m_SyslogQueue = null;
            }
            if (m_MessageQueue != null)
            {
                m_MessageQueue = null;
            }
            //사용자 설정 이벤트를 닫기 합니다.
            if (m_SyslogQueueMRE != null)
            {
                m_SyslogQueueMRE.Close();
                m_SyslogQueueMRE = null;
            }
            //스래드를 종료 합니다.
            if (m_SyslogProcessThread.IsAlive)
            {
                try
                {
                    //Syslog 처리 스래드를 강제 종료 합니다.
                    m_SyslogProcessThread.Abort();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //스래드를 종료 합니다.
            if (m_HashProcessThread.IsAlive)
            {
                try
                {
                    //Syslog 처리 스래드를 강제 종료 합니다.
                    m_HashProcessThread.Abort();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //사용자 설정 이벤트를 닫기 합니다.
            if (m_SyslogQueueMRE != null)
            {
                m_SyslogQueueMRE.Close();
                m_SyslogQueueMRE = null;
            }
            sSyslog.Dispose();
            m_Log.PrintLogEnter("CHAOSEND SYSLOG 서버가 중지 되었습니다.");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            m_isThread = false;
            m_isShutDown = true;
            //사용자 설정 이벤트를 닫기 합니다.
            if (m_SyslogQueue != null)
            {
                m_SyslogQueue = null;
            }
            //사용자 설정 이벤트를 닫기 합니다.
            if (m_MessageQueue != null)
            {
                m_MessageQueue = null;
            }

            //스래드를 종료 합니다.
            if (m_SyslogProcessThread != null)
            {
                if (m_SyslogProcessThread.IsAlive)
                {
                    try
                    {
                        //Syslog 처리 스래드를 강제 종료 합니다.
                        m_SyslogProcessThread.Abort();
                    }
                    catch (Exception ex)
                    {
                        ex = ex;
                    }
                }
            }
            //스래드를 종료 합니다.
            if (m_HashProcessThread != null)
            {
                if (m_HashProcessThread.IsAlive)
                {
                    try
                    {
                        //Syslog 처리 스래드를 강제 종료 합니다.
                        m_HashProcessThread.Abort();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            //사용자 설정 이벤트를 닫기 합니다.
            if (m_SyslogQueueMRE != null)
            {
                m_SyslogQueueMRE.Close();
                m_SyslogQueueMRE = null;
            }
            if (sSyslog != null)
            {
                sSyslog.Dispose();
            }
            if (m_isThread == true)
            {
                m_SyslogWorkThreadPool.Dispose();
                m_LogWorkThreadPool.Dispose();
            }
            m_Log.PrintLogEnter("CHAOSEND SYSLOG 서버가 종료 되었습니다.");
            m_Log.Dispose();
            this.Dispose();
            this.Close();
        }
        /// <summary>
        /// Syslog를 처리할 이벤트 메소드 입니다.
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체 입니다.</param>
        /// <param name="e">Syslog 정보가 들어있는 어규먼트 입니다.</param>
        private void m_SyslogServer_SyslogReceived(object sender, SyslogEventArgs e)
        {
            //받은 Syslog를 내부 큐에 저장합니다.
            if (m_isShutDown == false)
            {
                lock (m_SyslogQueue)
                {
                    m_SyslogQueue.Enqueue(e);
                    m_SyslogQueueMRE.Set();
                }
            }
        }

        /// <summary>
        /// Syslog를 처리할 메소드 입니다.
        /// </summary>
        private void SyslogCheckProcess()
        {
            SyslogEventArgs tSyslog = null;
            int tCount = 0;
            int SysCnt = 0;
            string tmpSyslog = "";
            try
            {
                while (!m_isShutDown)
                {
                    lock (m_SyslogQueue)
                    {
                        tCount = m_SyslogQueue.Count;
                        if (tCount > 0)
                        {
                            tSyslog = (SyslogEventArgs)m_SyslogQueue.Dequeue();
                        }
                    }

                    if (tCount > 0)
                    {
                        if (chkLog.Checked == true)
                        m_LogWorkThreadPool.QueueWorkItem(new TMSWorkItem(new WorkItemMethod(LogWriteProcess), tSyslog));
                        m_SyslogWorkThreadPool.QueueWorkItem(new TMSWorkItem(new WorkItemMethod(MessageProcess), tSyslog));
                    }

                    else
                    {
                        m_SyslogQueueMRE.Reset();
                        m_SyslogQueueMRE.WaitOne();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Syslog를 처리할 메소드 입니다.
        /// </summary>
        private void CheckProcess()
        {
            Process p_Proc = null;
            Process[] p_ProcList;
            try
            {
                while (!m_isProcShutDown)
                {
                    try
                    {
                        string sProcList = "";
                        p_Proc = new Process();
                        //p_Proc = System.Diagnostics.Process.GetProcessById(1);
                        p_ProcList = System.Diagnostics.Process.GetProcesses();
                        for (int i = 0; i < p_ProcList.Length; i++)
                        {
                            try
                            {
                                p_Proc = p_ProcList[i];
                                DataReceived = new DataReceivedEventHandler(DataReceived_Method);
                                sProcList += p_Proc.Id + " " + p_Proc.ProcessName + "         " + p_Proc.ToString() + "         " + p_Proc.UserProcessorTime.ToString() + "\n";
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        SetProcMonMessage(sProcList);
                    }
                    catch
                    {
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private object LogWriteProcess(object tSyslog)
        {
            SyslogEventArgs vSyslog;
            string tmpSyslog = "";
            vSyslog = (SyslogEventArgs)tSyslog;
            LogSysCnt++;
            m_Log.PrintLogEnter(" [" + vSyslog.SenderAddress.ToString() + "] " + vSyslog.SyslogMessage.Message);

            return null;
        }
        private object MessageProcess(object tSyslog)
        {
            SyslogEventArgs vSyslog;
            string tmpSyslog = "";
            vSyslog = (SyslogEventArgs)tSyslog;

            if (m_Filter != "")
            {
                if (vSyslog.SyslogMessage.Message.IndexOf(m_Filter) > -1)
                {
                    SysCnt++;
                    tmpSyslog = vSyslog.Time.ToString() + " [" + vSyslog.SenderAddress.ToString() + "] " + vSyslog.SyslogMessage.Message + "\n";
                    lock (m_MessageQueue)
                    {
                        m_MessageQueue.Enqueue(tmpSyslog);
                    }
                }
            }
            else
            {
                SysCnt++;
                tmpSyslog = vSyslog.Time.ToString() + " [" + vSyslog.SenderAddress.ToString() + "] " + vSyslog.SyslogMessage.Message + "\n";
                lock (m_MessageQueue)
                {
                    m_MessageQueue.Enqueue(tmpSyslog);
                }
            }
            return null;
        }

        /// <summary>
        /// Syslog를 처리할 메소드 입니다.
        /// </summary>
        private void HashTableProcess()
        {
            try
            {
                string tmpSyslog = "";
                int iSysCnt = 0;
                int tCount = 0;
                while (!m_isShutDown)
                {
                    lock (m_MessageQueue)
                    {
                        tCount = m_MessageQueue.Count;
                        if (tCount > 0)
                        {
                            for (int i = 0; i < tCount; i++)
                            {
                                tmpSyslog += (String)m_MessageQueue.Dequeue();
                            }
                        }
                    }

                    if (tmpSyslog != "")
                        SetProcessMessage(tmpSyslog);
                    tmpSyslog = "";
                    Thread.Sleep(300);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetProcessMessage(string strMsg)
        {
            if (rtSyslog.InvokeRequired)
            {
                ListRefresh lrf = new ListRefresh(SetProcessMessage);
                rtSyslog.Invoke(lrf, strMsg);
            }
            else
            {
                rtSyslog.AppendText(strMsg);
                rtSyslog.ScrollToCaret();
            }
        }

        private void SetProcMonMessage(string strMsg)
        {
            if (rtSyslog.InvokeRequired)
            {
                ListRefresh lrf = new ListRefresh(SetProcMonMessage);
                rtSyslog.Invoke(lrf, strMsg);
            }
            else
            {
                rtSyslog.Text = strMsg;
                rtSyslog.ScrollToCaret();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtSyslog.Text = "";
            LogSysCnt = 0;
            SysCnt = 0;
            
        }

        private void SyslogD_FormClosed(object sender, FormClosedEventArgs e)
        {
            //사용자 설정 이벤트를 닫기 합니다.
            if (m_SyslogQueue != null)
            {
                m_SyslogQueue = null;
            }
            //사용자 설정 이벤트를 닫기 합니다.
            if (m_SyslogQueueMRE != null)
            {
                m_SyslogQueueMRE.Close();
                m_SyslogQueueMRE = null;
            }
            //스래드를 종료 합니다.
            if (m_SyslogProcessThread != null)
            {
                if (m_SyslogProcessThread.IsAlive)
                {
                    try
                    {
                        //Syslog 처리 스래드를 강제 종료 합니다.
                        m_SyslogProcessThread.Abort();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            //스래드를 종료 합니다.
            if (m_HashProcessThread != null)
            {
                if (m_HashProcessThread.IsAlive)
                {
                    try
                    {
                        //Syslog 처리 스래드를 강제 종료 합니다.
                        m_HashProcessThread.Abort();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            if (sSyslog != null)
            {
                sSyslog.Dispose();
            }
            if (m_isThread == true)
            {
                m_SyslogWorkThreadPool.Dispose();
                m_LogWorkThreadPool.Dispose();
            }
            //sSyslog.Dispose();
            //sSyslog = null;
            m_Log.Dispose();
            this.Dispose();
            this.Close();
        }

        private void btnProcMon_Click(object sender, EventArgs e)
        {
            m_isProcShutDown = false;
            //m_ProcessThread = new Thread(new ThreadStart(CheckProcess));
            //m_ProcessThread.Start();
            System.Diagnostics.
            Process p_Proc = null;
            Process[] p_ProcList;
            string sProcList = "";
            p_Proc = new Process();
            //p_Proc = System.Diagnostics.Process.GetProcessById(1);
            p_ProcList = System.Diagnostics.Process.GetProcesses();
            for (int i = 0; i < p_ProcList.Length; i++)
            {
                try
                {
                    p_Proc = p_ProcList[i];
                    if (p_Proc.ProcessName == "OUTLOOK")
                    {
                        m_Proc = p_Proc;
                        m_Proc.OutputDataReceived += new DataReceivedEventHandler(DataReceived_Method);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void btnProcStop_Click(object sender, EventArgs e)
        {

            m_isProcShutDown = true;
            //스래드를 종료 합니다.
            //if (m_ProcessThread.IsAlive)
            //{
            //    try
            //    {
            //        //Syslog 처리 스래드를 강제 종료 합니다.
            //        m_ProcessThread.Abort();
            //    }
            //    catch (Exception ex)
            //    {
            //        //MessageBox.Show(ex.Message);
            //    }
            //}
            //스래드를 종료 합니다.
        }

        private void DataReceived_Method(object sender, DataReceivedEventArgs e)
        {
            //받은 Syslog를 내부 큐에 저장합니다.
            if (m_isProcShutDown == false)
            {
                SetProcMonMessage(e.Data.ToString());
            }
        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {

        }
    }
}
