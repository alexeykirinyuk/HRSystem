using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var service = new Service1();
            if (args.FirstOrDefault() == "test")
            {
                service.TestStart();
            }
            else
            {
                ServiceBase.Run(new ServiceBase[] {service});
            }
        }
    }
}