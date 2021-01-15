using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // 사운드 이름

    public AudioClip[] clip; // 사운드 파일
    AudioSource source;

    public float maxVolume;
    public float volume;
    public bool loop;
    public bool redo;
    
    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.volume = volume;
        source.loop = loop;
    }

    public void SetVolume()
    {
        source.volume = volume;
    }

    public void Play()
    {
        if (!source.isPlaying || redo)
        {
            int temp = Random.Range(0, clip.Length);
            source.clip = clip[temp];
            source.Play();
        }
    }

    public bool IsPlaying()
    {
        return source.isPlaying;
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;

    static public AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 : " + i + " = " + sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
    }

    public void Play(string _name)
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            if(_name == sounds[i].name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public bool IsPlaying(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                return true;
            }
        }
        return false;
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
            }
        }
    }

    public void SetVolume(string _name,float _volume)
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            if(_name == sounds[i].name)
            {
                sounds[i].volume = _volume;
                sounds[i].SetVolume();
                return;
            }
        }
    }

    public Sound GetSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                return sounds[i];
            }
        }

        return null;
    }

}
