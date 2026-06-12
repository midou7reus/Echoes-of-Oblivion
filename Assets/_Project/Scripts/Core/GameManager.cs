using UnityEngine;

/// <summary>
/// GameManager - مدير اللعبة الرئيسي
/// 
/// شرح الملف:
/// هذا الملف يدير حالة اللعبة العامة
/// يتحكم في مراحل اللعبة (تشغيل، توقف مؤقت، إلخ)
/// يدير الإحصائيات والبيانات العامة
/// </summary>
public class GameManager : MonoBehaviour
{
    // ======================== الـ Singleton Pattern ========================
    
    public static GameManager Instance { get; private set; }
    
    // ======================== المتغيرات ========================
    
    private bool isPaused = false;
    
    // ======================== دورة الحياة ========================
    
    private void Awake()
    {
        // إنشاء Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        // ربط الـ Events
        InputManager.Instance.OnPause += HandlePause;
    }
    
    private void OnDestroy()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnPause -= HandlePause;
    }
    
    // ======================== دوال إدارة اللعبة ========================
    
    /// <summary>
    /// HandlePause - معالج الـ Pause
    /// </summary>
    private void HandlePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    /// <summary>
    /// PauseGame - توقيف اللعبة مؤقتاً
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // إيقاف الوقت
        Debug.Log("اللعبة متوقفة مؤقتاً");
    }
    
    /// <summary>
    /// ResumeGame - استئناف اللعبة
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // استئناف الوقت
        Debug.Log("اللعبة مستأنفة");
    }
    
    /// <summary>
    /// IsPaused - هل اللعبة متوقفة؟
    /// </summary>
    public bool IsPaused() => isPaused;
}
