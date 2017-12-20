using UnityEngine;
using System.Collections;

public class EventEnum {

    public const string UPDATE_ASSETS_COMPLATE = "UPDATE_ASSETS_COMPLATE";//资源更新完毕
    public const string LOGIN_COMPLATE = "LOGIN_COMPLATE";//登录成功
    public const string LOADING_PROGRESS = "LOADING_PROGRESS";//加载进度
    public const string REQUEST_ERROR = "REQUEST_ERROR";//请求失败

    public const string UPDATE_BASE_DATA = "UPDATE_BASE_DATA";//更新基础用户数据  金币 能量 通缉令
    public const string BUILD_COMPLATE = "BUILD_COMPLATE";//建造成功
    public const string GET_SHIELD = "GET_SHIELD";//获得盾牌
    public const string GET_STAR = "GET_STAR";//获得星星
    public const string UPDATE_FRIENDS = "UPDATE_FRIENDS";//好友信息更新
    public const string SELECTED_FRIEND = "SELECTED_FRIEND";//选中好友事件
    public const string UPDATE_MAPINFO = "UPDATE_MAPINFO";//更新地图信息
    public const string UPDATE_BUILDING = "UPDATE_BUILDING";//更新自己建筑的信息
}
