using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fairy
{
    public sealed class HeroActionButtons : MonoBehaviour
    {
        [SerializeField] private List<ButtonWithText> Buttons;
        private readonly Dictionary<ButtonWithText, int> _buttonToIndex = new();
        public event Action<int> NodeClicked;

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
            _buttonToIndex.Clear();
            foreach (ButtonWithText button in Buttons)
                button.SetActive(false);
        }

        public void AddButton(string buttonText, int index)
        {
            ButtonWithText button = Buttons.Find(b => !b.IsActive);
            if (button == null)
            {
                Debug.LogError("No free buttons");
                return;
            }

            button.SetActive(true);
            button.Text.text = buttonText;
            _buttonToIndex.Add(button, index);
        }

        private void OnButtonClicked(ButtonWithText button)
        {
            if (_buttonToIndex.TryGetValue(button, out int index))
                NodeClicked?.Invoke(index);
            else
                Debug.LogError("Can't find next node id");
        }
    }
}