using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHelper
{
    /// <summary>
    /// CHeck if the device is connecting to Internet or not 
    /// </summary>
    /// <returns></returns>
    public static bool isNetworking()
    {
        bool result = Application.internetReachability != NetworkReachability.NotReachable;
        return result;
    }
}
