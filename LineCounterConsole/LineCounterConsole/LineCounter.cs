using System;
using System.IO;

namespace LineCounterConsole
{
    /// <summary>
    /// A LineCounter. Counts lines in source code files and can determine their total line 
    /// length as well as their significant lines. Note that line length isn't the only way
    /// to determine code quality and can even be a misleading metric.
    /// </summary>
    public class LineCounter
    {
        private readonly String _filename;

        //Utility constants to make reading easier
        private readonly String[] _commentStarters = { "//", "/*", "*/", "*" };//* catches eclipse block comments
        private readonly String[] _nonCodeStarters = { "import", "using", "namespace", "package", "#",
                                                       "class", "struct", "enum" };
        private readonly char[] _specialCharacters = { '{', '}' };
        private readonly String[] _modifiers = { "public", "private", "abstract", "virtual" };
        private const char Space = ' ';


        /// <summary>
        /// Creates a new LineCounter object with the given filename
        /// </summary>
        /// <param name="filename">Full or relative path to the source code file</param>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if file could not be found</exception>
        public LineCounter(String filename)
        {
            _filename = filename;
            CheckFileExists();
        }

        /// <summary>
        /// Helper method to count the number of lines. If sigificant is true then counts only signficant lines.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Throws if the file given at object creation no longer exists</exception>
        /// <param name="significant">true if only counting signficant lines, false otherwise</param>
        /// <returns>The number of lines counted</returns>
        private int GeneralCountLines(bool significant)
        {
            int lines = 0;
            CheckFileExists();
            using (StreamReader reader = new StreamReader(_filename))
            {
                while (!reader.EndOfStream)
                {
                    //Read next line
                    //Might want to switch to a char array or StringBuilder for faster operations
                    String line = reader.ReadLine().Trim();
                    //TODO: Support line breaks (check line up tothe next ;)
                    if (significant)
                    {
                        line = RemoveModifiers(line);
                        //if the line is NOT a comment, IS empty, or NOT important code, increment lines
                        if (!(LineContainsNonCode(line) ||
                              LineContainsComment(line) ||
                              !LineIsEmpty(line)))
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

        /// <summary>
        /// Helper method to remove modifiers from the line
        /// </summary>
        /// <param name="line">The to modify</param>
        /// <returns>The modified line without modifiers</returns>
        private String RemoveModifiers(String line)
        {
            foreach (String mod in _modifiers)
            {
                int index = line.IndexOf(mod);
                if (index != -1)
                {
                    line = line.Substring(index + mod.Length - 1);
                }
            }
            return line;
        }

        /// <summary>
        /// checks to see if the line is empty. An empty line has no information in it
        /// </summary>
        /// <param name="line">The line to check</param>
        /// <returns>true if the line is empty, false otherwise</returns>
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

        /// <summary>
        /// Checks to see if the entire line is a comment
        /// </summary>
        /// <param name="line">The line to check</param>
        /// <returns>true if the entire line is a comment, false otherwise</returns>
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

        /// <summary>
        /// Checks to see if the line contains non-code (imports, using, preprocessor, etc.)
        /// </summary>
        /// <param name="line">The line to check</param>
        /// <returns>true if the line starts with non-code</returns>
        private bool LineContainsNonCode(String line)
        {
            //if the line begins with a non-code starter, skip it
            foreach (String nonCode in _nonCodeStarters)
            {
                if (line.StartsWith(nonCode))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Counts the significant lines in the source code file.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if file could not be found</exception>
        /// <returns>Number of significant lines in the source code</returns>
        public int CountSignificantLines()
        {
            return GeneralCountLines(true);
        }
        /// <summary>
        /// Counts the total number of lines in the source code
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if file could not be found</exception>
        /// <returns>Total number of lines</returns>
        public int CountLines()
        {
            return GeneralCountLines(false);
        }

        /// <summary>
        /// Helper method to check if the file still exists
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if file could not be found</exception>
        private void CheckFileExists()
        {
            if (!File.Exists(_filename))
            {
                throw new FileNotFoundException("Cannot find file: " + _filename);
            }
        }
    }
}
