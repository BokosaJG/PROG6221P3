# PROG6221P3

CyberSecurity Awareness Bot
Overview
The CyberSecurity Awareness Bot is a Windows application designed to educate users about cybersecurity best practices through an interactive chat interface. The bot provides:

Cybersecurity tips and advice

Interactive quizzes to test knowledge

Task management for security-related reminders

Personalized recommendations based on user interests

Features

Core Functionalities
Interactive Chat Interface: Natural language processing for cybersecurity topics
Quiz System: 10+ cybersecurity questions with explanations
Task Management: Create and track security-related tasks
Activity Logging: Track your interactions with the bot
Sentiment Analysis: Adjusts responses based on user emotion

Educational Content

Password security best practices
Phishing attack awareness
Public WiFi safety
VPN usage
Malware protection
Two-factor authentication

Technical Components

Windows Forms UI with responsive design
Audio feedback (welcome sound)
Memory system to remember user preferences
Keyword recognition for common security topics

System Requirements

OS: Windows 10 or later
.NET: .NET Framework 4.7.2 or later
RAM: 2GB minimum
Disk Space: 10MB

Installation

Download the latest release package
Run CyberSecurityAwarenessBot.exe
Optionally, use command line argument console to run in console mode (not fully implemented)

Usage

Launch the application
The bot will ask for your name (optional)
Type your questions or commands:
"How do I create a strong password?"
"Tell me about phishing"
"Add task to enable 2FA tomorrow"
"Start quiz"
Use the side panel to view tasks and take quizzes

Command Line Options

console: Runs in console mode (limited functionality)

Code Structure

CyberSecurityAwarenessBot/
├── Program.cs            - Main entry point
├── Form1.cs              - Main application form
├── ChatBotComponent/     - NLP processing components
│   ├── KeywordRecognizer.cs
│   ├── SentimentAnalyzer.cs
│   └── MemoryManager.cs
└── CybersecurityChatbot/ - Core chatbot logic
    └── CybersecurityChatbot.cs
    
Dependencies

System.Windows.Forms
System.Drawing
System.Media

Known Limitations

Console mode not fully implemented
Limited to English language
Basic NLP with keyword matching

License
This project is licensed under the MIT License. See LICENSE file for details.

Referencing

SANS Institute 2022, Securing the Human: Security Awareness Report, viewed 25 June 2025, https://www.sans.org/securing-the-human/.
Cybersecurity & Infrastructure Security Agency (CISA) 2023, Tips for Creating a Strong Password, viewed 25 June 2025, https://www.cisa.gov/news-events/news/tips-creating-strong-password.
National Cyber Security Centre (NCSC) 2023, Phishing Attacks: How to Protect Yourself, viewed 25 June 2025, https://www.ncsc.gov.uk/guidance/phishing.
Federal Trade Commission (FTC) 2023, How to Recognize and Avoid Phishing Scams, viewed 25 June 2025, https://www.consumer.ftc.gov/articles/how-recognize-and-avoid-phishing-scams.

Github Link
https://github.com/BokosaJG/PROG6221P3.git





