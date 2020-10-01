using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShurikensCounter : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        Player.Instance.GetComponent<AbilityShurikens>().UseEvent.AddListener(Refresh);
        Refresh();
    }

    private void Refresh()
    {
        text.text = "Shurikens: " + Player.Instance.GetComponent<AbilityShurikens>().CurrentShurikensAmount;
    }

}