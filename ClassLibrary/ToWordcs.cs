using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


namespace ClassLibrary
{
    public class WordList
    {
        public string Name { get; }
        public string[] Languages { get; }

        private static readonly string userLocalFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly char directorySeparator = Path.DirectorySeparatorChar;
        private static readonly string dictionaryPath = userLocalFolder + directorySeparator + "WordList";
        private List<Word> listOfWords;

        public WordList(string name, params string [] languages)
        {
            if (!Directory.Exists(dictionaryPath))
            {
                Directory.CreateDirectory(dictionaryPath);
            }
            Name = name;
            Languages = languages;
            listOfWords = new List<Word>();
        }

        public static string [] GetLists()
        {
            List<string> files = new List<string>();
            foreach (string file in Directory.GetFiles(dictionaryPath + directorySeparator, "*.dat"))
            {
                files.Add(Path.GetFileNameWithoutExtension(file));
            }
            return files.ToArray();
        }

        public static WordList LoadList(string name)
        {
            WordList loadedList = null;

            foreach (string file in Directory.GetFiles(dictionaryPath + directorySeparator, "*.dat"))
            {
                try
                {
                    if (name.ToLower().Equals(Path.GetFileNameWithoutExtension(file).ToLower()))
                    {
                        String line;
                        using StreamReader reader = new StreamReader(file);
                        line = reader.ReadLine();

                    if (line[line.Length - 1] == ';')
                    {
                        line = line.Substring(0, line.Length - 1);
                    }
                    loadedList = new WordList(name, line.Split(';'));

                    while (reader.Peek() > 0)
                    {
                        line = reader.ReadLine();

                        if (line[line.Length - 1] == ';')
                        {
                            line = line.Substring(0, line.Length - 1); 

                        }
                    }

                    Word word = new Word(line.Split(';'));
                    loadedList.listOfWords.Add(word);

                    reader.Close();
                    break;
                }
            }

            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        return loadedList;
    }

    public void Save()
    {

        try
        {
            using StreamWriter file = new StreamWriter(dictionaryPath + directorySeparator + Name + ".dat");
            StringBuilder languagesSB = new StringBuilder("");

            foreach (String language in Languages)
            {
                languagesSB.Append(language + ';');
            }

            file.WriteLine(languagesSB.ToString());

            foreach (Word wordInstance in listOfWords)
            {
                StringBuilder wordSB = new StringBuilder("");
                foreach (string word in wordInstance.Translations)
                {
                    wordSB.Append(word + ';');
                }

                file.WriteLine(wordSB.ToString());
            }

            file.Close();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            Console.Error.WriteLine(e.StackTrace);
        }
    }

    public void Add(params string[] translations)
    {
        int numLanguagesInList = Languages.Length;

        if (translations.Length != numLanguagesInList)
        {
            string message = "The number of translations is incorrect.  The list stores " + numLanguagesInList +
                " languages and " + translations.Length + " translations were provided.";
            throw new ArgumentException(message);
        }
        listOfWords.Add(new Word(translations));
    }

    public bool Remove(int translation, string word)
    {
        bool removedLine = false;

        foreach (Word wordInList in listOfWords)
        {
            if (wordInList.Translations[translation].ToLower().Equals(word.ToLower()))

            {
                listOfWords.Remove(wordInList);
                removedLine = true;
                break;
            }
        }

        return removedLine;
    }

    public int Count()
    {
        int wordCount = 0;

        try
        {
            using StreamReader reader = new StreamReader(dictionaryPath + directorySeparator + Name + ".dat");

            reader.ReadLine();

            while (reader.Peek() > 0)
            {
                reader.ReadLine();
                ++wordCount;
            }

            reader.Close();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            Console.Error.WriteLine(e.StackTrace);
        }

        return wordCount;
    }

    public void List(int sortByTranslation, Action<string[]> showTranslations)
    {
        List<Word> sortedListOfWords = new List<Word>();
        if (sortByTranslation >= 0 && sortByTranslation < Languages.Length)
        {
            List<string> translationsToSortBy = new List<string>();
            foreach (Word word in listOfWords)
            {
                translationsToSortBy.Add(word.Translations[sortByTranslation]);
            }

            translationsToSortBy.Sort();

            foreach (string sortedTranslatedWord in translationsToSortBy)
            {
                foreach (Word word in listOfWords)
                {
                    if (sortedTranslatedWord.Equals(word.Translations[sortByTranslation]))
                    {
                        sortedListOfWords.Add(word);
                        break;
                    }
                }
            }

            foreach (Word word in sortedListOfWords)
            {
                showTranslations(word.Translations);
            }
        }
    }
    public Word GetWordToPractice()
    {
        Random random = new Random();
        int fromLanguage = 0;
        int toLanguage = 0;

        if (Languages.Length > 1)
        {
            fromLanguage = random.Next(0, Languages.Length);

            do
            {
                toLanguage = random.Next(0, Languages.Length);
            } while (toLanguage == fromLanguage);
        }

        return new Word(fromLanguage, toLanguage, listOfWords[random.Next(0, listOfWords.Count)].Translations);
    }
}
}
                