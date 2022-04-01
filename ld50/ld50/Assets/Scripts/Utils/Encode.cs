using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Encode {

    public static string Process(string payload)
    {
        var result = new StringBuilder();
        foreach(char c in Convert.ToBase64String(Encoding.UTF8.GetBytes(payload)))
        {
            result.Append((char)(c + 1));
        }
        return result.ToString();
    }

}
