using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

public class PlayerBuilder : Editor
{

    [MenuItem("Build/ Build Every Player")]
    static void Build()
    {
        WinBuild();
        MacBuild();
        LinuxBuild();
    }

    [MenuItem("Build/ Build Windows Player")]
    static void WinBuild()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        BuildReport report = BuildPipeline.BuildPlayer(scenes, "Builds/RoboSubWin/robosub_sim.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
        BuildSummary summary = report.summary;


        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
    [MenuItem("Build/ Build MacOS Player")]
    static void MacBuild()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        BuildReport report = BuildPipeline.BuildPlayer(scenes, "Builds/RoboSubMac/robosub_sim.app", BuildTarget.StandaloneOSX, BuildOptions.None);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
    [MenuItem("Build/ Build Linux Player")]
    static void LinuxBuild()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        BuildReport report = BuildPipeline.BuildPlayer(scenes, "Builds/RoboSubLinux/robosub_sim.x86_64", BuildTarget.StandaloneLinux64, BuildOptions.None);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}
