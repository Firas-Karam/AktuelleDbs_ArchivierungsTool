using System.Collections.Generic;
using System.Data.SqlClient;
    public class Cls_ReadDataBases
    {
        /// <summary>
        /// This Class to read Databases-Name from the Server
        /// </summary>
        public List<string> listOfDatabases { get; set; }
        public Cls_ReadDataBases(string serverName)
        {
            listOfDatabases = new List<string>();
            string connectionString = "Data Source=" + serverName + "; Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con))
                {
                    cmd.CommandTimeout = 0;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listOfDatabases.Add(dr[0].ToString());
                        }
                    }
                }
            }
        }
}
