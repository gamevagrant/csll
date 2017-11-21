using UnityEngine;
using System.Collections;

public class EventEnum {

    public const string UPDATE_ASSETS_COMPLATE = "UPDATE_ASSETS_COMPLATE";//资源更新完毕
    public const string LOGIN_START = "LOGIN_START";//开始登录
    public const string LOGIN_COMPLATE = "LOGIN_COMPLATE";//登录成功
    public const string LOADING_PROGRESS = "LOADING_PROGRESS";//加载进度
   

    public const string UPDATE_USERDATA = "UPDATE_USERDATA";//更新用户数据 金币 能量 通缉令
    public const string BUILD_COMPLATE = "BUILD_COMPLATE";//建造成功
    public const string GET_SHIELD = "GET_SHIELD";//获得盾牌
    public const string UPDATE_FRIENDS = "UPDATE_FRIENDS";//好友信息更新
    public const string SELECTED_FRIEND = "SELECTED_FRIEND";//选中好友事件
    public const string UPDATE_MAPINFO = "UPDATE_MAPINFO";//更新地图信息
}
