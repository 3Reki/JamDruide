﻿using Player;
using UnityEngine;

namespace Potions
{
    public abstract class AbstractPotion : ScriptableObject, IPotion
    {
        public bool IsActive => false;
        
        public Sprite Sprite => _sprite;

        [SerializeField] private Sprite _sprite;
        [SerializeField] private GameObject potionPrefab;

        public abstract void Drink(PlayerController player);

        public GameObject Throw()
        {
            return potionPrefab;
        }
    }
}