using System;
using System.Threading;
using ThreadDemoMac;

namespace ThreadDemo_Mac
{
    class Program
    {
        static void Main(string[] args)
        {
           
            string str = "0";
            ShowTip();
            for (str = Console.ReadLine(); str != "0";str = Console.ReadLine())
            {
                switch (str)
                {
                    case "1":
                        ThreadHelper.GetThreadInfo();
                        break;
                    case "2":
                        ThreadHelper.ValueNonSynchronization();
                        break;
                    case "3":
                        ThreadHelper.ValueSynchronization();
                        break;
                    case "4":
                        ThreadHelper.ThreadPoolQueue("小米");
                        ThreadHelper.ThreadPoolQueue(null);
                        ThreadHelper.ThreadPoolQueue("");
                        break;
                    case "5":
                        ThreadHelper.DelegateMethod();
                        break;
                    case "6":
                        ThreadHelper.TaskMethod();
                        break;
                    case "7":
                        ThreadHelper.ParallelMethod();
                        break;
                    case "8":
                        ThreadHelper.EventWaitHandleMethod();
                        break;
                    default:
                        Console.WriteLine("请输入有效值");
                        break;
                }
                ShowTip();
            }
           
        }
        /// <summary>
        /// Shows the tip.
        /// </summary>
        private static void ShowTip()
        {
            Console.WriteLine("请选择执行内容：");
            Console.WriteLine("0:退出；");
            Console.WriteLine("1:显示线程详情；");
            Console.WriteLine("2:处理多线程未对公用的变量同步；");
            Console.WriteLine("3:处理多线程对公用的变量同步；");
            Console.WriteLine("4:线程池的应用；");
            Console.WriteLine("5:委托的应用；");
            Console.WriteLine("6:Task的简单使用；");
            Console.WriteLine("7:Parallel的简单应用；");
            Console.WriteLine("8:EventWaitHandle的简单应用；");
        }
    }
}
