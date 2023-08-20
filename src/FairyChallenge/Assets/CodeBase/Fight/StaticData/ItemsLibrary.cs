﻿using System;
using System.Collections.Generic;
using Savidiy.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fight
{
    [CreateAssetMenu(fileName = nameof(ItemsLibrary), menuName = nameof(ItemsLibrary), order = 0)]
    public class ItemsLibrary : AutoSaveScriptableObject
    {
        private readonly Dictionary<ItemType, ValueDropdownList<string>> _itemIdsByType = new();
        private ValueDropdownList<string> _emptyValueDropdownList = new();

        public List<ItemStaticData> Items = new();

        public readonly ValueDropdownList<string> ItemIds = new();

        public ValueDropdownList<string> GetItemIds(ItemType itemType)
        {
            _emptyValueDropdownList = new ValueDropdownList<string>();
            return _itemIdsByType.TryGetValue(itemType, out var result) ? result : _emptyValueDropdownList;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            ItemIds.Clear();
            _itemIdsByType.Clear();
            foreach (ItemStaticData item in Items)
            {
                ItemIds.Add(item.ItemId);
                if (!_itemIdsByType.ContainsKey(item.ItemType))
                    _itemIdsByType.Add(item.ItemType, new ValueDropdownList<string>());

                _itemIdsByType[item.ItemType].Add(item.ItemId);
            }

            SavePrefab();
        }
    }

    [Serializable]
    public class ItemStaticData
    {
        public string ItemId = string.Empty;
        [EnumToggleButtons, LabelWidth(80)] public ItemType ItemType;
        [ListDrawerSettings(AlwaysAddDefaultValue = true)] public List<ItemEffect> Effects = new();
    }

    [Serializable]
    public class ItemEffect
    {
        private const int WIDTH = 100;
        [EnumToggleButtons, LabelWidth(WIDTH)] public ItemEffectType ItemEffectType;

        private bool IsChangeStat => ItemEffectType == ItemEffectType.ChangeStat;
        [ShowIf(nameof(IsChangeStat)), LabelWidth(WIDTH)] public ItemStatType StatType;
        [ShowIf(nameof(IsChangeStat)), LabelWidth(WIDTH)] public int Value;

        private bool IsAddAction => ItemEffectType == ItemEffectType.AddAction;
        [ShowIf(nameof(IsAddAction)), LabelWidth(WIDTH), ValueDropdown(nameof(ActionIds))] public string ActionId;
        private ValueDropdownList<string> ActionIds => OdinActionIdProvider.ActionIds;
    }

    public enum ItemStatType
    {
        Attack = 0,
        Defence = 1,
        HealthPoints = 2,
    }

    public enum ItemEffectType
    {
        AddAction = 0,
        ChangeStat = 1,
    }

    public enum ItemType
    {
        Weapon = 0,
        Armor = 1,
        Accessory = 2,
        Consumable = 3,
    }
}