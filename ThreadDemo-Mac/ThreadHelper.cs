﻿using System;
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
            Thread thread2 = new Thread(new ThreadStart(bookShop.SaleBooks));
            thread1.Start();
            thread2.Start();
        }

        /// <summary>
        /// 多线程访问，实现对某个变量的同步处理
        /// </summary>
        public static void ValueSynchronization()
        {
            BookShop bookShop = new BookShop();
            Thread thread1 = new Thread(new ThreadStart(bookShop.SynchronizeSaleBooks));
            Thread thread2 = new Thread(new ThreadStart(bookShop.SynchronizeSaleBooks));
            thread1.Start();
            thread2.Start();
        }

        /// <summary>
        /// Book shop.
        /// </summary>
        class BookShop
        {
            public int num = 1;
            public void SaleBooks()
            {
                int tmp = num;
                if (tmp > 0)
                {
                    Thread.Sleep(1000);
                    num--;
                    Console.WriteLine("售出1本书，剩余{0}本", num);

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
                        Console.WriteLine("售出1本书，剩余{0}本", num);

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
            if (param == null)
            {
                param = "无名氏";
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback(RunWorkerMethod), param);

        }

        private static void RunWorkerMethod(Object name)
        {
            if (name == null)
            {
                name = "无名氏";
            }
            Console.WriteLine("执行了线程池{0}方法！", name);
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
