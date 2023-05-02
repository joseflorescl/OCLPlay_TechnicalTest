using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;

public class SandboxSpineAnimation : MonoBehaviour
{
    public SkeletonAnimation arloAnimation;
    public AnimationReferenceAsset idle;
    public AnimationReferenceAsset run;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAnimation(idle, true, 1f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAnimation(run, true, 1f);
        }
    }

    void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {        
        Spine.TrackEntry track =  arloAnimation.state.SetAnimation(0, animation, loop);        
        track.TimeScale = timeScale;

        //track.Complete += CompleteHandler;
    }

    private void CompleteHandler(Spine.TrackEntry trackEntry)
    {
        throw new NotImplementedException();
    }
}
