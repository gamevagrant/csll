using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventDispatcher :IEventDispatcher{

	private static EventDispatcher _instance;
	public static EventDispatcher instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new EventDispatcher();
			}
			return _instance;
		}
	}
	private Dictionary<string, EventHandle> dicEvent;

	public EventDispatcher()
	{
		dicEvent = new Dictionary<string, EventHandle>();
	}

	public void addEventListener(string type, EventHandle listener)
	{

		if (dicEvent.ContainsKey(type))
		{
			//Debug.Log("EventDispatcher:+++"+this.ToString());
			dicEvent[type] += listener;

		}
		else
		{
			//Debug.Log("EventDispatcher:add"+this.ToString());
			dicEvent.Add(type, listener);

		}
	}

	public void removeEventListener(string type, EventHandle listener)
	{

		if (dicEvent.ContainsKey(type))
		{
			//Debug.Log("EventDispatcher：---"+this.ToString());
			dicEvent[type] -= listener;
			if (dicEvent[type] == null)
			{
				//Debug.Log("EventDispatcher"+this.ToString());
				dicEvent.Remove(type);
			}
		}
	}

	public bool dispatchEvent(string type,params object[] datas)
	{
		if (dicEvent.ContainsKey(type) && dicEvent[type] != null)
		{
			EventHandle fun = dicEvent[type];
			fun(datas);
			return true;
		}

		return false;
	}

	public void clearEvent()
	{
		dicEvent.Clear();
	}
}
