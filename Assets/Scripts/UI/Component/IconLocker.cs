using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IconLocker : MonoBehaviour {

    public enum LockIcon
    {
        NULL,
        DailyTask,
        Map,
        Piece,
        Achievement
    }
    [SerializeField]
    private Image lockImg;
    [SerializeField]
    private LockIcon type;

    public bool isLock
    {
        get
        {
            return lockImg.enabled;
        }
        set
        {
            lockImg.enabled = value;

        }
    }

    private void Awake()
    {
        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_BASE_DATA, OnUpdateHandle);
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_BASE_DATA, OnUpdateHandle);
    }

    public void OnEnable()
    {
        Refresh();
    }

    private void OnUpdateHandle(BaseEvent e)
    {
        UpdateBaseDataEvent evt = e as UpdateBaseDataEvent;
        if(evt.updateType == UpdateBaseDataEvent.UpdateType.island)
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        bool isLock = false;
        switch(type)
        {
            case LockIcon.DailyTask:
                isLock = GameMainManager.instance.model.userData.islandId < 2;
                break;
            case LockIcon.Map:
                isLock = GameMainManager.instance.model.userData.islandId < 3;
                break;
            case LockIcon.Piece:
                isLock = GameMainManager.instance.model.userData.islandId < 4;
                break;
            case LockIcon.Achievement:
                isLock = GameMainManager.instance.model.userData.islandId < 5;
                break;
        }
        this.isLock = isLock;
    }
}
