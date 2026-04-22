using UnityEngine;
using System.Runtime.InteropServices;

public static class WebGLBridge
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    static extern void QuitGame();
#endif

    public static void Quit()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        QuitGame();
#else
        Application.Quit();
#endif
    }
}