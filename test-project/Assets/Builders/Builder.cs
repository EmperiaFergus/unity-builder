#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
//using Cysharp.Threading.Tasks;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
// using SIDGIN.Common.Editors;
// using SIDGIN.Patcher.Unity;
// using SIDGIN.Patcher.Unity.Editors;
using UnityEditor;
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
            uploadToPatchKit(path + "/");
        }

        [MenuItem("Jobs/Build/ MacOS")]
        public static void StartMac()
        {
            string path = "build";
            var filename = path.Split('/');
            BuildPlayer(BuildTarget.StandaloneOSX, filename[filename.Length - 1], path + "/");
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

            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);

            string buildPath = path + filename + modifier + "/";
            string playerPath = buildPath + filename + modifier + fileExtension;
            BuildPipeline.BuildPlayer(GetScenePaths(buildTarget), playerPath, buildTarget,
                buildTarget == BuildTarget.StandaloneWindows ? BuildOptions.ShowBuiltPlayer : BuildOptions.None);

            string fullDataPath = buildPath + filename + modifier + dataPath;
            Debug.Log(fullDataPath);
            Console.WriteLine(fullDataPath);
            CreateZip("build/build.zip",fullDataPath);
        }

        static string[] GetScenePaths(BuildTarget buildTarget, bool useSidgin = false)
        {
            string[] scenes = new string[EditorBuildSettings.scenes.Length];
            int j = scenes.Length;
            for (int i = 0; i < j; i++)
            {
                if (!useSidgin)
                {
                    if (i == 0)
                    {
                        scenes = new string[scenes.Length-1];
                        continue;
                    }
                    scenes[i-1] = EditorBuildSettings.scenes[i].path;
                }
                else
                    scenes[i] = EditorBuildSettings.scenes[i].path;
            }
            return scenes;
        }
        public static void CreateZip(string stZipPath, string stDirToZip)
        {
            try
            {
                stDirToZip = Path.GetFullPath(stDirToZip);
                stZipPath = Path.GetFullPath(stZipPath);

                Console.WriteLine("Zip directory " + stDirToZip);

                Stack<FileInfo> stackFiles = DirExplore(stDirToZip);

                ZipOutputStream zipOutput = null;

                if (File.Exists(stZipPath))
                    File.Delete(stZipPath);

                Crc32 crc = new Crc32();
                zipOutput = new ZipOutputStream(File.Create(stZipPath));
                zipOutput.SetLevel(6); // 0 - store only to 9 - means best compression

                Debug.Log(stZipPath);
                Console.WriteLine(stZipPath);

                Console.WriteLine(stackFiles.Count + " files to zip.\n");

                int index = 0;
                foreach (FileInfo fi in stackFiles)
                {
                    ++index;
                    int percent = (int)((float)index / ((float)stackFiles.Count / 100));
                    if (percent % 1 == 0)
                    {
                        Console.CursorLeft = 0;
                        Console.Write(_stSchon[index % _stSchon.Length].ToString() + " " + percent + "% done.");
                    }
                    FileStream fs = File.OpenRead(fi.FullName);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);

                    //Create the right arborescence within the archive
                    string stFileName = fi.FullName.Remove(0, stDirToZip.Length + 1);
                    ZipEntry entry = new ZipEntry(stFileName);

                    entry.DateTime = DateTime.Now;

                    entry.Size = fs.Length;
                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);

                    entry.Crc = crc.Value;

                    zipOutput.PutNextEntry(entry);

                    zipOutput.Write(buffer, 0, buffer.Length);
                }
                zipOutput.Finish();
                zipOutput.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void amendSIDGINDefinitions(string definition)
        {
            var settingsData = File.ReadAllText($"{Application.dataPath}\\SIDGIN\\EditorResources\\SettingsData.asset");
            settingsData = settingsData.Replace("OSX",definition);
            settingsData = settingsData.Replace("Win64",definition);
            File.WriteAllText($"{Application.dataPath}\\SIDGIN\\EditorResources\\SettingsData.asset", settingsData);
        }

        static void uploadToPatchKit(string path)
        {
          string[] contents =
          {
            "#!/bin/sh",
            $"start {Application.dataPath}/builders/patchKit/patchkit-tools make-version -s ac1ae6ae296777d8f700b72ea5231cc8 -a ccfb4cd4e4aea80d14fcc2b649001f0b -l WHO -f {Application.dataPath}/../build/build_windows"
            //$"/c start {Application.dataPath}/builders/patchKit/patchkit-tools make-version -s ac1ae6ae296777d8f700b72ea5231cc8 -a ccfb4cd4e4aea80d14fcc2b649001f0b -l WHO -f {Application.dataPath}/../build/build_windows /c"
          };
          var filePath = $"{Application.dataPath}/builders/patchKit/upload.sh";
          File.WriteAllLines(filePath, contents);
          Process.Start(filePath);
          File.Delete(filePath);
        }
        static private Stack<FileInfo> DirExplore(string stSrcDirPath)
        {
            try
            {
                Stack<DirectoryInfo> stackDirs = new Stack<DirectoryInfo>();
                Stack<FileInfo> stackPaths = new Stack<FileInfo>();

                DirectoryInfo dd = new DirectoryInfo(Path.GetFullPath(stSrcDirPath));

                stackDirs.Push(dd);
                while (stackDirs.Count > 0)
                {
                    DirectoryInfo currentDir = (DirectoryInfo)stackDirs.Pop();


                    try
                    {
                        //Process .\files
                        foreach (FileInfo fileInfo in currentDir.GetFiles())
                        {
                            stackPaths.Push(fileInfo);
                        }

                        //Process Subdirectories
                        foreach (DirectoryInfo diNext in currentDir.GetDirectories())
                            stackDirs.Push(diNext);
                    }
                    catch (Exception)
                    {//Might be a system directory
                    }
                }
                return stackPaths;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static char[] _stSchon = new char[] { '-', '\\', '|', '/' };
    }
}
#endif
