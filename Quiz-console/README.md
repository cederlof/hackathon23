# Fr�gesport

## Fr�ga till AI

Jag vill ha koden till ett quiz-spel i C#. Fr�gor och svar ska h�mtas fr�n Internet. Jag vill att spelaren f�r ange namn och att resultat lagras i en highscore-tabell.

## Svar

H�r �r ett exempel p� en C#-kod som implementerar ett enkelt quiz-spel med fr�gor och svar fr�n Internet. Spelaren f�r ange sitt namn och resultatet sparas i en highscore-tabell. Observera att detta �r en grundl�ggande implementation och du kan anpassa den och ut�ka den efter dina egna behov.
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
            // Ladda ner JSON-data fr�n API:et
            string json = webClient.DownloadString(apiUrl);
            // Konvertera JSON till objekt
            dynamic data = JsonConvert.DeserializeObject(json);
            if (data != null && data.results != null)
            {
                // Loopa igenom fr�gorna i datan och skapa QuizQuestion-objekt
                foreach (var result in data.results)
                {
                    QuizQuestion question = new QuizQuestion {
                        Question = result.question,
                        CorrectAnswer = result.correct_answer,
                        IncorrectAnswers = new List<string>()
                    };
                    // L�gg till de inkorrekta svar som finns i datan
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
        // Slumpa fr�gor
        Random random = new Random();
        List<QuizQuestion> randomQuestions = new List<QuizQuestion>(questions);
        randomQuestions.Shuffle(random);
        int score = 0;
        // Fr�ga varje fr�ga i den slumpade ordningen
        foreach (var question in randomQuestions)
        {
            Console.WriteLine(question.Question);
            // Slumpa ordningen p� svarsalternativen
            List<string> options = new List<string>(question.IncorrectAnswers);
            options.Add(question.CorrectAnswer);
            options.Shuffle(random);
            // Visa svarsalternativen
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            // L�s spelarens svar
            int playerChoice;
            do
            {
                Console.Write("Your choice (1-4): ");
            } while (!int.TryParse(Console.ReadLine(), out playerChoice) || playerChoice < 1 || playerChoice > 4);
            // Kolla om spelarens svar �r korrekt
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
        // Visa spelarens resultat och fr�ga efter namn
        Console.WriteLine($"Your score: {score}");
        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine();
        // L�gg till resultatet i highscore-tabellen
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
Detta kodexempel anv�nder `WebClient`-klassen f�r att h�mta fr�gor och svar fr�n en webb-API (i detta fall anv�nder jag `opentdb.com` som levererar slumpm�ssiga trivia-fr�gor). JSON-data fr�n API:et konverteras till objekt med hj�lp av `JsonConvert.DeserializeObject`-metoden. Spelaren matar in svarsalternativet som de tror �r korrekt och deras resultat sparas i en highscore-tabell. Resultatet visas sedan p� sk�rmen.

## Fr�ga till AI

	
N�r spelet �r slut vill jag att det ska g� att spela igen.

## Svar

F�r att m�jligg�ra att spelet kan spelas igen beh�ver vi g�ra n�gra �ndringar i koden. H�r �r en uppdaterad version av koden d�r spelaren har m�jlighet att v�lja om de vill spela igen eller inte:

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
            // Ladda ner JSON-data fr�n API:et
            string json = webClient.DownloadString(apiUrl);
            // Konvertera JSON till objekt
            dynamic data = JsonConvert.DeserializeObject(json);
            if (data != null && data.results != null)
            {
                // Loopa igenom fr�gorna i datan och skapa QuizQuestion-objekt
                foreach (var result in data.results)
                {
                    QuizQuestion question = new QuizQuestion {
                        Question = result.question,
                        CorrectAnswer = result.correct_answer,
                        IncorrectAnswers = new List<string>()
                    };
                    // L�gg till de inkorrekta svar som finns i datan
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
        // Slumpa fr�gor
        Random random = new Random();
        List<QuizQuestion> randomQuestions = new List<QuizQuestion>(questions);
        randomQuestions.Shuffle(random);
        int score = 0;
        // Fr�ga varje fr�ga i den slumpade ordningen
        foreach (var question in randomQuestions)
        {
            Console.WriteLine(question.Question);
            // Slumpa ordningen p� svarsalternativen
            List<string> options = new List<string>(question.IncorrectAnswers);
            options.Add(question.CorrectAnswer);
            options.Shuffle(random);
            // Visa svarsalternativen
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            // L�s spelarens svar
            int playerChoice;
            do
            {
                Console.Write("Your choice (1-4): ");
            } while (!int.TryParse(Console.ReadLine(), out playerChoice) || playerChoice < 1 || playerChoice > 4);
            // Kolla om spelarens svar �r korrekt
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
        // Visa spelarens resultat och fr�ga efter namn
        Console.WriteLine($"Your score: {score}");
        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine();
        // L�gg till resultatet i highscore-tabellen
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
            // Fr�ga spelaren om de vill spela igen
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
I denna version av koden anv�nds en o�ndlig loop `while (true)` som g�r att spelaren kan spela spelet flera g�nger. Efter varje runda fr�gas spelaren om de vill spela igen. Om svaret inte �r "y" (f�r ja), avslutas loopen och programmet st�ngs av.

## Fr�ga till AI

Kan du ge f�rslag p� hur spelet kan f�rb�ttras?

## Svar

Absolut! H�r �r n�gra f�rslag p� hur du kan f�rb�ttra spelet:

1. **Anpassa sv�righetsgraden:** L�t spelaren v�lja sv�righetsgrad innan spelet b�rjar. Du kan erbjuda olika sv�righetsniv�er med olika typer av fr�gor och olika antal po�ng f�r varje korrekt svar.

2. **Tidsbegr�nsning:** L�gg till en tidsbegr�nsning f�r varje fr�ga. Spelaren m�ste svara inom en viss tid annars betraktas det som fel svar.

3. **Livsystem:** Ge spelaren ett visst antal liv i b�rjan av spelet. F�r varje felaktigt svar f�rlorar spelaren ett liv. Om spelaren f�rlorar alla liv �r spelet slut.

4. **Bel�ningar och power-ups:** Ge spelaren m�jligheten att f� bel�ningar eller power-ups genom att svara p� fr�gor korrekt. Det kan vara n�got som hj�lper spelaren att svara p� sv�ra fr�gor eller �kar deras po�ng.

5. **Dynamiska fr�gor:** H�mta inte bara en upps�ttning fr�gor fr�n API:et i b�rjan, utan ladda in nya fr�gor under spelets g�ng f�r att h�lla det intressant och varierat.

6. **Visuell design:** F�rb�ttra anv�ndargr�nssnittet med grafik, f�rger och animationer f�r att g�ra spelet mer engagerande och attraktivt.

7. **Multiplayer-l�ge:** Implementera ett flerspelarl�ge d�r spelarna kan t�vla mot varandra i realtid eller genom att j�mf�ra sina highscores.

Dessa �r bara n�gra exempel p� hur spelet kan f�rb�ttras och anpassas efter dina egna preferenser. Anv�nd din kreativitet f�r att g�ra spelet unikt och roligt att spela!
