using Controller;
using Mapbox.Json;
using SavyEngine.Controller;
using SavyEngine.Controller.Handler;
using SavySoda.SavySodaSDK;
using UnityEngine;

namespace SavySoda.MillionaireTycoonGo
{
    public class DeepLinkController : SingletonBehaviour<DeepLinkController> //MonoBehaviour
    {
        void Awake()
        {
          
            Application.deepLinkActivated += onDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                // Cold start and Application.absoluteURL not null so process Deep Link.
                onDeepLinkActivated(Application.absoluteURL);
            }
            // // Initialize DeepLink Manager global variable.
            // else deeplinkURL = "[none]";
            
            Debug.Log("DeepLink singleton awake");
        }
        
        #region deepLink

        /// <summary>
        /// currently there are 2 deep links:
        /// - when register with telegram, the deeplink will be sth like unityssmp://telegram.reg?{telegramUserInfoBase64Encoded}
        /// - when login with telegram, the deeplink will be sth like unityssmp://telegram.login?{telegramUserInfoBase64Encoded}
        /// </summary>
        /// <param name="url"></param>
        private void onDeepLinkActivated(string url)
        {
            // Update DeepLink Manager global variable, so URL can be accessed from anywhere.
            // deeplinkURL = url;
            if (string.IsNullOrWhiteSpace(url))
            {
                Debug.LogError("Deeplink url is empty");
                return; 
            }
            
            AlertController.ShowErrorAlert("DeepLink Info".Translate(), url,() =>
            {
                
            });  
            var urlInfo = url.Split('?');
            
            switch (urlInfo[0])
            {
                case "telegram.reg":
                case "telegram.login":
                    var telegramUserInfoJson = urlInfo[1].Base64Decode();
                    var telegramUserInfo = JsonConvert.DeserializeObject<TelegramLogin>(telegramUserInfoJson);
                    // default is for login
                    var intent = AuthController.AuthIntent.Login;
                    var downloadHandler = new DownloadCompletionHandler(gameObject,(message,success,errorCode) =>
                    {
                        GameSceneController.CurrentSceneController.ReloadGame();
                    });
                    // if not login, change to register
                    if (string.Equals(urlInfo[0], "telegram.reg"))
                    {
                        intent = AuthController.AuthIntent.Register;
                        downloadHandler = new DownloadCompletionHandler(gameObject,(message,success,errorCode) =>
                        {
                            AlertController.ShowBasicAlert("Login Successful!".Translate(),
                                string.Format(
                                    "You've successfully logged in with Telegram. Your account is now linked.".Translate()));
                        });
                    }
                    
                    AuthController.Instance.FinaliseTelegramLogin(intent, telegramUserInfo,downloadHandler);
                    
                    break;
                default:
                    Debug.LogError($"Unknown deeplink starts: {urlInfo[0]}");
                    return; 
            }

        }

        #endregion
    }
}