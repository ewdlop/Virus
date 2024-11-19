using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace WindowsService1
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            string parameter = "MySource1\" \"MyLogFile1";
            Context.Parameters["assemblypath"] = $"\"{Context.Parameters["assemblypath"]}\" \"{parameter}\"";
            base.OnBeforeInstall(savedState);
        }
    }
}
