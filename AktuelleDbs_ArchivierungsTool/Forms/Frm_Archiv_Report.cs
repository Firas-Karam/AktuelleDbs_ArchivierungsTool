using AktuelleDbs_ArchivierungsTool.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AktuelleDbs_ArchivierungsTool
{
    public partial class Frm_Archiv_Report : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Archiv_Report()
        {
            InitializeComponent();
        }

        private void Frm_Archiv_Report_Load(object sender, EventArgs e)
        {
 Cls_ReadServers readservers = new Cls_ReadServers();
            List<string> servers = readservers.listOfServers;
            Cmb_Server.Items.Clear();
            Cmb_Server.Items.AddRange(servers.ToArray());

            if (!string.IsNullOrEmpty(Properties.Settings.Default.archiv_server.ToString()))
            {
                Cmb_Server.Text = Properties.Settings.Default.archiv_server.ToString();
                Cmb_Db_Enter(null, null);
                Cmb_Db.Text = Properties.Settings.Default.archiv_db.ToString();
            }
            grdview_ReportDetails.ViewCaption = "";
        }

        private void Cmb_Server_Enter(object sender, EventArgs e)
        {
            Cmb_Db.Text = "";
            grdview_ReportDetails.ViewCaption = "";

        }

        private void Cmb_Db_Enter(object sender, EventArgs e)
        {
            Cmb_Db.Items.Clear();
            grdview_ReportDetails.ViewCaption = "";
            string serverName = Cmb_Server.Text.ToString();
            try
            {
                Cls_ReadDataBases readDatabases = new Cls_ReadDataBases(serverName);
                List<string> dataBases = readDatabases.listOfDatabases;
                Cmb_Db.Items.AddRange(dataBases.ToArray());
            }
            catch (Exception)
            {
                MessageBox.Show("Error Reading Source DB");
            }

        }

        private void Frm_Archiv_Report_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.archiv_server = Cmb_Server.Text.ToString();
            Properties.Settings.Default.archiv_db = Cmb_Db.Text.ToString();

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Reload();

        }

        private void Retrieve_Report()
        {
            Cntrl_ReportDetails.DataSource = null;
            grdview_ReportDetails.Columns.Clear();
            string serverName = Cmb_Server.Text.ToString();
            string DbName = Cmb_Db.Text.ToString();

            Cls_RetrieveReport cls_report = new Cls_RetrieveReport(serverName, DbName);
            DataTable dt_report = cls_report.Archiv_Table();

            if (dt_report == null) return;
            Cntrl_ReportDetails.DataSource = dt_report;
            grdview_ReportDetails.ViewCaption = "Server Name: " + serverName + "    DataBase Name: " + DbName;
            grdview_ReportDetails.Columns["trsf_time"].ColumnEdit = rps_dateTime;

        }
        private void Retrieve_Statistics()
        {
            string serverName = Cmb_Server.Text.ToString();
            string DbName = Cmb_Db.Text.ToString();
            Cls_RetrieveReport cls_reports = new Cls_RetrieveReport(serverName, DbName);
            List<string> lstof_satatistics = new List<string>();
            lstof_satatistics = cls_reports.Archiv_Statistics();
            lstbox_Statistics.DataSource = lstof_satatistics;
        }

        private void grdview_ReportDetails_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Retrieve_Report();
            Retrieve_Statistics();
            splashScreenManager1.CloseWaitForm();
        }
 
    }
}
