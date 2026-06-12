# 📖 شرح نظام الإدخال (Input System) - درس 1

## 🎯 مقدمة

نظام الإدخال هو الطريقة التي تتواصل بها لعبتك مع لاعبك.
اللاعب يضغط على أزرار، والكمبيوتر يفهم ماذا يريد ويستجيب.

### ما الفرق بين لوحة المفاتيح والـ Controller والماوس؟

```
┌─────────────────────────────────────────────────────────────┐
│                    نظام الإدخال الموحد                      │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  لوحة المفاتيح          جهاز التحكم          الماوس        │
│  (WASD + Click)      (Left Stick + X)    (Mouse Movement)  │
│       ↓                    ↓                    ↓           │
│  InputManager - يترجم جميع الأنواع إلى أوامر موحدة        │
│       ↓                                                     │
│  PlayerController - يفهم الأوامر ويطبقها                   │
│       ↓                                                     │
│  الشخصية تتحرك في اللعبة                                   │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔧 الملفات الرئيسية

### 1. InputManager.cs

**الدور**: إدارة جميع الإدخالات

```csharp
// الملفات التي استخدمناها:
// using UnityEngine;
// using UnityEngine.InputSystem;

// InputManager يحتوي على:
// 1. playerInputActions - متغير يحتفظ بإعدادات الإدخال
// 2. أحداث (Events) - تخبر الملفات الأخرى عن الإدخالات
// 3. دوال معالجة - تفسر ما فعله اللاعب
```

**المهام الأساسية**:
```
1. استقبال إدخالات من أي جهاز
2. تحويلها إلى أوامر موحدة
3. إرسال Events للملفات الأخرى
4. توفير دوال للوصول للبيانات
```

---

## 📊 كيفية عمل Events

**ما هو الـ Event؟**

الـ Event هو طريقة للتواصل بين الملفات.
مثل المذيع في الراديو - يعلن شيء ما، والجميع يسمعون.

```csharp
// في InputManager - المعلن (Broadcaster)
public delegate void InputAction(Vector2 input);
public event InputAction OnMove;  // حدث الحركة

// عندما يحرك اللاعب الفأرة، نعلن:
OnMove?.Invoke(movementInput);  // "يا جميع من يستمع: اللاعب تحرك!"

// في PlayerController - المستمع (Listener)
InputManager.Instance.OnMove += HandleMovement;  // "سأستمع للحركة"

private void HandleMovement(Vector2 input)
{
    // عندما نسمع الإعلان، نفعل شيء
    movementInput = input;
}
```

---

## ⌨️ الأزرار المدعومة

### لوحة المفاتيح (Keyboard)

```
الحركة:         WASD
  W = أعلى
  A = يسار
  S = أسفل
  D = يمين

الإجراءات:
  Shift = الجري (Run)
  Left Click (ماوس) = الهجوم
  Right Click (ماوس) = التفادي
  Escape = الـ Pause
```

### جهاز التحكم (Gamepad / Controller)

```
الحركة:         Left Stick (العصا اليسرى)

الإجراءات:
  LB = الجري (Run)
  X = الهجوم
  B = التفادي
  Start = الـ Pause
```

### الماوس (Mouse)

```
النظر:          Mouse Movement (حركة الماوس)
الهجوم:         Left Click
التفادي:        Right Click
```

---

## 🎮 مثال عملي: ماذا يحدث عندما يضغط اللاعب على W؟

```
1. اللاعب يضغط W
   ↓
2. Unity Input System يكتشف الضغطة
   ↓
3. يرسل رسالة إلى InputManager
   ↓
4. InputManager.OnMovePerformed يتم استدعاؤه
   ↓
5. يأخذ قيمة الحركة (في هذه الحالة: Vector2(0, 1))
   ↓
6. يرسل Event: OnMove?.Invoke(Vector2(0, 1))
   ↓
7. PlayerController يستقبل الـ Event
   ↓
8. يقول: "الشخصية تريد أن تتحرك لأعلى!"
   ↓
9. يحرك الشخصية لأعلى
   ↓
10. الكاميرا تتابع الشخصية
```

---

## 💻 الكود بالتفصيل

### الجزء الأول: الإعداد (Awake)

```csharp
private void Awake()
{
    // إنشاء Singleton
    // هذا يضمن وجود نسخة واحدة فقط من InputManager
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }
    Instance = this;
    
    // إنشاء نظام الإدخال
    // هذا يعد كل أنواع الإدخال
    playerInputActions = new PlayerInputActions();
}
```

**الشرح**:
- `Singleton` = نمط برمجي يضمن وجود نسخة واحدة فقط
- `Instance` = الطريقة للوصول إلى InputManager من أي مكان: `InputManager.Instance`
- `playerInputActions` = الـ object الذي يحتوي على جميع إعدادات الإدخال

---

### الجزء الثاني: تفعيل الإدخال (OnEnable)

```csharp
private void OnEnable()
{
    // تفعيل نظام الإدخال
    playerInputActions.Player.Enable();
    
    // ربط الأزرار بالدوال
    playerInputActions.Player.Move.performed += OnMovePerformed;
    //                                          ↑
    // عندما يتم الضغط على زر الحركة،
    // استدعِ الدالة OnMovePerformed
}
```

**الشرح**:
- `Enable()` = "استمع إلى الإدخالات"
- `performed += OnMovePerformed` = "عندما يحدث الإدخال، قم بهذا"
- `+=` = "أضف هذه الدالة لقائمة الدوال المراد تنفيذها"

---

### الجزء الثالث: معالجة الإدخال

```csharp
private void OnMovePerformed(InputAction.CallbackContext callback)
{
    // callback = المعلومات عن الإدخال
    
    // نأخذ قيمة الحركة
    movementInput = callback.ReadValue<Vector2>();
    
    // نخبر جميع من يستمعون أن اللاعب تحرك
    OnMove?.Invoke(movementInput);
    //    ↑
    // "?" = تحقق أولاً هل هناك أحد يستمع؟
    // إذا نعم، أخبرهم
}
```

**الشرح**:
- `ReadValue<Vector2>()` = "اقرأ قيمة من نوع Vector2"
- `Vector2` = إحداثي بـ (x, y) من -1 إلى 1
- `OnMove?.Invoke()` = "اعلن عن الحدث"

---

## 🔌 التوصيل في PlayerController

```csharp
private void Start()
{
    // ربط أحداث InputManager
    InputManager.Instance.OnMove += HandleMovement;
    //                              ↑
    // "استمع إلى أحداث الحركة"
}

private void HandleMovement(Vector2 input)
{
    // استقبال البيانات من InputManager
    movementInput = input;
    
    // تطبيق الحركة
    ApplyMovement();
}
```

---

## 🎯 التدريب العملي

### مهمة 1: اطبع قيم الحركة

أضف هذا السطر في `OnMovePerformed`:
```csharp
Debug.Log($"حركة اللاعب: X = {movementInput.x}, Y = {movementInput.y}");
```

افتح Console وشاهد القيم تتغير!

---

### مهمة 2: أضف جري مجاني

جرب بدون الضغط على Shift - هل الشخصية تتحرك؟
نعم! لأن `GetIsRunning()` يرجع false، فتستخدم `normalSpeed`.

عدّل السرعات:
```csharp
[SerializeField] private float normalSpeed = 5f;   // غيره إلى 10
[SerializeField] private float runSpeed = 8f;      // غيره إلى 5
```

الآن الجري أبطأ من المشي - مضحك! 😄

---

## 📝 ملاحظات مهمة

### 1. الفرق بين Performed و Canceled

```
Performed = "تم الضغط على الزر"
Canceled = "تم إطلاق الزر"

       Performed
          ↓
    ┌──────────┐
    │ اضغط هنا │
    └──────────┘
          ↓
       Canceled
```

### 2. Vector2.zero مقابل Vector2.Zero

كلاهما نفس الشيء: `(0, 0)`

### 3. Time.deltaTime

يُستخدم لجعل الحركة مستقلة عن FPS:
```csharp
// بدون Time.deltaTime - سرعة مختلفة على أجهزة مختلفة
position += velocity * 1;

// مع Time.deltaTime - سرعة موحدة
position += velocity * Time.deltaTime;  // صحيح!
```

---

## 🎓 الخلاصة

| المكون | الدور |
|--------|------|
| InputManager | يستقبل الإدخالات ويرسل Events |
| Events | تخبر الملفات الأخرى عن التغييرات |
| PlayerController | يستقبل الأحداث ويطبق الحركة |
| Time.deltaTime | يضمن حركة موحدة على جميع الأجهزة |

---

## 🚀 الخطوة التالية

في الدرس القادم، سنتعلم:
- نظام القتال
- نظام الأعداء
- نظام الصحة والضرر

**استمتع بالتطوير! 🎮**
