using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool IsPaused = false;

    /// <summary>
    /// Get character controller and camera
    /// </summary>
    
    [Header("Setup ")]
    [SerializeField] private CharacterController PlayerController;
    [SerializeField] private Camera Camera;
    [SerializeField] private Transform GorundCheck;

    public WeaponBase Weapon;

    /// <summary>
    /// Setup settings for player movement
    /// </summary>
    
    [Header("Setup movement")]
    [SerializeField] private float WalkSpeed = 4f;
    [SerializeField] private float RunSpeed = 10f;
    [SerializeField] private float smoothTime = 0.03f;
    [SerializeField] private float JumpForce = 7f;
    private float Speed;

    /// <summary>
    /// Player gravity settings
    /// </summary>
    
    [Header("Setup gravity")]
    private float Gravity;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private float GroundCheckDistance = 0.4f;


    [Header("Crosshair")]
    [SerializeField] private Image Crosshair;
    [SerializeField] private Sprite DefaultCrossHairSprite;

    /// <summary>
    /// Player camera settings
    /// </summary>
    
    public float sensitivity = 4f;

    private void Start()
    {
        Speed = WalkSpeed;

        Gravity = Physics.gravity.y;

        InitPos = WeaponPos.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (IsPaused) return;

        MoveController();
        CameraController();
        PlayerFall();
        Jump();
        IsMoving();
        GetUsableObjects();
        ShootControlls();
        CheckMotion();
    }

    /// <summary>
    /// Player smooth vector settings 
    /// </summary>

    private Vector2 targetDir;
    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;
    private Vector3 velocity;
    
    private void MoveController()
    {
        targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, smoothTime);

        Vector3 _velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Speed;

        PlayerController.Move( _velocity * Time.deltaTime );
    }

    public bool IsMoving()
    {
        return Convert.ToBoolean(targetDir.magnitude);
    }

    /// <summary>
    /// Player gravity fall
    /// </summary>

    private bool isGrounded;
    public bool PlayerGrounded => isGrounded;
    
    private void PlayerFall()
    {
        Ray ray = new Ray(GorundCheck.position, -Vector3.up);
        RaycastHit hit;
        
        isGrounded = Physics.Raycast(ray, out hit, GroundCheckDistance, GroundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += Gravity * Time.deltaTime;

        PlayerController.Move(velocity * Time.deltaTime);

        if (isGrounded && !hit.collider.gameObject.isStatic && !hit.collider.GetComponent<Rigidbody>())
            transform.parent = hit.collider.transform;
        else
            transform.parent = null;
    }

    /// <summary>
    /// Camera controller settings
    /// </summary>

    private float xRotation = 0f;
    private bool CanCameraRotate = true;
    private void CameraController()
    {
        if (Cursor.lockState == CursorLockMode.None || !CanCameraRotate) return;
        
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        xRotation -= mouseDelta.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);
        
        transform.Rotate(Vector3.up * mouseDelta.x * sensitivity);
        WeaponSway( mouseDelta );
        Camera.transform.localEulerAngles = Vector3.right * xRotation;
    }
    
    /// <summary>
    /// Jump method
    /// </summary>
    
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            velocity.y += JumpForce;
    }

    /// <summary>
    /// Pick up object's method
    /// </summary>

    [Header("Weapon")]
    [SerializeField] private LayerMask WeaponMask;
    [SerializeField] private float Distance = 3f;
    [SerializeField] private Transform WeaponPos;

    [Header("HealthSystem")]
    public float Health;

    private UsableBase use;

    private void GetUsableObjects()
    {
        Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast( ray, out hit, Distance ))
        {
            if (Weapon) return;

            WeaponBase weapon = hit.collider.GetComponent<WeaponBase>();
            use = hit.collider.GetComponent<UsableBase>();

            if (use)
                Crosshair.sprite = use.CrosshairImage;
            else if (weapon)
                Crosshair.sprite = weapon.CrosshairSprite;
            else
                Crosshair.sprite = DefaultCrossHairSprite;

            if (Input.GetMouseButtonDown(0))
            {
                if (weapon)
                {
                    weapon.Take(WeaponPos);
                    Weapon = weapon;
                    Crosshair.sprite = DefaultCrossHairSprite;
                }
                else if (use)
                {
                    use.Use();
                }
            }
        }
        else
        {
            Crosshair.sprite = DefaultCrossHairSprite;
        }

        if (Input.GetMouseButton(0))
        {
            if (use && use.CanUse)
            {
                CanCameraRotate = false;
                use.DownUse();
            }
        }
        else
        {
            CanCameraRotate = true;
            use = null;
        }
    }

    private void ShootControlls()
    {
        if (Weapon == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Weapon.PrimaryAttack();
        } 
        else if ( Input.GetMouseButtonDown(1))
        {
            Weapon.Drop();
            Weapon = null;
        }
    }

    /// <summary>
    /// Weapon Sway
    /// </summary>

    [Header("Weapon sway settings")]

    [SerializeField] private float SmoothTime;
    [SerializeField] private float Amount;

    private Vector3 FinalPos;
    private Vector3 InitPos;

    private void WeaponSway( Vector2 mouse )
    {
        mouse *= Amount;

        FinalPos = new Vector3(mouse.x, mouse.y, 0);

        WeaponPos.localPosition = Vector3.Lerp(WeaponPos.localPosition, InitPos - FinalPos, Time.deltaTime * SmoothTime);
    }

    /// <summary>
    /// Foot steps motion
    /// </summary>

    [Header("Foot steps motion")]
    [SerializeField] private float Amplitude = 0.001f;
    [SerializeField] private float Frequency = 10f;

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;

        pos.y += Mathf.Sin(Time.time * Frequency) * Amplitude;
        pos.x += Mathf.Cos(Time.time * Frequency / 2) * Amplitude / 2;

        return pos;
    }

    private void PlayMotion(Vector3 motion)
    {
        WeaponPos.localPosition += motion;
    }

    private void CheckMotion()
    {
        if (targetDir != Vector2.zero)
        {
            PlayMotion(FootStepMotion());
        }
    }

    private void ResetPosition()
    {
        if (WeaponPos.localPosition == InitPos) return;

        WeaponPos.localPosition = Vector3.Lerp(WeaponPos.localPosition, InitPos, 1 * Time.deltaTime);
    }

    /// <summary>
    /// Push rigid body objects
    /// </summary>

    [SerializeField] private float PushPower = 0.5f;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic) return;
        if (hit.moveDirection.y < -0.3f) return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity += (pushDir * PushPower) / body.mass;
    }

    public void LoadSave(Save.PlayerSaveData save)
    {
        PlayerController.enabled = false;

        transform.position = new Vector3(save.Position.x, save.Position.y, save.Position.z);
        transform.rotation = new Quaternion(save.Rotation.x, save.Rotation.y, save.Rotation.z, save.Rotation.w);

        PlayerController.enabled = true;

        if (save.WeaponName == null) return;

        Weapon = GameObject.Find(save.WeaponName).GetComponent<WeaponBase>();

        Weapon.Take(WeaponPos);
    }
}
