namespace netstrata_test;

class Program
{
    static void Main(string[] args)
    {
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

        var firstSplit = lines[0];

    }
}
