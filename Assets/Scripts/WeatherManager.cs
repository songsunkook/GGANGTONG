using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public ParticleSystem rain;
    public string rainSound;

    AudioManager theAudio;

    bool isRunning = false;

    private void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }


    public void Rain()
    {
        if (isRunning)
            return;
        isRunning = true;

        theAudio.Play(rainSound);
        StartCoroutine(RainDrop());
    }

    public void RainStop()
    {
        theAudio.Stop(rainSound);
        rain.Stop();
    }

    IEnumerator RainDrop()
    {
        for(int i = 1; i < 10; i ++)
        {
            rain.Emit(i);
            theAudio.SetVolume(rainSound, i * 0.01f);
            yield return new WaitForSeconds(0.2f);
        }
        theAudio.SetVolume(rainSound, theAudio.GetSound(rainSound).maxVolume);
        rain.Play();
        isRunning = false;
    }

}
