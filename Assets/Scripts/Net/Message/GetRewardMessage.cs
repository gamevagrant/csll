using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRewardMessage : NetMessage {

    public GetRewardData data;
}

public class GetRewardData
{
    public MailData[] user_mail;
    public RewardData[] user_rewards;
}
