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

    // ensure proper timescale and record number of tutorial pictures
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

    // progress through tutorial images adn then continue
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

    // prevent player from skipping tutorial too quickly
    IEnumerator WaitMinTime()
    {
        yield return new WaitForSeconds(0.4f);
        canProgress = true;
    }
}
