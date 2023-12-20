public class Program
{
    public static void Main()
    {
        //ShopSystem.SystemData.Stockroom.Add(new Product(0, "milk", 34, 100));

        //ShopSystem.SystemData.Users.Add(new Admin(0, "login", "password"));
        //ShopSystem.SystemData.Users.Add(new HR(1, "login1", "password1"));
        //ShopSystem.SystemData.Users.Add(new Accountant(2, "login2", "password2"));
        //ShopSystem.SystemData.Users.Add(new Cashier(3, "login3", "password3"));
        //ShopSystem.SystemData.Users.Add(new Stock(4, "login4", "password4"));

        ShopSystem.Run("users.json", "employees.json", "accounting.json", "stockroom.json");
    }
}