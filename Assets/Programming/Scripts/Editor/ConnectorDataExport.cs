using UnityEditor;
using System.Diagnostics;
public class ConnectorDataExport
{
    [MenuItem("MyTools/Initialize Connector Data")]
    public static void InitSaveFolder()
    {
        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");


        FileUtil.CopyFileOrDirectory("Abyssian_Data/StreamingAssets/ConnectorData", path + "/ConnectorData");
    }
}
