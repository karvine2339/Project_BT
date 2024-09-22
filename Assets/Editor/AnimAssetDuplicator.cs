using System.IO;
using UnityEditor;
using UnityEngine;

public class AnimAssetDuplicator : MonoBehaviour
{
    [MenuItem("Assets/Animation/Duplicate Animation Assets &d", false, 0)]
    public static void DuplicateAnimationAssetsInThisFolder()
    {
        foreach (var gameObject in Selection.gameObjects)
        {
            string path = AssetDatabase.GetAssetPath(gameObject);
            Object[] objects = AssetDatabase.LoadAllAssetsAtPath(path);
            foreach (Object obj in objects)
            {
                DuplicateAnimationClip(obj as AnimationClip);
            }
        }
    }

    private static void DuplicateAnimationClip(AnimationClip sourceClip)
    {
        if (sourceClip != null && !sourceClip.empty && !sourceClip.name.Contains("preview"))
        {
            string path = AssetDatabase.GetAssetPath(sourceClip);
            path = Path.Combine(Path.GetDirectoryName(path), sourceClip.name) + ".anim";
            // 중복인 경우 유니크한 파일레이블 설정 - 주석처리
            //string newPath = AssetDatabase.GenerateUniqueAssetPath(path);
            // 복제할 경로 재설정 (기존 파일 오버라이트)
            string newPath = path.Replace("_Artworks", "Prefabs");
            AnimationClip newClip = new AnimationClip();
            EditorUtility.CopySerialized(sourceClip, newClip);
            AssetDatabase.CreateAsset(newClip, newPath);
        }
    }
}
