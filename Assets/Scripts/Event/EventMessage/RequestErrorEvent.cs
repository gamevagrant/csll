using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;

public class RequestErrorEvent : BaseEvent {

    public HTTPRequest request;
    public HTTPResponse response;

    public RequestErrorEvent(HTTPRequest request, HTTPResponse response) :base(EventEnum.REQUEST_ERROR)
    {
        this.request = request;
        this.response = response;
    }
}
