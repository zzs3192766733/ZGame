//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/29 18:37:22
//========================================================

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class TestEditorWindow : EditorWindow
{
    [SerializeField] TreeViewState m_TreeViewState;

    SimpleTreeView m_TreeView;
    SearchField m_SearchField;

    void OnEnable ()
    {
        // if (m_TreeViewState == null)
        //     m_TreeViewState = new TreeViewState ();
        var allItems = new List<TreeViewItem>
        {
            new TreeViewItem {id = 1, depth = 0, displayName = "动物"},
            new TreeViewItem {id = 2, depth = 1, displayName = "哺乳动物"},
            new TreeViewItem {id = 3, depth = 2, displayName = "老虎"},
            new TreeViewItem {id = 4, depth = 2, displayName = "大大象"},
            new TreeViewItem {id = 5, depth = 2, displayName = "鹿"},
            new TreeViewItem {id = 6, depth = 2, displayName = "aaa"},
            new TreeViewItem {id = 7, depth = 1, displayName = "bbb"},
            new TreeViewItem {id = 8, depth = 2, displayName = "ccc"},
            new TreeViewItem {id = 9, depth = 2, displayName = "ddd"},
        };
        m_TreeView = new SimpleTreeView(new TreeViewState (),allItems);
        m_TreeView.AddItem(new TreeViewItem {id = 1, depth = 0, displayName = "动物"});
        //m_SearchField = new SearchField ();
        //m_SearchField.downOrUpArrowKeyPressed += m_TreeView.SetFocusAndEnsureSelectedItem;
    }

    void OnGUI ()
    {
        //DoToolbar ();
        DoTreeView ();
    }

    void DoToolbar()
    {
        GUILayout.BeginHorizontal (EditorStyles.toolbar);
        GUILayout.Space (100);
        GUILayout.FlexibleSpace();
        m_TreeView.searchString = m_SearchField.OnToolbarGUI (m_TreeView.searchString);
        GUILayout.EndHorizontal();
    }

    void DoTreeView()
    {
        Rect rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
        m_TreeView.OnGUI(rect);
    }

    [MenuItem ("TreeView Examples/Simple Tree Window")]
    static void ShowWindow ()
    {
        var window = GetWindow<TestEditorWindow> ();
        window.titleContent = new GUIContent ("My Window");
        window.Show ();
        
    }
}

class SimpleTreeView : TreeView
{
    private List<TreeViewItem> _items;
    public SimpleTreeView(TreeViewState treeViewState,List<TreeViewItem> item)
        : base(treeViewState)
    {
        _items = item;
        Reload();
    }

    public void AddItem(TreeViewItem item) => _items.Add(item);

    protected override TreeViewItem BuildRoot()
    {
        var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
        SetupParentsAndChildrenFromDepths(root, _items);
        return root;
    }
}
