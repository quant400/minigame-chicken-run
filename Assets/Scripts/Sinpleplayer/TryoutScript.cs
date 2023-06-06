using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ChickenRun
{
    public class TryoutScript : MonoBehaviour
    {
        [SerializeField]
        GameObject startscreen;
        [SerializeField]
        NFTGetView nftGetter;
        public void ButtonPressed(int i)
        {
            switch (i)
            {
                case 0:
                    nftGetter.Skip();
                    SceneManager.LoadScene(chickenGameModel.singlePlayerScene1.sceneName);
                    break;
                case 1:
                    nftGetter.Skip();
                    SceneManager.LoadScene(chickenGameModel.singlePlayerScene2.sceneName);
                    break;
                case 2:
                    nftGetter.Skip();
                    SceneManager.LoadScene(chickenGameModel.singlePlayerScene3.sceneName);
                    break;
                case 3:
                    nftGetter.Skip();
                    SceneManager.LoadScene(chickenGameModel.singlePlayerScene4.sceneName);
                    break;
                case 4:
                    nftGetter.Skip();
                    SceneManager.LoadScene(chickenGameModel.singlePlayerScene5.sceneName);
                    break;
            }
            transform.gameObject.SetActive(false);

        }

        public void BackButton()
        {
            gameObject.SetActive(false);
            startscreen.SetActive(true);
            gameplayView.instance.isTryout = false;
        }

        public void TryOutHomeBtn()
        {
            if (gameplayView.instance.isTryout)
            {
                Debug.Log("Called");
                transform.parent.GetChild(2).gameObject.SetActive(true);
                transform.parent.GetChild(2).GetChild(0).GetChild(4).gameObject.SetActive(true);
                transform.parent.GetChild(6).gameObject.SetActive(false);
                chickenGameModel.gameCurrentStep.Value = chickenGameModel.GameSteps.OnLogin;
                SceneManager.LoadScene(chickenGameModel.mainSceneLoadname.sceneName);

            }
        }
    }
}