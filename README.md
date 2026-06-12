# 🎮 Echoes of Oblivion

> لعبة مغامرة عميقة تجمع بين الاستكشاف والقتال والفن الجميل

![Unity](https://img.shields.io/badge/Unity-6.0.0+-blue)
![Platform](https://img.shields.io/badge/Platform-Windows%20PC-green)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow)

---

## 📖 نظرة عامة

**Echoes of Oblivion** هي لعبة مغامرة من نوع AA تجمع بين:

- 🗺️ **الاستكشاف العميق** (مستوحاة من Tonic)
- ⚔️ **القتال التكتيكي** (مستوحاة من Death Door)
- 🎨 **الفن الجميل** (مستوحاة من Ori)
- 📖 **القصة العميقة** التي تعتمد على الاستكشاف

---

## 🎯 المميزات

### 🌍 خمسة عوالم مختلفة
1. **غابة الأصول** - بدايات غامضة
2. **مملكة الحجر** - تاريخ ثقيل
3. **البحار الهادئة** - جمال مريب
4. **ملحمة الليل** - سحر ملغز
5. **قلب الضوء** - الحقيقة النهائية

### 🎮 أنظمة اللعب
- ✅ نظام إدخال كامل (Keyboard + Controller + Mouse)
- ✅ نظام حركة ناعم وطبيعي
- ✅ نظام كاميرا متقدم مع تأثيرات
- ✅ نظام قتال تكتيكي
- ✅ نظام استكشاف عميق
- ✅ نظام ألغاز متنوعة
- ✅ نظام RPG خفيف

---

## 🛠️ المتطلبات

- **Unity**: 6.0.0+ (6000.4.10f1)
- **نظام التشغيل**: Windows 10/11
- **RAM**: 4GB على الأقل
- **GPU**: جهاز رسوميات بسيط

---

## 📁 هيكل المشروع

```
Echoes-of-Oblivion/
├── Assets/
│   └── _Project/
│       ├── Scripts/
│       │   ├── Core/
│       │   │   ├── InputManager.cs
│       │   │   ├── GameManager.cs
│       │   │   └── EventSystem.cs
│       │   ├── Gameplay/
│       │   │   ├── Player/
│       │   │   │   └── PlayerController.cs
│       │   │   ├── Combat/
│       │   │   ├── Exploration/
│       │   │   └── Puzzles/
│       │   └── Camera/
│       │       └── CameraController.cs
│       ├── Prefabs/
│       ├── Scenes/
│       ├── Art/
│       ├── Audio/
│       ├── Data/
│       └── Documentation/
├── TUTORIALS/
├── README.md
└── .gitignore
```

---

## 🚀 البدء السريع

### 1. فتح المشروع
```bash
# في Unity Hub
1. اختر "Open"
2. اختر مجلد المشروع
3. افتح في Unity 6.0+
```

### 2. تشغيل اللعبة
```
1. افتح المشهد: Assets/_Project/Scenes/Game.unity
2. اضغط Play
3. جرب الحركة مع WASD أو Left Stick على Controller
```

### 3. التحكم

| الفعل | Keyboard | Controller | Mouse |
|------|----------|-----------|-------|
| الحركة | WASD | Left Stick | N/A |
| الجري | Shift | LB | N/A |
| الهجوم | Left Click | X | ✓ |
| التفادي | Right Click | B | ✓ |
| الـ Pause | Escape | Start | N/A |

---

## 📚 الدروس والتوثيق

### للمبتدئين
- 📖 [شرح نظام الإدخال](TUTORIALS/01_INPUT_SYSTEM_GUIDE.md)
- 📖 [شرح حركة الشخصية](TUTORIALS/02_PLAYER_MOVEMENT.md) *(قريباً)*
- 📖 [شرح الكاميرا](TUTORIALS/03_CAMERA_SYSTEM.md) *(قريباً)*

### للمتقدمين
- 📖 [نظام القتال](TUTORIALS/COMBAT_SYSTEM.md) *(قريباً)*
- 📖 [نظام الأعداء](TUTORIALS/ENEMY_SYSTEM.md) *(قريباً)*
- 📖 [نظام الألغاز](TUTORIALS/PUZZLE_SYSTEM.md) *(قريباً)*

---

## 📊 خطة التطوير

### ✅ المرحلة الحالية: Prototype
- [x] نظام الإدخال الكامل
- [x] حركة الشخصية
- [x] نظام الكاميرا
- [ ] نظام القتال الأساسي
- [ ] أول عدو بسيط
- [ ] أول عالم صغير

### 📋 المراحل المقبلة
- **Alpha**: 3 عوالم + قتال متقدم
- **Beta**: 5 عوالم + كل الأنظمة
- **Release**: لعبة كاملة مع تلميع

---

## 🎨 الفنيات

### البصريات
- **أسلوب**: 2D بكسل آرت عالي الجودة
- **الألوان**: تدرجية وعاطفية
- **التأثيرات**: Particle Effects جميلة
- **الحركة**: سلسة واحترافية

### الموسيقى
- **الأسلوب**: موسيقى حزينة وملغزة
- **التأثير**: تؤثر على أحاسيس اللاعب
- **التنوع**: مختلفة لكل عالم

---

## 🤖 الأدوات المستخدمة

### تطوير
- **محرك**: Unity 6.0+
- **البرمجة**: C#
- **Version Control**: Git + GitHub

### الفن والموسيقى
- **الفن**: Krita / Aseprite (مستقبلاً)
- **الموسيقى**: LMMS / Audacity (مستقبلاً)
- **AI**: Stable Diffusion للـ Concept Art

---

## 👨‍💻 المطورون

- **المطور**: midou7reus
- **AI المساعد**: GitHub Copilot
- **المصمم**: تم بواسطة فريق العمل

---

## 📝 الترخيص

هذا المشروع مفتوح المصدر ومرخص تحت MIT License.

---

## 💬 التواصل والمساعدة

هل لديك أسئلة؟ هل وجدت خطأ؟

- 📧 البريد: يمكنك التواصل عبر GitHub Issues
- 💬 المناقشات: استخدم GitHub Discussions
- 🐛 الأخطاء: أرسل تقرير في Issues

---

## 🎉 شكر خاص

شكراً لإلهامات:
- **Death Door**: نظام القتال الرائع
- **Tonic Trouble**: الاستكشاف الممتع
- **Ori and the Blind Forest**: الفن والجماليات
- **Hollow Knight**: البناء العالمي الرائع
- **Gris**: الألوان والعاطفة

---

## 📈 إحصائيات المشروع

```
سطور الكود: 1000+
ملفات Scenes: 5+
ملفات Scripts: 10+
ساعات العمل: 🚀 جارية!
مستوى الجودة: ⭐⭐⭐⭐
```

---

## 🎮 جرب اللعبة الآن!

```bash
# استنساخ المشروع
git clone https://github.com/midou7reus/Echoes-of-Oblivion.git

# فتح في Unity
# Unity Hub → Open → اختر المجلد
```

---

**استمتع برحلتك في الأصوات المجهولة! 🌙✨**
