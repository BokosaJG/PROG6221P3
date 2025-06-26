using System;
using System.Collections.Generic;

namespace CyberSecurityAwarenessBot
{
    internal class ChatBotComponent
    {
        // Base class for all chatbot components
        public abstract class ChatbotComponent
        {
            protected Random random = new Random();
            public abstract string Process(string input, Dictionary<string, string> memory);
        }

        public class KeywordRecognizer : ChatbotComponent
        {
            private Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>()
            {
                { "password", new List<string> {
                    "Always create complex passwords using letters, numbers, and symbols.",
                    "Make sure to use strong, unique passwords for each account.",
                    "Consider using a password manager for better security.",
                    "A good password should be at least 12 characters long."
                }},
                { "phishing", new List<string> {
                    "Phishing attacks usually come via email or messages—never click unknown links or give out personal info!",
                    "Be wary of unsolicited communications asking for personal information.",
                    "If an offer seems too good to be true, it probably is a scam."
                }},
                { "browsing", new List<string> {
                    "Stick to secure (https) websites and steer clear of suspicious downloads or popups.",
                    "Always check for the padlock icon in your browser's address bar."
                }},
                { "malware", new List<string> {
                    "Malware is malicious software designed to harm your device or steal your data.",
                    "Keep your antivirus software updated to protect against malware threats.",
                    "Never download attachments from unknown senders to avoid malware infections.",
                    "Regular system scans can help detect and remove malware early."
                }},
                { "vpn", new List<string> {
                    "A VPN encrypts your internet connection to protect your privacy online.",
                    "Always use a VPN when connecting to public WiFi networks.",
                    "VPNs help mask your IP address and location from trackers.",
                    "Choose a reputable VPN provider that doesn't keep activity logs."
                }},
                { "wifi", new List<string> {
                    "Public WiFi networks are often unsecured - avoid sensitive transactions on them.",
                    "Use a VPN when connecting to public WiFi to encrypt your data.",
                    "Disable file sharing when using public WiFi networks.",
                    "Your device might automatically connect to fake hotspots - disable auto-connect."
                }},
                { "mission", new List<string> {
                    "My mission is to share tips and guidance on staying safe in the digital world.",
                    "I'm here to help you navigate cybersecurity challenges."
                }},
                { "hello", new List<string> {
                    "Good day, I'm here to help you stay safe online.",
                    "Hello! Ready to learn about cybersecurity?"
                }},
                { "help", new List<string> {
                    "Feel free to ask about password safety, phishing awareness, or how to browse safely on the internet.",
                    "I can chat with you about: strong passwords, phishing scams, and secure browsing habits."
                }}
            };

            private Dictionary<string, List<string>> followUpResponses = new Dictionary<string, List<string>>()
            {
                { "password", new List<string> {
                    "Would you like more tips about creating strong passwords?",
                    "I can also explain how password managers work if you're interested.",
                    "Remember to change your passwords regularly. Need more advice?"
                }},
                { "phishing", new List<string> {
                    "Would you like to know how to identify phishing emails?",
                    "I can tell you more about common phishing techniques if you want.",
                    "It's also important to know what to do if you fall for a phishing scam. Interested?"
                }},
                { "browsing", new List<string> {
                    "Would you like tips for safer online shopping?",
                    "I can explain more about browser security settings if you'd like.",
                    "Knowing about VPNs can also help with secure browsing. Want to learn more?"
                }},
                { "malware", new List<string> {
                    "Would you like to learn how to spot malware infections on your device?",
                    "I can explain different types of malware and how they work.",
                    "Interested in learning about the best anti-malware tools?",
                    "Want to know how to prevent malware infections in the first place?"
                }},
                { "vpn", new List<string> {
                    "Would you like recommendations for reliable VPN services?",
                    "I can explain how VPN encryption works to protect your data.",
                    "Interested in learning when you should always use a VPN?",
                    "Want to know how to set up a VPN on your devices?"
                }},
                { "wifi", new List<string> {
                    "I can share techniques to stay safe on public networks.",
                    "Interested in learning about WiFi security tools?",
                    "Want to know what activities you should never do on public WiFi?"
                }}
            };

            public override string Process(string input, Dictionary<string, string> memory)
            {
                string lowerInput = input.ToLower();
                string response = null;

                if ((lowerInput.Contains("tip") || lowerInput.Contains("advice")) && memory.ContainsKey("interest"))
                {
                    string interest = memory["interest"];
                    var tips = new Dictionary<string, List<string>>()
                    {
                        { "passwords", new List<string> {
                            "Since you care about passwords, have you tried a password manager?",
                            "For password security, enable two-factor authentication where possible.",
                            "Remember to never reuse passwords across different sites!"
                        }},
                        { "phishing", new List<string> {
                            "For phishing protection, always verify sender email addresses.",
                            "As a phishing-aware user, remember: banks never ask for credentials via email.",
                            "Hover over links before clicking to see the real destination."
                        }},
                        { "browsing", new List<string> {
                            "For safer browsing, consider using a VPN on public networks.",
                            "Always look for HTTPS in the address bar when entering sensitive info.",
                            "Clear your browser cache regularly to protect your browsing history."
                        }},
                        { "malware", new List<string> {
                            "Avoid downloading software from unknown sources to prevent malware infections.",
                            "Keep your operating system and antivirus software up to date.",
                            "Be cautious of email attachments even from known contacts."
                        }},
                        { "vpn", new List<string> {
                            "Choose a VPN that doesn't log your activity and offers strong encryption.",
                            "Make sure your VPN is always on when using public WiFi.",
                            "Some VPNs offer extra security features like malware blocking."
                        }},
                        { "wifi", new List<string> {
                            "Public WiFi is often unsecured—avoid logging into sensitive accounts.",
                            "Turn off auto-connect to networks in your device settings.",
                            "Use a VPN to encrypt your traffic when using public WiFi."
                        }}
                    };

                    if (tips.ContainsKey(interest))
                    {
                        return tips[interest][random.Next(tips[interest].Count)];
                    }
                }

                if (memory.ContainsKey("last topic") &&
                    (lowerInput.Contains("more") || lowerInput.Contains("explain") || lowerInput.Contains("yes")))
                {
                    string lastTopic = memory["last topic"];
                    if (followUpResponses.ContainsKey(lastTopic))
                    {
                        var responses = followUpResponses[lastTopic];
                        response = responses[random.Next(responses.Count)];
                        memory["awaiting_followup"] = "true";
                        return response;
                    }
                }

                foreach (var keyword in keywordResponses.Keys)
                {
                    if (lowerInput.Contains(keyword))
                    {
                        var responses = keywordResponses[keyword];
                        response = responses[random.Next(responses.Count)];
                        memory["last topic"] = keyword;
                        memory["awaiting_followup"] = "true";
                        return response;
                    }
                }

                if (lowerInput.Contains("confused") || lowerInput.Contains("don't understand"))
                {
                    if (memory.ContainsKey("last topic"))
                    {
                        response = $"Let me explain that differently about {memory["last topic"]}. " +
                                   keywordResponses[memory["last topic"]][random.Next(keywordResponses[memory["last topic"]].Count)];
                        return response;
                    }
                    else
                    {
                        return "I'm sorry you're confused. Could you tell me what cybersecurity topic you're interested in?";
                    }
                }

                return response ?? "I'm not sure I understand. Could you ask about passwords, phishing, or safe browsing?";
            }
        }

        public class SentimentAnalyzer : ChatbotComponent
        {
            public override string Process(string input, Dictionary<string, string> memory)
            {
                string lowerInput = input.ToLower();

                if (lowerInput.Contains("worried") || lowerInput.Contains("concerned"))
                    return "worried";
                if (lowerInput.Contains("frustrated") || lowerInput.Contains("angry"))
                    return "frustrated";
                if (lowerInput.Contains("curious") || lowerInput.Contains("wonder"))
                    return "curious";

                return "neutral";
            }
        }

        public class MemoryManager : ChatbotComponent
        {
            public override string Process(string input, Dictionary<string, string> memory)
            {
                string lowerInput = input.ToLower();
                string response = null;

                if (lowerInput.StartsWith("my name is "))
                {
                    string name = lowerInput.StartsWith("my name is ") ? input.Substring(11).Trim() :
                                  lowerInput.StartsWith("i'm ") ? input.Substring(4).Trim() :
                                  input.Substring(5).Trim();

                    memory["name"] = name;
                    return $"Nice to meet you, {name}! I’ll remember your name.";
                }

                if (lowerInput.Contains("i'm interested in"))
                {
                    foreach (var topic in new[] { "wifi", "vpn", "passwords", "phishing", "browsing", "malware" })
                    {
                        if (lowerInput.Contains(topic))
                        {
                            memory["interest"] = topic;
                            response = $"Great! I'll remember that you're interested in {topic}. " +
                                       GetInterestBasedResponse(topic);
                            return response;
                        }
                    }
                }

                if (lowerInput.Contains("remember") || lowerInput.Contains("my name") || lowerInput.Contains("what do you know"))
                {
                    if (memory.ContainsKey("name") && memory.ContainsKey("interest"))
                    {
                        return $"{memory["name"]}, I remember you're interested in {memory["interest"]}. " +
                               GetPersonalizedTip(memory["interest"]);
                    }
                    else if (memory.ContainsKey("name"))
                    {
                        return $"I remember you, {memory["name"]}! But I don't know your interests yet.";
                    }
                    else if (memory.ContainsKey("interest"))
                    {
                        return $"I remember you're interested in {memory["interest"]}." +
                               (memory.ContainsKey("name") ? $" {memory["name"]}!" : "") +
                               $" {GetPersonalizedTip(memory["interest"])}";
                    }
                    return "I don't know much about you yet. Tell me your name or interests!";
                }

                if (memory.ContainsKey("interest") &&
                    (lowerInput.Contains("tip") || lowerInput.Contains("advice")))
                {
                    return GetPersonalizedTip(memory["interest"]);
                }

                return null;
            }

            private string GetInterestBasedResponse(string topic)
            {
                var responses = new Dictionary<string, List<string>>()
                {
                    { "passwords", new List<string> { "Passwords are your first line of defense!" }},
                    { "phishing", new List<string> { "Phishing awareness can save you from many scams!" }},
                    { "browsing", new List<string> { "Would you like tips on safe browsing?" }},
                    { "malware", new List<string> { "Would you like tips on malware?" }},
                    { "vpn", new List<string> { "Good for you VPN Man." }},
                    { "wifi", new List<string> { "Would you like to know some tips on staying safe on wifi?" }}
                };

                if (responses.ContainsKey(topic))
                {
                    return responses[topic][random.Next(responses[topic].Count)];
                }
                return "It's an important topic in cybersecurity!";
            }

            private string GetPersonalizedTip(string topic)
            {
                var tips = new Dictionary<string, List<string>>()
                {
                    { "passwords", new List<string> {
                        "Since you care about passwords, have you tried a password manager?",
                        "For password security, enable two-factor authentication where possible.",
                        "Remember to never reuse passwords across different sites!"
                    }},
                    { "phishing", new List<string> {
                        "For phishing protection, always verify sender email addresses.",
                        "As a phishing-aware user, remember: banks never ask for credentials via email.",
                        "Hover over links before clicking to see the real destination."
                    }},
                    { "browsing", new List<string> {
                        "For safer browsing, consider using a VPN on public networks.",
                        "Always look for HTTPS in the address bar when entering sensitive info.",
                        "Clear your browser cache regularly to protect your browsing history."
                    }},
                    { "malware", new List<string> {
                        "Don't download things from unsafe websites."
                    }},
                    { "vpn", new List<string> {
                        "Would you like recommendations for reliable VPN services?"
                    }},
                    { "wifi", new List<string> {
                        "Never connect to outdoor WiFi unless a VPN is with you."
                    }}
                };

                if (tips.ContainsKey(topic))
                {
                    return tips[topic][random.Next(tips[topic].Count)];
                }
                return "Here's a security tip: always keep your software updated!";
            }
        }
    }
}
