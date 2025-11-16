using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyMaterialToChildrenInEditor : MonoBehaviour
{
    public Material materialToApply;
}

[CustomEditor(typeof(ApplyMaterialToChildrenInEditor))]
public class ApplyMaterialToChildrenInEditorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ApplyMaterialToChildrenInEditor script = (ApplyMaterialToChildrenInEditor)target;

        if (GUILayout.Button("Apply Material to All Children"))
        {
            if (script.materialToApply == null)
            {
                Debug.LogError("❌ Material seçilmedi!");
                return;
            }

            Renderer[] renderers = script.GetComponentsInChildren<Renderer>(true);
            Undo.RecordObjects(renderers, "Apply Material to Children");

            foreach (Renderer rend in renderers)
            {
                int count = rend.sharedMaterials.Length;
                Material[] mats = new Material[count];

                for (int i = 0; i < count; i++)
                {
                    // Child için yeni bir instance oluştur
                    mats[i] = new Material(script.materialToApply);
                }

                rend.sharedMaterials = mats;
            }

            Debug.Log("✔ Material tüm child objelere uygulandı!");
        }
    }
}
