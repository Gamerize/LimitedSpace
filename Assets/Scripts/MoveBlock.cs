using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class MoveBlock : MonoBehaviour  
{
    private InputSystem m_InputSystem = null;
    private Vector2 m_MoveVector = Vector2.zero;
    private Vector2 m_RotateVector;
    private Rigidbody2D m_Rb = null;
    private PolygonCollider2D m_Collider = null;
    private float m_MoveSpeed = 50f;
    private float m_RotateAngle = 90f;
    private float m_CurrentTime;
    private float m_Time = 2f;
    private bool m_CanRotate;


    // Start is called before the first frame update
    void Awake()
    {
        m_InputSystem = new InputSystem();
        m_Rb = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<PolygonCollider2D>();
        m_CurrentTime = m_Time;
    }

    private void OnEnable()
    {
        m_InputSystem.Player.Movement.performed += OnMovementPerformed;
        m_InputSystem.Player.Movement.canceled += OnMovementCanceled;
        m_InputSystem.Player.Rotation.performed += OnRotationPerformed;
        m_InputSystem.Player.Rotation.canceled += OnRotationCanceled;
        m_InputSystem.Player.Confirm.performed += Confirm;
    }

    private void OnDisable()
    {
        m_InputSystem.Player.Movement.performed -= OnMovementPerformed;
        m_InputSystem.Player.Movement.canceled -= OnMovementCanceled;
        m_InputSystem.Player.Rotation.performed -= OnRotationPerformed;
        m_InputSystem.Player.Rotation.canceled -= OnRotationCanceled;
        m_InputSystem.Player.Confirm.performed -= Confirm;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if(hit.collider != null) 
            {
                if(hit.collider.CompareTag("Player"))
                {
                    m_InputSystem.Enable();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        m_Rb.velocity = m_MoveVector * m_MoveSpeed;
        transform.Rotate(Vector3.forward * m_RotateVector.y * m_RotateAngle * Time.deltaTime);
    }


    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        m_MoveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext value) 
    {
        m_MoveVector = Vector2.zero;
        SnapToGrid();
    }

    private void OnRotationPerformed(InputAction.CallbackContext value)
    {
        m_RotateVector = value.ReadValue<Vector2>();
    }

    private void OnRotationCanceled(InputAction.CallbackContext value)
    {
        m_RotateVector = Vector2.zero;
        SnapTo90();
    }

    private void Confirm(InputAction.CallbackContext context)
    {
        m_Rb.constraints = RigidbodyConstraints2D.FreezeAll;
        m_Collider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        m_InputSystem.Disable();
    }

    void SnapTo90()
    {
        float zRotation = transform.eulerAngles.z;
        float SnapRotation = Mathf.Round(zRotation / 90f) * 90f;
        transform.eulerAngles = new Vector3(0, 0, SnapRotation);
    }

    void SnapToGrid()
    {
        Vector3 OldPosition = transform.position;
        int RoundX = Mathf.FloorToInt(OldPosition.x);
        int RoundY = Mathf.FloorToInt(OldPosition.y);
        transform.position = new Vector3(RoundX, RoundY, 0);
    }


}
