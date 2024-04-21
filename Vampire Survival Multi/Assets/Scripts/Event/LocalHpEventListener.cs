public class LocalHpEventListener : GameEventListener
{
    private void Awake()
    {
        PlayerData playerData = LocalPlayerData.Instance.PlayerData;

        gameEvent = playerData.HPEvent;
    }
}