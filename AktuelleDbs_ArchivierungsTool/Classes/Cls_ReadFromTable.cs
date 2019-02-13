using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Cls_ReadFromTable
{
    public string _servername { get; set; }
    public string _DbName { get; set; }
    public string _schema { get; set; }
    public string _table { get; set; }
    public Cls_ReadFromTable(string serverName, string DbName, string schema, string table)
    {
        _servername = serverName;
        _DbName = DbName;
        _schema = schema;
        _table = table;
    }
    public DataTable GetTableColumns_Names()
    {
        DataTable dt_columns = new DataTable();
        if (string.IsNullOrEmpty(_servername) || string.IsNullOrEmpty(_DbName) || string.IsNullOrEmpty(_schema) || string.IsNullOrEmpty(_table))
        {
            return dt_columns;
        }
        string connectionString = "Data Source=" + _servername + "; Integrated Security=True;Initial Catalog= " + _DbName;
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string str = @" select COLUMN_NAME, DATA_TYPE from INFORMATION_SCHEMA.COLUMNS
                where TABLE_SCHEMA ='" + _schema + "' AND TABLE_NAME='" + _table + "'";
                using (SqlCommand cmd = new SqlCommand(str, con))
                {
                    cmd.CommandTimeout = 0;
                    using (SqlDataAdapter adptr = new SqlDataAdapter(cmd))
                    {
                        adptr.Fill(dt_columns);
                    }
                }
            }
        }
        catch (Exception)
        {
        }
        return dt_columns;
    }
    public List<string> GetTableColumns_NamesAndTypes()
    {
        List<string> lst_nameAndtype = new List<string>();
        if (string.IsNullOrEmpty(_servername) || string.IsNullOrEmpty(_DbName) || string.IsNullOrEmpty(_schema) || string.IsNullOrEmpty(_table))
        {
            return lst_nameAndtype;
        }
        string connectionString = "Data Source=" + _servername + "; Integrated Security=True;Initial Catalog= " + _DbName;
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string str = @" select COLUMN_NAME + ' (' + DATA_TYPE + ')' from INFORMATION_SCHEMA.COLUMNS
                where TABLE_SCHEMA ='" + _schema + "' AND TABLE_NAME='" + _table + "'";
                using (SqlCommand cmd = new SqlCommand(str, con))
                {
                    cmd.CommandTimeout = 0;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lst_nameAndtype.Add(dr[0].ToString());
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
        }
        return lst_nameAndtype;
    }
    public string GetTable_RowsCount(string condition)
    {
        string roCnt = "";
        if (string.IsNullOrEmpty(_servername) || string.IsNullOrEmpty(_DbName) || string.IsNullOrEmpty(_schema) || string.IsNullOrEmpty(_table))
        {
            return roCnt;
        }
        try
        {
            string connectionString = "Data Source=" + _servername + "; Integrated Security=True;Initial Catalog= " + _DbName;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string str = @" select Count(*) FROM " + _schema + "." + _table + " " + condition;
                using (SqlCommand cmd = new SqlCommand(str, con))
                {
                    cmd.CommandTimeout = 0;
                    roCnt = cmd.ExecuteScalar().ToString();
                }
            }
        }
        catch (Exception)
        {
        }
        return roCnt;
    }

}