using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fairy
{
    internal class ButtonWithText : MonoBehaviour
    {
        public TMP_Text Text;
        public Button Button;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}