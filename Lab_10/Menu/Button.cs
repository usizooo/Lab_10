public class Button : IMenuItem
{
    public EventHandler<MenuItemEventArgs>? OnPressed { get; set; }
    public string Name { get; set; }
    public string Value => string.Empty;

    public IWindow? Window { get; set; } = null;

    public Button(string name) => Name = name;

    public Button(string name, IWindow window) : this(name) => Window = window;

    public void Action(int numberMenuItemInMenu)
    {
        OnPressed?.Invoke(this, new MenuItemEventArgs(numberMenuItemInMenu, Window));
    }

    public void Clear() { }
}