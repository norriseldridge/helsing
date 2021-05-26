using Helsing.Client.Player.Api;
using Helsing.Client.World.Api;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Player
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField]
        float speed;

        [SerializeField]
        PlayerView view;

        [SerializeField]
        float moveDelay;

        float currentDelay;

        public ITile CurrentTile => currentTile;

        [Inject]
        private void Inject(ITileMap tileMap) =>
            this.tileMap = tileMap;

        private void Start()
        {
            currentTile = tileMap.TileAt(transform.position);
            currentDelay = 0;
        }

        private void Update()
        {
            currentDelay -= Time.deltaTime;
            if (nextTile == null)
            {
                if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && currentDelay <= 0)
                {
                    if (CanMove(Direction.Up))
                    {
                        nextTile = currentTile.GetNeighbor(Direction.Up);
                        currentDelay = moveDelay;
                    }
                }

                if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && currentDelay <= 0)
                {
                    if (CanMove(Direction.Down))
                    {
                        nextTile = currentTile.GetNeighbor(Direction.Down);
                        currentDelay = moveDelay;
                    }
                }

                if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && currentDelay <= 0)
                {
                    if (view.FlipX && CanMove(Direction.Left))
                        nextTile = currentTile.GetNeighbor(Direction.Left);
                    else
                        view.FlipX = true;

                    currentDelay = moveDelay;
                }

                if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && currentDelay <= 0)
                {
                    if (!view.FlipX && CanMove(Direction.Right))
                        nextTile = currentTile.GetNeighbor(Direction.Right);
                    else
                        view.FlipX = false;

                    currentDelay = moveDelay;
                }
            }
            else
            {
                MoveToNext();
            }

            if (!Input.anyKey)
                currentDelay = 0;
        }

        private bool CanMove(Direction direction) => currentTile.GetNeighbor(direction)?.IsFloor ?? false;

        private void MoveToNext()
        {
            view.State = EntityState.Walk;
            var targetPosition = new Vector3(nextTile.Position.x, nextTile.Position.y, transform.position.z);
            if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                return;
            }
            view.State = EntityState.Idle;
            transform.position = targetPosition;
            currentTile = nextTile;
            nextTile = null;
        }

        ITileMap tileMap;
        ITile currentTile;
        ITile nextTile;
    }
}