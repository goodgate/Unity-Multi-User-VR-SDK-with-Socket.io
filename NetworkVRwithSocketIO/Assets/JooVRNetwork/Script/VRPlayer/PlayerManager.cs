using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JYJ_Utils;
public class PlayerManager : MonoBehaviour
{
    public VRRemotePlayer pfRemotePlayer = null;
    public VRLocalPlayer pfLocalPlayer = null;
    [HideInInspector]
    public VRLocalPlayer localPlayer = null;
    public Dictionary<int, VRRemotePlayer> dicPlayer = new Dictionary<int, VRRemotePlayer>();
    public Transform[] spawnPoints = null;
    public static PlayerManager Instance = null;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        localPlayer = Instantiate<VRLocalPlayer>(pfLocalPlayer, Vector3.zero, Quaternion.identity);
        var pIdx = JooConfiguration.GetConfig<NetworkPlayer>().playerIndex;
        localPlayer.InitializePlayer(pIdx, spawnPoints[pIdx].position);
    }
    private void OnEnable()
    {
        JooNetManager.Instance.actEnter += OnPlayerEnter;
        JooNetManager.Instance.actExit += OnPlayerExit;
        JooNetManager.Instance.actDeadReckoning += OnDeadReckoning;
    }
    private void OnDisable()
    {
        JooNetManager.Instance.actEnter -= OnPlayerEnter;
        JooNetManager.Instance.actExit -= OnPlayerExit;
        JooNetManager.Instance.actDeadReckoning -= OnDeadReckoning;
    }

    private void OnDeadReckoning(TransformSyncDataContainer trs)
    {
        VRRemotePlayer player = null;
        bool isContained = dicPlayer.TryGetValue(trs.ownerIndex, out player);
        if (isContained)
        {
            player.SetTransforms(trs.data);
        }
    }
    private void OnPlayerEnter(NetworkPlayer[] players)
    {
        foreach(var p in players)
        {
            bool hasConnected = dicPlayer.ContainsKey(p.playerIndex);
            if (!hasConnected && p.playerIndex != JooConfiguration.GetConfig<NetworkPlayer>().playerIndex)
            {
                VRRemotePlayer tLocalPlayer = Instantiate<VRRemotePlayer>(pfRemotePlayer, Vector3.zero, Quaternion.identity);
                tLocalPlayer.InitializePlayer(p.playerIndex, spawnPoints[p.playerIndex].position);
                dicPlayer.Add(p.playerIndex, tLocalPlayer);
            }
        }
       
    }
    private void OnPlayerExit(NetworkPlayer player)
    {
        VRRemotePlayer tPlayer = null;
        bool hasConnected = dicPlayer.TryGetValue(player.playerIndex, out tPlayer);
        if (hasConnected)
        {
            Destroy(tPlayer.gameObject);
            dicPlayer.Remove(player.playerIndex);
        }
    }

  
}
