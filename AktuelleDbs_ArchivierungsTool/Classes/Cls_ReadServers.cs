using System.Collections.Generic;

public class Cls_ReadServers
{
    public List<string> listOfServers = new List<string>();
    public Cls_ReadServers()
    {
        listOfServers.Add("(local)");
        listOfServers.Add("DMSQL01");
        listOfServers.Add("DMSQL02");
    }
}
