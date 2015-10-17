using UnityEngine;
using System.Collections;

public struct UserCommand 
{
    //Serialized to server
    public byte commandid;
    public byte keystate;
    public byte actionstate;
    public float client_time;
}
