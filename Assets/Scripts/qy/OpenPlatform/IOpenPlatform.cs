
namespace QY.Open
{
    public interface IOpenPlatform
    {
        AccessToken token { get; }
        /// <summary>
        /// 判断是否已经登录
        /// </summary>
        bool IsLoggedIn { get; }
        /// <summary>
        /// 激活平台sdk
        /// </summary>
        void ActivateApp();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="onComplate"></param>
        void Init(System.Action onComplate);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="onComplate"></param>
        void Login(System.Action onComplate);
        /// <summary>
        /// 退出平台帐户
        /// </summary>
        void Logout();
        /// <summary>
        /// 邀请
        /// </summary>
        /// <param name="message"></param>
        /// <param name="to"></param>
        /// <param name="title"></param>
        /// <param name="callback"></param>
        void Invite(string message, string[] to, string title, System.Action<AppRequestResponse> callback);
        /// <summary>
        /// 获取可邀请好友列表
        /// </summary>
        /// <param name="callBack"></param>
        void GetInvitableFriends(System.Action<InvitableFriendsData[]> callBack);
        /// <summary>
        /// 分享链接
        /// </summary>
        /// <param name="url"></param>
        void ShareLink(string url);
    }
}

