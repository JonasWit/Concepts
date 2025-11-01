using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace OAuthServer;

public sealed class DevKeys : IDisposable
{
    private static readonly Lock Sync = new();

    public DevKeys( IWebHostEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "crypto_key");
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("path required", nameof(path));
        var fullPath = Path.GetFullPath(path);
        var rsa = RSA.Create();

        if (File.Exists(fullPath))
        {
            var existing = File.ReadAllBytes(fullPath);
            rsa.ImportRSAPrivateKey(existing, out _);
        }
        else
        {
            lock (Sync)
            {
                if (!File.Exists(fullPath))
                {
                    rsa = RSA.Create();
                    var priv = rsa.ExportRSAPrivateKey();
                    var tmp = Path.Combine(Path.GetDirectoryName(fullPath) ?? ".",
                        Path.GetFileName(fullPath) + ".tmp" + Guid.NewGuid().ToString("N"));
                    try
                    {
                        File.WriteAllBytes(tmp, priv);
                        try
                        {
                            var fi = new FileInfo(tmp);
                            if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
                                fi.UnixFileMode = UnixFileMode.UserRead | UnixFileMode.UserWrite;
                        }
                        catch
                        {
                            // ignored
                        }
                        File.Move(tmp, fullPath, true);
                    }
                    catch
                    {
                        try
                        {
                            File.Delete(tmp);
                        }
                        catch
                        {
                            // ignored
                        }

                        throw;
                    }
                }
                else
                {
                    var existing = File.ReadAllBytes(fullPath);
                    rsa.ImportRSAPrivateKey(existing, out _);
                }
            }
        }

        RsaKey = rsa;
    }

    public RSA RsaKey { get; }

    public RsaSecurityKey RsaSecurityKey => new(RsaKey);

    public void Dispose()
    {
        RsaKey?.Dispose();
    }
}