namespace Ambev.Dev.Test.Domain.Configs;

public class JwtConfig
{
    public string Issuer { get; set; }
    public int ExpiresIn { get; set; }
    public string SecretKey { get; set; }
}
