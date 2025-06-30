using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionaireQuiz
{
    class Question
    {
        public String question;
        public List<String> answers;
        public int correct;
        public String difficulty;
        
        public Question(String question, List<String> answers, int correct, String difficulty)
        {
            this.question = question;
            this.answers = answers;
            this.correct = correct;
            this.difficulty = difficulty;
        }
    }
}
