using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureController : MonoBehaviour
{

    public float Speed { get; set; }
    public Vector3 Direction { get; set; }

    public float XOffset => xOffset;

    float xPositionToDestroy;
    float xOffset;

    private void Awake()
    {
        xOffset = GetComponentInChildren<SpriteRenderer>().bounds.extents.x; ;
    }

    private void Start()
    {
        float xCameraLeft = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        xPositionToDestroy = xCameraLeft - xOffset;
    }

    private void Update()
    {
        if (transform.position.x <= xPositionToDestroy)
        {
            GameManager.Instance.TreasureDisappears(this);
        }
        else
        {
            transform.Translate(Speed * Time.deltaTime * Direction);
        }
    }

}
