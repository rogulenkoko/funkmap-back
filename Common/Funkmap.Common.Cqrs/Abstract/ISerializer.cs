namespace Funkmap.Common.Cqrs.Abstract
{
    public interface ISerializer
    {
        string Serialize(object value, SerializerOptions options = null);

        T Deserialize<T>(string value, SerializerOptions options = null) where T : class;
    }
}
