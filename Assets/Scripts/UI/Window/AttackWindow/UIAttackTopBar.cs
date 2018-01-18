using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using UnityEngine.UI;
using QY.UI;
public class UIAttackTopBar : MonoBehaviour {

    public HeadIcon head;

    public RectTransform topBar;
    public RectTransform panel;
    public BaseScrollView scrollView;
    public Toggle enemyToggle;
    public Toggle friendToggle;

    public event System.Action<SelectPlayerData> onSelectTarget;

    private List<SelectPlayerData> enemys;
    private List<SelectPlayerData> friends;
    private SelectPlayerData randomTarget;
    private SelectPlayerData _selectedTarget;
    private SelectPlayerData selectedTarget
    {
        get
        {
            return _selectedTarget;
        }
        set
        {
            _selectedTarget = value;
            head.setData(_selectedTarget.name, _selectedTarget.headImg, _selectedTarget.crowns, _selectedTarget.isVip);
        }
    }
    

    private bool panelIsOpend
    {
        get
        {
            return panel.anchoredPosition.y < 100;
        }
    }

    private void Awake()
    {
        panel.anchoredPosition = new Vector2(0,1000);
        panel.gameObject.SetActive(false);

        topBar.anchoredPosition = new Vector2(0,150);
        topBar.gameObject.SetActive(false);

        enemyToggle.onValueChanged.AddListener(OnChangeEnemyToggle);
        friendToggle.onValueChanged.AddListener(OnChangeFriendToggle);

    }

    private void OnDestroy()
    {
        enemyToggle.onValueChanged.RemoveListener(OnChangeEnemyToggle);
        friendToggle.onValueChanged.RemoveListener(OnChangeFriendToggle);
    }

    public void SetEnemysData(BadGuyData[] enemys,AttackTargetUserData randomTarget)
    {
        this.randomTarget = new SelectPlayerData(randomTarget);

        this.enemys = new List<SelectPlayerData>();
        //this.enemys.Add(this.randomTarget);
        this.selectedTarget = this.randomTarget;
        
        if (enemys!=null)
        {
            for(int i = 0;i< enemys.Length;i++)
            {
                BadGuyData badData = enemys[i];
                this.enemys.Add(new SelectPlayerData(badData,i+1));
            }

        }

    }

    public void SetFriendsData(FriendData[] friends)
    {
        if(friends!=null)
        {
            this.friends = new List<SelectPlayerData>();
            foreach (FriendData friend in friends)
            {
                this.friends.Add(new SelectPlayerData(friend));
            }
        }
       
    }

    public void OnChangeEnemyToggle(bool value)
    {
        if (value)
        {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.button_click);
            ShowPanel(enemys);
        }else
        {
            HidePanel();
        }
    }
    public void OnChangeFriendToggle(bool value)
    {
        if (value)
        {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.button_click);
            ShowPanel(friends);
        }
        else
        {
            HidePanel();
        }

    }

    public void OnClickClosePanelBtn()
    {
        HidePanel();

        ReSetToggle();
    }

    public void OnClickAttackBtn(UIEnemyPanelItem data)
    {
        selectedTarget = data.selectData;
        if(onSelectTarget != null)
        {
            onSelectTarget(selectedTarget);
        }
       
        HidePanel();

        ReSetToggle();
    }


    public void ShowBar()
    {
        topBar.gameObject.SetActive(true);
        topBar.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic);
        
    }

    public void HideBar()
    {
        topBar.DOAnchorPos(new Vector2(0,150), 0.5f).SetEase(Ease.OutCubic).onComplete+=()=> {
            topBar.gameObject.SetActive(false);
        };
    }

    private void ShowPanel(List<SelectPlayerData> list)
    {
       
        Sequence sq = DOTween.Sequence();
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_out);
        if(panelIsOpend)
        {
            sq.AppendInterval(0.5f);
        }
        sq.AppendCallback(() =>
        {
            DOTween.Kill(panel);
            panel.gameObject.SetActive(true);
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_in);
            SendSelectTargetData(list);
        });
        sq.Append(panel.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutBack));
        /*
        if (panelIsOpend)
        {
            Sequence sq = DOTween.Sequence();
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_out);
            sq.Append(panel.DOAnchorPos(new Vector2(0, 1000), 0.8f).SetEase(Ease.OutBack));
            sq.AppendCallback(() =>
            {
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_in);
                SendSelectTargetData(list);
            });
            sq.Append(panel.DOAnchorPos(Vector2.zero, 0.8f).SetEase(Ease.OutBack));


        }
        else
        {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_in);
            panel.gameObject.SetActive(true);
            SendSelectTargetData(list);
            panel.DOAnchorPos(Vector2.zero, 0.8f).SetEase(Ease.OutBack);
        }
        */
    }

    private void ReSetToggle()
    {
        enemyToggle.isOn = false;
        //enemyToggle.enabled = false;
        //enemyToggle.enabled = true;

        friendToggle.isOn = false;
        //friendToggle.enabled = false;
        //friendToggle.enabled = true;
    }

    private void HidePanel()
    {
        if (panelIsOpend)
        {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_out);
            panel.DOAnchorPos(new Vector2(0, 1000), 0.5f).SetEase(Ease.OutBack).OnComplete(() => {
                //panel.gameObject.SetActive(false);
            });
        }
       
    }

    private void SendSelectTargetData(List<SelectPlayerData> list)
    {
        if(list!=null && list.Count>0)
        {
            foreach (SelectPlayerData data in list)
            {
                data.isRandomUser = data.uid == randomTarget.uid ? true : false;
                data.isSelected = data.uid == selectedTarget.uid ? true : false;
            }
            scrollView.SetData (list);
        }else
        {
            scrollView.SetData (new List<SelectPlayerData>());
        }
       
    }


}
