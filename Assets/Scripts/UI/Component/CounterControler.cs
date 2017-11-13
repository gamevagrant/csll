using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CounterControler : MonoBehaviour {

    public Button minusBtn;
    public Button plusBtn;
    public TextMeshProUGUI text;
    public int min = 0;
    public int max = 99;
    public event System.Action<int> onChangeValue;

    private int _num = 0;
    public int num
    {
        get
        {
            return num;
        }
        set
        {
            _num = value;
            UpdateText();
        }
    }

    private void Awake()
    {
        minusBtn.onClick.AddListener(OnClickMinusBtn);
        plusBtn.onClick.AddListener(OnClickPlusBtn);
    }

    private void OnDestroy()
    {
        minusBtn.onClick.RemoveAllListeners();
        plusBtn.onClick.RemoveAllListeners();
    }

    private void OnClickMinusBtn()
    {
        _num--;
        _num = Mathf.Max(min, _num);
        UpdateText();
    }

    private void OnClickPlusBtn()
    {
        _num++;
        _num = Mathf.Min(max, _num);
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = _num.ToString() + "个";
        if (onChangeValue != null)
            onChangeValue(_num);
    }
}
