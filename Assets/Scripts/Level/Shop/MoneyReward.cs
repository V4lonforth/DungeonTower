using DungeonTower.Entity.Health;
using UnityEngine;

namespace DungeonTower.Level.Shop
{
    public class MoneyReward : MonoBehaviour
    {
        [SerializeField] private int reward;
        
        private void Awake()
        {
            GetComponent<EntityHealth>().OnDeath += GiveReward;
        }

        private void GiveReward(EntityHealth entityHealth)
        {
            MoneyController.Instance.AddMoney(reward);
        }
    }
}