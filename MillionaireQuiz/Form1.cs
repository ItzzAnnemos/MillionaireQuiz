namespace MillionaireQuiz
{
    public partial class Form1 : Form
    {
        List<string> list;
        Question q;
        int questionCounter = 0;
        Button[] buttons;
        TextBox[] questionOrder;
        TextBox[] moneyAchieved;
        string[] allOptions;

        public Form1()
        {
            InitializeComponent();
            buttons = new Button[] { button1, button2, button3, button4 };
            questionOrder = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12, textBox13, textBox14, textBox15 };
            moneyAchieved = new TextBox[] { textBox16, textBox17, textBox18, textBox19, textBox20, textBox21, textBox22, textBox23, textBox24, textBox25, textBox26, textBox27, textBox28, textBox29, textBox30 };
            allOptions = new string[] { "A", "B", "C", "D" };            
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            bool loaded = await FetchAndLoadNewQuestionAsync("Easy");
            if (!loaded)
            {
                MessageBox.Show("Could not load the first question. Exiting.");
                Application.Exit();
            }
        }

        private async Task<bool> FetchAndLoadNewQuestionAsync(string difficulty)
        {
            string rawQuestion = await GenerateQuestion.GenerateQuestionAsync(difficulty);

            if (string.IsNullOrWhiteSpace(rawQuestion))
            {
                MessageBox.Show("Failed to get question from Gemini API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                TriviaQuestion triviaQuestion = QuestionParser.Parse(rawQuestion);

                if (string.IsNullOrEmpty(triviaQuestion.Question) ||
                    string.IsNullOrEmpty(triviaQuestion.OptionA) ||
                    string.IsNullOrEmpty(triviaQuestion.OptionB) ||
                    string.IsNullOrEmpty(triviaQuestion.OptionC) ||
                    string.IsNullOrEmpty(triviaQuestion.OptionD) ||
                    string.IsNullOrEmpty(triviaQuestion.CorrectAnswer))
                {
                    MessageBox.Show("Received invalid question format from API.", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                int correctIndex = triviaQuestion.CorrectAnswer.ToUpper() switch
                {
                    "A" => 0,
                    "B" => 1,
                    "C" => 2,
                    "D" => 3,
                    _ => -1
                };

                if (correctIndex == -1)
                {
                    MessageBox.Show("Invalid correct answer letter in question data.", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                List<string> answers = new List<string>
        {
            triviaQuestion.OptionA,
            triviaQuestion.OptionB,
            triviaQuestion.OptionC,
            triviaQuestion.OptionD
        };

                q = new Question(triviaQuestion.Question, answers, correctIndex, triviaQuestion.Difficulty ?? difficulty);

                LoadQuestion();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error parsing question: " + ex.Message, "Parsing Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private void LoadQuestion()
        {
            questionTextBox.Text = q.question;
            button1.Text = q.answers[0];
            button2.Text = q.answers[1];
            button3.Text = q.answers[2];
            button4.Text = q.answers[3];

            foreach (Button btn in buttons)
            {
                btn.Enabled = true;
                btn.Visible = true;
                btn.BackColor = SystemColors.Control;

                btn.FlatStyle = FlatStyle.Standard;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = SystemColors.ControlDark;
            }
        }

        private async void checkCorrect(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton.Name.Equals("button1") && q.correct == 0)
            {
                clickedButton.BackColor = Color.Green;
                updateScreen();
            }
            else if (clickedButton.Name.Equals("button2") && q.correct == 1)
            {
                clickedButton.BackColor = Color.Green;
                updateScreen();
            }
            else if (clickedButton.Name.Equals("button3") && q.correct == 2)
            {
                clickedButton.BackColor = Color.Green;
                updateScreen();
            }
            else if (clickedButton.Name.Equals("button4") && q.correct == 3)
            {
                clickedButton.BackColor = Color.Green;
                updateScreen();
            }
            else
            {
                MessageBox.Show("Your answer is incorrect! The game is over!");
                await Task.Delay(1000);
                Application.Exit();
            }
        }

        private async void updateScreen()
        {
            
            if (questionCounter < questionOrder.Length)
            {
                questionOrder[questionCounter].BackColor = Color.Green;
                moneyAchieved[questionCounter].BackColor = Color.Green;
                questionCounter++;
                await Task.Delay(1000);

                string difficulty = GetDifficultyByQuestionNumber(questionCounter);

                bool loaded = await FetchAndLoadNewQuestionAsync(difficulty);
                if (!loaded)
                {
                    MessageBox.Show("The next question could not be loaded. The game will now end.");
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("The quiz has ended.!");
                await Task.Delay(5000);
                Application.Exit();
            }
        }

        private string GetDifficultyByQuestionNumber(int questionNumber)
        {
            if (questionNumber < 5) return "Easy";
            else if (questionNumber < 10) return "Medium";
            else return "Hard";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int correctAnswer = q.correct;
            string correctLetter = allOptions[correctAnswer];
            string friendLetter = "";
            double chance = 0;

            if (q.difficulty.Equals("Easy"))
                chance = 0.9;
            else if (q.difficulty.Equals("Medium"))
                chance = 0.7;
            else if (q.difficulty.Equals("Hard"))
                chance = 0.5;

            if (rand.NextDouble() <= chance)
            {
                friendLetter = correctLetter;
            }
            else
            {
                List<string> wrongOptions = allOptions.Where(opt => opt != correctLetter).ToList();
                friendLetter = wrongOptions[rand.Next(wrongOptions.Count)];
            }

            Button clickedButton = (Button)sender;

            if (clickedButton == callFriendButton)
                {
                MessageBox.Show("I believe the correct answer is " + friendLetter, "Phone a Friend");
                }
            else if (clickedButton == askAudienceButton)
            {
                MessageBox.Show("The audience votes for: " + friendLetter, "Ask the Audience");
            }

            clickedButton.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int correct = q.correct;
            List<int> all = new List<int> { 0, 1, 2, 3 };
            List<int> wrong = all.Where(i => i != correct).ToList();
            Random rand = new Random();
            List<int> toEliminate = new List<int>();

            if (q.difficulty == "Easy")
            {
                toEliminate = wrong.OrderBy(x => rand.Next()).Take(2).ToList();
            }
            else if (q.difficulty == "Medium")
            {
                int keepWrong = wrong.OrderBy(i => Math.Abs(i - correct)).First();
                toEliminate = wrong.Where(i => i != keepWrong).ToList();
            }
            else
            {
                if (rand.NextDouble() <= 0.2)
                {
                    int randomWrong = wrong[rand.Next(wrong.Count)];
                    toEliminate.Add(correct);
                    toEliminate.Add(randomWrong);
            }
                else
            {
                    toEliminate = wrong.OrderBy(x => rand.Next()).Take(2).ToList();
                }
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                if (!toEliminate.Contains(i))
            {
                    buttons[i].FlatStyle = FlatStyle.Flat;
                    buttons[i].FlatAppearance.BorderSize = 3;
                    buttons[i].FlatAppearance.BorderColor = Color.Orange;
                }
            }

            fiftyfiftyButton.Enabled = false;
        }
    }
}
