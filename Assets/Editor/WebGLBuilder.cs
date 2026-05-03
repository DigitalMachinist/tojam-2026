using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class WebGLBuilder
{
    public static void Build()
    {
        var args = Environment.GetCommandLineArgs();
        string outputPath = null;
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "-customBuildPath")
            {
                outputPath = args[i + 1];
                break;
            }
        }

        if (string.IsNullOrEmpty(outputPath))
        {
            throw new Exception("Missing -customBuildPath <path> argument.");
        }

        var scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            throw new Exception("No scenes are enabled in Build Settings.");
        }

        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outputPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        var report = BuildPipeline.BuildPlayer(options);
        var summary = report.summary;

        Debug.Log($"Build {summary.result}: {summary.totalSize} bytes -> {summary.outputPath}");

        if (summary.result != BuildResult.Succeeded)
        {
            EditorApplication.Exit(1);
        }
    }
}
