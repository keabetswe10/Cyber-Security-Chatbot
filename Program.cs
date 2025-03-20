using System;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading;

namespace CybersecurityChatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            // ASCII Art Title Screen
            string asciiArt = @"
      _________________________________________
     | |
     | CYBERSECURITY AWARENESS BOT |
     | [CAB] |
     |_________________________________________|
             
                  /\ /\
                 / \_______________/ \
                /_______________________\
                \ /
                 \ /
                  \__________________/
            ";

            Console.WriteLine(asciiArt);

            // Personalized Welcome Message
            SoundPlayer player = new SoundPlayer(@"audio.wav"); // Add your own audio file
            player.Play();

            // Ask user to enter their name
            Console.Write("Please enter your name: ");
            string userName = Console.ReadLine();

            // Welcome message
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nWelcome, " + userName + " , to the Cybersecurity Awareness Bot!");
            Console.ResetColor();
            Console.WriteLine("I'm here to help you stay safe online.");

            // Tell the user to ask a question
            Console.WriteLine("\n------------------------------------");
            Console.WriteLine(" Ask Me a Question");
            Console.WriteLine("------------------------------------\n");

            Console.WriteLine("Ask me a question (or type 'exit' to quit):");

            string input;
            while ((input = Console.ReadLine()) != "exit")
            {
                // Tell the user to ask another question
                ProcessInput(input, userName);
                Thread.Sleep(500); // Pause before asking the next question
                Console.WriteLine("Ask another question:");
            }
        }

        static void ProcessInput(string input, string userName)
        {
            input = input.Trim();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Please enter a question.");
                return; // Exit the method
            }

            input = input.ToLower(); 
            

            Console.ForegroundColor = ConsoleColor.Yellow; // Color for responses
            string response = ""; // Store the response in a variable

            if (input == "what is phishing?")
            {
                response = "Phishing, " + userName + " , is a type of cyberattack where attackers attempt to trick you into giving them your personal information, such as your passwords or credit card numbers.";
            }
            else if (input == "how to create strong password?")
            {
                response = "To create a strong password, " + userName + " , use a combination of uppercase and lowercase letters, numbers, and symbols. Make it at least 12 characters long and avoid using personal information or common words.";
            }
            else if (Regex.IsMatch(input, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                response = "That looks like a valid email address, " + userName + "!";
            }
            else
            {
                response = "I didn't quite understand that, " + userName + " . Could you rephrase?";
            }

            // Typing Effect
            foreach (char c in response)
            {
                Console.Write(c);
                Thread.Sleep(30); // Fix the typing speed
            }

            Console.WriteLine(); // Move to the next line after typing
            Console.ResetColor();
        }
    }
}
