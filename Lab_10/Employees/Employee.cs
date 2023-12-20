using Newtonsoft.Json;

public class Employee : IMenuItem
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string DateOfBirth { get; set; }
    public string PassportData { get; set; }
    public string Role { get; set; }
    public int Salary { get; set; }
    public string Account { get; set; }

    public string Name => string.Empty;
    public string Value => ToString();

    [JsonIgnore]
    public EventHandler<MenuItemEventArgs>? OnPressed { get; set; }

    public Employee(int id, string firstName, string secondName, string patronymic, 
        string dateOfBirth, string passportData, string role, int salary, string account)
    {
        ID = id;
        FirstName = firstName;
        LastName = secondName;
        Patronymic = patronymic;
        DateOfBirth = dateOfBirth;
        PassportData = passportData;
        Role = role;
        Salary = salary;
        Account = account;
        OnPressed += OnEmployeePressed;
    }

    public void Action(int numberMenuItemInMenu)
    {
        OnPressed?.Invoke(this, new MenuItemEventArgs(numberMenuItemInMenu));
    }
    
    public void OnEmployeePressed(object? sender, MenuItemEventArgs e)
    {
        var inputData = new InputData(new List<IMenuItem>
        {
            new IntField("ID", ID.ToString()),
            new StringField("Имя", FirstName),
            new StringField("Фамилия", LastName),
            new StringField("Отчество", Patronymic),
            new StringField("Дата рождения", DateOfBirth),
            new StringField("Серия и номер паспорта", PassportData),
            new StringField("Роль", Role),
            new IntField("Зарплата", Salary.ToString()),
            new StringField("Аккаунт", Account)

        });
        var button = new Button("Изменить данные сотрудника", inputData);
        button.OnPressed += OnEmployeeUpdated;
        button.OnPressed += inputData.OnInputDataClosed;
        inputData.MenuItems.Add(button);
        inputData.Show();
    }

    private void OnEmployeeUpdated(object? sender, MenuItemEventArgs e)
    {
        if (e.window == null || e.window is not InputData)
        {
            throw new ArgumentException();
        }

        // Нужно произвести проверку на корректность данных и
        // проверить не меняется ли роль у текущего пользователя

        this.ID = Convert.ToInt32(e.window.MenuItems[0].Value);
        this.FirstName = e.window.MenuItems[1].Value;
        this.LastName = e.window.MenuItems[2].Value;
        this.Patronymic = e.window.MenuItems[3].Value;
        this.DateOfBirth = e.window.MenuItems[4].Value;
        this.PassportData = e.window.MenuItems[5].Value;
        this.Role = e.window.MenuItems[6].Value;
        this.Salary = Convert.ToInt32(e.window.MenuItems[7].Value);
        this.Account = e.window.MenuItems[8].Value;
    }

    public void Clear() { }

    public override string ToString()
    {
        return $"{new string(' ', Menu.HorizontalShift)}{ID, -10}{FirstName, -20}{LastName,-20}{Patronymic,-20}{Role, -10}";
    }
}