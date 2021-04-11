using System;
using DungeonTower.Utils;

namespace DungeonTower.Level.Shop
{
    public class MoneyController : Singleton<MoneyController>
    {
        public int Money { get; private set; }
        
        public Action<int> OnMoneyChanged { get; set; }
        
        public void AddMoney(int money)
        {
            Money += money;
            OnMoneyChanged?.Invoke(Money);
        }
    }
}