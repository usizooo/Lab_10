using Newtonsoft.Json;

public class Record : IMenuItem
{
    public int ID { get; set; }
    public string RecordName { get; set; }
    public int Amount { get; set; }
    public DateTime TimeOfCreation { get; set; }
    public bool IsIncrease { get; set; }

    public string Name => string.Empty;

    public string Value => ToString();

    [JsonIgnore]
    public EventHandler<MenuItemEventArgs>? OnPressed { get; set; }

    public Record(int id, string recordName, int amount, bool isIncrease)
    {
        ID = id;
        RecordName = recordName;
        Amount = amount;
        TimeOfCreation = DateTime.Now;
        IsIncrease = isIncrease;
        OnPressed += OnRecordPressed;
        Accountant.Amount = IsIncrease ? Accountant.Amount + Amount : Accountant.Amount - Amount;
    }

    public void Action(int numberMenuItemInMenu)
    {
        OnPressed?.Invoke(this, new MenuItemEventArgs(numberMenuItemInMenu));
    }

    public void OnRecordPressed(object? sender, MenuItemEventArgs e)
    {
        var inputData = new InputData(new List<IMenuItem>
        {
            new IntField("ID", ID.ToString()),
            new StringField("Название", RecordName),
            new IntField("Сумма", Amount.ToString()),
            new BoolField("Прибавка?", IsIncrease.ToString())

        });
        var button = new Button("Изменить данные о записи бух. учёта", inputData);
        button.OnPressed += OnRecordUpdated;
        button.OnPressed += inputData.OnInputDataClosed;
        inputData.MenuItems.Add(button);
        inputData.Show();
    }

    private void OnRecordUpdated(object? sender, MenuItemEventArgs e)
    {
        if (e.window == null || e.window is not InputData)
        {
            throw new ArgumentException();
        }

        // Нужно произвести проверку на корректность данных

        this.ID = Convert.ToInt32(e.window.MenuItems[0].Value);
        this.RecordName = e.window.MenuItems[1].Value;
        this.Amount = Convert.ToInt32(e.window.MenuItems[2].Value);
        this.IsIncrease = Convert.ToBoolean(e.window.MenuItems[3].Value);
    }

    public void Clear() { }

    public override string ToString()
    {
        var time = TimeOfCreation.Date.ToString().Substring(0, 10) + " " 
            + TimeOfCreation.TimeOfDay.ToString().Substring(0, 8);
        return $"{new string(' ', Menu.HorizontalShift)}" +
            $"{ID, -10}{RecordName, -20}{Amount, -10}{time, -20}{IsIncrease, -10}";
    }
}