using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUi : MonoBehaviour
{
    [SerializeField] private Image skillImage;
    [SerializeField] private TextMeshProUGUI coolDownText;
    private Ability ability;
    private bool onCoolDown;

    public void Setup(Ability ability)
    {
        skillImage.sprite = ability.skillSprite;
        ability.Casted.AddListener(CoolDownStarted);
        this.ability = ability;
    }

    private void CoolDownStarted()
    {
        onCoolDown = true;
    }

    private void Update()
    {
        float cooldown = ability.nextUseTime - Time.time;
        if(cooldown > 0)
        {
            coolDownText.text = $"{cooldown:F1}";
        }
        else if (onCoolDown)
        {
            coolDownText.text = "";
            onCoolDown = false;
        }
    }
}
