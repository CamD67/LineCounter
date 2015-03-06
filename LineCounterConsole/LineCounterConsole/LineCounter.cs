using System;
using System.IO;

namespace LineCounterConsole
{
    public class LineCounter
    {
        private readonly String _filename;

        //Utility constants to make reading easier
        private readonly String[] _commentStarters = { "//", "/*", "*/", "*" };//is /// necessary?
        //Supporting Java (import, package), C# (using, namespace, #)
        private readonly String[] _nonCodeStarters = { "import", "using", "namespace", "package", "#", "class", "struct", "enum" };
        private readonly char[] _specialCharacters = { '{', '}' };
        private readonly String[] _modifiers = { "public", "private", "abstract", "virtual" };
        private const char Space = ' ';


        public LineCounter(String filename)
        {
            _filename = filename;
            CheckFileExists();
        }

        private int GeneralCountLines(bool significant)
        {
            int lines = 0;
            CheckFileExists();
            using (StreamReader reader = new StreamReader(_filename))
            {
                //BUG here. Loop taking a long time to work through
                while (!reader.EndOfStream)
                {
                    //TODO: Support line breaks (check line up tothe next ;)
                    if (significant)
                    {
                        //Read next line
                        //Might want to switch to a char array or StringBuilder for faster operations
                        String line = reader.ReadLine().Trim();
                        line = RemoveModifiers(line);
                        //if the line is NOT a comment, empty, or important code, increment lines
                        if (!(LineContainsNonCode(line) ||
                            LineContainsComment(line) ||
                            LineIsEmpty(line)))
                        {
                            lines++;
                        }
                    }
                    else
                    {
                        //just a general line count
                        lines++;
                    }
                }
            }
            return lines;
        }

        private String RemoveModifiers(String line)
        {
            foreach (String mod in _modifiers)
            {
                int index = line.IndexOf(mod);
                if (index != -1)
                {
                    line = line.Substring(index + mod.Length - 1);
                    Console.WriteLine("resulting line from removing modifiers: " + line);
                }
            }
            return line;
        }

        private bool LineIsEmpty(String line)
        {
            //replace all special chars with spaces
            foreach (char special in _specialCharacters)
            {
                //could also be a "removeAll" operation
                line = line.Replace(special, Space);
            }
            //trim leading and trailing whitespace (which is now all special chars too)
            line = line.Trim();
            return line.Length != 0;
        }

        private bool LineContainsComment(String line)
        {
            foreach (String comment in _commentStarters)
            {
                if (line.StartsWith(comment))
                {
                    return true;
                }
            }
            return false;
        }

        private bool LineContainsNonCode(String line)
        {
            //if the line begins with a non-code starter, skip it
            foreach(String nonCode in _nonCodeStarters)
            {
                if (line.StartsWith(nonCode))
                {
                    return true;
                }
            }
            return false;
        }

        public int CountSignificantLines()
        {
            return GeneralCountLines(true);
        }
        public int CountLines()
        {
            return GeneralCountLines(false);
        }

        private void CheckFileExists()
        {
            if (!File.Exists(_filename))
            {
                throw new FileNotFoundException("Cannot find file: " + _filename);
            }
        }
    }
}
