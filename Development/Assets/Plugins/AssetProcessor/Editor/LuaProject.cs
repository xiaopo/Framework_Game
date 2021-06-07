using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

//[EditorWindowTitle(title = "LuaProject", icon = "Project")]
public class LuaProject : EditorWindow
{
    const string PATHKEY = "luaPathKey";
    static string s_RootPath;

    public static string LuaRootPath
    {
        get
        {
            if (string.IsNullOrEmpty(s_RootPath))
                s_RootPath = Path.Combine(Application.dataPath, "../Lua_Scripts/");
            return LuaProject.s_RootPath;
        }
    }
    public static string LuaGamePath { get { return Path.Combine(s_RootPath, "game"); } }

    static string s_Modules = @"game\modules\";

    enum CreateType
    {
        None,
        Module,
        View,
        Panel,
        Window,
        ListItemRenderer,
        ContentRenderer,
    }

    [SerializeField]
    private List<int> m_SelectedIds = new List<int>();

    [SerializeField]
    private LuaProjectTreeView m_TreeView;
    private TreeViewState m_TreeState;
    private SearchField m_SearchField;
    private FileSystemWatcher m_FileSystemWatcher;
    private bool m_NeedReload = false;
    [SerializeField]
    private CreateType m_CreateType = CreateType.None;
    private string m_InputContent;
    private string m_InputLabel;
    private string m_CreateTip;
    private bool m_ValidationLua;

    void OnEnable()
    {
        ValidationLuaDirectory();
        if (!m_ValidationLua) return;
        Init();
    }

    void Init()
    {
        this.m_ValidationLua = true;
        this.m_TreeState = new TreeViewState();
        this.m_TreeView = new LuaProjectTreeView(this.m_TreeState, LuaRootPath);
        this.AddEvents();
        this.Reload();
        m_SearchField = new SearchField();
        m_SearchField.downOrUpArrowKeyPressed += m_TreeView.SetFocusAndEnsureSelectedItem;

        if (!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            //m_FileSystemWatcher = new FileSystemWatcher(LuaRootPath);
            //m_FileSystemWatcher.IncludeSubdirectories = true;
            //m_FileSystemWatcher.Created += OnWatcherChange;
            //m_FileSystemWatcher.Deleted += OnWatcherChange;
            //m_FileSystemWatcher.Renamed += OnWatcherChange;
            //m_FileSystemWatcher.EnableRaisingEvents = true;
        }
    }

    void ValidationLuaDirectory()
    {
        if (!Directory.Exists(LuaRootPath))
            s_RootPath = EditorPrefs.GetString(PATHKEY);

        if (string.IsNullOrEmpty(s_RootPath) || !Directory.Exists(s_RootPath))
            return;

        m_ValidationLua = true;
    }

    void SelectLuaDirectory()
    {
        string selectDirectory = EditorUtility.OpenFolderPanel("Select Lua Folder", "", "Lua");
        if (!string.IsNullOrEmpty(selectDirectory))
        {
            s_RootPath = selectDirectory;
            EditorPrefs.SetString(PATHKEY, selectDirectory);
            Init();
        }
    }

    void Reload()
    {
        this.m_TreeView.Reload();
        if (m_SelectedIds.Count == 0)
        {
            m_SelectedIds.Add(1);
            m_SelectedIds.Add(2);
            m_SelectedIds.Add(3);
        }
        this.m_TreeView.SetExpanded(m_SelectedIds);
    }

    void AddEvents()
    {
        this.m_TreeView.onItemContextClick += OnItemContextClick;
        this.m_TreeView.onItemDoubleClick += OnItemDoubleClick;
    }

    void OnItemDoubleClick(LuaProjectTreeItem item)
    {
        if (!item.IsFolder())
        {
            EditorUtility.OpenWithDefaultApp(item.fullPath);
        }
        else
        {
            this.m_TreeView.SetExpanded(item.id, !this.m_TreeView.IsExpanded(item.id));
        }
    }


    void OnItemContextClick(LuaProjectTreeItem item)
    {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("Create/Module"), false, () => { CreateModule(item); });

        string relativePath = item.fullPath.Substring(Mathf.Min(s_RootPath.Length, item.fullPath.Length));

        if (!string.IsNullOrEmpty(relativePath) && relativePath.StartsWith(s_Modules))
        {
            string[] sp = relativePath.Substring(s_Modules.Length).Split('\\');
            menu.AddItem(new GUIContent("Create/View"), false, () => { CreateView(item, sp[0]); });
            menu.AddItem(new GUIContent("Create/Panel"), false, () => { CreatePanel(item, sp[0]); });
            menu.AddItem(new GUIContent("Create/Window"), false, () => { CreateWindow(item, sp[0]); });
            menu.AddSeparator("Create/");
            menu.AddItem(new GUIContent("Create/ListItemRenderer"), false, () => { CreateListItemRenderer(item, sp[0]); });
            menu.AddItem(new GUIContent("Create/ContentRenderer"), false, () => { CreateContentRenderer(item, sp[0]); });
        }
        else
        {
            menu.AddDisabledItem(new GUIContent("Create/View"));
            menu.AddDisabledItem(new GUIContent("Create/Panel"));
            menu.AddDisabledItem(new GUIContent("Create/Window"));
            menu.AddSeparator("Create/");
            menu.AddDisabledItem(new GUIContent("Create/ListItemRenderer"));
            menu.AddDisabledItem(new GUIContent("Create/ContentRenderer"));
        }

        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Show in Explorer"), false, () => { ShowinExplorer(item); });
        menu.AddItem(new GUIContent("Refresh"), false, () => { Refresh(item); });

        if (relativePath.StartsWith(s_Modules))
            menu.AddItem(new GUIContent("Delete"), false, () => { Delete(item); });
        else
            menu.AddDisabledItem(new GUIContent("Delete"));
        menu.AddSeparator("");

        if (!item.IsFile())
            menu.AddDisabledItem(new GUIContent("Duplicate Package Name"));
        else
        {
            menu.AddItem(new GUIContent("Duplicate Package Name"), false, () => { DuplicatePackageName(item); });
            menu.AddItem(new GUIContent("Duplicate Package NameEx"), false, () => { DuplicatePackageNameEx(item); });
        }



        menu.AddItem(new GUIContent("SVN/Update"), false, () => { OnSvnUpdate(item); });
        menu.AddItem(new GUIContent("SVN/Commit"), false, () => { OnSvnCommit(item); });
        menu.AddItem(new GUIContent("SVN/Revert"), false, () => { OnSvnRevert(item); });
        menu.AddItem(new GUIContent("SVN/Log"), false, () => { OnSvnLog(item); });
        menu.ShowAsContext();
    }

    string GetProjePath(string path)
    {
        return path.Substring(Application.dataPath.Length - 6);
    }

    private void OnSvnLog(LuaProjectTreeItem item)
    {
        string path = GetProjePath(item.fullPath);
        string arg = "/command:log /closeonend:0 /path:\"";
        arg += path;
        arg += "\"";
        SVNMenu.SvnCommandRun(arg);
    }

    private void OnSvnRevert(LuaProjectTreeItem item)
    {
        string path = GetProjePath(item.fullPath);
        SVNMenu.RevertAtPaths(new List<string>() { path });
    }

    private void OnSvnCommit(LuaProjectTreeItem item)
    {
        string path = GetProjePath(item.fullPath);
        SVNMenu.CommitAtPaths(new List<string>() { path });
    }

    private void OnSvnUpdate(LuaProjectTreeItem item)
    {
        string path = GetProjePath(item.fullPath);
        SVNMenu.UpdateAtPaths(new List<string>() { path });
    }

    private void CreateListItemRenderer(LuaProjectTreeItem obj, string moduleName)
    {
        m_CreateTip = "创建列表渲染器";
        m_InputLabel = moduleName;
        m_CreateType = CreateType.ListItemRenderer;
    }

    private void CreateContentRenderer(LuaProjectTreeItem obj, string moduleName)
    {
        m_CreateTip = "创建内容渲染器";
        m_InputLabel = moduleName;
        m_CreateType = CreateType.ContentRenderer;
    }

    private void CreateWindow(LuaProjectTreeItem obj, string moduleName)
    {
        m_CreateTip = "创建窗口";
        m_InputLabel = moduleName;
        m_CreateType = CreateType.Window;
    }

    private void CreatePanel(LuaProjectTreeItem obj, string moduleName)
    {
        m_CreateTip = "创建面板";
        m_InputLabel = moduleName;
        m_CreateType = CreateType.Panel;
    }

    private void CreateView(LuaProjectTreeItem obj, string moduleName)
    {
        m_CreateTip = "创建视图";
        m_InputLabel = moduleName;
        m_CreateType = CreateType.View;
    }

    private void CreateModule(LuaProjectTreeItem obj)
    {
        m_CreateTip = "创建模块";
        this.m_InputLabel = "Module:";
        this.m_CreateType = CreateType.Module;
    }

    private void Delete(LuaProjectTreeItem item)
    {
        if (!EditorUtility.DisplayDialog("删除操作", item.fullPath.Substring(s_RootPath.Length), "确定", "取消"))
            return;

        if (item.IsFolder())
        {
            Directory.Delete(item.fullPath, true);
            RefreshModuleCommandLine();
            this.Reload();
        }
        else if (item.IsFile())
        {
            File.Delete(item.fullPath);
            RefreshModuleCommandLine();
            this.Reload();
        }
    }

    void Refresh(LuaProjectTreeItem item)
    {
        RefreshModuleCommandLine();
        SaveSelectIds();
        Reload();
    }

    void ShowinExplorer(LuaProjectTreeItem obj)
    {
        EditorUtility.OpenWithDefaultApp(Path.GetDirectoryName(obj.fullPath));
    }

    void DuplicatePackageName(LuaProjectTreeItem item)
    {
        string packageName = item.fullPath.Substring(s_RootPath.Length);
        packageName = packageName.Replace('\\', '.').Replace(".lua", "");
        string str = string.Format("\"{0}\"", packageName);

        TextEditor textEditor = new TextEditor();
        textEditor.text = str;
        textEditor.OnFocus();
        textEditor.Copy();
    }

    void DuplicatePackageNameEx(LuaProjectTreeItem item)
    {
        string packageName = item.fullPath.Substring(s_RootPath.Length);
        packageName = packageName.Replace('\\', '.').Replace(".lua", "");
        string[] stringArray = packageName.Split('.');
        string luaName = stringArray[stringArray.Length - 1];

        string str = string.Format("local {0} = require(\"{1}\")", luaName, packageName);

        TextEditor textEditor = new TextEditor();
        textEditor.text = str;
        textEditor.OnFocus();
        textEditor.Copy();
    }

    void SaveSelectIds()
    {
        m_SelectedIds.Clear();
        foreach (var item in this.m_TreeView.GetExpanded())
            m_SelectedIds.Add(item);
    }

    void OnDisable()
    {
        if (!m_ValidationLua) return;

        if (m_FileSystemWatcher != null)
        {
            m_FileSystemWatcher.Created -= OnWatcherChange;
            m_FileSystemWatcher.Deleted -= OnWatcherChange;
            m_FileSystemWatcher.Renamed -= OnWatcherChange;
            m_FileSystemWatcher.Dispose();
        }

        SaveSelectIds();
    }

    void OnWatcherChange(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType == WatcherChangeTypes.Created ||
            e.ChangeType == WatcherChangeTypes.Deleted ||
            e.ChangeType == WatcherChangeTypes.Renamed)
        {
            m_NeedReload = true;
        }
    }

    void Update()
    {
        if (this.m_NeedReload)
        {
            if (this.m_TreeView != null)
                this.m_TreeView.Reload();
            this.m_NeedReload = false;
        }
    }

    void OnGUI()
    {
        if (m_ValidationLua)
            DoTreeView();
        else
            DoDirectory();


    }

    void DoDirectory()
    {
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Please Lua Directory", MessageType.Error);
        Event evt = Event.current;
        if (evt.isMouse && evt.button == 0 && evt.type == EventType.MouseUp && new Rect(0, 0, position.width, position.height).Contains(evt.mousePosition))
            SelectLuaDirectory();
    }

    void DoTreeView()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        m_TreeView.searchString = m_SearchField.OnToolbarGUI(m_TreeView.searchString);

        GUILayout.EndHorizontal();

        if (m_CreateType != CreateType.None)
            DoCreateModule();

        Rect rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
        this.m_TreeView.OnGUI(rect);
    }


    void DoCreateModule()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(this.m_InputLabel, EditorStyles.miniLabel);
        //m_InputContent = EditorGUILayout.TextField(m_InputContent);
        //EditorGUI.DelayedTextField()
        EditorGUI.BeginChangeCheck();
        m_InputContent = EditorGUILayout.DelayedTextField(m_InputContent);
        if (EditorGUI.EndChangeCheck())
        {
            if (this.m_CreateType == CreateType.Module)
            {
                CreateModuleCommandLine(m_InputContent);
            }
            else if (this.m_CreateType == CreateType.View)
            {
                CreateViewCommandLine(m_InputLabel, m_InputContent);
            }
            else if (this.m_CreateType == CreateType.Panel)
            {
                CreatePanelCommandLine(m_InputLabel, m_InputContent);
            }
            else if (this.m_CreateType == CreateType.Window)
            {
                CreateWindowCommandLine(m_InputLabel, m_InputContent);
            }
            else if (this.m_CreateType == CreateType.ListItemRenderer)
            {
                CreateListItemRendererCommandLine(m_InputLabel, m_InputContent);
            }
            else if (this.m_CreateType == CreateType.ContentRenderer)
            {
                CreateContentRendererCommandLine(m_InputLabel, m_InputContent);
            }

            this.m_CreateType = CreateType.None;
            m_InputContent = string.Empty;

            Refresh(null);

            this.Reload();
  
        }


        EditorGUILayout.LabelField(this.m_CreateTip, GUILayout.MaxWidth(60f));

        //GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

    }


    static string GetGenLuaTemplatePath()
    {
        return Path.Combine(s_RootPath, "LuaTemplateGen/LuaTemplateGen.exe");
    }

    static void CreateModuleCommandLine(string moduleName)
    {
        ExecuteCommandLine(string.Format("{0} createModule {1}", s_RootPath, moduleName));
    }

    static void CreateViewCommandLine(string moduleName, string viewName)
    {
        string one = new string(new char[] { viewName[0] }).ToUpper();
        viewName = viewName.Remove(0, 1).Insert(0, one);
        ExecuteCommandLine(string.Format("{0} createView {1} {2}", s_RootPath, moduleName, viewName));
    }

    static void CreatePanelCommandLine(string moduleName, string panelName)
    {
        string one = new string(new char[] { panelName[0] }).ToUpper();
        panelName = panelName.Remove(0, 1).Insert(0, one);
        ExecuteCommandLine(string.Format("{0} createPanel {1} {2}", s_RootPath, moduleName, panelName));
    }

    static void CreateListItemRendererCommandLine(string moduleName, string panelName)
    {
        string one = new string(new char[] { panelName[0] }).ToUpper();
        panelName = panelName.Remove(0, 1).Insert(0, one);
        ExecuteCommandLine(string.Format("{0} createItemRenderer {1} {2}", s_RootPath, moduleName, panelName));
    }

    static void CreateContentRendererCommandLine(string moduleName, string panelName)
    {
        string one = new string(new char[] { panelName[0] }).ToUpper();
        panelName = panelName.Remove(0, 1).Insert(0, one);
        ExecuteCommandLine(string.Format("{0} createContentRenderer {1} {2}", s_RootPath, moduleName, panelName));
    }

    private void CreateWindowCommandLine(string moduleName, string winName)
    {
        string one = new string(new char[] { winName[0] }).ToUpper();
        winName = winName.Remove(0, 1).Insert(0, one);
        ExecuteCommandLine(string.Format("{0} createWindow {1} {2}", s_RootPath, moduleName, winName));
    }

    static void RefreshModuleCommandLine()
    {
        ExecuteCommandLine(string.Format("{0} updateDefine", s_RootPath));
    }

    static void ExecuteCommandLine(string command)
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.FileName = GetGenLuaTemplatePath();
        process.StartInfo.Arguments = command;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();

    }

    [MenuItem("Window/LuaProject")]
    static void ShowWindow()
    {
        // Get existing open window or if none, make a new one:
        var window = GetWindow<LuaProject>();
        window.titleContent = new GUIContent("LuaProject", EditorGUIUtility.FindTexture("Project"));
        window.Show();
    }


    class LuaProjectTreeItem : TreeViewItem
    {
        private string m_FullPath;
        public string fullPath { get { return m_FullPath; } }
        private string m_ReadMeContent;
        public LuaProjectTreeItem(int id, int depth, string displayName, string filePath)
            : base(id, depth, displayName)
        {
            this.m_FullPath = filePath;
        }

        public bool IsFolder()
        {
            return Directory.Exists(fullPath);
        }

        public bool IsFile()
        {
            return File.Exists(fullPath);
        }

        public string GetReadMeInfo()
        {
            if (string.IsNullOrEmpty(this.m_ReadMeContent) && IsFolder())
            {
                string readMePath = Path.Combine(fullPath, "readme.txt");
                if (File.Exists(readMePath))
                    this.m_ReadMeContent = File.ReadAllText(readMePath);
            }

            return this.m_ReadMeContent;
        }
    }

    class LuaProjectTreeView : TreeView
    {

        static Texture2D[] s_TestIcons =
        {
           EditorGUIUtility.FindTexture ("Folder Icon"),
           (Texture2D)AssetDatabase.GetCachedIcon(Path.Combine(Path.GetDirectoryName(EAssetManifest.s_AssetManifestPath), "Editor/temp.lua")),
        };

        private string m_RootPath;
        public System.Action<LuaProjectTreeItem> onItemContextClick;
        public System.Action<LuaProjectTreeItem> onItemDoubleClick;
        public LuaProjectTreeView(TreeViewState state, string rootPath)
            : base(state)
        {

            this.rowHeight = 18;
            this.showBorder = true;
            this.showAlternatingRowBackgrounds = true;
            this.m_RootPath = rootPath;
        }

        protected override TreeViewItem BuildRoot()
        {
            LuaProjectTreeItem root = new LuaProjectTreeItem(0, -1, "Root", "");
            LuaProjectTreeItem luaRoot = new LuaProjectTreeItem(1, 0, "Lua", this.m_RootPath);
            luaRoot.icon = s_TestIcons[0];
            root.AddChild(luaRoot);
            int count = 2;
            this.IterationTree(luaRoot, ref count);
            return root;
        }


        void IterationTree(LuaProjectTreeItem item, ref int id)
        {
            if (Directory.Exists(item.fullPath))
            {
                string[] folders = Directory.GetDirectories(item.fullPath);
                int depth = item.depth + 1;
                foreach (var folder in folders)
                {
                    LuaProjectTreeItem folderItem = new LuaProjectTreeItem(++id, depth, Path.GetFileName(folder), folder);
                    folderItem.icon = s_TestIcons[0];
                    item.AddChild(folderItem);
                    IterationTree(folderItem, ref id);
                }

                string[] files = Directory.GetFiles(item.fullPath, "*.lua");
                foreach (var file in files)
                {
                    LuaProjectTreeItem fileItem = new LuaProjectTreeItem(++id, depth, Path.GetFileName(file), file);
                    fileItem.icon = s_TestIcons[1];
                    item.AddChild(fileItem);
                }

            }
        }

        protected override void RowGUI(TreeView.RowGUIArgs args)
        {
            base.RowGUI(args);
            float startX = this.depthIndentWidth + args.item.depth * this.foldoutWidth + 40 + EditorStyles.label.CalcSize(new GUIContent(args.item.displayName)).x;
            startX += args.item.depth * this.baseIndent;

            LuaProjectTreeItem item = (LuaProjectTreeItem)args.item;
            string readme = item.GetReadMeInfo();
            GUIContent content = new GUIContent(readme);
            Rect rect = args.rowRect;
            bool richText = EditorStyles.label.richText;
            EditorStyles.label.richText = true;
            Vector2 size = EditorStyles.label.CalcSize(content);
            rect.x = rect.width - size.x;
            rect.y += 1;
            if (!string.IsNullOrEmpty(readme))
                EditorGUI.LabelField(rect, content);
            EditorStyles.label.richText = richText;
        }

        protected override void ContextClickedItem(int id)
        {
            LuaProjectTreeItem item = (LuaProjectTreeItem)FindItem(id, rootItem);
            onItemContextClick(item);
        }

        protected override void DoubleClickedItem(int id)
        {
            LuaProjectTreeItem item = (LuaProjectTreeItem)FindItem(id, rootItem);
            onItemDoubleClick(item);
        }
    }
}