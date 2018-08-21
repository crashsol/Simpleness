
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;

namespace UnitTest
{
   public class EmailTest
    {

        [Fact]
        public void Send_Email_Test()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Anuraj", "47147551@qq.com"));
            message.To.Add(new MailboxAddress("ttt", "47147551@qq.com"));
            message.Subject = "Hello World - A mail from ASPNET Core";
            message.Body = new TextPart("plain")
            {
                Text = "Hello World - A mail from ASPNET Core"
            };
            using (var client = new SmtpClient())
            {
                //client.QueryCapabilitiesAfterAuthenticating = false;
                client.Connect("smtp.qq.com", 25, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                // Note: since we don't have an OAuth2 token, disable 	
                // the XOAUTH2 authentication mechanism.     
                client.Authenticate("47147551@qq.com", "");
                client.Send(message);
                client.Disconnect(true);
            }
        }


    }
}