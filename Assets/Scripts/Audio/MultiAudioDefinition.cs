using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiAudioDefination : MonoBehaviour
{
    public string audioDiscription;

    public List<AudioClip> clips;

    public AudioClip PlayClipRandom()
    {
        return clips[Random.Range(0, clips.Count)];
    }

}
