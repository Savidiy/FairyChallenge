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

        public async UniTask ShowAsync()
        {
            await UniTask.NextFrame();
            gameObject.SetActive(true);
            StartButton.onClick.AddListener(OnStartButtonClick);
            _completionSource = new UniTaskCompletionSource();
            await _completionSource.Task;
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