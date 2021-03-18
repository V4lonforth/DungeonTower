using UnityEngine;

namespace DungeonTower.Utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (instance == null)
                    {
                        GameObject gameObject = new GameObject(typeof(T).Name);
                        instance = gameObject.AddComponent(typeof(T)) as T;
                    }
                }

                return instance;
            }
        }
    }
}
