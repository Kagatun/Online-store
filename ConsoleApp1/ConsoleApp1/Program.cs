public interface IWarehouse
{
    bool Contains(Good good, int count);
    void RemoveGood(Good good, int count);
}

public class Good
{
    private string _name;

    public Good(string name)
    {
        _name = name;
    }

    public string Name => _name;
}

public class Warehouse : IWarehouse
{
    private Dictionary<Good, int> _goods;

    public Warehouse()
    {
        _goods = new Dictionary<Good, int>();
    }

    public void ShowAllGoods()
    {
        if (_goods.Count == 0)
            throw new NullReferenceException("Склад пуст.");

        foreach (var item in _goods)
            Console.WriteLine($"Наименование: {item.Key.Name}; Количество: {item.Value}");
    }

    public bool Contains(Good good, int count)
    {
        if (!_goods.ContainsKey(good))
            return false;
        else if (_goods[good] < count)
            return false;

        return true;
    }

    public void Delive(Good good, int quantity)
    {
        if (_goods.ContainsKey(good))
        {
            _goods[good] += quantity;
        }
        else
        {
            _goods[good] = quantity;
        }
    }

    public void RemoveGood(Good good, int count)
    {
        if (!_goods.ContainsKey(good))
            throw new KeyNotFoundException($"Товар {good.Name} отсутствует на складе.");

        if (_goods[good] < count)
            throw new InvalidOperationException($"Недостаточно товара {good.Name} на складе для удаления. Доступно: {_goods[good]}.");

        _goods[good] -= count;
    }
}

public class Shop
{
    private IWarehouse _warehouse;

    public Shop(IWarehouse warehouse)
    {
        _warehouse = warehouse;
    }

    public Cart CreateCart() =>
         new Cart(_warehouse);
}

public class Cart
{
    private IWarehouse _warehouse;
    private Dictionary<Good, int> _goods;

    public Cart(IWarehouse warehouse)
    {
        if (warehouse == null)
            throw new NullReferenceException();

        _goods = new Dictionary<Good, int>();
        _warehouse = warehouse;
    }

    public void Add(Good good, int count)
    {
        int currentGoodInCart = _goods.TryGetValue(good, out var value) ? value : 0;
        int totalNeeded = currentGoodInCart + count;

        if (!_warehouse.Contains(good, totalNeeded))
            throw new InvalidOperationException();

        _goods[good] = totalNeeded;
    }

    public void BuyProduct()
    {
        foreach (var item in _goods)
            _warehouse.RemoveGood(item.Key, item.Value);

        _goods.Clear();
    }

    public void ShowGoods()
    {
        if (_goods.Count == 0)
            throw new NullReferenceException("Корзина пуста.");

        foreach (var item in _goods)
            Console.WriteLine($"Наименование: {item.Key.Name}; Количество: {item.Value}");
    }

    public OrderLink Order()
    {
        BuyProduct();

        return new OrderLink();
    }

}

public class OrderLink
{
    public string Paylink { get; private set; } = "https//...";
}