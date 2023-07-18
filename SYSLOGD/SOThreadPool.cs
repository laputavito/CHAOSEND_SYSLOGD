using System;
using System.Threading;
using System.Collections;
using System.Diagnostics;

namespace SECUONELibrary
{
	#region 스레드풀 이벤트 어규먼트 입니다 ------------------------------------------------
	/// <summary>
	/// 스래드 풀에서 발생한 이벤트에 들어갈 어규먼트 클래스 입니다.
	/// </summary>
	public class TMSWorkItemEventArgs : EventArgs
	{
		/// <summary>
		/// 작업 아이템입니다.
		/// </summary>
		private TMSWorkItem m_WorkItem;

		/// <summary>
		/// 작업아이템 이벤트 어규먼트 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vWorkItem">작업 아이템 입니다.</param>
		public TMSWorkItemEventArgs(TMSWorkItem vWorkItem)
		{
			m_WorkItem = vWorkItem;
		}

		/// <summary>
		/// 작업 아이템을 반환합니다.
		/// </summary>
		public TMSWorkItem WorkItem
		{
			get
			{
				return m_WorkItem;
			}
		}
	}
	#endregion //스레드풀 이벤트 어규먼트 입니다 ------------------------------------------------
	
	#region 스레드 풀 클래스 입니다 --------------------------------------------------------
	/// <summary>
	/// 동시에 여러개의 스래드를 관리하고 실행할 수 있습니다. 
	/// </summary>
	public class SOThreadPool : IDisposable
	{
		/// <summary>
		/// [기본] 동시에 작업할 스래드의 최개 개수입니다.
		/// </summary>
		private const int c_DefaultMaxWorkThreads = 25;					
		/// <summary>
		/// 동시에 작업할 스래드의 최개 개수입니다.
		/// </summary>
		private int m_MaxWorkThreads;		
		/// <summary>
		/// 작업아이템을 관리할 Queue입니다.
		/// </summary>
		private Queue m_WorkItemQueue;			
//		/// <summary>
//		///  스래드가 작업아이템을 기다리는 Waiter를 관리할 Queue입니다.
//		/// </summary>
//		private Queue m_WaitQueue;
		/// <summary>
		/// 작업 스래드 저장소 입니다.
		/// </summary>
		private Hashtable m_WorkThreads;
		/// <summary>
		/// 스래드 풀이 중지 되었는지의 여부입니다.
		/// </summary>
		private bool m_isShutDown;
		/// <summary>
		/// 현재 실행중인 작업의 개수를 나타냅니다.
		/// </summary>
		private int m_RunningWorkCount;
		
		/// <summary>
		/// 작업 아이템 하나의 작업이끝났을때 발생하는 작업완료 이벤트에 필요한 이벤트 핸들러입니다.
		/// </summary>
		public delegate void WorkCompletedEventHandler(object sender, TMSWorkItemEventArgs e);
		/// <summary>
		/// 작업 아이템의 작업이 끝나면 발생하는 이벤트 입니다.
		/// </summary>
		public event WorkCompletedEventHandler WorkCompleted;
		/// <summary>
		/// 스래드 풀이 종료 되었음을 알리는 이벤트 입니다.
		/// </summary>
		public event EventHandler ThreadPoolStoped;
		
		/// <summary>
		/// 스래드풀 클래스의 기본 생성자 입니다.
		/// </summary>
		public SOThreadPool() : this(c_DefaultMaxWorkThreads) {}

		/// <summary>
		/// 스래드 풀 클래스의 생성자 입니다.
		/// </summary>
		/// <param name="vMaxWorkThreads">동시에 작업할 최대 스래드 개수입니다.</param>
		public SOThreadPool(int vMaxWorkThreads)			
		{
			if(vMaxWorkThreads < 1)
			{
				throw new ArgumentOutOfRangeException("CTMSThreadPool", "vMaxWorkThreads가 1보다 작을 수 없습니다");
			}

			m_RunningWorkCount = 0;
			m_MaxWorkThreads = vMaxWorkThreads;
			Initialize();
		}	

		/// <summary>
		/// 스래드 풀을 중지합니다.
		/// </summary>
		public void Dispose()
		{
			StopThreadPool();

			m_WorkItemQueue = null;
			//m_WaitQueue = null;
			m_WorkThreads = null;
		}

		/// <summary>
		/// 스래드 풀을 초기화합니다
		/// </summary>
		private void Initialize()
		{
			m_isShutDown = true;
			
			//작업 큐를 초기화 한다
			m_WorkItemQueue = new Queue();
			//m_WaitQueue = new Queue();
			m_WorkThreads = new Hashtable();
		}

		/// <summary>
		/// 스레드 풀의 최대크기를 설정하거나 가져옵니다.
		/// </summary>
		public int MaxWorkThread
		{
			get { return m_MaxWorkThreads; }
			set 
			{ 
				if(value < 1) 
				{
					throw new ArgumentOutOfRangeException("MaxWorkThread", "value가 1보다 작을 수 없습니다");
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
		/// 스래드풀 작업 아이템을 처리합니다.
		/// </summary>
		private void ProcessWorkItem()
		{
			TMSWorkItem tWorkItem = null;
			bool tisRun = false;
			//ManualResetEvent tmre = null;

			try
			{
				//작업아이템을 실행합니다.
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
								//작업 아이템을 실행 한다
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
		/// 스래드 풀에 작업 아이템을 추가 합니다.
		/// </summary>
		/// <param name="vWorkItem">스래드 풀의 작업 아이템입니다</param>
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
		/// 스래드 풀을 시작합니다.
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
		/// 지정한 개수만큼 스래드를 시작합니다.
		/// </summary>
		/// <param name="vCount">시작 할 스래드의 개수 입니다.</param>
		private void RunWorkThread(int vCount)
		{
			//ManualResetEvent tmre;
			
			for(int i = 0; i < vCount; i++)
			{
				try
				{
					Thread tWorkThread = new Thread(new ThreadStart(ProcessWorkItem));
					tWorkThread.Priority = ThreadPriority.Highest;
				
//					//작업아이테이 없을때 스래드를 대기시키기위한 ManualResetEvent를 생성한다
//					tmre = new ManualResetEvent(false);
				
					//스래드를 해시 테이블에 등록한다
					lock(m_WorkThreads.SyncRoot)
					{
						m_WorkThreads.Add(tWorkThread, tWorkThread); 
					}				
					tWorkThread.IsBackground = true;
				
					//스래드를 시작한다
					tWorkThread.Start();
				}
				catch {}
			}
		}

		/// <summary>
		/// 스래드 풀을 중지합니다.
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
			
				//스래드 중지됨 이벤트 발생
				if(ThreadPoolStoped != null) ThreadPoolStoped(this, EventArgs.Empty);
			}
			catch(Exception ex1)
			{
				Console.WriteLine(ex1.ToString());
			}
		}

		/// <summary>
		/// 현재 Queue에 대기중인 작업 아이템의 개수를 반환합니다.
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
		/// 현재 실행중인 작업의 개수를 반환 합니다.
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
	#endregion //스레드 풀 클래스 입니다 --------------------------------------------------------

	#region 작업 아이템 클래스 -------------------------------------------------------------
	/// <summary>
	/// 스레드 작업아이템 델리게이트 입니다.
	/// </summary>
	public delegate object WorkItemMethod(object arg);
	
	//스래드 작업 아이템
	/// <summary>
	/// 스래드 풀에 작업을 요청하기위한 작업 아이템입니다.
	/// </summary>
	public class TMSWorkItem
	{		
		/// <summary>
		/// 스래드 작업을 수행할 Method 입니다.
		/// </summary>
		private WorkItemMethod m_Method = null;
		/// <summary>
		/// 스래드 작업 Method의 파라메터 배열 입니다.
		/// </summary>
		private object m_Argument = null;
		/// <summary>
		/// 스래드 작업 Method가 반환하는 반환값입니다.
		/// </summary>
		private object m_Return = null;
		/// <summary>
		/// 작업을 실행하지 않음
		/// </summary>
		private bool m_Cancel = false;

		/// <summary>
		/// 작업 아이템 클래스의 기본생성자 입니다.
		/// </summary>
		public TMSWorkItem() {}

		/// <summary>
		/// 작업 아이템 클래스의 생성자입니다.
		/// </summary>
		/// <param name="vMethod">스래드 작업을 수행할 Method 입니다.</param>
		/// <param name="vArgument">스래드 작업 Method에 필요한 파라메터 배열 입니다.</param>
		public TMSWorkItem(WorkItemMethod vMethod, object vArgument)
		{
			m_Method = vMethod;
			m_Argument = vArgument;
		}
		
		/// <summary>
		/// 스래드 작업을 실행할 Method를 가져오거나 설정합니다.
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
		/// 스래드 작업 Method의 파라메터 목록 가져오거나 설정합니다.
		/// </summary>
		public object Argument
		{
			get { return m_Argument; }
			set { m_Argument = value; }
		}

		/// <summary>
		/// 스래드 작업 Method의 반환값을 가져옵니다.
		/// </summary>
		public object Return
		{
			get { return m_Return; }
		}

		/// <summary>
		/// 스래드 작업 메소드를 실행 합니다.
		/// </summary>
		public void Execute()
		{
			m_Return = m_Method(m_Argument);
		}

		/// <summary>
		/// 스래드 작업의 취소 여부를 가져오거나 설정합니다.
		/// </summary>
		public bool Cancel
		{
			get { return m_Cancel; }
			set { m_Cancel = value; }
		}
	}
	#endregion //작업 아이템 클래스 --------------------------------------------------------

	#region 스레드풀 작업아이템 큐 클래스 입니다 -------------------------------------------
//	/// <summary>
//	/// 스래드 풀 작업아이템을 관리하기 위한 Queue입니다.
//	/// </summary>
//	public class TMSWorkItemQueue
//	{	
//		/// <summary>
//		/// 스래드 풀 작업 아이템(TMSWorkItem)을 저장할 Queue입니다.
//		/// </summary>
//		private Queue m_WorkItems;  
//
//		/// <summary>
//		/// 작업 아이템 큐 클래스의 기본 생성자 입니다.
//		/// </summary>
//		public TMSWorkItemQueue()
//		{
//			m_WorkItems = new Queue(); 
//		}
//
//		/// <summary>
//		/// 작업 아이템을 큐에 넣습니다.
//		/// </summary>
//		/// <param name="vWorkItem">스래드 풀 작업 아이템 입니다.</param>
//		public void EnqueueWorkItem(TMSWorkItem vWorkItem)
//		{			
//			if(vWorkItem == null)
//			{				
//				throw new ArgumentNullException("EnqueueWorkItem", "WrokItem이 Null일 수 없습니다.");
//			}
//
//			lock (this)
//			{
//				m_WorkItems.Enqueue(vWorkItem);
//			}
//		}
//
//		/// <summary>
//		/// 큐에서 작업 아이템을 꺼냅니다.
//		/// </summary>
//		/// <returns>큐에서 꺼낸 작업 아이템 입니다.</returns>
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
//		/// 큐에서 대기하고 있는 작업 아이템의 개수를 반환합니다.
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
//		/// 큐에 대기하고 있는 모든 아이템을 삭제합니다.
//		/// </summary>
//		public void Clear()
//		{
//			lock(this)
//			{
//				m_WorkItems.Clear();
//			}
//		}
//	}
	#endregion //스레드풀 작업아이템 큐 클래스 입니다 -------------------------------------------
}