using Newtonsoft.Json;

public class Product : IMenuItem
{
    public int ID { get; set; }
    public string ProductName { get; set; }
    public int Price { get; set; }
    public int Count { get; set; }
    [JsonIgnore]
    public int Selected { get; set; } = 0;
    public string Name => string.Empty;

    public string Value => ToString();
    [JsonIgnore]
    public EventHandler<MenuItemEventArgs>? OnPressed { get; set; }

    public Product(int id, string productName, int price, int count)
    {
        ID = id;
        ProductName = productName;
        Price = price;
        Count = count;
        OnPressed += OnProductPressed;
    }

    public void Action(int numberMenuItemInMenu)
    {
        OnPressed?.Invoke(this, new MenuItemEventArgs(numberMenuItemInMenu));
    }

    public virtual void OnProductPressed(object? sender, MenuItemEventArgs e)
    {
        var inputData = new InputData(new List<IMenuItem>
        {
            new IntField("ID", ID.ToString()),
            new StringField("Название", ProductName),
            new IntField("Цена за штуку", Price.ToString()),
            new IntField("Кол-во", Count.ToString()),

        });
        var button = new Button("Изменить данные о продукте", inputData);
        button.OnPressed += OnProductUpdated;
        button.OnPressed += inputData.OnInputDataClosed;
        inputData.MenuItems.Add(button);
        inputData.Show();
    }

    private void OnProductUpdated(object? sender, MenuItemEventArgs e)
    {
        if (e.window == null || e.window is not InputData)
        {
            throw new ArgumentException();
        }

        // Нужно произвести проверку на корректность данных

        this.ID = Convert.ToInt32(e.window.MenuItems[0].Value);
        this.ProductName = e.window.MenuItems[1].Value;
        this.Price = Convert.ToInt32(e.window.MenuItems[2].Value);
        this.Count = Convert.ToInt32(e.window.MenuItems[3].Value);
    }

    public void Clear() { }

    public override string ToString()
    {
        return $"{new string(' ', Menu.HorizontalShift)}{ID,-10}{ProductName,-20}{Price,-20}{Count,-20}";
    }
}