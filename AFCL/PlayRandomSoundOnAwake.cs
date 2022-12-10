using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;
using System.Collections;

namespace AndrewFTW
{ 
    public class PlayRandomSoundOnAwake : MonoBehaviour
    {
        public AudioClip[] AudioClips;
        public AudioSource AudioSource;

#if !(UNITY_EDITOR || UNITY_5)

        public void OnEnable()
        {
            int _radomVal = UnityEngine.Random.Range(0, AudioClips.Length);

            Debug.Log("clip " + _radomVal + " named " + AudioClips[_radomVal].name);

            AudioSource.clip = AudioClips[_radomVal];
            AudioSource.Play();
        }



#endif
    }
}



