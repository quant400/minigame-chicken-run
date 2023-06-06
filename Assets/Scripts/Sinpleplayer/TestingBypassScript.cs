using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChickenRun
{
    public class TestingBypassScript : MonoBehaviour
    {
        [SerializeField]
        GameObject skipButton;
        int pressed = 0;

        public void ByPass()
        {
            if (pressed < 10)
                pressed++;
            else if (pressed >= 10)
            {
                skipButton.SetActive(true);
                gameObject.SetActive(false);
            }

        }
    }
}