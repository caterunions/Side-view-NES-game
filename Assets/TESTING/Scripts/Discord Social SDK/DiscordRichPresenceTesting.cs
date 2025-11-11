using UnityEngine;
using UnityEngine.UI;
using Discord.Sdk;
using System.Linq;
using TMPro;

public class TestingDiscordRichPresence : MonoBehaviour
{
    [SerializeField]
    private ulong clientId;

    private Client client;

    void OnEnable()
    {
        //create client
        client = new Client();
        client.SetApplicationId(clientId); // <--- our clients id from discord dev portal
        client.AddLogCallback((msg, severity) => Debug.Log($"[DiscordSDK({severity})]: {msg}"), LoggingSeverity.Info);
        SetRichPresence();
    }

    //sets activity
    private void SetRichPresence()
    {
        //create activity
        Activity activity = new Activity();

        activity.SetName("Cate's Game");

        activity.SetState("Testing The Game");
        activity.SetDetails("Score: 1000");

        //game timer
        ActivityTimestamps timestamps = new ActivityTimestamps();
        timestamps.SetStart((ulong)System.DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        activity.SetTimestamps(timestamps);

        ActivityAssets assets = new ActivityAssets();
        assets.SetLargeImage("cate"); // <--- image keys set in discord dev portal
        assets.SetLargeText("Cate's Game!!!");
        assets.SetSmallImage("aiden"); 
        assets.SetSmallText("Playing as ShipName");
        activity.SetAssets(assets);

        //update rich presence
        client.UpdateRichPresence(activity, (ClientResult result) =>
        {
            if (result.Successful())
                Debug.Log("[DiscordSDK]: Rich presence updated");
            else
                Debug.LogWarning("[DiscordSDK]: Failed to update rich presence: " + result.Error());
        });

    }

    //dispose
    void OnApplicationQuit()
    {
        client?.Dispose();
        client = null;
    }
}