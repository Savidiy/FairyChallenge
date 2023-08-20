using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fairy
{
    public sealed class ActionButtons : MonoBehaviour
    {
        [SerializeField] private List<ButtonWithText> Buttons;
        private readonly Dictionary<ButtonWithText, string> _buttonToNodeId = new();
        public event Action<string> NodeClicked;

        private void Awake()
        {
            foreach (ButtonWithText buttonWithText in Buttons)
                buttonWithText.OnClicked += OnButtonClicked;

            HideButtons();
        }

        private void OnDestroy()
        {
            foreach (ButtonWithText buttonWithText in Buttons)
                buttonWithText.OnClicked -= OnButtonClicked;
        }

        public void HideButtons()
        {
            _buttonToNodeId.Clear();
            foreach (ButtonWithText button in Buttons)
                button.SetActive(false);
        }

        public void SendClick(string nodeId)
        {
            NodeClicked?.Invoke(nodeId);
        }

        public void AddButton(string buttonText, string nextNodeId)
        {
            ButtonWithText button = Buttons.Find(b => !b.IsActive);
            if (button == null)
            {
                Debug.LogError("No free buttons");
                return;
            }

            button.SetActive(true);
            button.Text.text = buttonText;
            _buttonToNodeId.Add(button, nextNodeId);
        }

        private void OnButtonClicked(ButtonWithText obj)
        {
            if (!_buttonToNodeId.TryGetValue(obj, out string nextNodeId))
            {
                Debug.LogError("Can't find next node id");
                return;
            }

            NodeClicked?.Invoke(nextNodeId);
        }
    }
}