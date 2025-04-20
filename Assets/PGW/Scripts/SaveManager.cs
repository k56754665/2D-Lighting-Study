using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private const string SAVE_POINT_INDEX_KEY = "SavePointIndex";
    private const string SAVE_POINT_POS_X_KEY = "SavePointPosX";
    private const string SAVE_POINT_POS_Y_KEY = "SavePointPosY";
    private const string SAVE_POINT_POS_Z_KEY = "SavePointPosZ";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetSave();
        }
    }

    public void SaveGame(int savePointIndex, Vector3 position)
    {
        PlayerPrefs.SetInt(SAVE_POINT_INDEX_KEY, savePointIndex);
        PlayerPrefs.SetFloat(SAVE_POINT_POS_X_KEY, position.x);
        PlayerPrefs.SetFloat(SAVE_POINT_POS_Y_KEY, position.y);
        PlayerPrefs.SetFloat(SAVE_POINT_POS_Z_KEY, position.z);
        PlayerPrefs.Save();
    }

    public bool LoadGame(out int savePointIndex, out Vector3 position)
    {
        if (PlayerPrefs.HasKey(SAVE_POINT_INDEX_KEY))
        {
            savePointIndex = PlayerPrefs.GetInt(SAVE_POINT_INDEX_KEY);
            position = new Vector3(
                PlayerPrefs.GetFloat(SAVE_POINT_POS_X_KEY),
                PlayerPrefs.GetFloat(SAVE_POINT_POS_Y_KEY),
                PlayerPrefs.GetFloat(SAVE_POINT_POS_Z_KEY)
            );
            return true;
        }
        savePointIndex = 0;
        position = Vector3.zero;
        return false;
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey(SAVE_POINT_INDEX_KEY);
        PlayerPrefs.DeleteKey(SAVE_POINT_POS_X_KEY);
        PlayerPrefs.DeleteKey(SAVE_POINT_POS_Y_KEY);
        PlayerPrefs.DeleteKey(SAVE_POINT_POS_Z_KEY);
        PlayerPrefs.Save();
    }

}