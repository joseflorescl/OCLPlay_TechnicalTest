using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{

    [SerializeField] private Collider2D coll2D;

    private void Start()
    {
        DeactivatePickupCollider();
    }


    public void ActivatePickupCollider()
    {
        coll2D.enabled = true;
    }

    public void DeactivatePickupCollider() 
    {  
        coll2D.enabled = false; 
    }
}
