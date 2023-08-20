using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fairy
{
    public sealed class TextPrintView : MonoBehaviour
    {
        [SerializeField] private TMP_Text Text;
        [SerializeField] private Button SkipButton;
        [SerializeField] private int MaxVisibleCharacters = 9999;
        [SerializeField] private Ease TextTypingEase = Ease.Linear;
        [SerializeField] private int SymbolsPerSecond = 25;

        [CanBeNull] private Tween _tween;
        private string _message;
        private bool _isPrintCompleted;
        private bool _isClicked;

        private void Awake()
        {
            SkipButton.onClick.AddListener(OnSkipClick);
        }

        private CancellationTokenSource _showTokenSource;

        public void Hide()
        {
            _showTokenSource?.Cancel();
            gameObject.SetActive(false);
            SkipButton.gameObject.SetActive(false);
        }

        public async UniTask ShowAsync(string message, CancellationToken token)
        {
            Show();
            _showTokenSource?.Cancel();
            _tween?.Kill();
            _showTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);

            _message = message;
            _isPrintCompleted = false;
            _isClicked = false;
            Text.text = _message;
            Text.ForceMeshUpdate();
            int totalCharacters = Text.text.Length;
            float duration = totalCharacters / (float) SymbolsPerSecond;
            Text.maxVisibleCharacters = 0;
            _tween = DOTween
                .To(() => Text.maxVisibleCharacters,
                    visibleChars => { Text.maxVisibleCharacters = visibleChars; },
                    totalCharacters,
                    duration)
                .SetEase(TextTypingEase);

            _tween.Play();
            await UniTask.WaitWhile(IsTweenPlaying, cancellationToken: _showTokenSource.Token);
            _isPrintCompleted = true;
            Text.maxVisibleCharacters = MaxVisibleCharacters;
            await UniTask.WaitWhile(IsClickWait, cancellationToken: token);
            SkipButton.gameObject.SetActive(false);
        }

        private bool IsClickWait() => !_isClicked;
        private bool IsTweenPlaying() => _tween.IsActive() && _tween.IsPlaying();
        
        private void OnSkipClick()
        {
            if (_isPrintCompleted)
                _isClicked = true;
            else
                ForceFinish();
        }

        private void ForceFinish()
        {
            _tween?.Kill(true);
            _tween = null;
        }

        private void Show()
        {
            gameObject.SetActive(true);
            SkipButton.gameObject.SetActive(true);
        }
    }
}