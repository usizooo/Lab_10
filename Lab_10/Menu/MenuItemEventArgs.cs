public class MenuItemEventArgs : EventArgs
{
    public readonly IWindow? window;
    public readonly int numberMenuItemInMenu;

    public MenuItemEventArgs(int numberMenuItemInMenu, IWindow? window = null)
    {
        this.window = window;
        this.numberMenuItemInMenu = numberMenuItemInMenu;
    }
}