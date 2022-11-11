using Player;
using UnityEngine;

namespace Potions
{
    public interface IPotion
    {
        public bool IsActive { get; }
        public Sprite Sprite { get; }

        public void Drink(PlayerController player, PlayerActions playerActions);
        public GameObject Throw();
    }
}