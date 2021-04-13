using UnityEditor;
using System.Diagnostics;
public class SaveFolderUtil
{
    [MenuItem("MyTools/Initialize Save System")]
    public static void InitSaveFolder()
    {
        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");


        FileUtil.CopyFileOrDirectory("Assets/GameSave", path + "/GameSave");
    }
}
