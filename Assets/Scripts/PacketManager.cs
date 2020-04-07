
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum ServerPacketID
{
    none,
    Logged,                 // LOGGED
    RemoveDialogs,          // QTDL
    RemoveCharDialog,       // QDL
    NavigateToggle,         // NAVEG
    Disconnect,             // FINOK
    CommerceEnd,            // FINCOMOK
    BankEnd,                // FINBANOK
    CommerceInit,           // INITCOM
    BankInit,               // INITBANCO
    UserCommerceInit,        // INITCOMUSU
    UserCommerceEnd,         // FINCOMUSUOK
    UserOfferConfirm,
    CommerceChat,
    UpdateSta,               // ASS
    UpdateMana,             // ASM
    UpdateHP,                // ASH
    UpdateGold,              // ASG
    UpdateBankGold,
    UpdateExp,               // ASE
    ChangeMap,               // CM
    PosUpdate,              // PU
    ChatOverHead,            // ||
    ConsoleMsg,              // || - Beware!! its the same as above, but it was properly splitted
    GuildChat,               // |+
    ShowMessageBox,          // !!
    UserIndexInServer,       // IU
    UserCharIndexInServer,   // IP
    CharacterCreate,         // CC
    CharacterRemove,         // BP
    CharacterChangeNick,
    CharacterMove,           // MP, +, * and _ //
    ForceCharMove,
    CharacterChange,         // CP
    HeadingChange,
    ObjectCreate,            // HO
    ObjectDelete,            // BO
    BlockPosition,           // BQ
    PlayMp3,
    PlayMidi,                // TM
    PlayWave,                // TW
    guildList,               // GL
    AreaChanged,             // CA
    PauseToggle,             // BKW
    RainToggle,              // LLU
    CreateFX,                // CFX
    UpdateUserStats,         // EST
    ChangeInventorySlot,     // CSI
    ChangeBankSlot,          // SBO
    ChangeSpellSlot,         // SHS
    Atributes,               // ATR
    BlacksmithWeapons,       // LAH
    BlacksmithArmors,        // LAR
    InitCarpenting,          // OBR
    RestOK,                  // DOK
    errorMsg,                // ERR
    Blind,                   // CEGU
    Dumb,                    // DUMB
    ShowSignal,              // MCAR
    ChangeNPCInventorySlot,  // NPCI
    UpdateHungerAndThirst,   // EHYS
    Fame,                    // FAMA
    MiniStats,               // MEST
    LevelUp,                 // SUNI
    AddForumMsg,             // FMSG
    ShowForumForm,           // MFOR
    SetInvisible,            // NOVER
    DiceRoll,                // DADOS
    MeditateToggle,          // MEDOK
    BlindNoMore,             // NSEGUE
    DumbNoMore,              // NESTUP
    SendSkills,              // SKILLS
    TrainerCreatureList,     // LSTCRI
    guildNews,               // GUILDNE
    OfferDetails,            // PEACEDE & ALLIEDE
    AlianceProposalsList,    // ALLIEPR
    PeaceProposalsList,      // PEACEPR
    CharacterInfo,           // CHRINFO
    GuildLeaderInfo,         // LEADERI
    GuildMemberInfo,
    GuildDetails,            // CLANDET
    ShowGuildFundationForm,  // SHOWFUN
    ParalizeOK,              // PARADOK
    ShowUserRequest,         // PETICIO
    ChangeUserTradeSlot,     // COMUSUINV
    SendNight,               // NOC
    Pong,
    UpdateTagAndStatus,
    
    //GM, messages
    SpawnList,               // SPL
    ShowSOSForm,             // MSOS
    ShowMOTDEditionForm,     // ZMOTD
    ShowGMPanelForm,         // ABPANEL
    UserNameList,            // LISTUSU
    ShowDenounces,
    RecordList,
    RecordDetails,
    
    ShowGuildAlign,
    ShowPartyForm,
    UpdateStrenghtAndDexterity,
    UpdateStrenght,
    UpdateDexterity,
    AddSlots,
    MultiMessage,
    StopWorking,
    CancelOfferItem,
    PalabrasMagicas,
    PlayAttackAnim,
    FXtoMap,
    AccountLogged, //CHOTS | Accounts
    SearchList,
    QuestDetails,
    QuestListSend,
    CreateDamage,           // CDMG
    UserInEvent,
    RenderMsg,
    DeletedChar,
    EquitandoToggle,
    EnviarDatosServer,
    InitCraftman,
    EnviarListDeAmigos
}

enum ClientPacketID
{ 
    none,
    LoginExistingChar,          //OLOGIN
    ThrowDices,                 //TIRDAD
    LoginNewChar,               //NLOGIN
    Talk,                       //;
    Yell,                       //-
    Whisper,                    //\
    Walk,                       //M
    RequestPositionUpdate,      //RPU
    Attack,                     //AT
    PickUp,                     //AG
    SafeToggle,                 //SEG & SEG  (SEG//s behaviour has to be coded in the client)
    ResuscitationSafeToggle,
    RequestGuildLeaderInfo,     //GLINFO
    RequestAtributes,           //ATR
    RequestFame,                //FAMA
    RequestSkills,              //ESKI
    RequestMiniStats,           //FEST
    CommerceEnd,                //FINCOM
    UserCommerceEnd,            //FINCOMUSU
    UserCommerceConfirm,
    CommerceChat,
    BankEnd,                    //FINBAN
    UserCommerceOk,             //COMUSUOK
    UserCommerceReject,         //COMUSUNO
    Drop,                       //TI
    CastSpell,                  //LH
    LeftClick,                  //LC
    DoubleClick,                //RC
    Work,                       //UK
    UseSpellMacro,              //UMH
    UseItem,                    //USA
    CraftBlacksmith,            //CNS
    CraftCarpenter,             //CNC
    WorkLeftClick,              //WLC
    CreateNewGuild,             //CIG
    sadasdA,
    EquipItem,                  //EQUI
    ChangeHeading,              //CHEA
    ModifySkills,               //SKSE
    Train,                      //ENTR
    CommerceBuy,                //COMP
    BankExtractItem,            //RETI
    CommerceSell,               //VEND
    BankDeposit,                //DEPO
    ForumPost,                  //DEMSG
    MoveSpell,                  //DESPHE
    MoveBank,
    ClanCodexUpdate,            //DESCOD
    UserCommerceOffer,          //OFRECER
    GuildAcceptPeace,           //ACEPPEAT
    GuildRejectAlliance,        //RECPALIA
    GuildRejectPeace,           //RECPPEAT
    GuildAcceptAlliance,        //ACEPALIA
    GuildOfferPeace,            //PEACEOFF
    GuildOfferAlliance,         //ALLIEOFF
    GuildAllianceDetails,       //ALLIEDET
    GuildPeaceDetails,          //PEACEDET
    GuildRequestJoinerInfo,     //ENVCOMEN
    GuildAlliancePropList,      //ENVALPRO
    GuildPeacePropList,         //ENVPROPP
    GuildDeclareWar,            //DECGUERR
    GuildNewWebsite,            //NEWWEBSI
    GuildAcceptNewMember,       //ACEPTARI
    GuildRejectNewMember,       //RECHAZAR
    GuildKickMember,            //ECHARCLA
    GuildUpdateNews,            //ACTGNEWS
    GuildMemberInfo,            //1HRINFO<
    GuildOpenElections,         //ABREELEC
    GuildRequestMembership,     //SOLICITUD
    GuildRequestDetails,        //CLANDETAILS
    Online,                     //ONLINE
    Quit,                       //SALIR
    GuildLeave,                 //SALIRCLAN
    RequestAccountState,        //BALANCE
    PetStand,                   //QUIETO
    PetFollow,                  //ACOMPANAR
    ReleasePet,                 //LIBERAR
    TrainList,                  //ENTRENAR
    Rest,                       //DESCANSAR
    Meditate,                   //MEDITAR
    Resucitate,                 //RESUCITAR
    Heal,                       //CURAR
    Help,                       //AYUDA
    RequestStats,               //EST
    CommerceStart,              //COMERCIAR
    BankStart,                  // /BOVEDA
    Enlist,                     // /ENLISTAR
    Information,                // /INFORMACION
    Reward,                     // /RECOMPENSA
    RequestMOTD,                // /MOTD
    UpTime,                     // /UPTIME
    PartyLeave,                 // /SALIRPARTY
    PartyCreate,                // /CREARPARTY
    PartyJoin,                  // /PARTY
    Inquiry,                    // /ENCUESTA ( with no params )
    GuildMessage,               // /CMSG
    PartyMessage,               // /PMSG
    GuildOnline,                // /ONLINECLAN
    PartyOnline,                // /ONLINEPARTY
    CouncilMessage,             // /BMSG
    RoleMasterRequest,          // /ROL
    GMRequest,                  // /GM
    bugReport,                  // /_BUG
    ChangeDescription,          // /DESC
    GuildVote,                  // /VOTO
    punishments,                // /PENAS
    ChangePassword,             // /CONTRASENA
    Gamble,                     // /APOSTAR
    InquiryVote,                // /ENCUESTA ( with parameters )
    LeaveFaction,               // /RETIRAR ( with no arguments )
    BankExtractGold,            // /RETIRAR ( with arguments )
    BankDepositGold,            // /DEPOSITAR
    Denounce,                   // /DENUNCIAR
    GuildFundate,               // /FUNDARCLAN
    GuildFundation,
    PartyKick,                  // /ECHARPARTY
    PartySetLeader,             // /PARTYLIDER
    PartyAcceptMember,          // /ACCEPTPARTY
    Ping,                       // /PING
    RequestPartyForm,
    ItemUpgrade,
    GMCommands,
    InitCrafting,
    Home,
    ShowGuildNews,
    ShareNpc,                   // /COMPARTIR
    StopSharingNpc,
    Consultation,
    moveItem,
    LoginExistingAccount,       //CHOTS | Accounts
    LoginNewAccount,            //CHOTS | Accounts
    CentinelReport,
    Ecvc,
    Acvc,
    IrCvc,
    DragAndDropHechizos,
    HungerGamesCreate,
    HungerGamesJoin,
    HungerGamesDelete,
    Quest,                      // /QUEST
    QuestAccept,
    QuestListRequest,
    QuestDetailsRequest,
    QuestAbandon,
    CambiarContrasena,
    FightSend,
    FightAccept,
    CloseGuild,
    Discord,
    DeleteChar,
    ObtenerDatosServer,
    CraftsmanCreate,
    AddAmigos,
    DelAmigos,
    OnAmigos,
    MsgAmigos
}

public class PacketManager : MonoBehaviour
{
    public static PacketManager Instance { get; private set; }

    AOGameManager GM;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Start()
    {
        GM = AOGameManager.Instance;
    }

    public void HandleReceivedData()
    {
        ServerPacketID packetID = (ServerPacketID)GM.incomingData.PeekByte();

        Debug.Log("Received data packet: " + System.Enum.GetName(typeof(ServerPacketID), packetID));

        switch (packetID)
        {
            case ServerPacketID.none:
                Debug.LogError("ERROR HandleReceivedData: Received packet ID 0");
                break;
            case ServerPacketID.Logged:
                HandleLogged();
                break;
            case ServerPacketID.RemoveDialogs:
                break;
            case ServerPacketID.RemoveCharDialog:
                break;
            case ServerPacketID.NavigateToggle:
                HandleNavigateToggle();
                break;
            case ServerPacketID.Disconnect:
                break;
            case ServerPacketID.CommerceEnd:
                break;
            case ServerPacketID.BankEnd:
                break;
            case ServerPacketID.CommerceInit:
                break;
            case ServerPacketID.BankInit:
                break;
            case ServerPacketID.UserCommerceInit:
                break;
            case ServerPacketID.UserCommerceEnd:
                break;
            case ServerPacketID.UserOfferConfirm:
                break;
            case ServerPacketID.CommerceChat:
                break;
            case ServerPacketID.UpdateSta:
                break;
            case ServerPacketID.UpdateMana:
                break;
            case ServerPacketID.UpdateHP:
                break;
            case ServerPacketID.UpdateGold:
                break;
            case ServerPacketID.UpdateBankGold:
                break;
            case ServerPacketID.UpdateExp:
                break;
            case ServerPacketID.ChangeMap:
                HandleChangeMap();
                break;
            case ServerPacketID.PosUpdate:
                HandlePosUpdate();
                break;
            case ServerPacketID.ChatOverHead:
                break;
            case ServerPacketID.ConsoleMsg:
                HandleConsoleMessage();
                break;
            case ServerPacketID.GuildChat:
                HandleGuildChat();
                break;
            case ServerPacketID.ShowMessageBox:
                break;
            case ServerPacketID.UserIndexInServer:
                HandleUserIndexInServer();
                break;
            case ServerPacketID.UserCharIndexInServer:
                HandleUserCharIndexInServer();
                break;
            case ServerPacketID.CharacterCreate:
                HandleCharacterCreate();
                break;
            case ServerPacketID.CharacterRemove:
                break;
            case ServerPacketID.CharacterChangeNick:
                break;
            case ServerPacketID.CharacterMove:
                break;
            case ServerPacketID.ForceCharMove:
                break;
            case ServerPacketID.CharacterChange:
                HandleCharacterChange();
                break;
            case ServerPacketID.HeadingChange:
                break;
            case ServerPacketID.ObjectCreate:
                HandleObjectCreate();
                break;
            case ServerPacketID.ObjectDelete:
                break;
            case ServerPacketID.BlockPosition:
                HandleBlockPosition();
                break;
            case ServerPacketID.PlayMp3:
                HandlePlayMP3();
                break;
            case ServerPacketID.PlayMidi:
                HandlePlayMIDI();
                break;
            case ServerPacketID.PlayWave:
                break;
            case ServerPacketID.guildList:
                break;
            case ServerPacketID.AreaChanged:
                HandleAreaChanged();
                break;
            case ServerPacketID.PauseToggle:
                break;
            case ServerPacketID.RainToggle:
                HandleRainToggle();
                break;
            case ServerPacketID.CreateFX:
                HandleCreateFX();
                break;
            case ServerPacketID.UpdateUserStats:
                HandleUpdateUserStats();
                break;
            case ServerPacketID.ChangeInventorySlot:
                HandleChangeInventorySlot();
                break;
            case ServerPacketID.ChangeBankSlot:
                break;
            case ServerPacketID.ChangeSpellSlot:
                HandleChangeSpellSlot();
                break;
            case ServerPacketID.Atributes:
                break;
            case ServerPacketID.BlacksmithWeapons:
                break;
            case ServerPacketID.BlacksmithArmors:
                break;
            case ServerPacketID.InitCarpenting:
                break;
            case ServerPacketID.RestOK:
                break;
            case ServerPacketID.errorMsg:
                HandleErrorMessage();
                break;
            case ServerPacketID.Blind:
                break;
            case ServerPacketID.Dumb:
                break;
            case ServerPacketID.ShowSignal:
                break;
            case ServerPacketID.ChangeNPCInventorySlot:
                break;
            case ServerPacketID.UpdateHungerAndThirst:
                HandleUpdateHungerAndThirst();
                break;
            case ServerPacketID.Fame:
                break;
            case ServerPacketID.MiniStats:
                break;
            case ServerPacketID.LevelUp:
                HandleLevelUp();
                break;
            case ServerPacketID.AddForumMsg:
                break;
            case ServerPacketID.ShowForumForm:
                break;
            case ServerPacketID.SetInvisible:
                HandleSetInvisible();
                break;
            case ServerPacketID.DiceRoll:
                HandleDiceRoll();
                break;
            case ServerPacketID.MeditateToggle:
                break;
            case ServerPacketID.BlindNoMore:
                break;
            case ServerPacketID.DumbNoMore:
                break;
            case ServerPacketID.SendSkills:
                HandleSendSkills();
                break;
            case ServerPacketID.TrainerCreatureList:
                break;
            case ServerPacketID.guildNews:
                HandleGuildNews();
                break;
            case ServerPacketID.OfferDetails:
                break;
            case ServerPacketID.AlianceProposalsList:
                break;
            case ServerPacketID.PeaceProposalsList:
                break;
            case ServerPacketID.CharacterInfo:
                break;
            case ServerPacketID.GuildLeaderInfo:
                break;
            case ServerPacketID.GuildMemberInfo:
                break;
            case ServerPacketID.GuildDetails:
                break;
            case ServerPacketID.ShowGuildFundationForm:
                break;
            case ServerPacketID.ParalizeOK:
                HandleParalizeOK();
                break;
            case ServerPacketID.ShowUserRequest:
                break;
            case ServerPacketID.ChangeUserTradeSlot:
                break;
            case ServerPacketID.SendNight:
                break;
            case ServerPacketID.Pong:
                break;
            case ServerPacketID.UpdateTagAndStatus:
                break;
            case ServerPacketID.SpawnList:
                break;
            case ServerPacketID.ShowSOSForm:
                break;
            case ServerPacketID.ShowMOTDEditionForm:
                break;
            case ServerPacketID.ShowGMPanelForm:
                break;
            case ServerPacketID.UserNameList:
                break;
            case ServerPacketID.ShowDenounces:
                break;
            case ServerPacketID.RecordList:
                break;
            case ServerPacketID.RecordDetails:
                break;
            case ServerPacketID.ShowGuildAlign:
                break;
            case ServerPacketID.ShowPartyForm:
                break;
            case ServerPacketID.UpdateStrenghtAndDexterity:
                HandleUpdateStrenghtAndDexterity();
                break;
            case ServerPacketID.UpdateStrenght:
                break;
            case ServerPacketID.UpdateDexterity:
                break;
            case ServerPacketID.AddSlots:
                break;
            case ServerPacketID.MultiMessage:
                HandleMultiMessage();
                break;
            case ServerPacketID.StopWorking:
                break;
            case ServerPacketID.CancelOfferItem:
                break;
            case ServerPacketID.PalabrasMagicas:
                break;
            case ServerPacketID.PlayAttackAnim:
                break;
            case ServerPacketID.FXtoMap:
                break;
            case ServerPacketID.AccountLogged:
                HandleAccountLogged();
                break;
            case ServerPacketID.SearchList:
                break;
            case ServerPacketID.QuestDetails:
                break;
            case ServerPacketID.QuestListSend:
                break;
            case ServerPacketID.CreateDamage:
                break;
            case ServerPacketID.UserInEvent:
                break;
            case ServerPacketID.RenderMsg:
                break;
            case ServerPacketID.DeletedChar:
                break;
            case ServerPacketID.EquitandoToggle:
                break;
            case ServerPacketID.EnviarDatosServer:
                break;
            case ServerPacketID.InitCraftman:
                break;
            case ServerPacketID.EnviarListDeAmigos:
                HandleReceiveFriendList();
                break;
            default:
                break;

        }
    }

    private void HandleAccountLogged()
    {
        ByteQueue buffer = new ByteQueue();

        buffer.CopyBuffer(GM.incomingData);

        byte packetID = buffer.ReadByte();

        string accountName = buffer.ReadASCIIString();
        string accountHash = buffer.ReadASCIIString();

        //Save current account data
        GM.currentAccount = new AccountInfo(accountName, accountHash);

        byte numberOfCharacters = buffer.ReadByte();

        GM.MainMenuWindow.CharSelectionPanel.gameObject.SetActive(true);

        if (numberOfCharacters > 0)
        {
            pjCuenta[] cPJ = new pjCuenta[numberOfCharacters];

            List<string> names = new List<string>();

            for (int i = 0; i < cPJ.Length; i++)
            {
                cPJ[i].nombre = buffer.ReadASCIIString();
                cPJ[i].body = buffer.ReadInt16();
                cPJ[i].head = buffer.ReadInt16();
                cPJ[i].weapon = (byte)buffer.ReadInt16();
                cPJ[i].shield = (byte)buffer.ReadInt16();
                cPJ[i].helmet = (byte)buffer.ReadInt16();
                cPJ[i].Class = buffer.ReadByte();
                cPJ[i].race = buffer.ReadByte();
                cPJ[i].mapa = buffer.ReadInt16();
                cPJ[i].level = buffer.ReadByte();
                cPJ[i].gold = buffer.ReadInteger();
                cPJ[i].criminal = buffer.ReadBoolean();
                cPJ[i].dead = buffer.ReadBoolean();

                cPJ[i].gameMaster = buffer.ReadBoolean();

                names.Add(cPJ[i].nombre);
            }

            GM.MainMenuWindow.CharSelectionDropdown.AddOptions(names);

        }

        GM.incomingData.CopyBuffer(buffer);
    }

    public void HandleChangeInventorySlot()
    {
        if (GM.incomingData.queueLength < 24)
        {
            Debug.Log("HandleChangeInventorySlot: not enough data to read");
            return;
        }

        ByteQueue buffer = new ByteQueue();

        buffer.CopyBuffer(GM.incomingData);

        buffer.ReadByte();

        byte slot;
        short objIndex;
        string name;
        short amount;
        bool equipped;
        int grhIndex;
        byte objType;
        short maxHit;
        short minHit;
        short maxDef;
        short minDef;
        float value;

        slot = buffer.ReadByte();
        objIndex = buffer.ReadInt16();
        name = buffer.ReadASCIIString();
        amount = buffer.ReadInt16();
        equipped = buffer.ReadBoolean();
        grhIndex = buffer.ReadInteger();
        objType = buffer.ReadByte();
        maxHit = buffer.ReadInt16();
        minHit = buffer.ReadInt16();
        maxDef = buffer.ReadInt16();
        minDef = buffer.ReadInt16();
        value = buffer.ReadSingle();

        //TODO: if equiped hago que se aplique efectos en la UI ver condigo de VB

        GM.incomingData.CopyBuffer(buffer);

    }

    public void HandleChangeSpellSlot()
    {
        if (GM.incomingData.queueLength < 4)
        {
            Debug.Log("HandleChangeSpellSlot: not enough data to read");
            return;
        }

        ByteQueue buffer = new ByteQueue();

        buffer.CopyBuffer(GM.incomingData);

        buffer.ReadByte();

        byte slot = buffer.ReadByte();

        string str;

        short[] UserHechizos = new short[35]; //TODO: esto tiene que ser parte de un objeto con la data del user

        UserHechizos[slot-1] = buffer.ReadInt16();

        //TODO: ver codigo VB ***********************
        if (slot <= 30)
        {
            //pido el nombre del hechi
            if (true /*si el nombre no es nulo*/)
            {
                // lo meto en la lista de hechizos directamente
            }
            else
            {
                // agrego texto con la palabra NADA en la lista
            }
        }
        else
        {
            //pido el nombre del hechi
            if (true /*si el nombre no es nulo*/)
            {
                // agrego un item a la lista
            }
            else
            {
                // agrego texto con la palabra NADA en la lista
            }
        }
        //ENDTODO: probablemente todo esto no sirva *****************

        GM.incomingData.CopyBuffer(buffer);
    }
    public void HandleReceiveFriendList()
    {
        if (GM.incomingData.queueLength < 6)
        {
            Debug.Log("HandleReceiveFriendList: not enough data to read");
            return;
        }

        ByteQueue buffer = new ByteQueue();

        buffer.CopyBuffer(GM.incomingData);

        buffer.ReadByte();

        byte slot = buffer.ReadByte();

        //TODO: handlear la lista de amigos
        buffer.ReadASCIIString(); //esto es el item que se deberia agregar a la lista.

        GM.incomingData.CopyBuffer(buffer);
    }

    public void HandleConsoleMessage()
    {
        if (GM.incomingData.queueLength < 4)
        {
            Debug.Log("HandleConsoleMessage: not enough data to read");
            return;
        }

        ByteQueue buffer = new ByteQueue();

        buffer.CopyBuffer(GM.incomingData);

        buffer.ReadByte();

        string chat;
        short fontIndex;
        string str;
        Color color;

        chat = buffer.ReadASCIIString();
        fontIndex = buffer.ReadByte();


        if (chat.IndexOf('~') == 0)
        {
            string[] fields = chat.Split('~');
            string colorString = chat.Substring(chat.IndexOf('~'), chat.LastIndexOf('~'));

            color.r = byte.Parse(fields[0]);
            color.g = byte.Parse(fields[1]);
            color.b = byte.Parse(fields[2]);

            Debug.Log(fields[3]);
        }
        else
        {
            Debug.Log(chat);
        }

        //TODO: esto tiene mas cosas para parties y imprime colores en la consola ver funcion de VB.

        GM.incomingData.CopyBuffer(buffer);
    }

    public void HandleParalizeOK()
    {
        GM.incomingData.ReadByte();

        //TODO: aca hace UserParalizado = !UserParalizado;

        short timeRemaining = GM.incomingData.ReadInt16();

        //TODO: UserParalizadoSegundosRestantes <- si el timeRemaining > 0 se lo agrega ver la funcion de VB

        //TODO: si efectivamente esta paralizado mostramos cuanto le queda de tiempo en pantalla

    }

    public void HandleUserIndexInServer()
    {
        if (GM.incomingData.queueLength < 3)
        {
            Debug.Log("HandleUserIndexInServer: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        //TODO: this should be set on an Object that stores player data
        short userIndex = GM.incomingData.ReadInt16();
    }

    public void HandleChangeMap()
    {
        if (GM.incomingData.queueLength < 5)
        {
            Debug.Log("HandleChangeMap: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        //TODO: this should be set on an Object that stores player data
        short userMap = GM.incomingData.ReadInt16();
        string nameMap = GM.incomingData.ReadASCIIString();

        //TODO: Once on-the-fly editor is implemented check for map version before loading....
        //For now we just drop it
        GM.incomingData.ReadInt16();

        //TODO:  in this part the map gets loaded from a file, rain played see VB, for now im filling the map data here

        MapData tempMapData;
        tempMapData.blocked = false;

        for (int x = 1; x <= 100; x++)
        {
            for (int y = 1; y <= 100; y++)
            {
                GM.mapData.Add(new Vector2(x, y), tempMapData);
            }
        }
    }

    public void HandlePlayMP3()
    {
        if (GM.incomingData.queueLength < 5)
        {
            Debug.Log("HandlePlayMP3: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        short currentMp3 = GM.incomingData.ReadInt16();
        short loops = GM.incomingData.ReadInt16();

        //TODO: play mp3 audio
    }

    public void HandlePlayMIDI()
    {
        if (GM.incomingData.queueLength < 5)
        {
            Debug.Log("HandlePlayMIDI: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        short currentMidi = GM.incomingData.ReadInt16();
        short loops = GM.incomingData.ReadInt16();

        //TODO: play midi audio
    }

    public void HandleSetInvisible()
    {
        if (GM.incomingData.queueLength < 4)
        {
            Debug.Log("HandleSetInvisible: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        //TODO: this has to be in  a object with user information
        short charIndex = GM.incomingData.ReadInt16();

        bool UserInvisible = GM.incomingData.ReadBoolean();
        //TODO: call  setInvisible()

        short timeRemaining = GM.incomingData.ReadInt16();

        //TODO: show timer of invisibility
    }

    public void HandleUserCharIndexInServer()
    {
        if (GM.incomingData.queueLength < 3)
        {
            Debug.Log("HandleUserCharIndexInServer: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        //TODO: seteamos el userindex ver codigo VB
        GM.currentAccount.userCharIndex = GM.incomingData.ReadInt16();
    }

    public void HandleUpdateUserStats()
    {
        if (GM.incomingData.queueLength < 26)
        {
            Debug.Log("HandleUserCharIndexInServer: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        //TODO: these should be part of an obj with user information

        short userMaxHP = GM.incomingData.ReadInt16();
        short userMinHP = GM.incomingData.ReadInt16();
        short userMaxMAN = GM.incomingData.ReadInt16();
        short userMinMAN = GM.incomingData.ReadInt16();
        short userMaxSTA = GM.incomingData.ReadInt16();
        short userMinSTA = GM.incomingData.ReadInt16();
        int userGLD = GM.incomingData.ReadInteger();
        byte userLvl = GM.incomingData.ReadByte();
        int userPasarNivel = GM.incomingData.ReadInteger();
        int userExp = GM.incomingData.ReadInteger();

        //TODO: update all the UI with this information see VB code

    }

    public void HandleUpdateHungerAndThirst()
    {
        if (GM.incomingData.queueLength < 5)
        {
            Debug.Log("HandleUpdateHungerAndThirst: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        //TODO: these should be part of an obj with user information
        byte userMaxAGU = GM.incomingData.ReadByte();
        byte userMinAGU = GM.incomingData.ReadByte();
        byte userMaxHAM = GM.incomingData.ReadByte();
        byte userMinHAM = GM.incomingData.ReadByte();

        //TODO: update all the UI with this information see VB code

    }

    public void HandleUpdateStrenghtAndDexterity()
    {
        if (GM.incomingData.queueLength < 3)
        {
            Debug.Log("HandleUpdateStrenghtAndDexterity: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        //TODO: these should be part of an obj with user information
        byte userFuerza = GM.incomingData.ReadByte();
        byte userAgilidad = GM.incomingData.ReadByte();

        //TODO: update all the UI with this information see VB code

    }

    public void HandleGuildChat()
    {
        if (GM.incomingData.queueLength < 3)
        {
            Debug.Log("HandleGuildChat: not enough data to read");
            return;
        }

        ByteQueue buffer = new ByteQueue();

        buffer.CopyBuffer(GM.incomingData);

        buffer.ReadByte();

        string chat = buffer.ReadASCIIString();
        string str;
        Color color;

        if (chat.IndexOf('~') == 0)
        {
            string[] fields = chat.Split('~');
            string colorString = chat.Substring(chat.IndexOf('~'), chat.LastIndexOf('~'));

            color.r = byte.Parse(fields[0]);
            color.g = byte.Parse(fields[1]);
            color.b = byte.Parse(fields[2]);

            Debug.Log(fields[3]);
        }
        else
        {
            Debug.Log(chat);
        }

        //TODO: esto  imprime colores en la consola ver funcion de VB.

        GM.incomingData.CopyBuffer(buffer);
    }

    public void HandleSendSkills()
    {
        if (GM.incomingData.queueLength < 2 + (AOGameManager.NUMSKILLS * 2)) //TODO: parar NUMSKILLS a una clase de statics
        {
            Debug.Log("HandleSendSkills: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        //TODO: these should be part of an obj with user information
        byte userClase = GM.incomingData.ReadByte();

        for (int i = 0; i < AOGameManager.NUMSKILLS; i++)
        {
            //TODO: add skill and skill value to user data
            GM.incomingData.ReadByte();
            GM.incomingData.ReadByte();
        }

        //TODO: see VB code

    }

    public void HandleLevelUp()
    {
        if (GM.incomingData.queueLength < 3)
        {
            Debug.Log("HandleLevelUp: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        //TODO: these should be part of an obj with user information
        short skillPoints = GM.incomingData.ReadInt16();

        //TODO: show skills available feedback if necessary
    }

    public void HandleNavigateToggle()
    {
        GM.incomingData.ReadByte();

        //TODO: these should be part of an obj with user information
        /*bool userNavigating = !userNavigating;*/
    }

    public void HandleMultiMessage()
    {
        byte bodyPart;
        short damage;
        short spellIndex;
        string name;

        GM.incomingData.ReadByte();

        switch ((eMessages)GM.incomingData.ReadByte())
        {
            case eMessages.NPCSwing:
                break;
            case eMessages.NPCKillUser:
                break;
            case eMessages.BlockedWithShieldUser:
                break;
            case eMessages.BlockedWithShieldOther:
                break;
            case eMessages.UserSwing:
                break;
            case eMessages.SafeModeOn:
                break;
            case eMessages.SafeModeOff:
                break;
            case eMessages.ResuscitationSafeOff:
                break;
            case eMessages.ResuscitationSafeOn:
                break;
            case eMessages.NobilityLost:
                break;
            case eMessages.CantUseWhileMeditating:
                break;
            case eMessages.NPCHitUser:
                GM.incomingData.ReadByte(); //TODO: see VB code
                break;
            case eMessages.UserHitNPC:
                GM.incomingData.ReadInteger(); //TODO: see VB code
                break;
            case eMessages.UserAttackedSwing:
                GM.incomingData.ReadInt16(); //TODO: see VB code
                break;
            case eMessages.UserHittedByUser:
                short attackerName = GM.incomingData.ReadInt16();//TODO: see VB code
                bodyPart = GM.incomingData.ReadByte();
                damage = GM.incomingData.ReadInt16();
                //TODO: see VB code
                break;
            case eMessages.UserHittedUser:
                short victimName = GM.incomingData.ReadInt16();//TODO: see VB code
                bodyPart = GM.incomingData.ReadByte();
                damage = GM.incomingData.ReadInt16();
                //TODO: see VB code
                break;
            case eMessages.WorkRequestTarget:
                byte usingSkill = GM.incomingData.ReadByte(); //TODO: this shoul be part of user data obj
                //TODO: see VB code
                break;
            case eMessages.HaveKilledUser:
                short killedUser = GM.incomingData.ReadInt16();
                int exp = GM.incomingData.ReadInteger();
                //TODO: see VB code
                break;
            case eMessages.UserKill:
                short killerUser = GM.incomingData.ReadInt16();
                //TODO: see VB code
                break;
            case eMessages.EarnExp:
                break;
            case eMessages.GoHome:
                byte distance = GM.incomingData.ReadByte();
                short time = GM.incomingData.ReadInt16();
                string hogar = GM.incomingData.ReadASCIIString();
                //TODO: see VB code
                break;
            case eMessages.CancelGoHome:
                break;
            case eMessages.FinishHome:
                break;
            case eMessages.UserMuerto:
                break;
            case eMessages.NpcInmune:
                break;
            case eMessages.Hechizo_HechiceroMSG_NOMBRE:
                spellIndex = GM.incomingData.ReadByte();
                name = GM.incomingData.ReadASCIIString();
                //TODO: see VB code
                break;
            case eMessages.Hechizo_HechiceroMSG_ALGUIEN:
                spellIndex = GM.incomingData.ReadByte();
                //TODO: see VB code
                break;
            case eMessages.Hechizo_HechiceroMSG_CRIATURA:
                spellIndex = GM.incomingData.ReadByte();
                //TODO: see VB code
                break;
            case eMessages.Hechizo_PropioMSG:
                spellIndex = GM.incomingData.ReadByte();
                //TODO: see VB code
                break;
            case eMessages.Hechizo_TargetMSG:
                spellIndex = GM.incomingData.ReadByte();
                name = GM.incomingData.ReadASCIIString();
                //TODO: see VB code
                break;
            default:
                Debug.Log("HandleMultiMessage: received incorrect enum id");
                break;
        }

    }

    public void HandleLogged()
    {
        GM.incomingData.ReadByte();

        //TODO: these should be part of an obj with user and world information
        byte userClase = GM.incomingData.ReadByte();
        bool engineRun = true;
        bool nombres = true;
        bool bRain = false;

        //TODO: at this point we should close the login window and start the game
        SceneManager.LoadScene("MainScene");

    }

    public void HandleGuildNews()
    {
        if (GM.incomingData.queueLength < 7)
        {
            Debug.Log("HandleGuildNews: not enough data to read");
            return;
        }

        ByteQueue buffer = new ByteQueue();

        buffer.CopyBuffer(GM.incomingData);

        buffer.ReadByte();

        int upperGuildList;
        string sTemp;

        //TODO: this string is the one that is showed in the guild news window
        string news = buffer.ReadASCIIString();

        string[] guildList = buffer.ReadASCIIString().Split('\0');


        for (int i = 0; i < guildList.Length; i++)
        {
            //TODO: ver codigo VB
        }

        //TODO: aca mostramos las noticias

        GM.incomingData.CopyBuffer(buffer);
    }

    public void HandleRainToggle()
    {
        GM.incomingData.ReadByte();

        //TODO: toggle rain and sounds se VB code;
    }

    public void HandleAreaChanged()
    {
        if (GM.incomingData.queueLength < 3)
        {
            Debug.Log("HandleAreaChanged: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        byte X = GM.incomingData.ReadByte();
        byte Y = GM.incomingData.ReadByte();

        //TODO: call cambio de area see VB
    }

    public void HandleObjectCreate()
    {
        if (GM.incomingData.queueLength < 7)
        {
            Debug.Log("HandleObjectCreate: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        byte x = GM.incomingData.ReadByte();
        byte y = GM.incomingData.ReadByte();
        int grhIndex = GM.incomingData.ReadInteger();

        //TODO: create object on map
    }

    public void HandleCharacterChange()
    {
        if (GM.incomingData.queueLength < 17)
        {
            Debug.Log("HandleObjectCreate: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        short CharIndex = GM.incomingData.ReadInt16();

        //TODO: set the char paperdolling and fx
        GM.incomingData.ReadInt16(); //body

        GM.incomingData.ReadInt16();//head

        GM.incomingData.ReadInt16();//weapon

        GM.incomingData.ReadInt16(); //shield

        GM.incomingData.ReadInt16(); // helmet

        GM.incomingData.ReadInt16();// FX
        GM.incomingData.ReadInt16();// FX second param
    }

    public void HandleCharacterCreate()
    {
        if (GM.incomingData.queueLength < 24)
        {
            Debug.Log("HandleCharacterCreate: not enough data to read");
            return;
        }

        ByteQueue buffer = new ByteQueue();

        buffer.CopyBuffer(GM.incomingData);

        buffer.ReadByte();

        short charIndex = buffer.ReadInt16();
        short body = buffer.ReadInt16();
        short head = buffer.ReadInt16();
        EHeading heading = (EHeading)buffer.ReadByte();
        GM.charList[charIndex].posX = buffer.ReadByte();
        GM.charList[charIndex].posY = buffer.ReadByte();
        short weapon = buffer.ReadInt16();
        short shield = buffer.ReadInt16();
        short helmet = buffer.ReadInt16();


        //TODO: these first two are for setting FXs. also all of these reads are set on the Charlist see VB code
        //SetFX
        buffer.ReadInt16();
        buffer.ReadInt16();

        //read nick
        buffer.ReadASCIIString();

        //read nick color and the set criminal status and atackable status
        byte nickColor = buffer.ReadByte();

        //read privs and set flags like Council or RoleMaster see VB code
        short privs = buffer.ReadByte();

        //ENDTODO

        //TODO: we make the char 

        GM.incomingData.CopyBuffer(buffer);

    }

    public void HandleBlockPosition()
    {
        if (GM.incomingData.queueLength < 24)
        {
            Debug.Log("HandleBlockPosition: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        Vector2 mapCoord = new Vector2(GM.incomingData.ReadByte(), GM.incomingData.ReadByte());
        bool block = GM.incomingData.ReadBoolean();

        //TODO: Set block on map
        MapData TempData;
        if (GM.mapData.TryGetValue(mapCoord,out TempData))
        {
            TempData.blocked = !TempData.blocked;
            GM.mapData[mapCoord] = TempData;
        }
        else
        {
            Debug.LogWarning("HandleBlockPosition: tried to block wrong map coordinates");
        }


    }

    public void HandleCreateFX()
    {
        if (GM.incomingData.queueLength < 7)
        {
            Debug.Log("HandleCreateFX: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        short charIndex = GM.incomingData.ReadInt16();
        short fx = GM.incomingData.ReadInt16();
        short loops = GM.incomingData.ReadInt16();

        //TODO: set FXs see VB code

    }

    public void HandlePosUpdate()
    {
        if (GM.incomingData.queueLength < 3)
        {
            Debug.Log("HandlePosUpdate: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        //TODO: remove old user and proper map position setting
        GM.charList[GM.currentAccount.userCharIndex].posX = GM.incomingData.ReadByte();
        GM.charList[GM.currentAccount.userCharIndex].posY = GM.incomingData.ReadByte();

    }

    public void HandleDiceRoll()
    {
        if (GM.incomingData.queueLength < 6)
        {
            Debug.Log("HandleDiceRoll: not enough data to read");
            return;
        }

        GM.incomingData.ReadByte();

        /* TODO: Acomodar este mambo en un objeto con la info. del char. */
        GM.incomingData.ReadByte();
        GM.incomingData.ReadByte();
        GM.incomingData.ReadByte();
        GM.incomingData.ReadByte();
        GM.incomingData.ReadByte();
        

    }

    public void HandleErrorMessage()
    {
        if (GM.incomingData.queueLength < 3)
        {
            Debug.Log("HandleErrorMessage: not enough data to read");
            return;
        }

        ByteQueue buffer = new ByteQueue();

        buffer.CopyBuffer(GM.incomingData);

        buffer.ReadByte();

        Debug.LogError("Server Error: " + buffer.ReadASCIIString());

        //TODO: aca desconecta el socket pero solo si no esta el frm crear pj visible (por que??)

        GM.incomingData.CopyBuffer(buffer);

    }

    /*********************************************************************************/
    /********************************** PACKET WRITING *******************************/
    /*********************************************************************************/

    public void WriteLoginExistingAccount(string Account, string Password)
    {
        GM.outgoingData.locked = true;

        GM.outgoingData.WriteByte(130);

        GM.outgoingData.WriteASCIIString(Account);
        GM.outgoingData.WriteASCIIString(Password);
        //App version
        GM.outgoingData.WriteByte(0);
        GM.outgoingData.WriteByte(13);
        GM.outgoingData.WriteByte(0);

        GM.outgoingData.locked = false;
    }

    public void WriteLoginExistingChar(string userName, string AccountHash)
    {
        GM.outgoingData.locked = true;

        GM.outgoingData.WriteByte(1);

        GM.outgoingData.WriteASCIIString(userName);
        GM.outgoingData.WriteASCIIString(AccountHash);

        //App version
        GM.outgoingData.WriteByte(0);
        GM.outgoingData.WriteByte(13);
        GM.outgoingData.WriteByte(0);

        GM.outgoingData.locked = false;
    }

    public void WriteLoginNewAccount(string email, string password)
    {
        GM.outgoingData.locked = true;

        GM.outgoingData.WriteByte((byte)ClientPacketID.LoginNewAccount);

        GM.outgoingData.WriteASCIIString(email);
        GM.outgoingData.WriteASCIIString(password);

        //App version
        GM.outgoingData.WriteByte(0);
        GM.outgoingData.WriteByte(13);
        GM.outgoingData.WriteByte(0);

        GM.outgoingData.locked = false;
    }

    public void WriteWalk(byte heading)
    {
        GM.outgoingData.locked = true;

        GM.outgoingData.WriteByte((byte)ClientPacketID.Walk);

        GM.outgoingData.WriteByte(heading);

        GM.outgoingData.locked = false;
    }

    public void WriteThrowDices()
    {
        GM.outgoingData.locked = true;

        GM.outgoingData.WriteByte((byte)ClientPacketID.ThrowDices);

        GM.outgoingData.locked = false;
    }

    public void WriteLoginNewChar(string nombreChar, int raza, int sexo, int clase, int head, int hogar)
    {
        GM.outgoingData.locked = true;

        GM.outgoingData.WriteByte((byte)ClientPacketID.LoginNewChar);

        GM.outgoingData.WriteASCIIString(nombreChar);
        GM.outgoingData.WriteASCIIString(GM.currentAccount.accountHash);

        GM.outgoingData.WriteByte(0);
        GM.outgoingData.WriteByte(13);
        GM.outgoingData.WriteByte(0);

        GM.outgoingData.WriteByte((byte)raza);
        GM.outgoingData.WriteByte((byte)sexo);
        GM.outgoingData.WriteByte((byte)clase);
        GM.outgoingData.WriteInt16((short)head);
        GM.outgoingData.WriteByte((byte)hogar);

        GM.outgoingData.locked = false;
    }
}

