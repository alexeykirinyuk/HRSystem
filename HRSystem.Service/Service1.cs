using System.ServiceProcess;

namespace HRSystem.Service
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            
            
            RecurrentExecutor.Run(() => { });
        }

        protected override void OnStop()
        {
        }
    }
}
