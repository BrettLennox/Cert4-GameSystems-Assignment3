using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    string path0 = "Assets/Resources/Save/CharacterPosition0.txt";
    string path1 = "Assets/Resources/Save/CharacterPosition1.txt";

    public void SaveGame(int index) // saves to the file depending on the passed in index
    {
        string myPath = index == 0 ? path0 : path1;
        StreamWriter writer = new StreamWriter(myPath, false);

        writer.WriteLine("position:" +  _player.transform.position);
        writer.WriteLine("rotation:" + _player.transform.rotation);

        writer.Close();
#if UNITY_EDITOR
        AssetDatabase.ImportAsset(myPath);
#endif
    }

    public void LoadGame(int index) // loads a file based on the passed in index
    {
        string myPath = index == 0 ? path0 : path1;
        StreamReader reader = new StreamReader(myPath);
        string line;
        while ((line = reader.ReadLine()) != null && line != "")
        {
            string[] parts = line.Split(':');

            switch(parts[0])
            {
                case "position":
                    ProcessPositions(parts);
                    _player.transform.position = ProcessPositions(parts);
                    break;
                case "rotation":
                    ProcessPositions(parts);
                    _player.transform.rotation = ProcessQuaternion(parts);
                    break;
            }
        }
        reader.Close();
    }

    public Vector3 ProcessPositions(string[] name) //splits the line of data in the txt file so the data can be used according to its line
    {
        name[1] = name[1].Replace("(", "");
        name[1] = name[1].Replace(")", "");

        string[] numbers = name[1].Split(',');
        float x = float.Parse(numbers[0]);
        float y = float.Parse(numbers[1]);
        float z = float.Parse(numbers[2]);

        Vector3 newPos = new Vector3(x, y, z);
        return newPos;
    }


    public Quaternion ProcessQuaternion(string[] name) //splits the line of data in the txt file so the data can be used according to its line
    {
        name[1] = name[1].Replace("(", "");
        name[1] = name[1].Replace(")", "");

        string[] numbers = name[1].Split(',');
        float x = float.Parse(numbers[0]);
        float y = float.Parse(numbers[1]);
        float z = float.Parse(numbers[2]);
        float w = float.Parse(numbers[3]);

        Quaternion newPos = new Quaternion(x, y, z, w);
        return newPos;
    }  
}
