using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using TRCAttributes;
using TRCDataAccess;
using TRCUtilities;

// namespaces...
namespace TRCBusiness {
  // public classes...
  [Author("Timothy Tosh", version = 1.0)]
  public class ObjectTAR4 {

    // const fields...
    private const int RRC_COMPANY_ID_INDEX = 1;
    private const int CTR_NO_INDEX = 2;
    private const int DATE_INDEX = 3;
    private const int CONTRACT_TEXT_INDEX = 4;

    // private fields...
    private static Dictionary<string, IList<DataRow>> tar4DataEntriesGroupedByContract = new Dictionary<string, IList<DataRow>>();

    // public fields...
    public static string report_type, tsp_no, start_dte, end_dte;

    // private properties...
    private static DataTable AuthorizingAgent {
      get {
        return DAL.AUTHORIZING_AGENT(report_type, tsp_no);
      }
    }
    private static DataTable DataPoint {
      get {
        return DAL.TAR4_DATAPOINT(tsp_no, start_dte, end_dte);
      }
    }

    // public methods...
    public static void CreateFileFromData() {
      var allFileLines = new DataTable();
      allFileLines.Columns.Add("fileline", Type.GetType("System.String")); // Not L10N

      var dataPointArray = DataPoint.Select();
      foreach (var row in dataPointArray) {
        var currentContractNumber = row[2].ToString();
        if (!tar4DataEntriesGroupedByContract.ContainsKey(currentContractNumber)) {
          tar4DataEntriesGroupedByContract.Add(currentContractNumber, new List<DataRow>());
        }
        tar4DataEntriesGroupedByContract[currentContractNumber].Add(row);
      }

      var currentContractList = new List<DataRow>();
      foreach (var tar4ListByContract in tar4DataEntriesGroupedByContract) {
        Common.CompileFileLines(AuthorizingAgent, ref allFileLines);

        var sb = new StringBuilder();
        currentContractList = (List<DataRow>)tar4DataEntriesGroupedByContract[tar4ListByContract.Key];
        sb.Append(string.Format("A1|{0}|{1}|{2}|", currentContractList[0][RRC_COMPANY_ID_INDEX], currentContractList[0][CTR_NO_INDEX],  currentContractList[0][DATE_INDEX])); // Not L10N

        var listCountTracker = 0;
        var duplicatesBucket = new List<string>();
        foreach (var datapoint in currentContractList) {
          if (!duplicatesBucket.Contains(datapoint[CONTRACT_TEXT_INDEX].ToString()) && !datapoint[CONTRACT_TEXT_INDEX].ToString().Contains("DO NOT USE")) { // Not L10N
            sb.Append(datapoint[CONTRACT_TEXT_INDEX].ToString());
            if (listCountTracker++ != currentContractList.Count - 1) { sb.Append("; "); }
            duplicatesBucket.Add(datapoint[CONTRACT_TEXT_INDEX].ToString());
          } else if (listCountTracker++ == currentContractList.Count - 1) { sb.Remove(sb.Length - 2, 2); } 
        }
        sb.Append("|");
        allFileLines.Rows.Add(sb.ToString());

        allFileLines.Rows.Add("Z9|1|"); // Not L10N
      }

      var filePath = new StringBuilder();
      filePath.Append(ConfigUtil.TargetDirectory);
      filePath.Append("TAR4_"); // Not L10N
      filePath.Append(tsp_no);
      filePath.Append("_"); // Not L10N
      if (DateTime.Parse(start_dte).ToString("MMMM").Equals(DateTime.Parse(end_dte).ToString("MMMM"))) { // Not L10N
        filePath.Append(DateTime.Parse(end_dte).ToString("MMMM")); // Not L10N
        filePath.Append("_"); // Not L10N
        filePath.Append(DateTime.Parse(end_dte).ToString("yyyy")); // Not L10N
      } else {
        filePath.Append(DateTime.Parse(start_dte).ToString("MMMM")); // Not L10N
        filePath.Append("_"); // Not L10N
        filePath.Append(DateTime.Parse(end_dte).ToString("MMMM")); // Not L10N
        filePath.Append("_"); // Not L10N
        filePath.Append(DateTime.Parse(end_dte).ToString("yyyy")); // Not L10N
      }
      filePath.Append(".txt"); // Not L10N

      FileIO.WriteFile(allFileLines, filePath.ToString());
    }
  }
}
