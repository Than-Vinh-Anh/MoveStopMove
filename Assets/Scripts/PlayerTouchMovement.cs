using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchMovement : MonoBehaviour
{
    enum State
    {
        Move,
        Idle,
    }

    [SerializeField]
    private Vector2 joyStickSize = new Vector2(300, 300);
    [SerializeField]
    private FloatingJoyStick joyStick;
    [SerializeField]
    private NavMeshAgent player;
    [SerializeField]
    private Animator animator;
    [SerializeField] Canvas guideCanvas;
    private Finger movementFinger;
    private Vector2 movementAmount;

    private State curState;

    public bool isStart;

    private void Start()
    {
        curState = State.Idle;
        isStart = false;
    }
    public void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandlFingerMove;
    }
    public void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandlFingerMove;
        movementAmount = Vector2.zero;
        EnhancedTouchSupport.Disable();
    }
    private void HandleFingerDown(Finger finger)
    {
        if (movementFinger == null)
        {
            movementFinger = finger;
            movementAmount = Vector2.zero;
            joyStick.gameObject.SetActive(true);
            joyStick.RectTransform.sizeDelta = joyStickSize;
            joyStick.RectTransform.anchoredPosition = ClampStartPosition(finger.screenPosition);
            guideCanvas.gameObject.SetActive(false);
        }
    }

    private void HandleLoseFinger(Finger LostFinger)
    {
        if (LostFinger == movementFinger)
        {
            movementFinger = null;
            joyStick.Knob.anchoredPosition = Vector2.zero;
            joyStick.gameObject.SetActive(false);
            movementAmount = Vector2.zero;

            PlayerController.instance.isMoving = false;

            curState = State.Idle;
        }
    }

    private void HandlFingerMove(Finger MovedFinger)
    {
        if (MovedFinger == movementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = joyStickSize.x / 2f;
            ETouch.Touch currentTouch = MovedFinger.currentTouch;

            if (Vector2.Distance(currentTouch.screenPosition,
                joyStick.RectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - joyStick.RectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - joyStick.RectTransform.anchoredPosition;
            }
            joyStick.Knob.anchoredPosition = knobPosition;
            movementAmount = knobPosition / maxMovement;

            PlayerController.instance.isMoving = true;
            curState = State.Move;
        }
    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        if (startPosition.x < joyStickSize.x / 2)
        {
            startPosition.x = joyStickSize.x / 2;
        }

        if (startPosition.y < joyStickSize.y / 2)
        {
            startPosition.y = joyStickSize.y / 2;
        }
        else if (startPosition.y > Screen.height - joyStickSize.y / 2)
        {
            startPosition.y = Screen.height - joyStickSize.y / 2;
        }
        return startPosition;
    }
    private void Update()
    {
        if (PlayerController.instance.isStart)
        {
            if (!PlayerController.instance.isDead && !LevelManager.instance.isWin)
            {
                Vector3 scaledMovement = player.speed * Time.deltaTime * new Vector3(movementAmount.x, 0, movementAmount.y);

                player.transform.LookAt(player.transform.position + scaledMovement, Vector3.up);
                player.Move(scaledMovement);

                if (curState == State.Move)
                {
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsAttack", false);
                }
                if (curState == State.Idle)
                {
                    animator.SetBool("IsIdle", true);
                }
            }
        }
    }
}
