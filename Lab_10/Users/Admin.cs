public class Admin : AbstractUser
{
    public override string HeadingForPersonalAccount =>
        $"{new string(' ', Menu.HorizontalShift * 2 + 1)}" +
        $"{"ID",-10}{"Логин",-20}{"Пароль",-20}{"Роль",-20}";

    public override string HeadingForInputData =>
        $"{new string(' ', Menu.HorizontalShift)}" +
        $"Роли: 0 - админ., 1 - бухгалтер, 2 - кассир, 3 - менеджер по персоналу, 4 - складской";

    public Admin(int id, string login, string password) : base(id, login, password)
    {
    }

    public override InputData InputDataWindow
    {
        get
        {
            var inputDataWindow = new InputData(new List<IMenuItem>
            {
                new IntField("ID"),
                new StringField("Логин"),
                new StringField("Пароль"),
                new IntField("Роль")
            });

            var button = new Button("Добавить нового сотрудника в базу", inputDataWindow);
            button.OnPressed += OnNewUserAdded;
            button.OnPressed += inputDataWindow.OnInputDataClosed;
            inputDataWindow.MenuItems.Add(button);
            
            return inputDataWindow;
        }
    }

    private static IList<IMenuItem> personalAccountMenuItems = new List<IMenuItem>();
    public override IList<IMenuItem> PersonalAccountMenuItems
    {
        get
        {
            personalAccountMenuItems.Clear();
            foreach (var user in ShopSystem.SystemData.Users)
            {
                personalAccountMenuItems.Add(user);
            }

            var buttonToInputDataWindow = new Button("Добавить нового пользователя", InputDataWindow);
            buttonToInputDataWindow.OnPressed += InputDataWindow.OnInputDataShowed;
            personalAccountMenuItems.Add(buttonToInputDataWindow);

            return personalAccountMenuItems;
        }
    }

    public void OnNewUserAdded(object? sender, MenuItemEventArgs e)
    {
        if (e.window == null || e.window is not InputData)
        {
            throw new ArgumentException();
        }

        var id = Convert.ToInt32(e.window.MenuItems[0].Value);
        var login = e.window.MenuItems[1].Value;
        var password = e.window.MenuItems[2].Value;
        var role = Convert.ToInt32(e.window.MenuItems[3].Value);

        AbstractUser.SetNewUserFromRole(role, id, login, password);
    }

    public override string GetRole() => "Администратор";

    public override int GetRoleNumber() => 0;
}