using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Fairy
{
    public class FightWindow : MonoBehaviour
    {
        public Button WinButton;
        public Button LoseButton;
        private UniTaskCompletionSource<bool> _completionSource;

        private void Awake()
        {
            Hide();
        }

        public UniTask<bool> ShowAsync()
        {
            gameObject.SetActive(true);
            SubscribeButtons();
            _completionSource = new UniTaskCompletionSource<bool>();
            return _completionSource.Task;
        }

        private void SubscribeButtons()
        {
            WinButton.onClick.AddListener(OnWinClick);
            LoseButton.onClick.AddListener(OnLoseClick);
        }

        public UniTask HideAsync()
        {
            Hide();
            return UniTask.CompletedTask;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnWinClick()
        {
            UnsubscribeButtons();
            _completionSource.TrySetResult(true);
        }

        private void UnsubscribeButtons()
        {
            WinButton.onClick.RemoveListener(OnWinClick);
            LoseButton.onClick.RemoveListener(OnLoseClick);
        }

        private void OnLoseClick()
        {
            UnsubscribeButtons();
            _completionSource.TrySetResult(false);
        }
    }
}