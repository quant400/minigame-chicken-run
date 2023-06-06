using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChickenRun
{
    public class MinimapFollow : MonoBehaviour
    {
        [SerializeField]
        Transform player;
        float storedShadowDistance;
        private void LateUpdate()
        {
            if (player == null && !gameplayView.instance.ended)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }

            if (player != null)
            {
                transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            }

        }
        /*
        void OnPreRender()
        {
            storedShadowDistance = QualitySettings.shadowDistance;
            QualitySettings.shadowDistance = 0;
        }

        void OnPostRender()
        {
            QualitySettings.shadowDistance = storedShadowDistance;
        }
        */
    }
}