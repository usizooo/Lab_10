public class BoolField : IMenuItem
{
    public EventHandler<MenuItemEventArgs>? OnPressed { get; set; }

    public string Name { get; set; }

    private string value = string.Empty;
    public string Value
    {
        get => value;
        set => this.value = value;
    }
    public BoolField(string name)
    {
        Name = name;
        OnPressed += BoolFieldActionProcessing;
    }


    public BoolField(string name, string value) : this(name) => this.value = value;


    public void Action(int numberMenuItemInMenu)
    {
        OnPressed?.Invoke(this, new MenuItemEventArgs(numberMenuItemInMenu));
    }

    private void BoolFieldActionProcessing(object? sender, MenuItemEventArgs e)
    {
        Console.SetCursorPosition(Menu.HorizontalShift + Name.Length + 1, Menu.VerticalShift + e.numberMenuItemInMenu);
        Console.WriteLine(new string(' ', value.Length));
        Console.SetCursorPosition(Menu.HorizontalShift + Name.Length + 1, Menu.VerticalShift + e.numberMenuItemInMenu);
        try
        {
            value = Convert.ToBoolean(Console.ReadLine()).ToString();
        }
        catch
        {
            Menu.ErrorMassage("Некорректный ввод");
        }
    }

    public void Clear() => value = string.Empty;
}