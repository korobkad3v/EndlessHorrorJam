using UnityEngine;
using System;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SOUNDS & MUSIC")]
    public SFX[] sounds;
    public SFX[] music;


    private SFX[] all;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        all = sounds.Concat(music).ToArray();

        foreach (SFX s in all)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    public void Play(string name)
    {

        SFX s = Array.Find(all, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        s.source.Play();
    }

    private void Start()
    {
        Play("ambient_0");
        Play("ambient_1");
    }
}
