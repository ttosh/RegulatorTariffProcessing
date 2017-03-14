using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

using TRCAttributes;
using TRCDataAccess;
using TRCUtilities;

namespace TRCTariffPDFUtil {

  /// <summary>
  /// PDF Utility for parsing GSD2 Files for tariff and contract numbers
  /// </summary>
  [Author("Timothy Tosh", version = 1.0)]
  public class PDFUtil {

    private const int TariffNumberIndex = 6;
    private const int ContractNumberIndex = 0;

    public const string eventSource = "TRCTariffFileSystemWatcher";

    protected string FileNameAndPath { get; private set; }

    public PDFUtil(string fileNameAndPath) {
      this.FileNameAndPath = fileNameAndPath;
    }

    /// <summary>
    /// Processing of the GSD2 files to insert/update the system
    /// </summary>
    /// <exception cref="ThreadStateException">The thread has already been started. </exception>
    /// <exception cref="OutOfMemoryException">There is not enough memory available to start this thread. </exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>source</name>
    ///   </paramref>
    ///   or <paramref>
    ///     <name>selector</name>
    ///   </paramref>
    ///   is null.</exception>
    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="FormatException"><paramref>
    ///     <name>format</name>
    ///   </paramref>
    ///   is invalid.-or- The index of a format item is less than zero, or greater than one. </exception>
    public void ProcessTRCTariffPDFFile() {

      // make sure file is fully written before reading
      if (!GetExclusiveFileLock(FileNameAndPath)) return;

      var foundContractNumberMoveToNextLine = false;
      var contractNumber = string.Empty;
      var tariffNumber = string.Empty;
      var pdfToTextString = new StringBuilder();

      Dictionary<string, List<int>> minTariffListPerContract = new Dictionary<string, List<int>>();

      using (var pdfReader = new PdfReader(FileNameAndPath)) {
        for (var i = 1; i <= pdfReader.NumberOfPages; i++) {
          pdfToTextString.Append(PdfTextExtractor.GetTextFromPage(pdfReader, i, new LocationTextExtractionStrategy()));
          var pdfToTextWithLineBreaks = pdfToTextString.ToString().Split('\n');

          // code to look for duplicate contracts with different tariff numbers
          // after duplicate found then min of tariff numbers is to be used.
          foreach (var pdfLine in pdfToTextWithLineBreaks) {
            
            if (foundContractNumberMoveToNextLine) {
              if (ValidContractNumber(pdfLine.Split(' ')[ContractNumberIndex])) {
                contractNumber = pdfLine.Split(' ')[ContractNumberIndex];
              }
              foundContractNumberMoveToNextLine = false;
            }
              
            if (pdfLine.Contains("RATE COMP.")) {
              foundContractNumberMoveToNextLine = true;
              continue;
            }

            if (pdfLine.Contains("TARIFF NO")) {
              tariffNumber = pdfLine.Split(' ')[TariffNumberIndex];
            }

            if (string.IsNullOrEmpty(tariffNumber) || string.IsNullOrEmpty(contractNumber))
              continue;


            List<int> minList = null;
            if (minTariffListPerContract.TryGetValue(contractNumber, out minList)) {
              if(minList != null) {
                minList.Add(Convert.ToInt32(tariffNumber));
                int minTariffNumber = minList.Min(s => s);
                minList.Clear();
                minList.Add(minTariffNumber);
              }
            } else {
              minList = new List<int>();
              minList.Add(Convert.ToInt32(tariffNumber));
              minTariffListPerContract.Add(contractNumber, minList);
            }

            tariffNumber = string.Empty;
            contractNumber = string.Empty;
          }
        }
      }

      List<int> finalMinTariffList = new List<int>();
      foreach (var minTariffValueList in minTariffListPerContract) {
        finalMinTariffList = finalMinTariffList.Concat(minTariffValueList.Value).ToList();
      }
      var contractNumbers = new List<string>(minTariffListPerContract.Keys);
      var fileNameList = minTariffListPerContract.Keys.Select(s => FileNameAndPath).ToList();

      // insert tariff number, contract number and other details into TRC_MASTER_TARIFF_NO table
      // update QPTM.KCTRL_CTR_USER_DEF table, User_DEV_5 column with Tariff number
      DAL.BulkInsertGSD2TariffData(finalMinTariffList.Select(i => i.ToString()).ToArray(), contractNumbers.ToArray(), fileNameList.ToArray());
      DAL.BulkUpdateTariffNumbers(finalMinTariffList.Select(i => i.ToString()).ToArray(), contractNumbers.ToArray());

      // archive the pdf file once processing is finalized
      // start thread so file can complete writing and then be opened
      var t = new Thread(ArchiveProcessedFile);
      t.Start();
    }

    private void ArchiveProcessedFile() {
      try {
        Thread.Sleep(2000);
        File.Move(FileNameAndPath,
          string.Format("{0}\\{1}", ConfigUtil.TariffFileWatchArchivePath, System.IO.Path.GetFileName(FileNameAndPath)));
      }
      catch (IOException) {
        // file already exists so delete instead of archiving
        File.Delete(string.Format("{0}\\{1}", ConfigUtil.TariffFileWatchArchivePath, System.IO.Path.GetFileName(FileNameAndPath)));
      } catch (Exception ex) {
        ExceptionHandler.LogError(eventSource, ex);
      }
    }

    /// <summary>
    /// Contract Validation logic per audit file
    /// </summary>
    /// <param name="contractNumber"></param>
    /// <returns></returns>
    private static bool ValidContractNumber(string contractNumber) {
      return !contractNumber.Contains("Gas") && !contractNumber.Contains("DESCRIPTION") && !contractNumber.Contains(".");
    }

    /// <summary>
    /// Ensure the file has been released before the service tries to use
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static bool GetExclusiveFileLock(string path) {
      var fileReady = false;
      const int MaximumAttemptsAllowed = 30;
      var attemptsMade = 0;

      while (!fileReady && attemptsMade <= MaximumAttemptsAllowed) {
        try {
          using (File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None)) {
            fileReady = true;
          }
        }
        catch (IOException) {
          attemptsMade++;
          Thread.Sleep(100);
        }
      }
      return fileReady;
    }
  }
}