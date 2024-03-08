using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionControl : MonoBehaviour
{
    [SerializeField] private GameObject GuideUI;

    void Start()
    {
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(GuideUI.transform.position).x - transform.localScale.x/2, Camera.main.ScreenToWorldPoint(GuideUI.transform.position).y, 0);
    }
}
