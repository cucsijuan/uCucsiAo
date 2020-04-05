using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    AOGameManager GM;
    PacketManager packetManager;
    TcpSocket gameSocket;

    void Awake()
    {
        GM = AOGameManager.Instance;

        packetManager = PacketManager.Instance;

        gameSocket = AOGameManager.gameSocket;

        GM.playerObject = Instantiate(GM.playerObject, new Vector3(0, 0, 0), Quaternion.identity);

        Debug.Log("Current game state when Awakes: " + GM.gameState);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Current game state when main game Starts: " + GM.gameState);
        GM.playerObject.transform.position = new Vector3(GM.charList[GM.currentAccount.userCharIndex].posX, GM.charList[GM.currentAccount.userCharIndex].posY, GM.playerObject.transform.position.z);

        foreach (var item in GM.mapData)
        {
            if (item.Value.blocked)
            {
                Instantiate(GM.blockObject, new Vector3(item.Key.x, item.Key.y, 0), Quaternion.identity);
            }
        }
    }

    void Update()
    {

        if (gameSocket.IsConnected())
        {
            HandleTCPData();
        }

    }

    private void HandleTCPData()
    {
        if (gameSocket.IsDataAvailableToRead())
        {
            gameSocket.Receive();
        }

        if (GM.incomingData.queueLength > 0)
        {
            packetManager.HandleReceivedData();
        }

        if (!GM.outgoingData.locked && GM.outgoingData.queueLength > 0)
        {
            GM.FlushBuffer();
        }
    }

    public void OnDisconnectClicked()
    {
        gameSocket.Disconnect();

        Debug.Log("Client Disconnected");

        SceneManager.LoadScene("MainMenu");
    }
}
