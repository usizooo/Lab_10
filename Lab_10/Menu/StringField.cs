public class StringField : IMenuItem
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

    public StringField(string name)
    {
        Name = name;
        OnPressed += StringFieldActionProcessing;
    }

    public StringField(string name, string value) : this(name) => this.value = value;

    public StringField(string name, string value, bool isReadonlyField) : this(name, value)
        => this.isReadonlyField = isReadonlyField;

    public void Action(int numberMenuItemInMenu)
    {
        if (!isReadonlyField)
        {
            OnPressed?.Invoke(this, new MenuItemEventArgs(numberMenuItemInMenu));
        }
    }

    public void StringFieldActionProcessing(object? sender, MenuItemEventArgs e)
    {
        Console.SetCursorPosition(Menu.HorizontalShift + Name.Length + 1, Menu.VerticalShift + e.numberMenuItemInMenu);
        Console.WriteLine(new string(' ', value.Length));
        Console.SetCursorPosition(Menu.HorizontalShift + Name.Length + 1, Menu.VerticalShift + e.numberMenuItemInMenu);
        value = Console.ReadLine() ?? throw new NullReferenceException();
    }

    public void Clear() => value = string.Empty;
}