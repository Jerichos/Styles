using UnityEditor;
using UnityEngine;

namespace Styles.Game.editor
{
[CustomEditor(typeof(InventoryUI))]
public class InventoryUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var inventoryUI = target as InventoryUI;

        if (!inventoryUI)
        {
            Debug.LogError($"target is not {nameof(inventoryUI)}");
            return;
        }

        if (GUILayout.Button("Force Update"))
        {
            inventoryUI.ForceUpdateEditor();
        }
        
        inventoryUI.UpdateEditor();
        
    }
}
}