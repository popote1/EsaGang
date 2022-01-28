using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Grabable))]
    class GrableEditorScripte : UnityEditor.Editor
    {
        private  Grabable Target;

        

        private void OnSceneGUI()
        {
            Target = (Grabable) target;

            GUIStyle textStyle = new GUIStyle();
            textStyle.fontSize = 4;
            textStyle.normal.textColor = Color.green;
            textStyle.alignment = TextAnchor.MiddleCenter;
            
            Handles.Label(Target.transform.position +Vector3.down*1.3f,Target.transform.position.ToString()  );
            
            
            Handles.color = Color.yellow;
            


        }
    }
}
