using UnityEngine;

public class PopupManager : MenuManagerScript, IPopupable
{        
    public bool isOpen { get; set; }
    protected KeyCode popupControllerButton;
    [field: SerializeField] public GameObject[] popupContent { get; set; }
 

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