using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

class Cls_ArchiveTables
{
    /// <summary>
    /// This Class to transfer Tables From Source to Destination throgh 3 Steps:
    /// 1' Step: Read the Table-Schemas from Source (Columns Name, Primarry Key, Columns DataType .....)
    /// 2' Step: Create (if Not Exist) the new Table in the Destination DataBase.
    /// 3' Step: Move the Data from the Source-Table in the New-Table in Destination-Databse using SqlBulkCopy technology.
    /// </summary>
    public string _source_Server { get; set; }
    public string _source_Db { get; set; }
    public string _source_Schema { get; set; }
    public string _source_Table { get; set; }
    public string _dest_Server { get; set; }
    public string _dest_Db { get; set; }
    public string _dest_Schema { get; set; }
    public string _dest_Table { get; set; }
    public string _conditoin { get; set; }

    private List<string> computedColumns_Name = new List<string>();
    private DataTable destTable_schema = new DataTable();
    public Cls_ArchiveTables()
    {

    }
    public Cls_ArchiveTables(string source_Server, string source_Db, string source_Schema, string source_Table, string dest_Server, string dest_Db, string dest_Schema, string dest_Table, string conditoin)
    {
        if (string.IsNullOrEmpty(source_Server) || string.IsNullOrEmpty(source_Db) || string.IsNullOrEmpty(dest_Server) || string.IsNullOrEmpty(dest_Db))
        {
            return;
        }
        if (string.IsNullOrEmpty(source_Schema) || string.IsNullOrEmpty(source_Table) || string.IsNullOrEmpty(dest_Schema) || string.IsNullOrEmpty(dest_Table))
        {
            return;
        }
        _source_Server = source_Server;
        _source_Db = source_Db;
        _source_Schema = source_Schema;
        _source_Table = source_Table;
        _dest_Server = dest_Server;
        _dest_Db = dest_Db;
        _dest_Schema = dest_Schema;
        _dest_Table = dest_Table;
        _conditoin = conditoin;
    }
    #region ****** Transfer data Methods 3 Steps ******
    //Step 1 : Build Source DataTable-Schema
    public string Read_TableSchema()
    {
        string _result = "No Change";
        //try
        //{
        string source_connString = "Data Source=" + _source_Server + "; Integrated Security=True;Initial Catalog= " + _source_Db + ";Connection Timeout=0";
        computedColumns_Name = new List<string>();
        // Fill Datatable from Source Table
        using (SqlConnection source_con = new SqlConnection(source_connString))
        {
            source_con.Open();
            SqlCommand command = source_con.CreateCommand();
            command.CommandText = "SELECT * FROM " + _source_Schema + "." + _source_Table + " WHERE 1=2 ";
            command.CommandTimeout = 0;
            SqlDataReader rdr = command.ExecuteReader();
            destTable_schema = rdr.GetSchemaTable();
            rdr.Close();
            _result = "Done";
        }
        ////////}
        //}
        //catch (Exception ex)
        //{
        //    _result = "Error Fill_Source_DataTable: " + ex.Message;
        //}
        return _result;
    }
    //Step 2: Check for Destination Table, when not exist  => create it
    public string Create_Table()
    {
        string _result = "No Change";
        string dest_connString = "Data Source=" + _dest_Server + "; Integrated Security=True;Initial Catalog= " + _dest_Db + ";Connection Timeout=0";
        using (SqlConnection dest_con = new SqlConnection(dest_connString))
        {
            string cols = "", colSize = "", primrKey = "";
            double number;
            foreach (DataRow ro in destTable_schema.Rows)
            {
                switch (ro["DataTypeName"].ToString())
                {
                    case "binary":
                    case "char":
                    case "nchar":
                    case "varchar":
                    case "nvarchar":
                    case "datetime2":
                    case "datetimeoffset":
                    case "time":
                    case "varbinary":
                        colSize = " (" + ro["ColumnSize"].ToString() + ") ";
                        bool isNumeric = double.TryParse(ro["ColumnSize"].ToString(), out number);
                        if (isNumeric == true)
                        {
                            if (number > 8000) colSize = " (Max) ";
                        }
                        break;
                    case "decimal":
                    case "numeric":
                        colSize = " (" + ro["NumericPrecision"].ToString() + "," + ro["NumericScale"].ToString() + ") ";
                        break;
                    default:
                        colSize = " ";
                        break;
                }
                if (ro["AllowDBNull"].ToString() == "True")
                {
                    colSize += " NULL";
                }
                else
                {
                    colSize += " NOT NULL";
                }

                if (ro["IsIdentity"].ToString() == "True")
                {
                    primrKey = " CONSTRAINT[PK_" + _dest_Table + "] PRIMARY KEY CLUSTERED( [" + ro["ColumnName"].ToString() + "] ASC) ";
                }
                cols += " [" + ro["ColumnName"].ToString() + "]" + " [" + ro["DataTypeName"].ToString() + "]" + colSize + ", ";
            }
            cols += " [Archive_User] [nvarchar](MAX) NULL DEFAULT('" + System.Security.Principal.WindowsIdentity.GetCurrent().Name + "'), ";
            cols += " [Archive_Date] [datetime] NULL DEFAULT(GETDATE()), ";

            string str = @"If not exists (SELECT schemas.name, objects.name FROM sys.objects JOIN sys.schemas ON schemas.schema_id = objects.schema_id where schemas.name = '" + _dest_Schema + "' AND objects.name = '" + _dest_Table + "') " +
              "CREATE TABLE " + _dest_Schema + "." + _dest_Table + " (" + cols + primrKey + ")";
            using (SqlCommand cmd = new SqlCommand(str, dest_con))
            {
                cmd.CommandText = str;
                dest_con.Open();
                cmd.ExecuteNonQuery();
                _result = "Done";
            }
        }
        //try
        //{

        //}
        //catch (Exception)
        //{
        //    _result = "Error Create_Table: " + e.Message;
        //}

        return _result;
    }
    //Step 3 : Save Destination Data.
    public string Save_Dest_data(bool solldel)
    {
        string _result = "No Change";
        string dest_connString = "Data Source=" + _dest_Server + "; Integrated Security=True;Initial Catalog= " + _dest_Db + ";Connection Timeout=0";
        SqlDataReader rdr;
        string source_connString = "Data Source=" + _source_Server + "; Integrated Security=True;Initial Catalog= " + _source_Db + ";Connection Timeout=0";
        string sorce_strs = "SELECT * FROM " + _source_Schema + "." + _source_Table + " " + _conditoin;
        computedColumns_Name = new List<string>();
        using (SqlConnection source_con = new SqlConnection(source_connString))
        {
            source_con.Open();
            SqlCommand command = source_con.CreateCommand();
            command.CommandText = sorce_strs;
            command.CommandTimeout = 0;
            rdr = command.ExecuteReader();
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(dest_connString, SqlBulkCopyOptions.TableLock))
                {
                    bulkCopy.BulkCopyTimeout = 0; // infinity
                    bulkCopy.DestinationTableName = _dest_Schema + "." + _dest_Table;
                    bulkCopy.WriteToServer(rdr);
                }
                _result = "Done";
                rdr.Close();
                if (solldel == true) // Delete after save (Archive and Delete)
                {
                    SqlCommand delCmmd = source_con.CreateCommand();
                    string sorce_del = "DELETE FROM " + _source_Schema + "." + _source_Table + " " + _conditoin;
                    delCmmd.CommandText = sorce_del;
                    delCmmd.CommandTimeout = 0;
                    int deletedRows = 0;
                    deletedRows = delCmmd.ExecuteNonQuery();
                    _result = _result + ",Step4 Del:" + deletedRows.ToString();
                }
            }
            catch (Exception e)
            {
                rdr.Close();
                _result = "Error Copying Data: " + _source_Schema + "." + _source_Table + " - " + e.Message;
            }
        }
        return _result;
    }
    #endregion  ****** Transfer data Methods ******

    public DataTable Fill_destDataTable()
    {
        DataTable dt_toFill = new DataTable();
        try
        {
            string dst_connString = "Data Source=" + _dest_Server + "; Integrated Security=True;Initial Catalog= " + _dest_Db + ";Connection Timeout=0";
            using (SqlConnection con = new SqlConnection(dst_connString))
            {
                con.Open();
                string cmdStr = "SELECT * FROM " + _dest_Schema + "." + _dest_Table + " " + _conditoin;
                using (SqlCommand cmd = new SqlCommand(cmdStr, con))
                {
                    cmd.CommandTimeout = 0;
                    using (SqlDataAdapter adptr = new SqlDataAdapter(cmd))
                    {
                        adptr.Fill(dt_toFill);
                    }
                }
            }
        }
        catch (Exception)
        {
        }
        return dt_toFill;
    }
    public DataTable Fill_srctDataTable()
    {
        DataTable dt_toFill = new DataTable();
        try
        {
            string src_connString = "Data Source=" + _source_Server + "; Integrated Security=True;Initial Catalog= " + _source_Db + ";Connection Timeout=0";
            using (SqlConnection con = new SqlConnection(src_connString))
            {
                con.Open();
                string cmdStr = "SELECT * FROM " + _source_Schema + "." + _source_Table + " " + _conditoin;
                using (SqlCommand cmd = new SqlCommand(cmdStr, con))
                {
                    cmd.CommandTimeout = 0;
                    using (SqlDataAdapter adptr = new SqlDataAdapter(cmd))
                    {
                        adptr.Fill(dt_toFill);
                    }
                }
            }
        }
        catch (Exception)
        {
        }
        return dt_toFill;
    }

    public string deleteRow(string del_condition)
    {
        string rslt = "";
        try
        {
            string src_connString = "Data Source=" + _source_Server + "; Integrated Security=True;Initial Catalog= " + _source_Db + ";Connection Timeout=0";
            using (SqlConnection con = new SqlConnection(src_connString))
            {
                con.Open();
                string cmdStr = "DELETE FROM " + _source_Schema + "." + _source_Table + " " + del_condition;
                using (SqlCommand cmd = new SqlCommand(cmdStr, con))
                {
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    rslt = "Done";
                }
            }
        }
        catch (Exception)
        {
        }
        return rslt;
    }

}