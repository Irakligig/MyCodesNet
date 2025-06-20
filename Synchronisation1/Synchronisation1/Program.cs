namespace Synchronisation1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cache cache = new Cache();

            List<string> keys = new List<string> { "A", "B", "C", "D", "E", "F", "G" };
            Random random = new Random();

            foreach (var key in keys)
            {
                int randomValue = random.Next(1, 101); // Random int from 1 to 100
                cache.AddOrUpdate(key, randomValue);
                Console.WriteLine($"[Init] Added key '{key}' with value {randomValue}");
            }

            List<string> list = new List<string>() {"A" , "B" , "C" };

            for (int i = 0; i < 5; i++)
            {
                int threadId = i;
                Thread readerthread = new Thread( 
                    () =>
                    {
                        Random random = new Random();
                        while (true)
                        {
                            string randomkey = list[random.Next(0, list.Count)];
                            int? value = cache.Get(randomkey);
                            Console.WriteLine($"Thread {threadId} : trying to read key {randomkey}");
                            Console.WriteLine($"Thread {threadId} : read value {value}");
                            Thread.Sleep(200);
                        }
                    }                    
                    );
                Thread.Sleep(50);
                readerthread.Start();
            }
            for (int i = 0; i < 3; i++)
            {
                int threadId = i;
                Thread writerThread = new Thread(() =>
                {
                    // pick random key and value
                    Random random = new Random();
                    string randomKey = list[random.Next(0, list.Count)];
                    int newValue = random.Next(1, 100); // new value to write

                    Console.WriteLine($"[Writer {threadId}] Trying to write key '{randomKey}' with value {newValue}");
                    cache.AddOrUpdate(randomKey, newValue);
                    Console.WriteLine($"[Writer {threadId}] Wrote key '{randomKey}' with value {newValue}");

                    Thread.Sleep(200); 

                });
                writerThread.Start();
            }

            // 🟨 Mixed Threads (GetOrAdd)
            for (int i = 0; i < 2; i++)
            {
                int threadId = i;
                Thread mixedThread = new Thread(() =>
                {
                    Random random = new Random();
                    for (int j = 0; j < 5; j++) // do 5 attempts per thread
                    {
                        string randomKey = list[random.Next(0, list.Count)];
                        int factoryValue = random.Next(1, 100);

                        Console.WriteLine($"[Mixed {threadId}] Trying GetOrAdd for key '{randomKey}'");
                        int result = cache.GetOrAdd(randomKey, () => factoryValue);
                        Console.WriteLine($"[Mixed {threadId}] Result for key '{randomKey}': {result}");

                        Thread.Sleep(200); // Simulate work
                    }
                });
                mixedThread.Start();
            }

            Console.ReadLine();
        }
    }
}
