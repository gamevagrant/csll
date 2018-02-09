using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDungeonCardBoard : MonoBehaviour {

    public UIDungeonCard[] cards;
    public QY.UI.Button btn;

	// Use this for initialization
	void Start () {
		
	}
	
	public void SetData(DungeonCardData[] data)
    {
        for(int i = 0;i<cards.Length;i++)
        {
            if(i<data.Length)
            {
                DungeonCardData cd = data[i];
                cards[i].SetData(cd);
            }else
            {
                cards[i].SetData(null);
            }
        }

        btn.interactable = data.Length == cards.Length;
    }

}
