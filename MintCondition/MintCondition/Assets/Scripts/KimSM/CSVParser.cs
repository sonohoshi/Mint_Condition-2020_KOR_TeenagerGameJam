using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVParser : MonoBehaviour
{
    private static readonly string mapCSVPath = "CSVs/map_";

    public static string[] GetMapFile(int stage)
    {
        string path = mapCSVPath + stage.ToString();
        var ta = Resources.Load(path) as TextAsset;
        return ta.text.Split('\n');
    }
}
