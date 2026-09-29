using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class LouiseController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float maxCameraAngle;
    [SerializeField] private Transform attachmentTransform;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private CinemachinePanTilt _panTilt; //kamerada x ve y rotasyonunu
    private CinemachineInputAxisController _inputAxisController; //mouseu okuyup pantilt'e ileten bileþen
    private GameObject _attachItem;
    public GameObject AttachedItem => _attachItem; 

    private Rigidbody _rb;
    private Animator _animator;
    private Vector3 _moveDirection;
    private Vector2 _moveInput;
    private IEnumerator _interpCoroutine;

    private bool _isPressed;
    private bool _cameraControl = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        if (cinemachineCamera != null)
        {
            _panTilt = cinemachineCamera.GetComponent<CinemachinePanTilt>();
            _inputAxisController = cinemachineCamera.GetComponent<CinemachineInputAxisController>();
        }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        if (!_cameraControl)
        {
            return;
        }

        if (Camera.main != null)
        {
            float targetYAngle = Camera.main.transform.eulerAngles.y; //camera'nýn y eksenindeki açýsýný alýyoruz
            _rb.MoveRotation(Quaternion.Euler(0, targetYAngle, 0)); //karakterin yönü camera yönüne eþit olacak
        }

        if (_isPressed)
        {
            _moveDirection = (transform.forward * _moveInput.y + transform.right * _moveInput.x) * moveSpeed;
            _rb.linearVelocity = new Vector3(_moveDirection.x, _rb.linearVelocity.y, _moveDirection.z);
        }
    }


    public void SetController(bool isActive)
    {
        _cameraControl = isActive;

        if (_inputAxisController != null)
        {
            _inputAxisController.enabled = isActive;
        }

        if (_panTilt != null)
        {
            _panTilt.enabled = isActive;
        }
    }

    public void MoveCharacter(Vector2 moveVector, bool isPressed)
    {
        if (!_cameraControl)
        {
            return;
        }
        _isPressed = isPressed;
        
        if (!_isPressed) //kayma hissiyatýný yok etmek için
        {
            _rb.linearVelocity = Vector3.zero;
        }

        if (_interpCoroutine != null)
        {
            StopCoroutine(_interpCoroutine); //Hareketi yumuþatmak için oluþturduðumuz coroutine'i durduruyoruz. Çünkü hareketin hedef vektörünü güncelledik ve yeni bir hareket baþlatacaðýz. O yüzden eski hareketin coroutine'ini durduruyoruz
        }

        _interpCoroutine = InterpolateMovement(moveVector); //Hareketi yumuþatmak için oluþturduðumuz coroutine'i baþlatmadan önce yeni bir hareket baþlatacaðýmýz için eski hareketin coroutine'ini durduruyoruz. O yüzden StopCoroutine ile eski coroutine'i durduruyoruz ve yeni bir coroutine oluþturuyoruz

        StartCoroutine(_interpCoroutine); //Hareketi yumuþatmak için oluþturduðumuz coroutine'i baþlatýyoruz. Bu coroutine hareketin hedef vektörüne doðru yumuþak bir þekilde ilerlemesini saðlayacak
    }
    public void SendRaycast()
    {
        if (_attachItem != null)
        {
            DetachItem();
        }
        else
        {
            Transform raycastSource = cinemachineCamera != null ? cinemachineCamera.transform : cameraTransform;

            if (Physics.Raycast(raycastSource.position, raycastSource.forward, out RaycastHit hit, 150f))
            {
                IInteractable interaction = hit.collider.GetComponent<IInteractable>();
                if (interaction != null)
                {
                    interaction.OnInteract(gameObject);
                }
            }
        }
    }
    public void AttachItem(GameObject item)
    {
        item.transform.position = attachmentTransform.position;
        item.transform.rotation = attachmentTransform.rotation;
        item.transform.parent = attachmentTransform;

        _attachItem = item;
    }

    public void DetachItem()
    {
        if (_attachItem)
        {
            IAttachable attachable = _attachItem.GetComponent<IAttachable>();
            attachable.OnDetach(gameObject);
            _attachItem.transform.parent = null;
            _attachItem = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interaction = other.transform.GetComponent<IInteractable>();
        if (interaction != null)
        {
            interaction.OnInteract(gameObject);
        }
    }

    private IEnumerator InterpolateMovement(Vector2 targetVector) //Bu coroutine hareketin hedef vektörüne doðru yumuþak bir þekilde ilerlemesini saðlayacak
    {
        while (_moveInput != targetVector)
        {
            _moveInput = Vector2.MoveTowards(_moveInput, targetVector, 7.5f * Time.deltaTime); // _moveInput'u targetVector'e doðru hareket ettiriyoruz yani interpolate ediyoruz.

            _animator.SetFloat("ForwardValue", _moveInput.y);
            _animator.SetFloat("RightValue", _moveInput.x);

            yield return new WaitForEndOfFrame();
        }
    }
    public void PlayRightStepSfx()
    {
        AudioManager.Instance.PlayRightStepSfx();
    }

    public void PlayLeftStepSfx()
    {
        AudioManager.Instance.PlayLeftStepSfx();
    }
}