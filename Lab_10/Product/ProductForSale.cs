public class ProductForSale : IMenuItem
{
    public Product Product { get; }

    public int Count { get; set; } = 0;

    public string Name => string.Empty;

    public string Value => ToString();

    public EventHandler<MenuItemEventArgs>? OnPressed { get; set; }

    public ProductForSale(Product product)
    {
        Product = product;
        OnPressed += OnProductForSalePressed;
    }

    public void Action(int numberMenuItemInMenu)
    {
        OnPressed?.Invoke(this, new MenuItemEventArgs(numberMenuItemInMenu));
    }

    public virtual void OnProductForSalePressed(object? sender, MenuItemEventArgs e)
    {
        var inputData = new InputData(new List<IMenuItem>
        {
            new IntField("ID", Product.ID.ToString(), true),
            new StringField("Название", Product.ProductName, true),
            new IntField("Цена за штуку", Product.Price.ToString(), true),
            new IntField("Кол-во", Count.ToString()),

        });
        var button = new Button("Добавить продукт на кассу", inputData);
        button.OnPressed += OnProductForSaleUpdated;
        button.OnPressed += inputData.OnInputDataClosed;
        inputData.MenuItems.Add(button);
        inputData.Show();
    }

    private void OnProductForSaleUpdated(object? sender, MenuItemEventArgs e)
    {
        if (e.window == null || e.window is not InputData)
        {
            throw new ArgumentException();
        }

        // Нужно произвести проверку на корректность данных

        Product.ID = Convert.ToInt32(e.window.MenuItems[0].Value);
        Product.ProductName = e.window.MenuItems[1].Value;
        Product.Price = Convert.ToInt32(e.window.MenuItems[2].Value);
        var selected = Convert.ToInt32(e.window.MenuItems[3].Value);
        if (selected < 0 || selected > Product.Count ) 
        {
            throw new ArgumentException();
        }
        Product.Selected = selected;
    }

    public void Clear() { }

    public override string ToString()
    {
        return $"{new string(' ', Menu.HorizontalShift)}{Product.ID,-10}" +
            $"{Product.ProductName,-20}{Product.Price,-20}{Product.Selected,-20}{Product.Count,-20}";
    }
}