using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;

namespace QY.Open
{
    
    public class OpenFacebook : IOpenPlatform
    {
        private const string DEBUG_TOKEN = "EAACOsVAkLvoBAJy0VbZCpljkRrmPEimQ3fG45AHkWmI388UpZBd2J5ZCzR5tZBS1qCnSBYQHKiDgeo0iQS5UyzGbZAU6cOvynJ1yx1ypeAxITS5TnOIcNqBG1ehjTvmiWcYjI967FQtBgHhvyHdd2VFFaunbp452IyoCIrQlwM2c9bHXlZBZCpQ";//测试用facebook访问token

        AccessToken _token;
        public AccessToken token
        {
            get
            {
                if (_token == null)
                {
                    _token = AccessToken.Create(Facebook.Unity.AccessToken.CurrentAccessToken);
                }
                return _token;
            }
        }


        public bool IsLoggedIn
        {
            get
            {
                return FB.IsLoggedIn;
            }
        }

        public void ActivateApp()
        {
            FB.ActivateApp();

        }

        public void Init(System.Action onComplate)
        {
            _token = null;
            if (!FB.IsInitialized)
            {
                FB.Init(() => {
                    if (Facebook.Unity.AccessToken.CurrentAccessToken != null)
                    {
                        UnityEngine.Debug.Log("facebook登录成功 token：" + Facebook.Unity.AccessToken.CurrentAccessToken.ToString());
                        
                    }
                    onComplate();
                }, (isGameShown) => {

                    UnityEngine.Debug.Log("pause");
                });
            }
            else
            {
                FB.ActivateApp();
                onComplate();
            }

        }

        public void Login(Action onComplate)
        {
            if (!FB.IsLoggedIn)
            {
#if UNITY_EDITOR
                Facebook.Unity.AccessToken.CurrentAccessToken = new Facebook.Unity.AccessToken(
                    DEBUG_TOKEN,
                    "2085135021772785",
                    DateTime.Now,
                    new string[] { "user_friends", "email", "public_profile" },
                    DateTime.Now);
                onComplate();
#else
                FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, (result) => {
                    UnityEngine.Debug.Log(result.Error);
                    onComplate();
                });
#endif
            }
            else
            {
                onComplate();
            }

        }

        public void Logout()
        {
            FB.LogOut();
        }



        public void Invite(string message,string[] to,string title,Action<AppRequestResponse> callback)
        {
            List<object> filter = new List<object>() { "app_non_users" };
            FB.AppRequest(message,
                to,
                null, 
                null, 
                null, 
                null, 
                title, 
                (res) => {
                    UnityEngine.Debug.Log(res.RawResult);
                    AppRequestResponse data = LitJson.JsonMapper.ToObject<AppRequestResponse>(res.RawResult);
                    callback(data);
                });
        }

        public void GetInvitableFriends(Action<InvitableFriendsData[]> callBack)
        {
            FB.API("me/invitable_friends", HttpMethod.GET, (res) =>
            {
                string str = LitJson.JsonMapper.ToJson(res.ResultDictionary["data"]);
                InvitableFriendsData[] datas = LitJson.JsonMapper.ToObject<InvitableFriendsData[]>(str);
                callBack(datas);
            });
        }

        private void AppInvite()
        {
            FB.Mobile.AppInvite(new Uri("https://csll.app.link/invite"), callback: this.HandleResult);//桌面
        }

        private void ShareLink()
        {
            FB.ShareLink(new Uri("https://csll.app.link/invite"), callback: this.HandleResult);
        }

        protected void HandleResult(IResult result)
        {
            if (result == null)
            {
                return;
            }

            // Some platforms return the empty string instead of null.
            if (!string.IsNullOrEmpty(result.Error))
            {
                UnityEngine.Debug.Log("Error:" + result.Error);
            }
            else if (result.Cancelled)
            {
                UnityEngine.Debug.Log("Cancelled:" + result.RawResult);
            }
            else if (!string.IsNullOrEmpty(result.RawResult))
            {
                UnityEngine.Debug.Log("Success Response:" + result.RawResult);
            }
            else
            {
                UnityEngine.Debug.Log("Response emety:" + result.RawResult);
            }

        }
    }

}

