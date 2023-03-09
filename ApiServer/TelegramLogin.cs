using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SavySoda.SharedModel;

namespace SavySoda.SavySodaSDK
{
    public class TelegramLogin: Jsonable
    {
        //id, first_name, last_name, username, photo_url, auth_date and hash fields
        [JsonProperty("id")]
        public string Id { set; get; }
        
        [JsonProperty("first_name")]
        public string FirstName { set; get; }
        
        [JsonProperty("last_name")]
        public string LastName { set; get; }
        
        [JsonProperty("username")]
        public string Username { set; get; }
        
        [JsonProperty("photo_url")]
        public string PhotoUrl { set; get; }
        
        [JsonProperty("auth_date")]
        public long AuthDate { set; get; }
        
        [JsonProperty("hash")]
        public string Hash { set; get; }

        public Dictionary<string, string> ToDictionary()
        {
            var d = new Dictionary<string, string>();
            d.Add("auth_date", AuthDate.ToString());
            
            if(!string.IsNullOrEmpty(FirstName)) d.Add("first_name", FirstName);
            if(!string.IsNullOrEmpty(Id)) d.Add("id", Id);
            if(!string.IsNullOrEmpty(LastName)) d.Add("last_name", LastName);
            if(!string.IsNullOrEmpty(PhotoUrl)) d.Add("photo_url", PhotoUrl);
           
            
            
            if(!string.IsNullOrEmpty(Username)) d.Add("username", Username);
            
            d.Add("hash", Hash);

            return d;
        }

        public static OAuthPlatformUserInfo ToPlatformUserInfo()
        {
            if (!TelegramLoginHelper.Instance.Valid(this.ToDictionary()))
            {
                throw new Exception("Invalid telegram login info");
            }
            
            return new OAuthPlatformUserInfo
            {
                authTime = this.AuthDate,
                firebaseUserId = "telegram-"+this.Id,
                pictureUrl = this.PhotoUrl,
                platformUserId = this.Id,
                platformUserName = this.Username,
                signInProvider = FirebaseAuthSignInProviders.Telegram,
            };
        }
    }
}