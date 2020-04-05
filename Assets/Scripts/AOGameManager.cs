using System.Collections.Generic;
using UnityEngine;

// Game States
// for now we are only using these two
public enum GameState { INTRO, MAIN_MENU, INGAME }

public enum EHeading : byte
{
    NONE,
    NORTH,
    EAST,
    SOUTH,
    WEST
}

public enum eMessages
{
    NPCSwing,
    NPCKillUser,
    BlockedWithShieldUser,
    BlockedWithShieldOther,
    UserSwing,
    SafeModeOn,
    SafeModeOff,
    ResuscitationSafeOff,
    ResuscitationSafeOn,
    NobilityLost,
    CantUseWhileMeditating,
    NPCHitUser,
    UserHitNPC,
    UserAttackedSwing,
    UserHittedByUser,
    UserHittedUser,
    WorkRequestTarget,
    HaveKilledUser,
    UserKill,
    EarnExp,
    GoHome,
    CancelGoHome,
    FinishHome,
    UserMuerto,
    NpcInmune,
    Hechizo_HechiceroMSG_NOMBRE,
    Hechizo_HechiceroMSG_ALGUIEN,
    Hechizo_HechiceroMSG_CRIATURA,
    Hechizo_PropioMSG,
    Hechizo_TargetMSG
}

public struct AccountInfo
{
    public string accountName;
    public string accountHash;

    public short userCharIndex;

    public AccountInfo(string name, string hash)
    {
        accountName = name;
        accountHash = hash;
        userCharIndex = 0;
    }
}

public struct Character
{
    public short charIndex;
    public short posX;
    public short posY;
}

public struct MapData
{
    public bool blocked;
}

public delegate void OnStateChangeHandler();

public class AOGameManager : MonoBehaviour
{
    public const int NUMSKILLS = 20;

    public event OnStateChangeHandler OnStateChange;    

    private static AOGameManager _instance;
    public static AOGameManager Instance => _instance;

    private static TcpSocket _gameSocket;
    public static TcpSocket gameSocket => _gameSocket;

    public GameState gameState { get; private set; }
    public AccountInfo currentAccount;

    public MainMenu MainMenuWindow;
    public GameObject playerObject;
    public GameObject blockObject; //TODO: used just for debugging

    public ByteQueue incomingData;
    public ByteQueue outgoingData;

    public Character[] charList = new Character[10000];
    public Dictionary<Vector2, MapData> mapData = new Dictionary<Vector2, MapData>();

    protected AOGameManager() { }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        incomingData = new ByteQueue();
        outgoingData = new ByteQueue();

    }

    public void SetGameState(GameState state)
    {
        this.gameState = state;
        OnStateChange();
    }

    public void OnApplicationQuit()
    {
        gameSocket.Disconnect();
        AOGameManager._instance = null;
    }

    public void InitProtocol()
    {
        if (gameSocket == null)
        {
            _gameSocket = new TcpSocket(ref incomingData,ref outgoingData);
            Debug.Log("Protocol created.");
        }
    }

    public void FlushBuffer()
    {
        if (outgoingData.queueLength > 0)
        {
            gameSocket.Send();
        }
    }
}
