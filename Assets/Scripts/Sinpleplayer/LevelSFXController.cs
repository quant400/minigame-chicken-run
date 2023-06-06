using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ChickenRun
{
    public class LevelSFXController : MonoBehaviour
    {
        AudioSource SFX;
        AudioSource music;

        [SerializeField]
        Image sfxButton, musicButton;
        float defaultMusicVol;
        float defaultSFXVol;
        Sprite regularImage;
        [SerializeField]
        Sprite disableImage;
        private void Start()
        {
            SFX = transform.GetChild(0).GetComponent<AudioSource>();
            music = transform.GetChild(1).GetComponent<AudioSource>();
            defaultMusicVol = music.volume;
            defaultSFXVol = SFX.volume;
            regularImage = sfxButton.sprite;
            //Debug.Log((PlayerPrefs.GetString("SFX"), PlayerPrefs.GetString("Music"), gameplayView.instance.GetSFXMuted()));
            if (PlayerPrefs.HasKey("Music"))
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
            if (SFX.volume == 0)
            {
                SFX.volume = defaultMusicVol;
                sfxButton.sprite = regularImage;// new Color(0.9450981f, 0.1215686f, 0.172549f, 1f);
                sfxButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
                PlayerPrefs.SetString("SFX", "on");
                gameplayView.instance.SetSFXMuted(false);
            }
            else
            {
                sfxButton.sprite = disableImage;
                sfxButton.transform.GetChild(0).GetComponent<Image>().color = new Color(0.9450981f, 0.1215686f, 0.172549f, 1f);
                SFX.volume = 0;
                PlayerPrefs.SetString("SFX", "off");
                gameplayView.instance.SetSFXMuted(true);
            }

        }

        public void MuteMusic()
        {
            if (music.volume == 0)
            {
                musicButton.sprite = regularImage;//new Color(0.9450981f, 0.1215686f, 0.172549f, 1f);
                musicButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                music.volume = defaultMusicVol;
                PlayerPrefs.SetString("Music", "on");
            }
            else
            {
                musicButton.sprite = disableImage;
                musicButton.transform.GetChild(0).GetComponent<Image>().color = new Color(0.9450981f, 0.1215686f, 0.172549f, 1f);
                music.volume = 0;
                PlayerPrefs.SetString("Music", "off");
            }
        }
    }
}