using System;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using System.Linq; // For Contains with arrays

namespace CybersecurityChatbot
{
    class Program
    {
        // Static Random instance for generating random numbers
        static Random random = new Random();
        static string currentTopic = ""; // To keep track of the current conversation topic
        static string userName = ""; // To store the user's name
        static string favoriteTopic = ""; // To store the user's favorite cybersecurity topic
        static bool rememberedFavorite = false; // Flag to track if we've acknowledged the favorite topic
        static readonly string[] worriedKeywords = { "worried", "concerned", "anxious", "afraid", "nervous" };

        static void Main(string[] args)
        {
            // Display the title screen
            DisplayTitleScreen();

            // Play the welcome audio
            PlayWelcomeAudio();

            // Get the user's name
            userName = GetUserName();

            // Display the personalized welcome message
            DisplayWelcomeMessage(userName);

            // Show the user available questions and topics
            DisplayAvailableQuestions();

            // Start the main chat loop
            StartChatLoop(userName);
        }

        #region UI Functions (Display)

        // Displays the ASCII art title screen
        static void DisplayTitleScreen()
        {
            string asciiArt = @"
    .----------------.
    | .--------------. |
    | |  CYBERSEC    | |
    | |  AWARENESS   | |
    | |     BOT      | |
    | '--------------' |
    '----------------'
        [==]    [==]
       /~~~~\  /~~~~\
      | |  | || |  | |
      |_|  |_||_|  |_|
                    ";
            Console.WriteLine(asciiArt);
        }

        // Displays the available questions and topics to the user
        static void DisplayAvailableQuestions()
        {
            Console.WriteLine("\nYou can ask me the following questions:");
            Console.WriteLine("1. What is phishing?");
            Console.WriteLine("2. How to create a strong password?");
            Console.WriteLine("3. Give me a phishing tip");
            Console.WriteLine("4. Give me a password tip");
            Console.WriteLine("5. [Enter a valid email address to check its format]");
            Console.WriteLine("\nYou can also mention keywords like:");
            Console.WriteLine("- password");
            Console.WriteLine("- scam");
            Console.WriteLine("- privacy");
            Console.WriteLine("\nYou can also tell me your favorite topic, like: 'I'm interested in privacy.'");
            Console.WriteLine("\nYou can also express your feelings, like: 'I'm worried about scams.'");
        }

        // Displays the personalized welcome message
        static void DisplayWelcomeMessage(string currentUserName)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nWelcome, {currentUserName}, to the Cybersecurity Awareness Bot!");
            Console.ResetColor();
            Console.WriteLine("I'm here to help you stay safe online.");
        }

        // Displays the typing effect for the chatbot's response
        static void DisplayTypingEffect(string response)
        {
            foreach (char c in response)
            {
                Console.Write(c);
                Thread.Sleep(30); // Typing speed
            }
            Console.WriteLine();
        }

        #endregion

        #region Audio and Input Functions

        // Plays the welcome audio
        static void PlayWelcomeAudio()
        {
            try
            {
                SoundPlayer player = new SoundPlayer(@"audio.wav"); // Add your own audio file
                player.PlaySync(); // Play audio synchronously
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing audio: " + ex.Message);
            }
        }

        // Gets the username from the user
        static string GetUserName()
        {
            Console.Write("Please enter your name: ");
            return Console.ReadLine();
        }

        #endregion

        #region Chat Logic (Main Loop and Response Handling)

        // Starts the main chat loop to continuously get user input
        static void StartChatLoop(string currentUserName)
        {
            Console.WriteLine("\n------------------------------------");
            Console.WriteLine("       Ask Me a Question");
            Console.WriteLine("------------------------------------\n");

            Console.WriteLine("Ask me a question (or type 'exit' to quit):");

            string input;
            while ((input = Console.ReadLine()) != "exit")
            {
                ProcessInput(input, currentUserName);
                Thread.Sleep(500); // Pause before asking the next question
                Console.WriteLine("Ask another question:");
            }
        }

        // Processes the user's input and calls the function to get the appropriate response
        static void ProcessInput(string input, string currentUserName)
        {
            input = input.Trim().ToLower();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Please enter a question.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;

            string response = GetResponse(input, currentUserName);
            DisplayTypingEffect(response);

            Console.ResetColor();
        }

        // Gets the appropriate response based on the user's input
        static string GetResponse(string input, string currentUserName)
        {
            // Sentiment Detection for "worried"
            if (worriedKeywords.Any(input.Contains))
            {
                string worriedWord = worriedKeywords.First(input.Contains);
                return $"It's completely understandable to feel {worriedWord} about cybersecurity, {currentUserName}. Let me share some information to help you feel more secure. What specifically has you feeling {worriedWord}?";
            }

            // Check if the user is expressing interest in a topic
            if (input.StartsWith("i'm interested in ") && !rememberedFavorite)
            {
                favoriteTopic = input.Substring("i'm interested in ".Length).Trim();
                rememberedFavorite = true;
                currentTopic = favoriteTopic;
                return $"Great! I'll remember that you're interested in {favoriteTopic}, {currentUserName}. It's a crucial part of staying safe online.";
            }
            else if (input.StartsWith("i am interested in ") && !rememberedFavorite)
            {
                favoriteTopic = input.Substring("i am interested in ".Length).Trim();
                rememberedFavorite = true;
                currentTopic = favoriteTopic;
                return $"Great! I'll remember that you're interested in {favoriteTopic}, {currentUserName}. It's a crucial part of staying safe online.";
            }

            var responses = new Dictionary<string, string>
            {
                { "how are you?", "I'm just a bot, so I don't have feelings, but thank you for asking!" },
                { "what's your purpose?", "My purpose is to help you stay safe online by providing information about cybersecurity." },
                { "what is phishing?", $"Phishing, {currentUserName}, is a type of cyberattack where attackers attempt to trick you into giving them your personal information, such as your passwords or credit card numbers." },
                { "how to create a strong password?", $"To create a strong password, {currentUserName}, use a combination of uppercase and lowercase letters, numbers, and symbols. Make it at least 12 characters long and avoid using personal information or common words." },
                { "give me a phishing tip", GetRandomPhishingTip(currentUserName) },
                { "give me a password tip", GetRandomPasswordTip(currentUserName) }
            };

            if (responses.ContainsKey(input))
            {
                currentTopic = input; // Set the current topic
                return responses[input];
            }

            if (Regex.IsMatch(input, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled))
            {
                currentTopic = "email validation"; // Set the current topic
                return $"That looks like a valid email address, {currentUserName}!";
            }

            var keywords = new Dictionary<string, string>
            {
                { "password", GetRandomPasswordTip(currentUserName) },
                { "scam", GetRandomScamTip(currentUserName) },
                { "privacy", GetRandomPrivacyTip(currentUserName) }
            };

            foreach (var keywordPair in keywords)
            {
                if (input.Contains(keywordPair.Key))
                {
                    currentTopic = keywordPair.Key; // Set the current topic
                    return $"{currentUserName}, when you mention '{keywordPair.Key}', it's important to know that {keywordPair.Value}";
                }
            }

            // Handle follow-up questions or requests for more details ONLY if no direct match or keyword is found
            if (input.Contains("tell me more about") || input.Contains("explain further") || input.Contains("what do you mean by"))
            {
                return GetFollowUpResponse(currentTopic, currentUserName);
            }

            // Personalised response based on favorite topic (example)
            if (!string.IsNullOrEmpty(favoriteTopic) && input.ToLower().Contains(favoriteTopic) && rememberedFavorite)
            {
                return $"Since you're interested in {favoriteTopic}, {currentUserName}, here's a specific tip related to it: {GetPersonalizedTip(favoriteTopic, currentUserName)}";
            }

            // Default response for unknown inputs (more helpful)
            return $"I'm not sure I understand, {currentUserName}. You can ask me about topics like phishing, passwords, scams, or privacy. You can also ask for a tip on phishing or passwords, or tell me your favorite topic.";
        }

        // Gets a follow-up response based on the current topic
        static string GetFollowUpResponse(string topic, string currentUserName)
        {
            switch (topic)
            {
                case "what is phishing?":
                    return $"Phishing often involves deceptive emails or websites that look legitimate but are designed to steal your sensitive information. For example, they might ask for your login details or credit card numbers.";
                case "how to create a strong password?":
                    return $"Besides length and character variety, it's also important to avoid using easily guessable patterns or sequences. Consider using a passphrase – a sentence that's easy for you to remember but hard for others to guess.";
                case "password":
                    return $"Remember, using the same password for multiple accounts is risky. If one account is compromised, all your accounts could be at risk.";
                case "scam":
                    return $"Scammers often use emotional tactics, like creating a sense of urgency or fear, to pressure you into acting without thinking. Always take your time and be skeptical of unexpected requests.";
                case "privacy":
                    return $"Regularly review the privacy settings of your online accounts. You might be sharing more information than you realize.";
                case "email validation":
                    return $"While the format might be valid, always be cautious of the content and sender of any email, especially if it asks for personal information or contains unusual links.";
                case "give me a phishing tip":
                    return $"Phishing often comes through emails, but it can also happen via text messages (SMS phishing or 'smishing') or even phone calls (voice phishing or 'vishing').";
                case "give me a password tip":
                    return $"Consider using a combination of real words that form a sentence or phrase that's easy for you to remember but doesn't look like a common saying.";
                default:
                    return $"I can provide more details on '{topic}'. Could you be more specific about what you'd like to know?";
            }
        }

        // Generates a personalized tip based on the user's favorite topic
        static string GetPersonalizedTip(string topic, string currentUserName)
        {
            switch (topic)
            {
                case "privacy":
                    return "Consider enabling two-factor authentication on all your accounts to add an extra layer of security to your personal information.";
                case "password":
                    return "Think about using a password manager to generate and store strong, unique passwords for all your online services.";
                case "scam":
                    return "Always double-check the URLs of websites you visit, especially when entering sensitive information, to avoid falling victim to fake sites.";
                case "phishing":
                    return "Be extra cautious of any unexpected emails with attachments or links, and never download or click on them unless you are absolutely sure of the sender's authenticity.";
                default:
                    return "Here's a general tip: Stay vigilant and always think before you click!";
            }
        }

        #endregion

        #region Random Response Generators

        // Generates a random phishing tip
        static string GetRandomPhishingTip(string currentUserName)
        {
            string[] phishingTips = new string[]
            {
                $"Be cautious of emails asking for personal information, {currentUserName}. Scammers often disguise themselves as trusted organizations.",
                $"Always verify the sender's email address carefully, {currentUserName}. Look for unusual spellings or domain names.",
                $"Never click on suspicious links in emails, {currentUserName}. Hover your mouse over them to see the actual URL before clicking.",
                $"Be wary of emails that create a sense of urgency or demand immediate action, {currentUserName}. This is a common tactic used by phishers.",
                $"If you're unsure about an email, {currentUserName}, contact the organization directly through a known and trusted method (e.g., their official website or phone number)."
            };
            int index = random.Next(phishingTips.Length);
            return phishingTips[index];
        }

        // Generates a random password tip
        static string GetRandomPasswordTip(string currentUserName)
        {
            string[] passwordTips = new string[]
            {
                $"Make sure your passwords are at least 12 characters long for better security, {currentUserName}.",
                $"Try to include a mix of uppercase and lowercase letters, numbers, and symbols in your passwords, {currentUserName}.",
                $"Avoid using personal information like your name, birthday, or pet's name in your passwords, {currentUserName}.",
                $"It's a good idea to use a different, unique password for each of your online accounts, {currentUserName}.",
                $"Consider using a password manager to securely store and generate strong passwords, {currentUserName}."
            };
            int index = random.Next(passwordTips.Length);
            return passwordTips[index];
        }

        // Generates a random scam awareness tip
        static string GetRandomScamTip(string currentUserName)
        {
            string[] scamTips = new string[]
            {
                $"Be wary of unsolicited messages asking for money or personal information, {currentUserName}. These are often scams.",
                $"Never share your bank details or credit card information with someone you don't know or trust online, {currentUserName}.",
                $"Be cautious of 'too good to be true' offers or prizes, {currentUserName}. These are often part of a scam.",
                $"Verify the legitimacy of websites before entering any personal information, {currentUserName}. Look for the padlock icon in the address bar.",
                $"If you receive a suspicious call or message, {currentUserName}, don't be afraid to hang up or ignore it."
            };
            int index = random.Next(scamTips.Length);
            return scamTips[index];
        }

        // Generates a random privacy tip
        static string GetRandomPrivacyTip(string currentUserName)
        {
            string[] privacyTips = new string[]
            {
                $"Review and adjust the privacy settings on your social media accounts and other online platforms, {currentUserName}.",
                $"Be mindful of the information you share online, {currentUserName}. Once it's out there, it can be hard to control.",
                $"Be cautious about the permissions you grant to apps and websites, {currentUserName}. Only allow access to information they truly need.",
                $"Consider using privacy-focused search engines and browsers to limit tracking, {currentUserName}.",
                $"Be aware of cookies and tracking technologies used by websites, {currentUserName}, and consider using browser extensions to manage them."
            };
            int index = random.Next(privacyTips.Length);
            return privacyTips[index];
        }

        #endregion
    }
}