using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fairy
{
    public class FightWindow : MonoBehaviour
    {
        private UniTaskCompletionSource<bool> _completionSource;
        public Image HeroImage;
        public Image EnemyImage;
        public HeroActionButtons ActionButtons;
        public event Action<int> ActionSelected;
        public event Action RepeatClicked;
        public event Action RestartClicked;
        [SerializeField] private Button RepeatButton;
        [SerializeField] private Button RestartButton;
        [SerializeField] private GameObject LosePanel;
        [SerializeField] private TMP_Text HeroHealthText;
        [SerializeField] private TMP_Text EnemyHealth;

        private void Awake()
        {
            Hide();
        }

        public UniTask<bool> ShowAsync()
        {
            gameObject.SetActive(true);
            _completionSource = new UniTaskCompletionSource<bool>();
            return _completionSource.Task;
        }

        private void SubscribeButtons()
        {
            ActionButtons.NodeClicked += OnActionClicked;
        }

        private void OnActionClicked(int index)
        {
            ActionSelected?.Invoke(index);
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

        private void UnsubscribeButtons()
        {
            ActionButtons.NodeClicked -= OnActionClicked;
        }

        public void Initialize(Hero hero, Hero enemy)
        {
            HeroImage.sprite = hero.StaticData.FightSprite;
            EnemyImage.sprite = enemy.StaticData.FightSprite;
            SetHealth(hero, HeroHealthText);
            SetHealth(enemy, EnemyHealth);
        }

        private void SetHealth(Hero hero, TMP_Text heroHealthText)
        {
            heroHealthText.text = $"HP: {hero.Stats.Get(StatType.HealthPoints)}/{hero.Stats.Get(StatType.MaxHealthPoints)}";
        }

        public void ShowActions(Hero hero)
        {
            SubscribeButtons();
            ActionButtons.HideButtons();
            int actionsCount = hero.HeroActions.Actions.Count;
            for (var index = 0; index < actionsCount; index++)
            {
                ActionData action = hero.HeroActions.Actions[index];
                ActionButtons.AddButton(action.UseText, index);
            }

            var showedItemId = new List<string>();
            IReadOnlyList<Item> consumables = hero.Inventory.Consumables;
            for (var index = 0; index < consumables.Count; index++)
            {
                Item item = consumables[index];
                string itemId = item.ItemStaticData.ItemId;
                if (showedItemId.Contains(itemId))
                    continue;

                showedItemId.Add(itemId);
                int count = CountSameItemsCount(index, consumables, itemId);

                string buttonText = item.ItemStaticData.UseText;
                if (count > 1)
                    buttonText += $" ({count})";
                ActionButtons.AddButton(buttonText, index + actionsCount);
            }
        }

        private static int CountSameItemsCount(int index, IReadOnlyList<Item> consumables, string itemId)
        {
            int count = 1;
            for (var j = index + 1; j < consumables.Count; j++)
            {
                Item item2 = consumables[j];
                if (item2.ItemStaticData.ItemId == itemId)
                    count++;
            }

            return count;
        }

        public void HideActions()
        {
            ActionButtons.HideButtons();
            UnsubscribeButtons();
        }

        public void ShowActionResult(Hero hero, Hero enemy, ActionResult actionResult)
        {
            SetHealth(hero, HeroHealthText);
            SetHealth(enemy, EnemyHealth);
        }

        public void ShowLose()
        {
            LosePanel.SetActive(true);
            RepeatButton.onClick.AddListener(OnRepeatClicked);
            RestartButton.onClick.AddListener(OnRestartClicked);
        }

        public void HideLose()
        {
            LosePanel.SetActive(false);
            RepeatButton.onClick.RemoveListener(OnRepeatClicked);
            RestartButton.onClick.RemoveListener(OnRestartClicked);
        }

        private void OnRestartClicked() => RestartClicked?.Invoke();

        private void OnRepeatClicked() => RepeatClicked?.Invoke();
    }
}