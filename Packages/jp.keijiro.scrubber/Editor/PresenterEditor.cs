using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Scrubber
{
    [CustomEditor(typeof(Presenter))]
    class PresenterEditor : Editor
    {
        ReorderableList _deckList;

        void OnEnable()
        {
            _deckList = new ReorderableList(
                serializedObject, serializedObject.FindProperty("_decks"),
                true, true, true, true
            );
            _deckList.drawHeaderCallback = DrawHeader;
            _deckList.drawElementCallback = DrawElement;
            _deckList.onRemoveCallback = OnRemoveElement;
        }

        void DrawHeader(Rect rect)
        {
            rect.xMin += 14;
            rect.y++;
            rect.height = 16;

            GUI.Label(rect, "Deck List", EditorStyles.label);
        }

        void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            rect.y += 2;
            rect.height = EditorGUIUtility.singleLineHeight;

            var e = _deckList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, e, GUIContent.none);
        }

        void OnRemoveElement(ReorderableList list)
        {
            var prop = list.serializedProperty;

            prop.GetArrayElementAtIndex(list.index).objectReferenceValue = null;
            prop.DeleteArrayElementAtIndex(list.index);

            if (list.index >= prop.arraySize - 1)
                list.index = prop.arraySize - 1;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            _deckList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem("GameObject/Scrubber/Presenter", false, 10)]
        static void CreatePresenter()
        {
            var path1 = "Packages/jp.keijiro.scrubber/Runtime/Presenter.prefab";
            var path2 = "Assets/Scrubber/Runtime/Presenter.prefab";

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path1) ??
                         AssetDatabase.LoadAssetAtPath<GameObject>(path2);

            var go = PrefabUtility.InstantiatePrefab(prefab);
            Selection.activeGameObject = (GameObject)go;
        }
    }
}
