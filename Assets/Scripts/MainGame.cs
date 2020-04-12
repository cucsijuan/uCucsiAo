using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{

    public AOTilemap tilemap;

    private AOGameManager GM;
    private PacketManager _packetManager;
    private TcpSocket _gameSocket;

    void Awake()
    {
        GM = AOGameManager.Instance;
        _packetManager = PacketManager.Instance;
        _gameSocket = AOGameManager.gameSocket;

        GM.MainGamewindow = this;
        GM.playerObject = Instantiate(GM.playerObject, new Vector3(0, 0, 0), Quaternion.identity);
        GM.playerObject.layer = 2;
        Debug.Log("Current game state when Awakes: " + GM.gameState);

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Current game state when main game Starts: " + GM.gameState);

        tilemap.LoadMap(GM.currentAccount.userMap);

        GM.playerObject.transform.position = GM.charList[GM.currentAccount.userCharIndex].pos.MapPositionToVector3();

        foreach (var item in GM.mapData)
        {
            if (item.Value.blocked == 1)
            {
                Instantiate(GM.blockObject, new Vector3(item.Key.x, 99 - item.Key.y, 0), Quaternion.identity);
            }
        }
    }

    void Update()
    {

        if (_gameSocket.IsConnected())
        {
            HandleTCPData();
        }

    }

    private void HandleTCPData()
    {
        if (_gameSocket.IsDataAvailableToRead())
        {
            _gameSocket.Receive();
        }

        if (GM.incomingData.queueLength > 0)
        {
            _packetManager.HandleReceivedData();
        }

        if (!GM.outgoingData.locked && GM.outgoingData.queueLength > 0)
        {
            GM.FlushBuffer();
        }
    }

    public void OnDisconnectClicked()
    {
        _gameSocket.Disconnect();

        Debug.Log("Client Disconnected");

        SceneManager.LoadScene("MainMenu");
    }
}
