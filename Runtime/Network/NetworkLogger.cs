using System;
using UnityEngine;

namespace unitils
{
    public static class NetworkLogger
    {
        public static void LogException(Exception ex)
        {
            HttpException httpEx = ex as HttpException;
            if(httpEx != null)
            {
                Debug.LogError(httpEx.ResponseCode + " " + httpEx.Message + Environment.NewLine + httpEx.Response);
            } else {
                Debug.LogException(ex);
            }
        }
    }
}
