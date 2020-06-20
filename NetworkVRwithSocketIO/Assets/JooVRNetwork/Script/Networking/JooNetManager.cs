using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Linq;
using JYJ_Utils;

public class JooNetManager : MonoBehaviour
{
    public SocketIOComponent socket = null;
    protected static JooNetManager instance = null;
    public static JooNetManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<JooNetManager>();
            }
            return instance;
        }
    }
    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Start()
    {
        yield return null;
        RegisterSocket(socket);
        socket.Connect();
        yield return new WaitForSeconds(0.5f);
        var jsonStr = JsonConvert.SerializeObject(JooConfiguration.GetConfig<NetworkPlayer>());
        socket.Emit("playerEnter", JSONObject.Create(jsonStr));
    }
    protected void RegisterSocket(SocketIOComponent pSocket)
    {
        pSocket.On("playerEnter", SE_PlayerEnter);
        pSocket.On("playerExit", SE_PlayerExit);
        pSocket.On("deadReckoning", SE_DeadReckoning);
    }

    public Action<NetworkPlayer[]> actEnter = (data) => { };
    protected void SE_PlayerEnter(SocketIOEvent msg)
    {
        var tType = new { players = new NetworkPlayer[1] };
        var networkPlayers = JsonConvert.DeserializeAnonymousType(msg.data.ToString(), tType);
        actEnter(networkPlayers.players);
    }
    public Action<NetworkPlayer> actExit = (data) => { };
    protected void SE_PlayerExit(SocketIOEvent msg)
    {
        var networkPlayer = JsonConvert.DeserializeObject<NetworkPlayer>(msg.data.ToString());
        actExit(networkPlayer);
    }
    public Action<TransformSyncDataContainer> actDeadReckoning = (data) => { };
    protected void SE_DeadReckoning(SocketIOEvent msg)
    {
        var trs = JsonConvert.DeserializeObject<TransformSyncDataContainer>(msg.data.ToString());
        actDeadReckoning(trs);
    }
}

[Serializable]
public class NetworkPlayer
{
    public string id = "";
    public int playerIndex = 0;
}