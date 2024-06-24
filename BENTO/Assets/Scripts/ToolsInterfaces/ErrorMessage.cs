using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorMessage : MonoBehaviour
{
    public static ErrorMessage Instance;
    private TextMeshProUGUI text;
    private Coroutine coroutine;

    // create and maintain singleton instance
    void Start()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        text = gameObject.GetComponent<TextMeshProUGUI>();
        if(text != null)
        {
            text.enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // display error text
    public void ShowText(string textToDisplay, Vector3 location)
    {
        text.transform.position = location;
        text.text = textToDisplay;
        text.enabled = true;

        if (coroutine == null)
        {
            coroutine = StartCoroutine(WaitAndDisable());
        }
        else
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(WaitAndDisable());
        }
    }

    // after timer, remove error message
    IEnumerator WaitAndDisable()
    {
        yield return new WaitForSeconds(1.2f);

        text.enabled = false;
        text.text = null;

        coroutine = null;
    }
}
