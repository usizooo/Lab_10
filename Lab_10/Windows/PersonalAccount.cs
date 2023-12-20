public class PersonalAccount : IWindow
{
    public IList<IMenuItem> MenuItems
    {
        get
        {
            if (ShopSystem.ActiveUser == null)
            {
                throw new ArgumentNullException();
            }
            return ShopSystem.ActiveUser.PersonalAccountMenuItems;
        }
    }

    public string Title { get; private set; } = string.Empty;
    public Menu CurrentMenu { get; private set; } = new Menu();

    public void Close() => CurrentMenu.MenuActive = false;

    public void Show()
    {
        if (ShopSystem.ActiveUser == null)
        {
            throw new ArgumentException();
        }

        Title = "Добро пожаловать, " + ShopSystem.ActiveUser.Login + "!"
            + "\t\t Роль: " + ShopSystem.ActiveUser.GetRole();

        CurrentMenu.Heading = ShopSystem.ActiveUser.HeadingForPersonalAccount;
        CurrentMenu.Show(this, Title);
    }
}