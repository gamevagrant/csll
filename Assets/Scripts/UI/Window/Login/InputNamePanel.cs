using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNamePanel : MonoBehaviour {

    public InputField inputField;
    public UGUISpriteAnimation spriteAnimation;
    public event System.Action<string> onConfirmName;
    public event System.Action onCancle;

    private void Start()
    {
        inputField.text = GameMainManager.instance.configManager.randomNamesConfig.GetRandomName();
    }

    public void OnClickRandomName()
    {
        StartCoroutine(StartRandomName());
    }

    private IEnumerator StartRandomName()
    {
        spriteAnimation.Play();
        yield return new WaitForSeconds(1);
        spriteAnimation.Pause();
        inputField.text = GameMainManager.instance.configManager.randomNamesConfig.GetRandomName();
    }

    public void OnClickConfirm()
    {
        string name = inputField.text;
        int length = System.Text.Encoding.UTF8.GetByteCount(name);

        if (!string.IsNullOrEmpty(name) && length <= 18)
        {
            if (onConfirmName != null)
            {
                onConfirmName(name);
            }
            gameObject.SetActive(false);
        }

    }

    public void OnClickCancle()
    {
        if (onCancle != null)
        {
            onCancle();
        }
        gameObject.SetActive(false);
    }

    string inputName;
    public void OnInputNameHandle(string str)
    {
        int length = System.Text.Encoding.UTF8.GetByteCount(str);
        if ( length > 18)
        {
            inputField.text = inputName;
        }else
        {
            inputName = str;
        }
    }
}
