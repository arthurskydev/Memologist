﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Bot.Services.StringProcService
{
    public class StringProcService : IStringProcService
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

        private Dictionary<string, string> _strings = new Dictionary<string, string>();

        public StringProcService(
            IConfiguration configuration,
            ILogger<StringProcService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            ChangeStringSet(_configuration["StringSetPath"]);
        }

        private string GetString(string key)
        {
            try
            {
                var result = _strings.FirstOrDefault(x => x.Key == key);
                if (!string.IsNullOrEmpty(result.Value))
                {
                    return result.Value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
            }

            return "Error code: 0";
        }

        private void ChangeStringSet(string filePath)
        {
            try
            {
                if (String.IsNullOrEmpty(filePath))
                {
                    filePath = $"{Directory.GetCurrentDirectory()}/stringset.json";
                }
                string jsonString = File.ReadAllText(filePath);
                _strings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

            }
            catch
            {
                _logger.LogError("Could not load string set. Will default to \"Error while reading string set / Error code: 0\".");
            }
        }
    }
}
