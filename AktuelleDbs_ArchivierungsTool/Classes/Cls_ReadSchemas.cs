using System.Collections.Generic;
using System.Data.SqlClient;

public class Cls_ReadSchemas
{
    /// <summary>
    /// This Class to read Schemas-Name from the DataBase.
    /// </summary>
    public List<string> listOfSchemas { get; set; }
    public Cls_ReadSchemas(string serverName, string DbName)
    {
        listOfSchemas = new List<string>();
        if (string.IsNullOrEmpty(serverName) || string.IsNullOrEmpty(DbName))
        {
            return;
        }
        string connectionString = "Data Source=" + serverName + "; Integrated Security=True;Initial Catalog= " + DbName;
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmdschema = new SqlCommand("SELECT name FROM sys.schemas", con))
                {
                    cmdschema.CommandTimeout = 0;
                    using (SqlDataReader dr = cmdschema.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listOfSchemas.Add(dr[0].ToString());
                        }
                    }
                }
            }
        }
        catch (System.Exception)
        {
        }
    }
}