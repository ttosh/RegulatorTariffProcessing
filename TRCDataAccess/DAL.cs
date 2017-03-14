using System;
using System.Configuration;
using Oracle.DataAccess.Client;
using System.Data;

using TRCAttributes;
using TRCUtilities;

// namespaces...
namespace TRCDataAccess {
  // public classes...
  [Author("Tom Kraft", version = 1.0)]
  [Author("Timothy Tosh", version = 1.1)]
  public class DAL {
    // public methods...
    /// <summary>
    /// Returns the Authorizing Agent record common to each report data set
    /// </summary>
    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> 
    /// is read-only.-or- The property is set, <paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   does not exist in the collection, and the 
    /// <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
    public static DataTable AUTHORIZING_AGENT(string fileType, string tspNo) {
      var procName = ConfigUtil.GetStoredProcedureName("AUTHORIZING_AGENT");

      var oracleParams = new OracleParameter[3];
      oracleParams[0] = new OracleParameter("i_file_type", OracleDbType.Varchar2, ParameterDirection.Input) {
        Value = fileType
      };
      oracleParams[1] = new OracleParameter("i_tsp_no", OracleDbType.Int32, ParameterDirection.Input) {
        Value = tspNo
      };
      oracleParams[2] = new OracleParameter("o_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

      var dt = OracleUtil.ExecuteOracleReader(procName, oracleParams, CommandType.StoredProcedure);
      return dt;
    }

    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> 
    /// is read-only.-or- The property is set, <paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   does not exist in the collection, and the 
    /// <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
    public static void BulkInsertGSD2TariffData(string[] tariffNumbers, string[] contractNumbers, string[] fileNames) {

      var procName = ConfigUtil.GetStoredProcedureName("TRC_GSD2_BULKINSERT");

      var oracleParams = new OracleParameter[3];
      oracleParams[0] = new OracleParameter("i_tariff_no", OracleDbType.Varchar2, ParameterDirection.Input) {
        Value = tariffNumbers
      };

      oracleParams[1] = new OracleParameter("i_ctr_no", OracleDbType.Varchar2, ParameterDirection.Input) {
        Value = contractNumbers
      };

      oracleParams[2] = new OracleParameter("i_file_name", OracleDbType.Varchar2, ParameterDirection.Input) {
        Value = fileNames
      };

      OracleUtil.ExecuteOracleBatchAction(procName, oracleParams, CommandType.StoredProcedure, tariffNumbers.Length);
    }

    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> 
    /// is read-only.-or- The property is set, <paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   does not exist in the collection, and the 
    /// <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
    public static void BulkUpdateTariffNumbers(string[] tariffNumbers, string[] contractNumbers) {
      var procName = ConfigUtil.GetStoredProcedureName("TRC_GSD2_BULKUPDATE");

      var oracleParams = new OracleParameter[2];
      oracleParams[0] = new OracleParameter("i_ctr_no", OracleDbType.Varchar2, ParameterDirection.Input) {
        Value = contractNumbers
      };

      oracleParams[1] = new OracleParameter("i_tariff_no", OracleDbType.Varchar2, ParameterDirection.Input) {
        Value = tariffNumbers
      };

      OracleUtil.ExecuteOracleBatchAction(procName, oracleParams, CommandType.StoredProcedure, tariffNumbers.Length);
    }

    /// <summary>
    /// Returns the file terminator record common to each report data set
    /// </summary>
    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> 
    /// is read-only.-or- The property is set, <paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   does not exist in the collection, and the 
    /// <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
    public static DataTable FILE_TERMINATOR(int recordCount) {
      var procName = ConfigUtil.GetStoredProcedureName("FILE_TERMINATOR");

      var oracleParams = new OracleParameter[2];
      oracleParams[0] = new OracleParameter("i_record_count", OracleDbType.Int32, ParameterDirection.Input) {
        Value = recordCount
      };
      oracleParams[1] = new OracleParameter("o_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

      var dt = OracleUtil.ExecuteOracleReader(procName, oracleParams, CommandType.StoredProcedure);
      return dt;
    }

    /// <summary>
    /// Returns the GSD2 D4 record
    /// </summary>
    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> 
    /// is read-only.-or- The property is set, <paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   does not exist in the collection, and the 
    /// <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
    /// <exception cref="FormatException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   is not in the correct format. </exception>
    /// <exception cref="OverflowException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
    public static DataTable GSD2_CURRATECOMP(string tspNo, string startDte, string endDte, string ctr_no) {
      var procName = ConfigUtil.GetStoredProcedureName("GSD2_CURRATECOMP");

      var oracleParams = new OracleParameter[5];
      oracleParams[0] = new OracleParameter("i_tsp_no", OracleDbType.Int32, ParameterDirection.Input) {
        Value = int.Parse(tspNo)
      };
      oracleParams[1] = new OracleParameter("i_start_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = startDte
      };
      oracleParams[2] = new OracleParameter("i_end_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = endDte
      };
      oracleParams[3] = new OracleParameter("i_ctr_no", OracleDbType.Varchar2, ParameterDirection.Input) {
        Value = ctr_no
      };
      oracleParams[4] = new OracleParameter("o_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

      var dt = OracleUtil.ExecuteOracleReader(procName, oracleParams, CommandType.StoredProcedure);
      return dt;
    }

    /// <summary>
    /// Returns the GSD2 B2 record
    /// </summary>
    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> 
    /// is read-only.-or- The property is set, <paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   does not exist in the collection, and the 
    /// <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
    /// <exception cref="FormatException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   is not in the correct format. </exception>
    /// <exception cref="OverflowException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
    public static DataTable GSD2_CUSTOMER(string tspNo, string startDte, string endDte, string ctr_no) {
      var procName = ConfigUtil.GetStoredProcedureName("GSD2_CUSTOMER");

      var oracleParams = new OracleParameter[5];
      oracleParams[0] = new OracleParameter("i_tsp_no", OracleDbType.Int32, ParameterDirection.Input) {
        Value = int.Parse(tspNo)
      };
      oracleParams[1] = new OracleParameter("i_start_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = startDte
      };
      oracleParams[2] = new OracleParameter("i_end_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = endDte
      };
      oracleParams[3] = new OracleParameter("i_ctr_no", OracleDbType.Varchar2, ParameterDirection.Input) {
        Value = ctr_no
      };
      //oracleParams[3] = new OracleParameter("row_id", OracleDbType.Int32, ParameterDirection.Input) {
      //  Value = rowId
      //};
      oracleParams[4] = new OracleParameter("o_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

      var dt = OracleUtil.ExecuteOracleReader(procName, oracleParams, CommandType.StoredProcedure);
      return dt;
    }

    /// <summary>
    /// Returns the GSD2 C3 record
    /// </summary>
    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> 
    /// is read-only.-or- The property is set, <paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   does not exist in the collection, and the 
    /// <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
    /// <exception cref="FormatException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   is not in the correct format. </exception>
    /// <exception cref="OverflowException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
    public static DataTable GSD2_DELIVERYPOINT(string tspNo, string startDte, string endDte, string ctr_no) {
      var procName = ConfigUtil.GetStoredProcedureName("GSD2_DELIVERYPOINT");

      var oracleParams = new OracleParameter[5];
      oracleParams[0] = new OracleParameter("i_tsp_no", OracleDbType.Int32, ParameterDirection.Input) {
        Value = int.Parse(tspNo)
      };
      oracleParams[1] = new OracleParameter("i_start_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = startDte
      };
      oracleParams[2] = new OracleParameter("i_end_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = endDte
      };
      oracleParams[3] = new OracleParameter("i_ctr_no", OracleDbType.Varchar2, ParameterDirection.Input) {
        Value = ctr_no
      };
      //oracleParams[3] = new OracleParameter("row_id", OracleDbType.Int32, ParameterDirection.Input) {
      //  Value = rowId
      //};
      oracleParams[4] = new OracleParameter("o_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

      var dt = OracleUtil.ExecuteOracleReader(procName, oracleParams, CommandType.StoredProcedure);
      return dt;
    }

    /// <summary>
    /// Returns the GSD2 A1 record
    /// </summary>
    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> 
    /// is read-only.-or- The property is set, <paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   does not exist in the collection, and the 
    /// <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
    /// <exception cref="FormatException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   is not in the correct format. </exception>
    /// <exception cref="OverflowException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
    public static DataTable GSD2_HEADER(string tspNo, string startDte, string endDte) {
      var procName = ConfigUtil.GetStoredProcedureName("GSD2_HEADER");

      var oracleParams = new OracleParameter[4];
      oracleParams[0] = new OracleParameter("i_tsp_no", OracleDbType.Int32, ParameterDirection.Input) {
        Value = int.Parse(tspNo)
      };
      oracleParams[1] = new OracleParameter("i_start_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = startDte
      };
      oracleParams[2] = new OracleParameter("i_end_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = endDte
      };
      oracleParams[3] = new OracleParameter("o_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

      var dt = OracleUtil.ExecuteOracleReader(procName, oracleParams, CommandType.StoredProcedure);
      return dt;
    }

    /// <summary>
    /// Returns the TAR2 A1 record
    /// </summary>
    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> 
    /// is read-only.-or- The property is set, <paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   does not exist in the collection, and the 
    /// <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
    /// <exception cref="FormatException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   is not in the correct format. </exception>
    /// <exception cref="OverflowException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
    public static DataTable TAR2_CUSTOMER(string tspNo, string startDte, string endDte) {
      var procName = ConfigUtil.GetStoredProcedureName("TAR2_CUSTOMER");

      var oracleParams = new OracleParameter[4];
      oracleParams[0] = new OracleParameter("i_tsp_no", OracleDbType.Int32, ParameterDirection.Input) {
        Value = int.Parse(tspNo)
      };
      oracleParams[1] = new OracleParameter("i_start_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = startDte
      };
      oracleParams[2] = new OracleParameter("i_end_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = endDte
      };
      oracleParams[3] = new OracleParameter("o_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

      var dt = OracleUtil.ExecuteOracleReader(procName, oracleParams, CommandType.StoredProcedure);
      return dt;
    }

    /// <summary>
    /// Returns the TAR3 A1 record
    /// </summary>
    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> 
    /// is read-only.-or- The property is set, <paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   does not exist in the collection, and the 
    /// <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
    /// <exception cref="FormatException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   is not in the correct format. </exception>
    /// <exception cref="OverflowException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
    public static DataTable TAR3_DELIVERYPOINT(string tspNo, string startDte, string endDte) {
      var procName = ConfigUtil.GetStoredProcedureName("TAR3_DELIVERYPOINT");

      var oracleParams = new OracleParameter[4];
      oracleParams[0] = new OracleParameter("i_tsp_no", OracleDbType.Int32, ParameterDirection.Input) {
        Value = int.Parse(tspNo)
      };
      oracleParams[1] = new OracleParameter("i_start_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = startDte
      };
      oracleParams[2] = new OracleParameter("i_end_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = endDte
      };
      oracleParams[3] = new OracleParameter("o_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

      var dt = OracleUtil.ExecuteOracleReader(procName, oracleParams, CommandType.StoredProcedure);
      return dt;
    }

    /// <summary>
    /// Returns the TAR4 A1 record
    /// </summary>
    /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> 
    /// is read-only.-or- The property is set, <paramref>
    ///     <name>key</name>
    ///   </paramref>
    ///   does not exist in the collection, and the 
    /// <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
    /// <exception cref="FormatException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   is not in the correct format. </exception>
    /// <exception cref="OverflowException"><paramref>
    ///     <name>s</name>
    ///   </paramref>
    ///   represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
    public static DataTable TAR4_DATAPOINT(string tspNo, string startDte, string endDte) {
      var procName = ConfigUtil.GetStoredProcedureName("TAR4_DATAPOINT");

      var oracleParams = new OracleParameter[4];
      oracleParams[0] = new OracleParameter("i_tsp_no", OracleDbType.Int32, ParameterDirection.Input) {
        Value = int.Parse(tspNo)
      };
      oracleParams[1] = new OracleParameter("i_start_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = startDte
      };
      oracleParams[2] = new OracleParameter("i_end_dte", OracleDbType.Date, ParameterDirection.Input) {
        Value = endDte
      };
      oracleParams[3] = new OracleParameter("o_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

      var dt = OracleUtil.ExecuteOracleReader(procName, oracleParams, CommandType.StoredProcedure);
      return dt;
    }
  }
}
