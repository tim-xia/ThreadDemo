using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadDemoMac
{

    public class ThreadHelper
    {
        static void HandleMyDelegate(string name, string doSome)
        {
        }


        #region 获取当前线程的基本信息
        /// <summary>
        /// 获取当前线程的基本信息
        /// </summary>
        public static void GetThreadInfo()
        {
            ///获取线程；获取线程相关属性并输出
            Thread thread = Thread.CurrentThread;
            if (string.IsNullOrEmpty(thread.Name))
            {
                thread.Name = "TimMarkThread";
            }
            string threadInfo = String.Format("Thread Name:{0}\nThreadId:{1}\nPriority:{2}\nThreadState:{3}\n" +
                                              "DomainId:{4}\n",
                                              thread.Name, thread.ManagedThreadId,
                                              thread.Priority, thread.ThreadState, Thread.GetDomainID());
            Console.WriteLine(threadInfo);
        }
        #endregion

        #region 多线程应用：Thread方式
        /// <summary>
        /// 多线程访问，未对某个对象进行同步处理
        /// </summary>
        public static void ValueNonSynchronization()
        {
            BookShop bookShop = new BookShop();
            Thread thread1 = new Thread(new ThreadStart(bookShop.SaleBooks));
            Thread thread2 = new Thread(new ParameterizedThreadStart(bookShop.SaleBooks));
            thread1.Start();
            thread2.Start(2);
        }

        /// <summary>
        /// 多线程访问，实现对某个变量的同步处理
        /// </summary>
        public static void ValueSynchronization()
        {
            BookShop bookShop = new BookShop();
            Thread thread1 = new Thread(new ThreadStart(bookShop.SynchronizeSaleBooks));
            Thread thread2 = new Thread(new ParameterizedThreadStart(bookShop.SynchronizeSaleBooks));
            thread1.Start();
            thread2.Start(2);
        }

        /// <summary>
        /// Book shop.
        /// </summary>
        class BookShop
        {
            public int num = 3;
            /// <summary>
            /// 每次卖出一本书
            /// </summary>
            public void SaleBooks()
            {
                int tmp = num;
                if (tmp > 0)
                {
                    Thread.Sleep(1100);
                    num--;
                    tmp--;
                    Console.WriteLine("售出1本书，剩余{0}本", tmp);

                }
                else
                {
                    Console.WriteLine("书已售完");
                }
            }
            public void SaleBooks(object param)
            {
                var i = (int) param;
                int tmp = num;
                if (tmp > i)
                {
                    Thread.Sleep(1000);
                    num -= i;
                    tmp -= i;
                    Console.WriteLine("售出"+i+"本书，剩余{0}本", tmp);
                }
                else if (tmp < i && i > 0)
                {
                    Thread.Sleep(1000);
                    num=0;
                    tmp = 0;
                    Console.WriteLine("售出" + tmp + "本书，剩余{0}本", tmp);
                }
                else
                {
                    Console.WriteLine("书已售完");
                }
            }
            public void SynchronizeSaleBooks()
            {
                lock (this)
                {
                    int tmp = num;
                    if (tmp > 0)
                    {
                        Thread.Sleep(500);
                        num--;
                        tmp--;
                        Console.WriteLine("售出1本书，剩余{0}本", tmp);

                    }
                    else
                    {
                        Console.WriteLine("书已售完");
                    }
                }
            }
            public void SynchronizeSaleBooks(object param)
            {
                var i = (int) param;
                lock (this)
                {
                    int tmp = num;
                    if (tmp >= i)
                    {
                        Thread.Sleep(500);
                        num -= i;
                        tmp -= i;
                        Console.WriteLine("售出"+i+"本书，剩余{0}本", tmp);

                    }
                    else if (tmp < i && tmp > 0)
                    {
                        Thread.Sleep(500);
                        num = 0;
                        tmp = 0;
                        Console.WriteLine("售出" + i + "本书，剩余{0}本", tmp);
                    }
                    else
                    {
                        Console.WriteLine("书已售完");
                    }
                }
            }
        }
        #endregion

        #region 多线程应用：ThreadPool和委托
        public static void ThreadPoolQueue(Object param)
        {
            ThreadPool.QueueUserWorkItem(RunWorkerMethod, param);
        }

        private static void RunWorkerMethod(Object param)
        {
            string name = "";
            if (param!=null)
            {
                name = (string) param;
            }
            else
            {
                name = "无名氏";
            }
            Console.WriteLine("执行了线程池{0}方法--线程{1}！", name,Thread.CurrentThread.ManagedThreadId);
        }
        delegate string MyDelegate(string name, string doSome);
        public static void DelegateMethod()
        {
            MyDelegate myDelegate = new MyDelegate(HelpSomeOneTodoSomeThing);
            IAsyncResult result = myDelegate.BeginInvoke("老婆", "拿东西", null, null);
            Console.WriteLine("执行中");
            string data = myDelegate.EndInvoke(result);
            Console.WriteLine("结果：{0}", data);

        }

        private static string HelpSomeOneTodoSomeThing(string name, string someThing)
        {

            Console.WriteLine("我帮{0}做{1}", name, someThing);
            Thread.Sleep(1000);
            return "执行成功";
        }

        #endregion

        #region Task应用
        public static void TaskMethod()
        {
            var task = new Task(() => { Console.WriteLine("123"); });
            var task1 = task.ContinueWith<string>((test) => { Console.WriteLine("456"); return "9"; });
            task.Start();
            Console.WriteLine(task1.Result);
        }

        #endregion

        #region Parallel应用
        public static void ParallelMethod()
        {
            Parallel.For(0, 10, i =>
            {
                Console.Write("执行次数：{0},线程Id：{1}\n", i,
                              Thread.CurrentThread.ManagedThreadId);
            });
        }

        #endregion

    }
}
