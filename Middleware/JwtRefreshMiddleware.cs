using System.IdentityModel.Tokens.Jwt;
using Sistema_Web_de_Nominas.Services;

namespace Sistema_Web_de_Nominas.Middleware
{
    public class JwtRefreshMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            var accessToken = context.Request.Cookies["access_token"];
            var refreshToken = context.Request.Cookies["refresh_token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                var handler = new JwtSecurityTokenHandler();

                try
                {
                    var jwtToken = handler.ReadJwtToken(accessToken);

                    if (jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        // Access token expirado: intentamos refrescar
                        if (!string.IsNullOrEmpty(refreshToken))
                        {
                            var newTokens = await authService.RefreshAccessTokenAsync(refreshToken);

                            if (newTokens != null)
                            {
                                var accessOptions = new CookieOptions
                                {
                                    HttpOnly = true,
                                    Secure = true,
                                    SameSite = SameSiteMode.Strict,
                                    Expires = newTokens.ExpiraEn
                                };

                                var refreshOptions = new CookieOptions
                                {
                                    HttpOnly = true,
                                    Secure = true,
                                    SameSite = SameSiteMode.Strict,
                                    Expires = DateTime.UtcNow.AddDays(7)
                                };

                                context.Response.Cookies.Append("access_token", newTokens.TokenAcceso, accessOptions);
                                context.Response.Cookies.Append("refresh_token", newTokens.TokenActualizacion, refreshOptions);
                            }
                            else
                            {
                                // Refresh fallido → limpiar cookies y redirigir
                                context.Response.Cookies.Delete("access_token");
                                context.Response.Cookies.Delete("refresh_token");
                                context.Response.Redirect("/Auth/Login");
                                return;
                            }
                        }
                    }
                }
                catch
                {
                    // Token malformado o inválido → limpiar cookies y redirigir
                    context.Response.Cookies.Delete("access_token");
                    context.Response.Cookies.Delete("refresh_token");
                    context.Response.Redirect("/Auth/Login");
                    return;
                }
            }

            await _next(context);
        }
    }
}
