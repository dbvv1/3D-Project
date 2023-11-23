using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MultiAudioDefination : MonoBehaviour
{
    public string audioDescription;

    public List<AudioClip> clips;

    public AudioClip PlayClipRandom()
    {
        return clips[Random.Range(0, clips.Count)];
    }

}
