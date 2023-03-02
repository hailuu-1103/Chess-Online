namespace MultiplayerNetwork.Controller
{
    using System.Collections.Generic;
    using Fusion;
    using MultiplayerNetwork.Configs;
    using MultiplayerNetwork.NetworkObject;

    public class NetworkState
    {
        public string           LobbyId { get; set; }
        public NetworkRunner    Runner  { get; set; }
        public NetworkInputData InputData;

        public GameSession GameSession { get; set; }

        // public ConnectionStatus                           ConnectionStatus { get; set; }
        // public Dictionary<PlayerRef, PlayerNetworkObject> Players          { get; } = new();
        public bool                                       IsMaster         => this.Runner != null && (this.Runner.IsServer || this.Runner.IsSharedModeMasterClient);

        public void Cleanup()
        {
            // this.Players.Clear();
            this.Runner      = null;
            this.GameSession = null;
        }
    }
}