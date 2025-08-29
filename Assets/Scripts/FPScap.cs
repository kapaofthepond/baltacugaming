using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class FPScap : MonoBehaviour
{
    [SerializeField] private int frameRate = 60;

    private void Start()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;
#endif
    }
}

