using System;
using System.Collections.Generic;
using Savidiy.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fairy
{
    [CreateAssetMenu(fileName = nameof(PersonLibrary), menuName = nameof(PersonLibrary), order = 0)]
    public class PersonLibrary : AutoSaveScriptableObject
    {
        public PersonStaticData DefaultPerson;

        [ListDrawerSettings(ListElementLabelName = "@this")]
        public List<PersonStaticData> Persons = new();

        public ValueDropdownList<string> PersonIds = new();

        protected override void OnValidate()
        {
            base.OnValidate();

            PersonIds.Clear();

            PersonIds.Add(DefaultPerson.PersonId);
            foreach (PersonStaticData node in Persons)
                PersonIds.Add(node.PersonId);
        }

        public PersonStaticData GetPersonData(string personId)
        {
            if (personId.Equals(DefaultPerson.PersonId))
                return DefaultPerson;

            foreach (PersonStaticData person in Persons)
                if (person.PersonId.Equals(personId))
                    return person;

            Debug.LogError($"PersonStaticData '{personId}' not found");
            return DefaultPerson;
        }
    }

    [Serializable]
    public class PersonStaticData
    {
        public string PersonId = string.Empty;
        public string Name;
        public Color Color = new (1, 1, 1, 1);
        public override string ToString() => PersonId;
    }
}