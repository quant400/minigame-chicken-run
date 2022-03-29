using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfoHolder : MonoBehaviour
{
    [SerializeField]
    Sprite [] bg;
    Image charPic;
    string charName;
    [SerializeField]
    Image display;
    [SerializeField]
    Sprite defaultImg;
    private void Awake()
    {
        int x = Random.Range(0, bg.Length);
        gameObject.GetComponent<Image>().sprite = bg[x];
        charPic = transform.GetChild(0).GetComponent<Image>();
        ResetSlot();
    }

    public void SetChar(string name)
    {
        //Debug.Log(name);
        charName = name;
        charPic.sprite = Resources.Load(Path.Combine("SinglePlayerPrefabs/DisplaySprites/HeadShots", name), typeof(Sprite)) as Sprite;
        charPic.color = new Color(225, 225, 225, 225);


    }

    public void OnClick()
    {
        display.sprite = Resources.Load(Path.Combine("SinglePlayerPrefabs/DisplaySprites/Display", charName), typeof(Sprite)) as Sprite;
        display.color = new Color(225, 225, 225, 225);
        transform.GetComponentInParent<characterSelectionView>().currentCharacter = transform.GetSiblingIndex();
    }

    private void ResetSlot()
    {
        charPic.sprite = defaultImg;
        charPic.color = new Color(225, 225, 225, 0);
    }
}
