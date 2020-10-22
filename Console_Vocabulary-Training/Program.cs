using System;
using ClassLibrary;
using System.Linq;

namespace Console_Vocabulary_Training
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].Equals("-runtests"))
                {
                    RunLibraryTests();
                }
            }
        }

        static void RunLibraryTests()
        {
            WordList.GetLists();

            WordList list1 = WordList.LoadList("does_not_exist");
            if (list1 == null)
            {
                Console.WriteLine("Load file doesn't exist test passes...");
            }
            else
            {
                Console.WriteLine("Load file doesn't exist test fails...");
            }

            WordList list2 = WordList.LoadList("example");
            if (list2 != null)
            {
                Console.WriteLine("Load list test passes...");
            }
            else
            {
                Console.WriteLine("Load list test fails...");
            }

            string[] languages1 = { "english", "spanish" };
            WordList list3 = new WordList("two_languages", languages1);
            list3.Save();

            string[] translations1 = { "hi", "hola" };
            list3.Add(translations1);
            list3.Save();

            try
            {
                string[] translations2 = { "bad mal" };
                list3.Add(translations2);
            }
            catch (ArgumentException ae)
            {
                Console.Error.WriteLine("Successfully caught:");
                Console.Error.WriteLine(ae.Message);
            }
            list3.Save();

            try
            {
                string[] translations3 = { "bad", "malo", "dålig" };
                list3.Add(translations3);
            }
            catch (ArgumentException ae)
            {
                Console.Error.WriteLine("Successfully caught:");
                Console.Error.WriteLine(ae.Message);
            }
            list3.Save();

            string[] languages2 = { "english" };
            WordList list4 = new WordList("one_language", languages2);
            list4.Save();

            string[] translations4 = { "hello" };
            list4.Add(translations4);

            string[] translations5 = { "goodbye" };
            list4.Add(translations5);
            list4.Save();

            string[] languages3 = { "english", "spanish", "swedish" };
            WordList list5 = new WordList("three_languages", languages3);
            list5.Save();

            string[] translations6 = { "hello", "hola", "hej" };
            list5.Add(translations6);

            string[] translations7 = { "goodbye", "adios", "hejdå" };
            list5.Add(translations7);

            string[] translations8 = { "bad", "bad", "bad" };
            list5.Add(translations8);
            list5.Save();
            string removeMe1 = "bad";

            if (list5.Remove(2, removeMe1))
            {
                Console.WriteLine("Successfully removed word: " + removeMe1);
            }
            else
            {
                Console.WriteLine("Unable to remove word: " + removeMe1);
            }

            string removeMe2 = "does_not_exist";
            if (list5.Remove(2, removeMe2))
            {
                Console.WriteLine("Successfully removed word: " + removeMe2);
            }
            else
            {
                Console.WriteLine("Unable to remove word: " + removeMe2);
            }
            list5.Save();

            string[] translations9 = { "slow", "lento", "långsam" };
            list5.Add(translations9);

            string[] translations10 = { "fast", "rapido", "snabb" };
            list5.Add(translations10);
            list5.Save();

            Console.WriteLine("List Count == " + list5.Count());

            Action<string[]> showTranslations = PrintSortedList;

            Console.WriteLine("List 5 sorted by -1: ");
            list5.List(-1, showTranslations);

            Console.WriteLine("List 5 sorted by 0: ");
            list5.List(0, showTranslations);

            Console.WriteLine("List 5 sorted by 1: ");
            list5.List(1, showTranslations);

            Console.WriteLine("list 5 sorted by 2: ");
            list5.List(2, showTranslations);

            Console.WriteLine("List 5 sorted by 4: ");
            list5.List(4, showTranslations);

            Word practiceWord;
            practiceWord = list5.GetWordtoPractice();
            Console.WriteLine("FromLanguage: " + practiceWord.Translation[practiceWord.FromLanguage] +
                " (" + practiceWord.FromLanguage + ") | ToLanguage: " +
                practiceWord.Translations[practiceWord.ToLanguage] +
                " (" + practiceWord.ToLanguage + ")");

            practiceWord = list5.GetWordToPractice();
            Console.WriteLine("FromLanguage: " + practiceWord.Translation[practiceWord.FromLanguage] +
                " (" + practiceWord.FromLanguage + ") | ToLanguage: " +
                practiceWord.Translations[practiceWord.ToLanguage] +
                " (" + practiceWord.ToLanguage + ")");

            practiceWord = list5.GetWordToPractice();
            Console.WriteLine("FromLanguage: " + practiceWord.Translation[practiceWord.FromLanguage] +
                " (" + practiceWord.FromLanguage + ") | ToLanguage: " +
                practiceWord.Translations[practiceWord.ToLanguage] +
                " (" + practiceWord.ToLanguage + ")");

            practiceWord = list5.GetWordToPractice();
            Console.WriteLine("FromLanguage: " + practiceWord.Translation[practiceWord.FromLanguage] +
                " (" + practiceWord.FromLanguage + ") | ToLanguage: " +
                practiceWord.Translations[practiceWord.ToLanguage] +
                " (" + practiceWord.ToLanguage + ")");

            practiceWord = list5.GetWordToPractice();
            Console.WriteLine("FromLanguage: " + practiceWord.Translation[practiceWord.FromLanguage] +
                " (" + practiceWord.FromLanguage + ") | ToLanguage: " +
                practiceWord.Translations[practiceWord.ToLanguage] +
                " (" + practiceWord.ToLanguage + ")");

        }

        static void PrintSortedList(string[] words)
        {
            foreach (string word in words)
            {
                Console.Write(word + ';');
            }

            Console.WriteLine("");
        }
    }
}





        
    

