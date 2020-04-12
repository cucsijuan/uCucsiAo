using System.Collections.Generic;
using System.IO;
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
    public int userMap;
    public string mapName;
    public AOPosition userPos;

    public AccountInfo(string name, string hash)
    {
        accountName = name;
        accountHash = hash;
        userCharIndex = 0;
        userMap = 0;
        mapName = "";
        userPos = new AOPosition(0,0);
}
}

public struct Character
{
    public short charIndex;
    public AOPosition pos;
}

public struct GrhData
{
    public short sX;
    public short sY;
    public int fileNum;
    public short pixelWidth;
    public short pixelHeight;
    public float tileWidth;
    public float TileHeight;
    public short NumFrames;
    public int[] Frames;
    public float speed;
}

public struct Grh //TODO: this is not useful in this engine keeping it for compatibility, but should check if its is really necessary
{
    public int grhIndex;
    public float frameCounter;
    public float speed;
    public byte started;
    public short loop;
    public float angle;
}

public struct WorldPos
{
    public short map;
    public short x;
    public short y;
}

public struct MapData
{
    public Grh[] graphic;
    public short charIndex;
    public Grh objGrh;

    public short NPCIndex;
    //public Obj objInfo; //TODO: implement objects
    public WorldPos tileExit;
    public byte blocked;

    public short trigger;
    public int[] engine_Light;
    public int particle_Group_Index;

    public Grh fX;
    public short FxIndex;
    
}

public delegate void OnStateChangeHandler();

public class AOGameManager : MonoBehaviour
{
    public const int NUMSKILLS = 20;

    public const int MAPMAX_X = 100;
    public const int MAPMAX_Y = 100;

    public event OnStateChangeHandler OnStateChange;    

    private static AOGameManager _instance;
    public static AOGameManager Instance => _instance;

    private static TcpSocket _gameSocket;
    public static TcpSocket gameSocket => _gameSocket;

    public static AOGameManager Instance { get; private set; }
    public static TcpSocket gameSocket { get; private set; }
    public GameState gameState { get; private set; }
    
    public AccountInfo currentAccount;

    public MainMenu MainMenuWindow;
    public MainGame MainGamewindow;
    public GameObject playerObject;
    public GameObject blockObject; //TODO: used just for debugging

    public ByteQueue incomingData;
    public ByteQueue outgoingData;

    public Dictionary<int, Character> charList = new Dictionary<int, Character>();
    public Dictionary<AOPosition, MapData> mapData = new Dictionary<AOPosition, MapData>();
    public GrhData[] grhData;
    public AOSpriteCache spriteCache;

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

    public void InitGrhData()
    {
        try
        {
            grhData = AoFileIO.LoadGrhs();

            if (grhData.Length == 0)
            {
                Debug.LogError("InitGrhData: No graphics has been loaded.");
                return;
            }

            Debug.Log("InitGrhData: Loaded " + grhData.Length + " graphics.");

            spriteCache = new AOSpriteCache();
            spriteCache.BuildCache();

            Debug.Log("Texture cache built.");
        }
        catch (System.InvalidOperationException ex)
        {
            Debug.LogError(ex.StackTrace);
            throw;
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
