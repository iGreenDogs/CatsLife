using System.Security.AccessControl;
using System.Diagnostics;
using Discord;
using System.Collections.Generic;
using UnityEngine;

public class Discord_Controller : MonoBehaviour
{

    public long applicationID;
    [Space]
    public string details = "Play at wasders.itch.io/cats-life";
    public string state = "Lurking in the menu";
    [Space]
    public string largeImage = "game_logo";
    public string largeText = "Cat's Life";

    private long time;

    private static bool instanceExists;
    public Discord.Discord discord;

    void Awake() {
        if (!instanceExists){
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length > 1){
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);

        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        UpdateStatus();
    }

    // Update is called once per frame
    void Update()
    {
        try{
            discord.RunCallbacks();
        }
        catch{
            Destroy(gameObject);
        }
    }
    void LateUpdate() {
        UpdateStatus();
    }
    void UpdateStatus() {
        try{
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity {
                Details = details,
                State = state,
                Assets = {
                    LargeImage = largeImage,
                    LargeText = largeText,
                    SmallImage = largeImage
                },
                Timestamps = {
                    Start = time
                }
            };

            activityManager.UpdateActivity(activity, (res) => {
                if (res != Discord.Result.Ok) UnityEngine.Debug.LogWarning("Failed connecting to Discord!");
            });
        }
        catch{
            Destroy(gameObject);
        }
    }
}