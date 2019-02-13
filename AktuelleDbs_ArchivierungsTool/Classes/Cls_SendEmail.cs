using System;
using System.Windows.Forms;
using MailBee.SmtpMail;

namespace AktuelleDbs_ArchivierungsTool
{ 
class Cls_SendEmail
{
    /// <summary>
    /// This Class to send E-Mail with Transfer-Results.
    /// The E-Mail Adresses are alles Parameteres saved in the Soulution-Settings under Names:
    /// mailadress_from, mailadress_to, mailadress_cc
    /// </summary>
    /// <param name="mailBody"></param>
    /// <returns></returns>
    public bool SendMail(string mailBody)
    {
        bool ifMailgesendet = false;
        Smtp oMailer = new Smtp("MN110-539B64549A169B5F9B3E01B283A3-F80C");
        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            try
            {
            oMailer.DnsServers.Autodetect();
            oMailer.SmtpServers.Add("smtp.local.combera.com");
            oMailer.SmtpServers[0].SmtpOptions = ExtendedSmtpOptions.NoChunking;
            oMailer.Log.Enabled = true;
            oMailer.Log.Filename = Application.StartupPath + @"\log\" + "log.txt";
            oMailer.Log.Clear();
            oMailer.From.Email = Properties.Settings.Default.mailadress_from;
            if (oMailer.To.Count != 0)
                oMailer.To.Clear();
            if (oMailer.Cc.Count != 0)
                oMailer.Cc.Clear();
            oMailer.To.AddFromString(Properties.Settings.Default.mailadress_to);
            oMailer.Cc.Add(Properties.Settings.Default.mailadress_cc);
            oMailer.Subject = "Aktuelle Archiv vom: " + DateTime.Now.ToString() + " bei: " + userName;
            oMailer.BodyPlainText = mailBody;
            oMailer.Send();
            ifMailgesendet = true;
        }
        catch (Exception)
        {
            ifMailgesendet = false;
        }
        return ifMailgesendet;
    }
}
}
 
