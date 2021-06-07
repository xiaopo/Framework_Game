using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace BuildTool
{
    public class BuildWindow : EditorWindow
    {
        float btnHeight = 50;
        float btnWidth = 150;
        static BuildWindow window;
        [MenuItem("Build/BuildWindow")]
        public static void OpenWindow()
        {
            window = GetWindow<BuildWindow>();
            window.minSize = new Vector2(800, 500);
            window.titleContent = new GUIContent("打包工具");
            Init();
        }

        public static BuildEditor buildEditor;
        public static AssetBundleEditor assetEditor;
        public static IEditor currEditor;

        public static void Init()
        {
            if(null == buildEditor)
            {
                buildEditor = new BuildEditor();
                currEditor = buildEditor;
            }
            if (null == assetEditor)
            {
                assetEditor = new AssetBundleEditor();
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("打包工具", GUILayout.Width(btnWidth), GUILayout.Height(btnHeight)))
            {
                currEditor = buildEditor;
            }
            if (GUILayout.Button("资源列表", GUILayout.Width(btnWidth), GUILayout.Height(btnHeight)))
            {
                currEditor = assetEditor;
            }
            GUILayout.EndHorizontal();
            currEditor.OnGUI();
        }
    }
}
