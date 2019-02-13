using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class Cls_CreateArchivTables
{
    public string Create_ArchivResult_Table(string server, string db)
    {
        string result = "No Change";
        string connString = "Data Source=" + server + "; Integrated Security=True;Initial Catalog= " + db + ";Connection Timeout=0";
        string commandStr = @"If not exists (select name from sys.objects where name = 'ArchivResult') 
                CREATE TABLE ArchivResult([id] [int] IDENTITY(1,1) NOT NULL,
                [src_server] [nvarchar] (50) NULL,
                [src_db] [nvarchar] (150) NULL,
                [src_schema] [nvarchar] (100) NULL,
                [src_TableName] [nvarchar] (300) NULL,
                [src_rowsCount] [nvarchar] (30) NULL,
                [condition] [nvarchar] (Max) NULL,
                [archivedRows] [nvarchar] (30) NULL,
                [dst_server] [nvarchar] (50) NULL,
                [dst_db] [nvarchar] (150) NULL,
                [dst_schema] [nvarchar] (100) NULL,
                [dst_TableName] [nvarchar] (300) NULL,
                [dst_rowsCount] [nvarchar] (30) NULL,
                [archive_Type] [nvarchar] (50) NULL,
                [note] [nvarchar] (Max) NULL,
                [trsf_status] [nvarchar] (Max) NULL,
                [trsf_result] [nvarchar] (300) NULL,
                [trsf_time] datetime,
                [trsf_user] [nvarchar] (Max) NULL,
                [trsf_period] [nvarchar] (30) NULL,
                CONSTRAINT [PK_ArchivResult] PRIMARY KEY CLUSTERED 
                ([id] ASC)
                WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
                ON [PRIMARY])";
        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(commandStr, con))
                {
                    command.CommandTimeout = 0;
                    command.ExecuteNonQuery();
                    result = "Done"; // = entweder Exist oder Neu
                }
            }
        }
        catch (Exception ex)
        {
            result = "Error: Creating (ArchivResult) Table" + ex.Message;
        }
        return result;
    }

}
