using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChickenRun
{
    public class AndroidLoginskipper : MonoBehaviour
    {
        [SerializeField]
        GameObject tryoutCanvas;
        [SerializeField]
        GameObject methodSelect;


        public void OpenMethodSelect()
        {
            methodSelect.SetActive(true);
        }
        public void Skip()
        {
            gameplayView.instance.isTryout = true;
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(false);
            }
            transform.GetChild(0).gameObject.SetActive(true);
            tryoutCanvas.SetActive(true);
        }

    }
}
