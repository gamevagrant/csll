using System;

//public delegate void EventHandle(params object[] datas);
public interface IEventDispatcher
{
	//void AddEventListener(string type, EventHandle listener);
    void AddEventListener(string type, Action<BaseEvent> listener);

    void RemoveEventListener(string type, Action<BaseEvent> listener);

	bool DispatchEvent(string type, params object[] datas);
    bool DispatchEvent(BaseEvent evt);
}
