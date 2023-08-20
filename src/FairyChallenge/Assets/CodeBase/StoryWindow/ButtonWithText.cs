using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fairy
{
    internal class ButtonWithText : MonoBehaviour
    {
        public TMP_Text Text;
        [SerializeField] private Button Button;
        public event Action<ButtonWithText> OnClicked;
        public bool IsActive => gameObject.activeSelf;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnEnable()
        {
            Button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            Button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            OnClicked?.Invoke(this);
        }
    }
}