using UnityEditor;
using UnityEngine;

public class XaekyWizardWindow : EditorWindow
{
  protected virtual string HeaderLabel => "";
  protected virtual void DrawHeader()
  {
    var blackBg = new Texture2D(1, 1);
    blackBg.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.25f));
    blackBg.Apply();
    var brandingHeaderStyle = new GUIStyle(GUI.skin.label) {
      fontSize = 12,
      fontStyle = FontStyle.Bold,
      padding = new RectOffset(16, 16, 2, 2),
      normal = { background = blackBg }
    };
    GUILayout.Label("Xaeky Avatars", brandingHeaderStyle);
    var headerStyle = new GUIStyle(GUI.skin.label) {
      fontStyle = FontStyle.Bold,
      fontSize = 16,
      padding = new RectOffset(16, 16, 12, 12),
      normal = { background = Texture2D.grayTexture }
    };
    GUILayout.Label(HeaderLabel, headerStyle);
  }

  protected virtual void DrawContent() {}

  private void OnGUI()
  {
    GUILayout.BeginArea(new Rect(0,0, position.width, position.height));
    DrawHeader();
    DrawContent();
    GUILayout.EndArea();
  }
}

public class XaekyReactive : EditorWindow
{
  public static string TextField(string label, string value, System.Action<string> onValueChanged)
  {
    string newValue = EditorGUILayout.TextField(label, value);
    if (newValue != value) onValueChanged?.Invoke(newValue);
    return newValue;
  }
}