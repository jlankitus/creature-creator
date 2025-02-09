using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Race : IndividualMinigame
    {
        #region Fields
        [SerializeField] private RaceCheckpoint startCheckpoint;
        [SerializeField] private PathCreator path;
        [SerializeField] private int laps;
        [SerializeField] private float rankingCooldown;

        private Dictionary<ulong, float> playerDistances = new Dictionary<ulong, float>();
        private Dictionary<ulong, int> playerLaps = new Dictionary<ulong, int>();

        private bool hasAnyoneFinished;
        #endregion

        #region Properties
        public RaceCheckpoint StartCheckpoint => startCheckpoint;

        public RaceCheckpoint CurrentCheckpoint { get; set; }
        #endregion

        #region Methods
        protected override void Setup()
        {
            base.Setup();

            starting.onEnter += OnStartingEnter;

            playing.onEnter += OnPlayingEnter;
        }

        #region Starting
        private void OnStartingEnter()
        {
            foreach (ulong clientId in players)
            {
                playerDistances.Add(clientId, 0f);
                playerLaps.Add(clientId, 0);
            }
        }

        public override Transform GetSpawnPoint()
        {
            if (CurrentCheckpoint != null)
            {
                return CurrentCheckpoint.transform;
            }
            else
            {
                return base.GetSpawnPoint();
            }
        }
        #endregion

        #region Playing
        private void OnPlayingEnter()
        {
            foreach (ulong clientId in players)
            {
                int lap = (playerLaps[clientId] = 1);
                SetLapClientRpc(lap, NetworkUtils.SendTo(clientId));
            }
            StartCheckpointClientRpc();

            StartCoroutine(RankRoutine());
        }

        protected override IEnumerator GameplayLogicRoutine()
        {
            yield return new WaitUntil(() => hasAnyoneFinished);
        }

        private IEnumerator RankRoutine()
        {
            while (State.Value == MinigameStateType.Playing)
            {
                List<PlayerDistance> distancesToSort = new List<PlayerDistance>();

                // Calculate distances
                for (int i = 0; i < players.Count; i++)
                {
                    ulong clientId = players[i];

                    Vector3 position = NetworkManager.SpawnManager.GetPlayerNetworkObject(clientId).transform.position;
                    float distanceAlongPath = path.path.GetClosestDistanceAlongPath(position);

                    float prevDistance = playerDistances[clientId];
                    float nextDistance = distanceAlongPath + ((playerLaps[clientId] - 1) * path.path.length);

                    if (Mathf.Abs(nextDistance - prevDistance) < path.path.length / 2f)
                    {
                        playerDistances[clientId] = nextDistance;
                    }

                    distancesToSort.Add(new PlayerDistance()
                    {
                        playerIdx = i,
                        distance = playerDistances[clientId]
                    });
                }

                // Sort distances
                distancesToSort.Sort();

                // Update scoreboard
                for (int i = 0; i < distancesToSort.Count; i++)
                {
                    PlayerDistance distance = distancesToSort[i];
                    Scoreboard[distance.playerIdx] = new Score(Scoreboard[distance.playerIdx], i + 1);
                }

                // Cooldown
                yield return new WaitForSeconds(rankingCooldown);
            }
        }

        [ClientRpc]
        private void StartCheckpointClientRpc()
        {
            if (InMinigame)
            {
                CurrentCheckpoint = startCheckpoint;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void LapServerRpc(ulong clientId)
        {
            int lap = ++playerLaps[clientId];
            if (lap > laps)
            {
                hasAnyoneFinished = true;
            }

            SetLapClientRpc(lap, NetworkUtils.SendTo(clientId));
        }

        [ClientRpc]
        private void SetLapClientRpc(int lap, ClientRpcParams sentTo = default)
        {
            if (InMinigame)
            {
                if (lap <= laps)
                {
                    MinigameManager.Instance.SetTitle($"{lap}/{laps}");
                }
                else
                {
                    MinigameManager.Instance.SetTitle(null);
                }
            }
        }
        #endregion

        #region Completing
        protected override void OnServerShutdown()
        {
            playerDistances.Clear();
            playerLaps.Clear();
            hasAnyoneFinished = false;

            base.OnServerShutdown();
        }
        protected override void OnClientShutdown()
        {
            if (InMinigame)
            {
                CurrentCheckpoint = null;
            }

            base.OnClientShutdown();
        }
        #endregion
        #endregion

        #region Nested
        public class PlayerDistance : IComparable<PlayerDistance>
        {
            public int playerIdx;
            public float distance;

            public int CompareTo(PlayerDistance other)
            {
                return other.distance.CompareTo(distance);
            }
        }
        #endregion
    }
}