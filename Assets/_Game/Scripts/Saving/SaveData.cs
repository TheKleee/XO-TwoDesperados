

public static class SaveData
{
    //Since this game is fairly small we'll only use 1 savedata for the whole project :)
    public delegate void SaveState();
    public static event SaveState save;

    public static void Save() =>    
        save.Invoke();

    public static void PrepareForSave()
    {

    }
    
}
