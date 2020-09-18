package com.company;

import java.io.*;
import java.text.ParseException;
import java.util.Scanner;

public class Main {

    //text file to be read and write
    private static final String TEST_INPUT_FILE_1 = "data/test6/in1.txt";
    private static final String TEST_INPUT_FILE_2 = "data/test6/in2.txt";
    private static final String TEST_OUTPUT_FILE = "data/test6/out.txt";

    //first input file: name of file and line where scanner is reading from
    private static final String FILE_1 = "in1.txt";
    private static int eLine1 = 1;
    //second input file: name of file and line where scanner is reading from
    private static final String FILE_2 = "in2.txt";
    private static int eLine2 = 1;
    //save name of current file that scanner is reading from
    private static String eFile = "";

    public static void main(String[] args){
        merge(TEST_INPUT_FILE_1, TEST_INPUT_FILE_2, TEST_OUTPUT_FILE);
    }

    //read from two text file and merge the content into a separate file while maintaining the sorted order
    public static void merge(String in1, String in2, String out){
        try
        {
            //open file as stream
            FileReader reader1 = new FileReader(in1);
            Scanner scanner1 = new Scanner(reader1);
            FileReader reader2 = new FileReader(in2);
            Scanner scanner2 = new Scanner(reader2);

            //open the output file for printing
            FileWriter writer = new FileWriter(out);
            PrintWriter printWriter = new PrintWriter(writer);

            //number that determine order of entries
            final int ORDER_DETERMINATOR = 0;

            //check if there is content in both of the file
            if(scanner1.hasNextLine() && scanner2.hasNextLine())
            {
                //first logs that will be compared
                eFile = FILE_1;     //current file that is being read
                Log log1 = new Log(scanner1.nextLine());  //get the first log from the file
                eLine1++;           //current line that is being read

                //second logs that will be compared
                eFile = FILE_2;
                Log log2 = new Log(scanner2.nextLine());
                eLine2++;

                //no more data to read
                boolean finish1 = false;      //from the first file
                boolean finish2 = false;

                //loop goes through all of the entries in both files
                while (scanner1.hasNextLine() || scanner2.hasNextLine())
                {
                    //check if there is an entry from each file to be compared
                    if(scanner1.hasNextLine() && scanner2.hasNextLine())
                    {
                        //determine order of two entries
                        int rhsFirst = 0;
                        rhsFirst = log1.compareTo(log2);

                        //entry from the first file is first or is equal to the entry from the second file
                        if(rhsFirst < ORDER_DETERMINATOR || rhsFirst == ORDER_DETERMINATOR)
                        {
                            eFile = FILE_1;
                            printWriter.println(log1.toString());     //print it
                            log1 = new Log(scanner1.nextLine());      //get next entry
                            eLine1++;
                        }
                        else if(rhsFirst > ORDER_DETERMINATOR)  //entry from the first file comes after the entry from the second file
                        {
                            eFile = FILE_2;
                            printWriter.println(log2.toString());
                            log2 = new Log(scanner2.nextLine());
                            eLine2++;
                        }
                    }
                    else
                    {
                        //first file still has entry but second file doesn't
                        if(scanner1.hasNextLine())
                        {
                            eFile = FILE_1;
                            printWriter.println(log1.toString());
                            log1 = new Log(scanner1.nextLine());
                            eLine1++;
                        }
                        else
                        {
                            //last entry for first file but second file still has some
                            if(finish1 == false)
                            {
                                //compare two entries
                                int rhsFirst = 0;
                                rhsFirst = log1.compareTo(log2);

                                //printing order for two entries
                                if(rhsFirst < ORDER_DETERMINATOR || rhsFirst == ORDER_DETERMINATOR)     //first file entry first
                                {
                                    printWriter.println(log1.toString());
                                    printWriter.println(log2.toString());
                                }
                                else if(rhsFirst > ORDER_DETERMINATOR)     //first file entry after
                                {
                                    eFile = FILE_2;
                                    printWriter.println(log2.toString());
                                    printWriter.println(log1.toString());
                                    log2 = new Log(scanner2.nextLine());  //second file still has entries, get next one
                                    eLine2++;
                                }
                                finish1 = true;  //no more entries for first file
                            }
                        }
                        if(scanner2.hasNextLine())   //second file still has entry but first file doesn't
                        {
                            eFile = FILE_2;
                            printWriter.println(log2.toString());
                            log2 = new Log(scanner2.nextLine());
                        }
                        else
                        {
                            //last entry for second file but first file still has some
                            if(finish2 == false)
                            {
                                //compare two entries
                                int rhsFirst = 0;
                                rhsFirst = log2.compareTo(log1);

                                //second file entry first
                                if(rhsFirst < ORDER_DETERMINATOR || rhsFirst == ORDER_DETERMINATOR)
                                {
                                    printWriter.println(log2.toString());
                                    printWriter.println(log1.toString());
                                }
                                else if(rhsFirst > ORDER_DETERMINATOR)  //second file entry after
                                {
                                    eFile = FILE_1;
                                    printWriter.println(log1.toString());
                                    printWriter.println(log2.toString());
                                    log1 = new Log(scanner1.nextLine());    //first file still has entries, get next one
                                    eLine1++;
                                }
                                finish2 = true;   //no more entries for second file
                            }
                        }
                    }
                }

                //print last entry for first file
                if(finish1 == false)
                {
                    eFile = FILE_1;
                    printWriter.println(log1.toString());
                }

                //print last entry for second file
                if(finish2 == false)
                {
                    eFile = FILE_2;
                    printWriter.println(log2.toString());
                }
            }
            else
            {
                //only first file has entries
                if(scanner1.hasNextLine())
                {
                    Log log= new Log(scanner1.nextLine());
                    //print all entries
                    while(scanner1.hasNextLine())
                    {
                        printWriter.println(log);
                        log = new Log(scanner1.nextLine());
                    }
                    //print last entry
                    printWriter.println(log);
                }
                else if(scanner2.hasNextLine())    //only second file has entries
                {
                    Log log= new Log(scanner2.nextLine());
                    //print all entries
                    while(scanner2.hasNextLine())
                    {
                        printWriter.println(log);
                        log = new Log(scanner2.nextLine());
                    }
                    //print last entry
                    printWriter.println(log);
                }
            }

            //close the files
            scanner1.close();
            scanner2.close();
            printWriter.close();
        }
        catch (Exception e)
        {
            if(eFile == FILE_1)  //error caught when reading through first file
                System.err.println(e.getMessage() + " (File: " + eFile + ", Line: " + eLine1 + ")");
            else    //error caught when reading through second file
                System.err.println(e.getMessage() + " (File: " + eFile + ", Line: " + eLine2 + ")");
        }
    }
}
