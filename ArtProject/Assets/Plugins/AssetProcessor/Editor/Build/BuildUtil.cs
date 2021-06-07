
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
public class BuildUtil
{

    public static string GetBuildVersion(string workDirectory = null,string version = null)
    {
        List<string> result = new List<string>();

        if(string.IsNullOrEmpty(workDirectory))
            workDirectory = Application.dataPath.Remove(Application.dataPath.LastIndexOf("/Assets", System.StringComparison.Ordinal));

        string dateStr = string.Format("{0:yyyyMMddHHmmss}", System.DateTime.Now);

        if (string.IsNullOrEmpty(version))
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "svn";
            process.StartInfo.Arguments = string.Format("info -r BASE {0}", workDirectory);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler((sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data) && !string.IsNullOrEmpty(e.Data.Trim()))
                    result.Add(e.Data);
            });

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            //process.WaitForExit();
            process.Close();

            int count = 0;
            foreach (var svnInfo in result)
                Debug.LogFormat("XBuildUtility::GetSvnVersion() svn:   {0}:{1}", count++, svnInfo);

            if (result.Count >= 6)
            {
                string revisionVer = result[5];
                string[] sps = revisionVer.Split(':');
                if (sps.Length >= 2)
                    return sps[1].Trim() + "\r\n" + dateStr;
            }
            return dateStr + "\r\n" + dateStr;
        }
        else
        {
            return version + "\r\n" + dateStr;
        }


    }

    /// <summary>
    /// 项目路径转全路径
    /// </summary>
    /// <param name="projectPath"></param>
    /// <returns></returns>
    public static string ProjToFullPath(string projectPath)
    {
        return Path.Combine(Application.dataPath, projectPath.Substring(7)).Replace("\\", "/");
    }
    public static string PackagesToFullPath(string projectPath)
    {
        return Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length-6), projectPath).Replace("\\", "/");
    }

    public static string ProjToABName(string projectPath,int offset = 11,string headName = "")
    {
        
        return projectPath.Substring(offset).Replace("\\", "/") + headName+".asset";
    }

    public static string FileToABName(string fullpath,int offset = 11,string headName = "")
    {
        fullpath = fullpath.Split('.')[0];

        return ProjToABName(FullToPorjPath(fullpath), offset, headName);
    }


    public static string TripPackageDirectory(string folder)
    {
        string clearlyFolder = folder.Split('@')[0];
        
        clearlyFolder = clearlyFolder.Replace(".", "");
        clearlyFolder = clearlyFolder.Replace("-", "");
        clearlyFolder = clearlyFolder.Replace("\\", "/");
        return clearlyFolder.Substring(Application.dataPath.Length - 6);
    }


    public static string TripPackageFile(string file)
    {
        string cfile = file.Replace("\\", "/");
        string[] strr = cfile.Split('@');
        string clearlyFile = strr[0] + strr[1].Substring(strr[1].IndexOf('/'));

        return clearlyFile;
    }

    /// <summary>
    /// 全路径转项目路径
    /// </summary>
    /// <param name="fullPath"></param>
    /// <returns></returns>
    public static string FullToPorjPath(string fullPath)
    {
        return fullPath.Substring(Application.dataPath.Length - 6).Replace("\\", "/");
    }

    //清除无效的文件
    public static void ClearUnnecessary(string path, AssetBundleBuild[] builds)
    {
        HashSet<string> exist = new HashSet<string>();
        foreach (var item in builds)
            exist.Add(item.assetBundleName);

        string[] files = Directory.GetFiles(path, "*");
        string dirName = Path.GetFileName(path);
        foreach (var item in files)
        {
            string fileExt = Path.GetExtension(item);
            string fileName = Path.GetFileName(item);
            if (fileExt == ".txt" || fileExt == ".manifest" || fileName == dirName)
                continue;

            if (!exist.Contains(fileName))
                File.Delete(item);
        }
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    public static void Recursive(string path, List<string> files)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            //paths.Add(dir.Replace('\\', '/'));
            Recursive(dir, files);
        }
    }


    //文件过滤
    static List<string> s_ExtNameFilters = new List<string>() { ".meta", ".cs", ".cginc", ".exr" ,".md"};//".shadergraph"
    static List<string> s_FileNameFilters = new List<string>() { "LightingData.asset", "NavMesh.asset" };
    public static bool Filter(string file,List<string> ex_extName = null)
    {
        if (Directory.Exists(file)) return false;
        string extName = Path.GetExtension(file);
        string fileName = Path.GetFileName(file);

        if (ex_extName != null && ex_extName.Contains(extName)) return true;

        return s_ExtNameFilters.Contains(extName) || s_FileNameFilters.Contains(fileName);
    }

    /// <summary>
    /// 变量全部子目录，一个目录一个包
    /// </summary>
    /// <param name="list"></param>
    /// <param name="folder"></param>
    public static void RecursiveCollectBundles(List<AssetBundleBuild> list, string folder,string headName="", List<string> ext_filters = null)
    {

        //打包本层资源
        CollectBundles(list, folder, true, false,11,headName, ext_filters);

        string fullPath = BuildUtil.ProjToFullPath(folder);

        string[] dirs = Directory.GetDirectories(fullPath);
        foreach (string dir in dirs)
        {
            RecursiveCollectBundles(list, BuildUtil.FullToPorjPath(dir), headName, ext_filters);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="folder"></param>
    /// <param name="along">打成一个AssetBundle</param>
    /// <param name="iteration">递归进入子文件夹</param>
    public static void CollectBundles(List<AssetBundleBuild> list, string folder, bool along = false,bool iteration = true,int offset = 11,string headName = "",List<string> ext_filters = null)
    {

        //输入一个Art下的文件夹名字或得他的构建
        //构建方式 
        // 1 每个文件夹一个包（不包括子文件夹）
        // 2 输入路径一个全包
        string fullPath = BuildUtil.ProjToFullPath(folder);
        if (string.IsNullOrEmpty(fullPath) || !Directory.Exists(fullPath)) return;

        string[] files = Directory.GetFiles(fullPath, "*", iteration ? SearchOption.AllDirectories:SearchOption.TopDirectoryOnly);//获得全部子文件
        if (files.Length < 1) return;

        if (along)
        {
            //把全部文件都打成一个AB包
            AssetBundleBuild ab = new AssetBundleBuild();
            ab.assetBundleName = BuildUtil.ProjToABName(folder, offset, headName);

            string[] addressableNames = new string[0];
            string[] assetNames = new string[0];
            foreach (var file in files)
            {
                if (Filter(file, ext_filters)) continue;
                ArrayUtility.Add<string>(ref addressableNames, Path.GetFileName(file));///加载资源时，使用的名字
                ArrayUtility.Add<string>(ref assetNames, BuildUtil.FullToPorjPath(file));//项目中的路径
            }

            ab.addressableNames = addressableNames;
            ab.assetNames = assetNames;

            if(assetNames.Length > 0) list.Add(ab);

        }
        else
        {
            //一个文件一个包
            foreach (var file in files)
            {
                if (Filter(file, ext_filters)) continue;

                AssetBundleBuild ab = new AssetBundleBuild();
                ab.assetBundleName = BuildUtil.FileToABName(file, offset, headName);
                ab.addressableNames = new string[1] { Path.GetFileName(file) };
                ab.assetNames = new string[1] { BuildUtil.FullToPorjPath(file) };

                list.Add(ab);

            }

        }

    }

    public static bool BuildAssetBundles(string outPath, AssetBundleBuild[] builds, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform,string version)
    {
        //版本文件 格式为  本地svn库版本号 编辑日期
        string versionPath = Path.Combine(outPath, "version.txt");
        if(File.Exists(versionPath))File.Delete(versionPath);

        //文件映射关系
        string fileORMPath = Path.Combine(outPath, "fileorm.txt");
        if (File.Exists(fileORMPath)) File.Delete(fileORMPath);

        //统一把文件名换成小写

        for (int i = 0; i < builds.Length; i++)
        {
            builds[i].assetBundleName = builds[i].assetBundleName.ToLower();
            //builds[i].addressableNames = builds[i].addressableNames.ToLower();
            for (int j = 0; j < builds[i].addressableNames.Length; j++)
                builds[i].addressableNames[j] = builds[i].addressableNames[j].ToLower();
        }

        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(outPath, builds, BuildDefinition.option, BuildDefinition.target);

        List<string> hashs = new List<string>();
        List<string> crcs = new List<string>();
        for (int i = 0; i < builds.Length; i++)
        {
            string hash = manifest.GetAssetBundleHash(builds[i].assetBundleName).ToString();
            uint crc = 0;
            string abpath = System.IO.Path.Combine(outPath, builds[i].assetBundleName).Replace("\\", "/");
            BuildPipeline.GetCRCForAssetBundle(abpath, out crc);
            crcs.Add(crc.ToString());
            hashs.Add(hash);
        }

       AssetsFileOrm.CreateFileOrm(fileORMPath, builds, builds, hashs, crcs);


        ClearUnnecessary(outPath, builds);

        File.WriteAllText(versionPath, version);

        return true;
    }

   
    public static void CombineManifest(ref string[] addressableNames, ref string[] assetNames, bool building = true)
    {
        
        BuildTarget target = BuildDefinition.target;

        string outPath = Path.Combine(BuildDefinition.OutBasePath, target.ToString());

        XAssetManifest manifest = ScriptableObject.CreateInstance<XAssetManifest>();
        manifest.ReBuild(outPath);


        string projectPath = BuildDefinition.s_XassetPath;
        AssetDatabase.CreateAsset(manifest, projectPath);
        if(building)
        {
            AssetBundleBuild[] abbs = new AssetBundleBuild[0];
            //minifest  文件
            AssetBundleBuild abbuild = new AssetBundleBuild();
            abbuild.assetBundleName = BuildDefinition.s_assetManifest;
            abbuild.addressableNames = new string[] { Path.GetFileName(projectPath) };
            abbuild.assetNames = new string[] { projectPath };


            ArrayUtility.Add<AssetBundleBuild>(ref abbs, abbuild);

            ////清单文件极限压缩
            BuildAssetBundleOptions options = BuildAssetBundleOptions.None | BuildAssetBundleOptions.ForceRebuildAssetBundle;
            BuildPipeline.BuildAssetBundles(outPath, abbs, options, target);

            AssetDatabase.DeleteAsset(projectPath);
            AssetDatabase.Refresh();

            //直接使用源文件kb 比 AssetBunld大
            //string outFilePath = Path.Combine(outPath, Path.GetFileName(projectPath));
            //FileUtil.CopyFileOrDirectory(BuildUtil.ProjToFullPath(projectPath), outFilePath);
        }
        else
        {
            ArrayUtility.Add<string>(ref addressableNames, Path.GetFileName(projectPath));
            ArrayUtility.Add<string>(ref assetNames, projectPath);
        }
        
    }

    public static string CombineFileList(string outPath)
    {
        //配置文件，记录了文件的属性及分包等... 
        string settingPath = Path.Combine(outPath, BuildDefinition.c_SettingFileName);
        XAssetsFiles settingFiles = null;
        if (File.Exists(settingPath))
            settingFiles = JsonUtility.FromJson<XAssetsFiles>(File.ReadAllText(settingPath));

        XAssetsFiles xAssetsFiles = new XAssetsFiles();
        xAssetsFiles.p_AllFiles = new List<XAssetsFiles.FileStruct>();

        string[] folders = Directory.GetDirectories(outPath);
        foreach (var folder in folders)
        {
            string folderName = Path.GetFileName(folder);

            //文件映射表
            string fileormPath = Path.Combine(folder, "fileorm.txt");
            Dictionary<string, string> fileNameToBundleName = null;
            Dictionary<string, List<string>> bunldeNameToAssetList = null;
            if (File.Exists(fileormPath))
            {
                AssetsFileOrm.FileOrm fileorm = AssetsFileOrm.FileOrm.Load(fileormPath);

                fileNameToBundleName = new Dictionary<string, string>();
                bunldeNameToAssetList = new Dictionary<string, List<string>>();
                foreach (var item in fileorm.p_AssetBundleList)
                    if (!fileNameToBundleName.ContainsKey(item.p_AssetBundleName))
                    {
                        fileNameToBundleName.Add(item.p_AssetBundleName, item.p_AssetBundleName);
                        List<string> fileList = new List<string>();
                        for (int i = 0; i < item.p_Assets.Count; i++)
                        {
                            fileList.Add(item.p_Assets[i].p_AssetPath);
                        }
                        bunldeNameToAssetList[item.p_AssetBundleName] = fileList;
                    }
            }


            string[] files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                string fileName = file.Replace(folder + "\\", "");
                                                                  
                string ext = Path.GetExtension(file);
                if (ext == ".txt" || ext == ".manifest" || fileName == folderName)
                {
                    continue;
                }

                XAssetsFiles.FileStruct fs = new XAssetsFiles.FileStruct();
                fs.path = Path.Combine(folderName, fileName).Replace("\\", "/");
                fs.md5 = LFileUtil.fileMD5(file);
                fs.size = LFileUtil.fileSize(file);

                //使用设置的包标识
                if (settingFiles != null)
                {
                    string bname = string.Empty;
                    fileName = fileName.Replace("\\", "/");
                    if (fileNameToBundleName != null && fileNameToBundleName.TryGetValue(fileName, out bname))
                    {
                        XAssetsFiles.FileStruct settingFs;
                        if (settingFiles.allFilesMap.TryGetValue(bname, out settingFs))
                        {
                            fs.options = settingFs.options;
                            fs.tag = settingFs.tag;
                            fs.priority = settingFs.priority;
                        }
                    }
                }
                else if (BuildDefinition.DefaultOptions.ContainsKey(fs.path))
                {
                    fs.options = BuildDefinition.DefaultOptions[fs.path];
                }

                xAssetsFiles.p_AllFiles.Add(fs);
            }
        }
        string filesPath = Path.Combine(outPath, BuildDefinition.c_FileListFileName);
        string jsonStr = JsonUtility.ToJson(xAssetsFiles, true);
        //使用LZ4 压缩算法，减小文件尺寸
        //var compressed = Convert.ToBase64String(LZ4Sharp.LZ4.Compress(Encoding.UTF8.GetBytes(jsonStr)));
        var compressed = LZ4Sharp.LZ4Util.CompressString(jsonStr);
        File.WriteAllText(filesPath, compressed);
        return filesPath;
    }


    public static string CombineVersionFile(string outPath)
    {
        XVersionFile version = new XVersionFile();
        string[] versions = Directory.GetFiles(outPath, BuildDefinition.c_VersionFileName, SearchOption.AllDirectories);
        foreach (var versionPath in versions)
        {
            string folderName = Path.GetFileName(Path.GetDirectoryName(versionPath));
            string[] lines = File.ReadAllLines(versionPath);
            if (lines.Length >= 2)
            {
                string svnVer = lines[0];
                string date = lines[1];

                if (folderName == "000")
                    version.p_LuaVersion = new XVersionFile.VersionStruct { svnVer = svnVer, buildDate = date };    
                else if (folderName == "001")
                    version.p_DevVersion = new XVersionFile.VersionStruct { svnVer = svnVer, buildDate = date };
                else if (folderName == "002")
                    version.p_ArtVersion = new XVersionFile.VersionStruct { svnVer = svnVer, buildDate = date };
            }
        }

        string filesPath = Path.Combine(outPath, BuildDefinition.c_VersionFileName);
        File.WriteAllText(filesPath, JsonUtility.ToJson(version, true));

        return filesPath;
    }



    static Dictionary<string, string> GetInputCommand(string[] args)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        string key = string.Empty;
        foreach (var item in args)
        {
            if (item.StartsWith("-"))
            {
                key = item.Trim();
            }
            else if (!string.IsNullOrEmpty(item) && !string.IsNullOrEmpty(key))
            {
                result.Add(key, item.Trim());
            }
        }
        return result;

    }
    public static void buildCommonline()
    {
        Dictionary<string, string> command = GetInputCommand(System.Environment.GetCommandLineArgs());

        BuildArt.Building();
        BuildDevelopment.Building();
        BuildLua.Building();
       // BuildUtil.BuildManifest();


        EditorApplication.Exit(1);
    }
}
