using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AnimationReferenceAsset swinging;
    [SerializeField] private AnimationReferenceAsset cop;

    SkeletonAnimation arloAnimation;
    Spine.AnimationState spineAnimationState;

    private void Start()
    {
        arloAnimation = GetComponentInChildren<SkeletonAnimation>();
        spineAnimationState = arloAnimation.AnimationState;

    }

    private void OnEnable()
    {
        GameManager.Instance.OnGrabButtonPressed += GrabButtonPressedHandler;
    }
    

    private void OnDisable()
    {
        GameManager.Instance.OnGrabButtonPressed -= GrabButtonPressedHandler;
    }

    private void GrabButtonPressedHandler()
    {
        spineAnimationState.SetAnimation(0, cop, false);
        spineAnimationState.AddAnimation(0, swinging, true, 0);
    }
    
}
