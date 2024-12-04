using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControllers : MonoBehaviour
{
    // Start to attack
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

}
