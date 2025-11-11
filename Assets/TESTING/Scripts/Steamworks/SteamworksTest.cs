using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using TMPro;


//tests steamworks API functionality. example of a "SteamManager"
public class SteamworksTest : MonoBehaviour
{
    public RawImage avatarImage;

    [SerializeField]
    private TextMeshProUGUI _usernameGUI;
    [SerializeField]
    private TextMeshProUGUI _debugGUI;
    private bool initialzed = false;

    //INITIALIZE

    void OnEnable()
    {
        //start steam API (Custom SteamManager)
        initialzed = SteamAPI.Init();

        if (!initialzed)
        {
            Debug.LogError("[Steamworks]: Unable to connect to Steam API. Is steam open?");
            return;
        }

        SteamFriends.SetRichPresence("status", "Testing The Game");
        SteamFriends.SetRichPresence("score", "1000");
        SteamFriends.SetRichPresence("ship", "ShipName");
        SteamFriends.SetRichPresence("steam_display", "#Status_InGame");

        Debug.Log("[Steamworks]: Connected to Steam API!");


        string debugString = "";



        CSteamID selfID = SteamUser.GetSteamID();
        debugString += "UserID: " + selfID.ToString() + "\n";

        string personaName = SteamFriends.GetPersonaName();
        _usernameGUI.text = "Hello " + personaName + "!";
        debugString += "Username: " + personaName + "\n";

        EPersonaState personaState = SteamFriends.GetFriendPersonaState(selfID);
        debugString += "State: " + GetPersonaStateString(personaState) + "\n";

        FriendGameInfo_t gameInfo;
        SteamFriends.GetFriendGamePlayed(selfID, out gameInfo);
        debugString += "AppID (static): " + gameInfo.m_gameID + "\n";

        int friends = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagAll);
        debugString += "Friend Count: " + friends + "\n";

        _debugGUI.text = debugString;

        //fetch avatar (async proc call)
        ApplyAvatarAsTexture(SteamFriends.GetLargeFriendAvatar(selfID));
    }

    void OnApplicationQuit()
    {
        if (!initialzed) return;
        SteamFriends.ClearRichPresence();
    }


    //UPDATE EVENTS/CALLBACKS (Custom SteamManager)

    void Update()
    {
        if (!initialzed) return;

        // Run callbacks/events from steam api (yes, this needs to be in update)
        SteamAPI.RunCallbacks();
    }

    //Helpers

    void ApplyAvatarAsTexture(int avatar)
    {
        if (avatar == 0)
        {
            Debug.LogError("[Steamworks]: Invalid avatar image ID.");
            return;
        }

        uint width, height;
        if (!SteamUtils.GetImageSize(avatar, out width, out height) || width == 0 || height == 0)
        {
            Debug.LogError("[Steamworks]: Failed to get avatar size.");
            return;
        }

        byte[] data = new byte[width * height * 4];
        if (!SteamUtils.GetImageRGBA(avatar, data, data.Length))
        {
            Debug.LogError("[Steamworks]: Failed to get avatar RGBA data.");
            return;
        }

        Texture2D tex = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
        tex.LoadRawTextureData(data);
        tex.Apply();

        avatarImage.texture = tex;
    }

    string GetPersonaStateString(EPersonaState state)
    {
        switch (state)
        {
            case EPersonaState.k_EPersonaStateOffline: return "Offline";
            case EPersonaState.k_EPersonaStateOnline: return "Online";
            case EPersonaState.k_EPersonaStateBusy: return "Busy";
            case EPersonaState.k_EPersonaStateAway: return "Away";
            case EPersonaState.k_EPersonaStateSnooze: return "Snooze";
            case EPersonaState.k_EPersonaStateLookingToTrade: return "Looking to Trade";
            case EPersonaState.k_EPersonaStateLookingToPlay: return "Looking to Play";
            default: return "Unknown";
        }
    }
}
