using System;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            // ASCII Art Title Screen
            string asciiArt = @"
      .----------------.  
     | .--------------. |  
     | |  CYBERSEC   | |  
     | |  AWARENESS  | |  
     | |     BOT     | |  
     | '--------------' |  
      '----------------'  
        [==]    [==]   
       /~~~~\  /~~~~\   
      | |  | || |  | |   
      |_|  |_||_|  |_|  
            ";

            Console.WriteLine(asciiArt);

            // Personalized Welcome Message
            SoundPlayer player = new SoundPlayer(@"audio.wav"); // Add your own audio file
            player.PlaySync(); // Play audio synchronously

            // Ask user to enter their name
            Console.Write("Please enter your name: ");
            string userName = Console.ReadLine();

            // Welcome message
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nWelcome, {userName}, to the Cybersecurity Awareness Bot!");
            Console.ResetColor();
            Console.WriteLine("I'm here to help you stay safe online.");

            // Display available questions
            DisplayAvailableQuestions();

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

        static void DisplayAvailableQuestions()
        {
            Console.WriteLine("\nYou can ask me the following questions:");
            Console.WriteLine("1. What is phishing?");
            Console.WriteLine("2. How to create a strong password?");
            Console.WriteLine("3. [Enter a valid email address to check its format]");
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

            // Dictionary to map inputs to responses
            var responses = new Dictionary<string, string>
            {
                { "how are you?", "I'm just a bot, so I don't have feelings, but thank you for asking!" },
                { "what's your purpose?", "My purpose is to help you stay safe online by providing information about cybersecurity." },
                { "what is phishing?", $"Phishing, {userName}, is a type of cyberattack where attackers attempt to trick you into giving them your personal information, such as your passwords or credit card numbers." },
                { "how to create a strong password?", $"To create a strong password, {userName}, use a combination of uppercase and lowercase letters, numbers, and symbols. Make it at least 12 characters long and avoid using personal information or common words." }
            };

            if (responses.ContainsKey(input))
            {
                response = responses[input];
            }
            else if (Regex.IsMatch(input, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                response = $"That looks like a valid email address, {userName}!";
            }
            else
            {
                response = $"I didn't quite understand that, {userName}. Could you rephrase?";
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
