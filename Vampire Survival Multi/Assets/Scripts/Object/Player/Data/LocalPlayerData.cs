using UnityEditor;
using UnityEngine;

public class LocalPlayerData : PlayerData
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Objects/Player";
    private const string FILE_PATH = "Assets/Resources/Objects/Player/LocalPlayerData.asset";

    private static LocalPlayerData _instance;
    public static LocalPlayerData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<LocalPlayerData>("Objects/Player/LocalPlayerData");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    string[] folders = FILE_DIRECTORY.Split('/');
                    string currentPath = folders[0];

                    for (int i = 1; i < folders.Length; i++)
                    {
                        if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                        {
                            AssetDatabase.CreateFolder(currentPath, folders[i]);
                        }

                        currentPath += "/" + folders[i];
                    }
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<LocalPlayerData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<LocalPlayerData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }
}