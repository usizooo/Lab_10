public class Menu
{
    public readonly static int HorizontalShift = 4;
    public readonly static int VerticalShift = 4;

    public string Heading { get; set; } = string.Empty;
    public bool MenuActive { get; set; } = true;

    public void Show(IWindow activeWindow, string title)
    {
        MenuActive = true;
        int currentItem = 0;
        while (MenuActive)
        {
            PrintTitle(title);
            PrintMenuItems(activeWindow.MenuItems, currentItem);
            if (activeWindow.MenuItems.Count == 0)
            {
                Console.ReadKey();
                continue;
            }
            var action = Console.ReadKey();
            switch (action.Key)
            {
                case ConsoleKey.UpArrow:
                    currentItem = (currentItem - 1) % activeWindow.MenuItems.Count < 0
                        ? activeWindow.MenuItems.Count - 1
                        : (currentItem - 1) % activeWindow.MenuItems.Count;
                    break;
                case ConsoleKey.DownArrow:
                    currentItem = (currentItem + 1) % activeWindow.MenuItems.Count;
                    break;
                case ConsoleKey.Enter:
                    activeWindow.MenuItems[currentItem].Action(currentItem);
                    break;
                case ConsoleKey.Escape:
                    // возможно тут нужно очистить буфер консоли
                    activeWindow.Close();
                    break;
                default:
                    ErrorMassage("Некорректный ввод, попытайтесь снова");
                    break;
            }
        }
    }

    public void ClearMenuItems(IList<IMenuItem> menuItems)
    {
        foreach (IMenuItem item in menuItems)
            item.Clear();
    }

    public void PrintMenuItems(IList<IMenuItem> menuItems, int currentItem)
    {
        Console.SetCursorPosition(0, VerticalShift);
        for (int i = 0; i < menuItems.Count; ++i)
        {
            var menuItemValue = menuItems[i] is PasswordField 
                ? new string('*', menuItems[i].Value.Length) 
                : menuItems[i].Value;
            Console.WriteLine
            (
                currentItem != i
                ? $"{new string(' ', HorizontalShift)}{menuItems[i].Name} {menuItemValue}"
                : $"{"->",-4}{menuItems[i].Name} {menuItemValue}"
            );
        }
    }

    public void PrintTitle(string title)
    {
        Console.Clear();
        Console.WriteLine(new string('-', Console.BufferWidth));
        Console.SetCursorPosition((Console.WindowWidth - title.Length) / 2, Console.CursorTop);
        Console.WriteLine(title);
        Console.WriteLine(new string('-', Console.BufferWidth));
        Console.WriteLine(Heading);
    }

    public static void ErrorMassage(string errorInfo)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(errorInfo);
        Console.ReadKey();
        Console.ResetColor();
    }

    public static void InformationMassage(string information)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(information);
        Console.ReadKey();
        Console.ResetColor();
    }

    public static void SuccessMassage(string successInfo)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(successInfo);
        Console.ReadKey();
        Console.ResetColor();
    }
}