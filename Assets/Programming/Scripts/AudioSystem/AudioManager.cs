using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioManager : MonoBehaviour
{
    protected List<Sound> _sounds;
    protected List<AudioSource> _sources;

    bool _bBypass = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _sounds = new List<Sound>();
        _sources = new List<AudioSource>();

        Initialize();
    }

    public abstract void Initialize();

    public void BypassToggle(bool b)
    {
        _bBypass = b;
    }
}
