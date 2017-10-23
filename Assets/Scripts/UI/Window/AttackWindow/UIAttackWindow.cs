using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIAttackWindow :UIWindowBase {

    public RectTransform topBar;
    public RectTransform artillery;
    public Image aimIcon;
    public IslandFactory island;
    public RectTransform shield;
    public RectTransform boomAnimation;
    public RectTransform particSys;
    public GameObject btnRoot;
    public RectTransform shell;
    public RectTransform bottomBar;
    public Text tips;

    private AttackTargetUserData attackTargetUser;

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

    protected override void StartShowWindow(object[] data)
    {
        topBar.anchoredPosition = new Vector2(0, 150);
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

    }

    protected override void EnterAnimation(Action onComplete)
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(DOTween.To(() => artillery.anchoredPosition,x => artillery.anchoredPosition = x,new Vector2(0,0),0.5f).SetEase(Ease.OutBack));
        sq.Append(DOTween.To(() => (island.transform as RectTransform).anchoredPosition, x => (island.transform as RectTransform).anchoredPosition = x, new Vector2(0, 0), 2));
        sq.Append(DOTween.To(() => topBar.anchoredPosition, x => topBar.anchoredPosition = x, new Vector2(0, 0), 1).SetEase(Ease.OutQuint));
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
        sq.Insert(0,DOTween.To(() => topBar.anchoredPosition, x => topBar.anchoredPosition = x, new Vector2(0, 150), 1).SetEase(Ease.OutBack));
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
            tips.text = string.Format("您成功损坏了<color=red>{0}</color>的建筑，获得了<color=red>{1}</color>金币", data.attackTarget.name,data.reward);

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
            tips.text = string.Format("您的攻击被<color=red>{0}</color>的盾牌阻挡了，获得了<color=red>{1}</color>金币", data.attackTarget.name, data.reward);

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
}
