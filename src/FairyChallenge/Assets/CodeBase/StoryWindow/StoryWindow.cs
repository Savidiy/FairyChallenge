using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fairy
{
    public class StoryWindow : MonoBehaviour
    {
        public Image Background;
        public TMP_Text PersonText;
        public TextPrintView TextPrintView;
        public ActionButtons ActionButtons;

        private void Awake()
        {
            Hide();
            HideTeller();
            TextPrintView.Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetBackground(BackgroundStaticData backgroundStaticData)
        {
            Background.sprite = backgroundStaticData.Sprite;
            Background.color = backgroundStaticData.Color;
        }

        public UniTask ShowDialogAsync(PersonStaticData personStaticData, string text, CancellationToken token)
        {
            string tellerName = personStaticData.Name;
            if (string.IsNullOrEmpty(tellerName))
                HideTeller();
            else
                ShowTeller(personStaticData, tellerName);

            return TextPrintView.ShowAsync(text, token);
        }

        private void ShowTeller(PersonStaticData personStaticData, string tellerName)
        {
            PersonText.gameObject.SetActive(true);
            PersonText.text = tellerName;
            PersonText.color = personStaticData.Color;
        }

        private void HideTeller()
        {
            PersonText.gameObject.SetActive(false);
        }
    }
}