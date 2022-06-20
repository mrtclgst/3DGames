using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner
{
    public class CameraController : MonoBehaviour
    {
        Transform target;
        Vector3 offset;
        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            offset = transform.position - target.position;
        }
        private void LateUpdate()
        {
            Vector3 newCameraPosition =
                new Vector3(transform.position.x, transform.position.y, offset.z + target.position.z
                );
            transform.position = newCameraPosition;
        }
    }
}