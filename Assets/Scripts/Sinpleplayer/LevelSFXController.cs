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

        if(PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetString("Music") == "off")
            {
                MuteMusic();
            }
        }

        if (PlayerPrefs.HasKey("SFX"))
            {
            if (PlayerPrefs.GetString("SFX") == "off")
            {
                MuteSFX();
            }
        }
    }


    public void MuteSFX()
    {
        if (gameplayView.instance.GetSFXMuted())
        {
            SFX.volume = defaultMusicVol;
            PlayerPrefs.SetString("SFX", "on");
        }
        else
        {
            SFX.volume = 0;
            PlayerPrefs.SetString("SFX", "off");
        }

        gameplayView.instance.SetSFXMuted(!gameplayView.instance.GetSFXMuted());
    }

    public void MuteMusic()
    {
        if(music.volume==0)
        {
            music.volume = defaultMusicVol;
            PlayerPrefs.SetString("Music", "on");
        }
        else
        {
            music.volume = 0;
            PlayerPrefs.SetString("Music", "off");
        }
    }
}
