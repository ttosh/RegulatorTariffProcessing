using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Globalization;

using TRCAttributes;
using TRCDataAccess;
using TRCUtilities;

// namespaces...
namespace TRCBusiness {
  // public classes...
  [Author("Tom Kraft", version = 1.0)]
  [Author("Timothy Tosh", version = 1.1)]
  public class ObjectGSD2 {
    // const fields...
    private const int fileLineCountMinusC3Lines = 4;
    private const int headerRowContractNumber = 35;

    private static readonly List<string> headerContractList = new List<string>(); 

    // private fields...
    private static readonly Dictionary<string, IList<string>> c3EntriesPerContract = new Dictionary<string, IList<string>>();

    // public fields...
    public static string report_type, tsp_no, start_dte, end_dte, ctr_no;
    public static int row_id;

    // private properties...
    private static DataTable AuthorizingAgent {
      get {
        return DAL.AUTHORIZING_AGENT(report_type, tsp_no);
      }
    }
    private static DataTable CurrentRateComponent {
      get {
        return DAL.GSD2_CURRATECOMP(tsp_no, start_dte, end_dte, ctr_no);
      }
    }
    private static DataTable Customer {
      get {
        return DAL.GSD2_CUSTOMER(tsp_no, start_dte, end_dte, ctr_no); // just need first row for B2
      }
    }
    private static DataTable DeliveryPoint {
      get {
        return DAL.GSD2_DELIVERYPOINT(tsp_no, start_dte, end_dte, ctr_no);
      }
    }
    private static DataTable Header {
      get {
        return DAL.GSD2_HEADER(tsp_no, start_dte, end_dte);
      }
    }

    // private methods...
    /// <summary>
    /// Remove the final column before compiling the Header Row, because that column 
    /// is row_id which should not appear in the output file
    /// </summary>
    /// <param name="headerRow"></param>
    /// <param name="allFileLines"></param>
    private static void CompileHeaderRow(DataRow headerRow, ref DataTable allFileLines) {
      var columnValues = headerRow.ItemArray;


      if (headerContractList.Contains(columnValues[headerRowContractNumber].ToString())) {
        return;
      }

      var columnCount = columnValues.Length;
      var columnValuesEdited = new object[columnCount - 2];

      for (var i = 0; i < columnCount - 2; i++) {
        columnValuesEdited[i] = columnValues[i];
      }
      headerContractList.Add(columnValues[headerRowContractNumber].ToString());
      Common.CompileFileLines(columnValuesEdited, ref allFileLines);
    }
    /// <summary>
    /// Datatable for the file terminator
    /// </summary>
    /// <param name="fileCount"></param>
    /// <returns></returns>
    private static DataTable FileTerminator(int fileCount) {
      return DAL.FILE_TERMINATOR(fileCount);
    }

    private static DataTable RecompileFileLinesBasedOnFinalAuditFile(IEnumerable<DataRow> rows) {
      var currentContractNumber = string.Empty;
      foreach (var row in rows.Reverse()) {
        if (row["fileline"].ToString().StartsWith("D4")) {
          currentContractNumber = row[0].ToString().Split('|')[2];
          if (!c3EntriesPerContract.ContainsKey(currentContractNumber)) {
            c3EntriesPerContract.Add(currentContractNumber, new List<string>());
          }
        }

        if (currentContractNumber == string.Empty) { continue; }

        if (row["fileline"].ToString().StartsWith("D4")) {
          c3EntriesPerContract[currentContractNumber].Add(row["fileline"].ToString());
        } else {
          if (row["fileline"].ToString().StartsWith("C3")) {
            c3EntriesPerContract[currentContractNumber].Add(row["fileline"].ToString());
          } else {
            if (row["fileline"].ToString().StartsWith("B2")) {
              var lastPos = row["fileline"].ToString().LastIndexOf('|');
              var lastPosButOne = row["fileline"].ToString().LastIndexOf('|', lastPos - 1);
              c3EntriesPerContract[currentContractNumber].Add(row["fileline"].ToString().Substring(0, lastPosButOne+1));
            } else {
              if (row["fileline"].ToString().StartsWith("A1")) {
                c3EntriesPerContract[currentContractNumber].Add(row["fileline"].ToString() + "|");
              } else {
                if (row["fileline"].ToString().StartsWith("AA")) {
                  c3EntriesPerContract[currentContractNumber].Add(row["fileline"].ToString());
                }
              }
            }
          }
        }
      }

      var newAuditFileLines = new DataTable();
      newAuditFileLines.Columns.Add("fileline", Type.GetType("System.String"));

      var fileLineCount = 0;
      var d4List = new List<string>();
      foreach (KeyValuePair<string, IList<string>> entry in c3EntriesPerContract.Reverse()) {
        var c3Count = 0;
        foreach (string fileLine in entry.Value.Reverse()) {
          if (fileLineCount == 0 && fileLine.StartsWith("AA")) {
            newAuditFileLines.Rows.Add(fileLine);
          } else {
            if (fileLine.StartsWith("A1")) {
              newAuditFileLines.Rows.Add(fileLine);
            } else {
              if (fileLine.StartsWith("B2")) {
                newAuditFileLines.Rows.Add(fileLine);
              } else {
                if (fileLine.StartsWith("C3")) {
                  newAuditFileLines.Rows.Add(fileLine);
                  c3Count++;
                } else {
                  if (fileLine.StartsWith("D4")) {
                    if (!d4List.Contains(fileLine)) {
                      d4List.Add(fileLine);
                    }
                  }
                }
              }
            }
          }
          if (fileLineCount == entry.Value.Count - 1 && fileLine.StartsWith("D4")) {
            var iterativeContractNumber = d4List[0].Split('|')[2];
            var sb = new StringBuilder();
            sb.Append("D4|||");
            sb.Append(iterativeContractNumber);
            sb.Append("|");
            // SINCE TAR4 WAS INTRODUCED THE D4 RECORD IS BLANK ON THE GSD2 FILE
            //foreach (string fileLineArray in d4List) {
            //  var arr = fileLineArray.Split('|').Distinct().ToArray();
            //  for (var y = 1; y <= arr.Length - 1; y++) {
            //    if (arr[y] == iterativeContractNumber || arr[y] == string.Empty) {
            //      continue;
            //    }
            //    sb.Append(arr[y]);
            //    if (y != arr.Length - 1) {
            //      sb.Append("; ");
            //    }
            //  }
            //}
            sb.Append("|");
            newAuditFileLines.Rows.Add(sb.ToString());
            newAuditFileLines.Rows.Add(string.Format("Z9|{0}|", c3Count + fileLineCountMinusC3Lines-1));
          }
          fileLineCount++;
        }
        d4List.Clear();
        fileLineCount = 0;
      }
      return newAuditFileLines;
    }


    // public methods...
    /// <summary>
    /// Create the file from the data 
    /// </summary>
    public static void CreateFileFromData() {
      var allFileLines = new DataTable();
      allFileLines.Columns.Add("fileline", Type.GetType("System.String"));

      var headerRowCtrNoList = new List<string>();
      var headerRowArray = Header.Select();
      foreach (DataRow headerRow in headerRowArray) {
        var fileCount = 1;

        Common.CompileFileLines(AuthorizingAgent, ref allFileLines);
        CompileHeaderRow(headerRow, ref allFileLines);

        ctr_no = headerRow["ctr_no"].ToString();
        if (headerRowCtrNoList.Any(tn => tn.Contains(ctr_no))) {
          continue;
        } // do not repeat contracts, not necessary
        headerRowCtrNoList.Add(ctr_no);

        DataTable customerDataTable = null;
        try {
          customerDataTable = Customer;
          if (customerDataTable.Rows.Count == 0) {
            ExceptionHandler.LogError("TRC_Files_Console_App",
              new Exception(string.Format("No Customer Record for Contract: {0}", ctr_no)));
            continue;
          }
        }
        catch (Exception) {
          ExceptionHandler.LogError("TRC_Files_Console_App",
              new Exception(string.Format("No Customer Record for Contract: {0}", ctr_no)));
          continue;
        }

        var delPointTable = DeliveryPoint;
        delPointTable.Columns.Add("SortDateValue", typeof(DateTime));
        foreach (DataRow row in delPointTable.Rows) {

          var day = row["MIN_EFF_DT_FROM"].ToString().Substring(0, 2);
          var month = row["MIN_EFF_DT_FROM"].ToString().Substring(2, 2);
          var year = row["MIN_EFF_DT_FROM"].ToString().Substring(4, 4);

          var stringDate = string.Format("{0}/{1}/{2}", day, month, year);

          row["SortDateValue"] = DateTime.ParseExact(stringDate, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToShortDateString();
        }
        delPointTable = delPointTable.AsEnumerable().OrderBy(r => r.Field<DateTime>("SortDateValue")).CopyToDataTable();
        delPointTable.Columns.Remove("SortDateValue");

        var currentRateTable = CurrentRateComponent;

        Common.CompileFileLines(customerDataTable, ref allFileLines);
        Common.CompileFileLines(delPointTable, ref allFileLines);
        Common.CompileFileLines(currentRateTable, delPointTable, ref allFileLines);

        fileCount += customerDataTable.Rows.Count;
        fileCount += delPointTable.Rows.Count;
        //fileCount += 1;  per Josh the D4 line should not be counted in the total file lines.

        Common.CompileFileLines(FileTerminator(fileCount), ref allFileLines);
      }

      var filePath = new StringBuilder();
      filePath.Append(ConfigUtil.TargetDirectory);
      filePath.Append("GSD2_");
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

      var dt = RecompileFileLinesBasedOnFinalAuditFile(allFileLines.Select());
      FileIO.WriteFile(dt, filePath.ToString());
    }
  }
}
