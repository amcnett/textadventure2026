using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    void ResetGame()
    {
        inventory.Clear();
    }
}
