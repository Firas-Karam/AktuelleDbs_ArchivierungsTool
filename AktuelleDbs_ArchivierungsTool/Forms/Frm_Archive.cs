using DevExpress.Utils.DragDrop;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace AktuelleDbs_ArchivierungsTool
{
    public partial class Frm_Archive : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Archive()
        {
            InitializeComponent();
        }
        DataTable dt_archiv = new DataTable();
        private void Frm_Archive_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Fill_Combos_WithServers();
            Build_Grids();
            HandleBehaviorDragDropEvents();
            if (!string.IsNullOrEmpty(Properties.Settings.Default.source_server_name.ToString()))
            {
                cmb_srcServer.Text = Properties.Settings.Default.source_server_name.ToString();
                cmb_srcDb.Text = Properties.Settings.Default.source_db_name.ToString();
                cmb_srcSchema.Text = Properties.Settings.Default.source_schema_name.ToString();
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.dest_server_name.ToString()))
            {
                cmb_destServer.Text = Properties.Settings.Default.dest_server_name.ToString();
                cmb_destDb.Text = Properties.Settings.Default.dest_db_name.ToString();
                cmb_destSchema.Text = Properties.Settings.Default.dest_schema_name.ToString();
            }
            splashScreenManager1.CloseWaitForm();
        }
        private void Frm_Archive_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.source_server_name = cmb_srcServer.Text.ToString();
            Properties.Settings.Default.source_db_name = cmb_srcDb.Text.ToString();
            Properties.Settings.Default.source_schema_name = cmb_srcSchema.Text.ToString();

            Properties.Settings.Default.dest_server_name = cmb_destServer.Text.ToString();
            Properties.Settings.Default.dest_db_name = cmb_destDb.Text.ToString();
            Properties.Settings.Default.dest_schema_name = cmb_destSchema.Text.ToString();

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Reload();
        }
        #region ******** Combos ***************
        private void Fill_Combos_WithServers()
        {
            Cls_ReadServers readservers = new Cls_ReadServers();
            List<string> servers = readservers.listOfServers;
            cmb_srcServer.Properties.Items.Clear();
            cmb_destServer.Properties.Items.Clear();
            cmb_srcServer.Properties.Items.AddRange(servers.ToArray());
            cmb_destServer.Properties.Items.AddRange(servers.ToArray());
            if (cmb_srcServer.Properties.Items.Count > 0) cmb_srcServer.SelectedIndex = 0;
            if (cmb_destServer.Properties.Items.Count > 0) cmb_destServer.SelectedIndex = 0;
        }
        private void Cmb_srcServer_Enter(object sender, EventArgs e)
        {
            cmb_srcDb.Properties.Items.Clear();
        }
        private void Cmb_srcDb_Enter(object sender, EventArgs e)
        {
            cmb_srcDb.Properties.Items.Clear();
            string serverName = cmb_srcServer.Text.ToString();
            try
            {
                Cls_ReadDataBases readDatabases = new Cls_ReadDataBases(serverName);
                List<string> dataBases = readDatabases.listOfDatabases;
                cmb_srcDb.Properties.Items.AddRange(dataBases.ToArray());
            }
            catch (Exception)
            {
                MessageBox.Show("Error Reading Source DB");
            }
            cmb_srcSchema.Properties.Items.Clear();
            cmb_srcSchema.Properties.Items.Add(" Select Schema ...");
            cmb_srcSchema.SelectedIndex = 0;
        }
        private void Cmb_srcSchema_Enter(object sender, EventArgs e)
        {
            cmb_srcSchema.Properties.Items.Clear();
            string serverName = cmb_srcServer.Text.ToString();
            string DbName = cmb_srcDb.Text.ToString();
            Cls_ReadSchemas readschemas = new Cls_ReadSchemas(serverName, DbName);
            List<string> schemas = readschemas.listOfSchemas;
            cmb_srcSchema.Properties.Items.AddRange(schemas.ToArray());
            cmb_srcSchema.Properties.Items.Add(" Select Schema ...");
            cmb_srcSchema.SelectedIndex = 0;
        }
        private void Cmb_destServer_Enter(object sender, EventArgs e)
        {
            cmb_destDb.Properties.Items.Clear();
            cmb_destSchema.Properties.Items.Clear();
        }
        private void Cmb_destDb_Enter(object sender, EventArgs e)
        {
            cmb_destDb.Properties.Items.Clear();
            string serverName = cmb_destServer.Text.ToString();
            try
            {
                Cls_ReadDataBases readDatabases = new Cls_ReadDataBases(serverName);
                List<string> dataBases = readDatabases.listOfDatabases;
                cmb_destDb.Properties.Items.AddRange(dataBases.ToArray());
            }
            catch (Exception)
            {
                MessageBox.Show("Error Reading Destination DB");
            }
        }
        private void Cmb_destSchema_Enter(object sender, EventArgs e)
        {
            cmb_destSchema.Properties.Items.Clear();
            string serverName = cmb_destServer.Text.ToString();
            string DbName = cmb_destDb.Text.ToString();
            Cls_ReadSchemas readschemas = new Cls_ReadSchemas(serverName, DbName);
            List<string> schemas = readschemas.listOfSchemas;
            cmb_destSchema.Properties.Items.AddRange(schemas.ToArray());
            cmb_destSchema.Properties.Items.Add(" Select Schema ...");
            cmb_destSchema.SelectedIndex = 0;
        }
        #endregion  ********* Combos ***************

        #region ****** Methodes ********
        private void Build_Grids()
        {
            // Archiv Grid
            Cntrl_ArchivTables.DataSource = null;
            grdview_ArchivTables.Columns.Clear();
            Cls_Build_DataTables build_Dt = new Cls_Build_DataTables();
            dt_archiv = build_Dt.Build_ArchivTables();
            Cntrl_ArchivTables.DataSource = dt_archiv;
            grdview_ArchivTables.Columns["Archive Type"].ColumnEdit = rps_CmbArchiveType;

            grdview_ArchivTables.Columns["From Schema"].OptionsColumn.AllowEdit = false;
            grdview_ArchivTables.Columns["From Table"].OptionsColumn.AllowEdit = false;
            grdview_ArchivTables.Columns["Total Rows"].OptionsColumn.AllowEdit = false;
            grdview_ArchivTables.Columns["Filtered Rows"].OptionsColumn.AllowEdit = false;
            grdview_ArchivTables.Columns["To Schema"].OptionsColumn.AllowEdit = false;
            grdview_ArchivTables.Columns["To Table"].OptionsColumn.AllowEdit = false;
            grdview_ArchivTables.Columns["Rows Count"].OptionsColumn.AllowEdit = false;
            grdview_ArchivTables.Columns["Process details"].OptionsColumn.AllowEdit = false;
            grdview_ArchivTables.Columns["Result"].OptionsColumn.AllowEdit = false;

            // Wert Grid
            Cntrl_Werte.DataSource = null;
            grdview_Werte.Columns.Clear();
            DataTable dt_wert = build_Dt.Build_WerteTable();
            Cntrl_Werte.DataSource = dt_wert;
            for (int i = 0; i < 20; i++)
            {
                grdview_Werte.AddNewRow();
            }
            grdview_Werte.FocusedRowHandle = 0;

        }


        private void Read_src()
        {
            string serverFrom = cmb_srcServer.Text.ToString();
            string DbFrom = cmb_srcDb.Text.ToString();
            string schemaFrom = cmb_srcSchema.Text.ToString();
            if (string.IsNullOrEmpty(serverFrom) || string.IsNullOrEmpty(DbFrom) || string.IsNullOrEmpty(schemaFrom) || schemaFrom == " Select Schema ...")
            {
                MessageBox.Show("Source Parameteres not complete (From Server, From Database or From Schema)");
                return;
            }
            // Read Source Tables-Name
            Cls_ReadFromDb readFromTables = new Cls_ReadFromDb(serverFrom, DbFrom, schemaFrom);
            DataTable dt_realTables = readFromTables.GetTables();
            //List<int> cnt = dt_archiv.Rows.OfType<DataRow>().Select(dr => dr.Field<int>("Sort")).ToList<int>();
            //int maxVal = 0;
            //if (cnt.Count > 0) maxVal = cnt.Max();
            DataRow ro;
            // 1- check table-Names in real db with tables in grid, when not exist then add newRo with max Sort-Nomber
            foreach (DataRow realRo in dt_realTables.Rows)
            {
                foreach (DataRow gridRo in dt_archiv.Rows)
                {
                    if (realRo["Schema"].ToString() == gridRo["From Schema"].ToString() && realRo["Table Name"].ToString() == gridRo["From Table"].ToString())
                    {
                        gridRo["Total Rows"] = realRo["Rowcount"].ToString();
                        goto found;
                    }
                }
                //maxVal += 1;
                ro = dt_archiv.NewRow();
                //ro["Sort"] = maxVal;
                ro["From Table"] = realRo["Table Name"].ToString();
                ro["From Schema"] = realRo["Schema"].ToString();
                ro["Total Rows"] = realRo["Rowcount"].ToString();
                ro["Archive Type"] = "Nothing";
                // ro["Archive"] = "True";
                dt_archiv.Rows.Add(ro);
            found:;
            }
            grdview_ArchivTables.Columns["Total Rows"].ColumnEdit = rps_txtNumber;
            grdview_ArchivTables.Columns["Filtered Rows"].ColumnEdit = rps_txtNumber;
            grdview_ArchivTables.Columns["Rows Count"].ColumnEdit = rps_txtNumber;

            grdview_ArchivTables.BestFitColumns();
        }
        private void Read_dest()
        {
            string serverTo = cmb_destServer.Text.ToString();
            string DbTo = cmb_destDb.Text.ToString();
            string schemaTo = cmb_destSchema.Text.ToString();
            if (string.IsNullOrEmpty(serverTo) || string.IsNullOrEmpty(DbTo) || string.IsNullOrEmpty(schemaTo) || schemaTo == " Select Schema ...")
            {
                MessageBox.Show("Destination Parameteres not complete (To Server, To Database or To Schema)");
                return;
            }
            // Read Source Tables-Name
            Cls_ReadFromDb readDestTables = new Cls_ReadFromDb(serverTo, DbTo, schemaTo);
            DataTable dt_destTables = readDestTables.GetTables();
            foreach (DataRow gridRo in dt_archiv.Rows)
            {
                foreach (DataRow destRo in dt_destTables.Rows)
                {
                    if (destRo["Table Name"].ToString() == gridRo["From Table"].ToString() + "_Archiv")
                    {
                        gridRo["To Table"] = destRo["Table Name"].ToString();
                        gridRo["To Schema"] = destRo["Schema"].ToString();
                        gridRo["Rows Count"] = destRo["Rowcount"].ToString();
                        gridRo["Rows Count"] = destRo["Rowcount"].ToString();
                        string rslt = Check_columns(gridRo); // CHECk Columns zwischen source und dest.
                        if (rslt == "Ready..")
                        {
                            gridRo["Process details"] = "";
                            gridRo["Result"] = "Ready..";
                        }
                        else
                        {
                            gridRo["Result"] = "Not Ready";
                            gridRo["Process details"] = rslt;
                        }
                        goto found;
                    }
                }
                gridRo["To Schema"] = schemaTo;
                gridRo["To Table"] = "New ...";
                gridRo["Result"] = "Ready..";
                gridRo["Process details"] = "";
            found:;
                grdview_ArchivTables.Columns["Total Rows"].ColumnEdit = rps_txtNumber;
                grdview_ArchivTables.Columns["Filtered Rows"].ColumnEdit = rps_txtNumber;
                grdview_ArchivTables.Columns["Rows Count"].ColumnEdit = rps_txtNumber;
                grdview_ArchivTables.BestFitColumns();
            }
        }
        string Check_columns(DataRow dt_Ro)
        {
            string serverFrom = cmb_srcServer.Text.ToString();
            string DbFrom = cmb_srcDb.Text.ToString();
            string schemaFrom = cmb_srcSchema.Text.ToString();
            string serverTo = cmb_destServer.Text.ToString();
            string DbTo = cmb_destDb.Text.ToString();
            string schemaTo = cmb_destSchema.Text.ToString();
            try
            {
                // this Method: check the differance between source table and destination table
                string fromTable = dt_Ro["From Table"].ToString();
                string toTable = dt_Ro["To Table"].ToString();
                string fromSchema = dt_Ro["From Schema"].ToString();
                string toSchema = dt_Ro["To Schema"].ToString();
                if (toTable == "New ...")
                {
                    return "Ready..";
                }
                Cls_ReadFromTable clsFrom = new Cls_ReadFromTable(serverFrom, DbFrom, fromSchema, fromTable);
                Cls_ReadFromTable clsTo = new Cls_ReadFromTable(serverTo, DbTo, toSchema, toTable);

                List<string> lst_fromNameAndType = clsFrom.GetTableColumns_NamesAndTypes();
                List<string> lst_ToNameAndType = clsTo.GetTableColumns_NamesAndTypes();
                lst_fromNameAndType.Add("Archive_User (nvarchar)");
                lst_fromNameAndType.Add("Archive_Date (datetime)");

                // Check Columns Count
                if (lst_fromNameAndType.Count != lst_ToNameAndType.Count)
                {
                    return "Columns Count Not equal";
                }
                // Check the columns and typs from Source table if exist in the destinatin Table
                foreach (string fromStr in lst_fromNameAndType)
                {
                    if (!lst_ToNameAndType.Contains(fromStr))
                    {
                        return ("ColumnName Or type (" + fromStr + ") from source-table not found in destination-table.");
                    }
                }
                // Check the columns and typs from destinatin table if exist in the Source Table
                foreach (string ToStr in lst_ToNameAndType)
                {
                    if (!lst_fromNameAndType.Contains(ToStr))
                    {
                        return ("ColumnName Or type (" + ToStr + ") from destination-table not found in source-table.");
                    }
                }
                // When no differance between source table and destination table then return ok
                return "Ready..";
            }
            catch (Exception ex)
            {
                return "Error bei Comparing Columns: " + ex.ToString();
            }
        }
        private void Tst_Condition()
        {
            string serverFrom = cmb_srcServer.Text.ToString();
            string DbFrom = cmb_srcDb.Text.ToString();
            for (int gridRo = 0; gridRo < grdview_ArchivTables.RowCount; gridRo++)
            {
                if (grdview_ArchivTables.GetRowCellValue(gridRo, "Archive Type").ToString() != "Nothing")
                {
                    try
                    {
                        string fromTable = grdview_ArchivTables.GetRowCellValue(gridRo, "From Table").ToString();
                        string fromSchema = grdview_ArchivTables.GetRowCellValue(gridRo, "From Schema").ToString();
                        if (fromTable == "New ..." || fromSchema == "" || fromSchema == " Select Schema ...")
                            continue;
                        string condition = grdview_ArchivTables.GetRowCellValue(gridRo, "Condition").ToString();
                        string[] values = new string[20];
                        for (int i = 0; i < grdview_Werte.RowCount; i++)
                        {
                            values[i] = grdview_Werte.GetRowCellValue(i, "Wert").ToString();
                        }

                        string conditionStr = string.Format(condition, values);
                        Cls_ReadFromTable clsFrom = new Cls_ReadFromTable(serverFrom, DbFrom, fromSchema, fromTable);
                        string rowsCnt = clsFrom.GetTable_RowsCount(conditionStr);
                        grdview_ArchivTables.SetRowCellValue(gridRo, "Filtered Rows", rowsCnt);
                        grdview_ArchivTables.SetRowCellValue(gridRo, "Result", "Ready..");
                    }
                    catch (Exception)
                    {
                        grdview_ArchivTables.SetRowCellValue(gridRo, "Filtered Rows", "falsche Condition");
                        grdview_ArchivTables.SetRowCellValue(gridRo, "Result", "Not Ready");
                    }
                }
            }
        }
        private void Do_Archive(int gridRo, bool sollDel)
        {
            string serverFrom = cmb_srcServer.Text.ToString();
            string DbFrom = cmb_srcDb.Text.ToString();
            string schemaFrom = cmb_srcSchema.Text.ToString();
            string serverTo = cmb_destServer.Text.ToString();
            string DbTo = cmb_destDb.Text.ToString();
            string schemaTo = cmb_destSchema.Text.ToString();

            string fromTable = grdview_ArchivTables.GetRowCellValue(gridRo, "From Table").ToString();
            string toTable = grdview_ArchivTables.GetRowCellValue(gridRo, "To Table").ToString();
            string fromSchema = grdview_ArchivTables.GetRowCellValue(gridRo, "From Schema").ToString();
            string toSchema = grdview_ArchivTables.GetRowCellValue(gridRo, "To Schema").ToString();
            string condition = grdview_ArchivTables.GetRowCellValue(gridRo, "Condition").ToString();
            string note = grdview_ArchivTables.GetRowCellValue(gridRo, "Note").ToString();

            string[] values = new string[20];
            for (int i = 0; i < grdview_Werte.RowCount; i++)
            {
                values[i] = grdview_Werte.GetRowCellValue(i, "Wert").ToString();
            }

            string conditionStr = string.Format(condition, values);
            string result_msg = "";

            // Begin Transfer Proccess 
            string TranResult = "";
            DateTime strt = DateTime.Now;
            grdview_ArchivTables.SetRowCellValue(gridRo, "Process details", 0);
            Application.DoEvents();
            DataTable source_Dt = new DataTable();
            Cls_ArchiveTables transfer_cls = new Cls_ArchiveTables(serverFrom, DbFrom, fromSchema, fromTable, serverTo, DbTo, toSchema, toTable, conditionStr);
            try
            {
                grdview_ArchivTables.SetRowCellValue(gridRo, "Process details", 1);
                Application.DoEvents();
                if (toTable == "New ...")// the destination table is NOT exist
                {
                    toTable = fromTable + "_Archiv";
                    //Step 1 : Build Source DataTable
                    transfer_cls = new Cls_ArchiveTables(serverFrom, DbFrom, fromSchema, fromTable, serverTo, DbTo, toSchema, toTable, conditionStr);
                    result_msg = transfer_cls.Read_TableSchema();
                    TranResult = "Step1: " + result_msg;
                    grdview_ArchivTables.SetRowCellValue(gridRo, "Process details", TranResult);
                    grdview_ArchivTables.SetRowCellValue(gridRo, "Result", 2);
                    Application.DoEvents();
                    if (result_msg == "Done")
                    {
                        //Step 2: Check Destination Table, when not exist => create it
                        result_msg = transfer_cls.Create_Table();
                        TranResult = TranResult + ", Step2: " + result_msg;
                        grdview_ArchivTables.SetRowCellValue(gridRo, "Process details", TranResult);
                        grdview_ArchivTables.SetRowCellValue(gridRo, "Result", 3);
                        Application.DoEvents();
                        if (result_msg == "Done")
                        {
                            //Step 3 : Save Destination Data.
                            result_msg = transfer_cls.Save_Dest_data(sollDel);
                            grdview_ArchivTables.SetRowCellValue(gridRo, "Process details", "Transfer Done");
                            Application.DoEvents();
                            TranResult = TranResult + ", Step3: " + result_msg;
                            grdview_ArchivTables.SetRowCellValue(gridRo, "Process details", TranResult);
                            grdview_ArchivTables.SetRowCellValue(gridRo, "To Table", toTable);
                            if (result_msg == "No Change" || result_msg.Substring(0, 4) == "Erro")
                            {
                                grdview_ArchivTables.SetRowCellValue(gridRo, "Result", "Error");
                            }
                            else
                            {
                                grdview_ArchivTables.SetRowCellValue(gridRo, "Result", "Done");
                            }
                        }
                    }
                }
                else // the destination table is exist
                {
                    //Step 3 : Save Destination Data.
                    result_msg = transfer_cls.Save_Dest_data(sollDel);
                    grdview_ArchivTables.SetRowCellValue(gridRo, "Process details", "Transfer Done");
                    Application.DoEvents();
                    TranResult = TranResult + "Step3: " + result_msg;
                    grdview_ArchivTables.SetRowCellValue(gridRo, "Process details", TranResult);
                    if (result_msg == "No Change" || result_msg.Substring(0, 4) == "Erro")
                    {
                        grdview_ArchivTables.SetRowCellValue(gridRo, "Result", "Error");
                    }
                    else
                    {
                        grdview_ArchivTables.SetRowCellValue(gridRo, "Result", "Done");
                    }
                }
            }
            catch (Exception ex)
            {
                grdview_ArchivTables.SetRowCellValue(gridRo, "Process details", "Err: " + ex.Message);
                grdview_ArchivTables.SetRowCellValue(gridRo, "Result", "Error");
            }
            DateTime endt = DateTime.Now;
            var time_def = endt - strt;
            SaveTableTransfer_To_ArchivResult(gridRo, time_def.ToString());
            Application.DoEvents();
            //}
        }
        private void DeleteRows(int gridRo)
        {
            string serverFrom = cmb_srcServer.Text.ToString();
            string DbFrom = cmb_srcDb.Text.ToString();
            string schemaFrom = cmb_srcSchema.Text.ToString();
            string serverTo = cmb_destServer.Text.ToString();
            string DbTo = cmb_destDb.Text.ToString();
            string schemaTo = cmb_destSchema.Text.ToString();

            string fromTable = grdview_ArchivTables.GetRowCellValue(gridRo, "From Table").ToString();
            string toTable = grdview_ArchivTables.GetRowCellValue(gridRo, "To Table").ToString();
            string fromSchema = grdview_ArchivTables.GetRowCellValue(gridRo, "From Schema").ToString();
            string toSchema = grdview_ArchivTables.GetRowCellValue(gridRo, "To Schema").ToString();
            string condition = grdview_ArchivTables.GetRowCellValue(gridRo, "Condition").ToString();
            string note = grdview_ArchivTables.GetRowCellValue(gridRo, "Note").ToString();
            DateTime strt = DateTime.Now;

            string[] values = new string[20];
            for (int i = 0; i < grdview_Werte.RowCount; i++)
            {
                values[i] = grdview_Werte.GetRowCellValue(i, "Wert").ToString();
            }
            string conditionStr = string.Format(condition, values);
            if (fromTable == "New ..." || fromSchema == "" || fromSchema == " Select Schema ...")
                return;
            if (toTable == "New ..." || toSchema == "" || toSchema == " Select Schema ...")
                return;
            try
            {
                // Fill dtatable with rows to delete (with condition)
                Cls_ArchiveTables transfer_cls = new Cls_ArchiveTables(serverFrom, DbFrom, fromSchema, fromTable, serverTo, DbTo, toSchema, toTable, conditionStr);
                DataTable dt_destRows = transfer_cls.Fill_destDataTable();

                Cls_ReadFromDb readPK = new Cls_ReadFromDb(serverFrom, DbFrom, fromSchema);
                string srcPk = readPK.Get_primaryKey(fromTable);
                int del_count = 0;

                foreach (DataRow Ro in dt_destRows.Rows)
                {
                    string cond = " WHERE " + srcPk + " = " + Ro[srcPk];
                    string sel_str = transfer_cls.deleteRow(cond);
                    if (sel_str == "Done") del_count += 1;
                }
                grdview_ArchivTables.SetRowCellValue(gridRo, "Process details", "Del. Rows: " + del_count.ToString());
                grdview_ArchivTables.SetRowCellValue(gridRo, "Result", "Done");
                DateTime endt = DateTime.Now;
                var time_def = endt - strt;
                SaveTableTransfer_To_ArchivResult(gridRo, time_def.ToString());
                Application.DoEvents();
            }
            catch (Exception)
            {
            }
        }

        private void SaveTableTransfer_To_ArchivResult(int gridro, string period)
        {
            string src_server = cmb_srcServer.Text.ToString();
            string src_Db = cmb_srcDb.Text.ToString();
            string dst_server = cmb_destServer.Text.ToString();
            string dst_Db = cmb_destDb.Text.ToString();

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string connString = "Data Source=" + dst_server + "; Integrated Security=True;Initial Catalog= " + dst_Db + ";Connection Timeout=0";

            string condition = grdview_ArchivTables.GetRowCellValue(gridro, "Condition").ToString();
            string[] values = new string[20];
            for (int i = 0; i < grdview_Werte.RowCount; i++)
            {
                values[i] = grdview_Werte.GetRowCellValue(i, "Wert").ToString();
            }

            string conditionStr = string.Format(condition, values);
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    string dest_schema = grdview_ArchivTables.GetRowCellValue(gridro, "To Schema").ToString();
                    string dest_table = grdview_ArchivTables.GetRowCellValue(gridro, "To Table").ToString();
                    string str = @"INSERT INTO " +
                 "ArchivResult(src_server, src_db, src_schema, src_TableName, src_rowsCount, condition, archivedRows, dst_server, dst_db, dst_schema, dst_TableName, dst_rowsCount, archive_Type, note, trsf_status, trsf_result, trsf_time, trsf_user, trsf_period ) " +
                 " VALUES    (@src_server,@src_db,@src_schema,@src_TableName,@src_rowsCount,@condition,@archivedRows,@dst_server,@dst_db,@dst_schema,@dst_TableName,@dst_rowsCount,@archive_Type,@note,@trsf_status,@trsf_result,@trsf_time,@trsf_user,@trsf_period)";
                    SqlCommand cmd = new SqlCommand(str, con);
                    cmd.Parameters.AddWithValue("@src_server", src_server);
                    cmd.Parameters.AddWithValue("@src_db", src_Db);
                    cmd.Parameters.AddWithValue("@src_schema", grdview_ArchivTables.GetRowCellValue(gridro, "From Schema").ToString());
                    cmd.Parameters.AddWithValue("@src_TableName", grdview_ArchivTables.GetRowCellValue(gridro, "From Table").ToString());
                    cmd.Parameters.AddWithValue("@src_rowsCount", grdview_ArchivTables.GetRowCellValue(gridro, "Total Rows").ToString());
                    cmd.Parameters.AddWithValue("@condition", conditionStr);
                    cmd.Parameters.AddWithValue("@archivedRows", grdview_ArchivTables.GetRowCellValue(gridro, "Filtered Rows").ToString());
                    cmd.Parameters.AddWithValue("@dst_server", dst_server);
                    cmd.Parameters.AddWithValue("@dst_db", dst_Db);
                    cmd.Parameters.AddWithValue("@dst_schema", grdview_ArchivTables.GetRowCellValue(gridro, "To Schema").ToString());
                    cmd.Parameters.AddWithValue("@dst_TableName", grdview_ArchivTables.GetRowCellValue(gridro, "To Table").ToString());
                    cmd.Parameters.AddWithValue("@dst_rowsCount", grdview_ArchivTables.GetRowCellValue(gridro, "Rows Count").ToString());
                    cmd.Parameters.AddWithValue("@archive_Type", grdview_ArchivTables.GetRowCellValue(gridro, "Archive Type").ToString());
                    cmd.Parameters.AddWithValue("@note", grdview_ArchivTables.GetRowCellValue(gridro, "Note").ToString());
                    cmd.Parameters.AddWithValue("@trsf_status", grdview_ArchivTables.GetRowCellValue(gridro, "Process details").ToString()); // = Steps, Error details ...
                    cmd.Parameters.AddWithValue("@trsf_result", grdview_ArchivTables.GetRowCellValue(gridro, "Result").ToString()); // = Error Or Done
                    cmd.Parameters.AddWithValue("@trsf_time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@trsf_user", userName);
                    cmd.Parameters.AddWithValue("@trsf_period", period);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion ****** Methodes ********

        #region ******* Grid events ******
        private void Grdview_ArchivTables_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                // Coloring Rows (Nothing)
                if (view.GetRowCellValue(e.RowHandle, "Archive Type").ToString() == "Nothing")
                    e.Appearance.BackColor = Color.White;
                // Coloring Rows (Archive without Delete)
                if (view.GetRowCellValue(e.RowHandle, "Archive Type").ToString() == "Archive without Delete")
                    e.Appearance.BackColor = Color.LightYellow;
                // Coloring Rows (Archive with Delete)
                if (view.GetRowCellValue(e.RowHandle, "Archive Type").ToString() == "Archive and Delete")
                    e.Appearance.BackColor = Color.LightSteelBlue;
                // Coloring Rows (Archive with Delete)
                if (view.GetRowCellValue(e.RowHandle, "Archive Type").ToString() == "Delete without Archive")
                    e.Appearance.BackColor = Color.LightSalmon;
                // Coloring Rows (Done)
                if (view.GetRowCellValue(e.RowHandle, "Result").ToString() == "Done")
                    e.Appearance.BackColor = Color.LawnGreen;
                // Coloring Rows (Error)
                if (view.GetRowCellValue(e.RowHandle, "Result").ToString() == "Error" || view.GetRowCellValue(e.RowHandle, "Process details").ToString() == "Error")
                    e.Appearance.BackColor = Color.OrangeRed;
                // Coloring Rows (Not Ready)
                if (view.GetRowCellValue(e.RowHandle, "Result").ToString() == "Not Ready")
                    e.Appearance.BackColor = Color.LightPink;

            }
        }
        private void Grdview_ArchivTables_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == "Process details")
            {
                rps_ProgressBar_step.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
                rps_ProgressBar_step.ShowTitle = true;
                rps_ProgressBar_step.PercentView = false;
                rps_ProgressBar_step.Minimum = 0;
                rps_ProgressBar_step.Maximum = 4;
                e.RepositoryItem = rps_ProgressBar_step;
            }
        }
        private void Grdview_ArchivTables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                if (MessageBox.Show("Delete this Row !!???", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    grdview_ArchivTables.DeleteSelectedRows();
                }
            }
        }
        private void Grdview_ArchivTables_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.CellValue == null) return;

            if (e.Column.Name == "colFromTable" && e.Button == MouseButtons.Right)
            {
                string tblName = e.CellValue.ToString();
                string serverFromName = cmb_srcServer.Text.ToString();
                string DbFromName = cmb_srcDb.Text.ToString();
                string Fromschema = cmb_srcSchema.Text.ToString();
                Cls_DialogColumns myControl = new Cls_DialogColumns(serverFromName, DbFromName, Fromschema, tblName);
                DevExpress.XtraEditors.XtraDialog.Show(myControl, "Table: " + tblName, MessageBoxButtons.OK);
            }
            if (e.Column.Name == "colToTable" && e.Button == MouseButtons.Right)
            {
                string tblName = e.CellValue.ToString();
                string serverToName = cmb_destServer.Text.ToString();
                string DbToName = cmb_destDb.Text.ToString();
                string Toschema = cmb_destSchema.Text.ToString();
                Cls_DialogColumns myControl = new Cls_DialogColumns(serverToName, DbToName, Toschema, tblName);
                DevExpress.XtraEditors.XtraDialog.Show(myControl, "Table: " + tblName, MessageBoxButtons.OK);
            }
        }
        private void Grdview_ArchivTables_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        private void grdview_ArchivTables_ClipboardRowPasting(object sender, ClipboardRowPastingEventArgs e)
        {
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            var cells = view.GetSelectedCells() as DevExpress.XtraGrid.Views.Base.GridCell[];
            if (cells.Length < 1 || e.Values.Count > 1 || System.Windows.Forms.Clipboard.GetText().Contains(System.Environment.NewLine))
                return;
            e.Cancel = true;
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].Column.OptionsColumn.AllowEdit != false)
                {
                    view.SetRowCellValue(cells[i].RowHandle, cells[i].Column, e.OriginalValues[0]);
                }
            }
        }
        private void grdview_Werte_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = "Val {" + (e.RowHandle).ToString() + "}";
        }
        #endregion ******* Grid events ******

        #region ****** XML ********
        private void LoadXml(string filePath)
        {
            Build_Grids();
            dt_archiv.TableName = "Table Archive";
            //////// Read from disk
            dt_archiv.ReadXml(filePath);
            Cntrl_ArchivTables.DataSource = dt_archiv;
            //  Read XML data:
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            XmlNodeList nodes = xml.SelectNodes("//Source_Parameters");
            foreach (XmlNode srcNode in nodes)
            {
                cmb_srcServer.Properties.Items.Add(srcNode.SelectSingleNode("FromServer").InnerText);
                cmb_srcDb.Properties.Items.Add(srcNode.SelectSingleNode("FromDB").InnerText);
                cmb_srcSchema.Properties.Items.Add(srcNode.SelectSingleNode("FromSchema").InnerText);
                cmb_srcServer.Text = srcNode.SelectSingleNode("FromServer").InnerText;
                cmb_srcDb.Text = srcNode.SelectSingleNode("FromDB").InnerText;
                cmb_srcSchema.Text = srcNode.SelectSingleNode("FromSchema").InnerText;
            }
            nodes = xml.SelectNodes("//Destination_Parameters");
            foreach (XmlNode dstNode in nodes)
            {
                cmb_destServer.Properties.Items.Add(dstNode.SelectSingleNode("ToServer").InnerText);
                cmb_destDb.Properties.Items.Add(dstNode.SelectSingleNode("ToDB").InnerText);
                cmb_destSchema.Properties.Items.Add(dstNode.SelectSingleNode("ToSchema").InnerText);
                cmb_destServer.Text = dstNode.SelectSingleNode("ToServer").InnerText;
                cmb_destDb.Text = dstNode.SelectSingleNode("ToDB").InnerText;
                cmb_destSchema.Text = dstNode.SelectSingleNode("ToSchema").InnerText;
            }
            // ReLoad data and test
            foreach (DataRow ro in dt_archiv.Rows)
            {
                Cls_ReadFromTable clsFrom = new Cls_ReadFromTable(cmb_srcServer.Text, cmb_srcDb.Text, ro["From Schema"].ToString(), ro["From Table"].ToString());
                string rowsCnt = clsFrom.GetTable_RowsCount("");
                ro["Total Rows"] = rowsCnt;
            }
            Read_dest();

            // Load Wert Grid
            filePath = filePath.Replace(".xml", "_Wert.wrt");
            if (File.Exists(filePath))
            {
                DataTable dt_wert = (DataTable)Cntrl_Werte.DataSource;
                dt_wert.Rows.Clear();
                dt_wert.TableName = "Table Werte";
                dt_wert.ReadXml(filePath);
            }
            Tst_Condition();
        }
        private void SaveXml(string filePath)
        {
            string serverFrom = cmb_srcServer.Text.ToString();
            string DbFrom = cmb_srcDb.Text.ToString();
            string schemaFrom = cmb_srcSchema.Text.ToString();
            string serverTo = cmb_destServer.Text.ToString();
            string DbTo = cmb_destDb.Text.ToString();
            string schemaTo = cmb_destSchema.Text.ToString();
            // export grid to xml
            DataTable dt_xml = (DataTable)Cntrl_ArchivTables.DataSource;
            dt_xml.TableName = "Table Archive";
            dt_xml.WriteXml(filePath);

            // Create a new XML Nodes for Source_Parameters :
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            XmlElement newcatalogentry = xml.CreateElement("Source_Parameters");
            XmlElement firstelement = xml.CreateElement("FromServer");
            firstelement.InnerText = serverFrom;
            newcatalogentry.AppendChild(firstelement);
            XmlElement secondelement = xml.CreateElement("FromDB");
            secondelement.InnerText = DbFrom;
            newcatalogentry.AppendChild(secondelement);
            XmlElement thirdelement = xml.CreateElement("FromSchema");
            thirdelement.InnerText = schemaFrom;
            newcatalogentry.AppendChild(thirdelement);
            xml.DocumentElement.InsertAfter(newcatalogentry, xml.DocumentElement.LastChild);

            // Create a new XML Nodes for Destination_Parameters :
            newcatalogentry = xml.CreateElement("Destination_Parameters");
            firstelement = xml.CreateElement("ToServer");
            firstelement.InnerText = serverTo;
            newcatalogentry.AppendChild(firstelement);
            secondelement = xml.CreateElement("ToDB");
            secondelement.InnerText = DbTo;
            newcatalogentry.AppendChild(secondelement);
            thirdelement = xml.CreateElement("ToSchema");
            thirdelement.InnerText = schemaTo;
            newcatalogentry.AppendChild(thirdelement);
            xml.DocumentElement.InsertAfter(newcatalogentry, xml.DocumentElement.LastChild);

            xml.Save(filePath);

            // export WERT grid to xml
            filePath = filePath.Replace(".xml", "_Wert.wrt");
            DataTable dtWert_xml = (DataTable)Cntrl_Werte.DataSource;
            dtWert_xml.TableName = "Table Werte";
            dtWert_xml.WriteXml(filePath);


            //// Modify XML data :
            //XmlDocument xml1 = new XmlDocument();
            //xml1.Load("settings.xml");
            //XmlNodeList nodes = xml1.SelectNodes("//settings");
            //foreach (XmlElement element in nodes)
            //{
            //    element.SelectSingleNode("AlarmFileLocation").InnerText = "Modified";
            //    element.SelectSingleNode("AutoLogin").InnerText = "OFF";
            //    xml1.Save("settings.xml");
            //}

            //// Delete XML Childs
            //XmlDocument xml1222 = new XmlDocument();
            //xml1.Load("settings.xml");
            //XmlNodeList nodes22 = xml1222.SelectNodes("//settings");
            //foreach (XmlElement element in nodes22)
            //{
            //    element.RemoveAll();
            //}
            //xml1222.Save("settings.xml");
        }
        private void Btn_loadXmlFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML-File | *.xml"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                splashScreenManager1.ShowWaitForm();
                LoadXml(openFileDialog.FileName);
                splashScreenManager1.CloseWaitForm();
            }
        }
        private void Btn_saveXmlFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML-File | *.xml"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                splashScreenManager1.ShowWaitForm();
                SaveXml(saveFileDialog.FileName);
                splashScreenManager1.CloseWaitForm();
            }
        }
        #endregion ****** XML ********

        #region ****** Buttons Events *******
        private void Btn_readSrc_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            dt_archiv.Rows.Clear();
            Read_src();
            splashScreenManager1.CloseWaitForm();
        }
        private void Btn_readDest_Click(object sender, EventArgs e)
        {
            // Read dest. and check columns
            splashScreenManager1.ShowWaitForm();
            Read_dest();
            splashScreenManager1.CloseWaitForm();
        }
        private void Btn_addNewSchema_Click(object sender, EventArgs e)
        {
            string destServer = cmb_destServer.Text;
            string destDb = cmb_destDb.Text;

            if (string.IsNullOrEmpty(destServer) || string.IsNullOrEmpty(destDb))
            {
                MessageBox.Show("Not Enough Parameteres ...");
                return;
            }
            string schemaName = XtraInputBox.Show("Enter the Schema-Name ...", "Create Schema", "");
            if (string.IsNullOrEmpty(schemaName))
                return;

            string dest_connString = "Data Source=" + destServer + "; Integrated Security=True;Initial Catalog= " + cmb_destDb.Text + ";Connection Timeout=0";
            using (SqlConnection dest_con = new SqlConnection(dest_connString))
            {
                string str = @" IF NOT EXISTS ( SELECT  *
                FROM sys.schemas
             WHERE   name = N'" + schemaName + "' ) EXEC('CREATE SCHEMA " + schemaName + " AUTHORIZATION dbo')";
                using (SqlCommand cmd = new SqlCommand(str, dest_con))
                {
                    cmd.CommandText = str;
                    dest_con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Schema created successfully ...");
                }
            }
        }
        private void Btn_testCondition_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Tst_Condition();
            splashScreenManager1.CloseWaitForm();
        }
        private void Btn_doArchive_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmb_destServer.Text.ToString()) || string.IsNullOrEmpty(cmb_destDb.Text.ToString()))
            {
                return;
            }

            string src_server = cmb_srcServer.Text.ToString();
            string src_Db = cmb_srcDb.Text.ToString();
            string src_schema = cmb_srcSchema.Text.ToString();
            string dst_server = cmb_destServer.Text.ToString();
            string dst_Db = cmb_destDb.Text.ToString();
            string dst_schema = cmb_destSchema.Text.ToString();

            int total_archiv = 0;
            int success_archiv = 0;
            int error_archiv = 0;
            int notReady = 0;
            string _error_items = "  - ";
            string _notReady_items = "  - ";

            int arch_WithoutDelete = 0;
            int arch_AndDelete = 0;
            int delete_withoutArchiv = 0;

            Cls_CreateArchivTables CreatArchivTable = new Cls_CreateArchivTables();
            string archivRslt = CreatArchivTable.Create_ArchivResult_Table(cmb_destServer.Text, cmb_destDb.Text);
            //archivieren();
            for (int i = 0; i < grdview_ArchivTables.RowCount; i++)
            {
                if (grdview_ArchivTables.GetRowCellValue(i, "Archive Type").ToString() != "Nothing" && grdview_ArchivTables.GetRowCellValue(i, "Result").ToString() == "Ready..")
                {
                    string archiv_type = grdview_ArchivTables.GetRowCellValue(i, "Archive Type").ToString();
                    switch (archiv_type)
                    {
                        case "Archive without Delete":
                            Do_Archive(i, false);
                            arch_WithoutDelete += 1;
                            total_archiv += 1;
                            break;
                        case "Archive and Delete":
                            Do_Archive(i, true);
                            arch_AndDelete += 1;
                            total_archiv += 1;
                            break;
                        case "Delete without Archive":
                            DeleteRows(i);
                            delete_withoutArchiv += 1;
                            total_archiv += 1;
                            break;
                    }
                }
                // Email Strings 
                if (grdview_ArchivTables.GetRowCellValue(i, "Result").ToString() == "Done")
                {
                    success_archiv += 1;
                }
                if (grdview_ArchivTables.GetRowCellValue(i, "Result").ToString() == "Not Ready")
                {
                    notReady += 1;
                    _notReady_items = _notReady_items + "\r\n" + grdview_ArchivTables.GetRowCellValue(i, "Process details").ToString();
                }
                if (grdview_ArchivTables.GetRowCellValue(i, "Result").ToString() == "Error")
                {
                    error_archiv += 1;
                    _error_items = _error_items + "\r\n" + grdview_ArchivTables.GetRowCellValue(i, "Process details").ToString();
                }
            }

            if (total_archiv > 0)
            {
                string mailBody = @"From Server: " + src_server +
                   "   From DB: " + src_Db +
                   "   From Schema: " + src_schema + "\r\n" +
                   "To Server:   " + dst_server +
                   "   To Db: " + dst_Db +
                   "   To Schema: " + dst_schema + "\r\n" + "\r\n" +
                   "Total Archived: " + total_archiv.ToString() + " Tables." + "\r\n" +
                   "    Success Archived: " + success_archiv.ToString() + "\r\n" +
                   "    Error Archived: " + error_archiv.ToString() + _error_items + "\r\n" +
                   "    Not Ready To Archive: " + notReady.ToString() + _notReady_items + "\r\n" + "\r\n" +
                   "Archive Types ... " + "\r\n" +
                   "    Archive without Delete: " + arch_WithoutDelete.ToString() + "\r\n" +
                   "    Archive and Delete: " + arch_AndDelete.ToString() + "\r\n" +
                   "    Delete without Archive: " + delete_withoutArchiv.ToString() + "\r\n" +
                   "";
                Cls_SendEmail clsSendMail = new Cls_SendEmail();
                bool ifSendet = clsSendMail.SendMail(mailBody);
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(Application.StartupPath + @"\log\" + "mailsLog.txt", true))
                {
                    file.WriteLine("Archiv vom: " + DateTime.Now.ToString() + "\r\n" + mailBody + " SendMail Status: " + ifSendet.ToString() + "\r\n");
                }
            }
        }
        private void btn_New_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("New Archive ????", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                splashScreenManager1.ShowWaitForm();
                Build_Grids();
                if (!string.IsNullOrEmpty(Properties.Settings.Default.source_server_name.ToString()))
                {
                    cmb_srcServer.Text = Properties.Settings.Default.source_server_name.ToString();
                    cmb_srcDb.Text = Properties.Settings.Default.source_db_name.ToString();
                    cmb_srcSchema.Text = Properties.Settings.Default.source_schema_name.ToString();
                }
                if (!string.IsNullOrEmpty(Properties.Settings.Default.dest_server_name.ToString()))
                {
                    cmb_destServer.Text = Properties.Settings.Default.dest_server_name.ToString();
                    cmb_destDb.Text = Properties.Settings.Default.dest_db_name.ToString();
                    cmb_destSchema.Text = Properties.Settings.Default.dest_schema_name.ToString();
                }
                splashScreenManager1.CloseWaitForm();
            }
        }
        #endregion ****** Buttons Events *******

        #region ***** drg and drop ******
        public void HandleBehaviorDragDropEvents()
        {
            DragDropBehavior gridControlBehavior = behaviorManager1.GetBehavior<DragDropBehavior>(this.grdview_ArchivTables);
            gridControlBehavior.DragDrop += Behavior_DragDrop;
            gridControlBehavior.DragOver += Behavior_DragOver;
        }
        private void Behavior_DragOver(object sender, DragOverEventArgs e)
        {
            DragOverGridEventArgs args = DragOverGridEventArgs.GetDragOverGridEventArgs(e);
            e.InsertType = args.InsertType;
            e.InsertIndicatorLocation = args.InsertIndicatorLocation;
            e.Action = args.Action;
            Cursor.Current = args.Cursor;
            args.Handled = true;
        }
        private void Behavior_DragDrop(object sender, DevExpress.Utils.DragDrop.DragDropEventArgs e)
        {
            GridView targetGrid = e.Target as GridView;
            GridView sourceGrid = e.Source as GridView;
            if (e.Action == DragDropActions.None || targetGrid != sourceGrid)
                return;
            DataTable sourceTable = sourceGrid.GridControl.DataSource as DataTable;

            Point hitPoint = targetGrid.GridControl.PointToClient(Cursor.Position);
            GridHitInfo hitInfo = targetGrid.CalcHitInfo(hitPoint);

            int[] sourceHandles = e.GetData<int[]>();

            int targetRowHandle = hitInfo.RowHandle;
            int targetRowIndex = targetGrid.GetDataSourceRowIndex(targetRowHandle);

            List<DataRow> draggedRows = new List<DataRow>();
            foreach (int sourceHandle in sourceHandles)
            {
                int oldRowIndex = sourceGrid.GetDataSourceRowIndex(sourceHandle);
                DataRow oldRow = sourceTable.Rows[oldRowIndex];
                draggedRows.Add(oldRow);
            }

            int newRowIndex;
            switch (e.InsertType)
            {
                case InsertType.Before:
                    newRowIndex = targetRowIndex > sourceHandles[sourceHandles.Length - 1] ? targetRowIndex - 1 : targetRowIndex;
                    for (int i = draggedRows.Count - 1; i >= 0; i--)
                    {
                        DataRow oldRow = draggedRows[i];
                        DataRow newRow = sourceTable.NewRow();
                        newRow.ItemArray = oldRow.ItemArray;
                        sourceTable.Rows.Remove(oldRow);
                        sourceTable.Rows.InsertAt(newRow, newRowIndex);
                    }
                    break;
                case InsertType.After:
                    newRowIndex = targetRowIndex < sourceHandles[0] ? targetRowIndex + 1 : targetRowIndex;
                    for (int i = 0; i < draggedRows.Count; i++)
                    {
                        DataRow oldRow = draggedRows[i];
                        DataRow newRow = sourceTable.NewRow();
                        newRow.ItemArray = oldRow.ItemArray;
                        sourceTable.Rows.Remove(oldRow);
                        sourceTable.Rows.InsertAt(newRow, newRowIndex);
                    }
                    break;
                default:
                    newRowIndex = -1;
                    break;
            }
            int insertedIndex = targetGrid.GetRowHandle(newRowIndex);
            targetGrid.FocusedRowHandle = insertedIndex;
            targetGrid.SelectRow(targetGrid.FocusedRowHandle);
        }
        #endregion  ***** drg and drop ******
    }
}
