# Lab4
An application to practice vocabulary. One may create and edit global lists in different languages in order to practice specific vocabulary lists.

There are two versions of this application: a Console application and one that utilizes a GUI (Winforms).  Both versions share a common library (i.e. Class Library)

File format
Each dictionary is saved as a separate file with file extension .Dat.  The first line in the file contains the names of
The languages, separated by semicolon. All .dat files are stored in a folder of the same name as the application under
The user's AppData \ LOCAL. If the folder does not already exist, the application creates a new folder.

Class Library.
The library implements the Word and WordList classes as below:
The Word class is used to store a single word and should has three Properties:
  Public String [] Translations {GET; }
  public int fromlanguage {get; }
  Public Int Tolanguage {GET; }
Translations stores the translations, one for each language. With fromlanguage and
Tolanguage.  It can specify which exercises for what language to translate to,
respectively, by using the WordList.getwordtopractice method ()
The class has a constructor in two versions 1)Public Word (Params String [] Translations): Initializes 'Translations' with data sent in as 'translations'
2) Public Word (Int Fromlanguage, Int Tolanguage, Params String [] Translations): also sets fromlanguage and tolanguage.

WordList.
The Word List class consists of a list of type Word, and provides methods for To load and save the list from .dat; add and remove words; 
It also uses random words for Exercises and other methods described as below:
Properties:
PUBLIC STRING NAME {GET; } The name of the list.
Public String [] Languages {GET; } The names of the languages.

Methods:
Public WordList (String Name, Params String [] Languages) Constructor => Puts Properites name and languages ​​to the values ​​of parameters.
Public Static String [] Getlists () => Returns Array with names of all lists stored (without the file extension).
Public Static WordList Loadlist (String Name) => Loads the dictionary (NAME specified without file extension) and returns like WordList.
Public Void Save () => Saves the list to a file with the same name as the list and file extension .Dat
Public Void Add (Params String [] Translations) => Adds words to the list. Throw ArgumentException if it is the wrong number of translations.
Public Bool Remove (Int Translation, String Word)Translation corresponds to the index in Languages => Search the language and remove the word.
Public Int Count () => Counts and returns the number of words in the list.
Public Void List (Int Black Translation, Action <String []> Show Translation) => Sortby translation = Which language list should be sorted.
Show Translations = Callback Called for each word in the list.
Public Word GetWordtOPractice () => Returns Random Word from the list, with randomly selected Fromlanguage and tolanguage.

Console Application.
One can create lists, add and remove words, practice, M.M by sending different arguments to the program. If one does not send any arguments (or
Incorrect arguments), the program will indicate:
USE ANY OF THE FOLLOWING PARAMETERS:
-lists
-New <List Name> <Language 1> <Language 2> <Langauge n>
-Add <List Name>
-Remove <List Name> <Language> <Word 1> <Word 2> .. <Word n>
-Words <LISTNAME> <sortBylanguage>
-Count <LISTNAME>
-Practice <LISTNAME>

  -lists
Lists the names of all glossaries from the AppData / Local / "folder with .dat files"
-New <List Name> <Language 1> <Language 2> <Langauge n> => Creates (and saves) a new list of specified name and as many languages ​​as
indicated. Goes directly into the loop to add new words (see -Add).
-Add <List Name> => Asks the user for a new word (on the first language of list), then asking in turn and order for translations into all languages ​​in the list. 
 The program continues to ask for new words until the user cancels by entering an empty line.
-Remove <List Name> <Language> <Word 1> <Word 2> .. <Word n> => Deletes specified words from named list and language.
-Words <LISTNAME> <sortBylanguage> => Lists words (all languages) from specified list. If you specify a certain language it will be sorted by that language.  Otherwise, the list is sorted by first language.
-Count <LISTNAME> => Prints how many words there is in named list.
-Practice <LISTNAME> => Asks the user translating a randomly chosen word out of the list from a randomly selected language to another. 
  Prints if it was right or wrong, and continues to ask for words until the user leaves an empty entry. Then the number of practiced words is written out, as well as
  how many words the user had right.
