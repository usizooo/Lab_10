public class Cashier : AbstractUser
{
    public override string HeadingForPersonalAccount =>
        $"{new string(' ', Menu.HorizontalShift * 2 + 1)}" +
        $"{"ID",-10}{"Название",-20}{"Цена за штуку",-20}{"Кол-во",-20}{"Кол-во на складе", -20}";

    public Cashier(int id, string login, string password) : base(id, login, password)
    {
    }

    private static IList<IMenuItem> personalAccountMenuItems = new List<IMenuItem>();
    public override IList<IMenuItem> PersonalAccountMenuItems
    {
        get
        {
            personalAccountMenuItems.Clear();
            foreach (var product in ShopSystem.SystemData.Stockroom)
            {
                personalAccountMenuItems.Add(new ProductForSale(product));
            }
            
            var button = new Button("Пробить продукты");
            button.OnPressed += OnProductsPurchased;
            personalAccountMenuItems.Add(button);

            return personalAccountMenuItems;
        }
    }

    public void OnProductsPurchased(object? sender, MenuItemEventArgs e)
    {
        int amount = 0;
        foreach (var product in ShopSystem.SystemData.Stockroom)
        {
            amount += product.Selected * product.Price;
            product.Count -= product.Selected;
            product.Selected = 0;
        }
        Menu.SuccessMassage($"{new string(' ', Menu.HorizontalShift)}Итоговая сумма: {amount} рублей");
        var record = new Record(0, "Продажа на кассе", amount, true);
        ShopSystem.SystemData.Accounting.Add(record);
    }

    public override int GetRoleNumber() => 2;

    public override string GetRole() => "Кассир";
}