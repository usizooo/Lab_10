public class InputData : IWindow
{
    public IList<IMenuItem> MenuItems { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public Menu CurrentMenu { get; private set; } = new Menu();

    public InputData(IList<IMenuItem> menuItems)
    {
        MenuItems = menuItems;
    }

    public void Close() => CurrentMenu.MenuActive = false;

    public void Show()
    {
        if (ShopSystem.ActiveUser == null)
        {
            throw new ArgumentException();
        }

        Title = "Добро пожаловать, " + ShopSystem.ActiveUser.Login + "!"
            + "\t\t Роль: " + ShopSystem.ActiveUser.GetRole();

        CurrentMenu.Heading = ShopSystem.ActiveUser.HeadingForInputData;
        CurrentMenu.Show(this, Title);
    }

    public void OnInputDataClosed(object? sender, MenuItemEventArgs e) => Close();
    public void OnInputDataShowed(object? sender, MenuItemEventArgs e) => Show();
}