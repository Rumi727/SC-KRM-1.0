using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameObjectAddMenu
{
    [MenuItem("GameObject/Setting UI/Input Field")]
    public static void SettingInputField(MenuCommand menuCommand)
    {
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/SC KRM/Editor/Game Object Add Menu/Setting Input Field.prefab", typeof(GameObject));
        gameObject = Object.Instantiate(gameObject, ((GameObject)menuCommand.context).transform);
        gameObject.name = "Setting Input Field";

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeObject = gameObject;
    }

    [MenuItem("GameObject/Setting UI/Slider")]
    public static void SettingSlider(MenuCommand menuCommand)
    {
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/SC KRM/Editor/Game Object Add Menu/Setting Slider.prefab", typeof(GameObject));
        gameObject = Object.Instantiate(gameObject, ((GameObject)menuCommand.context).transform);
        gameObject.name = "Setting Slider";

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeObject = gameObject;
    }

    [MenuItem("GameObject/Setting UI/Toggle")]
    public static void SettingToggle(MenuCommand menuCommand)
    {
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/SC KRM/Editor/Game Object Add Menu/Setting Toggle.prefab", typeof(GameObject));
        gameObject = Object.Instantiate(gameObject, ((GameObject)menuCommand.context).transform);
        gameObject.name = "Setting Toggle";

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeObject = gameObject;
    }
}
