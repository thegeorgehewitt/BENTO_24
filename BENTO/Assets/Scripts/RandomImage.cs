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

        UpdateImage(imageIndex);
    }

    public void UpdateImage(List<int> newValues)
    {
        Debug.Log("Fucntion called");
        if (spriteImage != null)
        {
            Debug.Log("sprite renderer found");
            Debug.Log(imageIndex.Count);
            Debug.Log(customerSprites.Length);



            if (imageIndex.Count == 0)
            {
                for (int i = 0; i < customerSprites.Length; i++)
                {
                    imageIndex.Add(i);
                }
            }
            
            int randomInt = Random.Range(0, imageIndex.Count - 1);
            spriteImage.sprite = customerSprites[imageIndex[randomInt]];
            Debug.Log(spriteImage.sprite.name);
            imageIndex.Remove(imageIndex[randomInt]);
        }
    }
}
