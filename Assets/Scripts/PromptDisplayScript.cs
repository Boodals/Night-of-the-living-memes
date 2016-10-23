using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PromptDisplayScript : MonoBehaviour
{

    public static PromptDisplayScript singleton;

    bool showPrompt = false;
    public Image button;
    public Text text;

    // Use this for initialization
    void Awake()
    {
        singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        Color targetCol = Color.white;
        Vector3 targetPos = new Vector3(0, -180, 0);

        if (!showPrompt)
        {
            targetCol = Color.clear;
            targetPos -= Vector3.up * 35;
        }

        text.transform.localPosition = Vector3.Lerp(text.transform.localPosition, targetPos, 6 * Time.deltaTime);
        button.color = Color.Lerp(button.color, targetCol, 8 * Time.deltaTime);
        text.color = button.color;
    }

    public void NewPrompt(string prompt)
    {
        text.text = prompt;
        showPrompt = true;
    }

    public void HidePrompt()
    {
        showPrompt = false;
    }
}
