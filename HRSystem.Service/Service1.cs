using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using HRSystem.Composition;
using HRSystem.Core;
using HRSystem.Data;
using Microsoft.EntityFrameworkCore;
using OneInc.ADEditor.ActiveDirectory;

namespace HRSystem.Service
{
    public partial class Service1 : ServiceBase
    {
        private DateTime _lastSyncedTime = new DateTime(2010, 01, 01);
        private readonly IEmployeeService _employeeService;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public Service1()
        {
            InitializeComponent();
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CompositionModule());
            builder.Register(a => GetActiveDirectorySettings()).AsSelf().SingleInstance();
            builder.RegisterType<HrSystemDb>().AsSelf();
            var dbContextOptionsBuilder = new DbContextOptionsBuilder();
            var dbContextOptions = dbContextOptionsBuilder.UseSqlServer(
                "Data Source=WS-NSK-97;Initial Catalog=HRSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
                .Options;
            builder.Register(a => dbContextOptions).As<DbContextOptions>();
            Mapper.Initialize(
                cfg =>
                {
                    cfg.AddProfile<AutomapperProfile>();
                });
            Mapper.AssertConfigurationIsValid();

            var container = builder.Build();
            _employeeService = container.Resolve<IEmployeeService>();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void TestStart()
        {
            OnStart(new [] { "test" });
        }

        protected override void OnStart(string[] args)
        {
            if (args.FirstOrDefault() == "test")
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    Sync();
                    Thread.Sleep(10000);
                }
            }
            else
            {
                RecurrentExecutor.Run(Sync, new TimeSpan(0, 0, 15),
                    _cancellationTokenSource.Token);
            }
        }

        private void Sync()
        {
            try
            {
                _employeeService.SyncWithActiveDirectory(_lastSyncedTime).Wait();
                _lastSyncedTime = DateTime.UtcNow;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected override void OnStop()
        {
            _cancellationTokenSource.Cancel();
        }

        private ActiveDirectorySettings GetActiveDirectorySettings()
        {
            if (!(ConfigurationManager.GetSection("ActiveDirectorySettings") is NameValueCollection section))
            {
                throw new ArgumentNullException(nameof(section));
            }
            
            return new ActiveDirectorySettings
            {
                Domain = section.Get("domain"),
                Login = section.Get("login"),
                Password = section.Get("password"),
                Paths = new Dictionary<string, string> {["User"] = section.Get("userPath"), ["Office"] = section.Get("officePath")},
                ProtocolVersion = int.Parse(section.Get("protocolVersion")),
                SecureSocketLayer = bool.Parse(section.Get("secureSocketLayer")),
                Server = section.Get("server"),
                TechincalUserAuthenticationMode = (TechincalUserAuthenticationMode)Enum.Parse(typeof(TechincalUserAuthenticationMode), section.Get("techincalUserAuthenticationMode")),
                Timeout = TimeSpan.Parse(section.Get("timeout")),
                UserCreationPathPrefix = section.Get("UserCreationPathPrefix")
            };
        }
    }
}