using UnityEditor;
using UnityEngine;

namespace Styles.Common.editor
{
[CustomPropertyDrawer(typeof(Sprite))]
public class SpritePropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        if (prop.objectReferenceValue != null)
        {
            return _textureSize;
        }
        else
        {
            return base.GetPropertyHeight(prop, label);
        }
    }

    private const float _textureSize = 65;


    // TODO: fix width scaling. For example it happens when sprite field is defined in serialized struct
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, prop);

        if (prop.objectReferenceValue != null)
        {
            position.width = EditorGUIUtility.labelWidth;
            GUI.Label(position, prop.displayName);
            
            position.x += position.width;
            position.width = _textureSize;
            position.height = _textureSize;
            
            prop.objectReferenceValue = EditorGUI.ObjectField(position, prop.objectReferenceValue, typeof(Sprite), false);
        }
        else
        {
            EditorGUI.PropertyField(position, prop, label, true);
        }

        EditorGUI.EndProperty();
    }
}
}