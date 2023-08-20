using TMPro;
using UnityEngine;

namespace Fairy
{
    public class StoryWindow : MonoBehaviour
    {
        public TMP_Text PersonText;
        public TMP_Text DialogText;
        public ActionButtons ActionButtons;

        private void Awake()
        {
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}