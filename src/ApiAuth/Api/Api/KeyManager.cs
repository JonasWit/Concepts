using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Api;

public class KeyManager
{
    public RSA RsaKey { get; }
    public KeyManager()
    {
        RsaKey = RSA.Create();
        if (File.Exists("key"))
        {
            RsaKey.ImportRSAPrivateKey(File.ReadAllBytes("key"), out _);
        }
        else
        {
            var pk = RsaKey.ExportRSAPrivateKey();
            File.WriteAllBytes("key", pk);
        }
    }
}