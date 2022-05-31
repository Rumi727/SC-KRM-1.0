using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SCKRM;
using SCKRM.UI.Setting;

[Serializable]
internal class ToolbarFPSSlider : BaseToolbarElement {
	[SerializeField] int minFPS = 1;
	[SerializeField] int maxFPS = 120;

	public override string NameInList => "[Slider] FPS";

	public ToolbarFPSSlider(int minFPS = 1, int maxFPS = 120) : base(200) {
		this.minFPS = minFPS;
		this.maxFPS = maxFPS;
	}

	protected override void OnDrawInList(Rect position) {
		position.width = 70.0f;
		EditorGUI.LabelField(position, "Min FPS");

		position.x += position.width + FieldSizeSpace;
		position.width = 50.0f;
		minFPS = Mathf.RoundToInt(EditorGUI.IntField(position, "", minFPS));

		position.x += position.width + FieldSizeSpace;
		position.width = 70.0f;
		EditorGUI.LabelField(position, "Max FPS");

		position.x += position.width + FieldSizeSpace;
		position.width = 50.0f;
		maxFPS = Mathf.RoundToInt(EditorGUI.IntField(position, "", maxFPS));
	}

	protected override void OnDrawInToolbar() 
	{
		EditorGUILayout.LabelField("FPS", GUILayout.Width(30));

		GUI.enabled = Kernel.isPlaying;
		int fpsLimit = EditorGUILayout.IntSlider("", VideoManager.SaveData.fpsLimit, minFPS, maxFPS, GUILayout.Width(WidthInToolbar - 30.0f));
		GUI.enabled = true;

		if (GUI.changed && Setting.settingInstance.TryGetValue("SCKRM.VideoManager+SaveData.fpsLimit", out Setting value))
		{
			VideoManager.SaveData.fpsLimit = fpsLimit;
			value.ScriptOnValueChanged();
		}
	}
}
