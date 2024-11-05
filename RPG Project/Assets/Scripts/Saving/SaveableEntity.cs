using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            //return new SerializableVector3(transform.position);
            foreach(ISaveable saveable in GetComponents<ISaveable>()){
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {

            Dictionary<string,object> stateDict = (Dictionary<string, object>)state;
            foreach(ISaveable saveable in GetComponents<ISaveable>()){
                string typeString = saveable.GetType().ToString();
                if(stateDict.ContainsKey(typeString)){
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
/*             SerializableVector3 vector = (SerializableVector3)state;
            print("Restoring state to " + vector.ToVector());
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = vector.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
 */
        }

#if UNITY_EDITOR
        private void Update()
        {
            //if we are in play mode, we don't want to generate a new GUID
            if (Application.IsPlaying(gameObject)) return;

            //if we are in the editor and the object is not in the scene
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;


            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();

            }

        }
#endif
    }

}
