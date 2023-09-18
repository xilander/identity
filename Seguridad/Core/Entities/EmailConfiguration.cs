using System;
namespace Seguridad.Core.Entities
{
	public class EmailConfiguration
	{
        public class MailSettings
        {
            public string Server { get; set; }
            public int Port { get; set; }
            public string SenderName { get; set; }
            public string SenderEmail { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class MailData
        {
            public string idcliente { get; set; }
            public string EmailToId { get; set; }
            public string EmailToName { get; set; }
            public string EmailSubject { get; set; }
            public string EmailBody { get; set; }
            public string path { get; set; }
            public string HtmlBody { get; set; }
        }


        public class HTMLMailData
        {
            public string EmailToId { get; set; }
            public string EmailToName { get; set; }
        }
    }
}

