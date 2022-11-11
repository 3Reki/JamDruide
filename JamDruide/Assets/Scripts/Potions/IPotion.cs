using Player;
using UnityEngine;

namespace Potions
{
    public interface IPotion
    {
        public bool IsActive { get; }
        public Sprite Sprite { get; }

        public void Drink(PlayerController player);
        public GameObject Throw();
    }
}