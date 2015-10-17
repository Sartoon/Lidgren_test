using UnityEngine;
using System.Collections;
using Lidgren.Network;
using System.Collections.Generic;
public class NetworkClientInfo {

    public const int CMD_BUFFER_SIZE = 20;
    public bool has_spawned;
    public readonly byte host_id = 0;
    public readonly NetConnection connection = null;
    public readonly HashSet<NetworkActor> proximity_set = new HashSet<NetworkActor>();
    public readonly Queue<UserCommand> cmd_queue = new Queue<UserCommand>();

    public NetworkClientInfo(byte id,NetConnection conn)
    {
        host_id = id;
        connection = conn;
        connection.Tag = this;
    }

    public bool has_cmd 
    {
        get
        {
            return cmd_queue.Count > 0;
        }
    }

    public UserCommand cmd
    {
        get
        {
            return cmd_queue.Peek();
        }
    }

    public float rtt
    {
        get
        {
            //平均往返时间
            return connection.AverageRoundtripTime;
        }
    }

    public override int GetHashCode()
    {
        return (int)host_id;
    }
}
