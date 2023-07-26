# FeatureVec
 Converts text files (.txt) to vectors of 1 and 0's. The length of the vectors needs to be specified as a constant in the code (program.cs) and recompiled
 
 A dictioary file as also needed as input, must be named dictionary.fil 
 
 This program reads in all the flattened .txt files in the directory where the program is executed, The input files need to be proccesed by ProcessDocs so they are stripped
 of punctuation and destemmed).
 The program then looks up the each word in the dictionary and creates a binary feature vector with 1's representing the words matched in the dictionary.
 Output are *.vec files and a single matrix "resume.mat" that is composed from all the row vectors with the length being the number of .txt files processed
 
 Requires the dictionary file "dictionary.fil", note this is not the direct output from Process docs. The file is loaded into excel trimmed of short words
 and the column of frequences removed (at this time these are for analysis only).
 ** Bug,program will consume any text file in the directory even if it is not a training example
