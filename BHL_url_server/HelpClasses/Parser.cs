using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using BHL_url_server.DTOs;
using BHL_url_server.HelpClasses;
using Google.Apis.Safebrowsing.v4;
using Google.Apis.Safebrowsing.v4.Data;
using Google.Apis.Services;

namespace MaxiBot
{
    public static class Parser
    {
        //public string message;
        
        // public Parser(string message)
        // {
        //     this.message = message;
        // }
        static bool IsVulgar(string text) 
        {
            string pattern1 = ".*idiot.*";
            string pattern2 = ".*dick.*";
            string pattern3 = ".*stupid.*";
            string bossPattern = ".*boss.*";
            Regex reg1 = new Regex(pattern1);
            Regex reg2 = new Regex(pattern2);
            Regex reg3 = new Regex(pattern3);
            Regex bossReg = new Regex(bossPattern);
            text = text.ToLower();
            string[] words = text.Split(" ");
            bool boss = false;
            bool badWord = false;
            foreach (string word in words)
            {
                if (reg1.IsMatch(word) || reg2.IsMatch(word) || reg3.IsMatch(word)) badWord = true;
                if (bossReg.IsMatch(word)) boss = true;
            }
            return boss && badWord;            
        }
        /* return values:
        0 - there is no dot, so no need to check it
        1 - checked in Database and found
        2 - checked in Database and not found. so the bot needs to check it
        */
        static (int val, string word) IsLink(string input, List<LinkDTO> links)
        {
            string[] words = input.Split(" ");
            string pattern = @".+\..+";
            Regex reg = new Regex(pattern);
            foreach(var word in words) 
            {
                if (reg.IsMatch(word)) 
                {
                    if (links.FindIndex(link => link.DomainAddress == word) != -1) return (1, word);
                    else return (2, word);
                }
            }
            return (0, "");
        }


        public static async Task<string> Check(string message, List<LinkDTO> links)
        {
            string res = "";
            var isLink = IsLink(message, links);
            if (IsVulgar(message))
            {
                if (isLink.val == 0)
                    return "DON'T INSULT YOUR BOSS!";
            }

            if (isLink.val == 1)
            {
                return "WARNING: DON'T CLICK THIS LINK! IT'S PHISHING!";
            }
            else if (isLink.val == 2)
            {
                var service = new SafebrowsingService(new BaseClientService.Initializer
                {
                    ApplicationName = "dotnet-client",
                    /* HERE SHOULD BE KEY */
                });

                var request = service.ThreatMatches.Find(new GoogleSecuritySafebrowsingV4FindThreatMatchesRequest()
                {
                    Client = new GoogleSecuritySafebrowsingV4ClientInfo()
                    {
                        ClientId = "Dotnet-client",
                        ClientVersion = "1.5.2"
                    },
                    ThreatInfo = new GoogleSecuritySafebrowsingV4ThreatInfo()
                    {
                        ThreatTypes = new List<string> { "Malware", "Phishing" },
                        PlatformTypes = new List<string> { "Windows", "Linux" },
                        ThreatEntryTypes = new List<string> { "URL" },
                        ThreatEntries = new List<GoogleSecuritySafebrowsingV4ThreatEntry>
                        {
                            new GoogleSecuritySafebrowsingV4ThreatEntry
                            {
                                Url = "http://" + isLink.word
                            },
                            new GoogleSecuritySafebrowsingV4ThreatEntry
                            {
                                Url = "https://" + isLink.word + "/"
                            },
                            new GoogleSecuritySafebrowsingV4ThreatEntry
                            {
                                Url = isLink.word
                            },
                            new GoogleSecuritySafebrowsingV4ThreatEntry
                            {
                                Url = isLink.word + "/"
                            }
                        }
                    }
                });

                var response = await request.ExecuteAsync();

                if (response.Matches.Count == 0)
                    return "OK";
                return "WARNING: DON'T CLICK THIS LINK! IT'S PROBABLY PHISHING! (GOOGLE SAFE SEARCH)";
            }
            else
            {
                return "OK";
            }
        }
    }
}
