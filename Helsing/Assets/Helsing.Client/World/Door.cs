using Helsing.Client.Audio.Api;
using Helsing.Client.Item;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.UI.Api;
using Helsing.Client.World.Api;
using UniRx;
using UnityEngine;
using Zenject;

namespace Helsing.Client.World
{
    public enum DoorState
    {
        Unlocked,
        Locked
    }

    public class Door : MonoBehaviour
    {
        [SerializeField]
        ItemConsumer itemConsumer;

        [SerializeField]
        DoorState doorState;

        [SerializeField]
        ItemData keyItem;

        [SerializeField]
        string nextLevel;

        [SerializeField]
        AudioClip lockedSound;

        [SerializeField]
        AudioClip openSound;

        IPlayerController playerController;
        IMessageBroker broker;
        IPromptMessage prompt;
        IAudioPool audioPool;
        Tile parentTile;

        [Inject]
        private void Inject(IPlayerController playerController,
            IMessageBroker broker,
            IPromptMessage prompt,
            IAudioPool audioPool) =>
            (this.playerController, this.broker, this.prompt, this.audioPool) =
            (playerController, broker, prompt, audioPool);

        private void Awake()
        {
            parentTile = GetComponentInParent<Tile>();

            if (parentTile == null)
                throw new System.Exception("Doors must be a child of a Tile!");
        }

        private void Update()
        {
            if (doorState == DoorState.Locked)
            {
                if (PlayerIsNeighbor())
                {
                    prompt.SetMessage("Press [SPACE] to open door.");

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (itemConsumer.Consume(keyItem, 1))
                        {
                            audioPool.Next().PlayOneShot(openSound);
                            doorState = DoorState.Unlocked;
                            parentTile.IsFloor = true;
                            broker.Publish(new MessageData("Door unlocked."));
                        }
                        else
                        {
                            audioPool.Next().PlayOneShot(lockedSound);
                            broker.Publish(new MessageData("Door is locked."));
                        }
                    }
                }
            }

            if ((ITile)parentTile == playerController.CurrentTile)
            {
                LoadNextLevel();
            }
        }

        private async void LoadNextLevel()
        {
            // disable the door because we're done and moving on to the next level
            enabled = false;

            // disable the player as well
            playerController.Enabled = false;

            // wait for the fade
            broker.Publish(new FadeData(true));
            await broker.Receive<FadeCompleteMessage>().Take(1).ToTask();

            // TODO fade out, load, fade in
            LevelLoader.Load(nextLevel);
        }

        private bool PlayerIsNeighbor()
        {
            foreach (var neighbor in parentTile.Neighbors)
            {
                if (neighbor == playerController.CurrentTile)
                    return true;
            }

            return false;
        }
    }
}