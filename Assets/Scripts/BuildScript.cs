using UnityEditor;
using System.IO;

public class BuildScript
{
    [MenuItem("Build/Windows")]
    static void BuildWindows()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.target = BuildTarget.StandaloneWindows64;
        options.locationPathName = "Builds/Windows/CardBattler.exe";
        options.scenes = new[] { "Assets/Scenes/MainScene.unity" };
        BuildPipeline.BuildPlayer(options);
    }

    [MenuItem("Build/Mac")]
    static void BuildMac()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.target = BuildTarget.StandaloneOSX;
        options.locationPathName = "Builds/Mac/CardBattler.app";
        options.scenes = new[] { "Assets/Scenes/MainScene.unity" };
        BuildPipeline.BuildPlayer(options);
    }

    [MenuItem("Build/WebGL")]
    static void BuildWebGL()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.target = BuildTarget.WebGL;
        options.locationPathName = "Builds/WebGL";
        options.scenes = new[] { "Assets/Scenes/MainScene.unity" };
        BuildPipeline.BuildPlayer(options);
    }

    [MenuItem("Build/Android")]
    static void BuildAndroid()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.target = BuildTarget.Android;
        options.locationPathName = "Builds/Android/CardBattler.apk";
        options.scenes = new[] { "Assets/Scenes/MainScene.unity" };
        BuildPipeline.BuildPlayer(options);
    }
}