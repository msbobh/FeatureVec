using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FeatureVec
{
    //
    // Need to specify the length of the feature vector as a constant since this is dependent on the length of
    // the wordlist aka dictionary.
    //
    //  This function reads in all the flattened .txt files that have been preprocessed (stripped of punctuation and destemmed) 
    //  looks up the each word in the dictionary and creates a binary feature vector with 1's representing the words matched in
    //  the dictionary. Output is *.vec files and a single matrix "test.mat" that is composed from all the row vectors.
    //
    //  Requires the dictionary file "dictionary.fil", note this is not the direct output from Process docs. The file is loaded into
    //  excel trimmed of short words and the column of frequences removed (these are for analysis only).
    //

    class Program
    {
       const int vectorlength = 536; // Note this is set to the number of unique words in the dictioanry

       static private int Lookupword(string[] vocab, string target)
        {
            int index = -1; // -1 means word not found
            for (int i = 0; i < vectorlength - 1; i++)
            {
                if (vocab[i] == target) // We found a match so return the index
                {
                    index = i;
                    break;
                }
            }
            return (index);
        }
        static private string[] creatMatrix(string filename)
        {
            string[] temp = new string[vectorlength-1];
            string path = Directory.GetCurrentDirectory();
            string[] vectorArray = Directory.GetFiles(path, "*.vec");
            int i = 0;
            using (StreamWriter outfile = new StreamWriter(filename)) // Using the "Using statement will close in the case of exceptions during write
            {
                foreach (var file in vectorArray)
                {
                    temp[i] = File.ReadAllText(file);
                    outfile.WriteLine(temp[i]);
                    i++;
                    Console.WriteLine(temp[i]);
                }
            }
            return (temp);
        }

        static void Main(string[] args)
        {
            // get list of docs (.txt files)
            Console.WriteLine("Getting list of files for processing");
            //*******************************************************************************************
            // Get files for processing  from the current directory, assumes files are pre processed .txt
            // List of files stored in fileArray. result feature vector stored in the same filename with
            // a .vec suffix
            //*******************************************************************************************
            string path = Directory.GetCurrentDirectory();
            string[] fileArray = Directory.GetFiles(path, "*.txt");
            // list all .txt files found
            Console.WriteLine("We found the following files: ");
            foreach (var file in fileArray)
            {
                Console.WriteLine(file);
            }

            //********************************************************************************************
            // Read in the dictionary and store into an array  
            //********************************************************************************************
            string[] dict = new string[vectorlength - 1];
            if (File.Exists ("dictionary.fil"))
                {
                   dict = File.ReadAllLines("dictionary.fil");
                }
            else
                {                
                    Console.WriteLine("Error trying to open dictionary");
                    System.Environment.Exit(1);
                }
            
            string fname;
            int[] features = new int[vectorlength - 1];  // feature vecter for each file

            foreach (var file in fileArray)
            {
                Array.Clear(features, 0, features.Length);
                string[] currentfile = File.ReadAllLines(file); // Read the current document into an array
                List<int> wordindexes = new List<int>();
                int result = 0;
                int x = 0;
                foreach (var word in currentfile) // I think I have this wrong Dictionary for 16000 should be the top index and loop on the word in the file and set vector
                {
                    // go look up the word and add the words index
                    result = Lookupword(dict, word);
                    if (result != -1)
                        wordindexes.Add(result);                                                                                                                                            
                }   // all done with words

                foreach (var found in wordindexes)
                    {
                        features[found] = 1;
                        x++;
                    }

                fname = file;
                fname = file.Replace(".txt", ".vec");
                Console.WriteLine("In file {0} found {1} words", Path.GetFileName(fname),x);
                
                using (StreamWriter outfile = new StreamWriter(fname)) // Using the "Using statement will close in the case of exceptions during write
                {
                    foreach (var entry in features)
                    {
                        outfile.Write("{0} ",entry);
                    }
                    outfile.WriteLine(); // Write out the feature columns separated by space and then a file EOL
                }

            } // complete each file
            creatMatrix("resume.mat");
        }
                
    }
}