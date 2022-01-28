using SQLite;

namespace SQLiteTesting
{
    public class SQLiteTesting
    {
        public static void Main()
        {
            var dbPath = "AsyncTest.db";

            // Check if db exists, delete if it does
            if (File.Exists(dbPath)) File.Delete(dbPath);

            // Ensure db file exists and the 'Car' table exists
            using(var con = new SQLiteConnection(dbPath))
            {
                con.CreateTable<Car>();
            }

            var numTasks = 10;
            var taskSize = 1000;

            var dbConnectionFlags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex;

            var startTime = DateTime.Now;
            // AsyncTestSingleConnection(dbPath, dbConnectionFlags, numTasks, taskSize);
            AsyncTestMultipleConnections(dbPath, dbConnectionFlags, numTasks, taskSize);
            var duration = (DateTime.Now - startTime).TotalMilliseconds;

            System.Console.WriteLine($"Duration: {duration} ms");
        }

        private static void AsyncTestSingleConnection(string dbPath, SQLiteOpenFlags flags, int numTasks, int taskSize)
        {
            // Create batches of cars to insert into the table
            var sampleData = GenerateSampleData(numTasks, taskSize);

            // Start tasks in async
            // Use a single db connection for all tasks
            var con = new SQLiteConnection(dbPath, flags);
            con.CreateTable<Car>();
            var tasks = new List<Task>();
            foreach (var sample in sampleData) {
                var task = Task.Factory.StartNew(() => con.InsertAll(sample, runInTransaction: false));
                tasks.Add(task);
            }
            
            // Wait for everything to finish
            Task.WaitAll(tasks.ToArray());

            con.Dispose();
        }

        private static void AsyncTestMultipleConnections(string dbPath, SQLiteOpenFlags flags, int numTasks, int taskSize)
        {
            // Create batches of cars to insert into the table
            var sampleData = GenerateSampleData(numTasks, taskSize);

            // Start tasks in async
            var tasks = new List<Task>();
            foreach (var sample in sampleData) {
                // Create a new db connection for every task
                var con = new SQLiteConnection(dbPath, flags);

                var task = Task.Factory.StartNew(() =>
                {
                    con.InsertAll(sample, runInTransaction: false);
                    con.Dispose();
                });

                tasks.Add(task);
            }
            
            // Wait for everything to finish
            Task.WaitAll(tasks.ToArray());
        }

        private static IEnumerable<IEnumerable<Car>> GenerateSampleData(int numTasks, int taskSize)
        {
            var rand = new Random();
            var sampleData = new List<IEnumerable<Car>>();
            for (var batchNumber = 0; batchNumber < numTasks; batchNumber++)
            {
                sampleData.Add(Car.SampleData(taskSize));
            }

            return sampleData;
        }
    }
}