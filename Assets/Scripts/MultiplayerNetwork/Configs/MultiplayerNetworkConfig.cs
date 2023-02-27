namespace MultiplayerNetwork.Configs
{
    using UnityEngine;

    [CreateAssetMenu(fileName = nameof(MultiplayerNetworkConfig), menuName = "MultiplayerNetwork")]
    public class MultiplayerNetworkConfig : ScriptableObject
    {
        [SerializeField] private bool         autoConnect;
        [SerializeField] private bool         isShareMode;
        [SerializeField] private SessionProps autoSessionProps;

        public bool         AutoConnect      => this.autoConnect;
        public bool         IsShareMode      => this.isShareMode;
        public SessionProps AutoSessionProps => this.autoSessionProps;
    }
}