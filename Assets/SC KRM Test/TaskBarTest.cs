using SCKRM.Input;
using SCKRM.UI.TaskBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBarTest : MonoBehaviour
{
    void Update()
    {
        TaskBarManager.taskBarShow = false;

        if (InputManager.GetKeyDown("gui.home", InputLockDeny.TaskBar))
            TaskBarManager.cropTheScreen = !TaskBarManager.cropTheScreen;

        if (InputManager.GetKeyDown(KeyCode.Return, InputLockDeny.TaskBar))
            TaskBarManager.SaveData.topMode = !TaskBarManager.SaveData.topMode;
    }
}
