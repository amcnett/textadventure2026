using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem.Processors;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<string> inventory = new List<string>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        NavigationManager.instance.onRestart += ResetGame; //notice no ()
        //Save();
        Load();
    }

    void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save")) // we want to load info
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream aFile = File.Open(Application.persistentDataPath + "/player.save", FileMode.Open);
            SaveState gameState = (SaveState)bf.Deserialize(aFile);
            aFile.Close();

            Room room = NavigationManager.instance.GetRoomByName(gameState.currentRoom);
            if (room != null)
                NavigationManager.instance.SwitchRooms(room);

        }
        else // new player!
            NavigationManager.instance.GameRestart();
    }

    public void Save()
    {
        SaveState gameState = new SaveState();
        gameState.currentRoom = NavigationManager.instance.currentRoom.name;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream aFile = File.Create(Application.persistentDataPath + "/player.save");
        //Debug.Log(Application.persistentDataPath);
        bf.Serialize(aFile, gameState);
        aFile.Close();
    }

    void ResetGame()
    {
        inventory.Clear();
    }
}
