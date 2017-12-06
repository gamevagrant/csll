using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AccessToken : MonoBehaviour {

    //public static AccessToken CurrentAccessToken;
    public string tokenString;
    public DateTime expirationTime;
    public IEnumerable<string> permissions;
    public string userId;
    public DateTime? lastRefresh;

    public AccessToken(Facebook.Unity.AccessToken token)
    {
        this.tokenString = token.TokenString;
        this.expirationTime = token.ExpirationTime;
        this.permissions = token.Permissions;
        this.userId = token.UserId;
        this.lastRefresh = token.LastRefresh;
    }
}
