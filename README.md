# FeatureVec

    This function reads in all the flattened .txt files that have been preprocessed (stripped of punctuation and destemmed) 
    The proceeds to look up the each word in the dictionary and creates a feature vector with freqeuncies for words present in the scanned
    document and dictionary and zeros otherwise.
    
    Output is one vector file for each document processed (suffixes are  *.vec) a Matrix of all the processed files is also created
    "resume.mat" that is composed from all the row vectors.
    
    In order to determine the length of the vectors (equal to the number of words in the dictionary) the program reads the number of
    lines in directrly from the dictionary and saves that for use in the program.
    
    Note: Requires the dictionary file "dictionary.fil", also note this is not the direct output from Process docs. The file is loaded into
    excel trimmed of short words and and saves as a csv.
    * Bug,program will consume any text file in the directory even if it is not a training example
