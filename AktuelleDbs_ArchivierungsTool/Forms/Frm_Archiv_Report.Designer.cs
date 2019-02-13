namespace AktuelleDbs_ArchivierungsTool
{
    partial class Frm_Archiv_Report
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Archiv_Report));
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Cntrl_ReportDetails = new DevExpress.XtraGrid.GridControl();
            this.grdview_ReportDetails = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cmbRep_DestSchema = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.cmbRep_TableStatus = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.rps_dateTime = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstbox_Statistics = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Cmb_Server = new System.Windows.Forms.ComboBox();
            this.Cmb_Db = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::AktuelleDbs_ArchivierungsTool.Forms.WaitForm1), true, true);
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Cntrl_ReportDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdview_ReportDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRep_DestSchema)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRep_TableStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rps_dateTime)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1503, 762);
            this.panel2.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Cntrl_ReportDetails);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.ForeColor = System.Drawing.Color.OrangeRed;
            this.groupBox2.Location = new System.Drawing.Point(0, 45);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1282, 717);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ArchivResult Table";
            // 
            // Cntrl_ReportDetails
            // 
            this.Cntrl_ReportDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Cntrl_ReportDetails.Location = new System.Drawing.Point(3, 17);
            this.Cntrl_ReportDetails.MainView = this.grdview_ReportDetails;
            this.Cntrl_ReportDetails.Name = "Cntrl_ReportDetails";
            this.Cntrl_ReportDetails.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cmbRep_DestSchema,
            this.cmbRep_TableStatus,
            this.rps_dateTime});
            this.Cntrl_ReportDetails.Size = new System.Drawing.Size(1276, 697);
            this.Cntrl_ReportDetails.TabIndex = 4;
            this.Cntrl_ReportDetails.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdview_ReportDetails});
            // 
            // grdview_ReportDetails
            // 
            this.grdview_ReportDetails.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 10F);
            this.grdview_ReportDetails.Appearance.ViewCaption.ForeColor = System.Drawing.Color.SteelBlue;
            this.grdview_ReportDetails.Appearance.ViewCaption.Options.UseFont = true;
            this.grdview_ReportDetails.Appearance.ViewCaption.Options.UseForeColor = true;
            this.grdview_ReportDetails.GridControl = this.Cntrl_ReportDetails;
            this.grdview_ReportDetails.IndicatorWidth = 50;
            this.grdview_ReportDetails.Name = "grdview_ReportDetails";
            this.grdview_ReportDetails.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.grdview_ReportDetails.OptionsSelection.MultiSelect = true;
            this.grdview_ReportDetails.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.grdview_ReportDetails.OptionsView.ShowAutoFilterRow = true;
            this.grdview_ReportDetails.OptionsView.ShowViewCaption = true;
            this.grdview_ReportDetails.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.grdview_ReportDetails_CustomDrawRowIndicator);
            // 
            // cmbRep_DestSchema
            // 
            this.cmbRep_DestSchema.AutoHeight = false;
            this.cmbRep_DestSchema.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbRep_DestSchema.ImmediatePopup = true;
            this.cmbRep_DestSchema.Name = "cmbRep_DestSchema";
            this.cmbRep_DestSchema.Sorted = true;
            // 
            // cmbRep_TableStatus
            // 
            this.cmbRep_TableStatus.AutoHeight = false;
            this.cmbRep_TableStatus.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbRep_TableStatus.Items.AddRange(new object[] {
            "New",
            "Exist"});
            this.cmbRep_TableStatus.Name = "cmbRep_TableStatus";
            this.cmbRep_TableStatus.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // rps_dateTime
            // 
            this.rps_dateTime.AutoHeight = false;
            this.rps_dateTime.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            this.rps_dateTime.Mask.UseMaskAsDisplayFormat = true;
            this.rps_dateTime.Name = "rps_dateTime";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstbox_Statistics);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.ForeColor = System.Drawing.Color.OrangeRed;
            this.groupBox1.Location = new System.Drawing.Point(1282, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(221, 717);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Statistics From Archiv-DataBase";
            // 
            // lstbox_Statistics
            // 
            this.lstbox_Statistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstbox_Statistics.FormattingEnabled = true;
            this.lstbox_Statistics.Location = new System.Drawing.Point(3, 17);
            this.lstbox_Statistics.Name = "lstbox_Statistics";
            this.lstbox_Statistics.Size = new System.Drawing.Size(215, 697);
            this.lstbox_Statistics.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.Cmb_Server);
            this.panel1.Controls.Add(this.Cmb_Db);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1503, 45);
            this.panel1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(1420, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 21);
            this.button2.TabIndex = 15;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(712, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 21);
            this.button1.TabIndex = 14;
            this.button1.Text = "Do ...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Cmb_Server
            // 
            this.Cmb_Server.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_Server.FormattingEnabled = true;
            this.Cmb_Server.Location = new System.Drawing.Point(79, 12);
            this.Cmb_Server.Name = "Cmb_Server";
            this.Cmb_Server.Size = new System.Drawing.Size(248, 21);
            this.Cmb_Server.Sorted = true;
            this.Cmb_Server.TabIndex = 9;
            this.Cmb_Server.Enter += new System.EventHandler(this.Cmb_Server_Enter);
            // 
            // Cmb_Db
            // 
            this.Cmb_Db.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_Db.FormattingEnabled = true;
            this.Cmb_Db.Location = new System.Drawing.Point(424, 12);
            this.Cmb_Db.Name = "Cmb_Db";
            this.Cmb_Db.Size = new System.Drawing.Size(248, 21);
            this.Cmb_Db.Sorted = true;
            this.Cmb_Db.TabIndex = 13;
            this.Cmb_Db.Enter += new System.EventHandler(this.Cmb_Db_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Archiv-Server:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(332, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Archiv-DataBase:";
            // 
            // splashScreenManager1
            // 
            this.splashScreenManager1.ClosingDelay = 500;
            // 
            // Frm_Archiv_Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1503, 762);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Frm_Archiv_Report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frm_Archiv_Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Archiv_Report_FormClosing);
            this.Load += new System.EventHandler(this.Frm_Archiv_Report_Load);
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Cntrl_ReportDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdview_ReportDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRep_DestSchema)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRep_TableStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rps_dateTime)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraGrid.GridControl Cntrl_ReportDetails;
        private DevExpress.XtraGrid.Views.Grid.GridView grdview_ReportDetails;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cmbRep_DestSchema;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cmbRep_TableStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstbox_Statistics;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox Cmb_Server;
        private System.Windows.Forms.ComboBox Cmb_Db;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rps_dateTime;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
    }
}