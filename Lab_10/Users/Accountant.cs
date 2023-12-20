public class Accountant : AbstractUser
{
    public static int Amount = 0;

    public override string HeadingForPersonalAccount => 
        $"{new string(' ', Menu.HorizontalShift * 2 + 1)}" +
        $"{"ID",-10}{"Название",-20}{"Сумма",-10}{"Время записи",-20}{"Прибавка?",-10} Итоговая сумма: {Amount} рублей";

    public override string HeadingForInputData => 
        $"{new string(' ', Menu.HorizontalShift)}" +
        $"Прибавка?: True - прибавка суммируется с итоговой выручкой, " +
        $"False - прибавка вычитается из итоговой выручки";

    public Accountant(int id, string login, string password) : base(id, login, password)
    {
    }

    public override InputData InputDataWindow
    {
        get
        {
            var inputDataWindow = new InputData(new List<IMenuItem>
            {
                new IntField("ID"),
                new StringField("Название"),
                new IntField("Сумма"),
                new BoolField("Прибавка?")
            });
            var button = new Button("Добавить новый запись бух. учета", inputDataWindow);
            button.OnPressed += OnNewRecordAdded;
            button.OnPressed += inputDataWindow.OnInputDataClosed;
            inputDataWindow.MenuItems.Add(button);
            return inputDataWindow;
        }
    }

    private void OnNewRecordAdded(object? sender, MenuItemEventArgs e)
    {
        if (e.window == null || e.window is not InputData)
        {
            throw new ArgumentException();
        }

        var id = Convert.ToInt32(e.window.MenuItems[0].Value);
        var recordName = e.window.MenuItems[1].Value;
        var amount = Convert.ToInt32(e.window.MenuItems[2].Value);
        var isIncrease = Convert.ToBoolean(e.window.MenuItems[3].Value);

        var newRecord = new Record(id, recordName, amount, isIncrease);

        // Проверить, что такого продукта ещё не было
        ShopSystem.SystemData.Accounting.Add(newRecord);
        Menu.InformationMassage($"{new string(' ', Menu.HorizontalShift)}" +
            $"Итоговая выручка измениться после выхода из личного кабинета");
        Console.ReadKey();
    }

    private static IList<IMenuItem> personalAccountMenuItems = new List<IMenuItem>();
    public override IList<IMenuItem> PersonalAccountMenuItems
    {
        get
        {
            personalAccountMenuItems.Clear();
            foreach (var record in ShopSystem.SystemData.Accounting)
            {
                personalAccountMenuItems.Add(record);
            }

            var buttonToInputDataWindow = new Button("Добавить запись бух. учёта", InputDataWindow);
            buttonToInputDataWindow.OnPressed += InputDataWindow.OnInputDataShowed;
            personalAccountMenuItems.Add(buttonToInputDataWindow);

            return personalAccountMenuItems;
        }
    }

    public override string GetRole() => "Бухгалтер";

    public override int GetRoleNumber() => 1;
}