public class Authorization : IWindow
{
    public string Title => "Добро пожаловать в магазин семёрочка!!!";
    public IList<IMenuItem> MenuItems { get; private set; } = new List<IMenuItem>()
    {
        new StringField("Логин"),
        new PasswordField("Пароль"),
        new Button("Авторизоваться", new PersonalAccount())
    };

    public Menu CurrentMenu { get; private set; } = new Menu();

    public void Close()
    {
        ShopSystem.ActiveUser = null;
        CurrentMenu.MenuActive = false;
    }

    public void Show()
    {
        MenuItems[2].OnPressed += OnAuthorizationButtonPressed;
        CurrentMenu.Show(this, Title);
    }

    public void OnAuthorizationButtonPressed(object? sender, MenuItemEventArgs e)
    {
        var user = ShopSystem.GetUser(MenuItems[0].Value, MenuItems[1].Value);
        if (user != null)
        {
            ShopSystem.ActiveUser = user;
            if (e.window == null)
            {
                throw new ArgumentNullException();
            }
            e.window.Show();
        }
        else
        {
            Menu.ErrorMassage("Пользователь не найден!");
        }
        CurrentMenu.ClearMenuItems(MenuItems);
    }
}