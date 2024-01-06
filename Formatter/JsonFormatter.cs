using System.Text;

namespace Formatter;

public static class JsonFormatter
{
    public static string Format(string json)
    {
        char? previous = null;
        var formattedJson = new StringBuilder();
        int indentationLevel = 0;
        var inValueString = false;

        for (int i = 0; i < json.Length; i++)
        {
            var setPrevious = true;
            var c = json[i];

            if (!inValueString && previous == '"' && c != ':' && c != ',' && c != ' ' && c != '}')
            {
                throw new Exception($"Unexpected character at {i} - '{c}'.");
            }

            switch (c)
            {
                case '[':
                case '{':
                    if (inValueString)
                    {
                        formattedJson.Append(c);
                    }
                    else
                    {
                        if (previous is not null && previous != ' ')
                            formattedJson.Append(' ');

                        formattedJson.Append(c);
                        formattedJson.Append('\n');
                        indentationLevel++;
                        formattedJson.Append('\t', indentationLevel);
                    }
                    break;

                case ']':
                case '}':
                    if (inValueString)
                    {
                        formattedJson.Append(c);
                    }
                    else
                    {
                        formattedJson.Append('\n');
                        indentationLevel--;
                        formattedJson.Append('\t', indentationLevel);
                        formattedJson.Append(c);
                    }
                    break;

                case ',':
                    formattedJson.Append(c);

                    if (!inValueString)
                    {
                        formattedJson.Append('\n');
                        formattedJson.Append('\t', indentationLevel);
                    }

                    break;

                case '\"':
                    if (i + 1 < json.Length
                        &&
                        (
                            (previous == ':' && json[i + 1] == '{')
                            || (previous == ':' && json[i + 1] == '[')
                            || (previous == '}' && json[i + 1] == ',')
                            || (previous == '}' && json[i + 1] == ']')
                            || (previous == ']' && json[i + 1] == '}')
                            || (previous == ']' && json[i + 1] == ',')
                            || (previous == ',' && json[i + 1] == '{')
                            || (previous == ',' && json[i + 1] == '[')
                        ))
                    {
                        setPrevious = false;
                        break;
                    }

                    formattedJson.Append(c);
                    inValueString = !inValueString;
                    break;

                case ' ':
                    if (inValueString || (!inValueString && previous == ':'))
                    {
                        formattedJson.Append(c);
                    }
                    else if (!inValueString
                        && (
                            previous == ' '
                            || previous == ','
                            || previous == '{'
                            || previous == '}'
                            || previous == '['
                            || previous == ']'
                            || previous == '"'
                            || previous == ':'))
                    {
                        break;
                    }
                    else if (!inValueString && i + 1 < json.Length && (json[i + 1] == ' ' || json[i + 1] == ','))
                    {
                        break;
                    }
                    else
                    {
                        formattedJson.Append(c);
                    }

                    break;

                case ':':
                    if (inValueString || (!inValueString && (previous == '"' || previous == ' ')))
                    {
                        formattedJson.Append(c);
                        formattedJson.Append(' ');
                    }
                    else
                    {
                        throw new Exception($"Invalid JSON error at char {i} - {previous}");
                    }

                    break;

                default:
                    if (!inValueString && previous == ':')
                    {
                        formattedJson.Append(' ');
                    }
                    else
                    {
                        formattedJson.Append(c);
                    }

                    break;
            }

            if (setPrevious)
            {
                previous = c;
            }
        }

        if (indentationLevel != 0)
        {
            throw new Exception("Invalid JSON format");
        }

        return formattedJson.ToString();
    }
}