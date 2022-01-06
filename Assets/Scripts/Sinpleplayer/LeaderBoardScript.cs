using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LeaderBoardScript : MonoBehaviour
{
    [SerializeField]
    Button defaultButton;
    internal void Activate()
    {
        transform.DOScale(Vector3.one, 1f);
        defaultButton.Select();
    }

    internal void Deactivate()
    {
        transform.DOScale(Vector3.zero, 1f).OnComplete(()=>gameObject.SetActive(false));
    }
}
