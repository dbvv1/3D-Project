using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAudioDefinition : MonoBehaviour
{
    public AudioClip clip;

    public bool isPlayer;

    public bool isBgm;

    public bool playOnEnable;

    private void OnEnable()
    {
        if(playOnEnable)
        {
            if (isBgm) AudioManager.Instance.PlayGeneralBGM(clip);
            else AudioManager.Instance.PlayGeneralFX(clip);
        }
    }

    public void PlayClip()
    {
        if (isBgm) AudioManager.Instance.PlayGeneralBGM(clip);
        else AudioManager.Instance.PlayGeneralFX(clip);
    }

}
