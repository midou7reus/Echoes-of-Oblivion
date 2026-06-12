using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputManager - نظام إدارة الإدخال الكامل
/// 
/// شرح الملف:
/// هذا الملف يدير جميع أنواع الإدخال (لوحة المفاتيح + جهاز التحكم + الماوس)
/// يحول إدخالات اللاعب إلى أوامر معروفة ويرسلها للمنظومات الأخرى
/// 
/// كيفية العمل:
/// 1. يستمع إلى إدخالات اللاعب باستخدام Input System
/// 2. يحول الإدخال إلى بيانات قياسية (Vector2 للحركة، bool للأكشن)
/// 3. يرسل البيانات للـ Player Controller
/// 4. يدعم Keyboard و Controller و Mouse في نفس الوقت
/// </summary>
public class InputManager : MonoBehaviour
{
    // ======================== المتغيرات الخاصة ========================
    
    /// <summary>
    /// هذا يحتوي على كل الإدخالات الممكنة من اللاعب
    /// يتم إنشاؤه تلقائياً من Input Actions في Unity
    /// </summary>
    private PlayerInputActions playerInputActions;
    
    // متغيرات تخزين الحركة الحالية
    private Vector2 movementInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    private bool isRunning = false;
    private bool isAttacking = false;
    private bool isDodging = false;
    private bool isPaused = false;
    
    // ======================== الـ Events ========================
    
    /// <summary>
    /// Events - أحداث يستمع لها الـ Player والمنظومات الأخرى
    /// عندما يحدث إدخال، نرسل event ليعرفوا المنظومات الأخرى
    /// </summary>
    public delegate void InputAction(Vector2 input);
    public delegate void InputActionBool();
    
    public event InputAction OnMove;
    public event InputAction OnLook;
    public event InputActionBool OnAttack;
    public event InputActionBool OnDodge;
    public event InputActionBool OnPause;
    
    // ======================== الـ Singleton Pattern ========================
    
    /// <summary>
    /// Singleton - نمط يضمن وجود نسخة واحدة فقط من InputManager في اللعبة
    /// يسمح للملفات الأخرى بالوصول إليه بسهولة: InputManager.Instance
    /// </summary>
    public static InputManager Instance { get; private set; }
    
    // ======================== الدوال الحياتية ========================
    
    private void Awake()
    {
        // إنشاء الـ Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        // إنشاء نظام الإدخال
        playerInputActions = new PlayerInputActions();
    }
    
    private void OnEnable()
    {
        // تفعيل نظام الإدخال
        // هذا يسمح باستقبال الإدخالات من اللاعب
        playerInputActions.Player.Enable();
        
        // ربط الدوال برسائل الإدخال
        // عندما يضغط اللاعب زر معين، سيتم تنفيذ الدالة المقابلة
        playerInputActions.Player.Move.performed += OnMovePerformed;
        playerInputActions.Player.Move.canceled += OnMoveCanceled;
        
        playerInputActions.Player.Look.performed += OnLookPerformed;
        playerInputActions.Player.Look.canceled += OnLookCanceled;
        
        playerInputActions.Player.Run.performed += OnRunPerformed;
        playerInputActions.Player.Run.canceled += OnRunCanceled;
        
        playerInputActions.Player.Attack.performed += OnAttackPerformed;
        playerInputActions.Player.Dodge.performed += OnDodgePerformed;
        playerInputActions.Player.Pause.performed += OnPausePerformed;
    }
    
    private void OnDisable()
    {
        // إلغاء تفعيل نظام الإدخال
        // هذا يتم عند إغلاق اللعبة أو تعطيل الكائن
        playerInputActions.Player.Disable();
        
        // فصل جميع الدوال عن رسائل الإدخال
        playerInputActions.Player.Move.performed -= OnMovePerformed;
        playerInputActions.Player.Move.canceled -= OnMoveCanceled;
        
        playerInputActions.Player.Look.performed -= OnLookPerformed;
        playerInputActions.Player.Look.canceled -= OnLookCanceled;
        
        playerInputActions.Player.Run.performed -= OnRunPerformed;
        playerInputActions.Player.Run.canceled -= OnRunCanceled;
        
        playerInputActions.Player.Attack.performed -= OnAttackPerformed;
        playerInputActions.Player.Dodge.performed -= OnDodgePerformed;
        playerInputActions.Player.Pause.performed -= OnPausePerformed;
    }
    
    // ======================== دوال معالجة الإدخال ========================
    
    /// <summary>
    /// معالج الحركة - عندما يضغط اللاعب على أزرار الحركة
    /// callback = البيانات التي يرسلها نظام الإدخال
    /// </summary>
    private void OnMovePerformed(InputAction.CallbackContext callback)
    {
        // نأخذ قيمة الحركة من الإدخال
        // WASD = Vector2 قيمته من -1 إلى 1
        // Left Stick Controller = نفس الفكرة
        movementInput = callback.ReadValue<Vector2>();
        
        // نرسل إشارة للـ Player بالحركة الجديدة
        OnMove?.Invoke(movementInput);
    }
    
    /// <summary>
    /// معالج إيقاف الحركة - عندما يترك اللاعب أزرار الحركة
    /// </summary>
    private void OnMoveCanceled(InputAction.CallbackContext callback)
    {
        // نصفر حركة الشخصية
        movementInput = Vector2.zero;
        OnMove?.Invoke(movementInput);
    }
    
    /// <summary>
    /// معالج النظر - عندما يحرك اللاعب الماوس أو Right Stick
    /// </summary>
    private void OnLookPerformed(InputAction.CallbackContext callback)
    {
        lookInput = callback.ReadValue<Vector2>();
        OnLook?.Invoke(lookInput);
    }
    
    /// <summary>
    /// معالج إيقاف النظر
    /// </summary>
    private void OnLookCanceled(InputAction.CallbackContext callback)
    {
        lookInput = Vector2.zero;
    }
    
    /// <summary>
    /// معالج الجري - عندما يضغط اللاعب على Shift (لوحة المفاتيح) أو LB (Controller)
    /// </summary>
    private void OnRunPerformed(InputAction.CallbackContext callback)
    {
        isRunning = true;
    }
    
    /// <summary>
    /// معالج إيقاف الجري
    /// </summary>
    private void OnRunCanceled(InputAction.CallbackContext callback)
    {
        isRunning = false;
    }
    
    /// <summary>
    /// معالج الهجوم - عندما يضغط اللاعب على Click (ماوس) أو X (Controller)
    /// </summary>
    private void OnAttackPerformed(InputAction.CallbackContext callback)
    {
        isAttacking = true;
        OnAttack?.Invoke();
    }
    
    /// <summary>
    /// معالج التفادي - عندما يضغط اللاعب على Right Click (ماوس) أو B (Controller)
    /// </summary>
    private void OnDodgePerformed(InputAction.CallbackContext callback)
    {
        isDodging = true;
        OnDodge?.Invoke();
    }
    
    /// <summary>
    /// معالج الـ Pause - عندما يضغط اللاعب على ESC (لوحة المفاتيح) أو Start (Controller)
    /// </summary>
    private void OnPausePerformed(InputAction.CallbackContext callback)
    {
        isPaused = !isPaused; // تبديل حالة الـ Pause
        OnPause?.Invoke();
    }
    
    // ======================== دوال الوصول للبيانات ========================
    
    /// <summary>
    /// دوال عامة للحصول على بيانات الإدخال الحالية
    /// تستخدم من قبل الملفات الأخرى مثل PlayerController
    /// </summary>
    
    public Vector2 GetMovementInput() => movementInput;
    public Vector2 GetLookInput() => lookInput;
    public bool GetIsRunning() => isRunning;
    public bool GetIsAttacking() => isAttacking;
    public bool GetIsDodging() => isDodging;
    public bool GetIsPaused() => isPaused;
    
    /// <summary>
    /// دالة لإعادة تعيين حالة الهجوم والتفادي
    /// تُستخدم بعد أن ينفذ الـ Player الفعل
    /// </summary>
    public void ResetAttack() => isAttacking = false;
    public void ResetDodge() => isDodging = false;
}
