using System;
using System.Text;

namespace WordCounter
{
    /// <summary>
    /// General helpers
    /// </summary>
    public class Helpers
    {
        /// <summary>
        /// Converts a numeric index to a string index. Examples: 0 becomes "a.". 1 becomes "b.". 27 becomes "aa."
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string ConvertNumericIndexToStringIndex(int index)
        {
            // quick explaination:
            // from indexes 0 to 26, this should return letters from 'a' to 'z'.
            // starting from 27, this should return strings from 'aa' to 'zz', and so forth, for every 26 indexes.
            // that is, for every 26 numbers, it should increase the number of characters.

            // number of characters from 'a' to 'z'
            const int charactersCount = 26;
            // the numeric value for 'a'
            const int aCharacterIndex = 97;

            // the number of times that index is greater than the character count
            var rounds = index / charactersCount;

            // this is the string index, not repeated. I mean, the string index can only go from 'a' to 'z', even if the number is bigger than 26.
            // What we do is to repeat this character N times so it appears exactly like the challenge description
            var baseIndex = index % charactersCount;

            // the string class has an unknown constructor parameter that is the number of times a character should be repeated
            return new string((char) (aCharacterIndex + baseIndex), rounds + 1) + '.';
        } 
    }
}