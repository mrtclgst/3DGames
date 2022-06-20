using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessRunner
{
    public class PlayerController : MonoBehaviour
    {
        CharacterController characterController;
        Animator animator;
        Lane lane;
        [SerializeField] Vector3 dir, targetPosition;

        [SerializeField] float forwardSpeed, maxSpeed, jumpForce;
        float laneDistance = 2.5f, gravity = -9.81f;
        bool isGround, isSliding = false;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
        }
        private void Start()
        {
            dir.z = forwardSpeed;
            lane = Lane.Middle;
        }
        private void Update()
        {
            if (!PlayerManager.gameStarted)
                return;

            GameSpeedUp();

            animator.SetTrigger("Run");

            if (SwipeManager.swipeRight)
            {
                switch (lane)
                {
                    case Lane.Left:
                        lane = Lane.Middle;
                        break;
                    case Lane.Middle:
                        lane = Lane.Right;
                        break;
                }
            }

            if (SwipeManager.swipeLeft)
            {
                switch (lane)
                {
                    case Lane.Right:
                        lane = Lane.Middle;
                        break;
                    case Lane.Middle:
                        lane = Lane.Left;
                        break;
                }
            }

            if (SwipeManager.swipeDown && !isSliding)
            {
                StartCoroutine(Slide());
            }

            characterController.Move(Time.deltaTime * dir);

            isGround = characterController.isGrounded;
            Jump(isGround);
        }

        IEnumerator Slide()
        {
            isSliding = true;
            Vector3 characterCenterOld = characterController.center;
            animator.SetTrigger("Slide");
            yield return new WaitForSeconds(.2f);
            characterController.center = new Vector3(0, -0.22f, 0);
            characterController.height = 0.1f;
            isSliding = false;
            yield return new WaitForSeconds(0.8f);
            characterController.center = characterCenterOld;
            characterController.height = 1.75f;
        }

        private void Jump(bool isGround)
        {
            if (dir.y < 0 && isGround)
            {
                dir.y = 0f;
            }
            if (SwipeManager.swipeUp && isGround)
            {
                animator.SetTrigger("Jump");
                dir.y += Mathf.Sqrt(jumpForce * -10.0f * gravity);
            }
            dir.y += gravity * 4f * Time.deltaTime;

            GetNewPositionOfLine(lane);
        }

        private void GetNewPositionOfLine(Lane lane)
        {
            switch (lane)
            {
                case Lane.Left:
                    targetPosition = new Vector3(-laneDistance, transform.position.y, transform.position.z);
                    break;
                case Lane.Middle:
                    targetPosition = new Vector3(0, transform.position.y, transform.position.z);
                    break;
                case Lane.Right:
                    targetPosition = new Vector3(laneDistance, transform.position.y, transform.position.z);
                    break;
            }

            if (transform.position == targetPosition)
                return;

            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
            moveDir.y = 0;

            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                characterController.Move(moveDir);
            else
                characterController.Move(diff);
        }

        void GameSpeedUp()
        {
            if (forwardSpeed < maxSpeed)
            {
                forwardSpeed += 0.1f * Time.deltaTime;
            }
            dir.z = forwardSpeed;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                PlayerManager.gameOver = true;
                FindObjectOfType<AudioManager>().Play("GameOver");
            }
        }
    }
}
enum Lane
{
    Left, Middle, Right
}