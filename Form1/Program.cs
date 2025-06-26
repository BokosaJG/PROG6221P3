using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Runtime.InteropServices;

namespace CyberSecurityAwarenessBot
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].ToLower() == "console")
            {
                RunConsoleMode();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }

        static void RunConsoleMode()
        {
            Console.WriteLine("Console mode not implemented in this version");
        }
    }

    public partial class Form1 : Form
    {
        // Add these for audio playback
        
        // Colors
        private readonly Color PrimaryColor = Color.FromArgb(0, 123, 255);
        private readonly Color SecondaryColor = Color.FromArgb(40, 44, 52);
        private readonly Color AccentColor = Color.FromArgb(255, 193, 7);
        private readonly Color LightBackground = Color.FromArgb(250, 250, 250);
        private readonly Color DarkText = Color.FromArgb(33, 37, 41);

        // Task management
        private List<CybersecurityTask> tasks = new List<CybersecurityTask>();

        // Quiz questions
        private List<QuizQuestion> quizQuestions = new List<QuizQuestion>();
        private int currentQuestionIndex = 0;
        private int quizScore = 0;

        // Activity log
        private List<string> activityLog = new List<string>();
        private const int MaxLogEntries = 10;

        // NLP keywords
        private readonly string[] taskKeywords = { "task", "remind", "reminder", "add", "create", "set" };
        private readonly string[] quizKeywords = { "quiz", "game", "test", "question", "challenge" };
        private readonly string[] logKeywords = { "log", "activity", "history", "summary", "what have you done" };

        // User context
        private string userName = "";
        private bool askedForName = false;

        // UI Controls
        private Panel mainPanel;
        private RichTextBox chatDisplay;
        private TextBox userInput;
        private Button sendButton;
        private Panel sidePanel;
        private Label sidePanelTitle;
        private ListBox taskListBox;
        private Panel quizPanel;
        private Label quizQuestionLabel;
        private FlowLayoutPanel quizOptionsPanel;
        private Button backToChatButton;
        private PictureBox botAvatar;

        public Form1()
        {
            InitializeComponent();
            InitializeQuizQuestions();
            SetupUI();
            PlayWelcomeSound();
            AskForName();
        }

        private void InitializeComponent()
        {
            // This method is intentionally left empty for designer support
        }


        private void SetupUI()
        {
            // Main form setup
            this.Text = "CyberSecurity Awareness Bot";
            this.Size = new Size(1000, 700);
            this.BackColor = LightBackground;
            this.Font = new Font("Segoe UI", 9);
            this.Icon = ExtractIconFromResources();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Main panel
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = LightBackground,
                Padding = new Padding(15)
            };
            this.Controls.Add(mainPanel);

            // Bot avatar
            botAvatar = new PictureBox
            {
                Image = CreateBotAvatar(),
                Size = new Size(80, 80),
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(20, 20)
            };
            mainPanel.Controls.Add(botAvatar);

            // Chat display
            chatDisplay = new RichTextBox
            {
                Location = new Point(120, 20),
                Size = new Size(600, 500),
                ReadOnly = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10),
                ForeColor = DarkText,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Margin = new Padding(0, 0, 0, 10)
            };
            mainPanel.Controls.Add(chatDisplay);

            // User input
            userInput = new TextBox
            {
                Location = new Point(120, 540),
                Size = new Size(520, 40),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ForeColor = DarkText
            };
            userInput.KeyPress += UserInput_KeyPress;
            mainPanel.Controls.Add(userInput);

            // Send button
            sendButton = new Button
            {
                Location = new Point(650, 540),
                Size = new Size(70, 40),
                Text = "Send",
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            sendButton.FlatAppearance.BorderSize = 0;
            sendButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 105, 217);
            sendButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 84, 174);
            sendButton.Click += SendButton_Click;
            mainPanel.Controls.Add(sendButton);

            // Side panel
            sidePanel = new Panel
            {
                Location = new Point(740, 20),
                Size = new Size(230, 560),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15)
            };
            mainPanel.Controls.Add(sidePanel);

            // Side panel title
            sidePanelTitle = new Label
            {
                Text = "Your Cybersecurity Tools",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(0, 0)
            };
            sidePanel.Controls.Add(sidePanelTitle);

            // Task list
            taskListBox = new ListBox
            {
                Location = new Point(0, 40),
                Size = new Size(200, 200),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.White,
                ForeColor = DarkText
            };
            sidePanel.Controls.Add(taskListBox);

            // Quiz panel (initially hidden)
            quizPanel = new Panel
            {
                Location = new Point(0, 250),
                Size = new Size(200, 250),
                Visible = false
            };
            sidePanel.Controls.Add(quizPanel);

            quizQuestionLabel = new Label
            {
                Location = new Point(0, 0),
                Size = new Size(200, 60),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = SecondaryColor,
                TextAlign = ContentAlignment.TopLeft
            };
            quizPanel.Controls.Add(quizQuestionLabel);

            quizOptionsPanel = new FlowLayoutPanel
            {
                Location = new Point(0, 70),
                Size = new Size(200, 150),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true
            };
            quizPanel.Controls.Add(quizOptionsPanel);

            // Back to chat button
            backToChatButton = new Button
            {
                Text = "Back to Chat",
                Size = new Size(200, 40),
                Location = new Point(0, 510),
                BackColor = SecondaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            backToChatButton.FlatAppearance.BorderSize = 0;
            backToChatButton.Click += (s, e) => { quizPanel.Visible = false; };
            sidePanel.Controls.Add(backToChatButton);
        }

        private Image CreateBotAvatar()
        {
            Bitmap bmp = new Bitmap(80, 80);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.FillEllipse(new SolidBrush(PrimaryColor), 0, 0, 80, 80);
                g.DrawString("🤖", new Font("Segoe UI Emoji", 30), Brushes.White, 15, 15);
            }
            return bmp;
        }

        private Icon ExtractIconFromResources()
        {
            return SystemIcons.Shield;
        }

        private void PlayWelcomeSound()
        {
            try
            {
                using (SoundPlayer player = new SoundPlayer("Recording.wav"))
                {
                    player.Play();
                }
            }
            catch
            {
                // Silent fail if sound can't be played
            }
        }

        private void AskForName()
        {
            AddBotMessage("Before we begin, may I know your name?");
            askedForName = true;
        }

        private void InitializeQuizQuestions()
        {
            quizQuestions.Add(new QuizQuestion(
                "What should you do if you receive an email asking for your password?",
                new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                2,
                "Reporting phishing emails helps prevent scams."
            ));

            quizQuestions.Add(new QuizQuestion(
                "True or False: Using the same password for multiple accounts is a good security practice.",
                new List<string> { "True", "False" },
                1,
                "Using unique passwords for each account is much safer."
            ));

            quizQuestions.Add(new QuizQuestion(
                "Which of these is NOT a characteristic of a strong password?",
                new List<string> { "At least 12 characters long", "Contains personal information", "Includes numbers and symbols", "Is unique for each account" },
                1,
                "Never use personal information in passwords."
            ));

            quizQuestions.Add(new QuizQuestion(
                "What does VPN stand for?",
                new List<string> { "Virtual Private Network", "Verified Public Network", "Virtual Public Node", "Verified Private Node" },
                0,
                "VPN stands for Virtual Private Network."
            ));

            quizQuestions.Add(new QuizQuestion(
                "When using public WiFi, you should:",
                new List<string> { "Access sensitive accounts", "Use a VPN", "Disable your firewall", "Share files freely" },
                1,
                "Always use a VPN on public WiFi."
            ));

            quizQuestions.Add(new QuizQuestion(
                "What is two-factor authentication (2FA)?",
                new List<string> {
                    "Using two different passwords",
                    "Verifying identity with two different methods",
                    "Having two security questions",
                    "Using both uppercase and lowercase letters in a password"
                },
                1,
                "2FA adds an extra layer of security by requiring two forms of verification."
            ));

            quizQuestions.Add(new QuizQuestion(
                "What should you do before clicking on a link in an email?",
                new List<string> {
                    "Click immediately if it looks interesting",
                    "Hover over it to see the actual URL",
                    "Forward it to all your contacts",
                    "Always trust links from known senders"
                },
                1,
                "Always verify links by hovering to see the real destination first."
            ));

            quizQuestions.Add(new QuizQuestion(
                "Which of these is a common sign of a phishing attempt?",
                new List<string> {
                    "Professional logo",
                    "Urgent or threatening language",
                    "Proper grammar and spelling",
                    "Coming from a familiar organization"
                },
                1,
                "Phishing often uses urgency or threats to pressure victims."
            ));

            quizQuestions.Add(new QuizQuestion(
                "What is the purpose of a firewall?",
                new List<string> {
                    "To block all internet traffic",
                    "To monitor and control network traffic",
                    "To speed up your internet connection",
                    "To prevent all computer viruses"
                },
                1,
                "Firewalls act as a barrier between trusted and untrusted networks."
            ));

            quizQuestions.Add(new QuizQuestion(
                "Why should you regularly update your software?",
                new List<string> {
                    "It makes your computer run faster",
                    "Updates often include security patches",
                    "To get new features only",
                    "It's not necessary if your computer works fine"
                },
                1,
                "Software updates frequently include critical security fixes."
            ));
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            ProcessUserInput();
        }

        private void UserInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ProcessUserInput();
                e.Handled = true;
            }
        }

        private void ProcessUserInput()
        {
            string input = userInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            AddUserMessage(input);
            userInput.Clear();

            // Handle name collection if we're asking for it
            if (askedForName)
            {
                userName = input;
                askedForName = false;
                AddBotMessage($"Nice to meet you, {userName}! How can I help you with cybersecurity today?");
                return;
            }

            // Analyze sentiment before processing
            string sentiment = AnalyzeSentiment(input);

            // Check if we're in quiz mode
            if (quizPanel.Visible)
            {
                ProcessQuizAnswer(input);
                return;
            }

            // NLP processing - check for keywords with sentiment awareness
            if (ContainsAny(input, taskKeywords))
            {
                ProcessTaskRequest(input, sentiment);
            }
            else if (ContainsAny(input, quizKeywords))
            {
                StartQuiz();
            }
            else if (ContainsAny(input, logKeywords))
            {
                ShowActivityLog();
            }
            else if (input.ToLower().Contains("my name is"))
            {
                userName = input.Substring(input.ToLower().IndexOf("my name is") + "my name is".Length).Trim();
                AddBotMessage($"Nice to meet you, {userName}! I'll remember that.");
            }
            else if (input.ToLower().Contains("what is my name") || input.ToLower().Contains("who am i"))
            {
                if (!string.IsNullOrEmpty(userName))
                    AddBotMessage($"Your name is {userName}!");
                else
                    AddBotMessage("I don't know your name yet. What should I call you?");
            }
            else
            {
                // Default response with sentiment awareness
                ProcessGeneralQuery(input, sentiment);
            }
        }

        private string AnalyzeSentiment(string input)
        {
            input = input.ToLower();
            
            // Positive sentiment indicators
            if (input.Contains("great") || input.Contains("awesome") || input.Contains("thank") || 
                input.Contains("cool") || input.Contains("love") || input.Contains("happy"))
                return "positive";
            
            // Negative sentiment indicators
            if (input.Contains("hate") || input.Contains("angry") || input.Contains("frustrat") || 
                input.Contains("annoy") || input.Contains("sad") || input.Contains("mad"))
                return "negative";
            
            // Worried/concerned indicators
            if (input.Contains("worri") || input.Contains("concern") || input.Contains("scare") || 
                input.Contains("afraid") || input.Contains("fear"))
                return "worried";
            
            return "neutral";
        }

        private void ProcessGeneralQuery(string input, string sentiment)
        {
            input = input.ToLower();
            string personalizedGreeting = string.IsNullOrEmpty(userName) ? "Hello there" : $"Hello {userName}";

            // Adjust response based on sentiment
            string sentimentPrefix = "";
            switch (sentiment)
            {
                case "positive":
                    sentimentPrefix = "Great to hear you're excited about ";
                    break;
                case "negative":
                    sentimentPrefix = "I understand this can be frustrating. Let me help with ";
                    break;
                case "worried":
                    sentimentPrefix = "I understand your concerns about ";
                    break;
            }

            if (input.Contains("hello") || input.Contains("hi"))
            {
                AddBotMessage($"{personalizedGreeting}! How can I help you with cybersecurity today?");
            }
            else if (input.Contains("password"))
            {
                AddBotMessage($"{sentimentPrefix}password security. 💡 Strong passwords should:\n" +
                    "- Be at least 12 characters long\n" +
                    "- Include uppercase, lowercase, numbers, and symbols\n" +
                    "- Be unique for each account\n" +
                    "- Avoid personal information\n\n" +
                    "Consider using a password manager to keep track of them!");
            }
            else if (input.Contains("phishing"))
            {
                AddBotMessage($"{sentimentPrefix}phishing attacks. 🛡️ These try to trick you into revealing sensitive information. Watch for:\n" +
                    "- Urgent or threatening language\n" +
                    "- Suspicious sender addresses\n" +
                    "- Requests for personal information\n" +
                    "- Misspellings and poor grammar\n\n" +
                    "When in doubt, don't click links - go directly to the official website.");
            }
            else if (input.Contains("wifi") || input.Contains("public wifi"))
            {
                AddBotMessage($"{sentimentPrefix}public WiFi. 📶 Safety Tips:\n" +
                    "- Avoid accessing sensitive accounts\n" +
                    "- Use a VPN if possible\n" +
                    "- Confirm the network name with staff\n" +
                    "- Turn off file sharing\n" +
                    "- Enable firewall protection\n\n" +
                    "Better yet, use your mobile data when handling private information.");
            }
            else if (input.Contains("vpn"))
            {
                AddBotMessage($"{sentimentPrefix}VPNs. 🔒 Virtual Private Networks provide security by:\n" +
                    "- Encrypting your internet connection\n" +
                    "- Hiding your IP address\n" +
                    "- Protecting data on public WiFi\n" +
                    "- Bypassing geographic restrictions\n\n" +
                    "Look for reputable VPN services with strong encryption and no-log policies.");
            }
            else if (input.Contains("malware"))
            {
                AddBotMessage($"{sentimentPrefix}malware. ⚠️ Protection Tips:\n" +
                    "- Keep software and OS updated\n" +
                    "- Use reputable antivirus software\n" +
                    "- Don't open email attachments from unknown senders\n" +
                    "- Be cautious with downloads\n" +
                    "- Regularly back up important data\n\n" +
                    "Remember: Prevention is much easier than removal!");
            }
            else
            {
                string unsureResponse = "I'm not sure I understand. You can ask me about:\n" +
                    "- Passwords\n" +
                    "- Phishing\n" +
                    "- Public WiFi safety\n" +
                    "- VPNs\n" +
                    "- Malware protection\n" +
                    "- Or type 'quiz' for a cybersecurity quiz";
                
                if (sentiment == "negative")
                    unsureResponse = "I'm sorry I didn't understand. " + unsureResponse;
                
                AddBotMessage(unsureResponse);
            }
        }

        private void ProcessTaskRequest(string input, string sentiment)
        {
            string sentimentPrefix = sentiment == "negative" ?
                "I understand managing tasks can be overwhelming. " :
                "";

            if (input.ToLower().Contains("add task") || input.ToLower().Contains("create task"))
            {
                string taskDescription = ExtractTaskDescription(input);
                if (!string.IsNullOrEmpty(taskDescription))
                {
                    DateTime? dueDate = ExtractDate(input);
                    string dueDateString = dueDate?.ToShortDateString() ?? "tomorrow";

                    var task = new CybersecurityTask
                    {
                        Title = "Security Task",
                        Description = taskDescription,
                        DueDate = dueDate ?? DateTime.Now.AddDays(1),
                        IsCompleted = false
                    };

                    tasks.Add(task);
                    UpdateTaskList();

                    string logEntry = $"Task added: '{task.Description}' (Due: {task.DueDate:MM/dd})";
                    AddToActivityLog(logEntry);
                    AddBotMessage($"{sentimentPrefix}✅ Task added: \"{task.Description}\" (Due: {dueDateString}). Would you like me to remind you about this?");
                }
                else
                {
                    AddBotMessage($"{sentimentPrefix}Please provide a task description. Example: \"Add task to enable two-factor authentication in 3 days\"");
                }
            }
            else if (input.ToLower().Contains("show tasks") || input.ToLower().Contains("list tasks"))
            {
                ShowTaskList();
            }
            else
            {
                AddBotMessage($"{sentimentPrefix}I can help you with cybersecurity tasks. Try saying \"Add task to enable two-factor authentication in 3 days\"");
            }
        }

        private string ExtractTaskDescription(string input)
        {
            var phrases = new[] { "add task to", "create task to", "remind me to", "task to" };
            foreach (var phrase in phrases)
            {
                if (input.ToLower().Contains(phrase))
                {
                    int start = input.ToLower().IndexOf(phrase) + phrase.Length;
                    return input.Substring(start).Trim();
                }
            }
            return input;
        }

        private DateTime? ExtractDate(string input)
        {
            input = input.ToLower();

            // Handle relative dates
            if (input.Contains("today")) return DateTime.Today;
            if (input.Contains("tomorrow")) return DateTime.Today.AddDays(1);
            if (input.Contains("next week")) return DateTime.Today.AddDays(7);

            // Handle "in X days" format
            var words = input.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == "in" && i + 1 < words.Length)
                {
                    // Check for "in X days" format
                    if (i + 2 < words.Length && words[i + 2] == "days")
                    {
                        if (int.TryParse(words[i + 1], out int days))
                        {
                            return DateTime.Now.AddDays(days);
                        }
                    }
                    // Also handle just "in X" (assuming days)
                    else if (int.TryParse(words[i + 1], out int days))
                    {
                        return DateTime.Now.AddDays(days);
                    }
                }
            }

            // Default to tomorrow if no date specified
            return DateTime.Now.AddDays(1);
        }

        private void UpdateTaskList()
        {
            taskListBox.Items.Clear();
            foreach (var task in tasks)
            {
                taskListBox.Items.Add($"{task.Description} (Due: {task.DueDate:MM/dd})");
            }
        }

        private void ShowTaskList()
        {
            if (tasks.Count == 0)
            {
                AddBotMessage("You don't have any tasks yet. Try adding one!");
                return;
            }

            AddBotMessage("📋 Here are your current cybersecurity tasks:");
            foreach (var task in tasks)
            {
                AddBotMessage($"- {task.Description} (Due: {task.DueDate.ToShortDateString()})");
            }
        }

        private void StartQuiz()
        {
            quizPanel.Visible = true;
            currentQuestionIndex = 0;
            quizScore = 0;
            ShowNextQuestion();
            AddBotMessage("🔍 Starting cybersecurity quiz! Answer the questions in the side panel.");
            AddToActivityLog("Quiz started");
        }

        private void ShowNextQuestion()
        {
            quizOptionsPanel.Controls.Clear();

            if (currentQuestionIndex < quizQuestions.Count)
            {
                var question = quizQuestions[currentQuestionIndex];
                quizQuestionLabel.Text = question.QuestionText;

                // Create option buttons
                for (int i = 0; i < question.Options.Count; i++)
                {
                    var optionButton = new Button
                    {
                        Text = question.Options[i],
                        Tag = i,
                        Size = new Size(190, 40),
                        Margin = new Padding(0, 5, 0, 5),
                        BackColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9)
                    };
                    optionButton.FlatAppearance.BorderColor = Color.LightGray;
                    optionButton.Click += (s, e) =>
                    {
                        ProcessQuizAnswer((s as Button).Text);
                    };
                    quizOptionsPanel.Controls.Add(optionButton);
                }
            }
            else
            {
                EndQuiz();
            }
        }

        private void ProcessQuizAnswer(string answer)
        {
            var question = quizQuestions[currentQuestionIndex];
            bool isCorrect = question.CheckAnswer(answer);

            if (isCorrect)
            {
                quizScore++;
                AddBotMessage($"✅ Correct! {question.Explanation}");
            }
            else
            {
                AddBotMessage($"❌ Incorrect. {question.Explanation}");
            }

            currentQuestionIndex++;
            ShowNextQuestion();
        }

        private void EndQuiz()
        {
            quizPanel.Visible = false;
            string feedback;

            double scorePercentage = (double)quizScore / quizQuestions.Count;
            if (scorePercentage >= 0.8)
                feedback = "🎉 Excellent! You're a cybersecurity expert!";
            else if (scorePercentage >= 0.5)
                feedback = "👍 Good job! You know quite a bit about cybersecurity.";
            else
                feedback = "📚 Keep learning! Cybersecurity is important for everyone.";

            AddBotMessage($"📊 Quiz complete! Your score: {quizScore}/{quizQuestions.Count}. {feedback}");
            AddToActivityLog($"Quiz completed - Score: {quizScore}/{quizQuestions.Count}");
        }

        private void ShowActivityLog()
        {
            AddBotMessage("📜 Here's your recent activity:");

            int start = Math.Max(0, activityLog.Count - MaxLogEntries);
            for (int i = start; i < activityLog.Count; i++)
            {
                AddBotMessage($"{i + 1}. {activityLog[i]}");
            }
        }

        private void AddToActivityLog(string entry)
        {
            activityLog.Add($"{DateTime.Now:g} - {entry}");
            if (activityLog.Count > MaxLogEntries * 2)
            {
                activityLog.RemoveRange(0, activityLog.Count - MaxLogEntries);
            }
        }

        private void AddUserMessage(string message)
        {
            chatDisplay.SelectionColor = PrimaryColor;
            chatDisplay.SelectionFont = new Font("Segoe UI", 10, FontStyle.Bold);
            chatDisplay.AppendText("You: ");
            chatDisplay.SelectionFont = new Font("Segoe UI", 10);
            chatDisplay.AppendText($"{message}\n");
            chatDisplay.ScrollToCaret();
        }

        private void AddBotMessage(string message)
        {
            chatDisplay.SelectionColor = SecondaryColor;
            chatDisplay.SelectionFont = new Font("Segoe UI", 10, FontStyle.Bold);
            chatDisplay.AppendText("CyberBot: ");
            chatDisplay.SelectionFont = new Font("Segoe UI", 10);
            chatDisplay.AppendText($"{message}\n");
            chatDisplay.ScrollToCaret();
        }

        private bool ContainsAny(string input, string[] keywords)
        {
            return keywords.Any(keyword => input.ToLower().Contains(keyword.ToLower()));
        }
    }

    public class CybersecurityTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class QuizQuestion
    {
        public string QuestionText { get; }
        public List<string> Options { get; }
        public int CorrectOptionIndex { get; }
        public string Explanation { get; }

        public QuizQuestion(string questionText, List<string> options, int correctOptionIndex, string explanation)
        {
            QuestionText = questionText;
            Options = options;
            CorrectOptionIndex = correctOptionIndex;
            Explanation = explanation;
        }

        public bool CheckAnswer(string userAnswer)
        {
            if (int.TryParse(userAnswer, out int answerIndex) && answerIndex - 1 == CorrectOptionIndex)
                return true;

            return string.Equals(userAnswer, Options[CorrectOptionIndex], StringComparison.OrdinalIgnoreCase);
        }
    }
}