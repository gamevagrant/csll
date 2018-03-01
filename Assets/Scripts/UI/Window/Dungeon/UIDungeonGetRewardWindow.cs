using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDungeonGetRewardWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIDungeonGetRewardWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public GameObjectPool pool;
    DungeonInfoData info;
    protected override void StartShowWindow(object[] data)
    {
        info = data[0] as DungeonInfoData;
        RewardData[] rewards = info.self_rewards;
        pool.resetAllTarget();
        foreach(RewardData rd in rewards)
        {
            RewardItem item = pool.getIdleTarget<RewardItem>();
            item.SetData(rd);
        }
       
    }

    public void OnClickGetRewardBtn()
    {
        GameMainManager.instance.netManager.DungeonReapReward(info.create_time, (ret, res) =>
        {
            OnClickClose();
        });
    }
}
