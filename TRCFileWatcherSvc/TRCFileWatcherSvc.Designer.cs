namespace TRCFileWatcherSvc
{
    partial class TRCFileWatcherSvc
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
      this.TRCTariffFileSystemWatcher = new System.IO.FileSystemWatcher();
      ((System.ComponentModel.ISupportInitialize)(this.TRCTariffFileSystemWatcher)).BeginInit();
      // 
      // TRCTariffFileSystemWatcher
      // 
      this.TRCTariffFileSystemWatcher.EnableRaisingEvents = true;
      this.TRCTariffFileSystemWatcher.NotifyFilter = ((System.IO.NotifyFilters)((((((((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName) 
            | System.IO.NotifyFilters.Attributes) 
            | System.IO.NotifyFilters.Size) 
            | System.IO.NotifyFilters.LastWrite) 
            | System.IO.NotifyFilters.LastAccess) 
            | System.IO.NotifyFilters.CreationTime) 
            | System.IO.NotifyFilters.Security)));
      // 
      // TRCFileWatcherSvc
      // 
      this.ServiceName = "TRCFileWatcherSvc";
      ((System.ComponentModel.ISupportInitialize)(this.TRCTariffFileSystemWatcher)).EndInit();

        }
        #endregion

        private System.IO.FileSystemWatcher TRCTariffFileSystemWatcher;
    }
}
