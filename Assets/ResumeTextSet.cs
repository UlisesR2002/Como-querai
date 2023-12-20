using TMPro;
using UnityEngine;

public class ResumeTextSet : MonoBehaviour
{
    public TextMeshProUGUI text;


    private void Start()
    {
        text.text = "Level " + SaveManager.GetLevel().ToString();
    }
}
