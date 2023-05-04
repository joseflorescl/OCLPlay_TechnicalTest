using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureController : MonoBehaviour
{
    // TODO: implementar la interfaz IVFXEntity para ser usada por el VFX Mgr

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

        GameManager.Instance.TreasureCreated(this);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // The Pickup layer can only collide with itself
        // That's why the player object has a specialized collider for this.
        // Nothing else can collide with a pickup, that's why the type of object that comes in collision is not validated with an if

        GameManager.Instance.TreasureCatched(this);
    }

}
