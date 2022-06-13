using System.Collections.Concurrent;

class Program

{
    static bool somethingFound = false;
    static int count = 0;

    private static void Main(string[] args)
    {

        Int32 nThreads, Delta;
        bool valide1 = Int32.TryParse(args[2], out nThreads);
        bool valide2 = Int32.TryParse(args[3], out Delta);
        string StringToSearch = args[1];

        if (args.Length != 4)
        {
            Console.WriteLine("Wrong number of args");
            Environment.Exit(0);
        }

        if (!valide1 || !valide2)
        {
            Console.WriteLine("One or more args not valide");
            Environment.Exit(0);
        }

        // get the txt file into list of strings- each string represent a line.
        // var allLines is a List of string of the file
        string line;
        List<string> allLines = new List<string>();
        using (StreamReader sr = new StreamReader(args[0]))
        {
            while ((line = sr.ReadLine()) != null)
            {
                allLines.Add(line);
            }
        }

        //set number of threads
        var threads = new Thread[nThreads];
        var lineRange = allLines.Count / nThreads;
        var start = 0;

        //give threds [1 - (n-1)] thir range job
        for (var i = 0; i < nThreads - 1; i++)
        {
            var LeftStart = start;
            threads[i] = new Thread(() => GeneralSearchAllLines(allLines, StringToSearch, Delta, LeftStart, LeftStart + lineRange));
            start += lineRange;
            threads[i].Start();
        }
        //give thred n his range job
        int lastRange = (start + lineRange + allLines.Count % nThreads);
        threads[nThreads - 1] = new Thread(() => GeneralSearchAllLines(allLines, StringToSearch, Delta, start, lastRange));
        threads[nThreads - 1].Start();

        //join all threads
        for (var i = 0; i < nThreads; i++)
            threads[i].Join();


        //if no substring found
        if (!somethingFound)
        {
            Console.WriteLine("not found");
        }

        //Console.WriteLine(count);
    }

    //highest level
    //this func gets allLines (List of strings - which hold the txt. each string is a line.
    //this func calls GeneralSearchLine for  searching in one line.
    static void GeneralSearchAllLines(List<string> allLines, string StringToSearch, int Delta, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            GeneralSearchLine(allLines, i, StringToSearch, Delta);
        }
    }


    //this func get current line- string, and search for all the sub strings with delta.
    //this func help us to solve the problem when delta is 0 or the whole string is in One Line
    //the func calls to find first char and then calls findNextChar recursvly.
    static void GeneralSearchLine(List<string> allLines, int workingLineNumber, string StringToSearch, int Delta)
    {
        int nLineToPrint = workingLineNumber;
        int counterFirst = 0;
        while (counterFirst >= 0)
        {
            //if not finds first char - return -1\ else return firstCharIndex
            int firstChar = FindFirstChar(allLines[workingLineNumber], StringToSearch, counterFirst);

            // find first char and the string is in ONE line 
            if (firstChar >= 0)
            {
                int nextSameLine = IsNextCharInSameLine(allLines[workingLineNumber], firstChar, Delta);
                //same line
                if (nextSameLine > 0)
                {
                    int nextIndex = firstChar + 1 + Delta;
                    FindNextCharInOneLine(allLines, workingLineNumber, StringToSearch.Substring(1), nextIndex, firstChar, nLineToPrint, Delta);
                    counterFirst = firstChar + 1;
                }
                //if the next char is "\n"
                else if (nextSameLine == 0)
                {
                    if (StringToSearch.Length - 1 == 0)
                    {
                        printPosition(nLineToPrint, firstChar);
                    }
                    counterFirst = firstChar + 1;
                }
                else if (nextSameLine < 0)
                {
                    DownLineRepeat(allLines, workingLineNumber + 1, StringToSearch.Substring(1), (nextSameLine * -1) - 1, firstChar, nLineToPrint, Delta);
                    counterFirst = firstChar + 1;
                }
            }
            //didn't find first char -> go to next line 
            else { counterFirst = -1; }
        }
    }


    // this func called after we found the first char. the func searchs for the next char. if found -> called it again with next char ->
    // repeat until the StringToSearch.len == 0 -> Print details. if NOT FOUND return -1 -> no prints and going back to the loop of searching first char.
    static void FindNextCharInOneLine(List<string> allLines, int workingLineNumber, string StringToSearch, int indexToCompare, int firstCharToPrint, int nLineToPrint, int Delta)
    {
        //if finised and find the whole string
        if (StringToSearch.Length == 0)
        {
            printPosition(nLineToPrint, firstCharToPrint);
            return;
        }
        else
        {
            //check the index to compare is not out of range
            if (indexToCompare < allLines[workingLineNumber].Length)
            {
                //the next char is what we searched for
                if (StringToSearch[0].Equals(allLines[workingLineNumber][indexToCompare]))
                {
                    //checking
                    int nextSameLine = IsNextCharInSameLine(allLines[workingLineNumber], firstCharToPrint, Delta);
                    //the next is in same line
                    if (nextSameLine > 0)
                    {
                        FindNextCharInOneLine(allLines, workingLineNumber, StringToSearch.Substring(1), indexToCompare + 1 + Delta, firstCharToPrint, nLineToPrint, Delta);
                    }
                    //the next char must be space - because one line down
                    else if (nextSameLine == 0)
                    {
                        ///checkk -1?
                        if (StringToSearch.Length -1 == 0)
                        {
                            printPosition(nLineToPrint, firstCharToPrint);
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    // the next char is in next line
                    else if (nextSameLine < 0)
                    {
                        DownLineRepeat(allLines, workingLineNumber + 1, StringToSearch.Substring(1), (nextSameLine * -1) - 1, firstCharToPrint, nLineToPrint, Delta);
                    }
                }
                //the char is not found
                else
                {
                    return;
                }
            }
            else
            {
                int new_IndexToCompare = ((allLines[workingLineNumber].Length - indexToCompare) * -1) - 1;
                if (new_IndexToCompare == -1)
                {
                    if (StringToSearch.Length == 0)
                    {
                        printPosition(nLineToPrint, firstCharToPrint);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                else if (new_IndexToCompare > -1)
                {
                    DownLineRepeat(allLines, workingLineNumber + 1, StringToSearch, new_IndexToCompare, firstCharToPrint, nLineToPrint, Delta);
                }
            }
            return;
        }
    }

    static int IsNextCharInSameLine(string line, int indexToCompare, int Delta)
    {
        // if diffrent > 0 -> its in the same line
        // if diffrent = 0 -> the string end in space of the new line
        // if diffrent < 0 -> the new char is in other line.
        int diffrent = line.Length - (indexToCompare + 1 + Delta);
        return diffrent;
        /*  if (indexToCompare + 1 + Delta < line.Length) { return true; }
          else {return false; }*/
    }


    //this func have line, string to search and starting point- need to finds the first char of the stringToSearch - if find - return the index// else(NOT FOUND- returns -1)
    static int FindFirstChar(string currentLine, string StringToSearch, int whereToBegin)
    {
        int i = whereToBegin;
        while (i < currentLine.Length)
        {
            if (StringToSearch[0].Equals(currentLine[i]))
            {
                return i;
            }
            i++;
        }
        return -1;
    }

    static void DownLineRepeat(List<string> allLines, int workingLineNumber, string StringToSearch, int indexToCompare, int firstCharToPrint, int nLineToPrint, int Delta)
    {
        //if finised and find the whole string
        if (StringToSearch.Length == 0)
        {
            printPosition(nLineToPrint, firstCharToPrint);
            return;
        }
        // if true -> it means end of the file so exit.
        else if (allLines.Count <= workingLineNumber)
        {
            return;
        }
        //detla > 0  -> should search only in the same line
        else if (Delta == 0)
        {
            return;
        }
        else
        {
            // check if line.length is bigger than index-
            // if bigger -> than we'll search in that line. else we need to call this function to search one line down.
            if (allLines[workingLineNumber].Length > indexToCompare)
            {
                // if true its the char. else its not the string.
                if (allLines[workingLineNumber][indexToCompare].Equals(StringToSearch[0]))
                {
                    // check if its in the same line or diffrent
                    int nextSameLine = IsNextCharInSameLine(allLines[workingLineNumber], indexToCompare, Delta);
                    //same line
                    if (nextSameLine > 0)
                    {
                        FindNextCharInOneLine(allLines, workingLineNumber, StringToSearch.Substring(1), indexToCompare + 1 + Delta, firstCharToPrint, nLineToPrint, Delta);
                    }
                    //the next char should be space!
                    else if (nextSameLine == 0)
                    {
                        if (StringToSearch.Length - 1 == 0)
                        {
                            printPosition(nLineToPrint, firstCharToPrint);
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (nextSameLine < 0)
                    {
                        DownLineRepeat(allLines, workingLineNumber + 1, StringToSearch.Substring(1), (nextSameLine * -1) - 1, firstCharToPrint, nLineToPrint, Delta);

                    }
                }
                //the char is NOT what we searched
                else
                {
                    return;
                }
            }
            else if (allLines[workingLineNumber].Length == indexToCompare)
            {
                ///checkk -1?
                if (StringToSearch.Length == 0)
                {

                    printPosition(nLineToPrint, firstCharToPrint);
                    return;
                }
                else
                {
                    return;
                }
            }
            //we need to call this function to search one line down.
            else
            {
                int new_indexToCompare = indexToCompare - allLines[workingLineNumber].Length - 1;
                DownLineRepeat(allLines, workingLineNumber + 1, StringToSearch, new_indexToCompare, firstCharToPrint, nLineToPrint, Delta);

            }
            return;
        }
    }

    //function to print the position where the substring started - [line number, indexinLine]
    static void printPosition(int nLineToPrint, int firstCharToPrint)
    {
        string position = "[" + nLineToPrint + "," + firstCharToPrint + "]";
        Console.WriteLine(position);
        count++;
        somethingFound = true;
    }
}