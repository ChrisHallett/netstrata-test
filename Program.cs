namespace netstrata_test;

class Program
{
    static void Main(string[] args)
    {
        var Parser = new ClassParser();
        Console.WriteLine("Enter file path to C# class input");

        var inputPath = Console.ReadLine();

        if (!File.Exists(inputPath))
        {
            throw new ApplicationException("File does not exist at given path");
        }

        //Read inputs
        using StreamReader reader = new(inputPath);
        string readText = reader.ReadToEnd();

        string[] lines = readText.Split(
            new[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );

        var parsedClass = Parser.ParseClass(lines);

        string binFolder = AppContext.BaseDirectory; 
        string projectRoot = Path.GetFullPath(Path.Combine(binFolder, @"..\..\..")); 

        string filePath = Path.Combine(projectRoot, "output.txt");
        File.WriteAllText(filePath, parsedClass);
    }
}
