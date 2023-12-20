public class IntField : IMenuItem
{
    public EventHandler<MenuItemEventArgs>? OnPressed { get; set; }

    private bool isReadonlyField = false;

    public string Name { get; set; }

    private string value = string.Empty;
    public string Value
    {
        get => value;
        set => this.value = value;
    }
    public IntField(string name)
    {
        Name = name;
        OnPressed += IntFieldActionProcessing;
    }
    
    public IntField(string name, string value) : this(name) => this.value = value;

    public IntField(string name, string value, bool isReadonlyField) : this(name, value) 
        => this.isReadonlyField = isReadonlyField;

    public void Action(int numberMenuItemInMenu)
    {
        if (!isReadonlyField)
        {
            OnPressed?.Invoke(this, new MenuItemEventArgs(numberMenuItemInMenu));
        }
    }

    private void IntFieldActionProcessing(object? sender, MenuItemEventArgs e)
    {
        Console.SetCursorPosition(Menu.HorizontalShift + Name.Length + 1, Menu.VerticalShift + e.numberMenuItemInMenu);
        Console.WriteLine(new string(' ', value.Length));
        Console.SetCursorPosition(Menu.HorizontalShift + Name.Length + 1, Menu.VerticalShift + e.numberMenuItemInMenu);
        try
        {
            value = Convert.ToInt32(Console.ReadLine()).ToString();
        }
        catch 
        {
            Menu.ErrorMassage("Некорректный ввод");
        }
    }

    public void Clear() => value = string.Empty;
}