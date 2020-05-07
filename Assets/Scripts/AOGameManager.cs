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
    public int body;
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

public struct HeadData
{
    public Grh[] Heads;
}

public struct BodyData
{
    public Grh[] Bodies;
    public Vector2 HeadOffset;
}

public struct WeaponAnimData
{
    public Grh[] WeaponAnims;
}

public struct ShieldAnimData
{
    public Grh[] ShieldAnims;
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

    public GameState gameState { get; private set; }
    
    public AccountInfo currentAccount;

    public MainMenu MainMenuWindow;
    public MainGame MainGamewindow;
    public GameObject playerObject;
    public GameObject npcObject;
    public GameObject blockObject; //TODO: used just for debugging

    public ByteQueue incomingData;
    public ByteQueue outgoingData;

    public Dictionary<int, Character> charList = new Dictionary<int, Character>();
    public Dictionary<AOPosition, MapData> mapData = new Dictionary<AOPosition, MapData>();

    //***** Graphic stuff ********************
    public GrhData[] grhData;
    public BodyData[] bodyData;
    public HeadData[] headData;
    public HeadData[] helmetAnimData;
    public WeaponAnimData[] weaponAnimData;
    public ShieldAnimData[] shieldAnimData;
    //TODO: public FxData[] As tIndiceFx
    public AOSpriteCache spriteCache;
    //*****************************************

    protected AOGameManager() { }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance != null)
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
        gameState = state;
        OnStateChange();
    }

    public void OnApplicationQuit()
    {
        gameSocket.Disconnect();
        _instance = null;
    }

    public void InitProtocol()
    {
        if (_gameSocket == null)
        {
            _gameSocket = new TcpSocket(ref incomingData,ref outgoingData);
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

            bodyData = AoFileIO.LoadBodies();
            headData = AoFileIO.LoadHeads();
            helmetAnimData = AoFileIO.LoadHelmets();
            weaponAnimData = AoFileIO.LoadWeaponAnims();
            

        }
        catch (System.InvalidOperationException ex)
        {
            Debug.LogError(ex.StackTrace);
            throw;
        }
    }

    public void UpdateCharBody(ref Character charInfo, PlayerController playerController) //TODO: this function should not be here maybe on a character base class?
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(playerController.BodyAnimator.runtimeAnimatorController);
        var clips = aoc.animationClips;

        var idleAnims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        for (int i = 0; i < bodyData[charInfo.body].Bodies.Length; i++)
        {
            AnimationClip tempAnim = Resources.Load<AnimationClip>("Animations/" + bodyData[charInfo.body].Bodies[i].grhIndex /*+ ".anim"*/);

            AnimationClip tempIdleAnim = Resources.Load<AnimationClip>("Animations/IDLE_" + bodyData[charInfo.body].Bodies[i].grhIndex /*+ ".anim"*/);

            if (tempAnim == null)
            {
                Debug.LogError("Animation: " + bodyData[charInfo.body].Bodies[i] + " does not found while loading body " + charInfo.body);
                return;
            }

            if (tempIdleAnim == null)
            {
                Debug.LogError("Animation: IDLE_" + bodyData[charInfo.body].Bodies[i] + " does not found while loading body " + charInfo.body);
                return;
            }

            idleAnims.Add(new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[i], tempAnim));
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[i], tempAnim));
        }

        idleAnims.AddRange(anims);
        aoc.ApplyOverrides(idleAnims);

        playerController.BodyAnimator.runtimeAnimatorController = aoc;
    }

    public void CreateCharacter(Character charInfo)
    {
        GameObject NewChar = GameObject.Instantiate(npcObject, charInfo.pos.MapPositionToVector3(), Quaternion.identity);
        PlayerController playerController = NewChar.GetComponent<PlayerController>();

        UpdateCharBody(ref charInfo, playerController);
    }

    public void FlushBuffer()
    {
        if (outgoingData.queueLength > 0)
        {
            gameSocket.Send();
        }
    }

}
