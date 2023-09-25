using System;
namespace Seguridad.Core.Entities
{
	public class EmailConfiguration
	{
        public class MailSettings
        {
            public required string Server { get; set; }
            public int Port { get; set; }
            public required string SenderName { get; set; }
            public required string SenderEmail { get; set; }
            public required string UserName { get; set; }
            public required string Password { get; set; }
        }

        public class MailData
        {
            public string? idcliente { get; set; }
            public string? EmailToId { get; set; }
            public string? EmailToName { get; set; }
            public string? EmailSubject { get; set; }
            public string? EmailBody { get; set; }
            public string? path { get; set; }
            public string? HtmlBody { get; set; }
        }


        public class HTMLMailData
        {
            public required string EmailToId { get; set; }
            public required string EmailToName { get; set; }
        }
    }
}

