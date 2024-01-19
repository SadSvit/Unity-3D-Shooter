using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MenuManagerScript, IPopupable
{
    [field: SerializeField] public GameObject[] popupContent { get; set; }
    public bool isOpen { get; set ;}
    protected KeyCode popupControllerButton;

    public virtual void Start()
    {
        isOpen = false;
        popupControllerButton = KeyCode.Escape;
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown(popupControllerButton))
        {
            if (isOpen)
            {
                Disappear();
            }
            else
            {
                Appear();
            }
        }
    }

    public virtual void Appear()
    {        
        foreach (GameObject appearContent in popupContent)
        {
            appearContent.SetActive(true);
        }

        isOpen = true;
    }

    public virtual void Disappear()
    {
        foreach (GameObject appearContent in popupContent)
        {
            appearContent.SetActive(false);
        }

        isOpen = false;
    }    
}