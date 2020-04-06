using System.Collections.Generic;
using UnityEngine;

// Game States
// for now we are only using these two
public enum GameState { INTRO, MAIN_MENU, INGAME }

public enum Gender
{
    NADA = 0,
    HOMBRE = 1,
    MUJER = 2
}

public enum Class
{
    NADA = 0,
    MAGO = 1,
    CLERIGO = 2,
    GUERRERO = 3,
    ASESINO = 4,
    LADRON = 5,
    BARDO = 6,
    DRUIDA = 7,
    BANDIDO = 8,
    PALADIN = 9,
    CAZADOR = 10,
    TRABAJADOR = 11,
    PIRATA = 12
}

public enum Homeland
{
    NADA = 0,
    ULLATHORPE = 1,
    NIX = 2,
    BANDERBILL = 3,
    LINDOS = 4,
    ARGHAL = 5,
    ARKHEIN = 6,
    LAST_CITY = 7
}

public enum Race : byte
{
    NADA = 0,
    HUMANO = 1,
    ELFO = 2,
    DROW = 3,
    GNOMO = 4,
    ENANO = 5
}

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

public struct GrhData
{
    short sX;
    short sY;
    int fileNum;
    short pixelWidth;
    short pixelHeight;
    float tileWidth;
    float TileHeight;
    short NumFrames;
    int[] Frames;
    float speed;
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

    public static AOGameManager Instance { get; private set; }
    public static TcpSocket gameSocket { get; private set; }
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
        DontDestroyOnLoad(gameObject);

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        incomingData = new ByteQueue();
        outgoingData = new ByteQueue();

    }

    public void SetGameState(GameState state)
    {
        gameState = state;
        OnStateChange();
    }

    public void OnApplicationQuit()
    {
        gameSocket.Disconnect();
        Instance = null;
    }

    public void InitProtocol()
    {
        if (gameSocket == null)
        {
            gameSocket = new TcpSocket(ref incomingData,ref outgoingData);
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
