using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ERP.Senders
{
    //تابع ارسال ایمیل برای فراموشی رمز عبور
    public class SendEmail
    {
        public static void Send(string to,string subject,string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("noreply.erpsystemmu@gmail.com","سامانه مدیریت منابع انسانی ERP");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            //System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("noreply.erpsystemmu@gmail.com", "imcmleeknruhdafk");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }
    }
}