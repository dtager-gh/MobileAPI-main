namespace MobileAPI.Authentication
{
    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
        string KeyName();
    }

}
