using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPlatformInput : MonoBehaviour
{

	public const string AXIS_HORIZONTAL = "AXIS_HORIZONTAL";
	public const string AXIS_VERTICAL = "AXIS_VERTICAL";
	public const string LEFT = "LEFT";
    public const string RIGHT = "RIGHT";
    public const string UP = "UP";
    public const string DOWN = "DOWN";
    void Awake () 
	{
#if UNITY_EDITOR
		gameObject.AddComponent<StandaloneInputHandle>();
#elif UNITY_ANDROID
		gameObject.AddComponent<MobileInputHandle>();
#elif UNITY_IPHONE
		gameObject.AddComponent<MobileInputHandle>();
#endif
    }

}
