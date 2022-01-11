using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class CharacterSelectionScript : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    Transform[] characters;
    int currentCharacter;
    bool selected;
    [SerializeField]
    float sideCharZdisp;
    [SerializeField]
    Button rightButton,leftButton;
    Info[] myNFT;

    [SerializeField]
    GameObject buttonsToEnable, ButtonToDisable;

    public void MoveRight()
    {
        rightButton.interactable = false;
        leftButton.interactable = false;
        if (currentCharacter < characters.Length-1)
        {
            if (selected)
            {
                characters[currentCharacter].GetComponent<Animator>().SetBool("Selected", false);
                selected = false;
            }
            characters[currentCharacter].transform.localPosition += new Vector3(0, 0, sideCharZdisp);
            currentCharacter++;
            characters[currentCharacter].transform.DOLocalMove(characters[currentCharacter].transform.localPosition + new Vector3(0, 0, -sideCharZdisp), 0.5f);
            cam.transform.DOMoveX(characters[currentCharacter].transform.position.x,  0.5f).OnStepComplete(()=> 
            {
                
                rightButton.interactable = true;
                leftButton.interactable = true;
            } );
        }
        else
        {
            characters[currentCharacter].transform.localPosition += new Vector3(0, 0, sideCharZdisp);
            currentCharacter = 0;
            characters[currentCharacter].transform.DOLocalMove(characters[currentCharacter].transform.localPosition + new Vector3(0, 0, -sideCharZdisp), 0.5f);
            cam.transform.DOMoveX(characters[currentCharacter].transform.position.x, 0.5f).OnStepComplete(() =>
            {
                
                rightButton.interactable = true;
                leftButton.interactable = true;
            });
        }
       
    }

    public void MoveLeft()
    {
        if (currentCharacter > 0)
        {
            if (selected)
            {
                characters[currentCharacter].GetComponent<Animator>().SetBool("Selected", false);
                selected = false;
            }
            characters[currentCharacter].transform.localPosition += new Vector3(0, 0, sideCharZdisp);
            currentCharacter--;
            characters[currentCharacter].transform.DOLocalMove(characters[currentCharacter].transform.localPosition + new Vector3(0, 0, -sideCharZdisp), 0.5f);

            cam.transform.DOMoveX(characters[currentCharacter].transform.position.x,0.5f).OnStepComplete(() =>
            {
                rightButton.interactable = true;
                leftButton.interactable = true;
            });
        }
        else
        {
            characters[currentCharacter].transform.localPosition += new Vector3(0, 0, sideCharZdisp);
            currentCharacter = characters.Length - 1;
            characters[currentCharacter].transform.DOLocalMove(characters[currentCharacter].transform.localPosition + new Vector3(0, 0, -sideCharZdisp), 0.5f);
            cam.transform.DOMoveX(characters[currentCharacter].transform.position.x, 0.5f).OnStepComplete(() =>
            {
                
                rightButton.interactable = true;
                leftButton.interactable = true;
            });
        }

        
    }

   
    public void Selected()
    {
        GameRoom.room.ChooseAvatar("PlayerAvatar" + currentCharacter);
        selected = true;
        characters[currentCharacter].GetComponent<Animator>().SetBool("Selected", true);

    }

    public void FinalSelect()
    {
        GameRoom.room.ChooseAvatar("PlayerAvatar" + currentCharacter);
        selected = true;
        characters[currentCharacter].GetComponent<Animator>().SetBool("Selected", true);
        rightButton.interactable = false;
        leftButton.interactable = false;

    }

    public void UndoFinalSelect()
    {
        rightButton.interactable = true;
        leftButton.interactable = true;
    }

    //added for single player
    public void FinalSelectSinglePlayer()
    {
        SingleplayerGameControler.instance.chosenAvatar = currentCharacter;
        selected = true;
        characters[currentCharacter].GetComponent<Animator>().SetBool("Selected", true);
        rightButton.interactable = false;
        leftButton.interactable = false;

    }



    internal void SetData(Info[] nFTData)
    {
        //will be respossible for setting up characters according to nft
        myNFT = nFTData;
        SetUpCharacters();
        
    }

    private void SetUpCharacters()
    {
        // change later to cycle through all characters in returned list and load according to names or id
        if (myNFT[0].id == 538)
            characters[1].gameObject.SetActive(true);
        
        if (myNFT[1].id == 542)
            characters[2].gameObject.SetActive(true);



        Done();
       

    }

    //change to private when skip removed
    public void Done()
    {
        buttonsToEnable.SetActive(true);
        ButtonToDisable.SetActive(false);
    }
}
