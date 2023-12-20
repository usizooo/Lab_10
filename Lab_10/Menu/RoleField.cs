public class RoleField : IMenuItem
{
    public EventHandler<MenuItemEventArgs>? OnPressed { get; set; }

    public string Name { get; set; }

    private string role = string.Empty;
    public string Value
    {
        get
        {
            switch (role)
            {
                case "0":
                    return "0 - Администратор";
                case "1":
                    return "1 - Бухгалтер";
                case "2":
                    return "2 - Кассир";
                case "3":
                    return "3 - Менеджер по персоналу";
                case "4":
                    return "4 - Складской";
                default:
                    throw new ArgumentException(nameof(role));
            }
        }
        set => this.role = value;
    }
    public IWindow? Window { get; set; } = null;
    public RoleField(string name)
    {
        Name = name;
        OnPressed += IntFieldActionProcessing;
    }

    public RoleField(string name, IWindow window) : this(name) => Window = window;

    public RoleField(string name, string value) : this(name) => this.role = value;

    public RoleField(string name, string value, IWindow window) : this(name, value) => Window = window;

    public void Action(int numberMenuItemInMenu)
    {
        OnPressed?.Invoke(this, new MenuItemEventArgs(numberMenuItemInMenu, Window));
    }

    private void IntFieldActionProcessing(object? sender, MenuItemEventArgs e)
    {
        Console.SetCursorPosition(Menu.HorizontalShift + Name.Length + 1, Menu.VerticalShift + e.numberMenuItemInMenu);
        Console.WriteLine(new string(' ', role.Length));
        Console.SetCursorPosition(Menu.HorizontalShift + Name.Length + 1, Menu.VerticalShift + e.numberMenuItemInMenu);
        try
        {
            var input = Console.ReadLine();
            if (input == Value)
            {
                return;
            }
            var _role = Convert.ToInt32(input);
            if (_role < 0 || _role > 4)
            {
                if (e.window == null)
                {
                    throw new ArgumentNullException();
                }
                Menu.ErrorMassage("Такой роли не существует");
                return;
            }
            role = _role.ToString();
        }
        catch
        {
            if (e.window == null)
            {
                throw new ArgumentNullException();
            }
            Menu.ErrorMassage("Некорректный ввод");
        }
    }

    public void Clear() => role = string.Empty;
}