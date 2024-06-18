using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_Tutorial : MonoBehaviour
{
    [SerializeField] private Image[] images;
    private int currentIndex;
    [SerializeField] private bool canProgress = true;
    [SerializeField] private string SceneToLoad;

    private void Start()
    {
        Time.timeScale = 1.0f;
        currentIndex = images.Length - 1;
    }

    private void OnEnable()
    {
        // subscribe to event for player touch input
        TouchInput.OnTouch += Progress;
    }

    private void OnDisable()
    {
        // unsubscribe from event for player touch input
        TouchInput.OnTouch -= Progress;
    }

    private void Progress()
    {
        if (canProgress)
        {
            canProgress = false;
            if (currentIndex >= 1)
            {
                images[currentIndex].gameObject.SetActive(false);
                currentIndex--;
            }
            else
            {
                SceneManager.LoadScene(SceneToLoad);
            }

             StartCoroutine(WaitMinTime());
        }
        
    }

    IEnumerator WaitMinTime()
    {
        yield return new WaitForSeconds(0.5f);
        canProgress = true;
    }
}
