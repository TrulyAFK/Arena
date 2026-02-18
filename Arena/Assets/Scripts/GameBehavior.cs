
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CustomExtensions;
using UnityEngine.InputSystem;

public class GameBehavior : MonoBehaviour, IManager
{
    public delegate void DebugDelegate(string newText);
    public DebugDelegate debug = Print;

    public Stack<string> LootStack = new Stack<string>();
    private string _state;
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }

    public string labelText = "Collect all 4 items and win your freedom!";
    public int maxItems = 4;
    public bool showWinScreen = false;
    public bool showLossScreen = false;
    private int _itemsCollected = 0;
    public int Items
    {
        get { return _itemsCollected; }
        set
        {
            _itemsCollected = value;
            if (_itemsCollected >= maxItems)
            {
                labelText = "You've found all the items!";
                showWinScreen = true;
                Time.timeScale = 0f;
            }
            else
            {
                labelText = string.Format("Item found, {0} remain.", maxItems - _itemsCollected);
            }
        }
    }
    private int _playerHP = 3;
    public int HP
    {
        get { return _playerHP; }
        set
        {
            _playerHP = value;
            if (_playerHP <= 0)
            {
                labelText = "You want another life with that?";
                showLossScreen = true;
                Time.timeScale = 0;
            }
            else
            {
                labelText = "Ouch... that's gota hurt.";
            }
        }
    }
    void Start()
    {
        Initialize();
        InventoryList<string> inventoryList = new InventoryList<string>();
        inventoryList.SetItem("Potion");
        Debug.Log(inventoryList.item);
    }
    public void Initialize()
    {
        _state = "Manager initialized..";
        _state.FancyDebug();
        Debug.Log(_state);
        LogWithDelegate(debug);
        LootStack.Push("Sword of Doom");
        LootStack.Push("HP+");
        LootStack.Push("Golden Key");
        LootStack.Push("Winged Boot");
        LootStack.Push("Mythril Bracers");
        GameObject player = GameObject.Find("Player");
        Player playerBehavior = player.GetComponent<Player>();
        playerBehavior.playerJump += HandlePlayerJump;
    }
    public void HandlePlayerJump()
    {
        Debug.Log("Player has jumped!");
    }
    public static void Print(string newText)
    {
        Debug.Log(newText);
    }
    public void LogWithDelegate(DebugDelegate del)
    {
        del("Delegating debug task...");
    }
    void OnGUI()
    {
        GUI.Box(new Rect(15, 15, Screen.width / 5, Screen.height / 18), "Player Health:" + _playerHP);
        GUI.Box(new Rect(15, Screen.height / 18 + 30, Screen.width / 5, Screen.height / 18), "Items Collected:" + _itemsCollected);
        GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height - 50, 300, 40), labelText);

        if (showWinScreen)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "YOU WON!"))
            {
                Utilities.RestartLevel();
            }
        }
        if (showLossScreen)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "You lose..."))
            {
                try
                {
                    Utilities.RestartLevel(-1);
                    debug("Level restarted successfully...");
                }
                catch (System.ArgumentException ex)
                {
                    Utilities.RestartLevel(0);
                    Debug.LogError("reverting to scene 0: " + ex.ToString());
                }
                finally
                {
                    debug("Restart handled...");
                }
                Utilities.RestartLevel(-1);
            }
        }
    }
    public void PrintLootReport()
    {
        Debug.LogFormat("There are {0} random loot items waiting for you!", LootStack.Count);
    }
}
