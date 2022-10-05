using System.Security.Cryptography;

namespace cLab1;

public class AesWrapper
{
    public void Encode(string pathToFile, byte[] key)
    {
        var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.Key = key;
        var cryptoReader = new CryptoStream(
            new BufferedStream(File.OpenRead(pathToFile)),
            aes.CreateEncryptor(),
            CryptoStreamMode.Read
        );
        var writer = new BufferedStream(File.OpenWrite(pathToFile + "cr"));
        cryptoReader.CopyTo(writer);

        cryptoReader.Close();
        cryptoReader.Dispose();
        writer.Close();
        writer.Dispose();
        File.Move(pathToFile + "cr", pathToFile, true);
    }
}