using System.Collections.Generic;

namespace Helsing.Client.Entity.Enemy.Api
{
    public interface IEnemyControllerBlackboard
    {
        public IEnumerable<EnemyControllerBlackboardPair> Items { get; set; }
        T Get<T>(string name) where T : UnityEngine.Object;
    }
}
