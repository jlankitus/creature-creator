using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class IndividualMinigame : Minigame
    {
        #region Fields
        [Header("Individual Minigame")]
        [SerializeField] private Transform[] spawnPoints;

        private int myIndex;
        #endregion

        #region Methods
        protected override void Setup()
        {
            base.Setup();

            waitingForPlayers.onExit += OnWaitingForPlayersExit;
        }

        #region Waiting For Players
        private void OnWaitingForPlayersExit()
        {
            Assign();
        }

        private void Assign()
        {
            // Randomize ordering of players
            players.Shuffle();

            // Assign indexes
            for (int i = 0; i < players.Count; i++)
            {
                AssignClientRpc(i, NetworkUtils.SendTo(players[i]));
            }
        }
        [ClientRpc]
        private void AssignClientRpc(int index, ClientRpcParams sendTo)
        {
            myIndex = index;
        }
        #endregion

        #region Starting
        public override void OnSetupScoreboard()
        {
            foreach (ulong clientId in players)
            {
                Scoreboard.Add(new Score()
                {
                    id = clientId.ToString(),
                    score = 0,
                    displayName = NetworkHostManager.Instance.Players[clientId].username
                });
            }
        }

        public override Transform GetSpawnPoint()
        {
            return spawnPoints[myIndex];
        }
        #endregion

        #region Completing
        protected override List<ulong> GetWinnerClientIds()
        {
            List<ulong> winnerClientIds = new List<ulong>();
            int maxScore = int.MinValue;

            foreach (Score s in Scoreboard)
            {
                ulong clientId = ulong.Parse(s.id.ToString());
                int score = s.score;

                if (score > maxScore)
                {
                    winnerClientIds.Clear();
                    winnerClientIds.Add(clientId);

                    maxScore = score;
                }
                else
                if (score == maxScore)
                {
                    winnerClientIds.Add(clientId);
                }
            }

            return winnerClientIds;
        }

        protected override string GetWinnerName()
        {
            return GetWinnerNames().JoinAnd();
        }
        private List<string> GetWinnerNames()
        {
            List<string> winnerNames = new List<string>();

            foreach (ulong clientId in GetWinnerClientIds())
            {
                winnerNames.Add(NetworkHostManager.Instance.Players[clientId].username);
            }

            return winnerNames;
        }
        #endregion
        #endregion
    }
}