using System.Data;

class Cls_Build_DataTables
{
    /// <summary>
    /// This Class Build DataTables for the Gridviews.
    /// </summary>
    /// <returns></returns>
    #region ***** Tables *****
    public DataTable Build_ArchivTables()
    {
        DataTable dtbl = new DataTable();
        dtbl.Columns.Add("Archive Type", typeof(string));
        dtbl.Columns.Add("From Schema", typeof(string));
        dtbl.Columns.Add("From Table", typeof(string));
        dtbl.Columns.Add("Total Rows", typeof(string));
        dtbl.Columns.Add("Condition", typeof(string));
        dtbl.Columns.Add("Filtered Rows", typeof(string));
        dtbl.Columns.Add("To Schema", typeof(string));
        dtbl.Columns.Add("To Table", typeof(string));
        dtbl.Columns.Add("Rows Count", typeof(string));
        dtbl.Columns.Add("Note", typeof(string));
        dtbl.Columns.Add("Process details", typeof(string));
        dtbl.Columns.Add("Result", typeof(string));
        return dtbl;
    }
    public DataTable Build_WerteTable()
    {
        DataTable dtbl = new DataTable();
        dtbl.Columns.Add("Wert", typeof(string));
        dtbl.Columns.Add("Beschreibung", typeof(string));
        return dtbl;
    }
    #endregion ****** Tables ********
}