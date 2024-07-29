using System.Net.Mail;

namespace StayHub.Services
{
    public class EmailService
    {
        public bool SendEmail(string body, string toEmail, string subject, List<Attachment> Attachment, string ccEmail = "", string bccEmail = "")
        {
            try
            {
                string user = string.Empty;
                string Passowrd = string.Empty;
                string From = string.Empty;
                string host = string.Empty;
                int port = 587;

                MailMessage mail = new MailMessage();
                mail.To.Add(toEmail);

                mail.From = new MailAddress(From);
                mail.Subject = subject;

                if (Attachment != null && Attachment.Count > 0)
                {
                    foreach (Attachment item in Attachment)
                    {
                        mail.Attachments.Add(item);
                    }
                }

                if (!string.IsNullOrEmpty(ccEmail))
                {
                    string[] ccid = ccEmail.Split(',');
                    foreach (string item in ccid)
                    {
                        mail.CC.Add(new MailAddress(item));
                    }
                }

                if (!string.IsNullOrEmpty(bccEmail))
                {
                    string[] bccid = bccEmail.Split(',');
                    foreach (string item in bccid)
                    {
                        mail.Bcc.Add(new MailAddress(item));
                    }
                }

                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(host, port)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(user, Passowrd),
                    Timeout = int.MaxValue
                };
                smtp.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                //throw e;
                return false;
            }
        }

        public bool SendEmail(string body, List<string> toEmail, string subject, List<Attachment> Attachment, string ccEmail = "", string bccEmail = "")
        {
            try
            {
                string user = string.Empty;
                string Passowrd = string.Empty;
                string From = string.Empty;
                string host = string.Empty;
                int port = 587;

                MailMessage mail = new MailMessage();
                foreach (string item in toEmail)
                {
                    mail.To.Add(item);
                }

                mail.From = new MailAddress(From);
                mail.Subject = subject;

                if (Attachment != null && Attachment.Count > 0)
                {
                    foreach (Attachment item in Attachment)
                    {
                        mail.Attachments.Add(item);
                    }
                }

                if (!string.IsNullOrEmpty(ccEmail))
                {
                    string[] ccid = ccEmail.Split(',');
                    foreach (string item in ccid)
                    {
                        mail.CC.Add(new MailAddress(item));
                    }
                }

                if (!string.IsNullOrEmpty(bccEmail))
                {
                    string[] bccid = bccEmail.Split(',');
                    foreach (string item in bccid)
                    {
                        mail.Bcc.Add(new MailAddress(item));
                    }
                }

                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(host, port)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(user, Passowrd),
                    Timeout = int.MaxValue
                };
                smtp.Send(mail);

                return true;
            }
            catch (Exception e)
            {
                
                return false;
            }
        }

    }
}
