using UnityEngine;
using System.Collections;
using System;
using Lidgren.Network;

public class NetworkActorRegistry : MonoBehaviour {

    public static NetworkActor[] objects = new NetworkActor[256];
    public static int[] DeliveryMethods = new int[256];
    public static int MaxIndex = -1;
    public static int Count = 0;

    public static void RegisterActor(NetworkActor obj)
    {
        var index = obj.actor_id;
        if(index>=objects.Length)
        {
            var size = Math.Min(objects.Length*2,NetworkActor.MaxId);
            var newobjects = new NetworkActor[size];
            Array.Copy(objects,newobjects,objects.Length);
            objects = newobjects;
        }
        MaxIndex = Math.Max(MaxIndex,index);
        objects[index] = obj;
        ++Count;
    }

    public static NetworkActor GetById(int id)
    {
        if(id<objects.Length)
        {
            return objects[id];
        }
        return null;
    }

    public static void UnRegisiterActor(NetworkActor obj)
    {
        objects[obj.actor_id] = null;
        --Count;
    }

    public static void RegisterDeliveryMethod(NetDeliveryMethod deliverymethod)
    {
        DeliveryMethods[(int)deliverymethod]++;
    }
    public static void UnRegisterDeliveryMethod(NetDeliveryMethod deliverymethod)
    {
        DeliveryMethods[(int)deliverymethod]--;
    }
    public static bool HasDeliveryMethod(NetDeliveryMethod deliverymethod)
    {
        return DeliveryMethods[(int)deliverymethod] > 0;
    }
}
