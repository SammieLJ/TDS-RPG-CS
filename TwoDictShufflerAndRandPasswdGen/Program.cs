using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TwoDictShufflerAndRandPasswdGen
{
    class Program
    {
        static void Main(string[] args)
        {
            //read file, set array and start program
            //init measure time
            //start = datetime.now();
            var stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();


            //create toolset object
            toolset toolset = new toolset();


            //show ico banner
            toolset.two_dictionary_passwd_gen_banner();

            //check if number of parameters are ok
            Console.WriteLine("List of arguments: {0}", args[0]); //String.Join("", list.ToArray())
            Console.WriteLine("No of arguments: " + args.Length);
            
            // if only parameter is missing, then exit program
            if (args.Length == 0)
            {
                System.Console.WriteLine("Parameters are missing! Enter: config.txt file name!");
                Environment.Exit(0); // return 1;
            }


            // Read configuration file called 'config.txt'
            var configList = toolset.readFileNames(args[0]);

            /*
             * config.txt:
             * Password length -                25
             * First dictionary text file -     TrendyMaleNames.txt
             * Second dictionary text file -    TrendyFemaleNames.txt
             * Final file to write passwords -  Generirana_gesla.txt
             * Amount of passwds. to generate - 50
             * 
             */

            // check if third argument has some value, if not default is 25
            var max_password_len = Int32.Parse(configList[0]);

            // value shouldn't be less than 25 chars, because algorithm is not optimized for less
            if (max_password_len < 25)
                max_password_len = 25;

            var amountPwdsToGen = Int32.Parse(configList[4]);
            Console.WriteLine("Maximalna dolžina: " + max_password_len);
            Console.WriteLine("Number of passwords to generate " + amountPwdsToGen);

            var male_names_array = toolset.readFileNames(configList[1]);
            var female_names_array = toolset.readFileNames(configList[2]);


            ShuffleAndGeneratePasses shuffler = new ShuffleAndGeneratePasses(toolset);
            List<string> gendNamesList = new List<string>();
            for (int i = 0; i < amountPwdsToGen; i++) {
                gendNamesList.Add(shuffler.getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len));
            }
            

        // Write down names to file
        toolset.writeDownToFile(gendNamesList, configList[3]);
        //stop stopwatch
        stopwatch.Stop();

        Console.WriteLine("Generated passes - ratio of all / met-criteria: " + ShuffleAndGeneratePasses.howManyMethodCallbacks + "/" + amountPwdsToGen);
        Console.WriteLine("Measured time: " + stopwatch.Elapsed + " sec");
        Console.ReadLine();
        }
    }

    class ShuffleAndGeneratePasses
    {
        private toolset toolset;
        public  ShuffleAndGeneratePasses(toolset toolset)
        {
            this.toolset = toolset;
        }
        
        // set global random
        Random random = new Random();

        // set (global) recursion counter for method getRandomAndShuffledPassword (it will be called more than No of passes to gen.)
        static int genRandPasses = 0;
        public static int howManyMethodCallbacks
        {
            get { return genRandPasses; }

        }

        static void AddOneToGenRandPasses()
        {
            genRandPasses++;
        }

        List<string> random_names_from_list(String[] nameList)
        {
            int OnceOrTwice = random.Next(1, 2);
            List<String> names = new List<String>();
            for (int index=0; index <= OnceOrTwice; index++) {
                names.Add(nameList[random.Next(nameList.Length)]);
            }

            return names;

        }

        internal string getRandomAndShuffledPassword(string[] male_names_array, string[] female_names_array, int max_password_len)
        {
            AddOneToGenRandPasses();
            string[] male_names = toolset.random_names_from_list(male_names_array, random);
            string[] female_names = toolset.random_names_from_list(female_names_array, random);

            // calculate how much numbers we need to add to max length of password
            // String.Join("", list.ToArray())
            int maxLenDiff = (String.Join("", male_names.ToArray()) + String.Join("", female_names.ToArray())).Length;
            int noNumbers = max_password_len - maxLenDiff;// - 2 (dash and underscore)

            // print ("Number of Numbers :" + str(noNumbers))

            // set array of number and special chars
            var numeric_chars = "1234567890_-";
            //var underscore_char = "_";
            //var dash_char = "-";
            var numbersAndSpecialChars = String.Empty;

            if (noNumbers > 0) {
                /*for (int x = 0; x < noNumbers; x++) {
                    //numbersAndSpecialChars += random.choice(chars);
                    numbersAndSpecialChars += toolset.GetRandomCharacters(numeric_chars);
                }*/
                numbersAndSpecialChars = toolset.GetRandomCharacters(numeric_chars, noNumbers, random);
            }

            //var final_password = String.Join("", male_names.ToArray()) + String.Join("", female_names.ToArray()) + String.Join("", numbersAndSpecialChars.ToArray());
            var final_password = new List<string>();

            final_password.AddRange(male_names);
            final_password.AddRange(female_names);
            final_password.Add(numbersAndSpecialChars);
            //final_password.Add(underscore_char);
            //final_password.Add(dash_char);


            // debug purposes
            // Console.WriteLine("Final prepaired words for password: ");
            // Console.WriteLine(final_password);

            toolset.Shuffle(final_password, random);

            // Console.WriteLine(("Finall password shuffled: ")
            // Console.WriteLine((final_password)
            var final_password_str = String.Join("", final_password.ToArray());

            //CheckIfPasswdMeetsRules(ref final_password_str, ref male_names_array, ref female_names_array, ref max_password_len);
            // aditional checkups, check for two rules (starts and ends with char)

            // PRVI ZNAK!
            if (char.IsDigit(final_password_str[0]) == true)
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "First char is " + final_password_str[0]);
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }
            
            if (final_password_str[0] == '_')
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "First char is " + final_password_str[0]);
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            if (final_password_str[0] == '-')
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "First char is " + final_password_str[0]);
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            // ZADNJI ZNAK
            if (char.IsDigit(final_password_str[final_password_str.Length - 1]) == true)
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "Last char is " + final_password_str[final_password_str.Length - 1]);
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            if (final_password_str[final_password_str.Length - 1] == '_')
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "Last char is " + final_password_str[final_password_str.Length - 1]);
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            if (final_password_str[final_password_str.Length - 1] == '-')
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "Last char is " + final_password_str[final_password_str.Length - 1]);
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            // check rule that every password should have _ or -
            if (final_password_str.Contains("_") == false && final_password_str.Contains("-") == false)
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules! Mandatory char _ or - was not found");
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            // check rule if only chars are in the password
            if (toolset.ContainsAlphaNumeric(final_password_str) == false)
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "JUST ALL CHARS, NO NUMBERS OR SPEC. CHARS!");
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            // Console.WriteLine(("Password : " + final_password_str)
            //CheckIfPasswdMeetsRules(final_password_str, male_names_array, female_names_array, max_password_len);
            return final_password_str;
        }

        void CheckIfPasswdMeetsRules(string final_password_str, string[] male_names_array, string[] female_names_array, int max_password_len)
        {
            // PRVI ZNAK!
            if (char.IsDigit(final_password_str[0]) == true)
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "First char is " + final_password_str[0]);
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            if (final_password_str[0] == '_')
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "First char is " + final_password_str[0]);
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            if (final_password_str[0] == '-')
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "First char is " + final_password_str[0]);
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            // ZADNJI ZNAK
            if (char.IsDigit(final_password_str[final_password_str.Length - 1]) == true)
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "Last char is " + final_password_str[final_password_str.Length - 1]);
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            if (final_password_str[final_password_str.Length - 1] == '_')
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "Last char is " + final_password_str[final_password_str.Length - 1]);
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            if (final_password_str[final_password_str.Length - 1] == '-')
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "Last char is " + final_password_str[final_password_str.Length - 1]);
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            // check rule that every password should have _ or -
            if (final_password_str.Contains("_") == false && final_password_str.Contains("-") == false)
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules! Mandatory char _ or - was not found");
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }

            // check rule if only chars are in the password
            if (toolset.ContainsAlphaNumeric(final_password_str) == false)
            {
                Console.WriteLine("Word " + final_password_str + " is not by the rules!" + "JUST ALL CHARS, NO NUMBERS OR SPEC. CHARS!");
                getRandomAndShuffledPassword(male_names_array, female_names_array, max_password_len);
            }
        }
    }
}
