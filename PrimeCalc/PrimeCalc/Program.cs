
using System.Collections.Concurrent;

class Program
{
    //the file to write output for
    const string fileName = "primes-output.txt";
    static Int32 counter = 0;

    private static void Main(string[] args)
    {

        Int64 min, max, threadCount;
        bool valide1 = Int64.TryParse(args[0], out min);
        bool valide2 = Int64.TryParse(args[1], out max);
        bool valide3 = Int64.TryParse(args[2], out threadCount);

        if (File.Exists(fileName))
            File.Delete(fileName);

        var file = new FileStream(fileName, FileMode.OpenOrCreate);
        var stdOutput = Console.Out;
        var writer = new StreamWriter(file);
        writer.AutoFlush = true;
        using (writer)
        {
            Console.SetOut(writer);

            //check inputs
            if (args.Length != 3)
            {
                Console.WriteLine("Wrong number of args");
                Environment.Exit(0);
            }

            if (!valide1 || !valide2 || !valide3)
            {
                Console.WriteLine("One or more args not valide");
                Environment.Exit(0);
            }

            if (max < min)
            {
                Console.WriteLine("Out of range! max range has to be equal or greater than min range");
                Environment.Exit(0);
            }

            if (threadCount <= 0)
            {
                Console.WriteLine("threads number can not be 0!");
                Environment.Exit(0);
            }


            //set number of threads
            var threads = new Thread[threadCount];
            var range = (max - min) / threadCount;
            var start = min;

            //give threds 1 - (n-1) thir range job
            for (var i = 0; i < threadCount - 1; i++)
            {
                var LeftStart = start;
                threads[i] = new Thread(() => FindPrimesInRange(LeftStart, range));
                start += range;
                threads[i].Start();
            }
            //give thred n his range job
            threads[threadCount - 1] = new Thread(() => FindPrimesInRange(start, range + (max - min) % threadCount));
            threads[threadCount - 1].Start();

            //join all threads
            for (var i = 0; i < threadCount; i++)
                threads[i].Join();

            //set the reguler Console.WriteLine back
            Console.SetOut(stdOutput);
        }


            //Console.WriteLine(counter);

        }

    //function to find if number is a prime number
    static bool IsPrime(Int64 n)
    {
        if (n == 2 || n == 3)
            return true;

        if (n <= 1 || n % 2 == 0 || n % 3 == 0)
            return false;
        for (int i = 5; i <= Math.Sqrt(n); i += 6)
        {
            if (n % i == 0 || n % (i + 2) == 0)
                return false;
        }
        return true;
    }


    //this function will find and print all the prime numbers in range [start, range]
    private static void FindPrimesInRange(Int64 start, Int64 range)
    {

        var end = start + range;
        for (var i = start; i < end; i++)
        {
            if (IsPrime(i))
            {
                string res = "Thread [" + Thread.CurrentThread.ManagedThreadId + "]: " + i;
                Console.WriteLine(res);
                counter++;
            }
        }
    }

}

