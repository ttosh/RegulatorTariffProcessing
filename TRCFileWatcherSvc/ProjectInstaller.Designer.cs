namespace TRCFileWatcherSvc
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.serviceProcessInstallerTRCFileWatcher = new System.ServiceProcess.ServiceProcessInstaller();
      this.serviceInstallerTRCFileWatcher = new System.ServiceProcess.ServiceInstaller();
      // 
      // serviceProcessInstallerTRCFileWatcher
      // 
      this.serviceProcessInstallerTRCFileWatcher.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
      this.serviceProcessInstallerTRCFileWatcher.Password = null;
      this.serviceProcessInstallerTRCFileWatcher.Username = null;
      // 
      // serviceInstallerTRCFileWatcher
      // 
      this.serviceInstallerTRCFileWatcher.Description = "Service to watch TRC PDF Files";
      this.serviceInstallerTRCFileWatcher.DisplayName = "TRC File Watcher Service";
      this.serviceInstallerTRCFileWatcher.ServiceName = "TRCFileWatcherSvc";
      this.serviceInstallerTRCFileWatcher.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
      this.serviceInstallerTRCFileWatcher.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstallerTRCFileWatcher_AfterInstall);
      // 
      // ProjectInstaller
      // 
      this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerTRCFileWatcher,
            this.serviceInstallerTRCFileWatcher});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerTRCFileWatcher;
        private System.ServiceProcess.ServiceInstaller serviceInstallerTRCFileWatcher;
    }
}