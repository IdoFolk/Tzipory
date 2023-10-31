using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Tzipory.Tools.RegularExpressions
{
    /*
     * ^ - Starts with
     * $ - Ends with
     * [] - Range
     * () - Group
     * . - Single character once
     * + - one or more characters in a row
     * ? - optional preceding character match
     * \ - escape character
     * \n - New line
     * \d - Digit
     * \D - Non-digit
     * \s - White space
     * \S - non-white space
     * \w - alphanumeric/underscore character (word chars)
     * \W - non-word characters
     * {x,y} - Repeat low (x) to high (y) (no "y" means at least x, no ",y" means that many)
     * (x|y) - Alternative - x or y
     *
     * [^x] - Anything but x (where x is whatever character you want)
     */
    
    public class RegularExpressionsTool
    {
        public static string SetValueOnKeyWord(string s,Dictionary<string,object> keywordValue)
        {
            foreach (var keyValuePair in keywordValue)
                Regex.Replace(s, keyValuePair.Key, $"{keyValuePair.Value}");

            return s;
        }
    }
}