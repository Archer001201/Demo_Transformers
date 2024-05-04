using System.Collections;
using System.Collections.Generic;
using DataSO;
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
        public GameObject copyableSign;
        public GameObject detectedCopyable;
        // public GameObject detectedSavingPoint;
        public List<BaseModule> modules;
        public LevelDataSO levelData;
        public float fallingThreshold;
        public bool isFallingHurt;
        public Camera mainCamera;

        private Rigidbody _rb;
        public InputControls inputControls;
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
            mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
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
            inputControls.Enable();
            EventHandler.enableJump += OnEnableJump;
        }

        private void OnDisable()
        {
            inputControls.Disable();
            EventHandler.enableJump -= OnEnableJump;
        }

        private void Update()
        {
            DetectCopyableItem();
            if (!isGrounded && _rb.velocity.y < fallingThreshold) isFallingHurt = true;
            if (!isGrounded && _rb.velocity.y < fallingThreshold*2) EventHandler.OnGameOver();
        }

        private void FixedUpdate()
        {
            Rotation();
            Move();
            if (Physics.CheckSphere(playerBase.position, 0.5f, groundLayer))
            {
                isGrounded = true;
                if (isFallingHurt)
                {
                    _playerAttribute.health--;
                    EventHandler.OnUpdateAttributePanel(Attribute.Health, false);
                    isFallingHurt = false;
                }
            }
            else
            {
                isGrounded = false;
            }
        }

        private void SetInputControls()
        {
            inputControls = new InputControls();
            inputControls.Gameplay.Look.performed += OnLook;
            inputControls.Gameplay.Move.performed += OnMovePerformed;
            inputControls.Gameplay.Move.canceled += OnMoveCanceled;
            inputControls.Gameplay.Jump.performed += OnJump;
            inputControls.Gameplay.Scan.performed += OnCtrlPressed;
            inputControls.Gameplay.Scan.canceled += OnCtrlReleased;
            inputControls.Gameplay.Copy.performed += OnCopy;
            inputControls.Gameplay.Paste.performed += OnPaste;
            inputControls.Gameplay.Revert.performed += OnRevert;
            // inputControls.Gameplay.Interact.performed += OnInteract;
            inputControls.Enable();
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
            if (isGrounded)
            {
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                // Debug.Log("jump");
            }
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
            if (_playerAttribute.energy > 0)
            {
                EventHandler.OnUpdateAttributePanel(Attribute.Energy, false);
                _playerAttribute.energy--;
            }
            else
            {
                EventHandler.OnUpdateAttributePanel(Attribute.Health, false);
                _playerAttribute.health--;
            }
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
            
            if (_playerAttribute.history.Count >= levelData.historyRecords)
            {
                _playerAttribute.history.RemoveAt(0);
                EventHandler.OnRemoveModuleFromHistory(0);
            }

            if (temp != Module.Empty)
            {
                _playerAttribute.history.Add(temp);
                EventHandler.OnAddModuleToHistory(temp);
            }
            
            ChangeCurrentModule();
        }

        private void OnRevert(InputAction.CallbackContext context)
        {
            var history = _playerAttribute.history;
            if (history.Count < 1 || !isScanning) return;
            var removeIndex = history.Count - 1;
            _playerAttribute.currentModule = history[^1];
            _playerAttribute.history.RemoveAt(removeIndex);
            // _playerAttribute.clipboard = Module.Empty;
            EventHandler.OnUpdateCurrentText(_playerAttribute.currentModule);
            EventHandler.OnRemoveModuleFromHistory(removeIndex);
            // EventHandler.OnUpdateClipboardText(_playerAttribute.clipboard);
            
            ChangeCurrentModule();
        }

        // private void OnInteract(InputAction.CallbackContext context)
        // {
        //     if (detectedSavingPoint != null)
        //     {
        //         detectedSavingPoint.GetComponent<SavingPoint>().SaveAndRecover();
        //     }
        // }

        private void OnEnableJump(bool result)
        {
            if (result) inputControls.Gameplay.Jump.Enable();
            else inputControls.Gameplay.Jump.Disable();
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
            _rb.angularVelocity = Vector3.zero;
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
            // 获取层的索引
            var playerLayer = LayerMask.NameToLayer("Player");
            var uiLayer = LayerMask.NameToLayer("Ignore Raycast");

            // 创建一个 LayerMask，其中玩家层和UI层都被忽略
            int layerMask = ~( (1 << playerLayer) | (1 << uiLayer) );

            // 从摄像机的屏幕中心位置发射一条射线，忽略玩家层和UI层
            var ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out var hit, rayDetectionRange, layerMask))
            {
                var detectedObj = hit.collider.gameObject;
                // Debug.Log(detectedObj.name);

                if (detectedObj.CompareTag("Copyable"))
                {
                    copyableSign.SetActive(true);
                    detectedCopyable = detectedObj;
                }
                else
                {
                    copyableSign.SetActive(false);
                    detectedCopyable = null;
                }

                // detectedSavingPoint = detectedObj.CompareTag("SavingPoint") ? detectedObj : null;
            }
            else
            {
                copyableSign.SetActive(false);
                detectedCopyable = null;
                // detectedSavingPoint = null;
            }

            Debug.DrawRay(ray.origin, ray.direction * rayDetectionRange, Color.red);
        }

        public void ChangeCurrentModule()
        {
            var newCurrentModule = _playerAttribute.currentModule;

            foreach (var m in modules)
            {
                m.enabled = m.module == newCurrentModule;
            }
        }
    }
}
