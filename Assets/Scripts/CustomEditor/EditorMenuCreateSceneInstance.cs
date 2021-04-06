using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// SceneInstance 오브젝트를 생성하는 스크립트
public sealed class EditorMenuCreateSceneInstance : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/Create SceneInstance", false, 10)]
    private static void CreateSceneInstance()
    {
        GameObject newSceneInstanceObj = new GameObject();
        newSceneInstanceObj.name = "SceneInstance";
        newSceneInstanceObj.AddComponent<SceneInstance>();
#endif
    }
}
