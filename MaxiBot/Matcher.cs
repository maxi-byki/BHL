using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiBot
{
    public class Matcher
    {
        List<string> add;
        public Matcher() 
        {
            string name = "urle.txt";
            using FileStream stream = new FileStream(name, FileMode.Open);
            add = new List<string>();
            using StreamReader reader = new StreamReader(stream);
            string line;
            while ((line = reader.ReadLine()) != null) 
            {
                add.Add(line);
            }
        }
        public bool IsMatch(string text)
        {
            foreach (var item in add) 
            {
                if (text == item) return true;
            }
            return false;
        }
    }
}
