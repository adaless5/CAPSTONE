using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound
{
    public AudioClip _clip;
    public string _name;
    public AudioMixerGroup _mixerGroup;
    public float _volume;
    public float _pitch;

    public Sound(AudioClip clip, string name, AudioMixerGroup mixerGroup, float volume = 1f, float pitch = 1f)
    {
        _clip = clip;
        _name = name;
        _mixerGroup = mixerGroup;
        _volume = volume;
        _pitch = pitch;
    }
}
