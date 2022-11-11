using Player;
using UnityEngine;

namespace Potions
{
    public abstract class AbstractPotion : ScriptableObject, IPotion
    {
        public bool IsActive => false;
        
        public Sprite Sprite => _sprite;

        [SerializeField] private Sprite _sprite;
        [SerializeField] private GameObject potionPrefab;
        public GameObject particleSystem;

        public abstract void Drink(PlayerController player, PlayerActions playerActions);

        public GameObject Throw()
        {
            return potionPrefab;
        }
    }
}