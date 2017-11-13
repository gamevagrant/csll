using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 支持多个列表的纵向排列scrollview，每个子列表有一个名字
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public class VerticalScrollView : MonoBehaviour {

    public RectTransform itemTemplate;
    public RectTransform listNameTemplate;
    public float spacing = 10;//间距

    private ScrollRect scrollRect;
    private RectTransform content;

    private GameObjectPool itemPool;
    private GameObjectPool namePool;
    private List<SubListData> listData;
    private List<ItemRect> itemDatas;
    //所有的rect显示区域的锚点都在左上角
    private Rect displayRect;//显示区域

    private void Awake()
    {
        scrollRect = gameObject.GetComponent<ScrollRect>();
        content = scrollRect.content;
        displayRect = new Rect(0,0,scrollRect.viewport.rect.width, scrollRect.viewport.rect.height);

        itemPool = content.gameObject.AddComponent<GameObjectPool>();
        itemPool.target = itemTemplate.gameObject;
        itemTemplate.gameObject.SetActive(false);
        itemTemplate.pivot = new Vector2(0, 1);
        itemTemplate.anchorMin = new Vector2(0,1);
        itemTemplate.anchorMax = new Vector2(0, 1);

        namePool = content.gameObject.AddComponent<GameObjectPool>();
        namePool.target = listNameTemplate.gameObject;
        listNameTemplate.gameObject.SetActive(false);
        listNameTemplate.pivot = new Vector2(0, 1);
        listNameTemplate.anchorMin = new Vector2(0, 1);
        listNameTemplate.anchorMax = new Vector2(0, 1);

        scrollRect.onValueChanged.AddListener(ListenerMethod);
    }

    private void OnDestroy()
    {
        scrollRect.onValueChanged.RemoveAllListeners();
    }

    public void SetData(List<SubListData> datas)
    {
        if (datas == null)
            return;

        itemPool.resetAllTarget();
        namePool.resetAllTarget();
        displayRect.x = 0;
        displayRect.y = 0;
        itemDatas = new List<ItemRect>();

        float y = 0;
        for (int i = 0; i < datas.Count; i++)
        {
            SubListData subList = datas[i];
            if (!string.IsNullOrEmpty(subList.name))
            {
                ItemRect nameItem = new ItemRect();
                nameItem.rect = new Rect(0, y, listNameTemplate.rect.width, listNameTemplate.rect.height);
                nameItem.data = subList.name;
                nameItem.isVisable = false;
                nameItem.type = ItemRect.Type.Name;
                itemDatas.Add(nameItem);
                y -= listNameTemplate.rect.height + spacing;
            }

            for(int j = 0;j< subList.list.Count;j++)
            {
                ItemRect item = new ItemRect();
                item.rect = new Rect(0, y, itemTemplate.rect.width, itemTemplate.rect.height);
                item.data = subList.list[j];
                item.type = ItemRect.Type.Item;
                item.isVisable = false;
                itemDatas.Add(item);
                y -= itemTemplate.rect.height + spacing;
            }
        }

        scrollRect.vertical = true;
        scrollRect.horizontal = false;
        content.anchorMin = new Vector2(0, 1);
        content.anchorMax = new Vector2(1, 1);
        content.sizeDelta = new Vector2(0, -y);
        scrollRect.verticalNormalizedPosition = 1;
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
                    if(item.type == ItemRect.Type.Name)
                    {
                        item.item = namePool.getIdleTarget<BaseItemView>();
                    }else
                    {
                        item.item = itemPool.getIdleTarget<BaseItemView>();
                    }
                    item.isVisable = true;
                    (item.item.transform as RectTransform).anchoredPosition = new Vector2(item.rect.x, item.rect.y);
                    item.item.SetData(item.data);
                }
            }else
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
        displayRect.y = -startPointY;
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
        public enum Type
        {
            Name,//列表名字的标签
            Item,
        }
        public Rect rect;//位置信息
        public bool isVisable = false;//是否显示
        public object data;
        public Type type;//对象的类型
        public BaseItemView item;
    }
}

/// <summary>
/// 子列表数据
/// </summary>
public class SubListData
{
    public string name;
    public List<object> list;

    public SubListData(string name, List<object> list)
    {
        this.name = name;
        this.list = list;
    }
}
