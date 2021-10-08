using System;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEngine;
using UnityEditor.Sprites;
using System.IO;
using System.Collections.Generic;
using LBDDK.iOS.Xcode;
using System.Linq;

namespace Game
{
    public class BuildPostProcess
    {
        /// <summary>
        /// 为Pro打包准备
        /// 1.删除无用文件夹
        /// 2.准备设置调取配置参数
        /// </summary>
        [MenuItem("Build/PRODUCT-Prepare")]
        public static void BuildGameIOSPRO()
        {
            BuildProcessGitReset();
            ChangePlayerSetting("PRODUCT");
        }
        [MenuItem("Build/DEVELOP-Prepare")]
        public static void BuildGameIOSDEV()
        {
            ChangePlayerSetting("DEVELOP");
        }
        [MenuItem("Build/TEST-Prepare")]
        public static void BuildGameIOSTEST()
        {
            ChangePlayerSetting("TEST");
        }

        [PostProcessBuild(101)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            //if (buildTarget != BuildTarget.iPhone) { // For Unity < 5
            if (buildTarget != BuildTarget.iOS)
            {
                Debug.LogWarning("Target is not iOS. AdColonyPostProcess will not run");
                return;
            }

            string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string target = proj.TargetGuidByName("Unity-iPhone");
            proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
            proj.AddLibraryToProject(target, "libsqlite3.tbd", false);
            proj.AddLibraryToProject(target, "libxml2.tbd", false);
#if PRODUCT
            proj.SetBuildProperty(target, "CODE_SIGN_IDENTITY", "iPhone Distribution: D-techno Co.,Ltd. (N39998WLUX)");
            proj.SetBuildProperty(target, "PROVISIONING_PROFILE", "be1f1b41-c3fe-4c21-baab-6290d603a4cb");
            var fileName = "osoparty.entitlements";
            var filePath = Path.Combine("Assets/Editor", fileName);
            File.Copy(filePath, Path.Combine(path, fileName));
            proj.AddFileToBuild(target, proj.AddFile(fileName, fileName, PBXSourceTree.Source));
            proj.AddBuildProperty(target, "CODE_SIGN_ENTITLEMENTS", fileName);
#else
            proj.SetBuildProperty(target, "CODE_SIGN_IDENTITY", "iPhone Distribution: MONSTAR LAB,INC.");
            proj.SetBuildProperty(target, "PROVISIONING_PROFILE", "91ceb346-8e0f-4bd3-aa63-7eb31e733593");
#endif
            File.WriteAllText(projPath, proj.WriteToString());

            //Xcodeプロジェクトへのパスを取得 buildPathはOnPostprocessBuildの引数
            string plistPath = Path.Combine(path, "Info.plist");

            //インスタンスを作成
            PlistDocument plist = new PlistDocument();

            //plistの読み込み 
            plist.ReadFromFile(plistPath);
            plist.root.SetString("NSCalendarsUsageDescription", "画像の保存に使用します。");
            plist.root.SetString("NSCameraUsageDescription", "画像の保存に使用します。");
            plist.root.SetString("NSLocationWhenInUseUsageDescription", "画像の保存に使用します。");
            plist.root.SetString("NSPhotoLibraryUsageDescription", "画像の保存に使用します。");
            plist.WriteToFile(plistPath);
        }


        //        [PostProcessBuildAttribute(0)]
        //        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        //        {
        //            #if UNITY_ANDROID || UNITY_IOS
        //            Debug.Log("BuildPostProcess Start!");
        //            DelCommonFiles();
        //            DelPlatformFiles();
        //            #else
        //            Debug.Log("BuildPostProcess Skip!");
        //            #endif
        ////            Debug.Log("Texture Repack Start!");
        ////            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        ////            Debug.Log("Texture Repack End!");
        //            Debug.Log("BuildPostProcess End!");
        //        }
        /// <summary>
        /// Raises the postprocess scene event.
        /// Only Go in Building and called by build every scene
        /// </summary>

        private static string[] commonFilePath =
            {
                "/Resources/assets",
                "/Resources/id",
                "/Resources/version",
                "/Images/frame",
                "/Gacha",
                "/StreamingAssets/csv"
            };
        /// <summary>
        /// Define PlatformFile Which need deleted
        /// if iOS Env then delete android files
        /// if Android Env then delete iOS files
        /// </summary>
#if UNITY_ANDROID
        private static string[] platformFilePath =
            {
                "/StreamingAssets/ios"
            };

        private static string[] platformSpecialFilePath =
        {
            "/StreamingAssets/android"
        };

        public static void ReadyForPlayerSetting(BuildSettingJson buildSetting)
        {
            if (buildSetting == null)
            {
                EditorUtility.DisplayDialog("Build Info", "Config File Error!", "OK");
            }
            else {
                PlayerSettings.bundleVersion = buildSetting.versionName;
                PlayerSettings.Android.bundleVersionCode = int.Parse(buildSetting.versionCode);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, buildSetting.ScriptDefineSymbols);
                UpdateVersionForSystemConfigFile(buildSetting.versionName);
            }
        }
#elif UNITY_IOS
        private static string[] platformFilePath =
        {
            "/StreamingAssets/android"
        };
        private static string[] platformSpecialFilePath =
        {
            "/StreamingAssets/ios"
        };
        public static void ReadyForPlayerSetting(BuildSettingJson buildSetting)
        {
            if (buildSetting == null)
            {
                EditorUtility.DisplayDialog("Build Info", "Config File Error!", "OK");
            }
            else {
                PlayerSettings.bundleVersion = buildSetting.versionName;
                PlayerSettings.iOS.buildNumber = buildSetting.versionCode;
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, buildSetting.ScriptDefineSymbols);
                UpdateVersionForSystemConfigFile(buildSetting.versionName);
            }
        }
#endif
        private static void UpdateVersionForSystemConfigFile(String versionName)
        {
            String filePath = Application.dataPath + "/Scripts/game/frame/common/constant/SystemConstant.cs";
            String[] lines = File.ReadAllLines(filePath);
            lines[8] = "    public const string CLIENT_VERSION = \"" + versionName + "\";";
            File.WriteAllLines(filePath, lines);
        }
        /// <summary>
        /// 删除平台共通文件
        /// </summary>
        private static void DelCommonFiles()
        {
            DelFiles(commonFilePath);
        }
        /// <summary>
        /// 删除平台特殊文件
        /// </summary>
        private static void DelPlatformFiles()
        {
            /// <summary>
            /// 普通文件和文件夹
            /// </summary>
            /// <param name="files">Files.</param>
            DelFiles(platformFilePath);
            /// <summary>
            /// 删除特殊指定类型文件和文件夹
            /// </summary>
            /// <param name="files">Files.</param>
            DelFilesWithParam(platformSpecialFilePath, "_local");
        }
        /// <summary>
        /// 删除文件和文件夹
        /// </summary>
        /// <param name="files">文件相对路径</param>
        private static void DelFiles(params string[] files)
        {
            if (files == null || files.Length == 0)
            {
                return;
            }
            string _appPath = Application.dataPath;

            foreach (var item in files)
            {
                bool _result = FileUtil.DeleteFileOrDirectory(_appPath + item);
                if (_result)
                {
                    Debug.Log(" Success Delete File/Directory !!! [" + _appPath + item + "]");
                }
                else {
                    Debug.Log("Delete File/Directory Skip!!! [" + _appPath + item + "]");
                }
            }
        }

        /// <summary>
        /// 删除文件和文件夹
        /// </summary>
        /// <param name="files">文件路径相对</param>
        /// <param name="skipTag">跳过对应tag文件名 index方式</param>
        /// <param name="delDir">如果文件夹<c>true</c> 则删除</param>
        private static void DelFilesWithParam(string[] files, string skipTag, bool delDir = false)
        {
            if (files == null || files.Length == 0)
            {
                return;
            }
            string _appPath = Application.dataPath;

            foreach (var item in files)
            {
                //过滤tag文件
                if (item.IndexOf(skipTag) > -1)
                {
                    continue;
                }
                FileAttributes attr = File.GetAttributes(_appPath + item);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    if (!delDir)
                    {
                        List<String> currentFiles = new List<string>();
                        var tmpFiles = Directory.GetFiles(_appPath + item);
                        foreach (var tmpFile in tmpFiles)
                        {
                            currentFiles.Add(tmpFile.Replace(Application.dataPath, ""));
                        }
                        DelFilesWithParam(currentFiles.ToArray(), skipTag, delDir);
                        continue;
                    }
                }
                //是否删除文件夹

                //删除文件或者文件夹
                bool _result = FileUtil.DeleteFileOrDirectory(_appPath + item);
                if (_result)
                {
                    Debug.Log(" Success Delete File/Directory !!! [" + _appPath + item + "]");
                }
                else {
                    Debug.Log("Delete File/Directory Skip!!! [" + _appPath + item + "]");
                }
            }
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

        #region BuildProcess
        public static void BuildProcessGitReset()
        {
            string gitCommand = "git";
            string gitReset = "reset HEAD --hard";
            string gitDelUnTracked = "clean -f";
            System.Diagnostics.Process.Start(gitCommand, gitReset);
            System.Diagnostics.Process.Start(gitCommand, gitDelUnTracked);
        }
        public static void BuildProcessDelFile()
        {
            DelCommonFiles();
            DelPlatformFiles();
            AssetDatabase.Refresh();
        }

        public static void ChangePlayerSetting(String ScriptDefineSymbols)
        {
            BuildWindow buildWindow = (BuildWindow)EditorWindow.GetWindow(typeof(BuildWindow));
            buildWindow.ScriptDefineSymbols = ScriptDefineSymbols;
        }
        #endregion
    }

    public class BuildSettingJson
    {
        public String versionName;
        public String versionCode;
        public String ScriptDefineSymbols;
        public BuildSettingJson() { }
        public BuildSettingJson(String versionName, String versionCode, String SDS)
        {
            this.versionName = versionName;
            this.versionCode = versionCode;
            this.ScriptDefineSymbols = SDS;
        }
    }

    public class BuildWindow : EditorWindow
    {
        private String versionName;
        private String versionCode;
        private String process = "OK";
        public String ScriptDefineSymbols;


        void OnGUI()
        {
            versionName = EditorGUILayout.TextField("Version Name", versionName);
            versionCode = EditorGUILayout.TextField("Version Code", versionCode);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.enabled = process.Equals("OK") && !String.IsNullOrEmpty(versionName) && !String.IsNullOrEmpty(versionCode);
            if (GUILayout.Button(process))
            {
                process = "Change Player Setting";
                BuildPostProcess.ReadyForPlayerSetting(new BuildSettingJson(versionName, versionCode, ScriptDefineSymbols));
                process = "Delete Files";
                BuildPostProcess.BuildProcessDelFile();
                process = "OK";
#if UNITY_IOS
                if (EditorUtility.DisplayDialog("Build Info", "Config All Ready For " + ScriptDefineSymbols, "OK"))
                {
                    GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
                    Close();
                }
#elif UNITY_ANDROID
                if (EditorUtility.DisplayDialog("Build Info", "Config All Ready For " + ScriptDefineSymbols + "\nPlease Check Keystore Setting!!!", "Go Check"))
                {
                    GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
                    Close();
                }
#endif
            }
            GUI.enabled = true;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}

