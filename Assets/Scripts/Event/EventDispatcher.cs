using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
	private Dictionary<string, Action<BaseEvent>> dicEvent;

	public EventDispatcher()
	{
		dicEvent = new Dictionary<string, Action<BaseEvent>>();
	}

	public void AddEventListener(string type, Action<BaseEvent> listener)
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

	public void RemoveEventListener(string type, Action<BaseEvent> listener)
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

	public bool DispatchEvent(string type,params object[] datas)
	{
		if (dicEvent.ContainsKey(type) && dicEvent[type] != null)
		{
            Action<BaseEvent> fun = dicEvent[type];
            BaseEvent evt = new BaseEvent(type,datas);
			fun(evt);
			return true;
		}

		return false;
	}

    public bool DispatchEvent(BaseEvent evt)
    {
        if (dicEvent.ContainsKey(evt.type) && dicEvent[evt.type] != null)
        {
            Action<BaseEvent> fun = dicEvent[evt.type];
            fun(evt);
            return true;
        }

        return false;
    }

	public void clearEvent()
	{
		dicEvent.Clear();
	}
}
