using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAudio : MonoBehaviour, IPointerClickHandler {


    public void OnPointerClick(PointerEventData eventData)
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.button_click);
    }

}
