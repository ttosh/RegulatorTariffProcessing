using System;
using System.Data;
using System.Text;

using TRCAttributes;
using TRCDataAccess;
using TRCUtilities;

// namespaces...
namespace TRCBusiness {
  // public classes...
  [Author("Tom Kraft", version = 1.0)]
  [Author("Timothy Tosh", version = 1.1)]
  public class ObjectTAR3 {
    // public fields...
    public static string report_type, tsp_no, start_dte, end_dte;

    // private properties...
    private static DataTable AuthorizingAgent {
      get {
        return DAL.AUTHORIZING_AGENT(report_type, tsp_no);
      }
    }
    private static DataTable DeliveryPoint {
      get {
        return DAL.TAR3_DELIVERYPOINT(tsp_no, start_dte, end_dte);
      }
    }

    // private methods...
    private static DataTable FileTerminator(int fileCount) {
      return DAL.FILE_TERMINATOR(fileCount);
    }

    // public methods...
    public static void CreateFileFromData() {
      var allFileLines = new DataTable();
      allFileLines.Columns.Add("fileline", Type.GetType("System.String"));

      Common.CompileFileLines(AuthorizingAgent, ref allFileLines);
      Common.CompileFileLines(DeliveryPoint, ref allFileLines);
      var fileCount = DeliveryPoint.Rows.Count;
      Common.CompileFileLines(FileTerminator(fileCount), ref allFileLines);

      var filePath = new StringBuilder();
      filePath.Append(ConfigUtil.TargetDirectory);
      filePath.Append("TAR3_");
      filePath.Append(tsp_no);
      filePath.Append("_");
      if (DateTime.Parse(start_dte).ToString("MMMM").Equals(DateTime.Parse(end_dte).ToString("MMMM"))) {
        filePath.Append(DateTime.Parse(end_dte).ToString("MMMM"));
        filePath.Append("_");
        filePath.Append(DateTime.Parse(end_dte).ToString("yyyy"));
      } else {
        filePath.Append(DateTime.Parse(start_dte).ToString("MMMM"));
        filePath.Append("_");
        filePath.Append(DateTime.Parse(end_dte).ToString("MMMM"));
        filePath.Append("_");
        filePath.Append(DateTime.Parse(end_dte).ToString("yyyy"));
      }
      filePath.Append(".txt");

      FileIO.WriteFile(allFileLines, filePath.ToString());
    }
  }
}
