using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_Speed = 12f;                 // How fast the tank moves forward and back.
    public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.

    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private float m_OriginalPitch;              // The pitch sof the audio source at the start of the scene.

    public Animator playerAnim;
    public WheelsController wheelsController;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }

    private void Update()
    {
        m_MovementInputValue = Input.GetAxis("Vertical");
        m_TurnInputValue = Input.GetAxis("Horizontal");

        Animate();
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    void Animate()
    {
        if (m_MovementInputValue  > 0)
        {
            playerAnim.SetBool("MoveForward", true);
            playerAnim.SetBool("MoveBackwards", false);
            wheelsController.Forward();
        }
        else if (m_MovementInputValue < 0)
        {
            playerAnim.SetBool("MoveBackwards", true);
            playerAnim.SetBool("MoveForward", false);
            wheelsController.Backwards();
        }
        else if (m_MovementInputValue == 0)
        {
            playerAnim.SetBool("MoveForward", false);
            playerAnim.SetBool("MoveBackwards", false);
        }

        if (m_TurnInputValue > 0)
        {
            playerAnim.SetBool("TurnRight", true);
            playerAnim.SetBool("TurnLeft", false);

            if (m_MovementInputValue == 0)
                wheelsController.Right();
        }
        else if (m_TurnInputValue < 0)
        {
            playerAnim.SetBool("TurnLeft", true);
            playerAnim.SetBool("TurnRight", false);

            if (m_MovementInputValue == 0)
                wheelsController.Left();
        }
        else
        {
            playerAnim.SetBool("TurnLeft", false);
            playerAnim.SetBool("TurnRight", false);
        }
    }

    private void Move()
    {
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void Turn()
    {
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}