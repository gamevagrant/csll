using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;

public class RequestErrorEvent : BaseEvent {

    public enum Type
    {
        TimeOut,
        Error,
        Other,
        AnalysisError,
    }

    public HTTPRequest request;
    public Type type;

    public RequestErrorEvent(Type type ,HTTPRequest request) :base(EventEnum.REQUEST_ERROR)
    {
        this.request = request;
        this.type = type;
    }
}
