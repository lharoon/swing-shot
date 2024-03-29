﻿using UnityEditor;
using UnityEngine;

namespace Febucci.UI.Core
{
#if UNITY_EDITOR

    [CustomEditor(typeof(TAnimGlobalDataScriptable))]
    class TAnimGlobalDataScriptableDrawer : Editor
    {
        TAnimGlobalDataScriptable script;

        SerializedProperty behaviorPresets;
        SerializedProperty appearancesPresets;
        SerializedProperty customActionsArray;

        TextAnimatorDrawer.UserPresetDrawer[] behaviorDrawers = new TextAnimatorDrawer.UserPresetDrawer[0];
        TextAnimatorDrawer.UserPresetDrawer[] appearancesDrawers = new TextAnimatorDrawer.UserPresetDrawer[0];

        protected virtual void OnEnable()
        {
            behaviorPresets = serializedObject.FindProperty("globalBehaviorPresets");
            appearancesPresets = serializedObject.FindProperty("globalAppearancePresets");
            customActionsArray = serializedObject.FindProperty("customActions");
            script = (TAnimGlobalDataScriptable)target;


            Undo.undoRedoPerformed += Redo;

        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= Redo;
        }

        void Redo()
        {
            serializedObject.UpdateIfRequiredOrScript(); //I have spent too much searching this method... :(
            Repaint();
            TryResettingTextAnimators();
        }

        bool showBehaviors = false;
        bool showAppearances = false;
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
                EditorGUILayout.LabelField($"[!!] Remember: Saves are applied in play mode.");

            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);

                TextAnimatorDrawer.ShowPresets(ref behaviorDrawers, ref showBehaviors, ref behaviorPresets, false, true);

                TextAnimatorDrawer.ShowPresets(ref appearancesDrawers, ref showAppearances, ref appearancesPresets, true, true);


                EditorGUI.indentLevel--;
            }


            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(customActionsArray, true);

                EditorGUI.indentLevel--;
            }


            if (serializedObject.hasModifiedProperties)
            {
                //Repaint();

                //Undo.RecordObject(serializedObject.targetObject, "Changed TextAnimator Global Data Scriptable");
                Undo.RecordObject(script, "Changed TextAnimator Global Data Scriptable");
                EditorUtility.SetDirty(script);

                //Undo.RegisterCompleteObjectUndo(script, "Changed TextAnimator Global Data Scriptable");
                serializedObject.ApplyModifiedProperties();

                Repaint();
                TryResettingTextAnimators();
            }

        }

        void TryResettingTextAnimators()
        {
            if (EditorApplication.isPlaying)
            {
                TAnim_EditorHelper.TriggerEvent();
            }
        }
    }

#endif


}