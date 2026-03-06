public class XaekyAvatarBase
{
  public string id;
  public string name;
  public string author;
  public string defaultBaseDir;
  public string defaultPrefabWindowsPath;
  public string defaultPrefabAndroidPath;
  public string defaultMaterialsDir;
  public string defaultMaterialsMobileDir;
  public string[] defaultMaterialsNames;
  public string[] defaultMaterialsMobileNames;

  public static XaekyAvatarBase[] avatarBases = new XaekyAvatarBase[] {
    new XaekyAvatarBase {
      id = "novabeast",
      name = "Novabeast",
      author = "Kittomatic",
      defaultBaseDir = "Assets/Novabeast_V1_2/Novabeast/",
      defaultPrefabWindowsPath = null,
      defaultPrefabAndroidPath = null,
      defaultMaterialsDir = "Materials/Poiyomi/",
      defaultMaterialsMobileDir = null,
      defaultMaterialsNames = new string[] { "blushMat", "bodyMat", "eyesMat", "fluffMat", "visorMat" },
      defaultMaterialsMobileNames = null
    },
    new XaekyAvatarBase {
      id = "novabeast_memphis",
      name = "Novabeast (Memphis Edit)",
      author = "Kittomatic",
      defaultBaseDir = "Assets/Xaeky/VRC Avatar Edit - Memphis Novabeast/",
      defaultPrefabWindowsPath = "Prefabs/VRC Memphis Novabeast Avatar.prefab",
      defaultPrefabAndroidPath = "Prefabs/VRC Memphis Novabeast Quest Avatar.prefab",
      defaultMaterialsDir = "Materials/",
      defaultMaterialsMobileDir = "Materials/Quest/",
      defaultMaterialsNames = new string[] { "Body", "Eyes", "Fluff", "GogglesLens" },
      defaultMaterialsMobileNames = new string[] { "BodyQ", "EyesQ", "FluffQ", "GogglesLensQ" }
    },
    new XaekyAvatarBase {
      id = "mayu_tora",
      name = "Mayu (Tora)",
      author = "AzukiTiger",
      defaultBaseDir = "Assets/AzukiTiger/VRC Mayu Avatar Bundle/",
      defaultPrefabWindowsPath = "VRC Mayu Tora Avatar PC Prefab.prefab",
      defaultPrefabAndroidPath = "VRC Mayu Tora Avatar Quest Prefab.prefab",
      defaultMaterialsDir = "VRC Mayu Avatar/Materials/",
      defaultMaterialsMobileDir = "VRC Mayu Avatar/Materials Quest/",
      defaultMaterialsNames = new string[] { "Mayu Tora Body", "Mayu Tora Head" },
      defaultMaterialsMobileNames = new string[] { "Mayu Tora Body Q", "Mayu Tora Head Q" }
    },
    new XaekyAvatarBase {
      id = "mayu_oyama",
      name = "Mayu (Oyama)",
      author = "AzukiTiger",
      defaultBaseDir = "Assets/AzukiTiger/VRC Mayu Avatar Bundle/",
      defaultPrefabWindowsPath = "VRC Mayu Oyama Avatar PC Prefab.prefab",
      defaultPrefabAndroidPath = "VRC Mayu Oyama Avatar Quest Prefab.prefab",
      defaultMaterialsDir = "VRC Mayu Avatar/Materials/",
      defaultMaterialsMobileDir = "VRC Mayu Avatar/Materials Quest/",
      defaultMaterialsNames = new string[] { "Mayu Oyama Body", "Mayu Oyama Head" },
      defaultMaterialsMobileNames = new string[] { "Mayu Oyama Body Q", "Mayu Oyama Head Q" }
    }
  };
}