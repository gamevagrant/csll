using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDungeonProgress : MonoBehaviour {

    public UIDungeonRewardIcon[] icones;
    public Slider slider;
    public UIDungeonContent content;

    private RewardData[][] rewards;
    // Use this for initialization
    void Start ()
    {
        content.gameObject.SetActive(false);

        for (int i =0;i<icones.Length;i++)
        {
            icones[i].onPress = OnShowReward;
        }
	}

    private void OnDestroy()
    {
        for (int i = 0; i < icones.Length; i++)
        {
            icones[i].onPress = null;
        }
    }

    public void SetData(float progress,RewardData[][] rewards,int rewardIndex)
    {
        this.rewards = rewards;
        slider.value = progress;
        for(int i=0;i< icones.Length;i++)
        {
            icones[i].interactable = i == rewardIndex;
        }
    }

    public void OnShowReward(int index,bool isShow)
    {
        if(isShow)
        {
            content.gameObject.SetActive(true);
            if (index < icones.Length)
            {
                content.transform.position = icones[index].transform.position;
                content.SetData(rewards[rewards.Length - index - 1]);
            }
        }else
        {
            content.gameObject.SetActive(false);
        }
       
       
    }
}
