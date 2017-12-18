using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AccessToken {

    //public static AccessToken CurrentAccessToken;
    public string tokenString;
    public DateTime expirationTime;
    public IEnumerable<string> permissions;
    public string userId;
    public DateTime? lastRefresh;

    public static AccessToken Create(Facebook.Unity.AccessToken token)
    {
        if(token == null)
        {
            return null;
        }else
        {
            AccessToken t = new AccessToken();
            t.tokenString = token.TokenString;
            t.expirationTime = token.ExpirationTime;
            t.permissions = token.Permissions;
            t.userId = token.UserId;
            t.lastRefresh = token.LastRefresh;
            return t;
        }
    }
}
