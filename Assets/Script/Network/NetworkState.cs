using UnityEngine;
using System.Collections;
using Lidgren.Network;
public abstract class NetworkState : MonoBehaviour {

    [HideInInspector]
    public NetworkActor net_actor = null;

    public NetworkClientInfo owner { get { return net_actor.owner; } }


    public virtual void Init(){ }

    public abstract void Send(NetOutgoingMessage msg);

    public abstract void Receive(NetIncomingMessage msg);

    public virtual void NetworkUpdateClent() { }

    public virtual void NetworkUpdateServer() { }


    public virtual void NetworkFixUpdateClent() { }

    public virtual void NetworkFixUpdateServer() { }
}
