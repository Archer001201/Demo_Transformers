using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Props
{
    public class DialogueController : MonoBehaviour
    {
        public GameObject dialogueCanvas;
        public TextMeshProUGUI dialogueText;
        public bool canTalk;
        [TextArea]
        public List<string> dialogueList;
        public int dialogueIndex;
        private InputControls _inputControls;
        private Transform _cameraTrans;

        private void Awake()
        {
            dialogueCanvas.SetActive(false);
            _inputControls = new InputControls();
            _inputControls.Gameplay.Interact.performed += OnNextDialogue;
            _cameraTrans = GameObject.FindWithTag("MainCamera").transform;
            dialogueIndex = 0;
        }

        private void Start()
        {
            DisplayDialogueText();
        }

        private void OnEnable()
        {
            _inputControls.Enable();
        }

        private void OnDisable()
        {
            _inputControls.Disable();
        }

        private void Update()
        {
            if (dialogueCanvas.activeSelf)
            {
                dialogueCanvas.transform.LookAt(_cameraTrans);
                dialogueCanvas.transform.Rotate(0,180,0);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || dialogueList.Count < 1) return;
            dialogueCanvas.SetActive(true);
            canTalk = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            dialogueCanvas.SetActive(false);
            canTalk = false;
        }

        private void OnNextDialogue(InputAction.CallbackContext value)
        {
            if (!canTalk) return;
            if (dialogueIndex < dialogueList.Count - 1) dialogueIndex++;
            else dialogueIndex = 0;

            DisplayDialogueText();
        }

        private void DisplayDialogueText()
        {
            if (dialogueList.Count < 1) return;
            dialogueText.text = dialogueList[dialogueIndex];
        }
    }
}
