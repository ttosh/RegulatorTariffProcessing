using System;
using System.Collections;
using System.Configuration;

using TRCAttributes;

namespace TRCUtilities {

  [Author("Tom Kraft", version = 1.0)]
  [Author("Timothy Tosh", version = 1.1)]
  public class ConfigUtil {

    public static readonly string TargetDirectory = ConfigurationManager.AppSettings["TargetDirectory"];
    public static readonly string TariffFileWatchPath = ConfigurationManager.AppSettings["TariffFileWatchPath"];
    public static readonly string TariffFileWatchArchivePath = ConfigurationManager.AppSettings["TariffFileWatchArchivePath"];
    public static readonly string LogDirectory = ConfigurationManager.AppSettings["logDirectory"];

    //Database connection string
    public static string connStr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=rsprod3.oneok.com)(PORT=1529))(CONNECT_DATA=(SERVICE_NAME=PQINTRA2)));User Id=QPTM_READONLY;Password=readonly;";
    //public static string connStr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=rstest12.oneok.com)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=QQINTRA2)));User Id=QPTM_ONK;Password=pieinsky;";
    //public static string connStr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=rstest12.oneok.com)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=TQINTRA2)));User Id=QPTM_ONK;Password=pieinsky;";
    //public static string connStr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=rstest12.oneok.com)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=DQINTRA2)));User Id=QPTM_ONK;Password=pieinsky;";
    //public static string connStr = EncryptionUtil.Decrypt(ConnectionStringEncrypted);

    private static string ConnectionStringEncrypted {
      get {
        var settings = ConfigurationManager.ConnectionStrings["QPTM_INTRA"];
        return settings.ConnectionString;
      }
    }

    /// <summary>
    /// Retrieves the stored procedure name from the 'storedProcedures' section of the config file for the given key.
    /// </summary>
    /// <param name="key">The key name of the desired entry in the 'storedProcedures' section of the config file.</param>
    /// <returns>The full stored procedure name from the config file.</returns>
    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is null. </exception>
    /// <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> 
    /// is read-only.-or- The property is set, <paramref name="key" /> does not exist in the collection, and the 
    /// <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
    public static string GetStoredProcedureName(string key) {
      var storedProcedureName = string.Empty;
      var storedProceduresSection = ConfigurationManager.GetSection("storedProcedures") as Hashtable;
      if (storedProceduresSection != null)
        storedProcedureName = storedProceduresSection[key] as string;

      return storedProcedureName;
    }
  }
}