using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(TokenSpawner))]
public class TokenSpawnRandomizer : Editor
{
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        DrawDefaultInspector();

        TokenSpawner spawner = (TokenSpawner)target;

        if (GUILayout.Button("Randomize Loop Offset")) {
            spawner.loopOffset = Random.Range(0, spawner.loopAfterTicks - 1);

            EditorUtility.SetDirty(spawner);
            EditorSceneManager.MarkSceneDirty(spawner.gameObject.scene);
        }
    }
}
