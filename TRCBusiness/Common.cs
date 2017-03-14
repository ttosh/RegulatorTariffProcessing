using System;
using System.Data;
using System.Linq;
using System.Text;

using TRCAttributes;

// namespaces...
namespace TRCBusiness {
  // public classes...
  /// <summary>
  /// Common class to assist in building the TRC files report
  /// </summary>
  [Author("Tom Kraft", version = 1.0)]
  [Author("Timothy Tosh", version = 1.1)]
  public class Common {
    // const fields...
    private const string ContractTextString = "CONTRACT_TEXT";
    private const int D4CommodityColumn = 3;
    private const int D4ContractColumn = 2;
    private const int D4DataColumn = 0;
    private const int FirstDataRow = 0;

    /// <summary>
    /// Assemble data from the passed-in DataTable into the single-column DataTable representing all of the file lines
    /// </summary>
    /// <param name="fileData"></param>
    /// <param name="allFileLines"></param>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>source</name>
    ///   </paramref>
    ///   or <paramref>
    ///     <name>selector</name>
    ///   </paramref>
    ///   is null.</exception>
    /// <exception cref="ArgumentException">The array is larger than the number of columns in the table. </exception>
    /// <exception cref="InvalidCastException">A value does not match its respective column type. </exception>
    /// <exception cref="ConstraintException">Adding the row invalidates a constraint. </exception>
    /// <exception cref="NoNullAllowedException">Trying to put a null in a column where <see cref="P:System.Data.DataColumn.AllowDBNull" /> is false. </exception>
    public static void CompileFileLines(DataTable fileData, ref DataTable allFileLines) {
      var dataRowArray = fileData.Select();
      foreach (var fileLine in dataRowArray.Select(t => t.ItemArray).Select(itemArray =>
        itemArray.Aggregate(string.Empty, (current, fieldValue) => current + (fieldValue + "|")))) {
        allFileLines.Rows.Add(fileLine);
      }
    }

    /// <summary>
    /// Referenced only by ObjectGSD2, because the multiple A1 records are looped-through, 
    /// and the row_id column needs to be removed so that it does not appear in the file.
    /// </summary>
    /// <param name="itemArray"></param>
    /// <param name="allFileLines"></param>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>source</name>
    ///   </paramref>
    ///   or <paramref>
    ///     <name>func</name>
    ///   </paramref>
    ///   is null.</exception>
    /// <exception cref="ArgumentException">The array is larger than the number of columns in the table. </exception>
    /// <exception cref="InvalidCastException">A value does not match its respective column type. </exception>
    /// <exception cref="ConstraintException">Adding the row invalidates a constraint. </exception>
    /// <exception cref="NoNullAllowedException">Trying to put a null in a column where <see cref="P:System.Data.DataColumn.AllowDBNull" /> is false. </exception>
    public static void CompileFileLines(object[] itemArray, ref DataTable allFileLines) {
      var fileLine = itemArray.Aggregate(string.Empty, (current, fieldValue) => current + (fieldValue + "|"));
      allFileLines.Rows.Add(fileLine);
    }

    /// <summary>
    /// View c3 data to see if there are multiple COMD points that differ from commodity and fuel
    /// </summary>
    /// <param name="fileData"></param>
    /// <param name="c3Data"></param>
    /// <param name="allFileLines"></param>
    /// <exception cref="ArgumentException">The column specified by <paramref>
    ///     <name>columnName</name>
    ///   </paramref>
    ///   cannot be found. </exception>
    /// <exception cref="DeletedRowInaccessibleException">Occurs when you try to set a value on a deleted row. </exception>
    /// <exception cref="InvalidCastException">Occurs when you set a value and its <see cref="T:System.Type" /> does not match <see cref="P:System.Data.DataColumn.DataType" />. </exception>
    /// <exception cref="ConstraintException">Adding the row invalidates a constraint. </exception>
    /// <exception cref="NoNullAllowedException">Trying to put a null in a column where <see cref="P:System.Data.DataColumn.AllowDBNull" /> is false. </exception>
    /// <exception cref="ArgumentOutOfRangeException">Enlarging the value of this instance would exceed <see cref="P:System.Text.StringBuilder.MaxCapacity" />. </exception>
    public static void CompileFileLines(DataTable fileData, DataTable c3Data, ref DataTable allFileLines) {
      var dataRowArray = fileData.Select();
      if (dataRowArray.Length == 0) {
        return;
      }
      var sb = new StringBuilder();
      sb.Append(dataRowArray[FirstDataRow][D4DataColumn]).Append("|");
      sb.Append(dataRowArray[FirstDataRow][D4ContractColumn]).Append("|");
      sb.Append(dataRowArray[FirstDataRow][D4CommodityColumn]).Append("|");
      foreach (var row in dataRowArray) {
        sb.Append(row[ContractTextString].ToString());
        sb.Append("|");
      }
      allFileLines.Rows.Add(sb.ToString());
    }
  }
}
