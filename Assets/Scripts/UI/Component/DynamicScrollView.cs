using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class DynamicScrollView : MonoBehaviour {

	public enum Direction
	{
		Horizontal,
		Vertical,
	}

	public ScrollRect scrollRect;
	public RectTransform itemTemplate;
	public RectTransform content;
	[SerializeField, Range(1, 3)]
	public float buffer = 2;//相对于显示区域，两边缓冲区域大小

	public Direction direction = Direction.Vertical;

    public float spacing = 30;//间距
    public int row = 3;
    public int col = 6;
    public Vector2 itemSize = new Vector2(512, 512);

	private GameObjectPool itemPool; 
	private List<ItemRect> itemDatas;

	//所有的rect显示区域的锚点都在左上角
	private Rect displayRect;//显示区域
	private int allPage;

	public float normalizedPosition
	{
		get
		{
			float v = 0;
			switch (direction)
			{
				case Direction.Horizontal:
					v = displayRect.x / (content.rect.width - displayRect.width);
					break;
				case Direction.Vertical:
					v = Mathf.Abs(displayRect.y / (content.rect.height - displayRect.height));
					break;
			}
			return v;
		}
		set
		{
			switch (direction)
			{
				case Direction.Horizontal:
					if (content.rect.width - displayRect.width == 0)
					{
						scrollRect.horizontalNormalizedPosition = 0;
					}
					else
					{
						//float f = value / (content.rect.width - displayRect.width);
						scrollRect.horizontalNormalizedPosition = value;
					}
					break;
				case Direction.Vertical:
					if (content.rect.height - displayRect.height == 0)
					{
						scrollRect.verticalNormalizedPosition = 0;
					}
					else
					{
						//float f = value / (content.rect.height - displayRect.height);
						scrollRect.verticalNormalizedPosition = 1 - value;
					}
					break;
			}
		}
	}

	private int currentPage
	{
		get
		{
			if (direction == Direction.Horizontal)
			{
				return (int)((displayRect.x + displayRect.width / 2) / displayRect.width);
			}
			else
			{
				return Mathf.Abs((int)((displayRect.y - displayRect.height / 2) / displayRect.height));
			}
			
		}
		set
		{
			float p = 0;
			if (direction == Direction.Horizontal)
			{
				p = value * displayRect.width / (content.rect.width - displayRect.width);
			}
			else
			{
				p = value * displayRect.height / (content.rect.height - displayRect.height);
			}

			DOTween.To(() => normalizedPosition, (x) => normalizedPosition = x, p, 2f).SetEase(Ease.OutCubic);
		}
	}



	void Awake()
	{
		itemPool = content.gameObject.AddComponent<GameObjectPool>();
		itemPool.target = itemTemplate.gameObject;
		itemTemplate.gameObject.SetActive(false);

		displayRect = new Rect(0,0,(itemSize.x + spacing) * col,(itemSize.y + spacing) * row);


		itemTemplate.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemSize.x);
		itemTemplate.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemSize.y);

        scrollRect.onValueChanged.AddListener(ListenerMethod);
        //(transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, displayRect.width);
        //(transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, displayRect.height);
    }




	public void nextRow()
	{
		int r = Mathf.RoundToInt(Mathf.Abs(displayRect.y / itemSize.y)) + 1;
		int allRow = itemDatas.Count / col + 1;
		if (r <= allRow - row)
		{
			float p = r * (itemSize.y + spacing) / (content.rect.height - displayRect.height);
			DOTween.To(() => normalizedPosition, (x) => normalizedPosition = x, p, 0.5f).SetEase(Ease.OutCubic);
		}
	}

	public void previousRow()
	{
		int r = Mathf.RoundToInt(Mathf.Abs(displayRect.y / itemSize.y)) - 1;
		if (r >=0)
		{
			float p = r * (itemSize.y + spacing) / (content.rect.height - displayRect.height);
			DOTween.To(() => normalizedPosition, (x) => normalizedPosition = x, p, 0.5f).SetEase(Ease.OutCubic);
		}
	}

	public void nextPage()
	{
		int page = currentPage + 1;
		if (page<allPage)
		{
			//float value = page * displayRect.width / (content.rect.width - displayRect.width);

			//DOTween.To(() => scrollRect.horizontalNormalizedPosition, (x) => scrollRect.horizontalNormalizedPosition = x, value, 0.5f).SetEase(Ease.OutCubic);
			currentPage = page;
		}
		
	}
	public void previousPage()
	{
		int page = currentPage - 1;
		if (page>=0)
		{
			//float value = page * displayRect.width / (content.rect.width - displayRect.width);

			//DOTween.To(() => scrollRect.horizontalNormalizedPosition, (x) => scrollRect.horizontalNormalizedPosition = x, value, 0.5f).SetEase(Ease.OutCubic);
			currentPage = page;
		}
		
		
	}

	public void ListenerMethod(Vector2 value)
	{
		if (direction == Direction.Vertical)
		{
			float startPointY = (1 - value.y) * (content.rect.height - displayRect.height);
			displayRect.y = -startPointY;
		}
		else
		{
			float startPointX = value.x * (content.rect.width - displayRect.width);
			displayRect.x = startPointX;
		}
		
		
		updateItems();
	}

	public void setDatas(IList datas)
	{
		itemPool.resetAllTarget();
        if (datas == null)
            return;

		displayRect.x = 0;
		displayRect.y = 0;
		itemDatas = new List<ItemRect>();
		if (direction == Direction.Horizontal)
		{
			for (int i = 0; i < datas.Count; i++)
			{
				ItemRect item = new ItemRect();
				float x = i / row;
				float y = i % row;

				Debug.Log(x.ToString() + "|" + y.ToString());

				item.rect = new Rect(x * (itemTemplate.rect.width + spacing)+spacing, -y * (itemTemplate.rect.height + spacing) - spacing, itemTemplate.rect.width, itemTemplate.rect.height);
				item.data = datas[i];
				item.isVisable = false;
				itemDatas.Add(item);
			}
			scrollRect.horizontal = true;
			scrollRect.vertical = false;
			allPage = itemDatas.Count / (row * col) + 1;
			content.anchorMin = new Vector2(0, 0);
			content.anchorMax = new Vector2(0, 1);
			content.anchoredPosition = new Vector2(0,0);
			content.sizeDelta = new Vector2(allPage * displayRect.width, 0);
			scrollRect.horizontalNormalizedPosition = 0;
		}
		else
		{
			for (int i = 0; i < datas.Count; i++)
			{
				ItemRect item = new ItemRect();
				float x = i % col;
				float y = i / col;

				//Debug.Log(x.ToString() + "|" + y.ToString());

				item.rect = new Rect(x * (itemTemplate.rect.width + spacing) + spacing, -y * (itemTemplate.rect.height + spacing) - spacing, itemTemplate.rect.width, itemTemplate.rect.height);
				item.data = datas[i];
				item.isVisable = false;
				itemDatas.Add(item);
			}

			scrollRect.vertical = true;
			scrollRect.horizontal = false;
			allPage = itemDatas.Count / (row * col) + 1;
			////content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, content);
			content.anchorMin = new Vector2(0,1);
			content.anchorMax = new Vector2(1,1);
			//content.anchoredPosition = new Vector2(0,0);
			content.sizeDelta = new Vector2(0, allPage * displayRect.height);
			scrollRect.verticalNormalizedPosition = 1;

		}
		
		updateItems();
	}



	private void updateItems()
	{
        if (itemDatas == null)
            return;
		//Rect rect = new Rect(displayRect.x - displayRect.width*(buffer - 1)/2,displayRect.y + displayRect.height*(buffer - 1)/2, displayRect.width*buffer, displayRect.height*buffer);
		Rect rect = new Rect(displayRect.x - displayRect.width * (buffer - 1) / 2, displayRect.y + displayRect.height * (buffer - 1) / 2, displayRect.width * buffer, displayRect.height * buffer);

		for (int i = 0; i < itemDatas.Count; i++)
		{
			ItemRect item = itemDatas[i];
			if (isIntersect(item.rect, rect))
			{
				if (!item.isVisable)
				{
					item.isVisable = true;
					item.item = itemPool.getIdleTarget<BaseItemView>();
					item.item.transform.localPosition = new Vector3(item.rect.x,item.rect.y,0);
					item.item.SetData(item.data);
					

				}
				else
				{
					float f = getIntersectSize(item.rect, displayRect);
					//f = Mathf.Min(1, f + 0.3f);
					item.item.transform.localPosition = new Vector3(item.rect.x - (1-f) * item.rect.width/2, item.rect.y , 0);
	
	
					//item.item.transform.localScale = f * Vector3.one;
					if (f < 1)
					{
						item.item.transform.SetSiblingIndex(0);
					}
					else
					{
						item.item.transform.SetSiblingIndex(100);
					}
				}
			}
			else
			{
				if (item.isVisable)
				{
					item.isVisable = false;
					if (item.item!=null)
					{
						item.item.gameObject.SetActive(false);
						item.item = null;
					}
					
				}
			}
		}
		
	}

	private float getIntersectSize(Rect a, Rect b)
	{
		if (direction == Direction.Vertical)
		{
			if (a.y<b.y-b.height/2)//下半部
			{
				float y = ((b.y - b.height) - a.y )/a.height;
				if (y < 0)
				{
					return Mathf.Min(-y,1) ;
				}
			}
			else
			{
				float y = (b.y  - (a.y - a.height)) / a.height;
				if (y > 0)
				{
					return Mathf.Min(y,1);
				}
			}
			
		}

		return 0;
	}

	//判断两个区域是否相交
	private bool isIntersect(Rect a,Rect b)
	{

		if (a.x+a.width<b.x || a.x > b.x + b.width || a.y-a.height>b.y || a.y<b.y-b.height)
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
