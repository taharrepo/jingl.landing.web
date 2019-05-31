using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace backend.jingle.net.Logic
{
    public class Helper
    {
        //listid = 16817
        //url = https://us20.api.mailchimp.com/3.0/lists/16817
        //key = 5d37a4dcc7a09f5bb6b96efbebd41233-us20

        public class Hash
        {
            public static string Create(string value, string salt)
            {
                var valueBytes = KeyDerivation.Pbkdf2(
                                    password: value,
                                    salt: Encoding.UTF8.GetBytes(salt),
                                    prf: KeyDerivationPrf.HMACSHA512,
                                    iterationCount: 10000,
                                    numBytesRequested: 256 / 8);

                return Convert.ToBase64String(valueBytes);
            }

            public static bool Validate(string value, string salt, string hash)
                => Create(value, salt) == hash;
        }

        public class Salt
        {
            public static string Create()
            {
                byte[] randomBytes = new byte[128 / 8];
                using (var generator = RandomNumberGenerator.Create())
                {
                    generator.GetBytes(randomBytes);
                    return Convert.ToBase64String(randomBytes);
                }
            }
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public class EmailHelper
        {
            public static void SendEmail(string emailto, string fname, string reffcode)
            {
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
                EmailContent = EmailContent.Replace("{0}", fname).Replace("{1}", reffcode).Replace("{2}", reffcode);

                mailMessage.ReplyToList.Add(new MailAddress("jinglproject@gmail.com"));
                mailMessage.To.Add(new MailAddress(emailto));
                mailMessage.Bcc.Add(new MailAddress("guritno.dimaz@sophieparis.com"));
                mailMessage.Subject = "Welcome to Jingl!Upload Videomu untuk Dapatkan Rp 50.000";
                mailMessage.Body = EmailContent;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();

            }

            public static void SendEmailForgot(string emailto, string href)
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
                using (StreamReader SourceReader = System.IO.File.OpenText("jingleemailforgot.html"))
                {

                    builder.Append(SourceReader.ReadToEnd());

                }

                string EmailContent = builder.ToString();
                EmailContent = EmailContent.Replace("{hrefline}", href);

                mailMessage.ReplyToList.Add(new MailAddress("jinglproject@gmail.com"));
                mailMessage.To.Add(new MailAddress(emailto));
                mailMessage.Bcc.Add(new MailAddress("guritno.dimaz@sophieparis.com"));
                mailMessage.Subject = "Jing! - Permintaan reset password";
                mailMessage.Body = EmailContent;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();

            }
        }
   


    }
}
