using RPG.Core;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.AI;


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
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 vector = (SerializableVector3)state;
            print("Restoring state to " + vector.ToVector());
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = vector.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();

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
