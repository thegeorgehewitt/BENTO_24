using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomImage : MonoBehaviour
{
    [SerializeField] private Sprite[] customerSprites;
    [SerializeField] private List<int> imageIndex = new List<int>();
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        for (int i = 0; i < customerSprites.Length; i++)
        {
            imageIndex.Add(i);
        }
    }

    public void UpdateImage(List<int> newValues)
    {
        Debug.Log("Fucntion called");
        if (spriteRenderer != null)
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
            spriteRenderer.sprite = customerSprites[randomInt];
            imageIndex.Remove(randomInt);
        }
    }
}
