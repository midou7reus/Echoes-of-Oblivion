using UnityEngine;

/// <summary>
/// PlayerController - متحكم الشخصية الرئيسية
/// 
/// شرح الملف:
/// هذا الملف يتحكم في حركة وسلوك اللاعب
/// يستقبل إدخالات من InputManager ويحولها إلى حركات في اللعبة
/// 
/// المسؤوليات:
/// 1. استقبال إدخالات الحركة من InputManager
/// 2. تحريك الشخصية في الاتجاه المطلوب
/// 3. إدارة الحالات (عادي، جري، قتال، إلخ)
/// 4. إرسال معلومات الموقع للكاميرا
/// </summary>
public class PlayerController : MonoBehaviour
{
    // ======================== المتغيرات العامة ========================
    
    [Header("الحركة")]
    [SerializeField] private float normalSpeed = 5f;      // سرعة الحركة العادية
    [SerializeField] private float runSpeed = 8f;         // سرعة الجري
    [SerializeField] private float acceleration = 10f;    // سرعة التسريع
    [SerializeField] private float deceleration = 8f;     // سرعة التباطؤ
    
    [Header("المراجع")]
    [SerializeField] private Rigidbody2D rb;              // الـ Rigidbody للحركة الفيزيائية
    [SerializeField] private Animator animator;           // الـ Animator للحركات
    [SerializeField] private SpriteRenderer spriteRenderer; // لتغيير اتجاه الشخصية
    
    // ======================== المتغيرات الخاصة ========================
    
    private Vector2 currentVelocity = Vector2.zero;  // السرعة الحالية
    private Vector2 movementInput = Vector2.zero;    // إدخال الحركة
    private float currentSpeed;                       // السرعة المستخدمة حالياً
    private bool facingRight = true;                 // اتجاه الشخصية
    
    // ======================== دورة الحياة ========================
    
    private void Start()
    {
        // التحقق من أن جميع المراجع موجودة
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        
        if (animator == null)
            animator = GetComponent<Animator>();
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        // ربط أحداث InputManager
        InputManager.Instance.OnMove += HandleMovement;
    }
    
    private void Update()
    {
        // هنا نعدّل اتجاه الشخصية حسب اتجاه الحركة
        UpdateDirection();
        
        // تحديث قيمة السرعة الحالية
        UpdateSpeed();
    }
    
    private void FixedUpdate()
    {
        // تطبيق الحركة على الـ Rigidbody
        // FixedUpdate يُستخدم للفيزياء
        ApplyMovement();
    }
    
    private void OnDestroy()
    {
        // فصل الـ Event عند حذف الكائن
        if (InputManager.Instance != null)
            InputManager.Instance.OnMove -= HandleMovement;
    }
    
    // ======================== دوال الحركة ========================
    
    /// <summary>
    /// HandleMovement - معالج إدخال الحركة من InputManager
    /// يتم استدعاؤها عندما يضغط اللاعب على أزرار الحركة
    /// </summary>
    private void HandleMovement(Vector2 input)
    {
        movementInput = input;
    }
    
    /// <summary>
    /// UpdateSpeed - تحديث السرعة المستخدمة
    /// تعتمد على ما إذا كان اللاعب يجري أم لا
    /// </summary>
    private void UpdateSpeed()
    {
        // إذا كان اللاعب يجري، استخدم سرعة الجري، وإلا استخدم السرعة العادية
        currentSpeed = InputManager.Instance.GetIsRunning() ? runSpeed : normalSpeed;
    }
    
    /// <summary>
    /// UpdateDirection - تحديث اتجاه الشخصية
    /// إذا كانت تتحرك لليسار، اقلب الصورة والعكس
    /// </summary>
    private void UpdateDirection()
    {
        // إذا كان هناك إدخال حركة
        if (movementInput.x != 0)
        {
            // إذا تتحرك لليمين ولكن الشخصية تنظر لليسار، اقلب
            if (movementInput.x > 0 && !facingRight)
            {
                Flip();
            }
            // إذا تتحرك لليسار ولكن الشخصية تنظر لليمين، اقلب
            else if (movementInput.x < 0 && facingRight)
            {
                Flip();
            }
        }
    }
    
    /// <summary>
    /// Flip - قلب اتجاه الشخصية
    /// يقلب الصورة ويغير حالة facingRight
    /// </summary>
    private void Flip()
    {
        facingRight = !facingRight;
        // تقليب الـ Sprite من خلال تغيير scale.x
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    /// <summary>
    /// ApplyMovement - تطبيق الحركة الفعلية
    /// يتم استدعاؤها في FixedUpdate للدقة الفيزيائية
    /// </summary>
    private void ApplyMovement()
    {
        // الحركة الناعمة (Smooth Movement)
        // بدلاً من القفز مباشرة للسرعة القصوى،
        // نزيد السرعة تدريجياً (تسريع) أو نقللها (تباطؤ)
        
        // الحساب: إذا كان هناك إدخال حركة، زد السرعة نحوها
        // وإلا قلل السرعة تدريجياً
        
        if (movementInput.magnitude > 0)
        {
            // الاتجاه المطلوب = إدخال الحركة × السرعة الحالية
            Vector2 targetVelocity = movementInput * currentSpeed;
            
            // إضافة التسريع للوصول للسرعة الهدف بشكل ناعم
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // لا يوجد إدخال، قلل السرعة تدريجياً
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }
        
        // تطبيق السرعة على الـ Rigidbody
        rb.velocity = currentVelocity;
        
        // تحديث الـ Animator
        // إذا كان هناك حركة، شغل animation الحركة
        animator.SetBool("isMoving", movementInput.magnitude > 0);
        animator.SetBool("isRunning", InputManager.Instance.GetIsRunning() && movementInput.magnitude > 0);
    }
    
    // ======================== دوال مساعدة ========================
    
    /// <summary>
    /// GetCurrentVelocity - الحصول على السرعة الحالية
    /// تُستخدم من قبل الكاميرا والأنظمة الأخرى
    /// </summary>
    public Vector2 GetCurrentVelocity() => currentVelocity;
    
    /// <summary>
    /// GetPosition - الحصول على موقع الشخصية
    /// تُستخدم من قبل الكاميرا للتابع
    /// </summary>
    public Vector3 GetPosition() => transform.position;
    
    /// <summary>
    /// IsFacingRight - هل الشخصية تنظر لليمين؟
    /// </summary>
    public bool IsFacingRight() => facingRight;
}
