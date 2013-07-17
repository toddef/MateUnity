using UnityEngine;
using System.Collections;

[AddComponentMenu("M8/Physics/GravityFieldSpherical")]
public class GravityFieldSpherical : GravityFieldBase {

    public bool inward = false;

    protected override Vector3 GetUpVector(Vector3 position) {
        Vector3 dir = inward ? transform.position - position : position - transform.position;
        return dir.normalized;
    }
}
