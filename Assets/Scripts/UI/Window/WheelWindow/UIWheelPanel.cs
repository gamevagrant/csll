using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIWheelPanel : MonoBehaviour {

    public TextMeshProUGUI[] names;//文字显示位置
    public Image[] images;//图片显示位置
    public Sprite[] sprites;//sprite集合
    
    public Image reflective;//反光
    public Transform wheel;
    public ParticleSystem goldEffect;
    public GameObject shield;
    public GameObject energy;
    public GameObject lightBG;
    public GameObject beaver;
    public GetStarGoldEffect getStarEffect;
    //public GameObjectPool starPool;//星星倍的星星
    //public TextMeshProUGUI starGoldText;

    public RawImage stealHead;
    public TextMeshProUGUI stealNameLabel;
    public TextMeshProUGUI stealMoneyLabel;

    public TextMeshProUGUI countDownLabel;
    public TextMeshProUGUI addEnergyCountLabel;
    public TextMeshProUGUI energyLabel;
    public TextMeshProUGUI surplusEnergyLabel;
    public Slider energyProgressSlider;
    public AudioClip[] audioClips;
    public UGUISpriteAnimation energyWaveAnimation;
    

    [SerializeField]
    private RectTransform rollBtn;
    [SerializeField]
    private RectTransform switchBtn;
    [SerializeField]
    private RectTransform panel;

    private bool isWorking;
    private bool isHoldOn = false;//是否在自动roll状态

    private Vector2 rollBtnOriginalValue;//roll点按钮位置原始值 下同
    private Vector2 switchOriginalValue;
    private Vector2 panelLocalOriginalValue;

    private TargetData stealTarget;
    private int shieldCount;//护盾个数

    private UserData user;

    private int _energyValue;
    private int energyValue
    {
        get
        {
            return _energyValue;
        }
        set
        {
            if(_energyValue!=value)
            {
                energyWaveAnimation.Play();
            }
            _energyValue = value;
            energyLabel.text = string.Format("{0}/{1}",Mathf.Min(value,user.maxEnergy), user.maxEnergy);
            surplusEnergyLabel.text = value>user.maxEnergy?(value - user.maxEnergy).ToString():"";
            energyProgressSlider.value = value / (float)user.maxEnergy;
            addEnergyCountLabel.text = value < user.maxEnergy ? "+ " + user.recoverEnergy.ToString() : "";
            
        }
    }
    // Use this for initialization
    void Awake ()
    {

        rollBtnOriginalValue = rollBtn.anchoredPosition;
        switchOriginalValue = switchBtn.anchoredPosition;
        panelLocalOriginalValue = panel.anchoredPosition;

        reflective.color = new Color(1, 1, 1, 0);

        rollBtn.anchoredPosition = new Vector2(rollBtn.anchoredPosition.x, -300);
        switchBtn.anchoredPosition = new Vector2(200, switchBtn.anchoredPosition.y);
        panel.anchoredPosition = new Vector2(-600, panel.anchoredPosition.y);

        getStarEffect.gameObject.SetActive(false);

        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_BASE_DATA, OnUpdateBaseData);
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_BASE_DATA, OnUpdateBaseData);
    }


    private void Start()
    {
        user = GameMainManager.instance.model.userData;
        energyValue = user.energy;
        
        isWorking = false;
    }

    private void Update()
    {
        if(energyValue < user.maxEnergy)
        {
            countDownLabel.text = GameUtils.TimestampToDateTime(user.timeToRecover - (long)(Time.time - user.timeTag)).ToString("mm:ss");
        }else
        {
            countDownLabel.text = "";
        }
        
    }

    public void EnterToBuildPanelState()
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_view_switch_in);


        DOTween.To(() => rollBtn.anchoredPosition, p => rollBtn.anchoredPosition = p, new Vector2(rollBtn.anchoredPosition.x, -300), 1).SetEase(Ease.InQuint);
        DOTween.To(() => rollBtn.localScale, x => rollBtn.localScale = x, new Vector3(1.5f,1.5f,1), 1).SetId(1);
        DOTween.To(() => switchBtn.anchoredPosition, p => switchBtn.anchoredPosition = p, new Vector2(200, switchBtn.anchoredPosition.y), 1);
        DOTween.To(() => panel.anchoredPosition, x => panel.anchoredPosition = x, new Vector2(-800,1000), 1);
        DOTween.To(() => panel.localScale, x => panel.localScale = x, new Vector3(2f, 2f, 1), 1).onComplete = ()=> {

            rollBtn.gameObject.SetActive(false);
            switchBtn.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
        };
    }

    public void EnterToWheelPanelState()
    {

        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_view_switch_out);
        GameMainManager.instance.uiManager.DisableOperation();
        rollBtn.gameObject.SetActive(true);
        switchBtn.gameObject.SetActive(true);
        panel.gameObject.SetActive(true);

        DOTween.To(() => rollBtn.anchoredPosition, p => rollBtn.anchoredPosition = p, rollBtnOriginalValue, 1).SetEase(Ease.OutQuint);
        DOTween.To(() => rollBtn.localScale, x => rollBtn.localScale = x, Vector3.one, 1);
        DOTween.To(() => switchBtn.anchoredPosition, p => switchBtn.anchoredPosition = p, switchOriginalValue, 1);
        DOTween.To(() => panel.anchoredPosition, x => panel.anchoredPosition = x, panelLocalOriginalValue, 1);
        DOTween.To(() => panel.localScale, x => panel.localScale = x, Vector3.one, 1).onComplete = () => {
            QY.Guide.GuideManager.instance.state = "wheel";
            GameMainManager.instance.uiManager.EnableOperation();
            if(isHoldOn)
            {
                Roll();
            }
        };
    }

    public void EnterToHideState(System.Action onComplate)
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(DOTween.To(() => rollBtn.anchoredPosition, p => rollBtn.anchoredPosition = p, new Vector2(rollBtn.anchoredPosition.x, -300), 1f).SetEase(Ease.OutExpo));
        sq.Insert(0.5f, DOTween.To(() => switchBtn.anchoredPosition, p => switchBtn.anchoredPosition = p, new Vector2(200, switchBtn.anchoredPosition.y), 1).SetEase(Ease.OutCubic));
        sq.Insert(0.5f, DOTween.To(() => panel.anchoredPosition, x => panel.anchoredPosition = x, new Vector2(-600, panel.anchoredPosition.y), 1));
        sq.OnComplete(() =>
        {
            onComplate();
        });
    }

    public void ShowBuildState()
    {
        rollBtn.anchoredPosition = new Vector2(rollBtn.anchoredPosition.x, -300);
        rollBtn.localScale = new Vector3(1.5f, 1.5f, 1);
        switchBtn.anchoredPosition = new Vector2(200, switchBtn.anchoredPosition.y);
        panel.anchoredPosition = new Vector2(-800, 1000);
        panel.localScale = new Vector3(2f, 2f, 1);

        rollBtn.gameObject.SetActive(false);
        switchBtn.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
    }

    public void ShowWheelState()
    {

        rollBtn.gameObject.SetActive(true);
        switchBtn.gameObject.SetActive(true);
        panel.gameObject.SetActive(true);

        rollBtn.anchoredPosition = rollBtnOriginalValue;
        rollBtn.localScale = Vector3.one;
        switchBtn.anchoredPosition = switchOriginalValue;
        panel.anchoredPosition = panelLocalOriginalValue;
        panel.localScale = Vector3.one;
    }

    public void SetStealerData(TargetData target)
    {
        if (stealTarget != null && stealTarget.uid != target.uid)
        {
            CanvasGroup canvasGroup = stealHead.transform.parent.GetComponent<CanvasGroup>();
            Sequence sq = DOTween.Sequence();
            sq.AppendInterval(2);
            sq.Append(stealHead.transform.parent.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1));
            sq.Insert(2, canvasGroup.DOFade(0, 1));
            sq.AppendCallback(() =>
            {
                stealNameLabel.text = target.name;
                stealMoneyLabel.text = GameUtils.GetCurrencyString(target.money);
                stealHead.transform.parent.DOScale(Vector3.one, 1);
                canvasGroup.DOFade(1, 1);
                AssetLoadManager.Instance.LoadAsset<Texture2D>(target.headImg, (text) =>
                {
                    stealHead.texture = text;
                    
                });
            });

        }
        else if (stealTarget == null)
        {
            stealNameLabel.text = target.name;
            stealMoneyLabel.text = GameUtils.GetCurrencyString(target.money);
            AssetLoadManager.Instance.LoadAsset<Texture2D>(target.headImg, (text) =>
            {
                stealHead.texture = text;
            });
        }
        stealTarget = target;
    }
    //设置转盘
    public void SetData(RollerItemData[] datas)
    {
        foreach (RollerItemData data in datas)
        {
            int index = data.index;
            TextMeshProUGUI t = names[index];
            Image pic = images[index];
            pic.enabled = true;
            if (data.type == "steal")
            {
                t.text = data.name;
                pic.sprite = sprites[0];
            }
            else if (data.type == "energy")
            {
                t.text = data.name;
                pic.sprite = sprites[1];
            }
            else if (data.type == "shield")
            {
                t.text = data.name;
                pic.sprite = sprites[2];
            }
            else if (data.type == "shoot")
            {
                t.text = data.name;
                pic.sprite = sprites[3];
            }
            else if (data.type == "coin")
            {
                t.text = data.name;
                pic.enabled = false;
            }
            else if (data.type == "xcrowns")
            {
                t.text = string.Format("{0}x{1}", "<sprite=0>", data.code);
                pic.enabled = false;
            }
            else
            {
                t.text = data.name;
                pic.enabled = false;
            }
        }
    }


    private void OnUpdateBaseData(BaseEvent e)
    {
        UpdateBaseDataEvent evt = e as UpdateBaseDataEvent;
        if (evt.updateType == UpdateBaseDataEvent.UpdateType.vip || evt.updateType == UpdateBaseDataEvent.UpdateType.Energy)
        {
            energyValue = user.energy;
        }
    }

    public void onClickRollBtn()
    {


        if (!isWorking)
        {

            Roll();
        }
       
    }

    public void OnHoldOnRollBtn(bool isHold)
    {
        if(isHold )
        {
            isHoldOn = true;
            if (!isWorking && !QY.UI.Interactable.isLock)
            {
                Roll();

            }
        }
        else
        {
            isHoldOn = false;
        }
       
    }

    private void Roll()
    {
        AudioManager.instance.PlaySound("wheel_roll_btn");
        if (GameMainManager.instance.model.userData.energy > 0)
        {
            StartCoroutine(StartRoll());

        }
        else if (!AccountManager.instance.isLoginAccount)
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIFacebookTipsWindow);
        }
        else
        {

            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIEmptyEnergyGuideWindow, new ShowEmptyGuideWindowData(ShowEmptyGuideWindowData.PanelType.EmptyEnergy));
        }
    }

    private IEnumerator StartRoll()
    {
        isWorking = true;
        GameMainManager.instance.uiManager.DisableOperation();

        bool getRes = false;
        RollerItemData rollItem = null;
        RollData rollData = null;
        float timeTag = Time.time;
        shieldCount = GameMainManager.instance.model.userData.shields;
        GameMainManager.instance.netManager.Roll((ret, data) => {
            if (data.isOK)
            {
                getRes = true;
                rollData = data.data;
                rollItem = data.data.rollerItem;

                energyValue = user.energy - 1;

            }
            else
            {
                isWorking = false;
                getRes = true;
                GameMainManager.instance.uiManager.EnableOperation();
            }

            

        });
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioClips[0];
        audio.loop = true;
        audio.Play();
        reflective.DOFade(1, 0.5f);
        while (!getRes || (Time.time - timeTag)<2 )
        {
            wheel.Rotate(new Vector3(0, 0, -360 * 4) * Time.deltaTime);

            yield return null;
        }

        reflective.DOFade(0, 1f);
        if (rollItem == null)
        {
            wheel.DOLocalRotate(new Vector3(0, 0, -(360 * 1 + 36 * 0)), 2f, RotateMode.FastBeyond360).SetEase(Ease.OutCirc);
            yield break;
        }
        audio.clip = audioClips[1];
        audio.loop = false;
        audio.Play();
        wheel.DOLocalRotate(new Vector3(0, 0, -(360 * 1 + 36 * rollItem.index)), 2f, RotateMode.FastBeyond360).SetEase(Ease.OutCirc).OnComplete(() => {
           
            showResault(rollData);
            audio.Stop();
            isWorking = false;

            if(isHoldOn && (rollData.rollerItem.type == "coin" || 
            rollData.rollerItem.type == "shield" || 
            rollData.rollerItem.type == "xcrowns" || 
            rollData.rollerItem.type == "energy"))
            {
                StartCoroutine(StartRoll());
            }
        });
    }

    

    //展示roll结果
    private void showResault(RollData rollData)
    {
        RollerItemData rollItem = rollData.rollerItem;
        
        if(rollItem.type == "energy")
        {
            ShowEnergy();
        }
        else if (rollItem.type == "shield")
        {
            ShowShield(shieldCount < rollData.shields);
        }
        else if (rollItem.type == "steal")
        {
            beaver.SetActive(true);
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_steal);
            Sequence sq = DOTween.Sequence();
            sq.AppendInterval(1.3f);
            sq.InsertCallback(0.5f, () => {
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_steal_gone);
            });
            sq.AppendCallback(() => {
                beaver.SetActive(false);
                GameMainManager.instance.uiManager.EnableOperation();
                Dictionary<UISettings.UIWindowID, object> stateData = new Dictionary<UISettings.UIWindowID, object>();
                stateData.Add(UISettings.UIWindowID.UIStealWindow, rollData.stealIslands);
                GameMainManager.instance.uiManager.ChangeState(new UIStateChangeBase(stateData,null,2));
            });
        }
        else if (rollItem.type == "shoot")
        {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_attack_prepare);
            GameMainManager.instance.uiManager.EnableOperation();
            Dictionary<UISettings.UIWindowID, object> stateData = new Dictionary<UISettings.UIWindowID, object>();
            stateData.Add(UISettings.UIWindowID.UIAttackWindow, rollData.attackTarget);
            GameMainManager.instance.uiManager.ChangeState(new UIStateChangeBase(stateData));
        }
        else if(rollItem.type == "xcrowns")
        {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_Star);

            int money = rollItem.code;

            getStarEffect.gameObject.SetActive(true);
            getStarEffect.Show(user.crowns,rollItem.code,()=> {

                goldEffect.emission.SetBursts(new ParticleSystem.Burst[1] { new ParticleSystem.Burst(0, 80, 100, 1, 0.01f) });
                goldEffect.Play();
                energyValue = user.energy;
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_gold_large);
                EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money, 0));
                GameMainManager.instance.uiManager.EnableOperation();
                
            });
           
        }
        else if (rollItem.type == "coin" )
        {
            int money = rollItem.code;

            if (money < 10000)
            {
                goldEffect.emission.SetBursts(new ParticleSystem.Burst[1] { new ParticleSystem.Burst(0, 10, 20, 1, 0.01f) });
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_gold_small);
            }
            else if (money < 50000)
            {
                goldEffect.emission.SetBursts(new ParticleSystem.Burst[1] { new ParticleSystem.Burst(0, 20, 50, 1, 0.01f) });
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_gold_med);
            }
            else if (money < 100000)
            {
                goldEffect.emission.SetBursts(new ParticleSystem.Burst[1] { new ParticleSystem.Burst(0, 50, 80, 1, 0.01f) });
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_gold_large);
            }
            else
            {
                goldEffect.emission.SetBursts(new ParticleSystem.Burst[1] { new ParticleSystem.Burst(0, 80, 100, 1, 0.01f) });
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_gold_large);
            }
            goldEffect.Play();
            energyValue = user.energy;
            GameMainManager.instance.uiManager.EnableOperation();
            EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money,0));
        }
    }
    //展示获得能量效果
    private void ShowEnergy()
    {
        GameObject icon = energy;

        GameObject backLight = lightBG;
        Vector3 moveTarget = new Vector3(0, 0, 0);

        icon.transform.localPosition = Vector3.zero;
        icon.transform.localScale = Vector3.zero;
        backLight.transform.localScale = Vector3.one;
        icon.SetActive(true);
        backLight.SetActive(true);

        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_energy_start);
        Sequence sq = DOTween.Sequence();
        sq.Append(backLight.transform.DOLocalRotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360));
        sq.Insert(0, icon.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
        sq.InsertCallback(1.5f, () => {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_energy_transform);
        });
        sq.Insert(1.5f,backLight.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutQuart));
        sq.Insert(1.5f,icon.transform.DOLocalMove(moveTarget, 0.5f).SetEase(Ease.InQuad));
        sq.Insert(1.5f, icon.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuad));
        //sq.AppendInterval(0.5f);
        sq.onComplete += () => {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_energy_change);
            GameMainManager.instance.uiManager.EnableOperation();
            icon.SetActive(false);
            backLight.SetActive(false);
            energyValue = user.energy;
            EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Energy,0));
        };
    }
    //展示护盾效果
    private void ShowShield(bool isGet)
    {
        GameObject icon = shield;

        GameObject backLight = lightBG;
        Vector3 moveTarget = new Vector3(-170, 449, 0);

        icon.transform.localPosition = Vector3.zero;
        icon.transform.localScale = Vector3.zero;
        backLight.transform.localScale = Vector3.one;
        icon.SetActive(true);
        backLight.SetActive(true);


        GetShieldPosEvent evt = new GetShieldPosEvent((pos) => {
            moveTarget = pos;
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_shiled_start);
            Sequence sq = DOTween.Sequence();
            sq.Append(backLight.transform.DOLocalRotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360));
            sq.Insert(0, icon.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
            sq.InsertCallback(1.5f, () => {
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_energy_transform);
            });
            sq.Insert(1.5f, backLight.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InQuart));
            sq.Insert(1.5f, icon.transform.DOMove(moveTarget, 1).SetEase(Ease.InQuart));
            sq.Insert(1.5f, icon.transform.DOScale(new Vector3(0.16f, 0.16f,0.16f), 0.8f));
            sq.AppendCallback(() => {

                Debug.Log(isGet);
                if (!isGet)
                {
                    GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_shield_full);
                }
                else
                {
                    GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_shield_got);
                }

            });
            if (!isGet)
            {
                sq.Append(icon.transform.DOLocalMove(Vector3.zero, 1f));
            }
            sq.onComplete += () => {
                
                GameMainManager.instance.uiManager.EnableOperation();
                icon.SetActive(false);
                backLight.SetActive(false);
                energyValue = user.energy;
                EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.sheild,0));

            };
        });

        EventDispatcher.instance.DispatchEvent(evt);
    }


}
