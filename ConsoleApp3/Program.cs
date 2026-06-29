namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Ввод информации о товаре ===");
            Console.WriteLine();

            try
            {
                Console.Write("Введите наименование товара: ");
                string name = Console.ReadLine();

                Console.Write("Введите производителя: ");
                string manufacturer = Console.ReadLine();

                Console.Write("Введите цену товара (руб.): ");
                decimal price = decimal.Parse(Console.ReadLine());

                Console.Write("Введите срок годности (в днях): ");
                int shelfLife = int.Parse(Console.ReadLine());

                Console.Write("Введите дату производства (в формате дд.мм.гггг): ");
                DateTime productionDate = DateTime.Parse(Console.ReadLine());

                Console.WriteLine();

                Product product = new Product(name, manufacturer, price, shelfLife, productionDate);

                Console.WriteLine("=== Информация о товаре ===");
                Console.WriteLine(product);

                Console.WriteLine();
                Console.WriteLine("=== Дополнительная информация ===");
                Console.WriteLine($"Дата истечения срока годности: {product.ExpirationDate:dd.MM.yyyy}");
                Console.WriteLine($"Товар просрочен: {(product.IsExpired() ? "ДА" : "НЕТ")}");

                if (!product.IsExpired())
                {
                    Console.WriteLine($"Дней до истечения срока годности: {product.DaysUntilExpiration()}");
                }
                else
                {
                    Console.WriteLine($"Просрочен на {Math.Abs(product.DaysUntilExpiration())} дней");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Введите корректные данные (числа и даты в правильном формате)!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// Класс, описывающий товар (продукт).
    /// </summary>
    public class Product
    {
        // Приватные поля
        private string _name;
        private string _manufacturer;
        private decimal _price;
        private int _shelfLife;
        private DateTime _productionDate;

        /// <summary>
        /// Конструктор класса Product.
        /// </summary>
        /// <param name="name">Наименование товара.</param>
        /// <param name="manufacturer">Производитель.</param>
        /// <param name="price">Цена товара.</param>
        /// <param name="shelfLife">Срок годности в днях.</param>
        /// <param name="productionDate">Дата производства.</param>
        public Product(string name, string manufacturer, decimal price, int shelfLife, DateTime productionDate)
        {
            Name = name;
            Manufacturer = manufacturer;
            Price = price;
            ShelfLife = shelfLife;
            ProductionDate = productionDate;
        }

        /// <summary>
        /// Наименование товара.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Наименование товара не может быть пустым!", nameof(value));
                _name = value;
            }
        }

        /// <summary>
        /// Производитель товара.
        /// </summary>
        public string Manufacturer
        {
            get => _manufacturer;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Производитель не может быть пустым!", nameof(value));
                _manufacturer = value;
            }
        }

        /// <summary>
        /// Цена товара.
        /// </summary>
        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Цена не может быть отрицательной!", nameof(value));
                _price = value;
            }
        }

        /// <summary>
        /// Срок годности в днях.
        /// </summary>
        public int ShelfLife
        {
            get => _shelfLife;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Срок годности должен быть положительным числом!", nameof(value));
                _shelfLife = value;
            }
        }

        /// <summary>
        /// Дата производства.
        /// </summary>
        public DateTime ProductionDate
        {
            get => _productionDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Дата производства не может быть в будущем!", nameof(value));
                _productionDate = value;
            }
        }

        /// <summary>
        /// Дата истечения срока годности (вычисляемое свойство).
        /// </summary>
        public DateTime ExpirationDate => ProductionDate.AddDays(ShelfLife);

        /// <summary>
        /// Проверяет, просрочен ли товар.
        /// </summary>
        /// <returns>True, если товар просрочен; иначе False.</returns>
        public bool IsExpired()
        {
            return DateTime.Now > ExpirationDate;
        }

        /// <summary>
        /// Возвращает количество дней до истечения срока годности.
        /// </summary>
        /// <returns>Количество дней до истечения срока годности (может быть отрицательным, если товар просрочен).</returns>
        public int DaysUntilExpiration()
        {
            TimeSpan difference = ExpirationDate - DateTime.Now;
            return difference.Days;
        }

        /// <summary>
        /// Переопределенный метод ToString для вывода информации о товаре.
        /// </summary>
        /// <returns>Строка с информацией о товаре.</returns>
        public override string ToString()
        {
            return $"Наименование: {Name}\n" +
                   $"Производитель: {Manufacturer}\n" +
                   $"Цена: {Price:F2} руб.\n" +
                   $"Срок годности: {ShelfLife} дн.\n" +
                   $"Дата производства: {ProductionDate:dd.MM.yyyy}";
        }
    }
}