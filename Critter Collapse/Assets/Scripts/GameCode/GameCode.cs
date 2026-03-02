using UnityEngine;

public class GameCode : MonoBehaviour
{
    [SerializeField] private string gameCode;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void setGameCode(string newCode)
    {
        gameCode = newCode;
    }

    public string getGameCode()
    {
        return "Game Code: " + gameCode;
    }
}
