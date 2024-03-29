﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FeatureVec
{
    //
    //
    //  This function reads in all the flattened .txt files that have been preprocessed (stripped of punctuation and destemmed) 
    //  The proceeds to look up the each word in the dictionary and creates a feature vector with freqeuncies for words present in the scanned
    //  document and dictionary and zeros otherwise.
    //
    //  Output is one vector file for each document processed (suffixes are  *.vec) a Matrix of all the processed files is also created
    //  "resume.mat" that is composed from all the row vectors.
    //
    // In order to determine the length of the vectors (equal to the number of words in the dictionary) the program reads the number of
    // lines in directrly from the dictionary and saves that for use in the program.
    //
    //  Note: Requires the dictionary file "dictionary.fil", also note this is not the direct output from Process docs. The file is loaded into
    //  excel trimmed of short words and and saves as a csv.
    // ** Bug,program will consume any text file in the directory even if it is not a training example

    class Program
    {

        static int vectorlength; // also number of words in the dictionary
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
            //
            // bugbug https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getfiles?redirectedfrom=MSDN&view=netframework-4.7.2#System_IO_Directory_GetFiles_System_String_
            // sort order not guaranteed
            //
            string[] fileArray = Directory.GetFiles(path, "*.txt");
            // list all .txt files found
            Array.Sort(fileArray);  // Ensure that files remain in the same order during processing.
            
            Console.WriteLine("We found the following files: ");
            foreach (var file in fileArray)
            {
                Console.WriteLine(file);
            }

            //********************************************************************************************
            // Read in the dictionary and store into an array  
            //********************************************************************************************
            string _dictionaryName = "dictionary.fil";
            if (File.Exists(_dictionaryName))
            {
                Program.vectorlength = File.ReadAllLines("dictionary.fil").Count();
            }
            else
            {
                Console.WriteLine("Error trying to open dictionary");
                System.Environment.Exit(1);
            }

            string[] dict = new string[vectorlength - 1];
            dict = File.ReadAllLines(_dictionaryName);
       
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
                    // go look up the word result is equal to the index of the word in the dictionary
                    // Then add that value to the wordindexes list
                    result = Lookupword(dict, word);
                    if (result != -1)
                        wordindexes.Add(result);                                                                                                                                            
                }   // all done with words

                foreach (var found in wordindexes)
                    {
                        features[found] += 1;   // Creates word frequencies for found words in documents
                        x++;
                    }
                
                fname = file;
                fname = file.Replace(".txt", ".vec");
                Console.WriteLine("In file {0} found {1} words", Path.GetFileName(fname),x); // Note X contains the count of duplicate words.
                
                using (StreamWriter outfile = new StreamWriter(fname)) // Using the "Using statement will close in the case of exceptions during write
                {
                    foreach (var entry in features)
                    {
                        outfile.Write("{0} ", entry); // note only writes out the unique words multiples are not counted
                        
                    }
                    outfile.WriteLine(); // Write out the feature columns separated by space and then a file EOL
                }

            } // complete each file
            creatMatrix("resume.mat", vectorlength);
        }
        
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
        static private string[] creatMatrix(string filename, int _vectorlength)
        {
            string[] temp = new string[_vectorlength - 1];
            string path = Directory.GetCurrentDirectory();
            string[] vectorArray = Directory.GetFiles(path, "*.vec");
            Array.Sort(vectorArray); // Maintain sorted order when creating the matrix
            int i = 0;
            using (StreamWriter outfile = new StreamWriter(filename)) // Using the "Using statement will close in the case of exceptions during write
            {
                foreach (var file in vectorArray)
                {
                    temp[i] = File.ReadAllText(file);
                    outfile.WriteLine(temp[i]); // Write anohter row in the matrix
                    i++;
                    Console.WriteLine("vecfile {0}", file);
                }
            }
            return (temp);
        }
    }
}