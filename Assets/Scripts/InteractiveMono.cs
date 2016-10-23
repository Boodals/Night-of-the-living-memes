using UnityEngine;
using System.Collections;

public class InteractiveMono : MonoBehaviour
{


    protected bool m_canInteract;
    public string prompt;

    private bool m_canInteractInternal=false;//extra safeguard on "once-per-frame" behaviour because for some reason interact was happening twice with doors!
    public virtual void init()
    { }
    public virtual void interact()
    { }

    void OnTriggerStay(Collider _other)
    {
        if (_other.tag == Tags.Player && Input.GetButtonDown("Action") && m_canInteract&& m_canInteractInternal)
        {
            m_canInteractInternal = false;
            //hide help msg
            PromptDisplayScript.singleton.HidePrompt();
            interact();
        }
    }
     
    void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == Tags.Player &&m_canInteract)
        {
            m_canInteractInternal = true;
            PromptDisplayScript.singleton.NewPrompt(prompt);
            //display help msg
        }
    }
    void OnTriggerExit(Collider _other)
    {
        if (_other.tag == Tags.Player && m_canInteract)
        {
            m_canInteractInternal = false;
            PromptDisplayScript.singleton.HidePrompt();
            //hide help msg
        }
    }
}
