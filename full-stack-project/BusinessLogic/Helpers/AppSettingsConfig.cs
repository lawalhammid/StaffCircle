using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Helpers
{
    public static class AppSettingsConfig
    {
        public static int MessageLength()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("AppSettings.json");

            IConfiguration configuration = configurationBuilder.Build();
           
            return  Convert.ToInt32(configuration["SMSMaxLength"]);
            
        }

        public static TwiloCredential TwiloCredential()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("AppSettings.json");

            IConfiguration configuration = configurationBuilder.Build();
            string TWILIO_ACCOUNT_SID =  configuration["TwiloCredential:TWILIO_ACCOUNT_SID"];
            string TWILIO_AUTH_TOKEN = configuration["TwiloCredential:TWILIO_AUTH_TOKEN"];
            
            return new TwiloCredential
            {
                 TWILIO_ACCOUNT_SID = TWILIO_ACCOUNT_SID,
                 TWILIO_AUTH_TOKEN = TWILIO_AUTH_TOKEN
            };
        }
    }
}

