
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

            var nullable = split[0].Contains('?');
            var isList = split[0].Contains("List"); 
            var typeName = ProcessTypeName(split[0], nullable, isList);

            //Lower case the first character of the name
            var lowerName = char.ToLower(split[1][0]) + split[1].Substring(1);
            if (nullable)
            {
                lowerName += nullable;
            }
            result += $"\t{lowerName}: {typeName};\n";
        }

        return result;
    }

    private string ProcessTypeName(string type, bool nullable, bool isList)
    {
        var result = "";

        if (isList)
        {
            var startIndex = type.IndexOf('<') + 1;
            var endIndex = type.IndexOf('>');
            var className = type.Substring(startIndex, endIndex - startIndex);
            result = $"{className}[]";
        }
        else
        {

            var filteredType = nullable ? type.Trim('?') : type;
            switch (filteredType.ToLower())
            {
                case "int":
                case "long":
                    {
                        result = "number";
                        break;
                    }
                case "string":
                    {
                        result = "string";
                        break;
                    }
                    default:
                    {
                        throw new ApplicationException("Not a valid type to parse");
                    }
            }
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