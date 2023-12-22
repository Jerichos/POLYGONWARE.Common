using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace POLYGONWARE.Common.Editor
{
public class MultiBuildRun : EditorWindow
{
    private int _numberOfInstances = 1;
    private string _buildPath = "/build/";
    
    private bool _playInEditor = false;
    private bool _cleanBuild = false;
    private bool _windowed = true;
    private bool _alwaysOnTop = true;
    
    private BuildTarget _buildTarget = BuildTarget.StandaloneWindows64;
    private BuildOptions _buildOptions = BuildOptions.Development;
    
    private List<Process> _runningProcesses = new();
    
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    private const uint SWP_SHOWWINDOW = 0x0040;
    
    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_NOSIZE = 0x0001;
    
    [MenuItem("POLYGONWARE/MultiBuild")]
    private static void ShowWindow()
    {
        var window = GetWindow<MultiBuildRun>();
        window.titleContent = new GUIContent("Build & Run");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Build and Run Options", EditorStyles.boldLabel);

        _numberOfInstances = EditorGUILayout.IntField("Number of Instances", _numberOfInstances);
        _playInEditor = EditorGUILayout.Toggle("Play in Editor", _playInEditor);
        _windowed = EditorGUILayout.Toggle("Windowed", _windowed);
        _alwaysOnTop = EditorGUILayout.Toggle("Always On Top", _alwaysOnTop);
        _cleanBuild = EditorGUILayout.Toggle("Clean Build", _cleanBuild);
        _buildPath = EditorGUILayout.TextField("Build folder", _buildPath);
        _buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", _buildTarget);
        _buildOptions = (BuildOptions)EditorGUILayout.EnumFlagsField("Build Options", _buildOptions);
        
        GUILayout.Space(10);

        for (int i = 0; i < _runningProcesses.Count; i++)
        {
            if(_runningProcesses[i] == null || _runningProcesses[i].HasExited)
                _runningProcesses.RemoveAt(i);
        }
        
        if (_runningProcesses.Count == 0 && GUILayout.Button("Build and Run"))
            BuildAndRun();

        if (_runningProcesses.Count > 0 && GUILayout.Button("Stop Running Instances"))
        {
            StopRunningInstances();
            
            // if editor is running stop it as well
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }
        }
    }

    private void LoadPrefs()
    {
        _numberOfInstances = EditorPrefs.GetInt("MultiBuildRun.NumberOfInstances", _numberOfInstances);
        _playInEditor = EditorPrefs.GetBool("MultiBuildRun.PlayInEditor", _playInEditor);
        _windowed = EditorPrefs.GetBool("MultiBuildRun.Windowed", _windowed);
        _alwaysOnTop = EditorPrefs.GetBool("MultiBuildRun.AlwaysOnTop", _alwaysOnTop);
        _cleanBuild = EditorPrefs.GetBool("MultiBuildRun.CleanBuild", _cleanBuild);
        _buildPath = EditorPrefs.GetString("MultiBuildRun.BuildPath", _buildPath);
        _buildTarget = (BuildTarget)EditorPrefs.GetInt("MultiBuildRun.BuildTarget", (int)_buildTarget);
        _buildOptions = (BuildOptions)EditorPrefs.GetInt("MultiBuildRun.BuildOptions", (int)_buildOptions);
    }

    private void SavePrefs()
    {
        EditorPrefs.SetInt("MultiBuildRun.NumberOfInstances", _numberOfInstances);
        EditorPrefs.SetBool("MultiBuildRun.PlayInEditor", _playInEditor);
        EditorPrefs.SetBool("MultiBuildRun.Windowed", _windowed);
        EditorPrefs.SetBool("MultiBuildRun.AlwaysOnTop", _alwaysOnTop);
        EditorPrefs.SetBool("MultiBuildRun.CleanBuild", _cleanBuild);
        EditorPrefs.SetString("MultiBuildRun.BuildPath", _buildPath);
        EditorPrefs.SetInt("MultiBuildRun.BuildTarget", (int)_buildTarget);
        EditorPrefs.SetInt("MultiBuildRun.BuildOptions", (int)_buildOptions);
    }

    private void OnEnable()
    {
        LoadPrefs();
    }
    
    private void OnDisable()
    {
        SavePrefs();
    }

    private void BuildAndRun()
    {
        // save scene
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        
        // get project folder path
        string exePath = Application.dataPath.Replace("/Assets", "");
        string exeName = "Tanks.exe";
        string buildPath = exePath + "/" + _buildPath + "/";
        exePath += "/" + _buildPath + "/" + exeName;
        _runningProcesses.Clear();
        Debug.Log(exePath);

        if (_cleanBuild)
        {
            Debug.Log("Clean build");
            
            if(Directory.Exists(buildPath))
                Directory.Delete(buildPath, true);
        }
        
        if (!Directory.Exists(buildPath))
            Directory.CreateDirectory(buildPath);
        
        // Build settings
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { EditorSceneManager.GetActiveScene().path },
            locationPathName = exePath,
            target = _buildTarget,
            options = _buildOptions
        };
        
        if (_windowed)
        {
            buildPlayerOptions.options |= BuildOptions.ShowBuiltPlayer;
        }
        
        // Build the game
        Debug.Log("Building... " + exePath);
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        if(File.Exists(exePath))
            Debug.Log("Build succeeded: " + exePath);
        else
        {
            Debug.Log("Build failed: " + exePath);
            return;
        }
        
        // Run multiple instances
        for (int i = 0; i < _numberOfInstances; i++)
        {
            var process = Process.Start(exePath);
            if (process != null)
            {
                _runningProcesses.Add(process);
                int instanceIndex = i;
                ThreadPool.QueueUserWorkItem(_ => PositionWindow(process, instanceIndex));
            }
        }
        
        if (_playInEditor)
        {
            EditorApplication.isPlaying = true;
        }
    }

    private void StopRunningInstances()
    {
        foreach (var process in _runningProcesses)
        {
            if (!process.HasExited)
            {
                try
                {
                    Debug.Log("stopping instance...");
                    process.Kill();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
        
        _runningProcesses.Clear();
        Debug.Log("stopped all running instances.");
    }
    
    private void PositionWindow(Process process, int instanceIndex)
    {
        // Delay to ensure the window is initialized
        Thread.Sleep(1000); // 5 seconds delay, adjust as needed

        if (!process.HasExited)
        {
            process.Refresh(); // Refresh to get updated process info
            
            int width = 800;  // Set to your desired width
            int height = 600; // Set to your desired height
            
            IntPtr hWnd = process.MainWindowHandle;
            if (hWnd != IntPtr.Zero)
            {
                int x = instanceIndex * width;
                int y = 0;
                
                if(!_alwaysOnTop)
                    SetWindowPos(hWnd, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOSIZE);
                else
                    SetWindowPos(hWnd, HWND_TOPMOST, x, y, width, height, SWP_SHOWWINDOW);
            }
        }
    }
}
}