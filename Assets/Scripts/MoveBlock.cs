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

    [SerializeField] NextBlock m_NextBlock;

    private SpriteRenderer m_SpriteRenderer;

    private Rigidbody2D m_Rb = null;
    private PolygonCollider2D m_Collider = null;

    private float m_MoveSpeed = 50f;
    private float m_RotateAngle = 90f;
    private float m_CurrentTime;
    private float m_Time = 2f;
    public int m_RandomDeleteTime;

    public bool m_IsCurrentBlock;
    public bool m_IsColliding;

    // Start is called before the first frame update
    void Awake()
    {
        m_InputSystem = new InputSystem();
        m_Rb = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<PolygonCollider2D>();
        m_CurrentTime = m_Time;
        m_IsCurrentBlock = true;
        m_SpriteRenderer= GetComponent<SpriteRenderer>();

        GameObject TargetObject = GameObject.FindGameObjectWithTag("Manager");
        if(TargetObject != null)
        {
            m_NextBlock = TargetObject.GetComponent<NextBlock>();
        }

        m_RandomDeleteTime = Random.Range(15, 30);
    }

    private void OnEnable()
    {
        m_InputSystem.Enable();
        m_InputSystem.Player.Movement.performed += OnMovementPerformed;
        m_InputSystem.Player.Movement.canceled += OnMovementCanceled;
        m_InputSystem.Player.Rotation.performed += OnRotationPerformed;
        m_InputSystem.Player.Rotation.canceled += OnRotationCanceled;
        m_InputSystem.Player.Confirm.performed += Confirm;
    }

    private void OnDisable()
    {
        m_InputSystem.Disable();
        m_InputSystem.Player.Movement.performed -= OnMovementPerformed;
        m_InputSystem.Player.Movement.canceled -= OnMovementCanceled;
        m_InputSystem.Player.Rotation.performed -= OnRotationPerformed;
        m_InputSystem.Player.Rotation.canceled -= OnRotationCanceled;
        m_InputSystem.Player.Confirm.performed -= Confirm;
    }

    private void FixedUpdate()
    {
        m_Rb.velocity = m_MoveVector * m_MoveSpeed;
        transform.Rotate(Vector3.forward * m_RotateVector.y * m_RotateAngle * Time.deltaTime);
        if(m_RandomDeleteTime == 0)
        {
            //StartCoroutine(FadeOut());
            Destroy(gameObject);
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        if (m_IsCurrentBlock)
            m_MoveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext value) 
    {
        m_MoveVector = Vector2.zero;
        SnapToGrid();
    }

    private void OnRotationPerformed(InputAction.CallbackContext value)
    {
        if (m_IsCurrentBlock)
            m_RotateVector = value.ReadValue<Vector2>();
    }

    private void OnRotationCanceled(InputAction.CallbackContext value)
    {
        m_RotateVector = Vector2.zero;
        SnapTo90();
    }

    private void Confirm(InputAction.CallbackContext context)
    {
        m_NextBlock.m_CurrentBlock.GetComponent<PolygonCollider2D>().isTrigger = true;
        if (!IsColliding())
        {
            m_Rb.constraints = RigidbodyConstraints2D.FreezeAll;
            m_Collider.isTrigger = true;
            m_IsCurrentBlock = false;
            m_SpriteRenderer.sortingOrder = 1;
            m_RandomDeleteTime--;
            Debug.Log(m_RandomDeleteTime);
        }
        m_NextBlock.m_CurrentBlock.GetComponent<PolygonCollider2D>().isTrigger = false;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            m_IsColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_IsColliding = false;
        }
    }

    public bool IsColliding()
    {
        return m_IsColliding;
    }

    //IEnumerator FadeOut()
    //{
    //    SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
    //    Color tempColor = sprite.color;

    //    while (tempColor.a >= 1f)
    //    {
    //        tempColor.a += Time.deltaTime / 1f;
    //        sprite.color = tempColor;

    //        if (tempColor.a <= 0f)
    //        {
    //            tempColor.a = 0f; 
    //        }
    //        yield return null;
    //    }

    //    Destroy(gameObject);
    //}
}
