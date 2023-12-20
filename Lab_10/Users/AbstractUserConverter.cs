using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class AbstractUserConverter : JsonCreationConverter<AbstractUser>
{
    protected override AbstractUser Create(Type objectType, JObject jObject)
    {
        var id = (jObject["ID"] ?? throw new ArgumentNullException()).Value<int>();
        var login = (jObject["Login"] ?? throw new ArgumentNullException()).Value<string>() 
            ?? throw new ArgumentNullException();
        var password = (jObject["Password"] ?? throw new ArgumentNullException()).Value<string>() 
            ?? throw new ArgumentNullException();
        switch ((jObject["AbstractUserType"] ?? throw new ArgumentNullException()).Value<int>())
        {
            case 0:
                return new Admin(id, login, password);
            case 1:
                return new Accountant(id, login, password);
            case 2:
                return new Cashier(id, login, password);
            case 3:
                return new HR(id, login, password);
            case 4:
                return new Stock(id, login, password);
            default:
                throw new ArgumentNullException();
        }
    }
}

public abstract class JsonCreationConverter<T> : JsonConverter
{
    protected abstract T Create(Type objectType, JObject jObject);

    public override bool CanConvert(Type objectType)
    {
        return typeof(T) == objectType;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        try
        {
            var jObject = JObject.Load(reader);
            var target = Create(objectType, jObject) ?? throw new ArgumentNullException();
            serializer.Populate(jObject.CreateReader(), target);
            return target;
        }
        catch (JsonReaderException)
        {
            throw new ArgumentException();
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}