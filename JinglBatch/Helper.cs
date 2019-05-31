using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace JinglBatch
{
    class Helper
    {
        public static void SendEmail(string emailto, string fname, string reffcode)
        {
            ////AWS
            //SmtpClient smtpClient = new SmtpClient()
            //{
            //    Port = 587,
            //    Host = "email-smtp.us-west-2.amazonaws.com",
            //    EnableSsl = true,
            //    Timeout = 100000,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    Credentials = new NetworkCredential("AKIAI632FYJXCEZKYM7A", "BCA6BHSDc4nmwU1SH3MqClUhU4LXwBGES4kaXAvBSjtM")
            //    //UseDefaultCredentials = true,
            //    //Credentials = CredentialCache.DefaultNetworkCredentials
            //};

            //Mailjet
            SmtpClient smtpClient = new SmtpClient()
            {
                Port = 587,
                Host = "in-v3.mailjet.com",
                EnableSsl = true,
                Timeout = 100000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("1858349cd3d1f7c61fb86a7317a18220", "13fa34ef0fca56d224c7e4755872e38c")
                //UseDefaultCredentials = true,
                //Credentials = CredentialCache.DefaultNetworkCredentials
            };

            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress("admin@jingl.net", "Jingl Team")
            };

            StringBuilder builder = new StringBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText("jingleemailtemplate.html"))
            {

                builder.Append(SourceReader.ReadToEnd());

            }

            string EmailContent = builder.ToString();
            EmailContent = EmailContent.Replace("{0}", fname);

            mailMessage.ReplyToList.Add(new MailAddress("jinglproject@gmail.com"));
            mailMessage.To.Add(new MailAddress(emailto));
            mailMessage.Bcc.Add(new MailAddress("guritno.dimaz@sophieparis.com"));
            mailMessage.Subject = "Hai, jangan lupa upload video ke Jingl ya, Rp 100.000 menanti kamu!";
            mailMessage.Body = EmailContent;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);
            smtpClient.Dispose();

        }
    }
}
