using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
class QuizQuestion
{
    public string Question { get; set; }
    public string CorrectAnswer { get; set; }
    public List<string> IncorrectAnswers { get; set; }
}
class HighScoreEntry
{
    public string PlayerName { get; set; }
    public int Score { get; set; }
}
class QuizGame
{
    private List<QuizQuestion> questions;
    private List<HighScoreEntry> highScores;
    public QuizGame()
    {
        questions = new List<QuizQuestion>();
        highScores = new List<HighScoreEntry>();
    }
    public void LoadQuestionsFromAPI()
    {
        string apiUrl = "https://opentdb.com/api.php?amount=5&type=multiple";
        using (var webClient = new WebClient())
        {
            // Ladda ner JSON-data från API:et
            string json = webClient.DownloadString(apiUrl);
            // Konvertera JSON till objekt
            dynamic data = JsonConvert.DeserializeObject(json);
            if (data != null && data.results != null)
            {
                // Loopa igenom frågorna i datan och skapa QuizQuestion-objekt
                foreach (var result in data.results)
                {
                    QuizQuestion question = new QuizQuestion
                    {
                        Question = result.question,
                        CorrectAnswer = result.correct_answer,
                        IncorrectAnswers = new List<string>()
                    };
                    // Lägg till de inkorrekta svar som finns i datan
                    foreach (var incorrectAnswer in result.incorrect_answers)
                    {
                        question.IncorrectAnswers.Add(incorrectAnswer.ToString());
                    }
                    questions.Add(question);
                }
            }
        }
    }
    public void AddHighScoreEntry(string playerName, int score)
    {
        HighScoreEntry entry = new HighScoreEntry
        {
            PlayerName = playerName,
            Score = score
        };
        highScores.Add(entry);
    }
    public void ShowHighScores()
    {
        Console.WriteLine("High Scores:");
        foreach (var entry in highScores)
        {
            Console.WriteLine($"{entry.PlayerName}: {entry.Score}");
        }
    }
    public void PlayQuiz()
    {
        // Slumpa frågor
        Random random = new Random();
        List<QuizQuestion> randomQuestions = new List<QuizQuestion>(questions);
        randomQuestions.Shuffle(random);
        int score = 0;
        // Fråga varje fråga i den slumpade ordningen
        foreach (var question in randomQuestions)
        {
            Console.WriteLine(question.Question);
            // Slumpa ordningen på svarsalternativen
            List<string> options = new List<string>(question.IncorrectAnswers);
            options.Add(question.CorrectAnswer);
            options.Shuffle(random);
            // Visa svarsalternativen
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            // Läs spelarens svar
            int playerChoice;
            do
            {
                Console.Write("Your choice (1-4): ");
            } while (!int.TryParse(Console.ReadLine(), out playerChoice) || playerChoice < 1 || playerChoice > 4);
            // Kolla om spelarens svar är korrekt
            if (options[playerChoice - 1] == question.CorrectAnswer)
            {
                Console.WriteLine("Correct!");
                score++;
            }
            else
            {
                Console.WriteLine("Incorrect!");
            }
            Console.WriteLine();
        }
        // Visa spelarens resultat och fråga efter namn
        Console.WriteLine($"Your score: {score}");
        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine();
        // Lägg till resultatet i highscore-tabellen
        AddHighScoreEntry(playerName, score);
        Console.WriteLine();
        Console.WriteLine("High Scores:");
        ShowHighScores();
    }
}
static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list, Random random)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        while (true)  // Loppa spelet tills spelaren vill avsluta
        {
            QuizGame quizGame = new QuizGame();
            quizGame.LoadQuestionsFromAPI();
            quizGame.PlayQuiz();
            // Fråga spelaren om de vill spela igen
            Console.Write("Play again? (y/n): ");
            string playAgain = Console.ReadLine();
            if (playAgain.ToLower() != "y")
            {
                break; // Avsluta om spelaren inte vill spela igen
            }
        }
    }
}
