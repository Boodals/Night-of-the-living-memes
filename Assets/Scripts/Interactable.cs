using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    protected bool m_canInteract;
    public virtual void interact()
    {}

    void OnTriggerStay(Collider _other)
    {
        if (_other.tag == Tags.Player && Input.GetButtonDown("Action") && m_canInteract)
        {
            //hide help msg
            interact();
        }
    }
     
    void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == Tags.Player)
        {
            //display help msg
        }
    }
    void OnTriggerExit(Collider _other)
    {
        if (_other.tag == Tags.Player)
        {
            //hide help msg
        }
    }
}
