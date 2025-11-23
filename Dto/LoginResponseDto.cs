namespace AppAutenticaciones.Dto
{
    public class LoginResponseDto
    {
        /*
        public string AccessToken
        public string RefreshToken
        public DateTime ExpiresAt
         */

        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }

    }
}
