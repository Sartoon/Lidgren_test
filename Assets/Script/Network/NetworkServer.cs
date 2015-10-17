using UnityEngine;
using System.Collections;
using Lidgren.Network;
using System.Collections.Generic;
public class NetworkServer :NetworkPeer {

    public const int SEND_RATE = 3;
    public const float SEND_TIME = 0.045F;

    public readonly NetServer netServer;
    public readonly HashSet<NetworkClientInfo> connected_clients = new HashSet<NetworkClientInfo>();

    byte host_id_counter;
    int actor_id_counter;

    NetOutgoingMessage out_msg;

    public NetworkServer():base(SEND_RATE)
    {
        instance = this;
        is_server = true;
        is_client = true;

        host_id = 0;
        host_id_counter = 0;
        actor_id_counter = 0;

        var config = CreateConfig();
        config.Port = NetworkPeer.APP_PORT;

        net_peer = netServer = new NetServer(config);
        netServer.Start();
    }

    public NetworkClientInfo GetClientInfo(NetIncomingMessage msg)
    {
        var connection = msg.SenderConnection;

        if(connection.Tag==null)
        {
            connection.Tag = new NetworkClientInfo(++host_id_counter,connection);

        }
        return ((NetworkClientInfo)connection.Tag);
    }

    protected override void OnStatuChanged(NetIncomingMessage msg)
    {
       switch(msg.SenderConnection.Status)
       {
           case NetConnectionStatus.Connected:
               var new_client = GetClientInfo(msg);
               connected_clients.Add(new_client);
               NetworkRemoteCall.CallOnClient(new_client,"Hello",new_client.host_id);
               break;
           case NetConnectionStatus.Disconnected:
           case NetConnectionStatus.Disconnecting:
               connected_clients.Remove(GetClientInfo(msg));
               break;
       }
    }

    ===================写到这里了
}
