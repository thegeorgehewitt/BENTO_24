using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RandomImage : MonoBehaviour
{
    [SerializeField] private Sprite[] customerSprites;
    [SerializeField] private List<int> imageIndex = new List<int>();
    [SerializeField] private Image spriteImage;

    private void Awake()
    {
        spriteImage = GetComponent<Image>();

        for (int i = 0; i < customerSprites.Length; i++)
        {
            imageIndex.Add(i);
        }
    }

    public void UpdateImage(List<int> newValues)
    {
        spriteImage.enabled = false;

        StartCoroutine(WaitAndUpdate());
    }

    IEnumerator WaitAndUpdate()
    {
        yield return new WaitForSeconds(0.5f);

        if (spriteImage != null)
        {
            if (imageIndex.Count == 0)
            {
                for (int i = 0; i < customerSprites.Length; i++)
                {
                    imageIndex.Add(i);
                }
            }

            int randomInt = Random.Range(0, imageIndex.Count - 1);
            spriteImage.sprite = customerSprites[imageIndex[randomInt]];
            imageIndex.Remove(imageIndex[randomInt]);

            spriteImage.enabled = true;
        }
    }
}
