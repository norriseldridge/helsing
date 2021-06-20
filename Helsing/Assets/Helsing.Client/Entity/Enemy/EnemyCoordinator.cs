﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.World.Api;

namespace Helsing.Client.Entity.Enemy
{
    public class EnemyCoordinator : IEnemyCoordinator
    {
        readonly Dictionary<EnemyLogicType, IEnemyLogic> logicCache;

        public EnemyCoordinator(IEnemyLogicFactory logicFactory)
        {
            logicCache = new Dictionary<EnemyLogicType, IEnemyLogic>();
            var types = System.Enum.GetValues(typeof(EnemyLogicType));
            foreach (EnemyLogicType logicType in types)
                logicCache[logicType] = logicFactory.Create(logicType);
        }

        public async Task<IEnumerable<ITile>> GetMoves(EnemyLogicType logicType, int maxMoves, ITile startingTile)
        {
            var logic = logicCache[logicType];
            
            var currentTile = startingTile;
            var moves = new List<ITile>();
            for (var i = 0; i < maxMoves; ++i)
            {
                currentTile = await logic.PickDestinationTile(currentTile);
                if (currentTile != null && !moves.Contains(currentTile))
                {
                    if (logic.CanShareTile || CanMoveToTile(currentTile))
                        moves.Add(currentTile);
                }
            }

            return moves;
        }

        bool CanMoveToTile(ITile tile)
        {
            var enemies = tile.GetGameObjectsOnTileOfType<IEnemy>();
            return enemies.Where(e => !logicCache[e.GetComponent<IEnemy>().EnemyLogicType].CanShareTile).Count() == 0;
        }
    }
}
