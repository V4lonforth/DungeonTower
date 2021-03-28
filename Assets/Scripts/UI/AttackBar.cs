using DungeonTower.Entity.Attack;
using TMPro;
using UnityEngine;

namespace DungeonTower.UI
{
    public class AttackBar : MonoBehaviour
    {
        [SerializeField] private EntityAttack attackController;
        [SerializeField] private TextMeshPro text;

        private void Awake()
        {
            DisplayAttack();
        }

        private void DisplayAttack()
        {
            text.text = attackController.AttackDamage.ToString();
        }
    }
}
