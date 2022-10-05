namespace cLab1;

static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length < 1)
        {
            PrintHelp();
            return 1;
        }

        var defender = new Defender();
        var aes = new AesWrapper();
        if (args.Length > 1 && !File.Exists(args[1]))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: file doesn't exist");
            Console.ForegroundColor = ConsoleColor.White;
            return 1;
        }

        switch (args[0])
        {
            case "prepare":
                defender.PrepareFile(args[1]);
                break;
            case "translate":
                defender.Translate(args[1] + "dict");
                break;
            case "encode":
                var key = new byte[16];
                Random.Shared.NextBytes(key);
                aes.Encode(args[1], key);
                aes.Encode(args[1] + "dict", key);
                break;
            case "decode":
                defender.Decode(args[1], args[1] + "dict");
                break;
            default:
                PrintHelp();
                return 1;
        }

        return 0;
    }

    private static void PrintHelp()
    {
        Console.WriteLine(@"Usage: ./cLab1.exe action path_to_file
actions:
    prepare - prepare file and create dictionary
    translate - create txt file with table of dictionary
    decode - decode file and delete dictionary
    encode - encode file and dictionary");
    }
}