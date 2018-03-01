using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDungeonLottoWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIDungeonLottoWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }
    public Transform cardsParent;
    public GameObject setBtn;
    public GameObject returnBtn;
    public GameObject okBtn;
    public Transform cardCell;

    private UIDungeonLottoCard[] cards;
    private UIDungeonLottoCard selectedCard;
    private int createTime;
    private long owner;

    private void Awake()
    {
        cards = cardsParent.GetComponentsInChildren<UIDungeonLottoCard>();
        foreach (UIDungeonLottoCard card in cards)
        {
            card.onSelected = OnSelectedCard;
            card.onUnSelected = OnUnSelectedCard;
        }
        SetCellState(false);
    }

    protected override void StartShowWindow(object[] data)
    {
        if(data!=null && data.Length>0)
        {
            owner = (long)data[0];
            createTime = (int)data[1];
        }
        
        SetCellState(false);
        setBtn.GetComponent<QY.UI.Button>().interactable = false;
    }

    private void SetCellState(bool selected)
    {
        setBtn.SetActive(!selected);
        returnBtn.SetActive(selected);
        okBtn.SetActive(selected);
    }

    private void OnSelectedCard(UIDungeonLottoCard selectedCard)
    {
        this.selectedCard = selectedCard;
        foreach (UIDungeonLottoCard card in cards)
        {
            if(card!=selectedCard)
            {
                card.UnSelected();
            }
        }
        setBtn.GetComponent<QY.UI.Button>().interactable = true;
    }
    private void OnUnSelectedCard()
    {
        this.selectedCard = null;
        setBtn.GetComponent<QY.UI.Button>().interactable = false;
    }
    public void OnClickSetBtn()
    {
        if(selectedCard!=null)
        {
            selectedCard.MoveTarget(cardCell);
            SetCellState(true);
        }
       
    }

    public void OnClickReturnBtn()
    {
        selectedCard.MoveReturn();
        SetCellState(false);
        selectedCard = null;
        setBtn.GetComponent<QY.UI.Button>().interactable = false;
    }

    public void OnClickOkBtn()
    {
        GameMainManager.instance.netManager.DungeonLottoCard(owner,createTime, (ret, res) =>
        {
            if(res.isOK)
            {
                OnClickClose();
                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIDungeonGetCardWindow, res.data.card_type);
            }
        });
    }

    public void OnClickHelpBtn()
    {
        OnClickClose();
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIDungeonLottoHelpWindow);
    }
}
