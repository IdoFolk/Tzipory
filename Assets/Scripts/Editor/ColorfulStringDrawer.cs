using System.Collections.Generic;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.Tools.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Tzipory.Editor
{
    [CustomPropertyDrawer(typeof(ColorfulStringAttribute))]
    public class ColorfulStringDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string stringValue = property.stringValue;
            // Apply color formatting to occurrences of the word "red"
            stringValue = RegularExpressionsTool.ColorKeyWords(stringValue, new List<string>() {  PopUpTextConfig.NameKeyCode, PopUpTextConfig.ModifierKeyCode, PopUpTextConfig.NewValueKeyCode, PopUpTextConfig.DeltaKeyCode }, Color.yellow);
            Debug.Log("Updfataedaf");
            // Use label field to display the colored string in the inspector
            EditorGUI.LabelField(position, label, new GUIContent(stringValue));
        } 
    }   
}