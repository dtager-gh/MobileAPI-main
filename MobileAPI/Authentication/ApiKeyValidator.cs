using Microsoft.Extensions.Options;

namespace MobileAPI.Authentication
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly string _keyName;
        private readonly string _validApiKey;

        public ApiKeyValidator(IOptions<ApiKeyOptions> apiKeyOptions)
        {
            _keyName = apiKeyOptions.Value.Header;
            _validApiKey = apiKeyOptions.Value.Key;
        }

        public bool IsValid(string apiKey)
        {
            return !string.IsNullOrWhiteSpace(apiKey) && apiKey == _validApiKey;
        }

        public string KeyName()
        { 
            return _keyName;
        }
    }
}
