using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pustok.Database.Models;
using Pustok.ViewModels.Client;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Components.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Pustok.Utilites
{
    public class Utilities
    {
        public string[] GetEmails(string email)
        {
            string[] emails = email
                .Trim(' ')
                .Split(',');
            return emails;
        }
        public string Trim(string input, int length)
        {
            if (input.Length <= length)
            {
                return input;
            }
            else
            {
                return input.Substring(0, length);
            }
        }
        public bool StringIsNull(string data)
        {
            if (data is null) { return true; } 
            else { return false; }
        }
        public bool Length(string data,int min,int max)
        {
             if(data.Length <= max&& data.Length >= min) { return true; }
             else { return false; }
        }
        public bool EmailSymbol(string data)
        {
            if(data.Contains("@") && data.Contains("."))
            {
                return true;
            }
            return false;
        }
        public Email CheckMessageInputs(Email email)
        {
            if(CheckTitle(email.Title) && CheckContent(email.Content)&& CheckEmail(email.EmailAddress))
            {
                return email;
            }
            return null;
        }
        public bool CheckTitle(string data)
        {
            if (!StringIsNull(data) && Length(data, 1, 20)) { return true; }
            return false;
        }
        public bool CheckContent(string data)
        {
            if (!StringIsNull(data) && Length(data, 1, 50)){ return true; }
            return false;
        }
        public bool CheckEmail(string data)
        {
            if (!StringIsNull(data) && Length(data,10,20)&& EmailSymbol(data)) { return true; }
            return false;
        }
        public void SendMessage(Email model, SmtpSettings smtpSettings)
        {
            string divideEmail = model.EmailAddress.ToString();
            string[] getEamil = GetEmails(divideEmail);

            for(int i = 0; i < getEamil.Length; i++)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Sender", "amrahrustamov94@yandex.com"));
                message.To.Add(new MailboxAddress("Recipient", getEamil[i]));
                message.Subject = model.Title;

                var builder = new BodyBuilder();
                builder.TextBody = model.Content;
                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(smtpSettings.Server, smtpSettings.Port, smtpSettings.UseSsl);
                    client.Authenticate(smtpSettings.Username, smtpSettings.Password);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
        }
    }
}
