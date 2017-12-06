using System.Collections;
using System.Collections.Generic;
using QY.CrossPlatformInput;
using UnityEngine;
using UnityEngine.EventSystems;

public class StandaloneInputHandle : MonoBehaviour
{

    private Vector3 downPos;


	// Update is called once per frame
	void Update () 
	{

        if(Input.GetMouseButtonDown(0))
        {
            downPos = Input.mousePosition;

        }else if(Input.GetMouseButtonUp(0))
        {
            if(downPos != Vector3.zero)
            {
                Vector3 move = Input.mousePosition - downPos;
                if(Mathf.Abs(move.x)> Mathf.Abs(move.y))
                {
                    if(move.x>0)
                    {
                        CrossPlatformInputManager.SetButtonDown(CrossPlatformInput.RIGHT);
                    }else
                    {
                        CrossPlatformInputManager.SetButtonDown(CrossPlatformInput.LEFT);
                    }
                }else
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

            downPos = Vector3.zero;
        }
		
		
	}
}
