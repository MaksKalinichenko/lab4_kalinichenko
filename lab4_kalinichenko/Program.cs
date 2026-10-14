Console.OutputEncoding = System.Text.Encoding.UTF8;

var catalog = new Dictionary<int, (string Name, decimal Price)>
{
    { 1, ("Молоко", 42.5m) },
    { 2, ("Хліб", 25m) },
    { 3, ("Яйця", 68m) },
    { 4, ("Сир", 180.9m) },
    { 5, ("Кава", 245m) }
};

var cart = new List<int>();

var back = new Stack<List<int>>();
var forward = new Stack<List<int>>();

var orders = new Queue<List<int>>();

void ShowCart()
{
    if (cart.Count == 0)
    {
        Console.WriteLine("Кошик порожній");
        return;
    }

    foreach (int code in cart)
        Console.WriteLine($"{catalog[code].Name} - {catalog[code].Price} грн");
    Console.WriteLine($"Разом: {cart.Sum(c => catalog[c].Price)} грн");
}

while (true)
{
    Console.WriteLine("\n1-Каталог 2-Додати 3-Видалити 4-Кошик 5-Назад 6-Вперед 7-Оформити 8-Обробити 0-Вихід");
    string choice = Console.ReadLine();
    if (choice == "0") break;

    switch (choice)
    {
        case "1": 
            foreach (var p in catalog)
                Console.WriteLine($"{p.Key}. {p.Value.Name} - {p.Value.Price} грн");
            break;


        case "2": // додати товар у кошик
            Console.Write("Код товару: ");
            if (int.TryParse(Console.ReadLine(), out int addCode) && catalog.ContainsKey(addCode))
            {
                back.Push(new List<int>(cart)); 
                forward.Clear();                
                cart.Add(addCode);
                Console.WriteLine("Товар додано");
            }
            else Console.WriteLine("Невірний код");
            break;
        case "4": // показати кошик
            ShowCart();
            break;
        case "3": // видалити товар з кошика
            Console.Write("Код товару: ");
            if (int.TryParse(Console.ReadLine(), out int delCode) && cart.Contains(delCode))
            {
                back.Push(new List<int>(cart));
                forward.Clear();
                cart.Remove(delCode);
                Console.WriteLine("Товар видалено");
            }
            else Console.WriteLine("Такого товару немає в кошику");
            break;
        case "5": // Назад
            if (back.Count > 0)
            {
                forward.Push(new List<int>(cart)); 
                cart = back.Pop();                 
                ShowCart();
            }
            else Console.WriteLine("Немає що скасовувати");
            break;
        case "6": // Вперед
            if (forward.Count > 0)
            {
                back.Push(new List<int>(cart));
                cart = forward.Pop();
                ShowCart();
            }
            else Console.WriteLine("Немає що повторювати");
            break;
        case "7": // оформити замовлення
            if (cart.Count == 0)
            {
                Console.WriteLine("Кошик порожній");
                break;
            }
            back.Push(new List<int>(cart));
            forward.Clear();
            orders.Enqueue(new List<int>(cart));
            cart.Clear();
            Console.WriteLine($"Замовлення прийнято. У черзі: {orders.Count}");
            break;
        case "8": // обробити перше замовлення з черги
            if (orders.Count > 0)
            {
                var order = orders.Dequeue();
                Console.WriteLine($"Оброблено замовлення на суму {order.Sum(c => catalog[c].Price)} грн. Залишилось: {orders.Count}");
            }
            else Console.WriteLine("Черга замовлень порожня");
            break;

        default:
            Console.WriteLine("Невірний пункт меню");
            break;
    }
}
