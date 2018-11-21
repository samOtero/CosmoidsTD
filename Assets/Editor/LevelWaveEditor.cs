using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(LevelWave))]
[CanEditMultipleObjects]
public class LevelWaveEditor :Editor {

    private ReorderableList waveGroup1;

    private void OnEnable()
    {
        waveGroup1 = new ReorderableList(serializedObject, serializedObject.FindProperty("WaveGroups"), true, true, true, true);
        waveGroup1.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = waveGroup1.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, 400, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("waveNum")
                );
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, 400, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("unitPrefab"), new GUIContent("Unit: ")
                );
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, 400, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("timeBetweenSpawns"), new GUIContent("Time Between: ")
                );
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 3, 400, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("numToSpawn"), new GUIContent("Amount to Spawn: "), true
                );
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4, 400, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("stepList"), new GUIContent("Steps: "), true
                );
        };
        waveGroup1.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Wave Groups");
        };

        waveGroup1.elementHeightCallback = (index) =>
        {
            var element = waveGroup1.serializedProperty.GetArrayElementAtIndex(index);
            var totalHeight = 0f;
            var childElement = element.FindPropertyRelative("waveNum");
            totalHeight += EditorGUI.GetPropertyHeight(childElement);
            while(childElement.NextVisible(false))
            {
                totalHeight += EditorGUI.GetPropertyHeight(childElement);
            }
            return totalHeight;
        };

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("currentWaveNum"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("totalWaves"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("currentStep"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("waveCounter"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("waveInitialCooldown"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("forceStartWave"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("groupSpawnCounter"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("finished"));
        waveGroup1.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

}
