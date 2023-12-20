public interface IMenuItem
{
    string Name { get; }
    string Value { get; }

    public EventHandler<MenuItemEventArgs>? OnPressed { get; set; }

    void Action(int numberMenuItemInMenu);

    void Clear();
}