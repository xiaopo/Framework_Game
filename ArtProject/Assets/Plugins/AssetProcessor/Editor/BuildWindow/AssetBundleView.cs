using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static AssetsFileOrm.FileOrm;

namespace BuildTool
{
    public class AssetBundleView : TreeViewWithTreeModel<AssetBundleElement>
    {
        enum ViewColumns
        {
            AssetBundleName,
            AssetBundleHash,
            AssetCRC,
            AssetBundleMd5,
            Action,
        }

        public enum SortOption
        {
            Name,
            Value1,
            Value2,
            Value3,
            Value4,
            None,
        }
        // Sort options per column
        SortOption[] m_SortOptions =
        {
            SortOption.Name,
            SortOption.Value1,
            SortOption.Value2,
            SortOption.Value3,
            SortOption.Value4,
            SortOption.None,
        };
        public AssetBundleView(TreeViewState state, MultiColumnHeader multicolumnHeader, TreeModel<AssetBundleElement> model) : base(state, multicolumnHeader, model)
        {
            Assert.AreEqual(m_SortOptions.Length, Enum.GetValues(typeof(SortOption)).Length, "Ensure number of sort options are in sync with number of MyColumns enum values");

            showAlternatingRowBackgrounds = true;
            showBorder = true;
            multicolumnHeader.sortingChanged += OnSortingChanged;

            Reload();
        }

        protected override bool CanStartDrag(CanStartDragArgs args)
        {
            return false;
        }

        // Note we We only build the visible rows, only the backend has the full tree information. 
        // The treeview only creates info for the row list.
        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            var rows = base.BuildRows(root);
            SortIfNeeded(root, rows);
            return rows;
        }

        void OnSortingChanged(MultiColumnHeader multiColumnHeader)
        {
            SortIfNeeded(rootItem, GetRows());
        }
        void SortIfNeeded(TreeViewItem root, IList<TreeViewItem> rows)
        {
            if (rows.Count <= 1)
                return;

            if (multiColumnHeader.sortedColumnIndex == -1)
            {
                return; // No column to sort for (just use the order the data are in)
            }

            // Sort the roots of the existing tree items
            SortByMultipleColumns();
            TreeToList(root, rows);
            Repaint();
        }
        public static void TreeToList(TreeViewItem root, IList<TreeViewItem> result)
        {
            if (root == null)
                throw new NullReferenceException("root");
            if (result == null)
                throw new NullReferenceException("result");

            result.Clear();

            if (root.children == null)
                return;

            Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
            for (int i = root.children.Count - 1; i >= 0; i--)
                stack.Push(root.children[i]);

            while (stack.Count > 0)
            {
                TreeViewItem current = stack.Pop();
                result.Add(current);

                if (current.hasChildren && current.children[0] != null)
                {
                    for (int i = current.children.Count - 1; i >= 0; i--)
                    {
                        stack.Push(current.children[i]);
                    }
                }
            }
        }
        void SortByMultipleColumns()
        {
            var sortedColumns = multiColumnHeader.state.sortedColumns;

            if (sortedColumns.Length == 0)
                return;

            var myTypes = rootItem.children.Cast<TreeViewItem<AssetBundleElement>>();
            var orderedQuery = InitialOrder(myTypes, sortedColumns);

            for (int i = 1; i < sortedColumns.Length; i++)
            {
                SortOption sortOption = m_SortOptions[sortedColumns[i]];
                bool ascending = multiColumnHeader.IsSortedAscending(sortedColumns[i]);

                switch (sortOption)
                {
                    case SortOption.Name:
                        orderedQuery = orderedQuery.ThenBy(l => l.data.name, ascending);
                        break;
                    case SortOption.Value1:
                        orderedQuery = orderedQuery.ThenBy(l => l.data.assetBundleInfo.p_AssetBundleName, ascending);
                        break;
                    case SortOption.Value2:
                        orderedQuery = orderedQuery.ThenBy(l => l.data.assetBundleInfo.p_AssetBundleMd5, ascending);
                        break;
                    case SortOption.Value3:
                        orderedQuery = orderedQuery.ThenBy(l => l.data.assetBundleInfo.p_AssetBundleHash, ascending);
                        break;
                }
            }
            rootItem.children = orderedQuery.Cast<TreeViewItem>().ToList();
        }

        IOrderedEnumerable<TreeViewItem<AssetBundleElement>> InitialOrder(IEnumerable<TreeViewItem<AssetBundleElement>> myTypes, int[] history)
        {
            SortOption sortOption = m_SortOptions[history[0]];
            bool ascending = multiColumnHeader.IsSortedAscending(history[0]);
            switch (sortOption)
            {
                case SortOption.Name:
                    return myTypes.Order(l => l.data.assetBundleInfo.p_AssetBundleName, ascending);
                case SortOption.Value1:
                    return myTypes.Order(l => l.data.assetBundleInfo.p_AssetBundleHash, ascending);
                case SortOption.Value2:
                    return myTypes.Order(l => l.data.assetBundleInfo.p_AssetCRC, ascending);
                case SortOption.Value3:
                    return myTypes.Order(l => l.data.assetBundleInfo.p_AssetBundleMd5, ascending);
                case SortOption.Value4:
                default:
                    return myTypes.Order(l => l.data.assetBundleInfo.p_AssetBundleName, ascending);
            }
        }
        public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(float treeViewWidth)
        {
            var columns = new[]
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("AssetBundleName"),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 300,
                    minWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("AssetBundleHash"),
                    headerTextAlignment = TextAlignment.Right,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Left,
                    width = 100,
                    minWidth = 60,
                    autoResize = true,
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("AssetCRC"),
                    headerTextAlignment = TextAlignment.Right,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Left,
                    width = 100,
                    minWidth = 60,
                    autoResize = true,
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("AssetBundleMd5"),
                    headerTextAlignment = TextAlignment.Right,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Right,
                    width = 100,
                    minWidth = 60,
                    autoResize = true,
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("操作"),
                    headerTextAlignment = TextAlignment.Right,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Right,
                    width = 100,
                    minWidth = 60,
                    autoResize = true,
                }
            };

            Assert.AreEqual(columns.Length, Enum.GetValues(typeof(ViewColumns)).Length, "Number of columns should match number of enum values: You probably forgot to update one of them.");

            var state = new MultiColumnHeaderState(columns);
            return state;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (TreeViewItem<AssetBundleElement>)args.item;

            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                CellGUI(args.GetCellRect(i), item, (ViewColumns)args.GetColumn(i), ref args);
            }
        }
        void CellGUI(Rect cellRect, TreeViewItem<AssetBundleElement> item, ViewColumns column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);
            AssetBundleInfo assetBundleInfo = item.data.assetBundleInfo;
            switch (column)
            {
                case ViewColumns.AssetBundleName:
                    {
                        //GUI.Label(cellRect, item.data.name);
                        base.RowGUI(args);
                    }
                    break;
                case ViewColumns.AssetBundleHash:
                    {
                        GUI.Label(cellRect, assetBundleInfo.p_AssetBundleHash);
                    }
                    break;
                case ViewColumns.AssetCRC:
                    {
                        GUI.Label(cellRect, assetBundleInfo.p_AssetCRC);
                    }
                    break;
                case ViewColumns.AssetBundleMd5:
                    {
                        GUI.Label(cellRect, assetBundleInfo.p_AssetBundleMd5);
                    }
                    break;
                case ViewColumns.Action:
                    //{
                    //    if (assetBundleInfo.isDestroy)
                    //    {
                    //        GUI.Label(cellRect, "Destroyed");
                    //    }
                    //    else
                    //    {
                    //        if (GUI.Button(cellRect, "选中"))
                    //        {
                    //            if (assetBundleInfo.gameObject)
                    //            {
                    //                //GameObject gameObject = GameObject.Find(meshInfo._gameObjectName);
                    //                //Selection.activeObject = gameObject;
                    //                Selection.activeObject = assetBundleInfo.gameObject;
                    //            }
                    //        }
                    //    }
                    //}
                    break;
            }
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return true;
        }
    }
    static class MyExtensionMethods
    {
        public static IOrderedEnumerable<T> Order<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
        {
            if (ascending)
            {
                return source.OrderBy(selector);
            }
            else
            {
                return source.OrderByDescending(selector);
            }
        }

        public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> selector, bool ascending)
        {
            if (ascending)
            {
                return source.ThenBy(selector);
            }
            else
            {
                return source.ThenByDescending(selector);
            }
        }
    }
}
