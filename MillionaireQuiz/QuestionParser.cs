using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionaireQuiz
{
    public class TriviaQuestion
    {
        public string Difficulty { get; set; }
        public string Question { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public static class QuestionParser
    {
        public static TriviaQuestion Parse(string raw)
        {
            var lines = raw.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var q = new TriviaQuestion();

            foreach (var line in lines)
            {
                if (line.StartsWith("Difficulty:", StringComparison.OrdinalIgnoreCase))
                    q.Difficulty = line.Substring("Difficulty:".Length).Trim();

                else if (line.StartsWith("Question:", StringComparison.OrdinalIgnoreCase))
                    q.Question = line.Substring("Question:".Length).Trim();

                else if (line.StartsWith("A.", StringComparison.OrdinalIgnoreCase))
                    q.OptionA = line.Substring(2).Trim();

                else if (line.StartsWith("B.", StringComparison.OrdinalIgnoreCase))
                    q.OptionB = line.Substring(2).Trim();

                else if (line.StartsWith("C.", StringComparison.OrdinalIgnoreCase))
                    q.OptionC = line.Substring(2).Trim();

                else if (line.StartsWith("D.", StringComparison.OrdinalIgnoreCase))
                    q.OptionD = line.Substring(2).Trim();

                else if (line.StartsWith("Correct Answer:", StringComparison.OrdinalIgnoreCase))
                    q.CorrectAnswer = line.Substring("Correct Answer:".Length).Trim();
            }

            return q;
        }
    }
}
