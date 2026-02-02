using UnityEngine;

public class JumpBehavior : MonoBehaviour
{
    private Player _player;
    private GameBehavior _gameManager;
    void Start()
    {
        _player=GameObject.Find("Player").GetComponent<Player>();
        _gameManager=GameObject.Find("GameManger").GetComponent<GameBehavior>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player")
        {   
            _player.jumpForce +=5;
            Destroy(this.transform.parent.gameObject);
            Debug.Log("Jump boost collected");
            _gameManager.labelText = "Boost collected: +5 jump";
        }
    }
}
