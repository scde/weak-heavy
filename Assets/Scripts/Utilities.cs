using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {
    public static float InitialJumpVelocity(float jumpHeight)
    {
		// source: https://math.stackexchange.com/questions/785375/calculate-initial-velocity-to-reach-height-y#785397
		return Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * jumpHeight);
    }
}
