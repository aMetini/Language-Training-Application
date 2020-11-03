using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.AccessControl;

namespace Lab4_Windform
{
    public partial class WordListGUI : Form
    {
        public WordListGUI()
        {
            InitializeComponent();
        }

        private void WordListGUI_Load(object sender, EventArgs e)
        {

        }

        private void buttonRunList_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Running Command: -list");
            Console.WriteLine("------------------------------");
            GetListOfDictionaries();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Command Complete: -list\n");
        }

        private void GetListOfDictionaries()
        {
            foreach (string file in WordList.GetLists())
            {
                Console.WriteLine(file);
            }
        }

        private void buttonRunNew_Click(object sender, EventArgs e)
        {
            List<string> args = new List<string>();
            StringBuilder parameters = new StringBuilder();
            args.Add("-new");
            parameters.Append("-new");
            foreach (string param in textBoxNewParams.Text.Split(' '))
            {
                args.Add(param);
                parameters.Append(" " + param);
            }
            Console.WriteLine("Running command: " + parameters.ToString());
            Console.WriteLine("------------------------------------------------------------");
            if (args.Count < 4)
            {
                Console.WriteLine("Input parameter error:\n" +
                    "-new <list name> <language 1> <language 2> .. <language n>\n");
            }
            else
            {
                CreateNewList(args.ToArray());
            }
            Console.WriteLine("------------------------------");
            Console.WriteLine("Command Complete: -list\n");
        }

        private void CreateNewList(string[] inputParams)
        {
            WordList wordList = new WordList(inputParams[1], new ArraySegment<string>(inputParams, 2, inputParams.Length - 2).ToArray<string>());

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

        private void buttonRunAdd_Click(object sender, EventArgs e)
        {
            List<string> args = new List<string>();
            StringBuilder parameters = new StringBuilder();
            args.Add(labelAddCmd.Text);
            parameters.Append(labelAddCmd.Text);
            foreach (string param in textBoxAddParams.Text.Split(' '))
            {
                args.Add(param);
                parameters.Append(" " + param);
            }
            Console.WriteLine("Running command: " + parameters.ToString());
            Console.WriteLine("---------------------------------------------------------------");
            if (args.Count != 2)
            {
                Console.WriteLine("Input parameter error:\n" +
                    "-add <list name>\n");
            }
            else
            {
                AddNewWordToDictionary(args.ElementAt(1));
            }
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("Command complete: " + parameters.ToString() + "\n");
        }
        private void AddNewWordToDictionary(string listName)
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
                            Console.Write("Add new word to list in" + language + ": ");
                        }
                        else
                        {
                            Console.Write("Add word translation to list in" + language + ": ");
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
                Console.Error.WriteLine("Error: unable to load list\"" + listName + "\"");
            }
        }

        private void buttonRunRemove_Click(object sender, EventArgs e)
        {
            List<string> args = new List<string>();
            StringBuilder parameters = new StringBuilder();
            args.Add(labelRemoveCmd.Text);
            parameters.Append(labelRemoveCmd);
            foreach (string param in textBoxRemoveParams.Text.Split(' '))
            {
                args.Add(param);
                parameters.Append(" " + param);
            }
            Console.WriteLine("Running command: " + parameters.ToString());
            Console.WriteLine("------------------------------------------------------------------");
            if (args.Count <= 3)
            {
                Console.WriteLine("Input parameter error:\n" +
                    "-remove <list name> <language> <word 1> <word 2> ..<word n>\n");
            }
            else
            {
                RemoveWordsFromDictionary(args.ElementAt(1), args.ElementAt(2), new ArraySegment<string>(args.ToArray(), 3, args.Count - 3).ToArray<string>());
            }
            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("Command complete: " + parameters.ToString() + "\n");
        }

        private void RemoveWordsFromDictionary(string listName, string language, string[] words)
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
                            Console.WriteLine("Successfully removed the " + language + " word\"" + word + "\"");
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

        private void buttonRunWords_Click(object sender, EventArgs e)
        {
            List<string> args = new List<string>();
            StringBuilder parameters = new StringBuilder();
            args.Add(labelWordCmd.Text);
            parameters.Append(labelWordCmd.Text);
            foreach (string param in textBoxWordParms.Text.Split(' '))
            {
                args.Add(param);
                parameters.Append(" " + param);
            }
            Console.WriteLine("Running command: " + parameters.ToString());
            Console.WriteLine("------------------------------------------------------------------------");
            if (args.Count != 3)
            {
                Console.WriteLine("Input parameter error:\n" +
                    "-words <list name> <sort by language>\n");
            }
            else
            {
                ListSortedWordsInDictionary(args.ElementAt(1), args.ElementAt(2));
            }
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("Command complete: " + parameters.ToString() + "\n");
        }
        private void ListSortedWordsInDictionary(string listName, string language)
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
                    Console.WriteLine("Error: unable to load list \"" + language + "\" does not exist in the list " + listName);
                }
            }
            else
            {
                Console.WriteLine("Error: unable to load list \"" + listName + "\"");
            }

        }
        private void buttonRunCount_Click(object sender, EventArgs e)
        {
            List<string> args = new List<string>();
            StringBuilder parameters = new StringBuilder();
            args.Add(labelCountCmd.Text);
            parameters.Append(labelCountCmd.Text);
            foreach (string param in textBoxCountParms.Text.Split(' '))
            {
                args.Add(param);
                parameters.Append(" " + param);
            }
            Console.WriteLine("Running command: " + parameters.ToString());
            Console.WriteLine("---------------------------------------------------------------------");
            if (args.Count != 2)
            {
                Console.WriteLine("Input parameter error:\n" +
                    "-count <list name>\n");
            }
            else
            {
                CountWordsInDictionary(args.ElementAt(1));
            }
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("Command complete: " + parameters.ToString() + "\n");
        }

        private void CountWordsInDictionary(string listName)
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

        private void buttonRunPractice_Click(ObjectSecurity sender, EventArgs e)
        {
            List<string> args = new List<string>();
            StringBuilder parameters = new StringBuilder();
            args.Add(labelPracticeCmd.Text);
            parameters.Append(labelPracticeCmd.Text);
            foreach (string param in textBoxPracticeParams.Text.Split(' '))
            {
                args.Add(param);
                parameters.Append(" " + param);
            }
            Console.WriteLine("Running command: " + parameters.ToString());
            Console.WriteLine("-------------------------------------------------------------");
            if (args.Count != 2)
            {
                Console.WriteLine("Input parameters error:\n " +
                    "-practice <list name>\n");
            }
            else
            {
                PracticeWordsInDictionary(args.ElementAt(1));
            }
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine("Command complete: " + parameters.ToString() + "\n");
        }

        private void PracticeWordsInDictionary(string listName)
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
                        " word \"" + practiceWord + "\" to " + char.ToUpper(toLanguage[0]) + toLanguage.Substring(1).ToLower() + ": ");
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
                        Console.WriteLine("Incorrect...the correct translation is: " + toPracticeWord);
                    }
                } while (!inputWord.Equals(""));

                Console.WriteLine("Words practiced: " + numberOfWordsPracticed);
                Console.WriteLine(" Words correct: " + numberOfCorrectWordsPracticed);
            }
            else
            {
                Console.Error.WriteLine("Error: unable to load list \"" + listName + "\"");
            }
        }

        private void PrintSortedList(string[] words)
        {
            foreach (string word in words)
            {
                Console.Write(word + ';');
            }

            Console.WriteLine("");
        }

        private void buttonRunAdd_Click_1(object sender, EventArgs e)
        {

        }

        private void buttonRunRemove_Click1(object sender, EventArgs e)
        {

        }

        private void buttonRunWord_Click(object sender, EventArgs e)
        {

        }



        private void buttonRunPractice_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBoxNewParams_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxAddParams_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxRemoveParams_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxWordParms_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxCountParms_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPracticeParams_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
