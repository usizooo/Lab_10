public class PasswordField : IMenuItem
{
    public EventHandler<MenuItemEventArgs>? OnPressed { get; set; }
    public string Name { get; set; }
    private string password = string.Empty;
    public string Value
    {
        get => password;
        set => password = value;
    }

    public PasswordField(string name)
    {
        Name = name;
        OnPressed += PasswordFieldActionProcessing;
    }

    public PasswordField(string name, string password) : this(name) => this.password = password;

    public void Action(int numberIMenuItemInMenu)
    {
        OnPressed?.Invoke(this, new MenuItemEventArgs(numberIMenuItemInMenu));
    }

    private void PasswordFieldActionProcessing(object? sender, MenuItemEventArgs e)
    {
        Console.SetCursorPosition(Menu.HorizontalShift + Name.Length + 1, Menu.VerticalShift + e.numberMenuItemInMenu);
        Console.WriteLine(new string(' ', password.Length));
        Console.SetCursorPosition(Menu.HorizontalShift + Name.Length + 1, Menu.VerticalShift + e.numberMenuItemInMenu);
        // Добавить звёздочки
        password = EnterPassword(string.Empty);
    }

    public void Clear() => password = string.Empty;

    public string EnterPassword(string enterText)
    {
        string enteredPassword = string.Empty;
        try
        {
            Console.Write(enterText);
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work  
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    enteredPassword += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && enteredPassword.Length > 0)
                    {
                        enteredPassword = enteredPassword.Substring(0, (enteredPassword.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        if (string.IsNullOrWhiteSpace(enteredPassword))
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Empty value not allowed.");
                            EnterPassword(enterText);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            } while (true);
        }
        catch
        {
            throw new Exception();
        }
        return enteredPassword;
    }
}