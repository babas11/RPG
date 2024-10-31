using System.IO;
using System.Text;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            FileStream stream = File.Open(path,FileMode.Create);
            byte[] byteArray = Encoding.UTF8.GetBytes("Hello World");
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();
        }

        public void Load(string saveFile)
        {
            print("Loading from " + GetPathFromSaveFile(saveFile));
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}