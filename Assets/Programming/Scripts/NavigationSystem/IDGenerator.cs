using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class creates a unique ID for each connector. It is used to prevent data overwriting in the save system as well as a way to 
//extract connector information from the registry.
public static class IDGenerator
{
    static char[] chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&".ToCharArray();
    static int _length = 5;

    //Generates a random Unique Identifier.
    public static string GenerateUniqueID()
    {
        string id = "";

        while (true)
        {
            for (int i = 0; i < _length; i++)
            {
                int rand = Random.Range(0, chars.Length - 1);
                id += chars[rand];
            }

            if (CheckIsUnique(id)) break;
            else id = "";
        }
        return id;
    }

    //Checks to see if ID currently exists in saved memory. returns true if ID is not already taken.
    public static bool CheckIsUnique(string id)
    {
        return SceneConnectorRegistry.Contains(id) ? false : true;
    }
}
