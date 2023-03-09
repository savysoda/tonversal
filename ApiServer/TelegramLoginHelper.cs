using System;
using System.Collections.Generic;
using Telegram.Bot.Extensions.LoginWidget;

namespace SavySoda.SavySodaSDK
{
    public class TelegramLoginHelper
    {

        private LoginWidget _loginWidget;
        public static TelegramLoginHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("Call TelegramLoginHelper.Setup(string botToken) before using it");
                }

                return _instance;
            }
        }

        public static void Setup(string botToken)
        {
            _instance = new TelegramLoginHelper(botToken);
        }
        
        private static TelegramLoginHelper _instance;

        private TelegramLoginHelper(string botToken)
        {
            // _secretKey = CryptographyHelper.HashSha256(botToken);
            _loginWidget = new LoginWidget(botToken);
        }
        public bool Valid(Dictionary<string, string> r)
        {
            return _loginWidget.CheckAuthorization(r) == Authorization.Valid;
        }


    }
}
