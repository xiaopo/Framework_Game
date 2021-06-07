using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using System.IO;
using static AssetsFileOrm;

namespace BuildTool
{
    public class AssetBundleEditor : IEditor
    {
        Rect toolbarRect
        {
            get { return new Rect(20f, 70f, 300f, 20f); }
        }
        Rect multiColumnTreeViewRect
        {
            get { return new Rect(20, 100, 900, 600); }
        }
        static TreeModel<AssetBundleElement> treeModel;
        [NonSerialized] static bool m_Initialized;
        [SerializeField] static TreeViewState m_TreeViewState;
        [SerializeField] static MultiColumnHeaderState m_MultiColumnHeaderState;
        static AssetBundleView m_TrisView;
        static SearchField m_SearchField;
        static AssetsFileOrm assetsFileOrm;
        public void OnGUI()
        {
            InitIfNeed();
            SearchBar(toolbarRect);
            m_TrisView.OnGUI(multiColumnTreeViewRect);
        }
        void InitIfNeed()
        {
            if (!m_Initialized)
            {
                if (m_TreeViewState == null)
                    m_TreeViewState = new TreeViewState();

                treeModel = new TreeModel<AssetBundleElement>(GetData());

                bool firstInit = m_MultiColumnHeaderState == null;
                var headerState = AssetBundleView.CreateDefaultMultiColumnHeaderState(multiColumnTreeViewRect.width);
                if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_MultiColumnHeaderState, headerState))
                    MultiColumnHeaderState.OverwriteSerializedFields(m_MultiColumnHeaderState, headerState);
                m_MultiColumnHeaderState = headerState;

                var multiColumnHeader = new MyMultiColumnHeader(headerState);

                m_TrisView = new AssetBundleView(m_TreeViewState, multiColumnHeader, treeModel);

                m_SearchField = new SearchField();
                m_SearchField.downOrUpArrowKeyPressed += m_TrisView.SetFocusAndEnsureSelectedItem;

                m_Initialized = true;

            }
        }
        IList<AssetBundleElement> SetRoot()
        {
            List<AssetBundleElement> ItemInfos = new List<AssetBundleElement>();
            ItemInfos.Add(new AssetBundleElement("Root") { id = 0, depth = -1 });
            return ItemInfos;
        }
        void SearchBar(Rect rect)
        {
            m_TrisView.searchString = m_SearchField.OnGUI(rect, m_TrisView.searchString);
        }

        public List<AssetBundleElement> GetData()
        {
            assetsFileOrm = new AssetsFileOrm();
            assetsFileOrm.Load("D:/assetBundles");
            List<AssetBundleElement> meshElementList = new List<AssetBundleElement>();
            int itemId = 0;
            meshElementList.Add(new AssetBundleElement("Root") { id = itemId, depth = -1 });
            foreach (var keyValue in assetsFileOrm.allFileOrm)
            {
                AssetBundleElement assetBundleType = new AssetBundleElement(keyValue.Key) { id = itemId++, depth = 0 };
                meshElementList.Add(assetBundleType);
                FileOrm fileOrm = keyValue.Value;
                if (null != fileOrm)
                {
                    foreach (var assetBundleInfo in fileOrm.p_AssetBundleList)
                    {
                        AssetBundleElement assetBundleElement = new AssetBundleElement(assetBundleInfo) { id = itemId++, depth = 1 };
                        meshElementList.Add(assetBundleElement);
                        foreach (var item in assetBundleInfo.p_Assets)
                        {
                            AssetBundleElement assetElement = new AssetBundleElement(item) { id = itemId++, depth = 2 };
                            //assetElement.parent = assetBundleElement;
                            meshElementList.Add(assetElement);
                        }
                    }
                }
            }
            return meshElementList;
            //treeModel.AddElements(meshElementList, treeModel.root, 0);
        }
        internal class MyMultiColumnHeader : MultiColumnHeader
        {
            Mode m_Mode;

            public enum Mode
            {
                LargeHeader,
                DefaultHeader,
                MinimumHeaderWithoutSorting
            }

            public MyMultiColumnHeader(MultiColumnHeaderState state)
                : base(state)
            {
                mode = Mode.DefaultHeader;
            }

            public Mode mode
            {
                get
                {
                    return m_Mode;
                }
                set
                {
                    m_Mode = value;
                    switch (m_Mode)
                    {
                        case Mode.LargeHeader:
                            canSort = true;
                            height = 37f;
                            break;
                        case Mode.DefaultHeader:
                            canSort = true;
                            height = DefaultGUI.defaultHeight;
                            break;
                        case Mode.MinimumHeaderWithoutSorting:
                            canSort = false;
                            height = DefaultGUI.minimumHeight;
                            break;
                    }
                }
            }

            protected override void ColumnHeaderGUI(MultiColumnHeaderState.Column column, Rect headerRect, int columnIndex)
            {
                // Default column header gui
                base.ColumnHeaderGUI(column, headerRect, columnIndex);

                // Add additional info for large header
                if (mode == Mode.LargeHeader)
                {
                    // Show example overlay stuff on some of the columns
                    if (columnIndex > 2)
                    {
                        headerRect.xMax -= 3f;
                        var oldAlignment = EditorStyles.largeLabel.alignment;
                        EditorStyles.largeLabel.alignment = TextAnchor.UpperRight;
                        GUI.Label(headerRect, 36 + columnIndex + "%", EditorStyles.largeLabel);
                        EditorStyles.largeLabel.alignment = oldAlignment;
                    }
                }
            }
        }
    }
}
