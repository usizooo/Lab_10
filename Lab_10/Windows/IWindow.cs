public interface IWindow
{
    public Menu CurrentMenu { get; }
    IList<IMenuItem> MenuItems { get; }
    void Show();
    void Close();
}