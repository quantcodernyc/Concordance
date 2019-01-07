using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter
{
    /// <summary>
    /// Processes words in a stream
    /// </summary>
    public class WordProcessor
    {
        /// <summary>
        /// Processes the words from the input stream and returns a SortedDictionary containing the word as the key and
        /// word data as the value.
        /// </summary>
        /// <param name="streamReader"></param>
        /// <returns></returns>
        public static SortedDictionary<string, WordData> Process(StreamReader streamReader)
        {
            if (streamReader == null) throw new ArgumentNullException("streamReader");

            // these are the characters that, besides letters and digits, are allowed to constitute a word
            char[] allowedSpecialCharacters = { '/', '\\', '*', '+', '-', '@', '&', '$', '%', '#' };
            //string[] allowedSpecialCharacters = { "/", "\\", "*", "+", "-", "@", "&", "$", "%", "#","i.e." };

            // what we're considering to be sentence-breakers
            char[] sentenceBreakers = { '.', '!', '?', '\r', '\n' };

            // what we're considering to be word-breakers
            char[] wordOnlyBreakers = { ' ', ',', ':', ';' };

            // what we're considering to be a numeric symbol. These characters, when having adjoining numbers will not break the word nor the sentence
            char[] numericSymbols = {'.', ','};

            // the "final" word breaker list contains both "sentenceBreakers" and "wordOnlyBreakers"
            // .NET doesn't provide any straight forward way of copying arrays without using  ToList() and Concat()
            // For the sake of performance, let's copy them the old fashion way :)
            
            char[] wordBreakers = new char[sentenceBreakers.Length + wordOnlyBreakers.Length];
            sentenceBreakers.CopyTo(wordBreakers, 0);
            wordOnlyBreakers.CopyTo(wordBreakers, wordOnlyBreakers.Length);
            
            


            // our word index and their data.
            var words = new SortedDictionary<string, WordData>();

            // this is the "temp" state of the current word. The reason we're not appending characters to strings is because String is 
            // an "immutable" type in C#. Meaning that everytime you append to an existing String, you are creating a new String.
            // For the sake of performance, let's use StringBuilder instead.
            var currentWordBuilder = new StringBuilder();

            var currentSentenceIndex = 1;
            var wordsInTheCurrentSentence = 0;

            // let's iterate through every character from the file. streamReader.Peek() returns the next character from the stream without consuming
            // it, or -1 if it's the end of the stream.

            while (streamReader.Peek() >= 0)
            {
                // the current character
                var character = (char)streamReader.Read();

                if (!wordBreakers.Contains(character))
                {
                    // we're still in the same word and sentense
                    if (char.IsLetterOrDigit(character) || allowedSpecialCharacters.Contains(character))                        
                    {
                        // we only append a character to the current word if it's allowed
                        currentWordBuilder.Append(char.ToLower(character));
                    }
                }
                
                if (wordBreakers.Contains(character) || streamReader.Peek() == -1)
                {
                    // if the current character is a word breaker OR it's the end of the stream. End of stream should count as a word breaker too
                    
                    if ((currentWordBuilder.Length > 0 && numericSymbols.Contains(character) && streamReader.Peek() >= 0 &&
                         char.IsDigit(currentWordBuilder[currentWordBuilder.Length - 1]) &&
                         char.IsDigit((char) streamReader.Peek())))
                    {
                        // this is *BASIC* support for numbers.
                        // what we do is to consider 2 digits separated by '.' or ',' to be part of the same word. We don't consider it to be a sentence or a word breaker
                        // no validation on the number is performed
                        
                        currentWordBuilder.Append(character);
                        // we have to continue because otherwise the '.' or ',' will be processed as a word or sentence breaker
                        continue;
                    }

                    if (currentWordBuilder.Length > 0)
                    {
                        // we found a *COMPLETE WORD*. The reason we're checking 'currentWordBuilder.Length > 0' is because
                        // there can be 2 or more wordBreakers sequentially. In this case, we don't want to add a new word

                        WordData wordData;

                        // the challenge description seems to be case insensitive, so we're assuming it's case insensitive.
                        var word = currentWordBuilder.ToString();
                        if (words.ContainsKey(word))
                        {
                            // in this case, the current word has been found already. Let's just update it.
                            wordData = words[word];
                            wordData.SentenceIndexes.Add(currentSentenceIndex);
                        }
                        else
                        {
                            // in this case, the current word has not been found already. Let's register it.
                            wordData = new WordData();
                            wordData.SentenceIndexes.Add(currentSentenceIndex);
                            words.Add(word, wordData);
                        }
                        //fixing abbreviations scenario


                        // currentWordBuilder should be cleared and start again
                        currentWordBuilder.Clear();
                        // wordsInTheCurrentSentence should be incremented;
                        wordsInTheCurrentSentence++;
                    }

                    if (sentenceBreakers.Contains(character))
                    {
                        // check for abbreviations cases for example,"e.g. or i.e. encountered in sentence"
                        //this piece of code relies on grammar assumption that after end of sentence there will be space and new sentence begins with uppercase character.
                        var breaker = character;
                        if (streamReader.Peek() >= 1)
                        {
                            var check1 = (char)streamReader.Read();
                            if (check1 == ' ')
                            {
                                var check2 = (char)streamReader.Peek();
                                if (char.IsUpper(check2))
                                {
                                    // we found a *COMPLETE SENTENCE*
                                    currentSentenceIndex++;
                                }
                            }                            
                        }
                        else
                        {
                            // we found a *COMPLETE SENTENCE* 

                            if (wordsInTheCurrentSentence > 0)
                            {
                                // if wordsInTheCurrentSentence is 0 and we found a new sentence breaker, that means there were multiple word breakers sequentially.
                                // In this case, we don't want to increment currentSentenceIndex
                                currentSentenceIndex++;
                                wordsInTheCurrentSentence = 0;
                            }
                        }
                    }
                }
            }

            return words;
        }
    }
}
