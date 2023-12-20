using System.Diagnostics;

public class Stock : AbstractUser
{
    public override string HeadingForPersonalAccount =>
        $"{new string(' ', Menu.HorizontalShift * 2 + 1)}" +
        $"{"ID",-10}{"Название",-20}{"Цена за штуку",-20}{"Кол-во на складе",-20}";

    public Stock(int id, string login, string password) : base(id, login, password)
    {
    }

    public override InputData InputDataWindow
    {
        get
        {
            var inputDataWindow = new InputData(new List<IMenuItem>
            {
                new IntField("ID"),
                new StringField("Название"),
                new IntField("Цена за штуку"),
                new IntField("Кол-во"),
            });
            var button = new Button("Добавить новый продукт на склад", inputDataWindow);
            button.OnPressed += OnNewProductAdded;
            button.OnPressed += inputDataWindow.OnInputDataClosed;
            inputDataWindow.MenuItems.Add(button);
            return inputDataWindow;
        }
    }

    private void OnNewProductAdded(object? sender, MenuItemEventArgs e)
    {
        if (e.window == null || e.window is not InputData)
        {
            throw new ArgumentException();
        }

        var id = Convert.ToInt32(e.window.MenuItems[0].Value);
        var productName = e.window.MenuItems[1].Value;
        var price = Convert.ToInt32(e.window.MenuItems[2].Value);
        var count = Convert.ToInt32(e.window.MenuItems[3].Value);

        var newProduct = new Product(id, productName, price, count);

        // Проверить, что такого продукта ещё не было
        ShopSystem.SystemData.Stockroom.Add(newProduct);
    }

    private static IList<IMenuItem> personalAccountMenuItems = new List<IMenuItem>();
    public override IList<IMenuItem> PersonalAccountMenuItems
    {
        get
        {
            personalAccountMenuItems.Clear();
            foreach (var product in ShopSystem.SystemData.Stockroom)
            {
                personalAccountMenuItems.Add(product);
            }

            var buttonToInputDataWindow = new Button("Добавить новый продукт на склад", InputDataWindow);
            buttonToInputDataWindow.OnPressed += InputDataWindow.OnInputDataShowed;
            personalAccountMenuItems.Add(buttonToInputDataWindow);

            return personalAccountMenuItems;
        }
    }

    public override string GetRole() => $"Складской";

    public override int GetRoleNumber() => 4;
}