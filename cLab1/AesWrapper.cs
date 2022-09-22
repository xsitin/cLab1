using System.Security.Cryptography;

namespace cLab1;

public class AesWrapper
{
    public void Encode(string pathToFile)
    {
        var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.Key = new byte[16];
        Random.Shared.NextBytes(aes.Key);
        var cryptoReader = new CryptoStream(
            new BufferedStream(File.OpenRead(pathToFile)),
            aes.CreateEncryptor(),
            CryptoStreamMode.Read
        );
        var writer = new BufferedStream(File.OpenWrite(pathToFile + "cr"));
        var buffRead = new Span<byte>(new byte[256]);
        var count = 256;
        while (count == 256)
        {
            count = cryptoReader.Read(buffRead);
            if (count == 256)
                writer.Write(buffRead);
            else if (count > 0)
                writer.Write(buffRead.Slice(0, count));
        }

        cryptoReader.Close();
        cryptoReader.Dispose();
        writer.Close();
        writer.Dispose();
        File.Move(pathToFile + "cr", pathToFile, true);
    }
}