﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Febucci.UI.Examples
{
    [CustomEditor(typeof(UsageExample))]
    public class UsageExampleCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                ButtonSetText();
                GUILayout.Space(10);

                base.OnInspectorGUI();

                GUILayout.Space(10);
                ButtonSetText();
            }
            else
            {
                base.OnInspectorGUI();
            }
        }

        void ButtonSetText()
        {
            if (GUILayout.Button("Set Text again"))
            {
                ((UsageExample)target)?.ShowText();
            }
        }
    }

}