using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public static class GenerateQuestion
{
    private const string ApiKey = "AIzaSyANVvT-VcWtZ_1uA8y1fARoKnvXqGZGLsI"; // Replace with your actual key
    private static readonly string Endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={ApiKey}";

    public static async Task<string> GenerateQuestionAsync(string difficulty)
    {
        using var client = new HttpClient();
        string[] categories = {
            "History", "Science", "Geography", "Literature", "Art",
            "Sports", "Technology", "Pop Culture", "Mythology", "Food and Drink"
        };
        Random rand = new Random();
        string randomCategory = categories[rand.Next(categories.Length)];

        string prompt = $@"
Generate a single, unique, general knowledge multiple-choice trivia question.
The question should be of '{difficulty}' difficulty.
Ensure the question is not a repetition of any previously generated trivia questions if you have access to such history (as a general guideline for the model, not a strict instruction to remember state).
The topic for this question should lean towards '{randomCategory}'.
The question must adhere strictly to the following format:

Difficulty: {difficulty}

Question: <question text, be creative and avoid common trivia facts>

A. <option A, ensure plausible but incorrect answers>
B. <option B, ensure plausible but incorrect answers>
C. <option C, ensure plausible but incorrect answers>
D. <option D, ensure the single correct answer>

Correct Answer: <A/B/C/D>

Focus on creating fresh, engaging content. Do not generate explanations or additional text beyond the specified format.
";

        var json = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            },
            generationConfig = new
            {
                temperature = 1.0,
                topK = 40,
                topP = 0.95
            }
        };

        var requestBody = new StringContent(
            Newtonsoft.Json.JsonConvert.SerializeObject(json),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync(Endpoint, requestBody);
        var responseBody = await response.Content.ReadAsStringAsync();

        try
        {
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(
                    $"❌ API ERROR: {response.StatusCode}\n\n{responseBody}",
                    "Gemini API Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return null;
            }

            var jsonObj = JObject.Parse(responseBody);
            var text = jsonObj["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show(
                    $"❌ No text found in Gemini response.\n\nFull Response:\n{responseBody}",
                    "Empty Response",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return null;
            }

            return text;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"❌ Exception while parsing Gemini response:\n\n{ex.Message}\n\nRaw response:\n{responseBody}",
                "Parsing Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            return null;
        }

    }
}
