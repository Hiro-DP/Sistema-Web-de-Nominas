namespace Sistema_Web_de_Nominas.Models.Settings
{
    public class JwtSettings
    {
        //Estos nombren van en el appsettings.json
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenValidityInMinutes { get; set; } = 10;

    }
}
