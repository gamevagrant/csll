public delegate void EventHandle(params object[] datas);
public interface IEventDispatcher
{
	void addEventListener(string type, EventHandle listener);

	void removeEventListener(string type, EventHandle listener);

	bool dispatchEvent(string type, params object[] datas);
}
