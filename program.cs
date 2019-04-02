//Program.cs
using System;
using System.Threading;
using HidLibrary;
using Scale;
using System.Timers;

namespace ScaleReader
{
    class Program
    {
        private static System.Timers.Timer aTimer;

        public static void Main(string[] args)
        {
            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.Enabled = true;

            Console.WriteLine("Let the scale do it's thing...");
            Console.ReadLine();
            
        }

        static void OnTimedEvent(object sender, System.EventArgs e)
        {
            decimal? weightInLb, weightInG, weightInOz;
            bool? isStable;

            USBScale s = new USBScale();
            s.Connect();

            if (s.IsConnected)
            {
                s.GetWeight(out weightInLb, out weightInG, out weightInOz, out isStable);
                s.DebugScaleData();
                Console.WriteLine("Weight: {0:0.00} g", weightInG);
                Console.WriteLine("Weight: {0:0.00} LBS", weightInLb);
                Console.WriteLine("Weight: {0:0.00} oz", weightInOz);
                Console.WriteLine("--------------------- {0}", DateTime.Now);
                s.Disconnect(); 
            }
            else
            {
                Console.WriteLine("No Scale Connected.");
            }
        }
    }
}