using UnityEngine;
using TMPro;


public class GameCodeText : MonoBehaviour
{
    [SerializeField] TMP_Text thisGameObject;
    private GameCode manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        manager = GameObject.Find("GameCodeManager").GetComponent<GameCode>();
        thisGameObject.text = manager.getGameCode();
    }
}
