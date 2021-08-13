#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Builders
{
    public class Builder : MonoBehaviour
    {
        [MenuItem("Jobs/Build/Windows")]
        public static void StartWindows()
        {
            // Get filename.
            string path = "build";
            var filename = path.Split('/');
            BuildPlayer(BuildTarget.StandaloneWindows, filename[filename.Length - 1], path + "/");
        }

        [MenuItem("Jobs/Build/Windows + Mac OSX + Linux")]
        public static void StartAll()
        {
            string path = "build";
            var filename = path.Split('/');
            BuildPlayer(BuildTarget.StandaloneOSX, filename[filename.Length - 1], path + "/");
            BuildPlayer(BuildTarget.StandaloneLinuxUniversal, filename[filename.Length - 1], path + "/");
            BuildPlayer(BuildTarget.StandaloneWindows, filename[filename.Length - 1], path + "/");
        }


        static void BuildPlayer(BuildTarget buildTarget, string filename, string path)
        {
            string fileExtension = "";
            string dataPath = "";
            string modifier = "";

            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    modifier = "_windows";
                    fileExtension = ".exe";
                    dataPath = "_Data/";
                    break;
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSX:
                    modifier = "_mac-osx";
                    fileExtension = ".app";
                    dataPath = fileExtension + "/Contents/";
                    break;
                case BuildTarget.StandaloneLinux:
                case BuildTarget.StandaloneLinux64:
                case BuildTarget.StandaloneLinuxUniversal:
                    modifier = "_linux";
                    dataPath = "_Data/";
                    switch (buildTarget)
                    {
                        case BuildTarget.StandaloneLinux:
                            fileExtension = ".x86";
                            break;
                        case BuildTarget.StandaloneLinux64:
                            fileExtension = ".x64";
                            break;
                        case BuildTarget.StandaloneLinuxUniversal:
                            fileExtension = ".x86_64";
                            break;
                    }

                    break;
            }

            Debug.Log("====== BuildPlayer: " + buildTarget.ToString() + " at " + path + filename);
            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);

            string buildPath = path + filename + modifier + "/";
            //Debug.Log("buildpath: " + buildPath);
            string playerPath = buildPath + filename + modifier + fileExtension;
            //Debug.Log("playerpath: " + playerPath);
            BuildPipeline.BuildPlayer(GetScenePaths(), playerPath, buildTarget,
                buildTarget == BuildTarget.StandaloneWindows ? BuildOptions.ShowBuiltPlayer : BuildOptions.None);

            string fullDataPath = buildPath + filename + modifier + dataPath;
            Debug.Log("fullDataPath: " + fullDataPath);
            //CopyFromProjectAssets( fullDataPath, "languages");
        }

        static string[] GetScenePaths()
        {
            string[] scenes = new string[EditorBuildSettings.scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = EditorBuildSettings.scenes[i].path;
            }

            return scenes;
        }

        static string GetProjectName()
        {
            string[] s = Application.dataPath.Split('/');
            return s[s.Length - 2];
        }

        static string GetProjectFolderPath()
        {
            var s = Application.dataPath;
            s = s.Substring(s.Length - 7, 7); // remove "Assets/"
            return s;
        }

        static void CopyFromProjectAssets(string fullDataPath, string assetsFolderPath, bool deleteMetaFiles = true)
        {
            Debug.Log("CopyFromProjectAssets: copying over " + assetsFolderPath);
            FileUtil.ReplaceDirectory(Application.dataPath + "/" + assetsFolderPath, fullDataPath + assetsFolderPath);

            if (deleteMetaFiles)
            {
                var metaFiles = Directory.GetFiles(
                    fullDataPath + assetsFolderPath,
                    "*.meta",
                    SearchOption.AllDirectories);

                foreach (var meta in metaFiles) FileUtil.DeleteFileOrDirectory(meta);
            }
        }

        static BuildPlayerOptions GetBuildPlayerOptions(
            bool askForLocation = false,
            BuildPlayerOptions defaultOptions = new BuildPlayerOptions())
        {
            var method = typeof(BuildPlayerWindow).GetMethod(
                "GetBuildPlayerOptionsInternal",
                BindingFlags.NonPublic | BindingFlags.Static);

            return (BuildPlayerOptions) method.Invoke(
                null,
                new object[] {askForLocation, defaultOptions});
        }
        // private static void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset) {
        //
        // 	string[] files = Directory.GetFiles(path);
        //
        // 	foreach (string filename in files) {
        //
        // 		FileInfo fi = new FileInfo(filename);
        //
        // 		string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
        // 		entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
        // 		ZipEntry newEntry = new ZipEntry(entryName);
        // 		newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity
        //
        // 		newEntry.Size = fi.Length;
        //
        // 		zipStream.PutNextEntry(newEntry);
        //
        // 		// Zip the file in buffered chunks
        // 		// the "using" will close the stream even if an exception occurs
        // 		byte[ ] buffer = new byte[4096];
        // 		using (FileStream streamReader = File.OpenRead(filename)) {
        // 			StreamUtils.Copy(streamReader, zipStream, buffer);
        // 		}
        // 		zipStream.CloseEntry();
        // 	}
        // 	string[ ] folders = Directory.GetDirectories(path);
        // 	foreach (string folder in folders) {
        // 		CompressFolder(folder, zipStream, folderOffset);
        // 	}
        // }
    }
}
#endif
