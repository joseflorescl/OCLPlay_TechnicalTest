using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private VFXParticles treasureCatched;

    private void OnEnable()
    {
        GameManager.Instance.OnTreasureCatched += TreasureCatchedHandler;
    }    

    private void OnDisable()
    {
        GameManager.Instance.OnTreasureCatched -= TreasureCatchedHandler;
    }

    private void TreasureCatchedHandler(IVFXEntity treasure)
    {
        treasureCatched.Play(treasure.Position);
    }
}
