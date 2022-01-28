using SQLite;

namespace SQLiteTesting {
    public class Car {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; }

        public int Price { get; set; }

        public Car(string name, int price) {
            Name = name;
            Price = price;
        }

        public static IEnumerable<Car> SampleData(int size) {
            var rand = new Random();

            var batch = new List<Car>();
            for (var dataNumber = 0; dataNumber < size; dataNumber++) {
                var name = RandomLib.RandomName(100);
                var price = rand.Next() % 100000;

                batch.Add(new Car(name, price));
            }

            return batch;
        }
    }
}