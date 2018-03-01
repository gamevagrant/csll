using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDungeonCardsPoolPanel : MonoBehaviour {

    public GridBaseScrollView bigScrollView;
    public GridBaseScrollView smallScrollView;

	// Use this for initialization
	void Start () {
		
	}
	
	public void SetData(DungeonCardData[] bigCards,DungeonCardData[] smallCards)
    {
        bigScrollView.SetData(bigCards);
        smallScrollView.SetData(smallCards);
    }

    
}
