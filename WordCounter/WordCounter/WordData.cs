using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter
{
    /// <summary>
    /// Contains information about a particular word
    /// </summary>
    public class WordData
    {
        public WordData()
        {
            this.SentenceIndexes = new List<int>();
        }
        
        /// <summary>
        /// The sentences in which the word occur
        /// </summary>
        public List<int> SentenceIndexes { get; set; }

        /// <summary>
        /// Displays a WordData as a String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sentenceIndexesAsString = string.Join(",", this.SentenceIndexes);
            return String.Format("{{{0}:{1}}}", this.SentenceIndexes.Count, sentenceIndexesAsString);
        }
    }
}
