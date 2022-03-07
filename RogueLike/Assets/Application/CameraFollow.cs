using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed = 1.0f;

    private Vector3 offset;

    private Vector3 targetPos;

    public void Init(Transform followTarget)
    {
        target = followTarget;
        var targetPosOffest = new Vector3(target.position.x, target.position.y, -15f);
        transform.position = targetPosOffest;
        offset = transform.position - targetPosOffest;
    }

    private void Update()
    {
        if (target == null) return;

        var targetPosOffset = new Vector3(target.position.x, target.position.y, -15f);
        targetPos = targetPosOffset + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }

}
