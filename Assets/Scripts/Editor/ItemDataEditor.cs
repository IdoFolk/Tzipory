using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Tzipory.ConfigFiles.Item;
using UnityEditor;
using UnityEngine;

public class ItemDataEditor : OdinMenuEditorWindow
{
    private const string ITEM_CONFIGS_NECKLACE_PATH = "Assets/Resources/ScriptableObjects/ItemConfigs/Necklace";
    private const string ITEM_CONFIGS_EARRING_PATH = "Assets/Resources/ScriptableObjects/ItemConfigs/Earring";
    private const string ITEM_CONFIGS_BELT_PATH = "Assets/Resources/ScriptableObjects/ItemConfigs/Belt";
    private const string ITEM_CONFIGS_BRACELET_PATH = "Assets/Resources/ScriptableObjects/ItemConfigs/Bracelet";
    private const string ITEM_CONFIGS_RING_PATH = "Assets/Resources/ScriptableObjects/ItemConfigs/Ring";
    
    private CreateNewItemData  _createNewItemData;
    
    [MenuItem("Tools/Item Editor")]
    private static void OpenWindows()
    {
        var window = GetWindow<ItemDataEditor>();
        window.position = new Rect(200,200,1000,1000);
        window.Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        _createNewItemData = new CreateNewItemData();
        
        tree.Add("Create new item",_createNewItemData);
        tree.AddAllAssetsAtPath("Necklace", ITEM_CONFIGS_NECKLACE_PATH, typeof(ItemConfig));
        tree.AddAllAssetsAtPath("Earring", ITEM_CONFIGS_EARRING_PATH, typeof(ItemConfig));
        tree.AddAllAssetsAtPath("Belt", ITEM_CONFIGS_BELT_PATH, typeof(ItemConfig));
        tree.AddAllAssetsAtPath("Bracelet", ITEM_CONFIGS_BRACELET_PATH, typeof(ItemConfig));
        tree.AddAllAssetsAtPath("Ring", ITEM_CONFIGS_RING_PATH, typeof(ItemConfig));
        
        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        base.OnBeginDrawEditors();
        OdinMenuTreeSelection selection  = MenuTree.Selection;
        
        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();
            
            if (SirenixEditorGUI.ToolbarButton("Delete Item"))
            {
                ItemConfig itemConfig = selection.SelectedValue as ItemConfig;
                
                string  path = AssetDatabase.GetAssetPath(itemConfig);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (_createNewItemData is not null)
            DestroyImmediate(_createNewItemData.ItemConfig);
    }

    private class CreateNewItemData
    {
        public CreateNewItemData()
        {
            ItemConfig  = CreateInstance<ItemConfig>();
            ItemConfig._itemName = "New Item";
        }
        
        [InlineEditor(ObjectFieldMode =  InlineEditorObjectFieldModes.Hidden)]
        public ItemConfig ItemConfig;

        [Button("Save new item")]
        private void CreateNewAsset()
        {
            ItemConfig._objectId = ItemConfig.ItemName.GetHashCode();
            
            switch (ItemConfig.ItemSlot)
            {
                case ItemSlot.Necklace:
                    AssetDatabase.CreateAsset(ItemConfig, $"{ITEM_CONFIGS_NECKLACE_PATH}/{ItemConfig.ItemName}.asset");
                    break;
                case ItemSlot.Earring:
                    AssetDatabase.CreateAsset(ItemConfig, $"{ITEM_CONFIGS_EARRING_PATH}/{ItemConfig.ItemName}.asset");
                    break;
                case ItemSlot.Belt:
                    AssetDatabase.CreateAsset(ItemConfig, $"{ITEM_CONFIGS_BELT_PATH}/{ItemConfig.ItemName}.asset");
                    break;
                case ItemSlot.Bracelet:
                    AssetDatabase.CreateAsset(ItemConfig, $"{ITEM_CONFIGS_BRACELET_PATH}/{ItemConfig.ItemName}.asset");
                    break;
                case ItemSlot.Ring:
                    AssetDatabase.CreateAsset(ItemConfig, $"{ITEM_CONFIGS_RING_PATH}/{ItemConfig.ItemName}.asset");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            AssetDatabase.SaveAssets();
        }
    }
}