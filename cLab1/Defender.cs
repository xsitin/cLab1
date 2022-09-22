using System.Text;

namespace cLab1;

public class Defender
{
    public void PrepareFile(string pathToFile, int blockSize = 16)
    {
        var reader = new BufferedStream(File.OpenRead(pathToFile));
        var writer = new BufferedStream(File.OpenWrite(pathToFile + "ex"));
        var buffRead = new Span<byte>(new byte[16]);
        var buffWrite = new Span<byte>(new byte[256]);
        var count = 16;
        while (count == 16)
        {
            count = reader.Read(buffRead);
            buffWrite.Clear();
            for (var i = 0; i < count; i++)
                buffWrite[i * 16 + 15] = buffRead[i];
            writer.Write(buffWrite);
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
        var preparedDict = Enumerable.Range(0, 256).Select(x=>(byte)x).Select(x =>
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
        {
            for (var j = 0; j < 16; j++)
                builder.Append(dict[i][j].ToString("X"));

            builder.Append($" -> {i.ToString("X")}\n");
        }

        var path = Path.Join(Path.GetDirectoryName(pathToDict), Path.GetFileName(pathToDict) + "_table.txt");
        File.WriteAllText( path, builder.ToString());
    }

    public void Decode(string pathToFile, string pathToDict)
    {
        var buffDict = GetDict(pathToDict);
        var dict = new Dictionary<byte[], byte>(buffDict.Select((x, i) => new KeyValuePair<byte[], byte>(x, (byte)i)), ArrayEqualityComparer<byte>.Default);
        var reader = new BufferedStream(File.OpenRead(pathToFile));
        var writer = new BufferedStream(File.OpenWrite(pathToFile + "std"));
        var buffRead = new Span<byte>(new byte[16]);
        var count = 16;
        while (count == 16)
        {
            count = reader.Read(buffRead);
            var val = dict[buffRead.ToArray()];
            writer.WriteByte(val);
        }
        reader.Close();
        reader.Dispose();
        writer.Close();
        writer.Dispose();
        File.Move(pathToFile + "std",pathToFile, true);
    }

    private byte[][] GetDict(string pathToDict)
    {
        return File.ReadAllBytes(pathToDict).Chunk(16).ToArray();
    }
}