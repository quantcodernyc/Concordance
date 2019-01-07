using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WordCounter
{
    /// <summary>
    /// The WordCounter program.
    /// </summary>
    /// <remarks>
    /// As the specs didn't specify anything about performance, so we're considering this app to be performance crictical. Because of that, we're reading
    /// the file stream only once and performing all the processing along the way. This program works with Gigabytes long files, as the file is never
    /// entirely kept in memory. If performance wasn't so important, we could have used some beautiful LINQ to Objects and Regexes to make things prettier :)
    /// </remarks>
    class Program
    {
        /// <summary>
        /// Global exception handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(((Exception)e.ExceptionObject).Message);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press Enter to continue");
            Console.ReadKey(false);
            Environment.Exit(1);
        }

        static void Main(string[] args)
        {
            // registers the global exception handler. Now we can just throw if anything is wrong, without worrying about the presentation
            AppDomain.CurrentDomain.UnhandledException += GlobalExceptionHandler;

            if (args.Length != 1)
            {
                throw new Exception("Wrong number of arguments. There should be one and only one argument.");
            }

            var fileName = args[0];
            if (!File.Exists(fileName))
            {
                throw new Exception(String.Format("The specified file name does not exist. File name: {0}", fileName));
            }

            // just so we can assess the performance.
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // the result of the word processing
            SortedDictionary<string, WordData> words;

            try
            {
                // opens a filestream and pass it to the word processor
                using (var streamReader = new StreamReader(fileName))
                {
                    words = WordProcessor.Process(streamReader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to process words", ex);
            }

            var index = 0;
            foreach (var wordPair in words)
            {
                var word = wordPair.Key;
                var wordData = wordPair.Value;
                // displays each of the results
                // the second parameter in the {} is the number of padding spaces to the left. Negative numbers make it to the right.
                Console.WriteLine(string.Format("{0, -20} {1, -20} {2}", Helpers.ConvertNumericIndexToStringIndex(index), word, wordData ));
                index++;
            }

            stopWatch.Stop();

            Console.WriteLine("");
            Console.WriteLine(string.Format("Time ellapsed: {0}ms", stopWatch.ElapsedMilliseconds));

            Console.ReadKey(false);
        }

    }
}
