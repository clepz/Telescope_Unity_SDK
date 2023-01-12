using UnityEngine;

public class TelescopeService : MonoBehaviour
{
    private static TelescopeService _instance;

    internal static void Initialize(GameObject go)
    {
        _instance = go.AddComponent<TelescopeService>();
    }
    // Start is called before the first frame update

    internal static bool IsInitialized()
    {
        return _instance != null;
    }

    internal static void Disable()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
