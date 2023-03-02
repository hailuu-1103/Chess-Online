namespace MultiplayerNetwork.Interface
{
    using System.Threading.Tasks;
    using Fusion;
    using MultiplayerNetwork.Configs;

    public interface IMultiplayerNetworkManager
    {
        /// <summary>
        /// Create connection to Photon Network/ Instantiate Network Runner
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnect Photon Network/ Shutdown current Network Runner
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        void JoinSession(SessionInfo info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="props"></param>
        void CreateSession(SessionProps props);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lobbyId"></param>
        /// <returns></returns>
        Task EnterLobby(string lobbyId);

        // /// <summary>
        // /// 
        // /// </summary>
        // /// <param name="playerRef"></param>
        // /// <param name="playerNetworkObject"></param>
        // void SetPlayer(PlayerRef playerRef, PlayerNetworkObject playerNetworkObject);
        //
        // /// <summary>
        // /// 
        // /// </summary>
        // /// <param name="ply"></param>
        // /// <returns></returns>
        // PlayerNetworkObject GetPlayer(PlayerRef ply = default);
        //
        // /// <summary>
        // /// 
        // /// </summary>
        // /// <param name="status"></param>
        // /// <param name="reason"></param>
        // void SetConnectionStatus(ConnectionStatus status, string reason = "");
        //
        // /// <summary>
        // /// 
        // /// </summary>
        // /// <param name="runner"></param>
        // /// <param name="prefabName"></param>
        // /// <param name="inputAuthority"></param>
        // /// <param name="position"></param>
        // /// <param name="rotation"></param>
        // /// <typeparam name="T"></typeparam>
        // /// <returns></returns>
        // Task<T> SpawnNetworkObject<T>(NetworkRunner runner, string prefabName, PlayerRef? inputAuthority = null, Vector3? position = null, Quaternion? rotation = null)
        //     where T : SimulationBehaviour;
    }
}