//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2022/1/1 9:33:55
//========================================================

using System;
using GameFramework.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace GameFramework.Game.Editor
{
    [CustomEditor(typeof(GameColliderEventBox))]
    public class GameColliderEventBoxEditor : UnityEditor.Editor
    {
        private SerializedProperty _center;
        private SerializedProperty _size;
        private BoxCollider _boxCollider;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            _center = serializedObject.FindProperty("center");
            _size = serializedObject.FindProperty("size");
            var isShow = serializedObject.FindProperty("isShow");
            var showColor = serializedObject.FindProperty("showColor");
            _boxCollider = serializedObject.FindProperty("boxCollider").objectReferenceValue as BoxCollider;
            var allEnterEvents = serializedObject.FindProperty("allEnterEvents");
            var allExitEvents = serializedObject.FindProperty("allExitEvents");

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                _center.vector3Value = EditorGUILayout.Vector3Field("中心:", _center.vector3Value);
                _size.vector3Value = EditorGUILayout.Vector3Field("尺寸:", _size.vector3Value);
                if (_boxCollider != null)
                {
                    _boxCollider.size = _size.vector3Value;
                    _boxCollider.center = _center.vector3Value;
                }
            }
            GUILayout.EndVertical();


            isShow.boolValue = EditorGUILayout.Toggle("是否一直在场景中显示范围:", isShow.boolValue);
            if (isShow.boolValue)
            {
                showColor.colorValue = EditorGUILayout.ColorField("选择显示区域颜色:", showColor.colorValue);
            }

            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("事件绑定");
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
                
                EditorGUILayout.PropertyField(allEnterEvents, new GUIContent("进入区域事件"), true);
                EditorGUILayout.PropertyField(allExitEvents, new GUIContent("离开区域事件"), true);
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}