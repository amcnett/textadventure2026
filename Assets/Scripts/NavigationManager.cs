using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager instance;

    public Room startingRoom;
    public Room currentRoom;
    public Exit toKeyNorth;

    public delegate void Restart();
    public event Restart onRestart;

    private Dictionary<string, Room> exitRooms = new Dictionary<string, Room>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentRoom = startingRoom;
        Unpack();
    }
    void Unpack()
    {
        string description = currentRoom.description;

        exitRooms.Clear(); //important -- need as switch from room to room
        foreach (Exit e in currentRoom.exits)
        {
            if (!e.isHidden) {
                description += " " + e.description;
                exitRooms.Add(e.direction.ToString(), e.room);
            }
        }

        InputManager.instance.UpdateStory(description);

        if(currentRoom.name == "dragon")
        {
            /*onRestart.Invoke(); // calling my restart event to happen
            currentRoom = startingRoom; //puts player back to starting point
            Unpack();*/
            GameRestart();
        }
    }

    public void GameRestart()
    {
        onRestart.Invoke(); // calling my restart event to happen
        currentRoom = startingRoom; //puts player back to starting point
        toKeyNorth.isHidden = true;
        Unpack();

    }

    public bool SwitchRooms(string direction)
    {
        if (exitRooms.ContainsKey(direction))
        {
            if (GameManager.instance.inventory.Contains("key") || !getExit(direction).isLocked) {
                currentRoom = exitRooms[direction];
                InputManager.instance.UpdateStory("You go " + direction);
                Unpack();
                return true;
            }
            else
                return false;
        }
        return false;
    }

    Exit getExit(string direction)
    {
        foreach(Exit e in currentRoom.exits) 
            if (e.direction.ToString() == direction)
                return e;
        return null;
    }

    public bool getItem(string item)
    {
        bool isFound = false;
        foreach(string i in currentRoom.items)
        {
            if (i == item)
            {
                isFound = true;
                if(item == "orb")
                {
                    toKeyNorth.isHidden = false;
                }
            }

            
        }

        if (isFound)
        {// let's remove it from the room if we pick it up. 
            currentRoom.items.Remove(item);
            currentRoom.description = "This room used to have a blue glow! Seems pretty plain now";
        }

        return isFound; // item not found in room
    }
   
}
