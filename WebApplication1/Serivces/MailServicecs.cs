using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebApplication1.Serivces
{
    public class MailServicecs
    {
        private string mail_account = "anclrew1327";
        private string mail_password = "and14109";
        private string mail_email = "anclrew1327@gmail.com";
        #region 產生驗證碼
        public string GetAuthCode()
        {
            string[] Code = { "A", "B", "C", "D", "E","F","G","H","I","J","K","L","M","N","P","Q","R","S","T","U","V","W","X","Y","Z"
                            , "a", "b", "c", "d", "e","f","g","h","i","j","k","l","m","n","p","q","r","s","t","u","v","w","x","y","z"
                            ,"1", "2", "3", "4", "5","6","7","8","9" };
            string ValidateStr = string.Empty;
            Random rd = new Random();
            for(int i = 0; i < 10; i++)
            {
                ValidateStr += Code[rd.Next(Code.Count())];
            }
            return ValidateStr;
        }
        #endregion
        #region 修改驗證信
        public string ChangeEmail(string TempString, string UserName, string ValidateUrl)
        {
            TempString = TempString.Replace("{{UserName}}", UserName);
            TempString = TempString.Replace("{{ValidateUrl}}", ValidateUrl);
            return TempString;
        }
        #endregion
        #region 寄驗證信
        public void SendEmail(string MailBody,string ToMail)
        {
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com", 587);
            smtpServer.Credentials = new System.Net.NetworkCredential(mail_account, mail_password);
            smtpServer.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mail_email);
            mail.To.Add(ToMail);
            mail.Subject = "會員驗證信";
            mail.Body = MailBody;
            mail.IsBodyHtml = true;
            smtpServer.Send(mail);
        }
        #endregion
    }
}