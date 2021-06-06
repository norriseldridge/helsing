using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helsing.Client.Entity.Api;
using Helsing.Client.World.Api;
using UniRx;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Entity
{
    public class TileMover : MonoBehaviour, ITileMover
    {
        public static List<GameObject> GetObjectsOnTile(ITile tile)
        {
            var tileMovers = FindObjectsOfType<MonoBehaviour>().OfType<TileMover>();
            var gameObjects = new List<GameObject>();
            foreach (var mover in tileMovers)
            {
                if (mover.CurrentTile.Value == tile)
                    gameObjects.Add(mover.gameObject);
            }
            return gameObjects;
        }

        [SerializeField]
        private float speed;

        public IReadOnlyReactiveProperty<ITile> CurrentTile => currentTile;
        public IReadOnlyReactiveProperty<ITile> NextTile => nextTile;

        readonly IReactiveProperty<ITile> currentTile = new ReactiveProperty<ITile>();
        readonly IReactiveProperty<ITile> nextTile = new ReactiveProperty<ITile>();

        IMessageBroker broker;
        ITileMap tileMap;

        [Inject]
        private void Inject(IMessageBroker broker, ITileMap tileMap) =>
            (this.broker, this.tileMap) = (broker, tileMap);

        private void Start()
        {
            currentTile.Value = tileMap.TileAt(transform.position);
            nextTile.Value = null;
        }

        private void Update() => MoveToNext();

        public async Task MoveTo(ITile newNextTile)
        {
            nextTile.Value = newNextTile;
            await nextTile.Where(n => n == null).Take(1).ToTask();
        }

        public bool CanMove(Direction direction) => currentTile.Value.GetNeighbor(direction)?.IsFloor ?? false;

        private void MoveToNext()
        {
            if (nextTile.Value == null) return;
            var targetPosition = new Vector3(nextTile.Value.Position.x, nextTile.Value.Position.y, transform.position.z);
            if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                return;
            }
            transform.position = targetPosition;
            currentTile.Value = nextTile.Value;
            nextTile.Value = null;
            broker.Publish(new TileMoverMovedMessage(this));
        }
    }
}
