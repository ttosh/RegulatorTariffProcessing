
using System;
using System.ServiceProcess;

namespace TRCFileWatcherSvc
{

  public static class Program
    {

    /// <summary>
    /// The main entry point for the file watcher service.
    /// </summary>
    /// <exception cref="ArgumentException">You did not supply a service to start. The array might be null or empty. </exception>
    public static void Main() {
            var ServicesToRun = new ServiceBase[] 
            { 
                new TRCFileWatcherSvc(), 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
