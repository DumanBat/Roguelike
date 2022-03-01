using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 1.0f;

        private Vector3 offset;

        private Vector3 targetPos;

        private void Start()
        {
            if (target == null) return;

            offset = transform.position - new Vector3(target.position.x, target.position.y, -15f);
        }

        private void Update()
        {
            if (target == null) return;

            var targetPosOffset = new Vector3(target.position.x, target.position.y, -15f);
            targetPos = targetPosOffset + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }

    }
}
