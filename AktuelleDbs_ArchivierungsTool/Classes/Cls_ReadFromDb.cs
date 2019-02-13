using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class Cls_ReadFromDb
{
    /// <summary>
    /// This Class to read the objects (Tables, Views, Stored Procedures, Functions) from the DataBase
    /// </summary>
    public string _servername { get; set; }
    public string _DbName { get; set; }
    public string _schema { get; set; }
    public Cls_ReadFromDb(string serverName, string DbName, string schema)
    {
        _servername = serverName;
        _DbName = DbName;
        _schema = schema;
    }
    public DataTable GetTables()
    {
        DataTable dtb_tables = new DataTable();
        if (string.IsNullOrEmpty(_servername) || string.IsNullOrEmpty(_DbName))
        {
            return dtb_tables;
        }
        string connectionString = "Data Source=" + _servername + "; Integrated Security=True;Initial Catalog= " + _DbName;
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string condition = "";
                if (!string.IsNullOrEmpty(_schema))
                {
                    condition = " WHERE sc.name='" + _schema + "' ";
                }

                string str = @" SELECT sc.name AS[Schema], 
                    T.name AS[Table Name], 
                    I.rows AS[Rowcount],
                    T.create_date AS[Create Date],
                    T.modify_date AS[Modify Date]  
                    FROM sys.tables AS T INNER JOIN sys.sysindexes AS I ON T.object_id = I.id AND I.indid < 2 INNER JOIN sys.schemas sc ON T.schema_id = sc.schema_id "
                    + condition + " ORDER BY [Schema], I.rows DESC ";
                using (SqlCommand cmd = new SqlCommand(str, con))
                {
                    cmd.CommandTimeout = 0;
                    using (SqlDataAdapter adptr = new SqlDataAdapter(cmd))
                    {
                        adptr.Fill(dtb_tables);
                    }
                }
            }
        }
        catch (System.Exception)
        {
        }
        return dtb_tables;
    }
    public string Get_primaryKey(string tableName)
    {
        string pk = "";
        string connectionString = "Data Source=" + _servername + "; Integrated Security=True;Initial Catalog= " + _DbName;
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string str = @"SELECT COLUMN_NAME
            FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
            WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1
            AND TABLE_NAME = '" + tableName + "' AND TABLE_SCHEMA = '" + _schema + "' ";
                using (SqlCommand cmd = new SqlCommand(str, con))
                {
                    cmd.CommandTimeout = 0;
                    pk = cmd.ExecuteScalar().ToString();
                }
            }
        }
        catch (System.Exception)
        {
            pk = "Kein PrimaryKey gefunden";
        }

        return pk;
    }

}