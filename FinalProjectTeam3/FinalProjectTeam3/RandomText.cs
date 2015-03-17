using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectTeam3
{
    public class RandomText
    {
        static Random _random = new Random();
        StringBuilder _builder;
        string[] _words;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomText"/> class.
        /// </summary>
        /// <param name="words">
        /// The words.
        /// </param>
        public RandomText(string[] words)
        {
            _builder = new StringBuilder();
            _words = words;
        }

        /// <summary>
        /// Add content paragraphs.
        /// </summary>
        /// <param name="numberParagraphs">
        /// The number of paragraphs.
        /// </param>
        /// <param name="minSentences">
        /// The min number sentences.
        /// </param>
        /// <param name="maxSentences">
        /// The max number of sentences.
        /// </param>
        /// <param name="minWords">
        /// The min number of words.
        /// </param>
        /// <param name="maxWords">
        /// The max number of words.
        /// </param>
        public void AddContentParagraphs(
            int numberParagraphs, 
            int minSentences,
            int maxSentences, 
            int minWords, 
            int maxWords)
        {
            for (int i = 0; i < numberParagraphs; i++)
            {
                AddParagraph(_random.Next(minSentences, maxSentences + 1),
                     minWords, maxWords);
                _builder.Append("\n\n");
            }
        }

        /// <summary>
        /// The add paragraph.
        /// </summary>
        /// <param name="numberSentences">
        /// The number sentences.
        /// </param>
        /// <param name="minWords">
        /// The min words.
        /// </param>
        /// <param name="maxWords">
        /// The max words.
        /// </param>
        void AddParagraph(int numberSentences, int minWords, int maxWords)
        {
            for (int i = 0; i < numberSentences; i++)
            {
                int count = _random.Next(minWords, maxWords + 1);
                AddSentence(count);
            }
        }

        /// <summary>
        /// The add sentence.
        /// </summary>
        /// <param name="numberWords">
        /// The number words.
        /// </param>
        void AddSentence(int numberWords)
        {
            StringBuilder b = new StringBuilder();
            // Add n words together.
            for (int i = 0; i < numberWords; i++) // Number of words
            {
                b.Append(_words[_random.Next(_words.Length)]).Append(" ");
            }
            string sentence = b.ToString().Trim() + ". ";
            // Uppercase sentence
            sentence = char.ToUpper(sentence[0]) + sentence.Substring(1);
            // Add this sentence to the class
            _builder.Append(sentence);
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        public string Content
        {
            get
            {
                return _builder.ToString();
            }
        }
    }
}
