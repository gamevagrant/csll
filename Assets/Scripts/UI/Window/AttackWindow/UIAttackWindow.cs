using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIAttackWindow :UIWindowBase {

    public UIAttackTopBar topBar;
    public IslandFactory island;//岛屿

    public QY.UI.Button[] btns;//选择建筑的按钮
    public GameObject btnRoot;//选择目标按钮根节点
    public Image aimIcon;//瞄准器
    public RectTransform artillery;//炮
    public RectTransform shield;//护盾
    public RectTransform boomAnimation;//爆炸序列帧
    public RectTransform particSys;//萨金币特效
    public RectTransform shell;//炮弹


    //private AttackTargetUserData randomAttackTarget;//随机攻击目标
    private SelectPlayerData selectAttackTarget;//选中攻击的目标


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
            selectAttackTarget = data;
            ChangeIsland(selectAttackTarget.islandId, selectAttackTarget.buildings);
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
        shield.gameObject.SetActive(false);

        AttackTargetUserData randomAttackTarget = data[0] as AttackTargetUserData;
        selectAttackTarget = new SelectPlayerData(randomAttackTarget);
        island.UpdateCityData(randomAttackTarget.islandId, randomAttackTarget.buildings);
        SetSelectBtn(randomAttackTarget.buildings);

        GameMainManager.instance.netManager.Vengeance((ret,res)=> {
            if(res.isOK)
            {
                topBar.SetEnemysData(res.data.enemies, randomAttackTarget);
                topBar.SetFriendsData(GameMainManager.instance.model.userData.friendInfo);
            }else
            {
                topBar.SetEnemysData(null, randomAttackTarget);
            }
        });

        QY.Guide.GuideManager.instance.state = "attack";
    }


    protected override void EnterAnimation(Action onComplete)
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(DOTween.To(() => artillery.anchoredPosition,x => artillery.anchoredPosition = x,new Vector2(0,0),0.5f).SetEase(Ease.OutBack));
        sq.AppendInterval(0.5f);
        sq.Append(DOTween.To(() => (island.transform as RectTransform).anchoredPosition, x => (island.transform as RectTransform).anchoredPosition = x, new Vector2(0, 0), 1));
        sq.InsertCallback(1, () =>
        {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_island_come);
        });
        sq.AppendCallback(() => 
        {
            
            topBar.ShowBar();
        });
        sq.Insert(1.8f,DOTween.To(() => island.transform.localScale, x => island.transform.localScale = x, Vector3.one, 1).SetEase(Ease.OutBack));
        sq.onComplete += () => {
            btnRoot.SetActive(true);
            onComplete();
        };


    }

    protected override void ExitAnimation(Action onComplete)
    {
        shield.gameObject.SetActive(false);
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_target_leave);
        Sequence sq = DOTween.Sequence();

        sq.Append(DOTween.To(() => island.transform.localScale, x => island.transform.localScale = x,Vector3.zero, 2).SetEase(Ease.OutCubic));

        sq.onComplete += () => {
            onComplete();
        };
    }

    public void OnClickSelectTargetBtn(int index)
    {
        btnRoot.SetActive(false);
        topBar.HideBar();
        GameMainManager.instance.netManager.Attack(selectAttackTarget.uid, index - 1, (ret, res) =>
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
        //Dictionary<UISettings.UIWindowID, object> data = new Dictionary<UISettings.UIWindowID, object>();
        //data.Add(UISettings.UIWindowID.UITopBarWindow, null);
        //data.Add(UISettings.UIWindowID.UIWheelWindow, null);
        //data.Add(UISettings.UIWindowID.UISideBarWindow, null);
        particSys.gameObject.SetActive(false);
        GameMainManager.instance.uiManager.ChangeState(new MainState(2));
    }

    private void Attack(int index, AttackData data)
    {
        RectTransform target = island.GetBuildTransform(index);
        aimIcon.transform.position = target.position;
        aimIcon.gameObject.SetActive(true);
        aimIcon.color = new Color(aimIcon.color.r, aimIcon.color.g, aimIcon.color.b, 0);
        boomAnimation.position = target.position;
        (boomAnimation as RectTransform).anchoredPosition += new Vector2(0,80);
        particSys.anchoredPosition3D = target.anchoredPosition3D + new Vector3(0, 0, -100);
        shell.position = artillery.position;
        shell.localScale = new Vector3(1.3f,1.3f,1);

        float angle = Vector2.Angle(new Vector2(0, 1), target.position - artillery.position);
        if ((target.position - artillery.position).x > 0)
        {
            angle = angle * -1;
        }

        string tips = "";
        if (!data.isShielded && !data.isMiniShielded)
        {

            if(data.attackTarget.buildings[index - 1].status == 2)
            {
                tips = string.Format("恭喜您，您成功摧毁了<#1995BCFF>{0}</color>的{2}，获得了<#D34727FF>{1}</color>金币", data.attackTarget.name, GameUtils.GetShortMoneyStr(data.reward), GameMainManager.instance.configManager.islandConfig.GetBuildingName(index-1));
            }
            else 
            {
                tips = string.Format("恭喜您，您成功损坏了<#1995BCFF>{0}</color>的{2}，获得了<#D34727FF>{1}</color>金币", data.attackTarget.name, GameUtils.GetShortMoneyStr(data.reward), GameMainManager.instance.configManager.islandConfig.GetBuildingName(index-1));
            }
            
           

            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_aim_target);
            Sequence sq = DOTween.Sequence();
            sq.Append(artillery.DORotate(new Vector3(0, 0, angle), 0.5f));
            sq.Insert(0,aimIcon.DOFade(1,1));
            sq.Insert(0, aimIcon.rectTransform.DOShakeAnchorPos(2, 30));
            sq.AppendCallback(() => {
                aimIcon.gameObject.SetActive(false);
                shell.gameObject.SetActive(true);
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_fire);
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_bomb_fly);
            });
            sq.Append(shell.transform.DOMove(target.position, 1).SetEase(Ease.OutBack));
            sq.Insert(2,shell.transform.DOScale(new Vector3(0.5f,0.5f,1), 1).SetEase(Ease.OutQuad));
            sq.AppendCallback(() =>
            {
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_boob_explode);
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_gold_med);
                shell.gameObject.SetActive(false);
                boomAnimation.gameObject.SetActive(true);
                particSys.gameObject.SetActive(true);
                island.UpdateBuildingData(index, data.attackTarget.buildings[index - 1]);
                EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money, 0));
            });
            sq.Insert(4.5f,DOTween.To(() => artillery.anchoredPosition, x => artillery.anchoredPosition = x, new Vector2(0, -250), 0.5f));
            sq.InsertCallback(4.5f,()=> {
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_in);
                //GameMainManager.instance.uiManager.OpenPopupModalBox(tips, "", OnClickOkBtn);
                Alert.ShowPopupBox(tips, OnClickOkBtn);
            });

            sq.AppendCallback(() =>
            {
                boomAnimation.gameObject.SetActive(false);
                particSys.gameObject.SetActive(false);
            });

        }
        else
        {

            tips = string.Format("您的攻击被<#1995BCFF>{0}</color>的盾牌阻挡了，获得了<#D34727FF>{1}</color>金币", data.attackTarget.name, GameUtils.GetShortMoneyStr(data.reward) );
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_aim_target);
            Sequence sq = DOTween.Sequence();
            sq.Append(artillery.DORotate(new Vector3(0, 0, angle), 0.5f));
            sq.Insert(0,aimIcon.DOFade(1, 1));
            sq.Insert(0, aimIcon.rectTransform.DOShakeAnchorPos(2, 30));
            sq.AppendCallback(() => {
                aimIcon.gameObject.SetActive(false);
                
                shell.gameObject.SetActive(true);
                
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_fire);
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_bomb_fly);
            });
            sq.Append(shell.transform.DOMove(target.position, 1).SetEase(Ease.OutBack));
            sq.Insert(2, shell.transform.DOScale(new Vector3(0.5f, 0.5f, 1), 1).SetEase(Ease.OutQuad));
            sq.AppendCallback(() =>
            {
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_hit_sheild);
                shield.gameObject.SetActive(true);
                particSys.gameObject.SetActive(true);
                EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money, 0));
            });
            Debug.Log(target.position);
            sq.Append(shell.DOAnchorPos(new Vector2(target.position.x>0?250:-350,-300),1).SetRelative(true));
            sq.AppendCallback(()=> {
                shell.gameObject.SetActive(false);
                particSys.gameObject.SetActive(false);
            });
            sq.Insert(4.5f,DOTween.To(() => artillery.anchoredPosition, x => artillery.anchoredPosition = x, new Vector2(0, -250), 0.5f));
            sq.InsertCallback(5, () => {
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_in);
                //GameMainManager.instance.uiManager.OpenPopupModalBox(tips, "", OnClickOkBtn);
                Alert.ShowPopupBox(tips, OnClickOkBtn);
            });

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
        sq.InsertCallback(1,() =>
        {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_target_change);
        });
        sq.AppendCallback(() =>
        {
            island.UpdateCityData(islandID, buildings);
            SetSelectBtn( buildings);
            (island.transform as RectTransform).anchoredPosition = new Vector2(600,0);
        });
        sq.AppendInterval(0.5f);
        sq.InsertCallback(1.5f,()=> {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_target_come);
        });
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
