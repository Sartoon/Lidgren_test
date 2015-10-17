using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Lidgren.Network;

public class NetworkActor : MonoBehaviour {

    [HideInInspector]
    public byte host_id = 0;

    [HideInInspector]
    public int actor_id = 0;

    [HideInInspector]
    public NetworkClientInfo owner = null;

    [HideInInspector]
    public bool is_owner = false;

    [HideInInspector]
    public string prefab_name = null;

    public const int MaxId = UInt16.MaxValue;
    List<NetworkState> net_states = new List<NetworkState>();

    public  void Send(NetOutgoingMessage msg)
    {
        msg.Write(actor_id);
        for (var i = 0; i < net_states.Count;++i)
        {
            net_states[i].Send(msg);
        }
    }

    public void Receive(NetIncomingMessage msg)
    {
        for(var i=0;i<net_states.Count;i++)
        {
            net_states[i].Receive(msg);
        }
    }

    public void NetworkFixedUpdate()
    {
        for(var i=0;i<net_states.Count;++i)
        {
            if(NetworkPeer.is_server)
            {
                net_states[i].NetworkFixUpdateServer();
            }
            else
            {
                net_states[i].NetworkFixUpdateClent();
            }
        }
    }



    public override int GetHashCode()
    {
        return actor_id;
    }

    public virtual void Init()
    {

    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
