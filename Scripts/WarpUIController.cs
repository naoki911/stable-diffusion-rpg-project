using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WarpUIController : MonoBehaviour
{
    public TextMeshProUGUI destinationText;
    public GameObject destinationUI;

    public void ShowDestination(string destinationName)
    {
        destinationUI.SetActive(true);
        destinationText.text = destinationName;
    }

    public void HideDestination()
    {
        destinationUI.SetActive(false);
        destinationText.text = "";
    }
}
