using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Fairy
{
    public class IntroWindow : MonoBehaviour
    {
        public Button StartButton;
        private UniTaskCompletionSource _completionSource;

        private void Awake()
        {
            Hide();
        }

        public UniTask ShowAsync()
        {
            StartButton.onClick.AddListener(OnStartButtonClick);
            gameObject.SetActive(true);
            _completionSource = new UniTaskCompletionSource();
            return _completionSource.Task;
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

        private void OnStartButtonClick()
        {
            StartButton.onClick.RemoveListener(OnStartButtonClick);
            _completionSource.TrySetResult();
        }
    }
}