using System.Collections;
using System.Collections.Generic;
using qy.CrossPlatformInput;
using UnityEngine;


public class MobileInputHandle : MonoBehaviour
{
    private Vector2 downPos;

    // Update is called once per frame
    void Update()
	{
        if(Input.touchCount>0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                downPos = Input.GetTouch(0).position;
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Vector2  move = Input.GetTouch(0).position - downPos;
                if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
                {
                    if (move.x > 0)
                    {
                        CrossPlatformInputManager.SetButtonDown(CrossPlatformInput.RIGHT);
                    }
                    else
                    {
                        CrossPlatformInputManager.SetButtonDown(CrossPlatformInput.LEFT);
                    }
                }
                else
                {
                    if (move.y > 0)
                    {
                        CrossPlatformInputManager.SetButtonDown(CrossPlatformInput.UP);
                    }
                    else
                    {
                        CrossPlatformInputManager.SetButtonDown(CrossPlatformInput.DOWN);
                    }
                }
            }
        }
	}
}

