using UnityEngine;
using System.Collections;

/// <summary>
/// CameraController - متحكم الكاميرا
/// 
/// شرح الملف:
/// هذا الملف يتحكم في حركة وسلوك الكاميرا في اللعبة
/// الكاميرا تتابع اللاعب وتحاول أن تبقى في موقع جيد
/// 
/// المسؤوليات:
/// 1. متابعة موقع الشخصية الرئيسية
/// 2. إضافة تأثيرات جميلة (Zoom، Shake)
/// 3. تحديد حدود الكاميرا (لا تخرج عن الخريطة)
/// 4. توفير رؤية جيدة للاعب
/// </summary>
public class CameraController : MonoBehaviour
{
    // ======================== المتغيرات العامة ========================
    
    [Header("المتابعة")]
    [SerializeField] private Transform targetToFollow;     // الهدف الذي تتابعه (الشخصية)
    [SerializeField] private float followSmoothness = 5f;  // سرعة تتابع الكاميرا
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // إزاحة من الهدف
    
    [Header("الحدود")]
    [SerializeField] private bool useBounds = true;        // هل نستخدم حدود للخريطة؟
    [SerializeField] private Vector2 boundsMin;            // أقل موقع للكاميرا
    [SerializeField] private Vector2 boundsMax;            // أقصى موقع للكاميرا
    [SerializeField] private float orthographicSize = 5f;  // حجم الكاميرا
    
    [Header("التأثيرات")]
    [SerializeField] private float shakeIntensity = 0.1f;  // شدة الاهتزاز
    [SerializeField] private float shakeDuration = 0.2f;   // مدة الاهتزاز
    
    // ======================== المتغيرات الخاصة ========================
    
    private Camera mainCamera;
    private Vector3 basePosition;                          // الموقع الأساسي للكاميرا
    private bool isShaking = false;                        // هل الكاميرا تهتز؟
    
    // ======================== الـ Singleton Pattern ========================
    
    public static CameraController Instance { get; private set; }
    
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
        
        mainCamera = GetComponent<Camera>();
    }
    
    private void Start()
    {
        // إذا لم نحدد الهدف، ابحث عن اللاعب تلقائياً
        if (targetToFollow == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                targetToFollow = player.transform;
        }
    }
    
    private void LateUpdate()
    {
        // LateUpdate يُستخدم للكاميرا لأنه يتم بعد تحديث الشخصية
        // هذا يضمن أن الكاميرا تتابع الموقع الفعلي للشخصية
        
        if (targetToFollow != null)
        {
            FollowTarget();
        }
    }
    
    // ======================== دوال المتابعة ========================
    
    /// <summary>
    /// FollowTarget - متابعة الهدف (الشخصية)
    /// تحريك الكاميرا نحو الشخصية بشكل ناعم
    /// </summary>
    private void FollowTarget()
    {
        // الموقع المطلوب = موقع الهدف + الإزاحة
        Vector3 desiredPosition = targetToFollow.position + offset;
        
        // تحريك الكاميرا نحو الموقع المطلوب بشكل ناعم
        // Lerp = Linear Interpolation (إدراج خطي)
        // كلما زادت القيمة، كانت الحركة أسرع
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSmoothness * Time.deltaTime);
        
        // تطبيق الحدود إذا كانت مفعلة
        if (useBounds)
        {
            smoothedPosition = ClampCameraPosition(smoothedPosition);
        }
        
        // تطبيق الموقع الجديد للكاميرا
        transform.position = smoothedPosition;
        basePosition = smoothedPosition;
    }
    
    /// <summary>
    /// ClampCameraPosition - تحديد حدود الكاميرا
    /// التأكد من أن الكاميرا لا تخرج عن حدود الخريطة
    /// </summary>
    private Vector3 ClampCameraPosition(Vector3 position)
    {
        // حساب أطراف الكاميرا بناءً على حجمها (orthographicSize)
        float cameraHeight = orthographicSize * 2;
        float cameraWidth = orthographicSize * 2 * mainCamera.aspect;
        
        // تحديد حدود الكاميرا
        float minX = boundsMin.x + cameraWidth / 2;
        float maxX = boundsMax.x - cameraWidth / 2;
        float minY = boundsMin.y + cameraHeight / 2;
        float maxY = boundsMax.y - cameraHeight / 2;
        
        // تطبيق الحدود على الموقع
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        
        return position;
    }
    
    // ======================== تأثيرات الكاميرا ========================
    
    /// <summary>
    /// ShakeCamera - تأثير اهتزاز الكاميرا
    /// يستخدم عند الانفجارات أو الضربات القوية
    /// </summary>
    public void ShakeCamera()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCameraCoroutine());
        }
    }
    
    /// <summary>
    /// ShakeCameraCoroutine - روتين الاهتزاز
    /// يهز الكاميرا بشكل عشوائي لفترة زمنية
    /// </summary>
    private IEnumerator ShakeCameraCoroutine()
    {
        isShaking = true;
        float elapsedTime = 0f;
        
        while (elapsedTime < shakeDuration)
        {
            // إزاحة عشوائية من الموقع الأساسي
            Vector3 randomOffset = Random.insideUnitCircle * shakeIntensity;
            transform.position = basePosition + randomOffset;
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // العودة للموقع الأساسي
        transform.position = basePosition;
        isShaking = false;
    }
    
    /// <summary>
    /// ZoomToSize - تقريب أو إبعاد الكاميرا
    /// يُستخدم عند الأحداث المهمة
    /// </summary>
    public void ZoomToSize(float targetSize, float zoomDuration)
    {
        StartCoroutine(ZoomCoroutine(targetSize, zoomDuration));
    }
    
    /// <summary>
    /// ZoomCoroutine - روتين التقريب
    /// يقرب أو يبعد الكاميرا بشكل ناعم
    /// </summary>
    private IEnumerator ZoomCoroutine(float targetSize, float zoomDuration)
    {
        float startSize = mainCamera.orthographicSize;
        float elapsedTime = 0f;
        
        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / zoomDuration;
            
            // تغيير حجم الكاميرا بشكل ناعم
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, progress);
            orthographicSize = mainCamera.orthographicSize;
            
            yield return null;
        }
        
        mainCamera.orthographicSize = targetSize;
        orthographicSize = targetSize;
    }
    
    // ======================== دوال مساعدة ========================
    
    /// <summary>
    /// SetTarget - تعيين هدف جديد للمتابعة
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        targetToFollow = newTarget;
    }
    
    /// <summary>
    /// SetBounds - تعيين حدود جديدة للخريطة
    /// </summary>
    public void SetBounds(Vector2 min, Vector2 max)
    {
        boundsMin = min;
        boundsMax = max;
    }
}
