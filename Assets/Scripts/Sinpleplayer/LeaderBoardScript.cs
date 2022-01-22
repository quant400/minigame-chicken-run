using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LeaderBoardScript : MonoBehaviour
{
    [SerializeField]
    Button defaultButton;

    [SerializeField]
    private Button[] buttons;
    internal void Activate()
    {
        transform.DOScale(Vector3.one, 1f);
        defaultButton.Select();
        defaultButton.interactable = false;
    }

    internal void Deactivate()
    {
        transform.DOScale(Vector3.zero, 1f).OnComplete(()=>gameObject.SetActive(false));
    }

    public void SetAllButtonsInteractable()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    public void OnButtonClicked(Button clickedButton)
    {
        int buttonIndex = System.Array.IndexOf(buttons, clickedButton);

        if (buttonIndex == -1)
            return;

        SetAllButtonsInteractable();

        clickedButton.interactable = false;
    }

}