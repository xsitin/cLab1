using System.Text;

namespace cLab1;

public class Defender
{
    public void PrepareFile(string pathToFile)
    {
        var reader = new BufferedStream(File.OpenRead(pathToFile));
        var writer = new BufferedStream(File.OpenWrite(pathToFile + "ex"));
        var buffWrite = new byte[16];
        while (reader.Position < reader.Length)
        {
            buffWrite[15] = (byte) reader.ReadByte();
            writer.Write(buffWrite, 0, buffWrite.Length);
        }

        reader.Close();
        reader.Dispose();
        writer.Close();
        writer.Dispose();
        File.Move(pathToFile + "ex", pathToFile, true);
        CreateDict(pathToFile);
    }

    private static void CreateDict(string pathToFile)
    {
        var preparedDict = Enumerable.Range(0, 256).Select(x => (byte) x).Select(x =>
        {
            var arr = new byte[16];
            arr[15] = x;
            return arr;
        }).SelectMany(x => x).ToArray();
        File.WriteAllBytes(pathToFile + "dict", preparedDict);
    }

    public void Translate(string pathToDict)
    {
        var builder = new StringBuilder();
        var dict = GetDict(pathToDict);
        for (var i = 0; i < 16; i++)
            builder.Append($"{BitConverter.ToString(dict[i])} -> {i.ToString("X")}\n");

        var path = Path.Join(Path.GetDirectoryName(pathToDict), Path.GetFileName(pathToDict) + "_table.txt");
        File.WriteAllText(path, builder.ToString());
    }

    public void Decode(string pathToFile, string pathToDict)
    {
        var buffDict = GetDict(pathToDict);
        var dict = new Dictionary<byte[], byte>(buffDict.Select((x, i) => new KeyValuePair<byte[], byte>(x, (byte) i)),
            ArrayEqualityComparer<byte>.Default);
        var reader = new BufferedStream(File.OpenRead(pathToFile));
        var writer = new BufferedStream(File.OpenWrite(pathToFile + "std"));
        var buffRead = new byte[16];
        while (reader.Position < reader.Length)
        {
            reader.Read(buffRead, 0, buffRead.Length);
            writer.WriteByte(dict[buffRead]);
        }

        reader.Close();
        reader.Dispose();
        writer.Close();
        writer.Dispose();
        File.Move(pathToFile + "std", pathToFile, true);
    }

    private byte[][] GetDict(string pathToDict)
    {
        return File.ReadAllBytes(pathToDict).Chunk(16).ToArray();
    }
}