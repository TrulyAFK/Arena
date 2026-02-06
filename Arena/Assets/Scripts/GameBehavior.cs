
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;


public class GameBehavior : MonoBehaviour
{
    public string labelText = "Collect all 4 items and win your freedom!";
    public int maxItems=4;
    public bool showWinScreen=false;
    public bool showLossScreen=false;
    private int _itemsCollected = 0 ;
    public int Items
    {
        get {return _itemsCollected;}
        set {
            _itemsCollected = value;
            if (_itemsCollected >= maxItems)
            {
                labelText =  "You've found all the items!";
                showWinScreen =true;
                Time.timeScale=0f;
            } else
            {
                labelText = string.Format("Item found, {0} remain.",maxItems-_itemsCollected);
            }
        }
    }
    private int _playerHP=10;
    public int HP
    {
        get{return _playerHP;}
        set
        {
            _playerHP = value;
            if (_playerHP <= 0)
            {
                labelText="You want another life with that?";
                showLossScreen=true;
                Time.timeScale=0;
            }
            else
            {
                labelText="Ouch... that's gota hurt.";
            }
            Debug.LogFormat("Items: {0}",_playerHP);   
        }
    }
    void RestartLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale=1.0f; 
    }
    void OnGUI()
    {
        GUI.Box(new Rect(15,15,Screen.width/5,Screen.height/18),"Player Health:"+_playerHP);
        GUI.Box(new Rect(15,Screen.height/18+30,Screen.width/5,Screen.height/18),"Items Collected:"+_itemsCollected);
        GUI.Box(new Rect(Screen.width/2-150,Screen.height-50,300,40),labelText);
        
        
        if (showWinScreen)
        {
            if (GUI.Button(new Rect (Screen.width/2-100,Screen.height/2-50,200,100),"YOU WON!"))
            {
                RestartLevel();
            }
        }
        if (showLossScreen)
        {
            if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2-50,200,100),"You lose..."))
            {
                RestartLevel();
            }
        }
    }
}
