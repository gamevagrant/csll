using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UINoticeWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UINoticeWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.NormalNavigation;
            }

            return _windowData;
        }
    }

    public RectTransform topBar;
    public RectTransform panel;
    public GameObjectPool pool;
    public RawImage topImage;

    private AnnouncementData data;

    private void Awake()
    {
        topBar.anchoredPosition = new Vector2(0, 110);
        panel.anchoredPosition = new Vector2(0, 900);
    }

    protected override void StartShowWindow(object[] data)
    {
        this.data = data[0] as AnnouncementData;

        pool.resetAllTarget();

        if (!string.IsNullOrEmpty(this.data.img_url))
        {
            AssetLoadManager.Instance.LoadAsset<Texture2D>(this.data.img_url, (tex) =>
            {
                topImage.texture = tex;
            });
        }
       
        if(this.data.sections!=null)
        {
            for (int i = 0; i < this.data.sections.Length; i++)
            {
                AnnouncementContentData content = this.data.sections[i];
                UINoticeItem item = pool.getIdleTarget<UINoticeItem>();
                item.SetData(content);
            }
        }
       

    }

    protected override void EnterAnimation(Action onComplete)
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(panel.DOAnchorPos(new Vector2(0, -55), 1f).SetEase(Ease.OutCubic));
        sq.Insert(0.2f, topBar.DOAnchorPos(new Vector2(0, 0), 0.5f).SetEase(Ease.OutCubic)).OnComplete(()=> {
            onComplete();
        });
    }

    protected override void ExitAnimation(Action onComplete)
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(panel.DOAnchorPos(new Vector2(0, 900), 1).SetEase(Ease.OutCubic));
        sq.Insert(0.1f,topBar.DOAnchorPos(new Vector2(0, 110), 0.5f).SetEase(Ease.InCubic)).OnComplete(() => {
            onComplete();
        });
       
    }
}
