using Player;

namespace Potions
{
    public interface IPotion
    {
        public bool IsActive { get; }
        public void Drink(PlayerController player);
        public void Throw();
    }
}
