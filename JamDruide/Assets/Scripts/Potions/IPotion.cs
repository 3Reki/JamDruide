using Player;

namespace Potions
{
    public interface IPotion
    {
        public void Drink(PlayerController player);
        public void Throw();
    }
}
