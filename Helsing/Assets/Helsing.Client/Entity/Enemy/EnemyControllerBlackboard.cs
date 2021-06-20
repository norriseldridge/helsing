using System.Collections.Generic;
using System.Linq;
using Helsing.Client.Entity.Enemy.Api;
using UnityEngine;

namespace Helsing.Client.Entity.Enemy
{
    [System.Serializable]
    public struct EnemyControllerBlackboardPair
    {
        public string key;
        public Object value;
    }

    public class EnemyControllerBlackboard : IEnemyControllerBlackboard
    {
        public IEnumerable<EnemyControllerBlackboardPair> Items { get; set; }

        public T Get<T>(string name) where T : Object
        {
            if (Items == null || Items.Count() == 0)
                throw new System.Exception($"Could not locate object by name {name}!");

            return Items.Where(i => i.key == name).FirstOrDefault().value as T;
        }
    }
}
