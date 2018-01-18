using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class GridBaseScrollView : MonoBehaviour {

    public enum Direction
    {
        Horizontal,
        Vertical,
    }

    public RectTransform itemTemplate;
    public Direction direction = Direction.Vertical;
    public float spacing = 10;//间距

    private ScrollRect scrollRect;
    private RectTransform content;

    private GameObjectPool itemPool;
    private List<ItemRect> itemDatas;
    //所有的rect显示区域的锚点都在左上角
    private Rect displayRect;//显示区域
    private int row;
    private int col;

    private void Awake()
    {
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
        content.anchorMin = new Vector2(0, 1);
        content.anchorMax = new Vector2(0, 1);
        content.pivot = new Vector2(0, 1);
        row = (int)(displayRect.height / itemTemplate.sizeDelta.y);
        col = (int)(displayRect.width / itemTemplate.sizeDelta.x); 
        scrollRect.onValueChanged.AddListener(ListenerMethod);
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
        itemDatas = new List<ItemRect>();
        Vector2 offset = Vector2.zero;
        for (int i = 0; i < datas.Count; i++)
        {
           
            if (direction == Direction.Vertical)
            {
                offset = new Vector2(i % col, i / col);
            }else
            {
                offset = new Vector2(i / row, i % row);
            }
           

            ItemRect item = new ItemRect();
            item.rect = new Rect(offset.x * (itemTemplate.rect.width + spacing) + spacing, -offset.y * (itemTemplate.rect.height + spacing) - spacing, itemTemplate.rect.width, itemTemplate.rect.height);
            item.data = datas[i];
            item.isVisable = false;
            itemDatas.Add(item);

        }


        content.sizeDelta = new Vector2((offset.x + 1) * (itemTemplate.rect.width + spacing), (offset.y + 1) * (itemTemplate.rect.height + spacing));
        if (direction == Direction.Vertical)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }
        else
        {
            scrollRect.horizontalNormalizedPosition = 1;
        }
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
                    (item.item.transform as RectTransform).anchoredPosition = new Vector2(item.rect.x, item.rect.y);
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
    private void ListenerMethod(Vector2 value)
    {
        float startPointY = (1 - value.y) * (content.rect.height - displayRect.height);
        float startPointX = value.x * (content.rect.width - displayRect.width);
        displayRect.y = -startPointY;
        displayRect.x = startPointX;

        updateItems();
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
