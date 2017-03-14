using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using TRCAttributes;

// namespaces...
namespace TRCUtilities {
  // public classes...
  [Author("Tom Kraft", version = 1.0)]
  [Author("Timothy Tosh", version = 1.1)]
  public class ExceptionHandler {
    // private methods...
    private static void WriteToLog(string moduleName, string msg) {
      var now = DateTime.Now.ToString(CultureInfo.InvariantCulture);
      var logLine = string.Format("{0}\t{1}", now, msg);
      var logFile = string.Format("{0}{1}.log", ConfigUtil.LogDirectory, moduleName);
      File.AppendAllText(logFile, logLine);
    }

    // public methods...
    /// <exception cref="ArgumentOutOfRangeException">Enlarging the value of this instance would exceed <see cref="P:System.Text.StringBuilder.MaxCapacity" />. </exception>
    public static void LogError(string moduleName, Exception ex) {
      var sb = new StringBuilder();
      sb.AppendLine(ex.Message);
      if (ex.InnerException != null) {
        sb.AppendLine(ex.InnerException.Message);
      }
      if (ex.StackTrace != null) {
        sb.AppendLine(ex.StackTrace);
      }

      WriteToLog(moduleName, sb.ToString());
    }

    /// <exception cref="SecurityException">The caller does not have sufficient security permission to perform this function. </exception>
    public static void Quit(int exitCode) {
      Environment.Exit(exitCode);
    }

    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>format</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="FormatException"><paramref>
    ///     <name>format</name>
    ///   </paramref>
    ///   is invalid.-or- The index of a format item is less than zero, or greater than one. </exception>
    public static void WriteArgsToLog(string moduleName, string[] args) {
      var argString = string.Empty;
      for (var i = 0; i < args.Length; i++) {
        argString += string.Format("arg[{0}] = {1}; ", i, args[i]);
      }
      argString += "\n";

      WriteToLog(moduleName, argString);
    }

    /// <exception cref="Win32Exception">The operating system reported an error when writing the event entry to the event log. A Windows error code is not available.</exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>format</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="FormatException"><paramref>
    ///     <name>format</name>
    ///   </paramref>
    ///   is invalid.-or- The index of a format item is less than zero, or greater than one. </exception>
    /// <exception cref="ArgumentException">The <paramref>
    ///     <name>source</name>
    ///   </paramref>
    ///   value is an empty string ("").- or -The <paramref>
    ///     <name>source</name>
    ///   </paramref>
    ///   value is null.- or -<paramref>
    ///     <name>eventID</name>
    ///   </paramref>
    ///   is less than zero or greater than <see cref="F:System.UInt16.MaxValue" />.- or -The message string is longer than 32766 bytes.
    /// - or -The source name results in a registry key path longer than 254 characters.</exception>
    /// <exception cref="InvalidOperationException">The registry key for the event log could not be opened.</exception>
    /// <exception cref="InvalidEnumArgumentException"><paramref>
    ///     <name>type</name>
    ///   </paramref>
    ///   is not a valid <see cref="T:System.Diagnostics.EventLogEntryType" />.</exception>
    public static void WriteToEventLog(string eventSource, string message) {
      var dt = DateTime.UtcNow;
      message = string.Format("{0}: {1}", dt.ToLocalTime(), message);
      EventLog.WriteEntry(eventSource, message, EventLogEntryType.Information, 1);
    }
  }
}
