using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fairy
{
    public sealed class ActionButtons : MonoBehaviour
    {
        [SerializeField] private List<ButtonWithText> Buttons;

        private void Awake()
        {
            HideButtons();
        }

        public void HideButtons()
        {
            foreach (ButtonWithText button in Buttons)
                button.SetActive(false);
        }
        
        public void SetButtons(List<string> actions)
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                ButtonWithText button = Buttons[i];
                if (i < actions.Count)
                {
                    button.SetActive(true);
                    button.Text.text = actions[i];
                }
                else
                {
                    button.SetActive(false);
                }
            }
        }
    }
}