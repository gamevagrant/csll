using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UGUISpriteAnimation : MonoBehaviour {

    private Image ImageSource;
    private RectTransform rectTransform;
    private int mCurFrame = 0;
    private float mDelta = 0;

    public float FPS = 5;
    public List<Sprite> SpriteFrames;
    public bool IsPlaying = false;
    public bool Foward = true;
    public bool AutoPlay = false;
    public bool Loop = false;

    public int FrameCount
    {
        get
        {
            return SpriteFrames.Count;
        }
    }

    void Awake()
    {
        ImageSource = GetComponent<Image>();
        rectTransform = transform as RectTransform;
    }

    void OnEnable()
    {
        if (AutoPlay)
        {
            Play();
        }
        else
        {
            IsPlaying = false;
        }
    }

    private void SetSprite(int idx)
    {
        ImageSource.sprite = SpriteFrames[idx];
        if(rectTransform.anchorMin == rectTransform.anchorMax)
        {
            ImageSource.SetNativeSize();
        }
        
    }

    public void Play()
    {
        SetSprite(mCurFrame);
        ImageSource.enabled = true;
        IsPlaying = true;
        Foward = true;
    }

    public void PlayReverse()
    {
        IsPlaying = true;
        Foward = false;
    }

    void Update()
    {
        if (!IsPlaying || 0 == FrameCount)
        {
            return;
        }

        mDelta += Time.deltaTime;
        if (mDelta > 1 / FPS)
        {
            mDelta = 0;
            if (Foward)
            {
                mCurFrame++;
            }
            else
            {
                mCurFrame--;
            }

            if (mCurFrame >= FrameCount)
            {
                if (Loop)
                {
                    mCurFrame = 0;
                }
                else
                {
                    Stop();
                    return;
                }
            }
            else if (mCurFrame < 0)
            {
                if (Loop)
                {
                    mCurFrame = FrameCount - 1;
                }
                else
                {
                    Stop();
                    return;
                }
            }

            SetSprite(mCurFrame);
        }
    }

    public void Pause()
    {
        IsPlaying = false;
    }

    public void Resume()
    {
        if (!IsPlaying)
        {
            IsPlaying = true;
        }
    }

    public void Stop()
    {
        mCurFrame = 0;
        ImageSource.enabled = false;
        IsPlaying = false;
    }

    public void Rewind()
    {
        mCurFrame = 0;
        SetSprite(mCurFrame);
        Play();
    }
}
