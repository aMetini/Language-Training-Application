using System;
using ClassLibrary;
using System.Collections.Generic;
using System.Linq;

namespace Console_Vocabulary_Training
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Use any of the following parameters:\n" +
                    "-lists\n" +
                    "-new <list name> <language 1> <language 2> .. <language n>\n" +
                    "-add <list name>\n" +
                    "-remove <list name> <language> <word 1> <word 2> .. <word n>\n" +
                    "-words <list name> <sortByLanguage>\n" +
                    "-count <list name>\n" +
                    "-practice <list name>\n");
            }
            else if (args.Length > 0)
            {
                if (args[0].Equals("-lists"))
                {
                    GetListOfDictionaries();
                }
                else if (args[0].Equals("-new"))
                {
                    if (args.Length < 4)
                    {
                        Console.WriteLine("Input parameter error:\n" +
                            "-new <list name> <language 1> <language 2> .. <language n>\n");
                        return;
                    }

                    CreateNewList(args);
                }
                else if (args[0].Equals("-add"))
                {
                    if (args.Length != 2)
                    {
                        Console.WriteLine("Input parameter error:\n" +
                        "-add <list name>\n");
                        return;
                    }

                    AddNewWordToDictionary(args[1]);
                }
                else if (args[0].Equals("-remove"))
                {
                    if (args.Length <= 3)
                    {
                        Console.WriteLine("Input parameter error:\n" +
                            "-remove <list name> <language> <word 1> <word 2> .. <word n>");
                        return;
                    }

                    RemoveWordsFromDictionary(args[1], args[2], new ArraySegment<string>(args, 3, args.Length - 3).ToArray());
                }
                else if (args[0].Equals("-words"))
                {
                    if (args.Length != 3)
                    {
                        Console.WriteLine("Input parameter error:\n" +
                            "-words <list name> <sort by language>\n");
                        return;
                    }

                    ListSortedWordsInDictionary(args[1], args[2]);
                }
                else if (args[0].Equals("-count"))
                {
                    if (args.Length != 2)
                    {
                        Console.WriteLine("Input parameter error:\n" +
                            "-count <list name>\n");
                        return;
                    }

                    CountWordsInDictionary(args[1]);
                }
                else if (args[0].Equals("-practice"))
                {
                    if (args.Length != 2)
                    {
                        Console.WriteLine("Input parameter error:\n" +
                            "-practice <list name>\n");
                        return;
                    }

                    PracticeWordsInDictionary(args[1]);
                }
                else if (args[0].Equals("-runtests"))
                {
                    RunLibraryTests();
                }
                else
                {
                    Console.Error.WriteLine("Error: unsupported parameter detected");
                    Console.Error.WriteLine("Use any of the following parameters:\n" +
                        "-lists\n" +
                        "-new <list name> <language 1> <language 2> .. <language n>\n" +
                        "-add <list name>\n" +
                        "-remove <list name> <language> <word 1> <word 2> .. <word n>\n" +
                        "-words <list name> <sortByLanguage>\n" +
                        "-count <list name>\n" +
                        "-practice <list name>\n");
                }
            }
        }

        static void GetListOfDictionaries()
        {
            foreach (string file in WordList.GetLists())
            {
                Console.WriteLine(file);
            }
        }

        static void CreateNewList(string [] inputParams)
        {
            List<string> languages = new List<string>();
            for (int i = 2; i < inputParams.Length; ++i)
            {
                languages.Add(inputParams[i]);
            }

            WordList wordList = new WordList(inputParams[1], languages.ToArray());
            

            if (wordList != null)
            {
                wordList.Save();
                AddNewWordToDictionary(inputParams[1]);
            }
            else
            {
                Console.Error.WriteLine("Error: unable to load list \"" + inputParams[1] + "\"");
            }
        }

        static void AddNewWordToDictionary(string listName)
        {
            WordList wordList = WordList.LoadList(listName);
            string inputWord = "";

            if (wordList != null)
            {

                do
                {
                    int index = 0;
                    List<string> words = new List<string>();

                    foreach (string language in wordList.Languages)
                    {
                        if (index == 0)
                        {
                            Console.Write("Add new word to list in " + language + ": ");
                        }
                        else
                        {
                            Console.Write("Add word translation to list in " + language + ": ");
                        }

                        inputWord = Console.ReadLine();

                        if (inputWord.Equals(""))
                        {
                            break;
                        }

                        words.Add(inputWord);
                        index++;
                    }

                    if (index > 0)
                    {
                        try
                        {
                            wordList.Add(words.ToArray());
                            wordList.Save();
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine(e.Message);
                        }
                    }

                } while (!inputWord.Equals(""));
            }
            else
            {
                Console.Error.WriteLine("Error: unable to load list \"" + listName + "\"");
            }
        }

        static void RemoveWordsFromDictionary(string listName, string language, string [] words)
        {
            WordList wordList = WordList.LoadList(listName);
            int languageIndex = 0;
            bool languageFound = false;
            bool wordFound = false;

            if (wordList != null)
            {
                foreach (string lang in wordList.Languages)
                {
                    if (lang.ToLower().Equals(language.ToLower()))
                    {
                        languageFound = true;
                        break;
                    }

                    ++languageIndex;
                }

                if (languageFound == true)
                {
                    foreach (string word in words)
                    {
                        bool result = wordList.Remove(languageIndex, word);
                        if (result == true)
                        {
                            Console.WriteLine("Successfully removed the " + language + " word \"" + word + "\"");
                            wordList.Save();
                            wordFound = true;
                        }
                    }

                    if (wordFound == false)
                    {
                        Console.Error.Write("Error: unable to find/remove the " + language + "word(s): ");
                        for (int i = 0; i < words.Length; ++i)
                        {
                            Console.Error.Write(words[i]);

                            if (i < words.Length - 1)
                            {
                                Console.Error.Write(", ");
                            }
                            else
                            {
                                Console.Error.WriteLine("");
                            }
                        }
                    }

                }
                else
                {
                    Console.Error.WriteLine("Error: the language \"" + language + "\" does not exist in the list " + listName);
                }
            }
            else
            {
                Console.Error.WriteLine("Error: unable to load list \"" + listName + "\"");
            }
        }

        static void ListSortedWordsInDictionary(string listName, string language)
        {
            WordList wordList = WordList.LoadList(listName);
            int languageIndex = 0;
            bool languageFound = false;
            Action<string[]> showTranslations = PrintSortedList;

            if (wordList != null)
            {
                foreach (string lang in wordList.Languages)
                {
                    if (lang.ToLower().Equals(language.ToLower()))
                    {
                        languageFound = true;
                        break;
                    }

                    ++languageIndex;
                }

                if (languageFound == true)
                {
                    wordList.List(languageIndex, showTranslations);
                }
                else
                {
                    Console.Error.WriteLine("Error: the language \"" + language + "\" does not exist in the list " + listName);
                }
            }
            else
            {
                Console.Error.WriteLine("Error: unable to load list \"" + listName + "\"");
            }
        }

        static void CountWordsInDictionary(string listName)
        {
            WordList wordList = WordList.LoadList(listName);

            if (wordList != null)
            {
                int count = wordList.Count();
                Console.WriteLine(listName + "'s word count: " + count);
            }
            else
            {
                Console.Error.WriteLine("Error: unable to load list \"" + listName + "\"");
            }
        }
        
        static void PracticeWordsInDictionary(string listName)
        {
            WordList wordList = WordList.LoadList(listName);
            string inputWord = "";
            int numberOfWordsPracticed = 0;
            int numberOfCorrectWordsPracticed = 0;

            if (wordList != null)
            {
                do
                {
                    Word practiceWord = wordList.GetWordToPractice();
                    string fromLanguage = wordList.Languages[practiceWord.FromLanguage];
                    string fromPracticeWord = practiceWord.Translations[practiceWord.FromLanguage];
                    string toLanguage = wordList.Languages[practiceWord.ToLanguage];
                    string toPracticeWord = practiceWord.Translations[practiceWord.ToLanguage];

                    Console.Write("Translate the " + char.ToUpper(fromLanguage[0]) + fromLanguage.Substring(1).ToLower() +
                        " word \"" + fromPracticeWord + "\" to " + char.ToUpper(toLanguage[0]) + toLanguage.Substring(1).ToLower() + ": ");
                    inputWord = Console.ReadLine();

                    if (inputWord.Equals(""))
                    {
                        break;
                    }

                    ++numberOfWordsPracticed;

                    if (inputWord.ToLower().Equals(toPracticeWord.ToLower()))
                    {
                        ++numberOfCorrectWordsPracticed;
                        Console.WriteLine("Correct!");
                    }
                    else
                    {
                        Console.WriteLine("Incorrect.  The correct translation is: " + toPracticeWord);
                    }
                } while (!inputWord.Equals(""));

                Console.WriteLine("Words practiced: " + numberOfWordsPracticed);
                Console.WriteLine("Words correct: " + numberOfCorrectWordsPracticed);
            }
            else
            {
                Console.Error.WriteLine("Error: unable to load list \"" + listName + "\"");
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

            string[] translations1 = { "hello", "hola" };
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

            string[] translations7 = { "goodbye", "adiós", "hejdå" };
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

            string[] translations10 = { "fast", "rápido", "snabb" };
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
            practiceWord = list5.GetWordToPractice();
            Console.WriteLine("FromLanguage: " + practiceWord.Translations[practiceWord.FromLanguage] +
                " (" + practiceWord.FromLanguage + ")  |  ToLanguage: " +
                practiceWord.Translations[practiceWord.ToLanguage] +
                " (" + practiceWord.ToLanguage + ")");

            practiceWord = list5.GetWordToPractice();
            Console.WriteLine("FromLanguage: " + practiceWord.Translations[practiceWord.FromLanguage] +
                " (" + practiceWord.FromLanguage + ")  |  ToLanguage: " +
                practiceWord.Translations[practiceWord.ToLanguage] +
                " (" + practiceWord.ToLanguage + ")");

            practiceWord = list5.GetWordToPractice();
            Console.WriteLine("FromLanguage: " + practiceWord.Translations[practiceWord.FromLanguage] +
                " (" + practiceWord.FromLanguage + ")  |  ToLanguage: " +
                practiceWord.Translations[practiceWord.ToLanguage] +
                " (" + practiceWord.ToLanguage + ")");

            practiceWord = list5.GetWordToPractice();
            Console.WriteLine("FromLanguage: " + practiceWord.Translations[practiceWord.FromLanguage] +
                " (" + practiceWord.FromLanguage + ")  |  ToLanguage: " +
                practiceWord.Translations[practiceWord.ToLanguage] +
                " (" + practiceWord.ToLanguage + ")");

            practiceWord = list5.GetWordToPractice();
            Console.WriteLine("FromLanguage: " + practiceWord.Translations[practiceWord.FromLanguage] +
                " (" + practiceWord.FromLanguage + ")  |  ToLanguage: " +
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


 





        
    

