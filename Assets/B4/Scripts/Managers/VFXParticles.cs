using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXParticles : MonoBehaviour
{
    [SerializeField] private GameObject vfxParticlesPrefab;

    ParticleSystem vfxParticlesInstance;

    private void Awake()
    {
        var particles = Instantiate(vfxParticlesPrefab, transform);
        vfxParticlesInstance = particles.GetComponentInChildren<ParticleSystem>();
    }

    public void Play(Vector3 position)
    {
        vfxParticlesInstance.transform.position = position;
        vfxParticlesInstance.Play();
    }

    public void Stop()
    {
        vfxParticlesInstance.Stop();
    }

}
