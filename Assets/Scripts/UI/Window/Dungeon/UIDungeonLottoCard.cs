using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIDungeonLottoCard : MonoBehaviour,IPointerClickHandler
{
    private enum State
    {
        Selected,
        UnSelected,
        InCell,
    }

    public System.Action<UIDungeonLottoCard> onSelected;
    public System.Action onUnSelected;

    private Vector3 position;
    private Quaternion rotation;
    private Transform parent;
    private int sibling;
    private State state = State.UnSelected;


    private void Start()
    {
        position = transform.localPosition;
        rotation = transform.localRotation;
        parent = transform.parent;
        sibling = transform.GetSiblingIndex();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(state== State.UnSelected)
        {
            Selected();
            if(onSelected!=null)
            {
                onSelected(this);
            }
        }else
        {
            UnSelected();
            if(onUnSelected!=null)
            {
                onUnSelected();
            }
        }
    }

    public void Selected()
    {
        if(state == State.UnSelected)
        {
            transform.localPosition = position;
            (transform as RectTransform).anchoredPosition += new Vector2(transform.up.x, transform.up.y) * 20;

            state = State.Selected;
            AudioManager.instance.PlaySound(AudioNameEnum.dungeon_extractCard);
        }
        
    }

    public void UnSelected()
    {
        if(state == State.Selected)
        {
            transform.localPosition = position;

            state = State.UnSelected;
            AudioManager.instance.PlaySound(AudioNameEnum.dungeon_extractCard);
        }
       
    }

    public void MoveTarget(Transform target)
    {
        transform.parent = target;

        transform.DOLocalMove(Vector3.zero,0.5f);
        transform.DOLocalRotate(Vector3.zero, 0.5f);
        AudioManager.instance.PlaySound(AudioNameEnum.dungeon_extractCard);
    }

    public void MoveReturn()
    {
        transform.parent = parent;
        transform.SetSiblingIndex(sibling);
        transform.DOLocalMove(position, 0.5f);
        transform.DOLocalRotate(rotation.eulerAngles, 0.5f);
        AudioManager.instance.PlaySound(AudioNameEnum.dungeon_extractCard);
    }
}
