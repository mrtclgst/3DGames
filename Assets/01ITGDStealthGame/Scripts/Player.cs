using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ITGD
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 7, smoothMoveTime = .1f, turnSpeed = 8f;
        float smoothInputMagnitude, smoothMoveVelocity, angle;
        Vector3 velocity;
        Rigidbody rigidbody;
        bool disabled;
        public event Action OnReachedFinishPoint;
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            Guard.OnGuardHasSpottedPlayer += Disable;
        }

        private void Update()
        {
            Vector3 inputDirection = Vector3.zero;
            if (!disabled)
            {
                inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            }

            float inputMagnitude = inputDirection.magnitude;
            smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);

            velocity = transform.forward * moveSpeed * smoothInputMagnitude;
        }
        private void Disable()
        {
            disabled = true;
        }
        private void FixedUpdate()
        {
            rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
            rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
        }
        private void OnDestroy()
        {
            Guard.OnGuardHasSpottedPlayer -= Disable;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Finish")
            {
                Disable();
                if (OnReachedFinishPoint != null)
                {
                    OnReachedFinishPoint();
                }
            }
        }
    }
}