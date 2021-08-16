#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
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
        }

        [MenuItem("Jobs/Build/Sidgin Test")]
        public static void StartSIDGINWindows()
        {
            // Get filename.
            string path = "build";
            var filename = path.Split('/');
            //CreateSidginUpdate(BuildTarget.StandaloneWindows, filename[filename.Length - 1], path + "/");
        }

        [MenuItem("Jobs/Build/ MacOS")]
        public static void StartAll()
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



            Debug.Log("====== BuildPlayer: " + buildTarget.ToString() + " at " + path + filename);
            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);

            string buildPath = path + filename + modifier + "/";
            //Debug.Log("buildpath: " + buildPath);
            string playerPath = buildPath + filename + modifier + fileExtension;
            //Debug.Log("playerpath: " + playerPath);
            BuildPipeline.BuildPlayer(GetScenePaths(buildTarget), playerPath, buildTarget,
                buildTarget == BuildTarget.StandaloneWindows ? BuildOptions.ShowBuiltPlayer : BuildOptions.None);

            string fullDataPath = buildPath + filename + modifier + dataPath;
            Debug.Log("fullDataPath: " + fullDataPath);
            //CopyFromProjectAssets( fullDataPath, "languages");

            //ICSharpCode.SharpZipLib.Zip.FastZip zip = new ICSharpCode.SharpZipLib.Zip.FastZip();
            //zip.CreateEmptyDirectories = true;
            //zip.CreateZip($"{fullDataPath}\\build.zip", fullDataPath, true,"");
            CreateZip("build/build.zip",fullDataPath);
        }

        static string[] GetScenePaths(BuildTarget buildTarget)
        {

            string[] scenes = new string[EditorBuildSettings.scenes.Length];
            int j = scenes.Length;
            for (int i = 0; i < j; i++)
            {
                if (buildTarget != BuildTarget.StandaloneWindows || buildTarget != BuildTarget.StandaloneWindows64)
                {
                    //skip sidgin scene if it's not windows
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

        // private static void CreateSidginUpdate(BuildTarget buildTarget, string s, string path)
        // {
        //
        //     var config = "configString";//path to settings file?
        //
        //     var versionToUpload = new VersionToUpload();
        //     versionToUpload.version = "1.0.0.0";
        //     versionToUpload.fullName = "ApolloDebug";
        //     versionToUpload.fullFilePath = "";
        //     versionToUpload.patchName = "";
        //     versionToUpload.patchFilePath = "";
        //     versionToUpload.versionName = "";
        //     versionToUpload.versionFilePath = "";
        //     versionToUpload.versionListFilePath = "";
        //
        //     var cancelSource = new CancellationTokenSource();
        //     var token = cancelSource.Token;
        //     //var task = new Task<VersionToUpload>()
        //     //configure task here
        //
        //     var settings = new ConfigurationReader(config).LoadData();
        //     //configure settings here
        //     var updater =  new VersionBuilder(settings);
        //
        //     updater.BuildPatch(false, token); //TODO: Find out what isInitial does
        //     var publisher = new VersionUploader(settings);
        //     publisher.Upload(versionToUpload);
        // }
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
        public static void CreateZip(string stZipPath, string stDirToZip)
        {
            try
            {
                //Sanitize inputs
                stDirToZip = Path.GetFullPath(stDirToZip);
                stZipPath = Path.GetFullPath(stZipPath);

                Console.WriteLine("Zip directory " + stDirToZip);

                //Recursively parse the directory to zip
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
                    //
                    // set Size and the crc, because the information
                    // about the size and crc should be stored in the header
                    // if it is not set it is automatically written in the footer.
                    // (in this case size == crc == -1 in the header)
                    // Some ZIP programs have problems with zip files that don't store
                    // the size and crc in the header.
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
                zipOutput = null;
            }
            catch (Exception)
            {
                throw;
            }
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
