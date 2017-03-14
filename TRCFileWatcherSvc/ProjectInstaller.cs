using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using TRCUtilities;

namespace TRCFileWatcherSvc {

  [RunInstaller(true)]
  public partial class ProjectInstaller : Installer {

    public ProjectInstaller() {
      InitializeComponent();
    }

    private void serviceInstallerTRCFileWatcher_AfterInstall(object sender, InstallEventArgs e) {
      try {
        var sc = new ServiceController {ServiceName = serviceInstallerTRCFileWatcher.ServiceName};
        if (sc.Status != ServiceControllerStatus.Stopped) return;
        sc.Start();
        sc.WaitForStatus(ServiceControllerStatus.Running);
      }
      catch (InstallException iex) {
        ExceptionHandler.LogError(TRCTariffPDFUtil.PDFUtil.eventSource, iex);
      }
      catch (System.Exception ex) {
        ExceptionHandler.LogError(TRCTariffPDFUtil.PDFUtil.eventSource, ex);
      }
    }
  }
}