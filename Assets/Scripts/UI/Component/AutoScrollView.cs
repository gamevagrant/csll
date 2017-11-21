using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(ScrollRect))]
public class AutoScrollView : MonoBehaviour {

    public enum Direction
    {
        Horizontal,
        Vertical,
    }

    public RectTransform itemTemplate;
    public float spacing = 10;//间距
    public Direction direction;
    public RectTransform center;//观察的主要位置，每个item都要能进入此位置并停留在中心

    private ScrollRect scrollRect;
    private RectTransform content;
    //所有的rect显示区域的锚点都在左上角
    private Rect displayRect;//显示区域
    private GameObjectPool itemPool;


    private List<ItemRect> itemDatas;
    private ItemRect _curItem;
    private ItemRect curItem
    {
        get
        {
            return _curItem;
        }
        set
        {
            _curItem = value;
            ItemRect item = _curItem;
            if (direction == Direction.Horizontal)
            {
                Vector2 endPos = item.rect.position + item.rect.size / 2 - center.anchoredPosition;
                endPos = new Vector2(endPos.x, 0);
                float p = endPos.x / (content.sizeDelta - displayRect.size).x;
                scrollRect.DOHorizontalNormalizedPos(p, 0.5f);
            }
            else
            {
                Vector2 endPos = item.rect.position - item.rect.size / 2 - center.anchoredPosition;
                endPos = new Vector2(0, endPos.y);
                float p = 1 + endPos.y / (content.sizeDelta - displayRect.size).y;
                scrollRect.DOVerticalNormalizedPos(p, 0.5f);
            }
            item.item.OnSelected(true);
        }
    }

    void Awake () {
        scrollRect = gameObject.GetComponent<ScrollRect>();
        content = scrollRect.content;
        displayRect = new Rect(0, 0, scrollRect.viewport.rect.width, scrollRect.viewport.rect.height);

        itemPool = content.gameObject.AddComponent<GameObjectPool>();
        itemPool.target = itemTemplate.gameObject;
        itemTemplate.gameObject.SetActive(false);
        itemTemplate.pivot = new Vector2(0, 1);
        itemTemplate.anchorMin = new Vector2(0, 1);
        itemTemplate.anchorMax = new Vector2(0, 1);

        scrollRect.vertical = direction == Direction.Vertical;
        scrollRect.horizontal = direction == Direction.Horizontal;
        content.anchorMin = direction == Direction.Horizontal ? new Vector2(0, 0.5f) : new Vector2(0.5f, 1);
        content.anchorMax = direction == Direction.Horizontal ? new Vector2(0, 0.5f) : new Vector2(0.5f, 1);
        content.pivot = direction == Direction.Horizontal ? new Vector2(0, 0.5f) : new Vector2(0.5f, 1);
        ChangeAnchor(center, direction == Direction.Horizontal ? new Vector2(0, 0.5f) : new Vector2(0.5f, 1));
        //center.anchoredPosition += new Vector2(0.5f * (center.parent as RectTransform).rect.width,0);
        //center.anchorMin = direction == Direction.Horizontal ? new Vector2(0, 0.5f) : new Vector2(0.5f, 1);
        //center.anchorMax = direction == Direction.Horizontal ? new Vector2(0, 0.5f) : new Vector2(0.5f, 1);

        scrollRect.onValueChanged.AddListener(ListenerMethod);

        EventTrigger et = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener(OnDragEndHandle);
        et.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry.callback.AddListener(OnDragHandle);
        et.triggers.Add(entry);
    }

    private void OnDestroy()
    {
        scrollRect.onValueChanged.RemoveAllListeners();
    }

    public void SetData(IList datas)
    {
        if (datas == null)
            return;

        itemPool.resetAllTarget();
        displayRect.x = 0;
        displayRect.y = 0;

        Vector2 offset = Vector2.zero;
        //加开头空余空间
        offset = new Vector2(center.anchoredPosition.x - itemTemplate.rect.width / 2, center.anchoredPosition.y + itemTemplate.rect.height / 2);
        itemDatas = new List<ItemRect>();
        for (int j = 0; j < datas.Count; j++)
        {
            ItemRect item = new ItemRect();
            item.rect = direction == Direction.Horizontal ? new Rect(offset.x, 0, itemTemplate.rect.width, itemTemplate.rect.height) : new Rect(0, offset.y, itemTemplate.rect.width, itemTemplate.rect.height);
            item.data = datas[j];
            item.isVisable = false;
            itemDatas.Add(item);
            offset += new Vector2(itemTemplate.rect.width + spacing, -(itemTemplate.rect.height + spacing));

        }
        //加末尾空余空间
        offset += new Vector2((center.parent as RectTransform).rect.width - center.anchoredPosition.x - itemTemplate.rect.width / 2, -((center.parent as RectTransform).rect.height + center.anchoredPosition.y - itemTemplate.rect.height / 2));

        content.sizeDelta = direction == Direction.Horizontal?new Vector2(offset.x, itemTemplate.rect.height) :new Vector2(itemTemplate.rect.width, -offset.y);
       
        //content.anchoredPosition = Vector2.zero;
        scrollRect.verticalNormalizedPosition = 1;
        scrollRect.horizontalNormalizedPosition = 0;
        updateItems();
        curItem = itemDatas[0];
    }

    private void OnDragHandle(BaseEventData e)
    {
        if(curItem != null)
        {
            curItem.item.OnSelected(false);
        }
    }
    private void OnDragEndHandle(BaseEventData e)
    {
        ItemRect item = GetNearestItem();

        curItem = item;
        
    }
    private void ListenerMethod(Vector2 value)
    {
        float startPointY = (1 - value.y) * (content.rect.height - displayRect.height);
        float startPointX = value.x * (content.rect.width - displayRect.width);
        displayRect.y = -startPointY;
        displayRect.x = startPointX;
        //Debug.Log(scrollRect.verticalNormalizedPosition);
        updateItems();
    }

    private void updateItems()
    {
        if (itemDatas == null)
            return;

        Rect rect = displayRect;
        for (int i = 0; i < itemDatas.Count; i++)
        {
            ItemRect item = itemDatas[i];
            if (isIntersect(item.rect, rect))
            {
                if (!item.isVisable)
                {
                    item.item = itemPool.getIdleTarget<BaseItemView>();
                    item.isVisable = true;
                    (item.item.transform as RectTransform).anchoredPosition = item.rect.position;
                    item.item.SetData(item.data);
                }
            }
            else
            {
                if (item.isVisable)
                {
                    item.isVisable = false;
                    if (item.item != null)
                    {
                        item.item.gameObject.SetActive(false);
                        item.item = null;
                    }

                }
            }
        } 
    }


    private ItemRect GetNearestItem()
    {
        Vector2 target = center.anchoredPosition;
        int head = 0;
        int end = itemDatas.Count-1;

        Vector2 half = direction == Direction.Horizontal ? new Vector2(itemDatas[head].rect.width / 2,0) : new Vector2(0,-itemDatas[head].rect.height / 2);
        while (head != end)
        {
            float q = (displayRect.position + target).sqrMagnitude;
            if (end - head == 1)
            {
                float a = q - (itemDatas[head].rect.position + half).sqrMagnitude;
                float b = (itemDatas[end].rect.position + half).sqrMagnitude - q;
                
                if(a>b)
                {
                    head = end;
                }else
                {
                    end = head;
                }
            }else
            {
                int index = Mathf.RoundToInt((end - head) / 2) + head;
                ItemRect item = itemDatas[index];
                if ((item.rect.position + half).sqrMagnitude < q)
                {
                    head = index;
                }
                else
                {
                    end = index;
                }
            }
            Debug.Log(string.Format("head:{0}  end:{1}",head,end));
        }
        return itemDatas[head];
    }

    //改变锚点但不改变位置，只支持锚点为一个点的情况
    private void ChangeAnchor(RectTransform rtf , Vector2 anchor)
    {
        Vector2 offset = rtf.anchorMin - anchor ;
        rtf.anchoredPosition += new Vector2(offset.x * (rtf.parent as RectTransform).rect.width, offset.y * (rtf.parent as RectTransform).rect.height);
        rtf.anchorMin = anchor;
        rtf.anchorMax = anchor;
    }

    //判断两个区域是否相交
    private bool isIntersect(Rect a, Rect b)
    {

        if (a.x + a.width < b.x || a.x > b.x + b.width || a.y - a.height > b.y || a.y < b.y - b.height)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    private class ItemRect
    {

        public Rect rect;//位置信息
        public bool isVisable = false;//是否显示
        public object data;
        public BaseItemView item;
    }
}
