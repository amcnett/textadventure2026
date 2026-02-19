using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Text/Room")]
public class Room : ScriptableObject
{
    public string roomName;
    [TextArea]
    public string description;
    public Exit[] exits;

    // Another way we can add pickup items
    //public bool hasKey;
    //public bool hasOrb;

    // Our way to state a pickup item is in a room
    public List <string> items; // make sure this is a list!!! not an array (for easier processing!!!)

}
