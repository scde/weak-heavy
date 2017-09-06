using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    // LayerMask check (source): http://answers.unity3d.com/answers/454913/view.html
    public static bool CheckLayerMask(LayerMask mask, GameObject obj)
    {
        return (mask.value & 1 << obj.layer) != 0;
    }

    // source: http://answers.unity3d.com/questions/823090/equivalent-of-degree-to-vector2-in-unity.html
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    // source: https://math.stackexchange.com/questions/785375/calculate-initial-velocity-to-reach-height-y#785397
    public static float InitialJumpVelocity(float jumpHeight)
    {
        return Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * jumpHeight);
    }

    public static IEnumerator waitForRespawn(float RespawnTime, GameObject gOBJ)
    {
        gOBJ.SetActive(false);
        yield return new WaitForSeconds(RespawnTime);
        gOBJ.SetActive(true);
    }

    public static float GetClipLength(Animator anim, string clipName)
    {
        float clipLength = 0.0f;
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == clipName)
            {
                clipLength = ac.animationClips[i].length;
                break;
            }
        }
        return clipLength;
    }
}
