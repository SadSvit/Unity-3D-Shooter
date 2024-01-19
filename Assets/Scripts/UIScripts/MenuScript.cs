using UnityEngine;
using Photon.Pun;

public abstract class MenuScript : MonoBehaviourPunCallbacks
{
    [SerializeField] protected MenuManagerScript menuManager;
    [SerializeField] protected GameObject[] contentInMenu;
    public MenuNames MenuName { get; protected set; }
     
    public virtual void Open()
    {
        foreach (GameObject content in contentInMenu)
        {
            content.SetActive(true);
        }
    }

    public virtual void Close()
    {
        foreach (GameObject content in contentInMenu)
        {
            content.SetActive(false);
        }
    }
}
