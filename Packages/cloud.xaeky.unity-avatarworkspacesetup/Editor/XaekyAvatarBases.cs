namespace Xaeky
{
  public class XaekyAvatarBase
  {
    public string id;
    public string name;
    public string author;
    public string baseDir;
    public DefaultPrefabs defaultPrefabs;
    public DefaultMaterials defaultMaterials;
    public DefaultProps defaultProps;

    public class DefaultPrefabs
    {
      public string windows;
      public string quest;
    }
    public class DefaultProps
    {
      public string bodyRendererName;
    }
    public class DefaultMaterialsItem
    {
      public string path;
      public string[] names;
      public int[] bodyAssignment;
    }
    public class DefaultMaterials
    {
      public DefaultMaterialsItem windows;
      public DefaultMaterialsItem quest;
    }

    public static XaekyAvatarBase[] avatarBases = new XaekyAvatarBase[] {
    new XaekyAvatarBase {
      id = "novabeast",
      name = "Novabeast",
      author = "Kittomatic",
      baseDir = "Assets/Novabeast_V1_2/Novabeast/",
      defaultPrefabs = new DefaultPrefabs {
        windows = null,
        quest = null
      },
      defaultMaterials = new DefaultMaterials {
        windows = new DefaultMaterialsItem {
          path = "Materials/",
          names = new string[] { "blushMat", "bodyMat", "eyesMat", "fluffMat", "visorMat" },
          bodyAssignment = null
        },
        quest = null
      },
      defaultProps = new DefaultProps {
        bodyRendererName = "Body"
      }
    },
    new XaekyAvatarBase {
      id = "novabeast_memphis",
      name = "Novabeast (Memphis Edit)",
      author = "Kittomatic",
      baseDir = "Assets/Xaeky/VRC Avatar Edit - Memphis Novabeast/",
      defaultPrefabs = new DefaultPrefabs {
        windows = "Prefabs/VRC Memphis Novabeast Avatar.prefab",
        quest = "Prefabs/VRC Memphis Novabeast Quest Avatar.prefab"
      },
      defaultMaterials = new DefaultMaterials {
        windows = new DefaultMaterialsItem {
          path = "Materials/",
          names = new string[] { "blushMat", "bodyMat", "eyesMat", "fluffMat", "visorMat" },
          bodyAssignment = null
        },
        quest = new DefaultMaterialsItem {
          path = "Materials/Quest/",
          names = new string[] { "blushMatQ", "bodyMatQ", "eyesMatQ", "fluffMatQ", "visorMatQ" },
          bodyAssignment = null
        }
      },
      defaultProps = new DefaultProps {
        bodyRendererName = "Body"
      }
    },
    new XaekyAvatarBase {
      id = "mayu_tora",
      name = "Mayu (Tora)",
      author = "AzukiTiger",
      baseDir = "Assets/AzukiTiger/VRC Mayu Avatar Bundle/",
      defaultPrefabs = new DefaultPrefabs {
        windows = "VRC Mayu Tora Avatar PC Prefab.prefab",
        quest = "VRC Mayu Tora Avatar Quest Prefab.prefab"
      },
      defaultMaterials = new DefaultMaterials {
        windows = new DefaultMaterialsItem {
          path = "VRC Mayu Avatar/Materials/",
          names = new string[] { "Mayu Tora Body", "Mayu Tora Head" },
          bodyAssignment = new int[] { 1, -1, 0 }
        },
        quest = new DefaultMaterialsItem {
          path = "VRC Mayu Avatar/Materials Quest/",
          names = new string[] { "Mayu Tora Body Q", "Mayu Tora Head Q" },
          bodyAssignment = new int[] { 1, 0 }
        }
      },
      defaultProps = new DefaultProps {
        bodyRendererName = "Body"
      }
    },
    new XaekyAvatarBase {
      id = "mayu_oyama",
      name = "Mayu (Oyama)",
      author = "AzukiTiger",
      baseDir = "Assets/AzukiTiger/VRC Mayu Avatar Bundle/",
      defaultPrefabs = new DefaultPrefabs {
        windows = "VRC Mayu Oyama VRC Avatar PC Prefab.prefab",
        quest = "VRC Mayu Oyama VRC Avatar Quest Prefab.prefab"
      },
      defaultMaterials = new DefaultMaterials {
        windows = new DefaultMaterialsItem {
          path = "VRC Mayu Avatar/Materials/",
          names = new string[] { "Mayu Oyama Body", "Mayu Oyama Head" },
          bodyAssignment = new int[] { 1, -1, 0 }
        },
        quest = new DefaultMaterialsItem {
          path = "VRC Mayu Avatar/Materials Quest/",
          names = new string[] { "Mayu Oyama Body Q", "Mayu Oyama Head Q" },
          bodyAssignment = new int[] { 1, 0 }
        }
      },
      defaultProps = new DefaultProps {
        bodyRendererName = "Body"
      }
    }
  };
  }
}