﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNamePanel : MonoBehaviour {

    public InputField inputField;
    public UGUISpriteAnimation spriteAnimation;
    public event System.Action<string> onConfirmName;
    public event System.Action onCancle;

    public void OnClickRandomName()
    {
        StartCoroutine(StartRandomName());
    }

    private IEnumerator StartRandomName()
    {
        spriteAnimation.Play();
        yield return new WaitForSeconds(1);
        spriteAnimation.Pause();
        inputField.text = "游客"+ Random.Range(1,10000).ToString();
    }

    public void OnClickConfirm()
    {
        string name = inputField.text;
        int length = System.Text.Encoding.Default.GetByteCount(name);
        if(!string.IsNullOrEmpty(name) && length < 12)
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
}