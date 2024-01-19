using UnityEngine;

public interface IPopupable
{
    public GameObject[] popupContent { get; set; }
    public bool isOpen { get; set; }
    public void Appear();
    public void Disappear();
}
