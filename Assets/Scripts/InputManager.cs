using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public TMP_Text storyText; // the story 
    public TMP_InputField userInput; // the input field object
    public TMP_Text inputText; // part of the input field where user enters response
    public TMP_Text placeHolderText; // part of the input field for initial placeholder text
    public ScrollRect scrollRect; // controls how our story scrolls
    
    private string story; // holds the story to display
    private List<string> commands = new List<string>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        commands.Add("go");
        commands.Add("get");
        commands.Add("restart");
        commands.Add("save");

        story = storyText.text;
        userInput.onEndEdit.AddListener(GetInput);

        //NavigationManager.instance.onRestart += someFunction;
    }

    IEnumerator ScrollToBottom() { 
         yield return new WaitForEndOfFrame(); // wait until after the text is updated in the UI

        scrollRect.verticalNormalizedPosition = 0f; // move to bottom (scroll)
    }

    void GetInput(string input)
    {
        
        userInput.text = "";
        userInput.ActivateInputField();

        if (input != "")
        {
            char[] delims = { ' ' };
            string[] parts = input.ToLower().Split(delims); //parts[0] command, parts[1] is direction or thing picking up

            if (parts.Length >= 2)
            {
                if (commands.Contains(parts[0])) //command is valid
                {
                    UpdateStory(input);
                    if (parts[0] == "go")
                    {
                        if (NavigationManager.instance.SwitchRooms(parts[1]))
                            ;
                        else
                            UpdateStory("Direction does not exist or is locked. Please try again.");
                    }
                    else if (parts[0] == "get")
                    {
                        if (NavigationManager.instance.getItem(parts[1]))
                        {
                            GameManager.instance.inventory.Add(parts[1]);
                            UpdateStory("You added a(n) " + parts[1] + " to your inventory.");
                        }
                        else
                            UpdateStory("Sorry, " + parts[1] + " not found in this room.");
                    }
                }
            }// two parts end
            else if (parts.Length == 1)
            {
                if (parts[0] == "restart")
                    NavigationManager.instance.GameRestart();
                else if (parts[0] == "save")
                {
                    GameManager.instance.Save();
                    UpdateStory("Progress saved!");
                }
                else //command not valid
                {
                    UpdateStory("Invalid command. Please try again.");
                }
            }// one part end
        } // no empty end
    }

    public void UpdateStory(string msg)
    {
        story += "\n" + msg;
        storyText.text = story;
        StartCoroutine("ScrollToBottom");
    }
}
