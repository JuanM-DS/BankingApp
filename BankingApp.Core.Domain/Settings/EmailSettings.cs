namespace BankingApp.Core.Domain.Settings
{
    public class EmailSettings
    {
        public string EmailFrom { get; set; }

        public string SmtpHost { get; set; }

        public string SmtpPort { get; set; }

        public string SmtpUser { get; set; }

        public string SmtpPassword { get; set; }

        public string DisplayName { get; set; }
    }
}
