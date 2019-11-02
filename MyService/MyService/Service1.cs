using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MyService
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer;
        private ServiceWork service;
        public Service1()
        {
            InitializeComponent();
            service = new ServiceWork();
            service.GetInfo();

            timer = new Timer(30*60000D); // 30 minutes
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(Func);
        }

        protected override void OnStart(string[] args)
        {
            timer.Start();
        }

        public void Func(object sender, ElapsedEventArgs e)
        {
            service.GetInfo();
        }
        protected override void OnStop()
        {
            timer.Stop();
            timer = null;
        }
    }
}
