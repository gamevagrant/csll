using UnityEngine;
using System;
using QY.Open;

public class AccountManager {

    private static AccountManager _instance;
    public static AccountManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AccountManager();
            }
            return _instance;
        }
    }

    private IOpenPlatform open;

    public AccountManager()
    {
        open = GameMainManager.instance.open;
    }

    public bool isLoginAccount
    {
        get
        {
            return open.token != null;
        }
    }


    public void Init(Action onComplate)
    {
        open.Init(() => {

            if (open.IsLoggedIn)
            {
                Debug.Log("facebook已经登录:"+open.token.tokenString);
                LoginGameServer(open.token.tokenString);
            }else
            {
                onComplate();
            }
        });
    }

    public void LoinPlatform(Action<bool> isSuccess)
    {
        Debug.Log("平台登录");
        open.Login(() =>
        {
            if (open.IsLoggedIn)
            {
                isSuccess(true);
                LoginGameServer(open.token.tokenString);
            }
            else
            {
                isSuccess(false);
            }
        });
    }

    public void LoginGuest(string name = "")
    {
        Debug.Log("游客登录");
        SimpleUserData guest = LocalDatasManager.loggedGuest;
        if (guest == null)
        {
            guest = new SimpleUserData();
            guest.uuid = Guid.NewGuid().ToString("N");
            if (string.IsNullOrEmpty(name))
            {
                guest.name = "游客_" + UnityEngine.Random.Range(0, 100000000).ToString();
            }
            else
            {
                guest.name = name;
            }

            LocalDatasManager.loggedGuest = guest;
        }
       

        LoginGameServer(guest.uuid,guest.name);
    }

    public void BindAccount(Action<bool> isSuccess)
    {
        Debug.Log("绑定帐号");
        open.Login(() =>
        {
            if (open.IsLoggedIn)
            {
                GameMainManager.instance.netManager.GetIsBind(open.token.userId, (ret,res) =>
                {
                    if(res.errcode == -1)//未绑定
                    {
                        SimpleUserData user = LocalDatasManager.loggedGuest;
                        if (user == null)
                        {
                            isSuccess(false);
                        }else
                        {
                            Binding(user);
                        }
                    }else
                    {
                        LoginGameServer(open.token.tokenString);
                    }
                });
            }
            else
            {
                isSuccess(false);
            }
        });
    }

    public void Logout()
    {
        if(open.token!=null)
        {
            open.Logout();
        }
       
    }

    private void Binding(SimpleUserData user)
    {
        Debug.Log("正在绑定facebook帐号");
        GameMainManager.instance.netManager.BindAccount(user.uuid, open.token.tokenString, (ret, res) =>
        {
            if (res.isOK)
            {
                SimpleUserData simpleUser = new SimpleUserData();
                simpleUser.name = res.data.name;
                simpleUser.level = res.data.crowns;
                LocalDatasManager.loggedAccount = simpleUser;
            }
            OnLoginComplateHandle(res);
        });
    }

    /// <summary>
    /// 平台登录
    /// </summary>
    /// <param name="accessToken"></param>
    private void LoginGameServer(string accessToken)
    {
        Debug.Log("登录游戏服务器");
        EventDispatcher.instance.DispatchEvent(new LoadingEvent("Login", 0.3f));
        GameMainManager.instance.netManager.LoginFB(accessToken, (ret, res) =>
        {
            if (res.isOK)
            {
                SimpleUserData simpleUser = new SimpleUserData();
                simpleUser.name = res.data.name;
                simpleUser.level = res.data.crowns;
                LocalDatasManager.loggedAccount = simpleUser;
            }

            OnLoginComplateHandle(res);

        });
    }

    /// <summary>
    /// 游客登录
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="name"></param>
    private void LoginGameServer(string uuid,string name)
    {
        EventDispatcher.instance.DispatchEvent(new LoadingEvent("Login", 0.3f));
        GameMainManager.instance.netManager.LoginGuest(uuid, name, (ret, res) =>
        {
            OnLoginComplateHandle(res);
           
        });
    }

    private void OnLoginComplateHandle(LoginMessage data)
    {
        if (data.isOK)
        {
            /*
            if (data.data.tutorial < 18)
            {
                JumpOverTutorial(18 - (int)data.data.tutorial, () => {

                    EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.LOGIN_COMPLATE));
                });
            }
            else
            {
                EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.LOGIN_COMPLATE));
            }
            */
            EventDispatcher.instance.DispatchEvent(new LoadingEvent("Login", 1));
            EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.LOGIN_COMPLATE));
        }
        else
        {
            Debug.Log("登录失败:" + data.errmsg);
        }
    }

    private void JumpOverTutorial(int count, System.Action callback = null)
    {
        if (count > 0)
        {
            Debug.Log("正在跳跃新手引导：" + count);
            GameMainManager.instance.netManager.TutorialComplete((ret, res) =>
            {
                if (res.isOK)
                {
                    count--;
                    JumpOverTutorial(count, callback);

                }
            });
        }
        else
        {
            if (callback != null)
            {
                Debug.Log("跳跃新手引导结束");
                callback();
            }
        }

    }
}
