namespace MultiplayerNetwork.NetworkObject
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Fusion;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using MultiplayerNetwork.Configs;
    using MultiplayerNetwork.Controller;
    using MultiplayerNetwork.Interface;
    using MultiplayerNetwork.Signals;
    using Unity.VisualScripting;
    using UnityEngine;
    using Zenject;

    public class GameSession : NetworkBehaviour
    {
        [Networked] public TickTimer               PostLoadCountDown  { get; set; }
        public             SessionProps            Props              => new SessionProps(this.Runner.SessionInfo.Properties);
        public             SessionInfo             Info               => this.Runner.SessionInfo;
        // public             MapNetworkObject        MapNetworkObject   { get; set; }
        // public             List<TeamNetworkObject> TeamNetworkObjects { get; private set; } = new List<TeamNetworkObject>();

        private HashSet<PlayerRef> finishedLoading = new HashSet<PlayerRef>();

        #region Injection

        private NetworkState               networkState;
        private IScreenManager             screenManager;
        private SignalBus                  signalBus;
        private ChessOnlineSceneDirector        sceneDirector;
        private IMultiplayerNetworkManager networkManager;

        [Inject]
        public virtual void Inject(NetworkState networkStateParam, IScreenManager screenManagerParam, SignalBus signalBusParam, ChessOnlineSceneDirector director,
            IMultiplayerNetworkManager networkManagerParam)
        {
            this.networkState              = networkStateParam;
            this.screenManager             = screenManagerParam;
            this.signalBus                 = signalBusParam;
            this.sceneDirector             = director;
            this.networkManager            = networkManagerParam;
        }

        #endregion

        public override void Spawned()
        {
            this.networkState.GameSession = this;
            this.signalBus.Subscribe<PlayerJoinedSessionSignal>(this.OnPlayerJoinedSession);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            this.signalBus.TryUnsubscribe<PlayerJoinedSessionSignal>(this.OnPlayerJoinedSession);
        }

        private async void OnPlayerJoinedSession(PlayerJoinedSessionSignal obj)
        {
            Debug.Log("GameSession - OnPlayerJoinedSession");


            // if (this.networkState.IsMaster && this.TeamNetworkObjects.Count < modeRecord.MaxTeam)
            // {
            //     for (var i = 0; i < modeRecord.MaxTeam; i++)
            //     {
            //         // create new team
            //         var newTeam = await this.networkManager.SpawnNetworkObject<TeamNetworkObject>(this.networkState.Runner, nameof(TeamNetworkObject));
            //         newTeam.TeamId = i;
            //     }
            // }
            //
            // await UniTask.WaitUntil(() => this.TeamNetworkObjects.Count == modeRecord.MaxTeam);
            //
            // TeamNetworkObject chosenTeam       = null;
            // var               maxPlayerPerTeam = modeRecord.MaxPlayer / modeRecord.MaxTeam;
            // // get team not reach max player yet
            // foreach (var teamNetworkObject in this.TeamNetworkObjects.Where(teamNetworkObject => teamNetworkObject.Players.Count < maxPlayerPerTeam))
            // {
            //     if (chosenTeam == null)
            //     {
            //         chosenTeam = teamNetworkObject;
            //     }
            //     else if (chosenTeam.Players.Count > teamNetworkObject.Players.Count)
            //     {
            //         chosenTeam = teamNetworkObject;
            //     }
            // }
            //
            // if (chosenTeam != null) chosenTeam.Players.Add(this.networkManager.GetPlayer(obj.PlayerRef));
        }

        [Rpc(RpcSources.All, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_FinishedLoadingBattleScene(PlayerRef playerRef)
        {
            this.finishedLoading.Add(playerRef);
            Debug.Log($"RPC_FinishedLoadingBattleScene {playerRef}");
            // if (this.finishedLoading.Count >= this.networkState.Players.Count)
            {
                // this.CallMasterTransitBattleState<BattleLoadingState>();
            }
        }

        public void FinishedLoadingBattleScene(PlayerRef playerRef)
        {
            this.finishedLoading.Add(playerRef);
            Debug.Log($"FinishedLoadingBattleScene {playerRef}");
            // if (this.finishedLoading.Count >= this.networkState.Players.Count)
            // {
                // this.CallMasterTransitBattleState<BattleLoadingState>();
            // }
        }

        //
        // [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        // public void RPC_LoadSelectCharacterScreen() { this.screenManager.OpenScreen<SelectCharacterScreenPresenter>(); }
        //
        // [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        // public void RPC_ChangeStage(int stageId) { this.Props.StageId = stageId; }
        //
        // [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        // private void RPC_BattleStateTransitionTo(NetworkString<_512> state) { this.signalBus.Fire(new ChangeBattleStateSignal() { StateType = Type.GetType(state.Value) }); }
        //
        // [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        // public void RPC_LoadBattleScene() { this.sceneDirector.LoadBattleScene(); }
        //
        //
        // public void LoadMap(int stageId)
        // {
        //     this.finishedLoading.Clear();
        //     foreach (var player in this.networkState.Players.Values)
        //         player.InputEnabled = false;
        //     this.Runner.SetActiveScene(ChessOnlineSceneDirector.SceneName.Battle);
        //     // this.Runner.SetActiveScene((int)stageId);
        // }

        public void CallMasterTransitBattleState<T>() where T : IState
        {
            // if (this.Object.HasStateAuthority)
            //     this.RPC_BattleStateTransitionTo(typeof(T).FullName);
        }
    }
}