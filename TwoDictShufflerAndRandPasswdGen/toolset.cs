using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoDictShufflerAndRandPasswdGen
{
    class toolset
    {

        public string[] readFileNames(string fileName)
        {
            string[] lines = System.IO.File.ReadAllLines(fileName);
            return lines;
        }

        public string[] random_names_from_list(string[] nameList, Random rnd)
        {
            //Random rnd = new Random();
            int OnceOrTwice = rnd.Next(1, 3);
            var namesStrList = new List<string>();

            for (int i = 0; i < OnceOrTwice; i++) {
                namesStrList.Add(nameList[rnd.Next(1, nameList.Length)]);
            }
            var names = namesStrList.ToArray();
            return names;
        }

        public void two_dictionary_passwd_gen_banner()
        {
            var two_dictionary_passwd_gen_banner = @"
    ##############################################################
    #   C# - Dictionary shuffler and Random Password Generetor   #
    ############################################################## 
    #                         CONTACT                            #
    ##############################################################
    #               DEVELOPER : SAMMY 76 LJ                      #
    #          Mail Address : sammy76lj@gmail.com                #
    #     DESC: Loads two dictionaries from file for shuffeling  #
    #     DESC: Shuffles 1 or 2 Slovenian male and female name,  #
    #     DESC: and adds rand. numbers to fill up min 25 chars   #
    #            USAGE: Intended as internal tool, now it's o.s. #
    ##############################################################
	";
            Console.WriteLine(two_dictionary_passwd_gen_banner);
        }

        public void writeDownToFile(List<string> genList, string fileName)
        {
            // Open a file
            Console.WriteLine("Write to file: " + fileName);

            System.IO.File.WriteAllLines(fileName, genList);

            /*var fo = open(fileName, "w+");
            // Write sequence of lines at the end of the file.
            fo.writelines(from item in genList
                          select String.Format("%s\n", item));
            // Close opend file
            fo.close(); */
        }

        public string GetRandomCharacters(string text, int noNumbers, Random rng)
        {
            StringBuilder tempText = new StringBuilder();
            //Random rng = new Random();
            for (int x = 0; x < noNumbers; x++)
            {
                tempText.Append(GetRandomCharacter(text, rng));
            }

            if (CheckForDashesAndUnderscores(tempText.ToString()) == false)
            {
                GetRandomCharacters(text, noNumbers, rng);
            }
            return tempText.ToString();
        }

        public char GetRandomCharacter(string text, Random rng)
        {
            //Random rng = new Random();
            int index = rng.Next(text.Length);
            return text[index];
        }

        public bool CheckForDashesAndUnderscores(string text)
        {
            if (text.Contains("_") == false && text.Contains("-") == false)
            {
                //Console.WriteLine("[FROM toolset!] Word " + text + " is not by the rules! Mandatory char _ or - was not found");
                return false;
            }
            return true;
        }            

        public void Shuffle<T>(IList<T> list, Random rnd)
        {
            int n = list.Count;
            //Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public bool ContainsAlphaNumeric(string strToCheck)
        {
            foreach (char c in strToCheck)
            {
                if (char.IsLetterOrDigit(c))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsAlpha(string strToCheck)
        {
            foreach (char c in strToCheck)
            {
                if (char.IsLetter(c))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
