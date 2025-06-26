using System;
using System.Collections.Generic;
using static CyberSecurityAwarenessBot.ChatBotComponent;

namespace CyberSecurityAwarenessBot
{
    namespace CybersecurityChatbot
    {
        public class CybersecurityChatbot
        {
            private List<ChatbotComponent> components = new List<ChatbotComponent>();
            private Dictionary<string, string> memory = new Dictionary<string, string>();

            public CybersecurityChatbot()
            {
                components.Add(new KeywordRecognizer());
                components.Add(new SentimentAnalyzer());
                components.Add(new MemoryManager());
            }

            public string ProcessInput(string input)
            {
                if (string.IsNullOrWhiteSpace(input))
                    return "I didn't receive any input. Please try again.";

                string response = "";
                string sentiment = "neutral";

                foreach (var component in components)
                {
                    string result = component.Process(input, memory);

                    if (component is SentimentAnalyzer)
                    {
                        sentiment = result ?? "neutral";
                    }
                    else if (result != null)
                    {
                        response += result + " ";
                    }
                }
                foreach (var component in components)
                {
                    string result = component.Process(input, memory);
                    if (component is MemoryManager && result != null)
                    {
                        response = result; // Take the first non-null response
                        break;
                    }
                }


                if (string.IsNullOrWhiteSpace(response))
                {
                    return "I'm not sure I understand. Could you ask about cybersecurity topics or Maybe this is what you're looking for: ";
                }

                return AdjustForSentiment(response, sentiment).Trim();
            }

            private string AdjustForSentiment(string response, string sentiment)
            {
                switch (sentiment)
                {
                    case "worried":
                        return $"I understand this can be concerning. {response}";
                    case "frustrated":
                        return $"I'm here to help. {response}";
                    case "curious":
                        return $"Great question! {response}";
                    default:
                        return response;
                }
            }
        }
    }
}
