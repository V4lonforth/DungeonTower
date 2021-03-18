using DungeonTower.Level.Base;
using UnityEngine;

namespace DungeonTower.Entity.Base
{
    public abstract class CellEntity : MonoBehaviour
    {
        [SerializeField] private EntityTeam team;

        public EntityTeam Team => team;
        public Cell Cell { get; protected set; }
        public GameObject GameObject => gameObject;

        public abstract void Attach(Cell cell);
        public abstract void Detach();

        public void Destroy()
        {
            Destroy(GameObject);
        }

        private void OnDestroy()
        {
            Detach();
        }
    }
}
