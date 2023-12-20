using Newtonsoft.Json;

public abstract class AbstractUser : IMenuItem
{
    [JsonIgnore]
    public EventHandler<MenuItemEventArgs>? OnPressed { get; set; }
    public int ID { get; private set; }
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    [JsonIgnore]
    public string Name => string.Empty;
    [JsonIgnore]
    public string Value => ToString();

    [JsonIgnore]
    public virtual string HeadingForPersonalAccount { get; protected set; } = string.Empty;
    [JsonIgnore]
    public virtual string HeadingForInputData { get; protected set; } = string.Empty;

    [JsonIgnore]
    public abstract IList<IMenuItem> PersonalAccountMenuItems { get; }

    [JsonIgnore]
    public virtual InputData? InputDataWindow { get; } = null;

    public int AbstractUserType => GetRoleNumber();

    public AbstractUser(int id, string login, string password)
    {
        ID = id;
        Login = login;
        Password = password;
        OnPressed += OnUserPressed;
    }

    public void Action(int numberMenuItemInMenu)
    {
        OnPressed?.Invoke(this, new MenuItemEventArgs(numberMenuItemInMenu));
    }

    public void Clear() { }

    public void OnUserPressed(object? sender, MenuItemEventArgs e)
    {
        var inputData = new InputData(new List<IMenuItem>
        {
            new IntField("ID", ID.ToString()),
            new StringField("Логин", Login),
            new StringField("Пароль", Password),
            new IntField("Роль", GetRoleNumber().ToString())
        });
        var button = new Button("Изменить данные пользователя", inputData);
        button.OnPressed += OnUserUpdated;
        button.OnPressed += inputData.OnInputDataClosed;
        inputData.MenuItems.Add(button);
        inputData.Show();
    }

    public void OnUserUpdated(object? sender, MenuItemEventArgs e)
    {
        if (e.window == null || e.window is not InputData)
        {
            throw new ArgumentException();
        }

        var id = Convert.ToInt32(e.window.MenuItems[0].Value);
        var login = e.window.MenuItems[1].Value;
        var password = e.window.MenuItems[2].Value;
        var role = Convert.ToInt32(e.window.MenuItems[3].Value);
        if (role < 0 || role > 4)
        {
            Menu.ErrorMassage("Роли с таким номером не существует");
            return;
        }

        // если роль не менялась
        if (this.GetRoleNumber() == role)
        {
            this.ID = Convert.ToInt32(e.window.MenuItems[0].Value);
            this.Login = e.window.MenuItems[1].Value;
            this.Password = e.window.MenuItems[2].Value;
        }
        // если роль менялась у кого-то кто не данный админ
        // вопрос, сравнятся ли юзеры так они вообще
        else if (this != ShopSystem.ActiveUser)
        {
            var oldUser = ShopSystem.GetUser(Login, Password);
            if (oldUser == null)
            {
                throw new ArgumentNullException();
            }
            ShopSystem.SystemData.Users.Remove(oldUser);
            AbstractUser.SetNewUserFromRole(role, id, login, password);
        }
        // если роль меняется у данного админа
        else
        {
            Menu.ErrorMassage("Нельзя изменить роль текущему пользователю!");
        }
    }

    public abstract string GetRole();
    public abstract int GetRoleNumber();

    public static void SetNewUserFromRole(int role, int id, string login, string password)
    {
        switch (role)
        {
            case 0:
                ShopSystem.SystemData.Users.Add(new Admin(id, login, password));
                break;
            case 1:
                ShopSystem.SystemData.Users.Add(new Accountant(id, login, password));
                break;
            case 2:
                ShopSystem.SystemData.Users.Add(new Cashier(id, login, password));
                break;
            case 3:
                ShopSystem.SystemData.Users.Add(new HR(id, login, password));
                break;
            case 4:
                ShopSystem.SystemData.Users.Add(new Stock(id, login, password));
                break;
            default:
                throw new ArgumentException();
        }
    }

    public override string ToString()
    {
        return $"{new string(' ', Menu.HorizontalShift)}{ID,-10}{Login,-20}{Password,-20}{GetRole(),-20}";
    }
}