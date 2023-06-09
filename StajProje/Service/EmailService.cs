using StajProje.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace StajProje.Service
{
    public class EmailService
    {
        private readonly ApplicationDbContext _context;
        public EmailService(ApplicationDbContext context)
        {
            _context = context;
        }
        List<int> arr = new List<int>();
        public void emailTest()
        {
            using (MailMessage mm = new MailMessage("keskinmurat888@gmail.com", "legend918@hotmail.com"))
            {
              
                string body = "";
                for (int i = 0; i < arr.Count(); i++)
                {

                    body += " " + arr[i];
                }
                arr.Clear();
                body += " kapasite sıkıntısı olan atmler." + DateTime.Now.ToString();
                ; mm.Subject = "test";
                mm.Body = body;
                mm.IsBodyHtml = false;
                using SmtpClient smtp = new SmtpClient();
                //   "iyzavjzketybhoer"
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("keskinmurat888@gmail.com", "iyzavjzketybhoer");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }

        }
        public void emailTest2(string path)
        {
            using (MailMessage mm = new MailMessage("keskinmurat888@gmail.com", "keskinmurat888@gmail.com"))
            {
                string body = "";
                for (int i = 0; i < arr.Count(); i++)
                {
                    if (!arr.Contains(arr[i]))
                        body += " " + arr[i];
                }
                body += " kapasite sıkıntısı olan atmler." + DateTime.Now.ToString();
                ;
                mm.Subject = "test";
                mm.Body = body;
                //6.hafta 2/3.gün email gönderimi rota ss gönderimi e postaya l.ss paketi  gmail smtp 
                mm.Attachments.Add(new Attachment(path));
                mm.IsBodyHtml = false;
                using SmtpClient smtp = new SmtpClient();
                //   "iyzavjzketybhoer"
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("keskinmurat888@gmail.com", "iyzavjzketybhoer");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }
}
