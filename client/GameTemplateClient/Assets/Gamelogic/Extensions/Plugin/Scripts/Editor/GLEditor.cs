﻿//----------------------------------------------//
// Gamelogic Grids                              //
// http://www.gamelogic.co.za                   //
// Copyright (c) 2013 Gamelogic (Pty) Ltd       //
//----------------------------------------------//

using System;
using System.Linq;
using System.Reflection;
using Gamelogic.Extensions.Internal;
using UnityEditor;
using UnityEngine;



namespace Gamelogic.Extensions.Editor.Internal
{
	[Version(1, 3)]
	public class GLEditor<T> : UnityEditor.Editor
	{
		public T Target
		{
			get { return (T) (object) target; }
		}

		public T[] Targets
		{
			get { return targets.Cast<T>().ToArray(); }
		}

		public void Splitter()
		{
			GLEditorGUI.Splitter();
		}

		public static int AddCombo(string[] options, int selectedIndex)
		{
			return EditorGUILayout.Popup(selectedIndex, options);
		}

		public bool HasProperty(string propertyName)
		{
			var property = serializedObject.FindProperty(propertyName);

			return property != null;
		}

		public GLSerializedProperty FindProperty(string propertyName)
		{
			var property = new GLSerializedProperty
			{
				SerializedProperty = serializedObject.FindProperty(propertyName),
				CustomTooltip = ""
			};

			if (property.SerializedProperty == null)
			{
				Debug.LogError("Could not find property " + propertyName + " in class " + typeof (T).Name);

				return property;
			}

			Type type = typeof (T);

			FieldInfo field = type.GetField(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			if (field == null)
			{
				Debug.LogError("Could not find field " + propertyName + " in class " + typeof (T).Name);

				return property;
			}

			/*
			var descriptions = field.GetCustomAttributes(typeof (DescriptionAttribute), true);

			if (descriptions.Any())
			{
				property.CustomTooltip = (descriptions.First() as DescriptionAttribute).Description;
			}
			*/
			return property;
		}

		protected void AddField(SerializedProperty prop)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(prop, true);
			EditorGUILayout.EndHorizontal();
		}

		protected void AddField(GLSerializedProperty prop)
		{
			if (prop == null) return;
			if (prop.SerializedProperty == null) return;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(prop.SerializedProperty,
				new GUIContent(prop.SerializedProperty.name.SplitCamelCase(), prop.CustomTooltip), true);
			EditorGUILayout.EndHorizontal();
		}

		protected void AddLabel(string title, string text)
		{
			EditorGUILayout.LabelField(title, text);
		}

        protected void AddTextAndButton(string text, string buttonLabel, Action buttonAction)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);

            if (GUILayout.Button(buttonLabel))
            {
                if (buttonAction != null)
                    buttonAction();
            }

            EditorGUILayout.EndHorizontal();
        }

        protected void ArrayGUI(SerializedObject obj, SerializedProperty property)
        {
            int size = property.arraySize;

            int newSize = EditorGUILayout.IntField(property.name + " Size", size);

            if (newSize != size)
            {
                property.arraySize = newSize;
            }

            EditorGUI.indentLevel = 3;

            for (int i = 0; i < newSize; i++)
            {
                var prop = property.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(prop);
            }
        }
    }
}


