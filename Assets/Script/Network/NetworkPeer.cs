using UnityEngine;
using System.Collections;
using Lidgren.Network;

public abstract class NetworkPeer  {
    //Instance variables
    public readonly int send_rate;
    public int ticks;
    public NetPeer net_peer;
    public byte host_id;
    // Static variables
    public static bool is_server;
    public static bool is_client;
    public static NetworkPeer instance;
    //Configuration options
    public const int APP_PORT = 14000;
    public const string APP_IDENTIFIER = "mobax";
    public const float SIMULATED_LOSS = 0.0f;
    public const float SIMULATED_MIN_LATENCY = 0.0f;
    public const float SIMULATDE_RANDOM_LATENCY = 0.0f;
    // Message flags
    public const byte REMOTE_CALL_FLAG = 0;
    public const byte USER_COMMAND_FLAG = 1;
    public const byte ACTOR_STATE_FLAG = 2;
    public const byte ACTOR_EVENT_FLAG = 3;
    public NetworkPeer(int send_rate):base()
    {
        this.send_rate = send_rate;
    }

    public void MessagePump()
    {
        NetIncomingMessage msg;
        ticks += 1;
        BeforePump();
        while((msg=net_peer.ReadMessage())!=null)
        {
            switch(msg.MessageType)
            {
                case NetIncomingMessageType.Data:
                    OnDataMessage(msg);
                    break;
                case NetIncomingMessageType.VerboseDebugMessage:
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.ErrorMessage:
                    OnDebugMessage(msg);
                    break;
                case NetIncomingMessageType.StatusChanged:
                    OnStatuChanged(msg);
                    break;
                default:
                    OnUnknownMessage(msg);
                    break;
            }
            net_peer.Recycle(msg);
        }
        //此处用意？
        Simulate();
        //此处用意？
        if(ticks==send_rate)
        {
            OnSend();
            ticks = 0;
        }
    }

    protected  virtual void OnSend()
    {
        throw new System.NotImplementedException();
    }

    protected NetPeerConfiguration CreateConfig()
    {
        var config = new NetPeerConfiguration(APP_IDENTIFIER);
        config.SimulatedMinimumLatency = SIMULATED_MIN_LATENCY;
        config.SimulatedRandomLatency = SIMULATDE_RANDOM_LATENCY;
        config.SimulatedLoss = SIMULATED_LOSS;
        return config;

    }
    public virtual NetOutgoingMessage CreateMessage()
    {
        return net_peer.CreateMessage();
    }
    protected virtual void Simulate()
    {
        NetworkActor obj;
        for(var i=0;i<=NetworkActorRegistry.MaxIndex;i++)
        {
            if((obj=NetworkActorRegistry.objects[i])!=null)
            {
                obj.NetworkFixedUpdate();
            }
        }
        AfterSimulate();
    }

    protected virtual void AfterSimulate()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void OnUnknownMessage(NetIncomingMessage msg)
    {
        throw new System.NotImplementedException();
    }

    protected virtual void OnStatuChanged(NetIncomingMessage msg)
    {
        throw new System.NotImplementedException();
    }

    protected virtual void OnDebugMessage(NetIncomingMessage msg)
    {
        throw new System.NotImplementedException();
    }

    protected virtual void OnDataMessage(NetIncomingMessage msg)
    {
        throw new System.NotImplementedException();
    }

    protected  virtual void BeforePump()
    {
        throw new System.NotImplementedException();
    }
}
