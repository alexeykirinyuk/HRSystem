using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using Autofac;
using AutoMapper;
using HRSystem.Composition;
using HRSystem.Core;
using HRSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using OneInc.ADEditor.ActiveDirectory;
using OneInc.ADEditor.Dal.Mapping;

namespace HRSystem.Service
{
    public partial class Service1 : ServiceBase
    {
        private DateTime _lastSyncedTime = new DateTime(2010, 01, 01);
        private IEmployeeService _employeeService;
        private CancellationTokenSource _cancellationTokenSource;

        public Service1()
        {
            InitializeComponent();
        }

        private void Initialize()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CompositionModule());
            builder.RegisterInstance(GetActiveDirectorySettings()).AsSelf().SingleInstance();
            builder.RegisterType<HrSystemDb>().AsSelf();
            var connectionString =
                ((NameValueCollection) ConfigurationManager.GetSection("DataBase")).Get("connectionString");
            var options = new DbContextOptionsBuilder<HrSystemDb>()
                .UseSqlServer(connectionString).Options;
            
            builder.RegisterInstance(options).As<DbContextOptions>();
            Mapper.Initialize(
                cfg =>
                {
                    cfg.AddProfile<AutomapperProfile>();
                    cfg.AddProfile<ActiveDirectoryAutomapperProfile>();
                });
            Mapper.AssertConfigurationIsValid();

            var container = builder.Build();
            _employeeService = container.Resolve<IEmployeeService>();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void TestStart()
        {
            OnStart(new[] {"test"});
        }

        protected override void OnStart(string[] args)
        {
            Initialize();

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
                Paths = new Dictionary<string, string>
                {
                    ["User"] = section.Get("userPath"),
                    ["Office"] = section.Get("officePath")
                },
                ProtocolVersion = int.Parse(section.Get("protocolVersion")),
                SaslMethod = section.Get("saslMethod"),
                Server = section.Get("server"),
                TechincalUserAuthenticationMode = (TechincalUserAuthenticationMode) Enum.Parse(
                    typeof(TechincalUserAuthenticationMode), section.Get("techincalUserAuthenticationMode")),
                Timeout = TimeSpan.Parse(section.Get("timeout")),
                UserCreationPathPrefix = section.Get("UserCreationPathPrefix")
            };
        }
    }
}