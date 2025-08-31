
using Microsoft.VisualBasic;

public class ClassParser
{
    private string GetSetFilter = "{ get; set; }";
    private string PublicFilter = "public";

    private string typeScriptClass = "export interface";

    public ClassParser()
    {

    }

    public string ParseClass(string[] lines)
    {
        var result = "";
        if (lines == null || lines.Length <= 0)
        {
            throw new ApplicationException("Need data to parse properly");
        }

        // var classDefinitions = lines.Where(x => x.Contains("class")).ToArray();

        // if (classDefinitions != null)
        // {
        //     var firstClass = classDefinitions[0];
        //     var firstLine = FilterLine(firstClass);

        //     var className = firstLine[1];
        //     result += $"{typeScriptClass} {className}";
        // }

        var properties = new List<string>();
        var nestedProperty = new List<string>();
        var inFirstProperty = false;
        var inSecondProperty = false;

        foreach (var line in lines)
        {
            //Class check
            if (line.Contains("class"))
            {
                if (inFirstProperty)
                {
                    var processedProps = ProcessProperties(properties);
                    inSecondProperty = true;
                    inFirstProperty = false;
                    result += processedProps;
                }
                else
                {
                    inFirstProperty = true;
                    var filteredString = FilterLine(line);
                    var split = filteredString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    result += $"{typeScriptClass} {split[1]} {{\n";
                }                
            }
            else if (line == "{")
            {
                //If open line
                continue;
            }
            else if (line == "}")
            {
                //Close the property
                // if (inFirstProperty)
                // {

                // }
                // else
                // {
                //     if (inSecondProperty)
                //     {
                //     }
                // }
                continue;
            }
            else
            {
                //Parse the line
                var filteredLine = FilterLine(line);
                if (inFirstProperty)
                {
                    properties.Add(filteredLine);
                }
                else if (inSecondProperty)
                {
                    nestedProperty.Add(filteredLine);
                }
            }
        }

        return result;
    }

    private string ProcessProperties(List<string> properties)
    {
        var result = "";

        foreach (var prop in properties)
        {
            var split = prop.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            result += $"\t{split[1]}:{split[0].ToLower()};\n";
        }

        return result;
    }

    public string FilterLine(string line)
    {
        var result = "";

        result = line.Replace(PublicFilter, "");
        result = result.Replace(GetSetFilter, "");

        return result;
    }
}