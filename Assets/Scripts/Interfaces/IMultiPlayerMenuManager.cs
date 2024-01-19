using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMultiPlayerMenuManager 
{
    public void OpenMenuForAllPlayers(MenuNames menuToOpen);
}
