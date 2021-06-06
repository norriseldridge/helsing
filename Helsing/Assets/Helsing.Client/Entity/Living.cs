using Helsing.Client.Entity.Api;
using UnityEngine;

namespace Helsing.Client.Entity
{
    public class Living : MonoBehaviour, ILiving
    {
        [SerializeField]
        int lives;

        public int Lives => lives;

        public void DealDamage() => lives--;
    }
}
