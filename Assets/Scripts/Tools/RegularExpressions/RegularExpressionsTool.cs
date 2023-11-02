using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tzipory.Helpers;
using Unity.VisualScripting;
using UnityEngine;

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
            string output = s;
            foreach (var keyValuePair in keywordValue)
                output = Regex.Replace(output, keyValuePair.Key, $"{keyValuePair.Value}");
            
            return  output;
        }

        public static string ColorKeyWords(string s, List<string> keywords,Color color)
        {
            string colorHex = color.ToHexString();
            string output = s;
            foreach (var keyword in keywords)
                output = Regex.Replace(output, keyword, $"<color={ColorLogHelper.GREEN}>{keyword}</color>");
            
            return  output;
        }
    }
}