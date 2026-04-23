using System.Runtime.InteropServices;
public static class OrientationBridge
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    static extern void UnlockOrientation();
#endif

    public static void Unlock()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        UnlockOrientation();
#endif
    }
}