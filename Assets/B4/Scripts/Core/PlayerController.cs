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
    Animator anim;
    int grabHash;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        grabHash = Animator.StringToHash("Grab");
    }

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
        anim.SetTrigger(grabHash);
        spineAnimationState.SetAnimation(0, cop, false);
        spineAnimationState.AddAnimation(0, swinging, true, 0);

        // TODO: vfx de las partículas al agarrar un tesoro
        // TODO: size adecuado del player, ahora está muy grande
    }
    
}
