using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameObjectAddMenu
{
    static void PrefabInstantiate(string name, MenuCommand menuCommand)
    {
        Object gameObject = AssetDatabase.LoadAssetAtPath($"Assets/SC KRM/Editor/Game Object Add Menu/{name}.prefab", typeof(Object));
        if (menuCommand.context != null)
            gameObject = PrefabUtility.InstantiatePrefab(gameObject, ((GameObject)menuCommand.context).transform);
        else
            gameObject = PrefabUtility.InstantiatePrefab(gameObject, null);

        gameObject.name = name; 

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + name);
        Selection.activeObject = gameObject;
    }

    [MenuItem("GameObject/Kernel/UI/Dropdown")]
    public static void Dropdown(MenuCommand menuCommand) => PrefabInstantiate("Dropdown", menuCommand);

    [MenuItem("GameObject/Kernel/Setting UI/Title")]
    public static void SettingTitle(MenuCommand menuCommand) => PrefabInstantiate("Title", menuCommand);

    [MenuItem("GameObject/Kernel/Setting UI/Input Field")]
    public static void SettingInputField(MenuCommand menuCommand) => PrefabInstantiate("Setting Input Field", menuCommand);

    [MenuItem("GameObject/Kernel/Setting UI/Slider")]
    public static void SettingSlider(MenuCommand menuCommand) => PrefabInstantiate("Setting Slider", menuCommand);

    [MenuItem("GameObject/Kernel/Setting UI/Toggle")]
    public static void SettingToggle(MenuCommand menuCommand) => PrefabInstantiate("Setting Toggle", menuCommand);

    [MenuItem("GameObject/Kernel/Setting UI/Dropdown")]
    public static void SettingDropdown(MenuCommand menuCommand) => PrefabInstantiate("Setting Dropdown", menuCommand);

    [MenuItem("GameObject/Kernel/Setting UI/Space")]
    public static void SettingSpace(MenuCommand menuCommand) => PrefabInstantiate("Space", menuCommand);

    [MenuItem("GameObject/Kernel/Setting UI/Line")]
    public static void SettingLine(MenuCommand menuCommand) => PrefabInstantiate("Line", menuCommand);
}
