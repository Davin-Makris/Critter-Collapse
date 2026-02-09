using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

public class RelayManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeText;
    [SerializeField] private TMP_InputField joinCodeInputField;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        //Await: Runs concurrently and does not pause on this line of code. Since we're asking for information from the server, not having await would make us wait
        //for a response instead of moving on to the next line. 
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void StartRelay()
    {
        string joinCode = await StartHostWithRelay();
        joinCodeText.text = "Lobby Code: " + joinCode;
        NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public async void JoinRelay()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            try
            {
                Debug.Log("Trying Code: " + joinCodeInputField.text);
                await StartClientWithRelay(joinCodeInputField.text);
                NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            }
            catch
            {
                Debug.Log("Wrong!");
            }
        }
        
    }

    private async Task<string> StartHostWithRelay(int maxConnections = 4)
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections); //Create a server connection pipeline!

        RelayServerData rsd = allocation.ToRelayServerData("dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(rsd); //Send our networkManager the server data

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId); //Generate a code for clients to load onto

        Debug.Log("JoinCode: " + joinCode);
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }
    
    private async Task<bool> StartClientWithRelay(string joinCode)
    {
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        RelayServerData rsd = joinAllocation.ToRelayServerData("dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(rsd);

        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }
}
