using System;
using System.Threading;
using System.Collections;
using System.Diagnostics;

namespace SECUONELibrary
{
	#region ������Ǯ �̺�Ʈ ��Ը�Ʈ �Դϴ� ------------------------------------------------
	/// <summary>
	/// ������ Ǯ���� �߻��� �̺�Ʈ�� �� ��Ը�Ʈ Ŭ���� �Դϴ�.
	/// </summary>
	public class TMSWorkItemEventArgs : EventArgs
	{
		/// <summary>
		/// �۾� �������Դϴ�.
		/// </summary>
		private TMSWorkItem m_WorkItem;

		/// <summary>
		/// �۾������� �̺�Ʈ ��Ը�Ʈ Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vWorkItem">�۾� ������ �Դϴ�.</param>
		public TMSWorkItemEventArgs(TMSWorkItem vWorkItem)
		{
			m_WorkItem = vWorkItem;
		}

		/// <summary>
		/// �۾� �������� ��ȯ�մϴ�.
		/// </summary>
		public TMSWorkItem WorkItem
		{
			get
			{
				return m_WorkItem;
			}
		}
	}
	#endregion //������Ǯ �̺�Ʈ ��Ը�Ʈ �Դϴ� ------------------------------------------------
	
	#region ������ Ǯ Ŭ���� �Դϴ� --------------------------------------------------------
	/// <summary>
	/// ���ÿ� �������� �����带 �����ϰ� ������ �� �ֽ��ϴ�. 
	/// </summary>
	public class SOThreadPool : IDisposable
	{
		/// <summary>
		/// [�⺻] ���ÿ� �۾��� �������� �ְ� �����Դϴ�.
		/// </summary>
		private const int c_DefaultMaxWorkThreads = 25;					
		/// <summary>
		/// ���ÿ� �۾��� �������� �ְ� �����Դϴ�.
		/// </summary>
		private int m_MaxWorkThreads;		
		/// <summary>
		/// �۾��������� ������ Queue�Դϴ�.
		/// </summary>
		private Queue m_WorkItemQueue;			
//		/// <summary>
//		///  �����尡 �۾��������� ��ٸ��� Waiter�� ������ Queue�Դϴ�.
//		/// </summary>
//		private Queue m_WaitQueue;
		/// <summary>
		/// �۾� ������ ����� �Դϴ�.
		/// </summary>
		private Hashtable m_WorkThreads;
		/// <summary>
		/// ������ Ǯ�� ���� �Ǿ������� �����Դϴ�.
		/// </summary>
		private bool m_isShutDown;
		/// <summary>
		/// ���� �������� �۾��� ������ ��Ÿ���ϴ�.
		/// </summary>
		private int m_RunningWorkCount;
		
		/// <summary>
		/// �۾� ������ �ϳ��� �۾��̳������� �߻��ϴ� �۾��Ϸ� �̺�Ʈ�� �ʿ��� �̺�Ʈ �ڵ鷯�Դϴ�.
		/// </summary>
		public delegate void WorkCompletedEventHandler(object sender, TMSWorkItemEventArgs e);
		/// <summary>
		/// �۾� �������� �۾��� ������ �߻��ϴ� �̺�Ʈ �Դϴ�.
		/// </summary>
		public event WorkCompletedEventHandler WorkCompleted;
		/// <summary>
		/// ������ Ǯ�� ���� �Ǿ����� �˸��� �̺�Ʈ �Դϴ�.
		/// </summary>
		public event EventHandler ThreadPoolStoped;
		
		/// <summary>
		/// ������Ǯ Ŭ������ �⺻ ������ �Դϴ�.
		/// </summary>
		public SOThreadPool() : this(c_DefaultMaxWorkThreads) {}

		/// <summary>
		/// ������ Ǯ Ŭ������ ������ �Դϴ�.
		/// </summary>
		/// <param name="vMaxWorkThreads">���ÿ� �۾��� �ִ� ������ �����Դϴ�.</param>
		public SOThreadPool(int vMaxWorkThreads)			
		{
			if(vMaxWorkThreads < 1)
			{
				throw new ArgumentOutOfRangeException("CTMSThreadPool", "vMaxWorkThreads�� 1���� ���� �� �����ϴ�");
			}

			m_RunningWorkCount = 0;
			m_MaxWorkThreads = vMaxWorkThreads;
			Initialize();
		}	

		/// <summary>
		/// ������ Ǯ�� �����մϴ�.
		/// </summary>
		public void Dispose()
		{
			StopThreadPool();

			m_WorkItemQueue = null;
			//m_WaitQueue = null;
			m_WorkThreads = null;
		}

		/// <summary>
		/// ������ Ǯ�� �ʱ�ȭ�մϴ�
		/// </summary>
		private void Initialize()
		{
			m_isShutDown = true;
			
			//�۾� ť�� �ʱ�ȭ �Ѵ�
			m_WorkItemQueue = new Queue();
			//m_WaitQueue = new Queue();
			m_WorkThreads = new Hashtable();
		}

		/// <summary>
		/// ������ Ǯ�� �ִ�ũ�⸦ �����ϰų� �����ɴϴ�.
		/// </summary>
		public int MaxWorkThread
		{
			get { return m_MaxWorkThreads; }
			set 
			{ 
				if(value < 1) 
				{
					throw new ArgumentOutOfRangeException("MaxWorkThread", "value�� 1���� ���� �� �����ϴ�");
				}

				lock(this)
				{
					if(m_MaxWorkThreads < value)
					{
						int tCnt = value - m_MaxWorkThreads;
						m_MaxWorkThreads = value;
						RunWorkThread(tCnt);
					}
					else m_MaxWorkThreads = value;
				}
			}
		}		

		/// <summary>
		/// ������Ǯ �۾� �������� ó���մϴ�.
		/// </summary>
		private void ProcessWorkItem()
		{
			TMSWorkItem tWorkItem = null;
			bool tisRun = false;
			//ManualResetEvent tmre = null;

			try
			{
				//�۾��������� �����մϴ�.
				while(!m_isShutDown)
				{		
					tWorkItem = null;

					lock(m_WorkItemQueue.SyncRoot)
					{						
						if(m_WorkItemQueue.Count > 0)
						{
							tWorkItem = (TMSWorkItem)m_WorkItemQueue.Dequeue();
						}
					}					
					
					if(tWorkItem != null)
					{						
						if(tWorkItem.Cancel == false)
						{
							
							m_RunningWorkCount++;
							tisRun = true;

							try
							{
								//�۾� �������� ���� �Ѵ�
								tWorkItem.Execute();
							}
							catch(Exception ex)
							{
								Console.WriteLine("** ThreadWork Exception : " + ex.ToString());
							}
							
							m_RunningWorkCount--;
							tisRun = false;							
							
						}
						if(WorkCompleted != null) WorkCompleted(this, new TMSWorkItemEventArgs(tWorkItem));
						
						Thread.Sleep(1); //Thread to Thread Interval						
					}
					else
					{						
						Thread.Sleep(50); //Thread to Thread Interval
					}			
				}
			}
			catch(ThreadAbortException tae)
			{
				tae = tae;
				if(tisRun == true)
				{
					lock(this)
					{
						m_RunningWorkCount--;
					}
				}
			}
			catch(Exception e)
			{
				if(tisRun == true)
				{
					lock(this)
					{
						m_RunningWorkCount--;
					}
				}
				e = e;
			}
		}

		/// <summary>
		/// ������ Ǯ�� �۾� �������� �߰� �մϴ�.
		/// </summary>
		/// <param name="vWorkItem">������ Ǯ�� �۾� �������Դϴ�</param>
		public void QueueWorkItem(TMSWorkItem vWorkItem)
		{
				try
				{
					if(m_isShutDown) return;

					lock(m_WorkItemQueue.SyncRoot)
					{
						m_WorkItemQueue.Enqueue(vWorkItem);
					}
//					if(m_WaitQueue.Count > 0)
//					{
//						ManualResetEvent tmre = (ManualResetEvent)m_WaitQueue.Dequeue();
//						tmre.Set();
//					}
				}
				catch {}
		}		

		/// <summary>
		/// ������ Ǯ�� �����մϴ�.
		/// </summary>
		public void StartThreadPool()
		{
			if(m_isShutDown == true)
			{
				m_isShutDown = false;
				RunWorkThread(m_MaxWorkThreads);
			}
		}

		/// <summary>
		/// ������ ������ŭ �����带 �����մϴ�.
		/// </summary>
		/// <param name="vCount">���� �� �������� ���� �Դϴ�.</param>
		private void RunWorkThread(int vCount)
		{
			//ManualResetEvent tmre;
			
			for(int i = 0; i < vCount; i++)
			{
				try
				{
					Thread tWorkThread = new Thread(new ThreadStart(ProcessWorkItem));
					tWorkThread.Priority = ThreadPriority.Highest;
				
//					//�۾��������� ������ �����带 ����Ű������ ManualResetEvent�� �����Ѵ�
//					tmre = new ManualResetEvent(false);
				
					//�����带 �ؽ� ���̺� ����Ѵ�
					lock(m_WorkThreads.SyncRoot)
					{
						m_WorkThreads.Add(tWorkThread, tWorkThread); 
					}				
					tWorkThread.IsBackground = true;
				
					//�����带 �����Ѵ�
					tWorkThread.Start();
				}
				catch {}
			}
		}

		/// <summary>
		/// ������ Ǯ�� �����մϴ�.
		/// </summary>
		public void StopThreadPool()
		{		
			int tCnt = 0;

			m_isShutDown = true;

			lock(m_WorkItemQueue.SyncRoot)
			{
				m_WorkItemQueue.Clear();
			}			

			Thread.Sleep(500);

			try
			{
				lock(this)
				{					
					Thread.Sleep(100);

					foreach(Thread thread in m_WorkThreads.Keys)
					{
						if(thread != null)
						{		
							try
							{
								tCnt = 0;

								while(thread.IsAlive)
								{
									if(tCnt > 3) break;
									tCnt++;
									thread.Join(1000);								
								}

								if(tCnt > 3) thread.Abort();
							}
							catch(Exception ex)
							{
								Console.WriteLine(ex.ToString());
							}
						}							
					}						
					m_WorkThreads.Clear();						
				}
			
				//������ ������ �̺�Ʈ �߻�
				if(ThreadPoolStoped != null) ThreadPoolStoped(this, EventArgs.Empty);
			}
			catch(Exception ex1)
			{
				Console.WriteLine(ex1.ToString());
			}
		}

		/// <summary>
		/// ���� Queue�� ������� �۾� �������� ������ ��ȯ�մϴ�.
		/// </summary>
		public int QueueWorkItemCount
		{
			get
			{
				lock(m_WorkItemQueue)
				{
					return m_WorkItemQueue.Count;
				}
			}
		}
		
		/// <summary>
		/// ���� �������� �۾��� ������ ��ȯ �մϴ�.
		/// </summary>
		public int RunningWorkCount
		{
			get
			{
				lock(this)
				{
					return m_RunningWorkCount;
				}
			}
		}
	}
	#endregion //������ Ǯ Ŭ���� �Դϴ� --------------------------------------------------------

	#region �۾� ������ Ŭ���� -------------------------------------------------------------
	/// <summary>
	/// ������ �۾������� ��������Ʈ �Դϴ�.
	/// </summary>
	public delegate object WorkItemMethod(object arg);
	
	//������ �۾� ������
	/// <summary>
	/// ������ Ǯ�� �۾��� ��û�ϱ����� �۾� �������Դϴ�.
	/// </summary>
	public class TMSWorkItem
	{		
		/// <summary>
		/// ������ �۾��� ������ Method �Դϴ�.
		/// </summary>
		private WorkItemMethod m_Method = null;
		/// <summary>
		/// ������ �۾� Method�� �Ķ���� �迭 �Դϴ�.
		/// </summary>
		private object m_Argument = null;
		/// <summary>
		/// ������ �۾� Method�� ��ȯ�ϴ� ��ȯ���Դϴ�.
		/// </summary>
		private object m_Return = null;
		/// <summary>
		/// �۾��� �������� ����
		/// </summary>
		private bool m_Cancel = false;

		/// <summary>
		/// �۾� ������ Ŭ������ �⺻������ �Դϴ�.
		/// </summary>
		public TMSWorkItem() {}

		/// <summary>
		/// �۾� ������ Ŭ������ �������Դϴ�.
		/// </summary>
		/// <param name="vMethod">������ �۾��� ������ Method �Դϴ�.</param>
		/// <param name="vArgument">������ �۾� Method�� �ʿ��� �Ķ���� �迭 �Դϴ�.</param>
		public TMSWorkItem(WorkItemMethod vMethod, object vArgument)
		{
			m_Method = vMethod;
			m_Argument = vArgument;
		}
		
		/// <summary>
		/// ������ �۾��� ������ Method�� �������ų� �����մϴ�.
		/// </summary>
		public WorkItemMethod Method
		{
			get { return m_Method; }
			set
			{
				if(m_Method.Equals(value) == false)
				{
					m_Method = value;
				}
			}
		}

		/// <summary>
		/// ������ �۾� Method�� �Ķ���� ��� �������ų� �����մϴ�.
		/// </summary>
		public object Argument
		{
			get { return m_Argument; }
			set { m_Argument = value; }
		}

		/// <summary>
		/// ������ �۾� Method�� ��ȯ���� �����ɴϴ�.
		/// </summary>
		public object Return
		{
			get { return m_Return; }
		}

		/// <summary>
		/// ������ �۾� �޼ҵ带 ���� �մϴ�.
		/// </summary>
		public void Execute()
		{
			m_Return = m_Method(m_Argument);
		}

		/// <summary>
		/// ������ �۾��� ��� ���θ� �������ų� �����մϴ�.
		/// </summary>
		public bool Cancel
		{
			get { return m_Cancel; }
			set { m_Cancel = value; }
		}
	}
	#endregion //�۾� ������ Ŭ���� --------------------------------------------------------

	#region ������Ǯ �۾������� ť Ŭ���� �Դϴ� -------------------------------------------
//	/// <summary>
//	/// ������ Ǯ �۾��������� �����ϱ� ���� Queue�Դϴ�.
//	/// </summary>
//	public class TMSWorkItemQueue
//	{	
//		/// <summary>
//		/// ������ Ǯ �۾� ������(TMSWorkItem)�� ������ Queue�Դϴ�.
//		/// </summary>
//		private Queue m_WorkItems;  
//
//		/// <summary>
//		/// �۾� ������ ť Ŭ������ �⺻ ������ �Դϴ�.
//		/// </summary>
//		public TMSWorkItemQueue()
//		{
//			m_WorkItems = new Queue(); 
//		}
//
//		/// <summary>
//		/// �۾� �������� ť�� �ֽ��ϴ�.
//		/// </summary>
//		/// <param name="vWorkItem">������ Ǯ �۾� ������ �Դϴ�.</param>
//		public void EnqueueWorkItem(TMSWorkItem vWorkItem)
//		{			
//			if(vWorkItem == null)
//			{				
//				throw new ArgumentNullException("EnqueueWorkItem", "WrokItem�� Null�� �� �����ϴ�.");
//			}
//
//			lock (this)
//			{
//				m_WorkItems.Enqueue(vWorkItem);
//			}
//		}
//
//		/// <summary>
//		/// ť���� �۾� �������� �����ϴ�.
//		/// </summary>
//		/// <returns>ť���� ���� �۾� ������ �Դϴ�.</returns>
//		public TMSWorkItem DequeueWorkItem()
//		{
//			TMSWorkItem tWorkItem;
// 
//			lock (this)
//			{
//				if(m_WorkItems.Count > 0)
//				{
//					tWorkItem = m_WorkItems.Dequeue() as TMSWorkItem;
//					return tWorkItem;
//				}
//			}
//			return null;
//		}
//
//		/// <summary>
//		/// ť���� ����ϰ� �ִ� �۾� �������� ������ ��ȯ�մϴ�.
//		/// </summary>
//		public int Count
//		{
//			get
//			{
//				lock (this)
//				{
//					return m_WorkItems.Count;
//				}
//			}
//		}
//
//		/// <summary>
//		/// ť�� ����ϰ� �ִ� ��� �������� �����մϴ�.
//		/// </summary>
//		public void Clear()
//		{
//			lock(this)
//			{
//				m_WorkItems.Clear();
//			}
//		}
//	}
	#endregion //������Ǯ �۾������� ť Ŭ���� �Դϴ� -------------------------------------------
}