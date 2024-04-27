using Props;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;
using EventHandler = Utilities.EventHandler;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public Transform cameraFollowPosition;
        public float rotationSensitivity = 0.3f;
        public float moveSpeed = 6f;
        public float jumpForce = 5f;
        public LayerMask groundLayer;
        public Transform playerBase;
        public bool isGrounded;
        public bool isScanning;
        public float rayDetectionRange = 20f;
        public string targetTag;
        public GameObject copyableSign;
        public GameObject detectedCopyable;

        private Rigidbody _rb;
        private InputControls _input;
        private Vector2 _look;
        private Vector2 _move;
        private float _rotationX;
        private PlayerAttribute _playerAttribute;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _playerAttribute = GetComponent<PlayerAttribute>();
            copyableSign.SetActive(false);
            SetInputControls();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            EventHandler.OnUpdateCurrentText(_playerAttribute.currentModule);
            EventHandler.OnUpdateClipboardText(_playerAttribute.clipboard);
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void Update()
        {
            DetectCopyableItem();
        }

        private void FixedUpdate()
        {
            Rotation();
            Move();
            isGrounded = Physics.CheckSphere(playerBase.position, 0.5f, groundLayer);
        }

        private void SetInputControls()
        {
            _input = new InputControls();
            _input.Gameplay.Look.performed += OnLook;
            _input.Gameplay.Move.performed += OnMovePerformed;
            _input.Gameplay.Move.canceled += OnMoveCanceled;
            _input.Gameplay.Jump.performed += OnJump;
            _input.Gameplay.Scan.performed += OnCtrlPressed;
            _input.Gameplay.Scan.canceled += OnCtrlReleased;
            _input.Gameplay.Copy.performed += OnCopy;
            _input.Gameplay.Paste.performed += OnPaste;
            _input.Gameplay.Revert.performed += OnRevert;
        }

        private void OnLook(InputAction.CallbackContext value)
        {
            _look = value.ReadValue<Vector2>();
        }

        private void OnMovePerformed(InputAction.CallbackContext value)
        {
            _move = value.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext value)
        {
            _move = Vector2.zero;
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (isGrounded) _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void OnCtrlPressed(InputAction.CallbackContext context)
        {
            isScanning = true;
            EventHandler.OnCtrlPressed(true);
        }

        private void OnCtrlReleased(InputAction.CallbackContext context)
        {
            isScanning = false;
            EventHandler.OnCtrlPressed(false);
        }

        private void OnCopy(InputAction.CallbackContext context)
        {
            if (detectedCopyable == null || !isScanning) return;
            _playerAttribute.clipboard = detectedCopyable.GetComponent<CopyableItem>().module;
            EventHandler.OnUpdateClipboardText(_playerAttribute.clipboard);
        }

        private void OnPaste(InputAction.CallbackContext context)
        {
            var temp = _playerAttribute.currentModule;
            var clipboard = _playerAttribute.clipboard;
            if (clipboard == Module.Empty || !isScanning) return;
            _playerAttribute.currentModule = clipboard;
            _playerAttribute.clipboard = Module.Empty;
            EventHandler.OnUpdateCurrentText(_playerAttribute.currentModule);
            EventHandler.OnUpdateClipboardText(_playerAttribute.clipboard);
            
            if (_playerAttribute.history.Count >= 3)
            {
                _playerAttribute.history.RemoveAt(0);
                EventHandler.OnRemoveModuleFromHistory(0);
            }

            if (temp != Module.Empty)
            {
                _playerAttribute.history.Add(temp);
                EventHandler.OnAddModuleToHistory(temp);
            }
        }

        private void OnRevert(InputAction.CallbackContext context)
        {
            var history = _playerAttribute.history;
            if (history.Count < 1 || !isScanning) return;
            var removeIndex = history.Count - 1;
            _playerAttribute.currentModule = history[^1];
            _playerAttribute.history.RemoveAt(removeIndex);
            _playerAttribute.clipboard = Module.Empty;
            EventHandler.OnUpdateCurrentText(_playerAttribute.currentModule);
            EventHandler.OnRemoveModuleFromHistory(removeIndex);
            EventHandler.OnUpdateClipboardText(_playerAttribute.clipboard);
        }

        private void Rotation()
        {
            var mouseX = _look.x * rotationSensitivity; 
            var mouseY = _look.y * rotationSensitivity; 

            _rotationX -= mouseY;
            _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);
            
            cameraFollowPosition.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
            
            _look = Vector2.zero;
        }

        private void Move()
        {
            var forwardMovement = transform.forward * _move.y;
            var strafeMovement = transform.right * _move.x;

            var moveVector = (forwardMovement + strafeMovement) * moveSpeed;

            _rb.MovePosition(_rb.position + moveVector * Time.fixedDeltaTime);
        }

        private void DetectCopyableItem()
        {
            var ray = new Ray(cameraFollowPosition.position, transform.forward);

            if (Physics.Raycast(ray, out var hit, rayDetectionRange))
            {
                var detectedObj = hit.collider.gameObject;
                if (detectedObj.CompareTag(targetTag))
                {
                    copyableSign.SetActive(true);
                    detectedCopyable = detectedObj;
                }
                else
                {
                    copyableSign.SetActive(false);
                    detectedCopyable = null;
                }
            }
            else
            {
                copyableSign.SetActive(false);
                detectedCopyable = null;
            }

            Debug.DrawRay(ray.origin, ray.direction * rayDetectionRange, Color.red);
        }
    }
}
