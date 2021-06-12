using UnityEngine;

namespace Code.Utils
{
    public class GetJsonData<T>
    {
        public static T GetJsonInfo(string path)
        {
            string json = Resources.Load(path).ToString();

            T myObject = JsonUtility.FromJson<T>(json);

            return myObject;
        }
    }
}