﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Versioning;
using System.Diagnostics;

using Livet;

namespace SRNicoNico.Models.NicoNicoViewer {
    public class BPSCounter :  NotificationObject {
     
        //通信速度を取得する
        public static float Bps { get; private set; }

        //速度取得開始
        public static void InitAndStart() {


            Task.Run(() => {

                PerformanceCounter counter = new PerformanceCounter();
                counter.CategoryName = ".NET CLR Networking 4.0.0.0";
                counter.CounterName = "Bytes Received";

                counter.InstanceName = VersioningHelper.MakeVersionSafeName("SRNicoNico.exe", ResourceScope.Machine, ResourceScope.AppDomain);
                counter.ReadOnly = true;
                string str = VersioningHelper.MakeVersionSafeName("SRNicoNico.exe", ResourceScope.Machine, ResourceScope.AppDomain);


                float f1 = 0, f2 = 0;
                ;
                for (;;) {


                    f2 = counter.NextValue();
                    float difReceived = f2 - f1;
                    f1 = f2;


                    Console.WriteLine("受信:" + difReceived + "KB/s");
                    Thread.Sleep(1000);
                }


            });

        }



    }
}
