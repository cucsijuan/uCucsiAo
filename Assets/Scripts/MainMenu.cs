using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

struct pjCuenta
{
    public string  nombre;
    public short   head;
    public short   body;
    public byte    shield;
    public byte    helmet;
    public byte    weapon;
    public short   mapa;
    public byte    Class;
    public byte    race;
    public short   map;
    public byte    level;
    public int     gold;
    public bool    criminal;
    public bool    dead;
    public bool    gameMaster;

}

public class MainMenu : MonoBehaviour
{
    AOGameManager GM;
    PacketManager packetManager;
    TcpSocket gameSocket;


    [SerializeField] public Image CharSelectionPanel;
    [SerializeField] public TMP_Dropdown CharSelectionDropdown;
    [SerializeField] public TMP_InputField AccountText;
    [SerializeField] public TMP_InputField PasswordText;

    void Awake()
    {
        GM = AOGameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
        GM.MainMenuWindow = this;
        GM.InitProtocol();

        packetManager = PacketManager.Instance;        

        gameSocket = AOGameManager.gameSocket;

        Debug.Log("Current game state when Awakes: " + GM.gameState);
    }

    void Start()
    {
        Debug.Log("Current game state when Starts: " + GM.gameState);
        GM.SetGameState(GameState.MAIN_MENU);
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

    public void HandleOnStateChange()
    {
        Debug.Log("Handling state change to: " + GM.gameState);
        //Invoke("LoadLevel", 3f);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnConnectClicked()
    {
        gameSocket.Connect("Localhost", 7666);

        packetManager.WriteLoginExistingAccount(AccountText.text, PasswordText.text);
        Debug.Log("Starting Client");
    }

    public void OnLoginClicked()
    {
        packetManager.WriteLoginExistingChar(CharSelectionDropdown.options[CharSelectionDropdown.value].text, GM.currentAccount.accountHash);
        Debug.Log("Logging with " + CharSelectionDropdown.options[CharSelectionDropdown.value].text);
    }

}
