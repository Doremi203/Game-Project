using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{

    // Базовый класс для всех предметов, с которыми можно взаимодействовать.
    // InteractionText это текст, который будет высвечиваться при предложении провзаимодействовать.

    [SerializeField] private string interactionText;

    public virtual string GetInteractionText()
    {
        return interactionText;
    }

    public virtual void Interact(Player player)
    {

    }
  
}