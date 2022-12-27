using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace TestingTwilio
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static  void Main()
        {
            SendTwilioAPISMS();
        }

        public class Email
        {
            private string to;
            private string from;
            private string subject;
            private string text;

            public Email(string to, string from, string subject, string text)
            {
                this.to = to;
                this.from = from;
                this.subject = subject;
                this.text = text;
            }

        }

        public class SMS
        {
            private string To;
            private string From;
            private string Body;
         

            public SMS(string To, string From, string Body )
            {
                this.To = To;
                this.From = From;
                this.Body = Body;
        
            }

        }

        //https://api.sendgrid.com/api/mail.send.json
        static string SendTwilioAPIMail()
        {

            var url = "https://api.sendgrid.com/api/mail.send.json";
            using var client = new HttpClient();
            var emails = new Email("komal.shah87@gmail.com", "komal.shah87@gmail.com", "Test","Test");

            var json = JsonConvert.SerializeObject(emails);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "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");
            var response = client.PostAsync(url, data);
            return response.Result.ToString();
        }

        private static void SendTwilioAPISMS()
        {

            var url = "https://api.twilio.com/2010-04-01/Accounts/AC72f2ae89735fa933136da89ecfe194cf/Messages";
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var dict = new Dictionary<string, string>();
            var byteArray = Encoding.ASCII.GetBytes("AC72f2ae89735fa933136da89ecfe194cf:1b2607f1ee6af1fe5c6d7d42c76e63f9_b_O7bndkc - cw0x4jvnQiYydv3M82RX - EWBv323i");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            dict.Add("To", "+919869900564");
            dict.Add("From", "+12183049089");
            dict.Add("Body", "TestPostman");
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(dict) };
            var response = client.SendAsync(requestMessage);
        }

        static async Task SendGoogleMailAsync()
        {
            try
            {
                MailMessage newMail = new MailMessage();
                // use the Gmail SMTP Host
                SmtpClient client = new SmtpClient("smtp.gmail.com");

                // Follow the RFS 5321 Email Standard
                newMail.From = new MailAddress("Komal.shah87@gmail.com", "Komal");

                newMail.To.Add("<<RECIPIENT-EMAIL>>");

                newMail.Subject = "Test Mail"; 

                newMail.IsBodyHtml = true; newMail.Body = "<h1> This is my first Templated Email in C# </h1>";

                newMail.Attachments.Insert(0, new System.Net.Mail.Attachment("fileName"));
                // enable SSL for encryption across channels
                client.EnableSsl = true;
                // Port 465 for SSL communication
                client.Port = 587;
                // Provide authentication information with Gmail SMTP server to authenticate your sender account
                client.Credentials = new System.Net.NetworkCredential("komal.shah87@gmail.com", "PASSWORD");
                
                client.Send(newMail); // Send the constructed mail
                Console.WriteLine("Email Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error -" + ex);
            }

        }

        //
        private static void SendSMS()
        {
            string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

            TwilioClient.Init(accountSid, authToken);


            var message = MessageResource.Create(
                body: "This is a Test message",
                from: new Twilio.Types.PhoneNumber("+12183049089"),
                to: new Twilio.Types.PhoneNumber("+919869900564")
            );
        }


        static async Task TwilioMail()
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var plainTextContent = "SendGrid with C#";
            var htmlContent = "<strong>TEST using Send Grid</strong>";

            var messageEmail = new SendGridMessage()
            {
                From = new EmailAddress("komal.shah87@gmail.com", "Komal"),
                Subject = "Sending with SendGrid is Fun",
                PlainTextContent = plainTextContent,
                HtmlContent = htmlContent
            };
            messageEmail.AddTo(new EmailAddress("komal.shah87@gmail.com", "Komal"));
            var filePath = @"E:\Komal\index.txt";
            var bytes = File.ReadAllBytes(filePath);
            var file = Convert.ToBase64String(bytes);
            messageEmail.AddAttachment("Voucher Details Report.txt", file);

            var response = await client.SendEmailAsync(messageEmail);
        
        }
    }
}

//    var message = new SendGridMessage();

//    message.Personalizations = new List<Personalization>(){
//    new Personalization(){
//        Tos = new List<EmailAddress>(){
//            new EmailAddress(){
//                Email = "komal.shah87@gmail.com",
//                Name = "John Doe"
//            },
//            new EmailAddress(){
//                Email = "komal.shah87@gmail.com",
//                Name = "Julia Doe"
//            }
//        },
//        Ccs = new List<EmailAddress>(){
//            new EmailAddress(){
//                Email = "komal.shah87@gmail.com",
//                Name = "Jane Doe"
//            }
//        },

//    },
//    new Personalization(){
//        From = new EmailAddress(){
//            Email = "komal.shah87@gmail.com",
//            Name = "Example Sales Team"
//        },
//        Tos = new List<EmailAddress>(){
//            new EmailAddress(){
//                Email = "komal.shah87@gmail.com",
//                Name = "Janice Doe"
//            }
//        },
//        Bccs = new List<EmailAddress>(){
//            new EmailAddress(){
//                Email = "jordan_doe@example.com",
//                Name = "Jordan Doe"
//            }
//        }
//    }
//};

//    message.From = new EmailAddress()
//    {
//        Email = "komal.shah87@gmail.com",
//        Name = "Example Order Confirmation"
//    };

//    message.ReplyTo = new EmailAddress()
//    {
//        Email = "komal.shah87@gmail.com",
//        Name = "Example Customer Service Team"
//    };

//    message.Subject = "Your Example Order Confirmation";

//    message.Contents = new List<Content>(){
//    new Content(){
//        Type = "text/html",
//        Value = "<p>Hello from Twilio SendGrid!</p><p>Sending with the email service trusted by developers and marketers for <strong>time-savings</strong>, <strong>scalability</strong>, and <strong>delivery expertise</strong>.</p><p>%open-track%</p>"
//    }
//};

//    message.Attachments = new List<Attachment>(){
//    new Attachment(){
//        //Content = "PCFET0NUWVBFIGh0bWw+CjxodG1sIGxhbmc9ImVuIj4KCiAgICA8aGVhZD4KICAgICAgICA8bWV0YSBjaGFyc2V0PSJVVEYtOCI+CiAgICAgICAgPG1ldGEgaHR0cC1lcXVpdj0iWC1VQS1Db21wYXRpYmxlIiBjb250ZW50PSJJRT1lZGdlIj4KICAgICAgICA8bWV0YSBuYW1lPSJ2aWV3cG9ydCIgY29udGVudD0id2lkdGg9ZGV2aWNlLXdpZHRoLCBpbml0aWFsLXNjYWxlPTEuMCI+CiAgICAgICAgPHRpdGxlPkRvY3VtZW50PC90aXRsZT4KICAgIDwvaGVhZD4KCiAgICA8Ym9keT4KCiAgICA8L2JvZHk+Cgo8L2h0bWw+Cg==",
//        Filename = "index.txt",
//        //Type = "text/html",
//        Disposition = "attachment"
//    }
//};


//    string apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
//    var client = new SendGridClient(apiKey);
//    var response = await client.SendEmailAsync(message).ConfigureAwait(false);

//    Console.WriteLine(response.StatusCode);
//    Console.WriteLine(response.Body.ReadAsStringAsync().Result);
//    Console.WriteLine(response.Headers.ToString());
//}



//private static void Main()
//{
//    Execute().Wait();
//}




