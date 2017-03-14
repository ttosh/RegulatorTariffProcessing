using System;
using System.Collections.Generic;
using System.IO;
using TRCAttributes;
using TRCBusiness;
using TRCUtilities;

using TRCTariffPDFUtil;

// namespaces...
namespace TRCFiles {
  // internal classes...
  [Author("Tom Kraft", version = 1.0)]
  [Author("Timothy Tosh", version = 1.1)]
  internal class Program {
    // public fields...
    public static string processQueueId;

    // private methods...
    private static void CreateTrcFile(IList<string> args) {

      switch (args[1]) {
        case "TAR2":
          processQueueId = args[0];
          ObjectTAR2.report_type = args[1];
          ObjectTAR2.tsp_no = args[2];
          ObjectTAR2.start_dte = args[3];
          ObjectTAR2.end_dte = args[4];

          ObjectTAR2.CreateFileFromData();
          break;

        case "TAR3":
          processQueueId = args[0];
          ObjectTAR3.report_type = args[1];
          ObjectTAR3.tsp_no = args[2];
          ObjectTAR3.start_dte = args[3];
          ObjectTAR3.end_dte = args[4];

          ObjectTAR3.CreateFileFromData();
          break;

        case "TAR4":
          processQueueId = args[0];
          ObjectTAR4.report_type = args[1];
          ObjectTAR4.tsp_no = args[2];
          ObjectTAR4.start_dte = args[3];
          ObjectTAR4.end_dte = args[4];

          ObjectTAR4.CreateFileFromData();
          break;

        case "GSD2":
          processQueueId = args[0];
          ObjectGSD2.report_type = args[1];
          ObjectGSD2.tsp_no = args[2];
          ObjectGSD2.start_dte = args[3];
          ObjectGSD2.end_dte = args[4];

          ObjectGSD2.CreateFileFromData();
          break;
      }
    }
    public static void Main(string[] args) {

      //TRCTariffPDFUtil.PDFUtil pdf = new PDFUtil(@"\\srvapp525oke\data\QPTM_Intra\PRod\Regulatory\TRC_INBOUND\7543 - GSD2 - 20160301.pdf");
      //pdf.ProcessTRCTariffPDFFile();

      //pdf = new PDFUtil(@"\\srvapp525oke\data\QPTM_Intra\Prod\Regulatory\TRC_INBOUND\7706 - GSD2 - 20160301.pdf");
      //pdf.ProcessTRCTariffPDFFile();

      //args[3] = DateTime.Parse(args[3]).ToString("dd-MMM-yyyy");
      //args[4] = DateTime.Parse(args[4]).ToString("dd-MMM-yyyy");

      args = new[] { "1", "TAR4", "49", "01-Feb-2016", "29-Feb-2016" };
     
      try {
        CreateTrcFile(args);
      } catch (Exception ex) {
        ExceptionHandler.WriteArgsToLog("TRC_Files_Console_App", args);
        ExceptionHandler.LogError("TRC_Files_Console_App", ex);
        ExceptionHandler.Quit(-1);
      }
    }
  }
}
