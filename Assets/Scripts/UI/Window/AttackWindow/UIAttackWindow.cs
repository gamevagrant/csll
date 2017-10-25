using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIAttackWindow :UIWindowBase {

    public UIAttackTopBar topBar;
    public IslandFactory island;//岛屿

    public Button[] btns;//选择建筑的按钮
    public GameObject btnRoot;//选择目标按钮根节点
    public Image aimIcon;//瞄准器
    public RectTransform artillery;//炮
    public RectTransform shield;//护盾
    public RectTransform boomAnimation;//爆炸序列帧
    public RectTransform particSys;//萨金币特效
    public RectTransform shell;//炮弹
    public RectTransform bottomBar;//底部对话框
    public Text tips;//提示文字

    private AttackTargetUserData attackTargetUser;//随机攻击目标


    public override UIWindowData windowData
    {
        get
        {
            if(_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIAttackWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    private void Start()
    {
        topBar.onSelectTarget += (data) => {

            ChangeIsland(data.islandId, data.buildings);
        };
    }

    protected override void StartShowWindow(object[] data)
    {
        artillery.anchoredPosition = new Vector2(0, -250);
        artillery.localRotation = Quaternion.identity;
        (island.transform as RectTransform).anchoredPosition = new Vector2(600, 0);
        island.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
        btnRoot.SetActive(false);
        particSys.gameObject.SetActive(false);
        boomAnimation.gameObject.SetActive(false);
        aimIcon.gameObject.SetActive(false);
        shell.gameObject.SetActive(false);
        bottomBar.gameObject.SetActive(false);
        bottomBar.anchoredPosition = new Vector2(0, -350);
        shield.gameObject.SetActive(false);

        attackTargetUser = data[0] as AttackTargetUserData;
        island.UpdateCityData(attackTargetUser.islandId, attackTargetUser.buildings);
        SetSelectBtn(attackTargetUser.buildings);

        GameMainManager.instance.netManager.Vengeance((ret,res)=> {
            if(res.isOK)
            {
                topBar.SetEnemysData(res.data.enemies, attackTargetUser);
                topBar.SetFriendsData(GameMainManager.instance.model.userData.friendInfo);
            }
        });

    }


    protected override void EnterAnimation(Action onComplete)
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(DOTween.To(() => artillery.anchoredPosition,x => artillery.anchoredPosition = x,new Vector2(0,0),0.5f).SetEase(Ease.OutBack));
        sq.Append(DOTween.To(() => (island.transform as RectTransform).anchoredPosition, x => (island.transform as RectTransform).anchoredPosition = x, new Vector2(0, 0), 2));
        sq.AppendCallback(() => { topBar.ShowBar(); });
        sq.Insert(2.5f,DOTween.To(() => island.transform.localScale, x => island.transform.localScale = x, Vector3.one, 1).SetEase(Ease.OutBack));
        sq.onComplete += () => {
            btnRoot.SetActive(true);
            onComplete();
        };


    }

    protected override void ExitAnimation(Action onComplete)
    {
        shield.gameObject.SetActive(false);
        Sequence sq = DOTween.Sequence();
        sq.Append(DOTween.To(() => artillery.anchoredPosition, x => artillery.anchoredPosition = x, new Vector2(0, -250), 0.5f).SetEase(Ease.OutBack));
        sq.Insert(0,DOTween.To(() => (island.transform as RectTransform).anchoredPosition, x => (island.transform as RectTransform).anchoredPosition = x, new Vector2(600, 0), 1).SetEase(Ease.OutQuint));
        sq.InsertCallback(0,()=> { topBar.HideBar(); });
        sq.Insert(0,DOTween.To(() => bottomBar.anchoredPosition, x => bottomBar.anchoredPosition = x, new Vector2(0, -350), 1).SetEase(Ease.OutBack));
        sq.onComplete += () => {
            onComplete();
        };
    }

    public void OnClickSelectTargetBtn(int index)
    {
        btnRoot.SetActive(false);

        GameMainManager.instance.netManager.Attack(attackTargetUser.uid, index, (ret, res) =>
        {
            if(res.isOK)
            {
                AttackData attackData = res.data;
                Attack(index, attackData);
            }
        });
    }

    public void OnClickOkBtn()
    {
        Dictionary<UISettings.UIWindowID, object> data = new Dictionary<UISettings.UIWindowID, object>();
        data.Add(UISettings.UIWindowID.UIWheelWindow, null);
        GameMainManager.instance.uiManager.ChangeState(new UIStateChangeBase(data));
    }

    private void Attack(int index, AttackData data)
    {
        Transform target = island.getBuildTransform(index);
        aimIcon.transform.position = target.position;
        aimIcon.gameObject.SetActive(true);
        aimIcon.color = new Color(aimIcon.color.r, aimIcon.color.g, aimIcon.color.b, 0);
        boomAnimation.position = target.position;
        particSys.position = target.position + new Vector3(0, 0, -100);
        shell.position = artillery.position;
        bottomBar.gameObject.SetActive(true);

        float angle = Vector2.Angle(new Vector2(0, 1), target.position - artillery.position);
        if ((target.position - artillery.position).x > 0)
        {
            angle = angle * -1;
        }

        if (!data.isShielded && !data.isMiniShielded)
        {
            tips.text = string.Format("您成功损坏了<color=red>{0}</color>的建筑，获得了<color=red>{1}</color>金币", data.attackTarget.name,GameUtils.GetCurrencyString(data.reward) );

            Sequence sq = DOTween.Sequence();
            sq.Append(artillery.DORotate(new Vector3(0, 0, angle), 0.5f));
            sq.Insert(0,aimIcon.DOFade(1,1));
            sq.Insert(0, aimIcon.rectTransform.DOShakeScale(2));
            sq.AppendCallback(() => {
                aimIcon.gameObject.SetActive(false);
                shell.gameObject.SetActive(true);
            });
            sq.Append(shell.transform.DOMove(target.position, 1).SetEase(Ease.OutBack));
            sq.AppendCallback(() =>
            {
                shell.gameObject.SetActive(false);
                boomAnimation.gameObject.SetActive(true);
                particSys.gameObject.SetActive(true);
                island.UpdateBuildingData(attackTargetUser.islandId, index, data.attackTarget.buildings[index - 1]);
            });
            sq.Append(DOTween.To(() => artillery.anchoredPosition, x => artillery.anchoredPosition = x, new Vector2(0, -250), 0.5f));
            sq.Insert(3.2f,bottomBar.DOAnchorPos(Vector2.zero,0.5f).SetEase(Ease.OutQuint));
            sq.AppendCallback(() =>
            {
                boomAnimation.gameObject.SetActive(false);
                particSys.gameObject.SetActive(false);
            });

        }
        else
        {
            tips.text = string.Format("您的攻击被<color=red>{0}</color>的盾牌阻挡了，获得了<color=red>{1}</color>金币", data.attackTarget.name, GameUtils.GetCurrencyString(data.reward) );

            Sequence sq = DOTween.Sequence();
            sq.Append(artillery.DORotate(new Vector3(0, 0, angle), 0.5f));
            sq.Insert(0,aimIcon.DOFade(1, 1));
            sq.Insert(0, aimIcon.rectTransform.DOShakeAnchorPos(2, 60));
            sq.AppendCallback(() => {
                aimIcon.gameObject.SetActive(false);
                shell.gameObject.SetActive(true);
            });
            sq.Append(shell.transform.DOMove(target.position, 1).SetEase(Ease.OutBack));
            sq.AppendCallback(() =>
            {
                shield.gameObject.SetActive(true);
                particSys.gameObject.SetActive(true);
            });
            sq.Append(shell.DOAnchorPos(new Vector2(-150,-200),1).SetRelative(true));
            sq.AppendCallback(()=> {
                shell.gameObject.SetActive(false);
                particSys.gameObject.SetActive(false);
            });

            sq.Append(DOTween.To(() => artillery.anchoredPosition, x => artillery.anchoredPosition = x, new Vector2(0, -250), 0.5f));
            sq.Insert(4.2f, bottomBar.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutQuint));

            sq.onComplete += () =>
            {
                shell.gameObject.SetActive(false);
            };
        }
    }

    private void ChangeIsland(int islandID,BuildingData[] buildings)
    {
       
        GameMainManager.instance.uiManager.DisableOperation();
        Sequence sq = DOTween.Sequence();
        sq.AppendInterval(0.5f);
        sq.AppendCallback(()=> {
            btnRoot.SetActive(false);
        });
        sq.Append((island.transform as RectTransform).DOAnchorPos(new Vector2(-600, 0), 0.8f).SetEase(Ease.InBack));
        sq.AppendCallback(() =>
        {
            island.UpdateCityData(islandID, buildings);
            SetSelectBtn( buildings);
            (island.transform as RectTransform).anchoredPosition = new Vector2(600,0);
        });
        sq.AppendInterval(0.1f);
        sq.Append((island.transform as RectTransform).DOAnchorPos(Vector2.zero, 0.8f).SetEase(Ease.OutBack));
        sq.OnComplete(()=> {
            btnRoot.SetActive(true);
            GameMainManager.instance.uiManager.EnableOperation();
        });

    }

    private void SetSelectBtn(BuildingData[] buildings)
    {
        for(int i = 0;i<buildings.Length;i++)
        {
            BuildingData build = buildings[i];
            if(build.level==0 || build.status == 2)
            {
                btns[i].gameObject.SetActive(false);
            }else
            {
                btns[i].gameObject.SetActive(true);
            }
        }
    }
}
