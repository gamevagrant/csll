using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDungeonContent : MonoBehaviour {

    public RewardItem[] items; 
	// Use this for initialization
	void Start () {
		
	}
	
	public void SetData(RewardData[] rewards)
    {
        for(int i=0;i< items.Length;i++)
        {
            if(i<rewards.Length)
            {
                items[i].gameObject.SetActive(true);
                items[i].SetData(rewards[i]);
            }else
            {
                items[i].gameObject.SetActive(false);
            }
        }
    }
}
