using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Fungus
{

	[CustomEditor (typeof(Character))]
	public class CharacterEditor : Editor
	{
		Material spriteMaterial;

		SerializedProperty nameTextProp;
		SerializedProperty nameColorProp;
		SerializedProperty portraitsProp;
		SerializedProperty notesProp;

		void OnEnable()
		{
			nameTextProp = serializedObject.FindProperty ("nameText");
			nameColorProp = serializedObject.FindProperty ("nameColor");
			portraitsProp = serializedObject.FindProperty ("portraits");
			notesProp = serializedObject.FindProperty ("notes");

			Shader shader = Shader.Find("Sprites/Default");
			if (shader != null)
			{
				spriteMaterial = new Material(shader);
				spriteMaterial.hideFlags = HideFlags.DontSave;
			}
		}

		void OnDisable()
		{
			DestroyImmediate(spriteMaterial);
		}

		public override void OnInspectorGUI() 
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(nameTextProp, new GUIContent("Name Text", "Name of the character display in the dialog"));

			EditorGUILayout.PropertyField(nameColorProp, new GUIContent("Name Color", "Color of name text display in the dialog"));

			EditorGUILayout.PropertyField(portraitsProp, new GUIContent("Portraits", "Character portrait to display with dialog"), true);

			EditorGUILayout.PropertyField(notesProp, new GUIContent("Notes", "Notes about this story character (personality, attibutes, etc.)"));

			EditorGUILayout.Separator();

			Character t = target as Character;
			if (t.portraits != null &&
			    t.portraits[0] != null &&
			    spriteMaterial != null)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				float aspect = (float)t.portraits[0].texture.width / (float)t.portraits[0].texture.height;
				Rect imagePreviewRect = GUILayoutUtility.GetAspectRect(aspect, GUILayout.Width(300), GUILayout.ExpandWidth(false));
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();

				DrawPreview(imagePreviewRect, t.portraits[0].texture);
			}

			serializedObject.ApplyModifiedProperties();
		}

		public void DrawPreview(Rect previewRect, Texture2D texture)
		{
			if (texture == null)
			{
				return;
			}
			EditorGUI.DrawPreviewTexture(previewRect, 
			                             texture,
			                             spriteMaterial);
		}
	}

}