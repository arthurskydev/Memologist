using Bot.Common.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Bot.Common
{
    public class StringService : IStringService
    {
        public string this[string key]
        {
            get
            {
                return GetString(key);
            }

            set { }
        }

        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        private Dictionary<string, string> _strings = new();

        public StringService(
            IConfiguration configuration,
            ILogger<StringService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            ChangeStringSet(_configuration["StringSetPath"]);
        }

        private string GetString(string key)
        {

            var result = _strings.FirstOrDefault(x => x.Key == key);
            if (!string.IsNullOrEmpty(result.Value))
            {
                return result.Value;
            }
            throw new Exception(message: "No string value found");
        }

        private void ChangeStringSet(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = $"{Directory.GetCurrentDirectory()}/Settings/stringset.json";
                }
                string jsonString = File.ReadAllText(filePath);
                _strings = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

            }
            catch
            {
                _logger.LogError("Could not load string set. Will default to \"Error while reading string set / Error code: 0\".");
            }
        }
    }
}
