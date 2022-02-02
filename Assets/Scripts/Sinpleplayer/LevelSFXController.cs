using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSFXController : MonoBehaviour
{
    AudioSource SFX;
    AudioSource music;

    float defaultMusicVol;
    float defaultSFXVol;
    private void Start()
    {
        SFX = transform.GetChild(0).GetComponent<AudioSource>();
        music = transform.GetChild(1).GetComponent<AudioSource>();
        defaultMusicVol = music.volume;
        defaultSFXVol = SFX.volume;
    }


    public void MuteSFX()
    {
        if (gameplayView.instance.GetSFXMuted())
        {
            SFX.volume = defaultMusicVol;
        }
        else
        {
            SFX.volume = 0;
        }

        gameplayView.instance.SetSFXMuted(!gameplayView.instance.GetSFXMuted());
    }

    public void MuteMusic()
    {
        if(music.volume==0)
        {
            music.volume = defaultMusicVol;
        }
        else
        {
            music.volume = 0;
        }
    }
}
