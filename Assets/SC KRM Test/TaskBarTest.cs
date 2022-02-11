using SCKRM.Input;
using SCKRM.UI.StatusBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBarTest : MonoBehaviour
{
    void Awake() => StatusBarManager.allowStatusBarShow = false;
}
