using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.ServiceProcess;

using TRCTariffPDFUtil;
using TRCUtilities;

// namespaces...
namespace TRCFileWatcherSvc {
  // public classes...
  /// <summary>
  /// TRC Phase II File Watcher class for PDF Files
  /// </summary>
  public partial class TRCFileWatcherSvc : ServiceBase {
    
    // public constructors...
    public TRCFileWatcherSvc() {
      InitializeComponent();
    }

    // private methods...
    /// <summary>
    /// Event hander for when a file is added, modified or deleted
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="fsea"></param>
    private static void OnCreated(object sender, FileSystemEventArgs fsea) {
      var pdfUtil = new PDFUtil(fsea.FullPath);
      var pdfTask = new BackgroundWorker();
      pdfTask.DoWork += pdfTask_DoWork;
      pdfTask.RunWorkerCompleted += pdfTask_RunWorkerCompleted;
      pdfTask.RunWorkerAsync(pdfUtil);
    }
    private static void pdfTask_DoWork(object sender, DoWorkEventArgs dwea) {
      ((PDFUtil)dwea.Argument).ProcessTRCTariffPDFFile();
    }

    private static void pdfTask_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs rwcea) {
      if (rwcea.Error != null) {
        ExceptionHandler.LogError(PDFUtil.eventSource, rwcea.Error);
      }
    }
    /// <summary>
    /// Inits all properties for the file watcher when the service starts
    /// </summary>
    private void SetStartPropertiesForFileWatcher() {
      TRCTariffFileSystemWatcher.Path = ConfigurationManager.AppSettings["TariffFileWatchPath"];
      TRCTariffFileSystemWatcher.Filter = "*.pdf";
      TRCTariffFileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess
                                                | NotifyFilters.LastWrite
                                                | NotifyFilters.FileName
                                                | NotifyFilters.DirectoryName;

      TRCTariffFileSystemWatcher.Created += OnCreated;
      TRCTariffFileSystemWatcher.EnableRaisingEvents = true;
    }
    /// <summary>
    /// Stops all properties forthe file watcher when the service stops
    /// </summary>
    private void SetStopPropertiesForFileWatcher() {
      TRCTariffFileSystemWatcher.Created -= OnCreated;
      TRCTariffFileSystemWatcher.EnableRaisingEvents = false;
      TRCTariffFileSystemWatcher.Dispose();
    }

    // protected methods...
    /// <summary>
    /// Entry point to the windows service
    /// </summary>
    /// <param name="args"></param>
    protected override void OnStart(string[] args) {
      SetStartPropertiesForFileWatcher();
    }
    /// <summary>
    /// Exit point for the windows service
    /// </summary>
    protected override void OnStop() {
      SetStopPropertiesForFileWatcher();
    }
  }
}
