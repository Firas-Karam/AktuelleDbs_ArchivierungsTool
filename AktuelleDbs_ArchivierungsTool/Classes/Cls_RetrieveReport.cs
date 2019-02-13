using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AktuelleDbs_ArchivierungsTool.Classes
{
    class Cls_RetrieveReport
    {
        /// <summary>
        /// This Class to read Transfer-Results 
        /// </summary>
        public string _servername { get; set; }
        public string _DbName { get; set; }
        public Cls_RetrieveReport(string serverName, string DbName)
        {
            _servername = serverName;
            _DbName = DbName;
        }
        public DataTable Archiv_Table() // Read ArchivResult Table
        {
            DataTable dt_report = new DataTable();
            if (string.IsNullOrEmpty(_servername) || string.IsNullOrEmpty(_DbName))
            {
                return dt_report;
            }
            string connectionString = "Data Source=" + _servername + "; Integrated Security=True;Initial Catalog= " + _DbName;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string str = @"If exists (select name from sys.objects where name = 'ArchivResult') SELECT * FROM ArchivResult ORDER by id ";
                    using (SqlCommand cmd = new SqlCommand(str, con))
                    {
                        cmd.CommandTimeout = 0;
                        using (SqlDataAdapter adptr = new SqlDataAdapter(cmd))
                        {
                            adptr.Fill(dt_report);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return dt_report;
        }
        public List<string> Archiv_Statistics() // Read Tables From Archiv-Databse and objects From Archiv_Scripts Table
        {
            List<string> lst_statistics = new List<string>();
            if (string.IsNullOrEmpty(_servername) || string.IsNullOrEmpty(_DbName))
            {
                return lst_statistics;
            }
            DataTable dt_objects = new DataTable();
            string connectionString = "Data Source=" + _servername + "; Integrated Security=True;Initial Catalog= " + _DbName;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    // Tables
                    string str = @"SELECT sc.name AS[Schema], COUNT(T.name) AS[Tables Count]
                    FROM sys.tables AS T INNER JOIN
                    sys.sysindexes AS I ON T.object_id = I.id AND I.indid < 2 INNER JOIN
                    sys.schemas AS sc ON T.schema_id = sc.schema_id
                    GROUP BY sc.name
                    ORDER BY[Schema]";
                    lst_statistics.Add("**** Tables ****");
                    using (SqlCommand cmd = new SqlCommand(str, con))
                    {
                        cmd.CommandTimeout = 0;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lst_statistics.Add("Schema:(" + dr[0].ToString() + "): " + dr[1].ToString());
                            }
                        }
                    }
                    lst_statistics.Add("");

                    //Objects (Views,Procedures,Functions ....)
                    str = @"If exists (select name from sys.objects where name = 'ArchivScript') SELECT src_server, src_db, src_schema, object_Type, COUNT(*) AS count
                    FROM dbo.ArchivScript
                    GROUP BY src_server, src_db, src_schema, object_Type
                    ORDER BY object_Type";
                    using (SqlCommand cmd = new SqlCommand(str, con))
                    {
                        cmd.CommandTimeout = 0;
                        using (SqlDataAdapter adptr = new SqlDataAdapter(cmd))
                        {
                            adptr.Fill(dt_objects);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            if (dt_objects.Rows.Count > 0)
            {
                DataRow[] ros = dt_objects.Select("object_Type='View' ");
                if (ros.Length > 0) lst_statistics.Add("**** Views ****");
                foreach (DataRow ro in ros)
                {
                    lst_statistics.Add("Src_DB:(" + ro["src_db"] + "): " + ro["count"].ToString());
                }
                lst_statistics.Add("");

                ros = dt_objects.Select("object_Type='Procedure' ");
                if (ros.Length > 0) lst_statistics.Add("**** Procedures ****");
                foreach (DataRow ro in ros)
                {
                    lst_statistics.Add("Src_DB:(" + ro["src_db"] + "): " + ro["count"].ToString());
                }
                lst_statistics.Add("");

                ros = dt_objects.Select("object_Type='Function' ");
                if (ros.Length > 0) lst_statistics.Add("**** Functions ****");
                foreach (DataRow ro in ros)
                {
                    lst_statistics.Add("Src_DB:(" + ro["src_db"] + "): " + ro["count"].ToString());
                }
                lst_statistics.Add("");
            }
            return lst_statistics;
        }
    }
}
