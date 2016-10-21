using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public virtual void init() { }

    protected bool m_canInteract;
    public string prompt;

    public virtual void interact()
    {
        PromptDisplayScript.singleton.HidePrompt();
    }

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
            PromptDisplayScript.singleton.NewPrompt(prompt);
            //display help msg
        }
    }
    void OnTriggerExit(Collider _other)
    {
        if (_other.tag == Tags.Player)
        {
            PromptDisplayScript.singleton.HidePrompt();
            //hide help msg
        }
    }
}
