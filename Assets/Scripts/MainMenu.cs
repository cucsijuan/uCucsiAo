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

    // Panel PJ's
    [SerializeField] public Image CharSelectionPanel;
    [SerializeField] public TMP_Dropdown CharSelectionDropdown;

    // Panel - Login
    [SerializeField] public Image LoginPanel;
    [SerializeField] public TMP_InputField AccountText;
    [SerializeField] public TMP_InputField PasswordText;

    // Panel - Create Account
    [SerializeField] public Image AccountCreationPanel;
    [SerializeField] public TMP_InputField EmailText;
    [SerializeField] public TMP_InputField CreateAccount_PasswordText;
    [SerializeField] public TMP_InputField CreateAccount_ConfirmPasswordText;

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
        // Connect to the server.
        gameSocket.Connect("localhost", 7666);
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

    private void ShowScreen(Image screenToShow)
    {
        // Busco todos los elementos con el tag "screens"
        GameObject[] screens = GameObject.FindGameObjectsWithTag("screens");

        // Los oculto.
        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }

        // Abro el que quiero.
        screenToShow.gameObject.SetActive(true);
    }

    private void HandleTCPData()
    {
        try
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

        } catch(System.Exception ex)
        {
            Debug.Log("Error: " + ex.StackTrace);
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

    public void LoginPanel_OnConnectClicked()
    {
        packetManager.WriteLoginExistingAccount(AccountText.text, PasswordText.text);
        Debug.Log("Sending account login");
    }

    public void LoginPanel_OnLoginClicked()
    {
        packetManager.WriteLoginExistingChar(CharSelectionDropdown.options[CharSelectionDropdown.value].text, GM.currentAccount.accountHash);
        Debug.Log("Logging with " + CharSelectionDropdown.options[CharSelectionDropdown.value].text);
    }
    

    public void LoginPanel_OnCreateAccountClicked()
    {
        ShowScreen(AccountCreationPanel);
    }

    public void AccountCreationPanel_OnCreateAccountClicked()
    {
        if(!CreateAccount_PasswordText.text.Equals(CreateAccount_ConfirmPasswordText.text)) return;
        
        packetManager.WriteLoginNewAccount(EmailText.text, CreateAccount_PasswordText.text);

        Debug.Log("Creating account with  " + EmailText.text + " and pass " + CreateAccount_ConfirmPasswordText.text);

    }

    public void AccountCreationPanel_OnBackClicked()
    {
        ShowScreen(LoginPanel);
    }

}
