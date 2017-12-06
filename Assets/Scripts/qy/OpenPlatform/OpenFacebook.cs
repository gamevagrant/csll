using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;

namespace QY.Open
{
    public class OpenFacebook : IOpenPlatform
    {
        AccessToken _token;
        public AccessToken Token
        {
            get
            {
                if (_token == null)
                {
                    _token = new AccessToken(Facebook.Unity.AccessToken.CurrentAccessToken);
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
                    GameSetting.accessToken,
                    "11111",
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

