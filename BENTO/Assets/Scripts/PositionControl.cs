using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sets position of 'prepped food area' based on position of edge of screen
public class PositionControl : MonoBehaviour
{
    [SerializeField] private GameObject GuideUI;

    void Start()
    {
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(GuideUI.transform.position).x - transform.localScale.x/2, Camera.main.ScreenToWorldPoint(GuideUI.transform.position).y, 0);
    }
}
