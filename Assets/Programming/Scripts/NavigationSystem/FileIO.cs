using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class FileIO
{
    static bool buildMode = true;


    const string _testPath = "Assets/Design/Resources/Data/GameSave/";
    const string _path = "Abyssian_Data/StreamingAssets/GameSave/";


    public enum WriteMode
    {
        Append,
        Overwrite,
    }

    public static void SaveToFile(string scene, string id, string val)
    {
        string path = (buildMode) ? _path : _testPath;

        try
        {
            using (StreamReader sr = new StreamReader(path + scene + "/" + id + ".txt"))
            {
                //Read each line
                string line;
                int count = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if(line.Length != 0)
                    {
                        if (val == line) //If line exists, overwrite it
                        {
                            string[] lines = File.ReadAllLines(path + scene + "/" + id + ".txt");
                            lines[count] = val;
                            File.WriteAllLines(path + scene + "/" + id + ".txt", lines);
                            break;
                        }
                    }
                    count++;
                }
            }
        }
        catch (Exception e) //File does not exist
        {
            //Create proper directory and file
            DirectoryInfo directoryInfo = Directory.CreateDirectory(path + scene);
            FileStream fileStream = File.Create(path + scene + "/" + id + ".txt");
            fileStream.Close();
            //

            //Write the data to file
            using (StreamWriter sw = new StreamWriter(path + scene + "/" + id + ".txt"))
            {
                sw.WriteLine(val);
            }
            //
        }
    }

    public static void SaveConnectorToFile(string scene, string connectorName, string data)
    {
        string path = (buildMode) ? "Abyssian_Data/StreamingAssets/ConnectorData/" : "Assets/Design/Resources/Data/ConnectorData/";

        //Create proper directory and file
        DirectoryInfo directoryInfo = Directory.CreateDirectory(path + scene);
        FileStream fileStream = File.Create(path + scene + "/" + connectorName + ".txt");
        fileStream.Close();

        //Write the data to file
        using (StreamWriter sw = new StreamWriter(path + scene + "/" + connectorName + ".txt"))
        {
            sw.WriteLine(data);
        }
    }

    public static void SaveToFile(string scene, string id, bool val)
    {
        SaveToFile(scene, id, val.ToString());
    }

    public static void SaveToFile(string scene, string id, int val)
    {
        SaveToFile(scene, id, val.ToString());
    }

    public static void SaveToFile(string scene, string id, float val)
    {
        SaveToFile(scene, id, val.ToString());
    }

    public static string LoadFromFile(string scene, string id)
    {
        string path = (buildMode) ? _path : _testPath;

        try
        {
            using (StreamReader sr = new StreamReader(path + scene + "/" + id + ".txt"))
            {
                //Read each line
                string line;
                int count = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length != 0)
                    {
                        return line;
                    }
                    count++;
                }
            }
        }
        catch (Exception e) //File does not exist
        {
            return null;
        }
        return null;
    }

    public static string LoadConnectorFromFile(string scene, string connectorName)
    {
        string path = (buildMode) ? "Abyssian_Data/StreamingAssets/ConnectorData/" : "Assets/Design/Resources/Data/ConnectorData/";

        try
        {
            using (StreamReader sr = new StreamReader(path + scene + "/" + connectorName + ".txt"))
            {
                //Read each line
                string line;
                int count = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length != 0)
                    {
                        return line;
                    }
                    count++;
                }
            }
        }
        catch (Exception e) //File does not exist
        {
            return null;
        }
        return null;
    }

    //public static void ImportConnectorDataFromFile()
    //{
    //    try
    //    {
    //        using (StreamReader sr = new StreamReader("Assets/Design/Resources/Data/Connector_Data.txt"))
    //        {
    //            //Read each line
    //            string line;
    //            int count = 0;
    //            while ((line = sr.ReadLine()) != null)
    //            {
    //                SceneConnector.SceneConnectorData data = new SceneConnector.SceneConnectorData();
    //                data.FromString(line);

    //                if (!SceneConnectorRegistry.Contains(data.ID))
    //                {
    //                    SceneConnectorRegistry.Add(data);
    //                    SaveSystem.Save(data.name + data.sceneName, "", data.sceneName, data.ToString(), SaveSystem.SaveType.CONNECTOR);
    //                    count++;
    //                }
    //            }
    //            Debug.Log("Import Connector Data : Found " + count + " new Scene Connectors");
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log("The file could not be read: " + e.Message);
    //    }
    //}

    //public static void ExportConnectorDataFromFile()
    //{

    //    //Clear the file contents
    //    FileStream fileStream = File.Open("Assets/Design/Resources/Data/Connector_Data.txt", FileMode.Open);
    //    fileStream.SetLength(0);
    //    fileStream.Close();
    //    //

    //    //Export Data
    //    List<SceneConnector.SceneConnectorData> data = SceneConnectorRegistry.GetRegistry();

    //    using (StreamWriter sw = new StreamWriter("Assets/Design/Resources/Data/Connector_Data.txt"))
    //    {
    //        foreach (SceneConnector.SceneConnectorData connector in data)
    //        {
    //            sw.WriteLine(connector.ToString());
    //        }

    //        Debug.Log("Export Connector Data : " + data.Count + " Connectors Successfully Exported to Text");
    //    }
    //}

    public static void ExportRespawnInfoToFile(Transform playerTransform, string scene)
    {
        SaveSystem.RespawnInfo_Data data = new SaveSystem.RespawnInfo_Data(playerTransform, scene);
        string path = (buildMode) ? _path : _testPath;

        //Create proper directory and file
        DirectoryInfo directoryInfo = Directory.CreateDirectory(path + "RESPAWN");
        FileStream fileStream = File.Create(path + "RESPAWN/respawn.txt");
        fileStream.Close();

        //Write the data to file
        using (StreamWriter sw = new StreamWriter(path + "RESPAWN/respawn.txt"))
        {
            sw.WriteLine(data.ToString());
        }
    }

    public static string FetchRespawnInfo()
    {
        string path = (buildMode) ? _path : _testPath;
        try
        {
            using (StreamReader sr = new StreamReader(path + "RESPAWN/respawn.txt"))
            {
                //Read each line
                string line;
                int count = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length != 0)
                    {
                        return line;
                    }
                    count++;
                }
            }
        }
        catch (Exception e) //File does not exist
        {
            return "";
        }
        return "";
    }

    public static void ClearAllSavedData()
    {
        string path = (buildMode) ? "Abyssian_Data/StreamingAssets/GameSave" : "Assets/Design/Resources/Data/GameSave";
        DirectoryInfo SaveDir = new DirectoryInfo(path);

        foreach (FileInfo file in SaveDir.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in SaveDir.GetDirectories())
        {
            dir.Delete(true);
        }
    }
}
