using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameObjectAddMenu
{
    [MenuItem("GameObject/Kernel/UI/Dropdown")]
    public static void Dropdown(MenuCommand menuCommand)
    {
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/SC KRM/Editor/Game Object Add Menu/Dropdown.prefab", typeof(GameObject));
        if (menuCommand.context != null)
            gameObject = Object.Instantiate(gameObject, ((GameObject)menuCommand.context).transform);
        else
            gameObject = Object.Instantiate(gameObject, null);

        gameObject.name = "Dropdown";

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeObject = gameObject;
    }

    [MenuItem("GameObject/Kernel/Setting UI/Title")]
    public static void SettingTitle(MenuCommand menuCommand)
    {
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/SC KRM/Editor/Game Object Add Menu/Title.prefab", typeof(GameObject));
        if (menuCommand.context != null)
            gameObject = Object.Instantiate(gameObject, ((GameObject)menuCommand.context).transform);
        else
            gameObject = Object.Instantiate(gameObject, null);

        gameObject.name = "Title";

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeObject = gameObject;
    }

    [MenuItem("GameObject/Kernel/Setting UI/Input Field")]
    public static void SettingInputField(MenuCommand menuCommand)
    {
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/SC KRM/Editor/Game Object Add Menu/Setting Input Field.prefab", typeof(GameObject));
        if (menuCommand.context != null)
            gameObject = Object.Instantiate(gameObject, ((GameObject)menuCommand.context).transform);
        else
            gameObject = Object.Instantiate(gameObject, null);

        gameObject.name = "Setting Input Field";

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeObject = gameObject;
    }

    [MenuItem("GameObject/Kernel/Setting UI/Slider")]
    public static void SettingSlider(MenuCommand menuCommand)
    {
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/SC KRM/Editor/Game Object Add Menu/Setting Slider.prefab", typeof(GameObject));
        if (menuCommand.context != null)
            gameObject = Object.Instantiate(gameObject, ((GameObject)menuCommand.context).transform);
        else
            gameObject = Object.Instantiate(gameObject, null);

        gameObject.name = "Setting Slider";

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeObject = gameObject;
    }

    [MenuItem("GameObject/Kernel/Setting UI/Toggle")]
    public static void SettingToggle(MenuCommand menuCommand)
    {
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/SC KRM/Editor/Game Object Add Menu/Setting Toggle.prefab", typeof(GameObject));
        if (menuCommand.context != null)
            gameObject = Object.Instantiate(gameObject, ((GameObject)menuCommand.context).transform);
        else
            gameObject = Object.Instantiate(gameObject, null);

        gameObject.name = "Setting Toggle";

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeObject = gameObject;
    }

    [MenuItem("GameObject/Kernel/Setting UI/Dropdown")]
    public static void SettingDropdown(MenuCommand menuCommand)
    {
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/SC KRM/Editor/Game Object Add Menu/Setting Dropdown.prefab", typeof(GameObject));
        if (menuCommand.context != null)
            gameObject = Object.Instantiate(gameObject, ((GameObject)menuCommand.context).transform);
        else
            gameObject = Object.Instantiate(gameObject, null);

        gameObject.name = "Setting Dropdown";

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeObject = gameObject;
    }

    [MenuItem("GameObject/Kernel/Setting UI/Space")]
    public static void SettingSpace(MenuCommand menuCommand)
    {
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/SC KRM/Editor/Game Object Add Menu/Space.prefab", typeof(GameObject));
        if (menuCommand.context != null)
            gameObject = Object.Instantiate(gameObject, ((GameObject)menuCommand.context).transform);
        else
            gameObject = Object.Instantiate(gameObject, null);

        gameObject.name = "Space";

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeObject = gameObject;
    }

    [MenuItem("GameObject/Kernel/Setting UI/Line")]
    public static void SettingLine(MenuCommand menuCommand)
    {
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/SC KRM/Editor/Game Object Add Menu/Line.prefab", typeof(GameObject));
        if (menuCommand.context != null)
            gameObject = Object.Instantiate(gameObject, ((GameObject)menuCommand.context).transform);
        else
            gameObject = Object.Instantiate(gameObject, null);

        gameObject.name = "Line";

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeObject = gameObject;
    }
}
