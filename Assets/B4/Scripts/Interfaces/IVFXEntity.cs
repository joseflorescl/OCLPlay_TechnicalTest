using UnityEngine;

public interface IVFXEntity
{
    // For now only the position is required, but the idea is to place here all the parameters
    // required by the VFXManager to create the visual effects (color, material, etc)

    Vector3 Position { get; }

}