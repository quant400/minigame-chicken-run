using UnityEngine;
using UnityEngine.UI;

public class CaharcterButtonScript : MonoBehaviour
{
    public void Clicked()
    {
        gameObject.GetComponent<Image>().color = Color.green;
    }
}
