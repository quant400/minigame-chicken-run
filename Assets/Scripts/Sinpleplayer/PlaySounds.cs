using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChickenRun
{
    public class PlaySounds : MonoBehaviour
    {
        public static PlaySounds instance;
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;

            }
        }
        public void Play()
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
