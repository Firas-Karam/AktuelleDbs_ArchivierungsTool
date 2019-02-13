using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class Cls_DialogColumns : XtraUserControl
{

    public Cls_DialogColumns(string servername, string DbName, string schema, string table)
    {
        Panel pnl = new Panel();
        pnl.Dock = DockStyle.Fill;
        Cls_ReadFromTable readColumns = new Cls_ReadFromTable(servername, DbName, schema, table);
        DataTable dt_names = readColumns.GetTableColumns_Names();
        DevExpress.XtraGrid.GridControl grd_cntrl = new DevExpress.XtraGrid.GridControl();
        DevExpress.XtraGrid.Views.Grid.GridView grdview_names = new DevExpress.XtraGrid.Views.Grid.GridView(grd_cntrl);
        grdview_names.OptionsBehavior.Editable = false;
        grdview_names.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
        grdview_names.OptionsSelection.MultiSelect = true;
        grdview_names.OptionsClipboard.AllowCopy = DevExpress.Utils.DefaultBoolean.True;
        grdview_names.OptionsClipboard.CopyColumnHeaders = DevExpress.Utils.DefaultBoolean.False;
        grd_cntrl.Dock = DockStyle.Fill;
        grd_cntrl.DataSource = dt_names;
        grd_cntrl.MainView = grdview_names;
        pnl.Controls.Add(grd_cntrl);
        Label lbl_colmnsCount = new Label();
        lbl_colmnsCount.Dock = DockStyle.Bottom;
        lbl_colmnsCount.Text ="Columns Count: " + dt_names.Rows.Count.ToString();
        pnl.Controls.Add(lbl_colmnsCount);
        this.Controls.Add(pnl);
        this.Height = 500;
        this.Width = 400;
        this.Dock = DockStyle.Top;
    }

    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // Cls_DialogColumns
            // 
            this.Name = "Cls_DialogColumns";
            this.ResumeLayout(false);

    }
}

