using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBindingRewardMessage : NetMessage {

    public GetBindingRewardData data;

    public class GetBindingRewardData
    {
        //{"errcode":0,"errmsg":"","data":{"energy":185,"extra":{"energy":50,"money":500000},"money":558768,"rewarded":true,"uid":1125}}
        /// <summary>
        /// true，可以领取 false，不可领
        /// </summary>
        public bool rewarded;
        public long money;
        public int energy;
        public int uid;
    }
}
