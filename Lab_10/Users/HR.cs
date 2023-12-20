public class HR : AbstractUser
{
    public override string HeadingForPersonalAccount =>
        $"{new string(' ', Menu.HorizontalShift * 2 + 1)}" +
        $"{"ID",-10}{"Имя",-20}{"Фамилия",-20}{"Отчество",-20}{"Должность",-10}";

    public override string HeadingForInputData =>
        $"{new string(' ', Menu.HorizontalShift)}" +
        $"Роли: 0 - админ., 1 - бухгалтер, 2 - кассир, 3 - менеджер по персоналу, 4 - складской";

    public HR(int id, string login, string password) : base(id, login, password)
    {
    }

    public override InputData InputDataWindow
    {
        get
        {
            var inputDataWindow = new InputData(new List<IMenuItem>
            {
                new IntField("ID"),
                new StringField("Имя"),
                new StringField("Фамилия"),
                new StringField("Отчество"),
                new StringField("Дата рождения"),
                new StringField("Серия и номер паспорта"),
                new StringField("Роль"),
                new IntField("Зарплата"),
                new StringField("Аккаунт")

            });
            var button = new Button("Добавить нового сотрудника", inputDataWindow);
            button.OnPressed += OnNewEmployeeAdded;
            button.OnPressed += inputDataWindow.OnInputDataClosed;
            inputDataWindow.MenuItems.Add(button);
            return inputDataWindow;
        }
    }

    private void OnNewEmployeeAdded(object? sender, MenuItemEventArgs e)
    {
        if (e.window == null || e.window is not InputData)
        {
            throw new ArgumentException();
        }

        var id = Convert.ToInt32(e.window.MenuItems[0].Value);
        var firstName = e.window.MenuItems[1].Value;
        var lastName = e.window.MenuItems[2].Value;
        var patronymic = e.window.MenuItems[3].Value;
        var dateOfBirth = e.window.MenuItems[4].Value;
        var passportData = e.window.MenuItems[5].Value;
        var role = e.window.MenuItems[6].Value;
        var salary = Convert.ToInt32(e.window.MenuItems[7].Value);
        var account = e.window.MenuItems[8].Value;

        var newEmployee = 
            new Employee(id, firstName, lastName, patronymic, dateOfBirth, passportData, role, salary, account);

        // Проверить, что такого сотрудника ещё не было
        ShopSystem.SystemData.Employees.Add(newEmployee);
    }

    private static IList<IMenuItem> personalAccountMenuItems = new List<IMenuItem>();
    public override IList<IMenuItem> PersonalAccountMenuItems
    {
        get
        {
            personalAccountMenuItems.Clear();
            foreach (var employee in ShopSystem.SystemData.Employees)
            {
                personalAccountMenuItems.Add(employee);
            }

            var buttonToInputDataWindow = new Button("Добавить нового сотрудника", InputDataWindow);
            buttonToInputDataWindow.OnPressed += InputDataWindow.OnInputDataShowed;
            personalAccountMenuItems.Add(buttonToInputDataWindow);

            return personalAccountMenuItems;
        }
    }

    public override int GetRoleNumber() => 3;
    public override string GetRole() => "Менеджер по персоналу";
}