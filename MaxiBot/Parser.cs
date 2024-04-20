using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SlackNet.Events;
namespace MaxiBot
{
    class Parser
    {
        public Parser()
        {

        }

        
        public bool IsVulgar(string text) 
        {
            string pattern1 = ".*idiot.*";
            string pattern2 = ".*chuj.*";
            string pattern3 = ".*kurw.*";
            string bossPattern = ".*szef.*";
            Regex reg1 = new Regex(pattern1);
            Regex reg2 = new Regex(pattern2);
            Regex reg3 = new Regex(pattern3);
            Regex bossReg = new Regex(bossPattern);
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
        public int IsLink(string input)
        {
            string[] words = input.Split(" ");
            string pattern = @".+\..+";
            Regex reg = new Regex(pattern);
            foreach(var word in words) 
            {
                if (reg.IsMatch(word)) 
                {
                    //check if this word suits some urls in database, if not, let the bot check
                }
            }
            return 0;
            
        }
    }
}
