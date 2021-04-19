using UnityEditor;
using System.Diagnostics;
//using UnityEngine.Windows;

public class SaveFolderUtil
{
    [MenuItem("MyTools/Initialize Save System")]
    public static void InitSaveFolder()
    {
        // Get filename.;
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");


        FileUtil.CopyFileOrDirectory("Assets/GameSave", path + "/GameSave");
    }

    ////[MenuItem("MyTools/Clear Save Folder")]
    //public static void ClearSaveFolder()
    //{
    //    string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");

    //    if (Directory.Exists(path + "/GameSave"))
    //    {
    //        Directory.Delete(path + "/GameSave");
    //    }

    //    Directory.CreateDirectory(path + "/GameSave");
    //    //FileUtil.cre
    //}
}
