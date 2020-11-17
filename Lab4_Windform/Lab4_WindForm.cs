using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ClassLibrary;

namespace Lab4_Windform
{
    public partial class Lab4_WindForm : Form
    {
        private TextBoxStreamWriter writer;

        public Lab4_WindForm()
        {
            writer = null;
            InitializeComponent();
        }

        private void Lab4_Windform_Load(object sender, EventArgs e)
        {
            writer = new TextBoxStreamWriter(textBoxConsole);
            Console.SetOut(writer);
        }

        private void GetListOfDictionaries()
        {
            foreach (string file in WordList.GetLists())
            {
                Console.WriteLine(file);
            }
        }

        private void CreateNewList(string[] inputParams)
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
                            Console.Write("Add new word to list in " + language + ": ");
                        }
                        else
                        {
                            Console.Write("Add word translation to list in " + language + ": ");
                        }

                        InputBox inputBox = new InputBox();
                        if(inputBox.ShowDialog() == DialogResult.OK)
                        {
                            inputWord = inputBox.GetInputText();
                        }
                        else
                        {
                            inputWord = "";
                        }

                        Console.WriteLine("");

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
                            Console.WriteLine("Successfully removed the " + language + " word \"" + word + "\"");
                            wordList.Save();
                            wordFound = true;
                        }
                    }

                    if (wordFound == false)
                    {
                        Console.Error.Write("Error: unable to find/remove the " + language + " word(s): ");

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
                    InputBox inputBox = new InputBox();
                    if (inputBox.ShowDialog() == DialogResult.OK)
                    {
                        inputWord = inputBox.GetInputText();
                    }
                    else
                    {
                        inputWord = "";
                    }

                    Console.WriteLine("");

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
                Console.WriteLine("  Words correct: " + numberOfCorrectWordsPracticed);

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

        private void buttonRunList_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Running command: -list");
            Console.WriteLine("------------------------------------------------------------------");
            GetListOfDictionaries();
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Command complete: -list\n");
        }

        private void buttonRunNew_Click(object sender, EventArgs e)
        {
            List<string> args = new List<string>();
            StringBuilder parameters = new StringBuilder();
            args.Add("-new");
            parameters.Append("-new");
            foreach(string param in textBoxNewParams.Text.Split(' '))
            {
                args.Add(param);
                parameters.Append(" " + param);
            }
            Console.WriteLine("Running command: " + parameters.ToString());
            Console.WriteLine("------------------------------------------------------------------");
            if (args.Count < 4)
            {
                Console.WriteLine("Input parameter error:\n" +
                    "-new <list name> <language 1> <language 2> .. <language n>\n");
            }
            else
            {
                CreateNewList(args.ToArray());
            }
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Command complete: " + parameters.ToString() + "\n");
        }

        private void buttonRunAdd_Click(object sender, EventArgs e)
        {
            List<string> args = new List<string>();
            StringBuilder parameters = new StringBuilder();
            args.Add(labelAddCmdLabel.Text);
            parameters.Append(labelAddCmdLabel.Text);
            foreach (string param in textBoxAddParams.Text.Split(' '))
            {
                args.Add(param);
                parameters.Append(" " + param);
            }
            Console.WriteLine("Running command: " + parameters.ToString());
            Console.WriteLine("------------------------------------------------------------------");
            if (args.Count != 2)
            {
                Console.WriteLine("Input parameter error:\n" +
                    "-add <list name>\n");
            }
            else
            {
                AddNewWordToDictionary(args.ElementAt(1));
            }
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Command complete: " + parameters.ToString() + "\n");
        }

        private void buttonRunRemove_Click(object sender, EventArgs e)
        {
            List<string> args = new List<string>();
            StringBuilder parameters = new StringBuilder();
            args.Add(labelRmvCmdLabel.Text);
            parameters.Append(labelRmvCmdLabel.Text);
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
                    "-remove <list name> <language> <word 1> <word 2> .. <word n>\n");
            }
            else
            {
                RemoveWordsFromDictionary(args.ElementAt(1), args.ElementAt(2), new ArraySegment<string>(args.ToArray(), 3, args.Count - 3).ToArray<string>());
            }
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Command complete: " + parameters.ToString() + "\n");
        }

        private void buttonRunWords_Click(object sender, EventArgs e)
        {
            List<string> args = new List<string>();
            StringBuilder parameters = new StringBuilder();
            args.Add(labelWordCmdLabel.Text);
            parameters.Append(labelWordCmdLabel.Text);
            foreach (string param in textBoxWordParms.Text.Split(' '))
            {
                args.Add(param);
                parameters.Append(" " + param);
            }
            Console.WriteLine("Running command: " + parameters.ToString());
            Console.WriteLine("------------------------------------------------------------------");
            if (args.Count != 3)
            {
                Console.WriteLine("Input parameter error:\n" +
                    "-words <list name> <sort by language>\n");
            }
            else
            {
                ListSortedWordsInDictionary(args.ElementAt(1), args.ElementAt(2));
            }
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Command complete: " + parameters.ToString() + "\n");
        }

        private void buttonRunCount_Click(object sender, EventArgs e)
        {
            List<string> args = new List<string>();
            StringBuilder parameters = new StringBuilder();
            args.Add(labelCountCmdLabel.Text);
            parameters.Append(labelCountCmdLabel.Text);
            foreach (string param in textBoxCountParms.Text.Split(' '))
            {
                args.Add(param);
                parameters.Append(" " + param);
            }
            Console.WriteLine("Running command: " + parameters.ToString());
            Console.WriteLine("------------------------------------------------------------------");
            if (args.Count != 2)
            {
                Console.WriteLine("Input parameter error:\n" +
                    "-count <list name>\n");
            }
            else
            {
                CountWordsInDictionary(args.ElementAt(1));
            }
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Command complete: " + parameters.ToString() + "\n");
        }

        private void buttonRunPractice_Click(object sender, EventArgs e)
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
            Console.WriteLine("------------------------------------------------------------------");
            if (args.Count != 2)
            {
                Console.WriteLine("Input parameter error:\n" +
                    "-practice <list name>\n");
            }
            else
            {
                PracticeWordsInDictionary(args.ElementAt(1));
            }
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Command complete: " + parameters.ToString() + "\n");
        }
    }
}
