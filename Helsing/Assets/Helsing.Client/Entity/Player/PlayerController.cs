using System.Threading.Tasks;
using Helsing.Client.Entity.Api;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.World.Api;
using UniRx;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Entity.Player
{
    [RequireComponent(typeof(ILiving))]
    [RequireComponent(typeof(ITileMover))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField]
        PlayerView view;

        public ILiving Living => living;
        public ITile CurrentTile => tileMover.CurrentTile.Value;

        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        public bool Visibility
        {
            get => view.Visible;
            set => view.Visible = value;
        }

        public bool IsHidden => CurrentTile?.IsHidingSpot ?? false;

        IMessageBroker broker;
        ILiving living;
        ITileMover tileMover;
        bool isTurn = false;
        IReactiveProperty<ITile> destinationTile = new ReactiveProperty<ITile>();

        [Inject]
        private void Inject(IMessageBroker broker) =>
            this.broker = broker;

        private void Awake()
        {
            tileMover = GetComponent<ITileMover>();
            living = GetComponent<ILiving>();
        }

        private void Start()
        {
            broker.Receive<HealthPickUpMessage>()
                .Subscribe(_ => living.AddLife())
                .AddTo(this);
        }

        private void Update()
        {
            view.Visible = !IsHidden;
            if (!isTurn) return;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                if (tileMover.CanMove(Direction.Up))
                {
                    destinationTile.Value = tileMover.CurrentTile.Value.GetNeighbor(Direction.Up);
                }
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (tileMover.CanMove(Direction.Down))
                {
                    destinationTile.Value = tileMover.CurrentTile.Value.GetNeighbor(Direction.Down);
                }
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (view.FlipX && tileMover.CanMove(Direction.Left))
                    destinationTile.Value = tileMover.CurrentTile.Value.GetNeighbor(Direction.Left);
                else
                    view.FlipX = true;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (!view.FlipX && tileMover.CanMove(Direction.Right))
                    destinationTile.Value = tileMover.CurrentTile.Value.GetNeighbor(Direction.Right);
                else
                    view.FlipX = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                destinationTile.Value = CurrentTile;
            }
        }

        public async Task TakeTurn()
        {
            isTurn = true;
            // wait for the user to select a destination tile
            destinationTile.Value = null;
            await destinationTile.Where(d => d != null).Take(1);

            // wait for the user to arrive at the next tile
            view.State = EntityState.Walk;
            await tileMover.MoveTo(destinationTile.Value);
            view.State = EntityState.Idle;
            isTurn = false;
        }
    }
}