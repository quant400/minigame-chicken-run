using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.IO;

public class GameOverScript : MonoBehaviour
{
    [SerializeField]
    Transform characterDisplay;
    GameObject[] characters;
    [SerializeField]
    TMP_Text currentScore, highScore, leaderboard;
    [SerializeField]
    GameObject canvasToDisable;
   // [SerializeField]
    //SinglePlayerSpawner spawner;
    private void OnEnable()
    {
        //characters = spawner.GetCharacterList();
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        GameObject displayChar = Resources.Load(Path.Combine("SinglePlayerPrefabs/Characters", SingleplayerGameControler.instance.chosenNFT.name)) as GameObject;
        var temp=Instantiate(displayChar, characterDisplay.position, Quaternion.identity,characterDisplay);
        
        //destroying all player related components
        Destroy(temp.transform.GetChild(1).gameObject);
        Destroy(temp.transform.GetChild(0).gameObject);
        Destroy(temp.transform.GetChild(2).gameObject);
        Destroy(temp.transform.GetChild(3).gameObject);
        Destroy(temp.GetComponent<StarterAssetsInputs>());
        Destroy(temp.GetComponent<ThirdPersonController>());
        Destroy(temp.GetComponent<CharacterController>());
        Destroy(temp.GetComponent<PlayerInput>());
        temp.GetComponent<Animator>().SetBool("Ended", true);

        temp.transform.localPosition = Vector3.zero;
        temp.transform.localRotation = Quaternion.identity;
        temp.transform.localScale = Vector3.one * 2;
        currentScore.text = SinglePlayerScoreBoardScript.instance.GetScore().ToString();
        //upddate other values here form leaderboard
        canvasToDisable.SetActive(false);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SingleplayerGameControler.instance.GetSinglePlayerScene());
    }
}
