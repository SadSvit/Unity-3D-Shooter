using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuManagerScript : MonoBehaviourPunCallbacks 
{
    [SerializeField] protected List<MenuScript> menus;

    [PunRPC]
    public virtual void OpenMenu(MenuNames menuToOpen)
    {
        foreach (MenuScript menu in menus)
        {
            if (menu.MenuName == menuToOpen)
            {
                menu.Open();
            }
            else
            {
                menu.Close();
            }
        }
    }   
}
