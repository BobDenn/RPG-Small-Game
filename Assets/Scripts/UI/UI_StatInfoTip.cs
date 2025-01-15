using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatInfoTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;

    public void ShowStatInfoTip(string text)
    {
        description.text = text;
        
        gameObject.SetActive(true);
    }

    public void HideStatInfoTip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }
}
