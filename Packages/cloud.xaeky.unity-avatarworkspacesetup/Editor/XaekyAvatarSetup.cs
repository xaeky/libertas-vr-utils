using UnityEditor;
using UnityEngine;

namespace Xaeky
{
  public class XaekyCustomAvatarSetup : MonoBehaviour
  {
    public static bool UserDirExists(string usernamePath = "")
    {
      return AssetDatabase.IsValidFolder($"Assets/{usernamePath}");
    }

    public static bool ValidateUsernameDir(string usernamePath = "")
    {
      if (string.IsNullOrEmpty(usernamePath)) return false;
      if (System.Text.RegularExpressions.Regex.IsMatch(usernamePath, @"[\/\\:\*\?""\<\>\|]"))
      {
        return false;
      }
      return true;
    }

    public static bool ValidateAvatarDir(string usernamePath = "", string avatarName = "")
    {
      if (string.IsNullOrEmpty(avatarName)) return false;
      if (System.Text.RegularExpressions.Regex.IsMatch(avatarName, @"[\/\\:\*\?""\<\>\|]"))
      {
        return false;
      }
      if (UserDirExists($"{usernamePath}/VRC Avatars/VRC Avatar - {avatarName}"))
      {
        return false;
      }
      return true;
    }

    public static string GetAvatarWorkspacePath(string usernamePath = "", string avatarName = "")
    {
      return $"Assets/{usernamePath}/VRC Avatars/VRC Avatar - {avatarName}";
    }

    // Utility function to create a folder if it doesn't exist, given a parent path and folder name
    public static void CreateSafeFolder(string path, string folderName)
    {
      if (!AssetDatabase.IsValidFolder($"{path}/{folderName}"))
      {
        AssetDatabase.CreateFolder(path, folderName);
      }
    }

    public static void MaterialSafeUnlock(Material material)
    {
      Debug.Log($"Shader name of material \"{material.name}\" is \"{material.shader.name}\".");
      if (material != null && material.shader != null && material.shader.name.Contains("Locked/.poiyomi"))
      {
        Thry.ThryEditor.ShaderOptimizer.UnlockMaterials(new Material[] { material });
        Debug.Log($"Material \"{material.name}\" was unlocked.");
      }
    }

    public static void UnlockMaterialsInFolder(string folderPath)
    {
      // Only unlock materials if material's shader name contains "Locked" (Poiyomi)
      string[] materialGuids = AssetDatabase.FindAssets("t:Material", new[] { folderPath });
      foreach (string guid in materialGuids)
      {
        string matPath = AssetDatabase.GUIDToAssetPath(guid);
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        MaterialSafeUnlock(mat);
      }
    }

    public static bool AvatarBaseDirExists(XaekyAvatarBase avatarBase)
    {
      return AssetDatabase.IsValidFolder(avatarBase.baseDir);
    }

    public static void CreateAvatarWorkspace(string usernamePath = "", string avatarName = "", XaekyAvatarBase avatarBase = null, bool showSceneOnCreation = true)
    {
      var fullAvatarPath = GetAvatarWorkspacePath(usernamePath, avatarName);
      bool generatedWindowsMaterials = false;
      bool avatarBaseExists = false;
      if (avatarBase != null)
      {
        // Check if the avatar base default root path exists
        if (!AvatarBaseDirExists(avatarBase))
        {
          EditorUtility.DisplayDialog("Avatar base not found", $"The base avatar \"{avatarBase.name}\" could not be found in the project. Please make sure to import it before creating your avatar workspace, or select None to proceed without a base.", "OK");
          return;
        }
        avatarBaseExists = true;
      }
      Debug.Log($"Creating avatar workspace at path \"{fullAvatarPath}\". Avatar base selected: {avatarBase?.name ?? "None"}.");
      CreateSafeFolder("Assets", usernamePath);
      CreateSafeFolder($"Assets/{usernamePath}", "VRC Avatars");
      if (AssetDatabase.IsValidFolder(fullAvatarPath))
      {
        EditorUtility.DisplayDialog("Avatar folder already exists", $"The avatar folder \"{fullAvatarPath}\" already exists. Please choose a different avatar name.", "OK");
        return;
      }
      CreateSafeFolder($"Assets/{usernamePath}/VRC Avatars", $"VRC Avatar - {avatarName}");
      CreateSafeFolder(fullAvatarPath, "Textures");
      CreateSafeFolder(fullAvatarPath, "Materials");
      CreateSafeFolder($"{fullAvatarPath}/Materials", "Quest");
      CreateSafeFolder(fullAvatarPath, "Prefabs");
      // Duplicate materials (Windows)
      if (avatarBaseExists)
      {
        string matFoldersPath = $"{avatarBase.baseDir}{avatarBase.defaultMaterials.windows.path}";
        if (AssetDatabase.IsValidFolder(matFoldersPath))
        {
          foreach (var matName in avatarBase.defaultMaterials.windows.names)
          {
            var matPath = $"{matFoldersPath}{matName}.mat";
            if (AssetDatabase.LoadAssetAtPath<Material>(matPath) != null)
            {
              AssetDatabase.CopyAsset(matPath, $"{fullAvatarPath}/Materials/{avatarName}.{matName}.mat");
              Debug.Log($"Material \"{matName}\" copied from \"{matPath}\" to \"{fullAvatarPath}/Materials/{avatarName}.{matName}.mat\".");
              var pathToNewMat = $"{fullAvatarPath}/Materials/{avatarName}.{matName}.mat";
              MaterialSafeUnlock(AssetDatabase.LoadAssetAtPath<Material>(pathToNewMat));
              generatedWindowsMaterials = true;
            }
            else
            {
              Debug.LogWarning($"Material \"{matName}\" not found at path \"{matPath}\". Skipping.");
            }
          }
        }
        // Duplicate materials (Quest)
        if (!string.IsNullOrEmpty(avatarBase.defaultMaterials.quest.path) && avatarBase.defaultMaterials.quest.names != null)
        {
          string questMatFoldersPath = $"{avatarBase.baseDir}{avatarBase.defaultMaterials.quest.path}";
          if (AssetDatabase.IsValidFolder(questMatFoldersPath))
          {
            foreach (var matName in avatarBase.defaultMaterials.quest.names)
            {
              var matPath = $"{questMatFoldersPath}{matName}.mat";
              if (AssetDatabase.LoadAssetAtPath<Material>(matPath) != null)
              {
                AssetDatabase.CopyAsset(matPath, $"{fullAvatarPath}/Materials/Quest/{avatarName}.{matName}.mat");
                Debug.Log($"Quest material \"{matName}\" copied from \"{matPath}\" to \"{fullAvatarPath}/Materials/Quest/{avatarName}.{matName}.mat\".");
              }
              else
              {
                Debug.LogWarning($"Quest material \"{matName}\" not found at path \"{matPath}\". Skipping.");
              }
            }
          }
        }
      }
      // Setup StandaloneWindows scene, if there's a prefab, duplicate and put it in the scene.
      var scenePath = $"{fullAvatarPath}/{avatarName}.main.unity";
      var newScene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.DefaultGameObjects);
      var avatarPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(avatarBaseExists && !string.IsNullOrEmpty(avatarBase.defaultPrefabs.windows) ? $"{avatarBase.baseDir}{avatarBase.defaultPrefabs.windows}" : null);
      if (avatarPrefab != null)
      {
        var prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(avatarPrefab);
        prefabInstance.name = avatarName;
        // Assign the duplicated materials (Body, Head, etc.) to the avatar's prefab body, if we generated them. The GameObject usually is named "Body".
        // use defaultMaterialsWindowsBodyAssignment to determine which material goes to which index in the avatar prefab's material array, if an item of the array is "-1" or null, skip that index. This is to accommodate avatar bases that have more materials than the avatar prefab's body material array, and to allow flexibility in which material gets assigned to which slot.
        // use defaultAvatarBodyRendererPath to find the body renderer in the prefab, if it's defined. If not, cancel operation.
        if (generatedWindowsMaterials)
        {
          if (!string.IsNullOrEmpty(avatarBase.defaultProps.bodyRendererName))
          {
            var bodyRenderer = prefabInstance.transform.Find(avatarBase.defaultProps.bodyRendererName)?.GetComponent<Renderer>();
            if (bodyRenderer != null)
            {
              var newMats = bodyRenderer.sharedMaterials;
              for (int i = 0; i < avatarBase.defaultMaterials.windows.bodyAssignment.Length; i++)
              {
                int matIndex = avatarBase.defaultMaterials.windows.bodyAssignment[i];
                if (matIndex != -1 && matIndex < avatarBase.defaultMaterials.windows.names.Length)
                {
                  string matName = avatarBase.defaultMaterials.windows.names[matIndex];
                  string newMatPath = $"{fullAvatarPath}/Materials/{avatarName}.{matName}.mat";
                  Material newMat = AssetDatabase.LoadAssetAtPath<Material>(newMatPath);
                  if (newMat != null)
                  {
                    newMats[i] = newMat;
                    Debug.Log($"Assigned material \"{newMat.name}\" to slot {i} of the avatar prefab instance's body renderer.");
                  }
                  else
                  {
                    Debug.LogWarning($"Material \"{matName}\" not found at path \"{newMatPath}\". Cannot assign to slot {i} of the avatar prefab instance's body renderer.");
                  }
                }
              }
              bodyRenderer.sharedMaterials = newMats;
            }
          }
        }
        // Save this prefab instance as a new prefab in the avatar workspace prefabs folder
        var newPrefabPath = $"{fullAvatarPath}/Prefabs/{avatarName}.prefab";
        var savedPrefab = PrefabUtility.SaveAsPrefabAsset(prefabInstance, newPrefabPath);
        PrefabUtility.InstantiatePrefab(savedPrefab);
        DestroyImmediate(prefabInstance);
        Debug.Log($"Prefab instance of \"{avatarPrefab.name}\" saved as new prefab at \"{newPrefabPath}\".");
      }
      UnityEditor.SceneManagement.EditorSceneManager.SaveScene(newScene, scenePath);
      // Setup Quest scene, if there's a prefab for it, duplicate and put it in the scene.
      if (avatarBaseExists && !string.IsNullOrEmpty(avatarBase.defaultPrefabs.quest))
      {
        var questScenePath = $"{fullAvatarPath}/{avatarName}.quest.unity";
        var newQuestScene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.DefaultGameObjects);
        var avatarQuestPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{avatarBase.baseDir}{avatarBase.defaultPrefabs.quest}");
        if (avatarQuestPrefab != null)
        {
          var prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(avatarQuestPrefab);
          prefabInstance.name = $"{avatarName} Quest";
          // Save this prefab instance as a new prefab in the avatar workspace prefabs folder
          var newPrefabPath = $"{fullAvatarPath}/Prefabs/{avatarName} Quest.prefab";
          var savedPrefab = PrefabUtility.SaveAsPrefabAsset(prefabInstance, newPrefabPath);
          PrefabUtility.InstantiatePrefab(savedPrefab);
          DestroyImmediate(prefabInstance);
          Debug.Log($"Quest prefab instance of \"{avatarQuestPrefab.name}\" saved as new prefab at \"{newPrefabPath}\".");
        }
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(newQuestScene, questScenePath);
      }
      if (showSceneOnCreation)
      {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath);
        ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath)); // Focus the project window on the new scene asset
      }
    }
  }

  public class XaekyAvatarFormState
  {
    public string Username = "";
    public bool UsernameIsEmpty = true;
    public bool UsernameDirExists = false;
    public bool UsernameIsValid = false;
    public string AvatarName = "";
    public bool AvatarNameIsEmpty = true;
    public bool AvatarNameIsValid = false;
    public XaekyAvatarBase AvatarBase = null;
    public bool ShowSceneOnCreation = true;
  }

  public class XaekyCustomAvatarSetupWindow : XaekyWizardWindow
  {
    private XaekyAvatarFormState formState = new XaekyAvatarFormState();
    public static string HeaderLabelRaw => "Avatar Workspace Setup";
    protected override string HeaderLabel => HeaderLabelRaw;
    [MenuItem("Xaeky/Avatar Workspace Setup")]
    public static void Init()
    {
      GetWindow<XaekyCustomAvatarSetupWindow>(HeaderLabelRaw);
    }

    protected override void DrawContent()
    {
      // Setup vars
      string[] avatarBases = new string[XaekyAvatarBase.avatarBases.Length + 1];
      avatarBases[0] = "None";
      for (int i = 0; i < XaekyAvatarBase.avatarBases.Length; i++)
      {
        avatarBases[i + 1] = XaekyAvatarBase.avatarBases[i].name;
      }
      int selectedBaseIndex = formState.AvatarBase == null ? 0 : System.Array.IndexOf(XaekyAvatarBase.avatarBases, formState.AvatarBase) + 1;
      // Draw content
      GUILayout.BeginArea(new Rect(16, 80, position.width - 32, position.height - 112));
      GUILayout.Label("This will create a folder with your name in the Assets directory, and set up a basic avatar workspace structure for you to work with.", EditorStyles.wordWrappedLabel);
      GUILayout.Space(16);
      XaekyReactive.TextField("Username", formState.Username, (v) => populateFieldsFromState(nameof(formState.Username), v));
      if (!formState.UsernameIsEmpty && !formState.UsernameIsValid)
      {
        EditorGUILayout.HelpBox($"The username '{formState.Username}' contains invalid characters.", MessageType.Error);
      }
      if (!formState.UsernameIsEmpty && formState.UsernameDirExists)
      {
        EditorGUILayout.HelpBox($"The username '{formState.Username}' already exists.", MessageType.Info);
      }
      XaekyReactive.TextField("Avatar Name", formState.AvatarName, (v) => populateFieldsFromState(nameof(formState.AvatarName), v));
      if (!formState.AvatarNameIsEmpty && !formState.AvatarNameIsValid)
      {
        EditorGUILayout.HelpBox($"The avatar name '{formState.AvatarName}' contains invalid characters or already exists in the user folder.", MessageType.Error);
      }
      int newSelectedBaseIndex = EditorGUILayout.Popup("Avatar Base", selectedBaseIndex, avatarBases);
      if (newSelectedBaseIndex != selectedBaseIndex)
      {
        formState.AvatarBase = newSelectedBaseIndex == 0 ? null : XaekyAvatarBase.avatarBases[newSelectedBaseIndex - 1];
      }
      if (formState.AvatarBase != null && !XaekyCustomAvatarSetup.AvatarBaseDirExists(formState.AvatarBase))
      {
        EditorGUILayout.HelpBox($"The base avatar \"{formState.AvatarBase.name}\" could not be found in the project. Please make sure to import it before creating your avatar workspace, or select None to proceed without a base.", MessageType.Warning);
      }
      GUILayout.Space(16);
      if (!formState.UsernameIsEmpty && !formState.AvatarNameIsEmpty && formState.UsernameIsValid && formState.AvatarNameIsValid)
      {
        var avatarPath = XaekyCustomAvatarSetup.GetAvatarWorkspacePath(formState.Username, formState.AvatarName);
        EditorGUILayout.HelpBox($"Avatar folder will be created at \"{avatarPath}\".", MessageType.Info);
      }
      formState.ShowSceneOnCreation = EditorGUILayout.ToggleLeft("Show scene on creation", formState.ShowSceneOnCreation);
      using (new EditorGUI.DisabledScope(formState.UsernameIsEmpty || formState.AvatarNameIsEmpty || !formState.UsernameIsValid || !formState.AvatarNameIsValid))
      {
        if (GUILayout.Button("Create Avatar Workspace"))
        {
          XaekyCustomAvatarSetup.CreateAvatarWorkspace(formState.Username, formState.AvatarName, formState.AvatarBase, formState.ShowSceneOnCreation);
          formState.AvatarName = "";
          formState.AvatarNameIsEmpty = true;
          formState.AvatarNameIsValid = false;
          GUIUtility.ExitGUI();
          this.Repaint();
        }
      }
      GUILayout.EndArea();
    }

    private void populateFieldsFromState(string propName, string propValue)
    {
      typeof(XaekyAvatarFormState).GetField(propName)?.SetValue(formState, propValue);
      OnUsernameFieldChanged(formState.Username);
      OnAvatarNameFieldChanged(formState.AvatarName);
    }

    private void OnUsernameFieldChanged(string newValue)
    {
      var newUsername = newValue.Trim();
      formState.UsernameIsValid = XaekyCustomAvatarSetup.ValidateUsernameDir(newUsername);
      formState.UsernameIsEmpty = string.IsNullOrEmpty(newUsername);
      formState.UsernameDirExists = XaekyCustomAvatarSetup.UserDirExists(newUsername);
    }

    private void OnAvatarNameFieldChanged(string newValue)
    {
      var newAvatarName = newValue.Trim();
      formState.AvatarNameIsValid = XaekyCustomAvatarSetup.ValidateAvatarDir(formState.Username, newAvatarName);
      formState.AvatarNameIsEmpty = string.IsNullOrEmpty(newAvatarName);
    }
  }
}