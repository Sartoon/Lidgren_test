using UnityEngine;
using System.Collections;
using Lidgren.Network;
using System.Collections.Generic;
using System;

public static class NetworkRemoteCall  {

    const NetDeliveryMethod deliveryMethod = NetDeliveryMethod.ReliableOrdered;

    public static void CallOnClient(NetworkClientInfo client,string medthod_name,params object[] args)
    {
        CallOnClients(new[] {client},medthod_name,args);
    }
    public static void CallOnClients(IList<NetworkClientInfo> clients,string method_name,params object[] args)
    {
        if(!NetworkPeer.is_server)
        {
            throw new Exception();
        }

        var connections = new List<NetConnection>(clients.Count);
        for(var i=0;i<clients.Count;i++)
        {
            connections.Add(clients[i].connection);
        }
        CallOnConnections(connections,method_name,args);

    }

    private static void CallOnConnections(IList<NetConnection> connections, string method_name, params object[] args)
    {
        var msg = BuildMessage(0,method_name,args);
        //调用RPC的方法是在这里
        NetworkPeer.instance.net_peer.SendMessage(msg,connections,deliveryMethod,1);
    }

    private static NetOutgoingMessage BuildMessage(int p, string method_name, object[] args)
    {
        var msg = NetworkPeer.instance.CreateMessage();
        msg.Write(NetworkPeer.REMOTE_CALL_FLAG);
        msg.Write(p);
        msg.Write(method_name);

        for (var i = 0; i < args.Length; ++i)
        {
            WriteArgument(msg, args[i]);
        }

        return msg;
    }
    static void WriteArgument(NetOutgoingMessage msg, object a)
    {
        if (a is byte)
        {
            msg.Write((byte)a);

        }
        else if (a is int)
        {
            msg.Write((int)a);

        }
        else if (a is float)
        {
            msg.Write((float)a);

        }
        else if (a is Vector3)
        {
            NetworkUtils.Write(msg, (Vector3)a);

        }
        else if (a is Quaternion)
        {
            NetworkUtils.Write(msg, (Quaternion)a);

        }
        else if (a is string)
        {
            msg.Write((string)a);

        }
        else
        {
            throw new Exception("Unsupported remote call argument type '" + a.GetType() + "'");
        }
    }
    
}
