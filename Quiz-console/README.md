# Frågesport

## Fråga till AI

Jag vill ha koden till ett quiz-spel i C#. Frågor och svar ska hämtas från Internet. Jag vill att spelaren får ange namn och att resultat lagras i en highscore-tabell.

## Svar

Här är ett exempel på en C#-kod som implementerar ett enkelt quiz-spel med frågor och svar från Internet. Spelaren får ange sitt namn och resultatet sparas i en highscore-tabell. Observera att detta är en grundläggande implementation och du kan anpassa den och utöka den efter dina egna behov.
```
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
                    QuizQuestion question = new QuizQuestion {
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
        HighScoreEntry entry = new HighScoreEntry {
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
        QuizGame quizGame = new QuizGame();
        quizGame.LoadQuestionsFromAPI();
        quizGame.PlayQuiz();
    }
}

```
Detta kodexempel använder `WebClient`-klassen för att hämta frågor och svar från en webb-API (i detta fall använder jag `opentdb.com` som levererar slumpmässiga trivia-frågor). JSON-data från API:et konverteras till objekt med hjälp av `JsonConvert.DeserializeObject`-metoden. Spelaren matar in svarsalternativet som de tror är korrekt och deras resultat sparas i en highscore-tabell. Resultatet visas sedan på skärmen.

## Fråga till AI

	
När spelet är slut vill jag att det ska gå att spela igen.

## Svar

För att möjliggöra att spelet kan spelas igen behöver vi göra några ändringar i koden. Här är en uppdaterad version av koden där spelaren har möjlighet att välja om de vill spela igen eller inte:

```
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
                    QuizQuestion question = new QuizQuestion {
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
        HighScoreEntry entry = new HighScoreEntry {
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

```
I denna version av koden används en oändlig loop `while (true)` som gör att spelaren kan spela spelet flera gånger. Efter varje runda frågas spelaren om de vill spela igen. Om svaret inte är "y" (för ja), avslutas loopen och programmet stängs av.

## Fråga till AI

Kan du ge förslag på hur spelet kan förbättras?

## Svar

Absolut! Här är några förslag på hur du kan förbättra spelet:

1. **Anpassa svårighetsgraden:** Låt spelaren välja svårighetsgrad innan spelet börjar. Du kan erbjuda olika svårighetsnivåer med olika typer av frågor och olika antal poäng för varje korrekt svar.

2. **Tidsbegränsning:** Lägg till en tidsbegränsning för varje fråga. Spelaren måste svara inom en viss tid annars betraktas det som fel svar.

3. **Livsystem:** Ge spelaren ett visst antal liv i början av spelet. För varje felaktigt svar förlorar spelaren ett liv. Om spelaren förlorar alla liv är spelet slut.

4. **Belöningar och power-ups:** Ge spelaren möjligheten att få belöningar eller power-ups genom att svara på frågor korrekt. Det kan vara något som hjälper spelaren att svara på svåra frågor eller ökar deras poäng.

5. **Dynamiska frågor:** Hämta inte bara en uppsättning frågor från API:et i början, utan ladda in nya frågor under spelets gång för att hålla det intressant och varierat.

6. **Visuell design:** Förbättra användargränssnittet med grafik, färger och animationer för att göra spelet mer engagerande och attraktivt.

7. **Multiplayer-läge:** Implementera ett flerspelarläge där spelarna kan tävla mot varandra i realtid eller genom att jämföra sina highscores.

Dessa är bara några exempel på hur spelet kan förbättras och anpassas efter dina egna preferenser. Använd din kreativitet för att göra spelet unikt och roligt att spela!
