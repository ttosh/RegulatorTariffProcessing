using System;
using System.Data;
using System.IO;

using TRCAttributes;

// namespaces...
namespace TRCDataAccess {
  // public classes...
  [Author("Tom Kraft", version = 1.0)]
  [Author("Timothy Tosh", version = 1.1)]
  public class FileIO {
    // public methods...
    /// <summary>
    /// Loop through a DataTable, writing each line to a specified file
    /// </summary>
    public static void WriteFile(DataTable fileData, string filePath) {
      var fileLines = new string[0];
      var dataRowArray = fileData.Select();

      for (var i = 0; i < dataRowArray.Length; i++) {
        var itemArray = dataRowArray[i].ItemArray;
        var fileLine = itemArray[0].ToString();

        Array.Resize(ref fileLines, i + 1);
        fileLines[i] = fileLine;
      }
      File.WriteAllLines(filePath, fileLines);
    }
  }
}
