using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChickenRun.StarterAssets;
namespace ChickenRun
{
    public class MoonLevelSettings : MonoBehaviour
    {
        GameObject player;
        [SerializeField]
        float jumpmultiplyer;
        private void Start()
        {
            Invoke("ChangeSettings", 0.5f);

        }
        void ChangeSettings()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.transform.GetChild(0).GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            player.GetComponent<ThirdPersonController>().JumpHeight *= jumpmultiplyer;
        }
    }
}
