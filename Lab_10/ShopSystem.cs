using Newtonsoft.Json;

public class SystemData
{
    public IList<AbstractUser> Users { get; set; } = new List<AbstractUser>();
    public IList<Employee> Employees { get; set; } = new List<Employee>();
    public IList<Product> Stockroom { get; set; } = new List<Product>();
    public IList<Record> Accounting { get; set; } = new List<Record>();

    public SystemData(IList<AbstractUser> users, IList<Employee> employees, 
        IList<Product> stockroom, IList<Record> accounting)
    {
        Users = users;
        Employees = employees;
        Stockroom = stockroom;
        Accounting = accounting;
    }
}

public static class ShopSystem
{
    public static SystemData SystemData { get; set; }
        = new SystemData(new List<AbstractUser>(), new List<Employee>(), new List<Product>(), new List<Record>());

    public static AbstractUser? ActiveUser { get; set; } = null;

    public static void Run(string users, string employees, string accounting, string stockroom)
    {
        SystemData.Users = Deserialization(users, SystemData.Users, new AbstractUserConverter());
        SystemData.Employees = Deserialization(employees, SystemData.Employees);
        SystemData.Accounting = Deserialization(accounting, SystemData.Accounting);
        SystemData.Stockroom = Deserialization(stockroom, SystemData.Stockroom);

        new Authorization().Show();

        Serialization(users, SystemData.Users);
        Serialization(employees, SystemData.Employees);
        Serialization(accounting, SystemData.Accounting);
        Serialization(stockroom, SystemData.Stockroom);
    }

    public static AbstractUser? GetUser(string login, string password)
    {
        for (int i = 0; i < SystemData.Users.Count; i++)
        {
            if (SystemData.Users[i].Login == login && SystemData.Users[i].Password == password)
            {
                return SystemData.Users[i];
            }
        }
        return null;
    }
    public static void Serialization<T>(string path, T value)
    {
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            streamWriter.Write(JsonConvert.SerializeObject(value));
        }
    }

    public static T Deserialization<T>(string path, T outValue, AbstractUserConverter? converter = null)
    {
        using (StreamReader streamReader = new StreamReader(path))
        {
            string jsonData = streamReader.ReadToEnd();
            if (jsonData == string.Empty)
            {
                return outValue;
            }
            if (converter == null)
            {
                outValue = JsonConvert.DeserializeObject<T>(jsonData)
                    ?? throw new NullReferenceException("Данные файла повреждены повреждены.");
            }
            else
            {
                outValue = JsonConvert.DeserializeObject<T>(jsonData, converter)
                    ?? throw new NullReferenceException("Данные файла повреждены повреждены.");
            }
        }
        return outValue;
    }
}