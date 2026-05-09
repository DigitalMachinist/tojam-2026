using UnityEditor;
using UnityEngine;

public static class AnimationClipLegacyToggle
{
    [MenuItem("Assets/Mark As Legacy Animation")]
    static void MarkLegacy()
    {
        foreach (var obj in Selection.objects)
        {
            if (obj is AnimationClip clip)
            {
                var so = new SerializedObject(clip);
                so.FindProperty("m_Legacy").boolValue = true;
                so.ApplyModifiedProperties();
            }
        }
    }
}
