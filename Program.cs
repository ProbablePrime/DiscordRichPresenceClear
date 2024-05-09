// See https://aka.ms/new-console-template for more information
using DiscordGameSDKWrapper;

/// Use your client ID from Discord's developer site.
var clientID = Environment.GetEnvironmentVariable("DISCORD_CLIENT_ID");
if (clientID == null)
{
    clientID = "418559331265675294";
}
var discord = new Discord(Int64.Parse(clientID), (ulong)CreateFlags.NoRequireDiscord);

var manager = discord.GetActivityManager();
var activity = new Activity();

activity.Timestamps.Start = DateTimeOffset.Now.ToUnixTimeSeconds();
activity.State = "";
activity.Type = ActivityType.Playing;
activity.Details = "";
activity.Assets.LargeText = "";
activity.SupportedPlatforms = (uint)ActivitySupportedPlatformFlags.Desktop;
activity.Party = new ActivityParty
{
    Id = "Blank",
    Privacy = ActivityPartyPrivacy.Private,
    Size = {
        CurrentSize = 0,
        MaxSize = 0
    },
};

discord.SetLogHook(LogLevel.Debug, (level, message) =>
{
    Console.WriteLine("Log[{0}] {1}", level, message);
});

Console.WriteLine("Now setting activity:");
manager.UpdateActivity(activity, OnUpdateActivityDone);

Console.WriteLine("Now clearing activity:");

// WILL FAIL SEE: https://github.com/discord/discord-api-docs/issues/6612
manager.ClearActivity(OnClearActivityDone);

try
{
    while (true)
    {
        discord.RunCallbacks();
        Thread.Sleep(1000 / 60);
    }
}
finally
{
    discord.Dispose();
}


void OnUpdateActivityDone(Result result)
{
    Console.WriteLine($"Setting:{result}");
}



void OnClearActivityDone(Result result)
{
    Console.WriteLine($"Clearing result:{result}");
}

