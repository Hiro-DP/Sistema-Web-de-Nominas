namespace Sistema_Web_de_Nominas.Models.Settings
{
    public class SmtpSettings
    {
        //Estos nombren van en el appsettings.json
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
