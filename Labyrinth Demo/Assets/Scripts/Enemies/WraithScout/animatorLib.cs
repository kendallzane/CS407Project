using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorLib : MonoBehaviour {

	public static void ToggleWalkState(Animator animator, string walkAnimatorDir, bool walkState)
    {
        animator.SetBool("WalkLeft", false);
        animator.SetBool("WalkUp", false);
        animator.SetBool("WalkRight", false);
        animator.SetBool("WalkDown", false);
        animator.SetBool(walkAnimatorDir, walkState);
    }
}
