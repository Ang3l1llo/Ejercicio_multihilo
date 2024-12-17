class JuegoDosHilos
{
    static readonly string[] choices = { "Rock", "Paper", "Scissors" }; 
    static readonly Random random = new Random();
    static readonly object locker = new object(); // Para sincronizar las elecciones

    static string choice1 = "";
    static string choice2 = "";

    static int winsThread1 = 0; 
    static int winsThread2 = 0; 

    static void Main()
    {
        Console.WriteLine("Game between threads - Best of 3:");

        int round = 1; // Contador de rondas

        while (winsThread1 < 2 && winsThread2 < 2)
        {
            Console.WriteLine($"\nRound {round}");

            Thread thread1 = new Thread(() => PlayGame("Thread 1", ref choice1));
            Thread thread2 = new Thread(() => PlayGame("Thread 2", ref choice2));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            if (!DetermineWinner()) // Si hay empate, se repite
            {
                Console.WriteLine("This round is a draw, replaying...\n");
                continue;
            }

            round++;
        }

        if (winsThread1 == 2)
            Console.WriteLine("\nThread 1 wins the match!");
        else if (winsThread2 == 2)
            Console.WriteLine("\nThread 2 wins the match!");
    }

    static void PlayGame(string threadName, ref string choice)
    {
        lock (locker)
        {
            choice = choices[random.Next(choices.Length)];
            Console.WriteLine($"{threadName} chose: {choice}");
        }
    }

    static bool DetermineWinner()
    {
        Console.WriteLine($"\nResults:\nThread 1: {choice1}\nThread 2: {choice2}");

        if (choice1 == choice2)
        {
            return false; 
        }
        else if ((choice1 == "Rock" && choice2 == "Scissors") ||
                 (choice1 == "Paper" && choice2 == "Rock") ||
                 (choice1 == "Scissors" && choice2 == "Paper"))
        {
            Console.WriteLine("Thread 1 wins this round!");
            winsThread1++;
        }
        else
        {
            Console.WriteLine("Thread 2 wins this round!");
            winsThread2++;
        }

        return true; 
    }
}

