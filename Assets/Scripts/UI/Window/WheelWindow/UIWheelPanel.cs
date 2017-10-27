using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIWheelPanel : MonoBehaviour {

    public Text[] names;//文字显示位置
    public Image[] images;//图片显示位置
    public Sprite[] sprites;//sprite集合
    public Image reflective;//反光
    public Transform wheel;
    public ParticleSystem goldEffect;
    public GameObject shield;
    public GameObject energy;
    public GameObject light;
    public GameObject beaver;

    public RawImage stealHead;
    public Text stealNameLabel;
    public Text stealMoneyLabel;

    public Text countDownLabel;
    public Text addEnergyCountLabel;
    public Text energyLabel;
    public Slider energyProgressSlider;
    public AudioClip[] audioClips;

    [SerializeField]
    private GameObject rollBtn;
    [SerializeField]
    private GameObject switchBtn;
    [SerializeField]
    private GameObject panel;

    private bool isWorking;


    private Vector2 rollBtnOriginalValue;//roll点按钮位置原始值 下同
    private Vector2 switchOriginalValue;
    private Vector2 panelLocalOriginalValue;

    private TargetData stealTarget;

    // Use this for initialization
    void Awake ()
    {
        rollBtnOriginalValue = (rollBtn.transform as RectTransform).anchoredPosition;
        switchOriginalValue = (switchBtn.transform as RectTransform).anchoredPosition;
        panelLocalOriginalValue = (panel.transform as RectTransform).anchoredPosition;

        reflective.color = new Color(1, 1, 1, 0);
    }

   

    private void Start()
    {
        isWorking = false;
    }


    public void SetEnergyData(int maxEnergy,int energy,int recoverEnergy,long timeToRecover)
    {
        energyLabel.text = string.Format("{0}/{1}", energy, maxEnergy);
        addEnergyCountLabel.text = "+ "+recoverEnergy.ToString();
        countDownLabel.text = GameUtils.TimestampToDateTime(timeToRecover).ToString("mm:ss");
        energyProgressSlider.value = energy / (float)maxEnergy;
    }

    public void SetStealerData(TargetData target)
    {
        if(stealTarget != null && stealTarget.uid != target.uid)
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
                AssetLoadManager.Instance.LoadAsset<Texture2D>(target.headImg, (text) =>
                {
                    stealHead.texture = text;
                    stealHead.transform.parent.DOScale(Vector3.one, 1);
                    canvasGroup.DOFade(1, 1);
                });
            });

        }
        else if(stealTarget == null)
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

    public void setData(RollerItemData[] datas)
    {
        foreach(RollerItemData data in datas)
        {
            int index = data.index;
            Text t = names[index];
            Image pic = images[index];
            pic.enabled = true;
            if(data.type == "steal")
            {
                t.text = data.name;
                pic.sprite = sprites[0];
            }else if(data.type == "energy")
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
        }
    }

    public void enterToBuildPanelState()
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_view_switch_in);
        RectTransform rollBtnTF = rollBtn.transform as RectTransform;

        DOTween.To(() => rollBtnTF.anchoredPosition, p => rollBtnTF.anchoredPosition = p, new Vector2(rollBtnTF.anchoredPosition.x, -300), 1).SetEase(Ease.InQuint);
        DOTween.To(() => rollBtnTF.localScale, x => rollBtnTF.localScale = x, new Vector3(1.5f,1.5f,1), 1);

        RectTransform switchBtnTF = switchBtn.transform as RectTransform;
        DOTween.To(() => switchBtnTF.anchoredPosition, p => switchBtnTF.anchoredPosition = p, new Vector2(200, switchBtnTF.anchoredPosition.y), 1);

        RectTransform rollPanelTF = panel.transform as RectTransform;
        DOTween.To(() => rollPanelTF.anchoredPosition, x => rollPanelTF.anchoredPosition = x, new Vector2(-800,1000), 1);
        DOTween.To(() => rollPanelTF.localScale, x => rollPanelTF.localScale = x, new Vector3(2f, 2f, 1), 1).onComplete = ()=> {

            rollBtn.SetActive(false);
            switchBtn.SetActive(false);
            panel.SetActive(false);
        };
    }

    public void enterToWheelPanelState()
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_view_switch_out);
        GameMainManager.instance.uiManager.DisableOperation();
        rollBtn.SetActive(true);
        switchBtn.SetActive(true);
        panel.SetActive(true);

        RectTransform rollBtnTF = rollBtn.transform as RectTransform;
        DOTween.To(() => rollBtnTF.anchoredPosition, p => rollBtnTF.anchoredPosition = p, rollBtnOriginalValue, 1).SetEase(Ease.OutQuint);
        DOTween.To(() => rollBtnTF.localScale, x => rollBtnTF.localScale = x, Vector3.one, 1);

        RectTransform switchBtnTF = switchBtn.transform as RectTransform;
        DOTween.To(() => switchBtnTF.anchoredPosition, p => switchBtnTF.anchoredPosition = p, switchOriginalValue, 1);

        RectTransform rollPanelTF = panel.transform as RectTransform;
        DOTween.To(() => rollPanelTF.anchoredPosition, x => rollPanelTF.anchoredPosition = x, panelLocalOriginalValue, 1);
        DOTween.To(() => rollPanelTF.localScale, x => rollPanelTF.localScale = x, Vector3.one, 1).onComplete = () => {

            GameMainManager.instance.uiManager.EnableOperation();
        };
    }

    public void ClosePanel(System.Action onComplate)
    {
        RectTransform rollBtnTF = rollBtn.transform as RectTransform;
        RectTransform switchBtnTF = switchBtn.transform as RectTransform;
        RectTransform rollPanelTF = panel.transform as RectTransform;

        Sequence sq = DOTween.Sequence();
        sq.Append(DOTween.To(() => rollBtnTF.anchoredPosition, p => rollBtnTF.anchoredPosition = p, new Vector2(rollBtnTF.anchoredPosition.x, -300), 1f).SetEase(Ease.OutExpo));
        sq.Insert(0.5f,DOTween.To(() => switchBtnTF.anchoredPosition, p => switchBtnTF.anchoredPosition = p, new Vector2(200, switchBtnTF.anchoredPosition.y), 1).SetEase(Ease.OutCubic));
        sq.Insert(0.5f, DOTween.To(() => rollPanelTF.anchoredPosition, x => rollPanelTF.anchoredPosition = x, new Vector2(-600, rollPanelTF.anchoredPosition.y), 1));
        sq.onComplete+= () => {

            rollBtn.SetActive(false);
            switchBtn.SetActive(false);
            panel.SetActive(false);
            onComplate();
        };
    }

    public void OpenPanel(System.Action onComplate)
    {
        GameMainManager.instance.uiManager.DisableOperation();
        rollBtn.SetActive(true);
        switchBtn.SetActive(true);
        panel.SetActive(true);

        RectTransform rollBtnTF = rollBtn.transform as RectTransform;
        RectTransform switchBtnTF = switchBtn.transform as RectTransform;
        RectTransform rollPanelTF = panel.transform as RectTransform;

        rollBtnTF.anchoredPosition = new Vector2(rollBtnTF.anchoredPosition.x, -300);
        switchBtnTF.anchoredPosition = new Vector2(200, switchBtnTF.anchoredPosition.y);
        rollPanelTF.anchoredPosition = new Vector2(-600, rollPanelTF.anchoredPosition.y);

        Sequence sq = DOTween.Sequence();
        sq.Append(DOTween.To(() => rollBtnTF.anchoredPosition, p => rollBtnTF.anchoredPosition = p, rollBtnOriginalValue, 1f).SetEase(Ease.OutExpo));
        sq.Insert(0,DOTween.To(() => switchBtnTF.anchoredPosition, p => switchBtnTF.anchoredPosition = p, switchOriginalValue, 1).SetEase(Ease.OutCubic));
        sq.Insert(0, DOTween.To(() => rollPanelTF.anchoredPosition, x => rollPanelTF.anchoredPosition = x, panelLocalOriginalValue, 1));
        sq.onComplete += () => {

            GameMainManager.instance.uiManager.EnableOperation();
            onComplate();
        };
    }

    public void onClickRollBtn()
    {
        //startRotate();
        StartCoroutine(StartRoll());
    }
    /*
    private void startRotate()
    {
        if(!isWorking)
        {
            isWorking = true;
            GameMainManager.instance.uiManager.DisableOperation();
            GameMainManager.instance.netManager.Roll((ret, data) => {
                if(data.isOK)
                {
                    StartCoroutine(rotateWheel(data.data.rollerItem,()=> {

                        showResault(data.data);
                    }));
                }else
                {
                    GameMainManager.instance.uiManager.EnableOperation();
                }
               
            });
        }
    }*/

    private IEnumerator StartRoll()
    {
        isWorking = true;
        GameMainManager.instance.uiManager.DisableOperation();

        bool getRes = false;
        RollerItemData rollItem = null;
        RollData rollData = null;
        float timeTag = Time.time;
        GameMainManager.instance.netManager.Roll((ret, data) => {
            if (data.isOK)
            {
                getRes = true;
                rollData = data.data;
                rollItem = data.data.rollerItem;
            }
            else
            {
                isWorking = false;
                GameMainManager.instance.uiManager.EnableOperation();
            }

        });
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioClips[0];
        audio.loop = true;
        audio.Play();
        reflective.DOFade(1, 0.5f);
        while (!getRes || (Time.time - timeTag)<3 )
        {
            wheel.Rotate(new Vector3(0, 0, -360 * 4) * Time.deltaTime);

            yield return null;
        }
        reflective.DOFade(0, 1f);
        audio.clip = audioClips[1];
        audio.loop = false;
        audio.Play();
        wheel.DOLocalRotate(new Vector3(0, 0, -(360 * 1 + 36 * rollItem.index)), 2.3f, RotateMode.FastBeyond360).SetEase(Ease.OutCirc).OnComplete(() => {
            isWorking = false;
            showResault(rollData);
            audio.Stop();
        });
    }
    /*
    private IEnumerator rotateWheel(RollerItemData rollItem,System.Action callback)
    {

        reflective.gameObject.SetActive(true);
        wheel.DOLocalRotate(new Vector3(0, 0, -(360 * 6 + 36 * rollItem.index)), 4, RotateMode.FastBeyond360).SetEase(Ease.OutQuart).OnComplete(() => {
            isWorking = false;
            callback();
           
        });
        yield return new WaitForSeconds(1.5f);
        reflective.gameObject.SetActive(false);
    }*/

    private void showResault(RollData rollData)
    {
        RollerItemData rollItem = rollData.rollerItem;
        if (rollItem.type == "coin")
        {
            if(rollItem.code<10000)
            {
                goldEffect.emission.SetBursts(new ParticleSystem.Burst[1]{ new ParticleSystem.Burst(0, 10, 20, 1, 0.01f)});
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_gold_small);
            }else if(rollItem.code <50000)
            {
                goldEffect.emission.SetBursts(new ParticleSystem.Burst[1] { new ParticleSystem.Burst(0, 20, 50, 1, 0.01f) });
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_gold_med);
            }
            else if(rollItem.code <100000)
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
            GameMainManager.instance.uiManager.EnableOperation();
        }
        else if(rollItem.type == "energy")
        {
            ShowEnergy();
        }
        else if (rollItem.type == "shield")
        {
            ShowShield(GameMainManager.instance.model.userData.shields != rollData.shields);
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
    }

    private void ShowEnergy()
    {
        GameObject icon = energy;

        GameObject backLight = light;
        Vector3 moveTarget = new Vector3(270, 349, 0);

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
        sq.Insert(1.5f,icon.transform.DOLocalMove(moveTarget, 1).SetEase(Ease.OutQuart));
        sq.Insert(1.5f, icon.transform.DOScale(new Vector3(0.2f,0.2f,1), 0.5f).SetEase(Ease.OutQuart));
        sq.AppendInterval(0.5f);
        sq.onComplete += () => {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_energy_change);
            GameMainManager.instance.uiManager.EnableOperation();
            icon.SetActive(false);
            backLight.SetActive(false);
        };
    }
    private void ShowShield(bool isGet)
    {
        GameObject icon = shield;

        GameObject backLight = light;
        Vector3 moveTarget = new Vector3(-170, 449, 0);

        icon.transform.localPosition = Vector3.zero;
        icon.transform.localScale = Vector3.zero;
        backLight.transform.localScale = Vector3.one;
        icon.SetActive(true);
        backLight.SetActive(true);

        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_shiled_start);
        Sequence sq = DOTween.Sequence();
        sq.Append(backLight.transform.DOLocalRotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360));
        sq.Insert(0, icon.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
        sq.InsertCallback(1.5f, () => {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_energy_transform);
        });
        sq.Insert(1.5f, backLight.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InQuart));
        sq.Insert(1.5f, icon.transform.DOLocalMove(moveTarget, 1).SetEase(Ease.InQuart));
        sq.Insert(1.5f, icon.transform.DOScale(new Vector3(0.2f, 0.2f, 1), 0.8f));
        sq.AppendCallback(() => {

            Debug.Log(isGet);
            if(!isGet)
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
           
            sq.Append(icon.transform.DOLocalMove(new Vector3(100, -100, 0), 0.5f).SetRelative(true));
        }
        sq.AppendInterval(0.5f);
        sq.onComplete += () => {
            
            GameMainManager.instance.uiManager.EnableOperation();
            icon.SetActive(false);
            backLight.SetActive(false);
        };
    }
}
