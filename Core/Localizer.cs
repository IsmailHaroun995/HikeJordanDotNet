using System.Globalization;

namespace HikeJordanDotNet.Core;

/// <summary>
/// Lightweight UI-string localizer. English is the source; Arabic strings come from the
/// dictionary below. Anything not in the dictionary falls back to the English source, so
/// user-generated content (posts, comments, names) is never altered.
/// The current language is set per-request by ASP.NET's request-localization middleware
/// from the culture cookie.
/// </summary>
public static class Localizer
{
    public static bool IsArabic =>
        CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

    public static string Dir => IsArabic ? "rtl" : "ltr";
    public static string Lang => IsArabic ? "ar" : "en";

    /// <summary>Translate an English UI string to the current language.</summary>
    public static string T(string english) =>
        IsArabic && Ar.TryGetValue(english, out var v) ? v : english;

    private static readonly Dictionary<string, string> Ar = new(StringComparer.Ordinal)
    {
        // ── Nav & footer ──────────────────────────────────────────────
        ["Feed"] = "الرئيسية",
        ["Explore"] = "استكشف",
        ["+ Post"] = "+ منشور",
        ["Admin"] = "الإدارة",
        ["Settings"] = "الإعدادات",
        ["Sign out"] = "تسجيل الخروج",
        ["Sign in"] = "تسجيل الدخول",
        ["Join"] = "انضم",
        ["The community for Jordan's outdoor explorers."] = "مجتمع مستكشفي الطبيعة في الأردن.",
        ["Privacy"] = "الخصوصية",
        ["English"] = "English",
        ["العربية"] = "العربية",

        // ── Feed (home) ───────────────────────────────────────────────
        ["Latest"] = "الأحدث",
        ["Following"] = "متابَعة",
        ["Share a trip or a tip…"] = "شارك رحلة أو نصيحة…",
        ["Post"] = "نشر",
        ["Jordan's outdoor community"] = "مجتمع الطبيعة في الأردن",
        ["Share your trips, follow other hikers, and discover new places through the people who've been there."]
            = "شارك رحلاتك، تابِع المتنزهين، واكتشف أماكن جديدة من خلال من زاروها.",
        ["Join the community"] = "انضم إلى المجتمع",
        ["Explore posts"] = "استكشف المنشورات",
        ["Your following feed is empty."] = "قائمة متابَعاتك فارغة.",
        ["Find people to follow →"] = "ابحث عن أشخاص لمتابعتهم ←",
        ["No posts yet. Be the first to share something."] = "لا توجد منشورات بعد. كن أول من يشارك.",
        ["Posts"] = "منشورات",
        ["Members"] = "أعضاء",
        ["Create your profile"] = "أنشئ ملفك الشخصي",
        ["Who to follow"] = "أشخاص لمتابعتهم",
        ["Follow"] = "متابعة",
        ["Explore by region"] = "استكشف حسب المنطقة",

        // ── Explore ───────────────────────────────────────────────────
        ["Search posts, places, people…"] = "ابحث في المنشورات والأماكن والأشخاص…",
        ["Search"] = "بحث",
        ["All"] = "الكل",
        ["People"] = "أشخاص",
        ["No posts found."] = "لا توجد منشورات.",

        // ── Post detail ───────────────────────────────────────────────
        ["← Back"] = "→ رجوع",
        ["Comments"] = "التعليقات",
        ["likes"] = "إعجاب",
        ["comments"] = "تعليق",
        ["Add a comment…"] = "أضف تعليقًا…",
        ["Comment"] = "تعليق",
        ["to join the conversation."] = "للمشاركة في النقاش.",
        ["No comments yet."] = "لا توجد تعليقات بعد.",

        // ── Profile ───────────────────────────────────────────────────
        ["Edit profile"] = "تعديل الملف",
        ["Followers"] = "متابِعون",
        ["Joined"] = "انضم في",
        ["You haven't posted anything yet."] = "لم تنشر أي شيء بعد.",
        ["hasn't posted anything yet."] = "لم ينشر أي شيء بعد.",

        // ── Compose ───────────────────────────────────────────────────
        ["New post"] = "منشور جديد",
        ["Share a trip"] = "شارك رحلة",
        ["Post a trip report, a photo, or a tip for the community."] = "انشر تقرير رحلة أو صورة أو نصيحة للمجتمع.",
        ["What's the story?"] = "ما القصة؟",
        ["Describe the trail, conditions, tips…"] = "صف المسار والظروف والنصائح…",
        ["Region"] = "المنطقة",
        ["Select a region"] = "اختر منطقة",
        ["Specific place (optional)"] = "مكان محدد (اختياري)",
        ["e.g. Sunset Ridge"] = "مثال: قمة الغروب",
        ["Photo (optional)"] = "صورة (اختياري)",
        ["Cancel"] = "إلغاء",

        // ── Settings ──────────────────────────────────────────────────
        ["Display name"] = "الاسم الظاهر",
        ["Bio"] = "نبذة",
        ["Tell the community about yourself…"] = "عرّف المجتمع بنفسك…",
        ["Location"] = "الموقع",
        ["e.g. Amman"] = "مثال: عمّان",
        ["Profile photo"] = "صورة الملف",
        ["Cover photo"] = "صورة الغلاف",
        ["View profile"] = "عرض الملف",
        ["Save changes"] = "حفظ التغييرات",
        ["Profile updated."] = "تم تحديث الملف.",

        // ── Login ─────────────────────────────────────────────────────
        ["Welcome back"] = "مرحبًا بعودتك",
        ["Sign in to your HikeJordan account."] = "سجّل الدخول إلى حسابك في هايك الأردن.",
        ["Email"] = "البريد الإلكتروني",
        ["Password"] = "كلمة المرور",
        ["New here?"] = "جديد هنا؟",
        ["Create an account"] = "أنشئ حسابًا",
        ["Please verify your email before signing in."] = "يرجى تأكيد بريدك الإلكتروني قبل تسجيل الدخول.",
        ["Resend verification email"] = "إعادة إرسال بريد التأكيد",

        // ── Register ──────────────────────────────────────────────────
        ["Join HikeJordan"] = "انضم إلى هايك الأردن",
        ["Create a profile to post trips, follow hikers, and save places."]
            = "أنشئ ملفًا لنشر الرحلات ومتابعة المتنزهين وحفظ الأماكن.",
        ["Username"] = "اسم المستخدم",
        ["Create account"] = "إنشاء حساب",
        ["Already have an account?"] = "لديك حساب بالفعل؟",
        ["Account type"] = "نوع الحساب",
        ["Person"] = "شخص",
        ["Group"] = "مجموعة",
        ["A personal account to share your own trips."] = "حساب شخصي لمشاركة رحلاتك.",
        ["A hiking group or company that runs trips."] = "مجموعة أو شركة تنظّم رحلات هايكنج.",
        ["Instagram page"] = "صفحة إنستغرام",
        ["WhatsApp number"] = "رقم واتساب",
        ["Reviews"] = "التقييمات",
        ["No reviews yet."] = "لا توجد تقييمات بعد.",
        ["Rate this group"] = "قيّم هذه المجموعة",
        ["Your name"] = "اسمك",
        ["Your rating"] = "تقييمك",
        ["Your review"] = "مراجعتك",
        ["Share your experience with this group…"] = "شارك تجربتك مع هذه المجموعة…",
        ["Submit review"] = "إرسال التقييم",
        ["Thanks for your review!"] = "شكرًا على تقييمك!",
        ["Review QR code"] = "رمز QR للتقييم",
        ["Share this code with joiners so they can rate your group."] = "شارك هذا الرمز مع المشاركين ليقيّموا مجموعتك.",
        ["Download QR"] = "تنزيل الرمز",
        ["Copy review link"] = "نسخ رابط التقييم",

        // ── Register confirmation / verify email ──────────────────────
        ["Check your email"] = "تحقق من بريدك",
        ["Resend verification email "] = "إعادة إرسال بريد التأكيد",
        ["Already verified?"] = "تم التأكيد بالفعل؟",
        ["Email verified"] = "تم تأكيد البريد",
        ["You're all set and signed in. Welcome to the community!"] = "كل شيء جاهز وتم تسجيل دخولك. مرحبًا بك في المجتمع!",
        ["Go to your feed"] = "اذهب إلى صفحتك الرئيسية",
        ["Already verified"] = "تم التأكيد مسبقًا",
        ["This email is already confirmed. You can sign in."] = "هذا البريد مؤكَّد بالفعل. يمكنك تسجيل الدخول.",
        ["Link expired"] = "انتهت صلاحية الرابط",
        ["This verification link has expired. Request a fresh one below."] = "انتهت صلاحية رابط التأكيد. اطلب رابطًا جديدًا أدناه.",
        ["Send a new link"] = "إرسال رابط جديد",
        ["Invalid link"] = "رابط غير صالح",

        // ── Admin ─────────────────────────────────────────────────────
        ["Moderation"] = "الإشراف",
        ["Comments "] = "تعليقات",
        ["Author"] = "الكاتب",
        ["When"] = "متى",
        ["Status"] = "الحالة",
        ["Actions"] = "إجراءات",
        ["Visible"] = "ظاهر",
        ["Hidden"] = "مخفي",
        ["Hide"] = "إخفاء",
        ["Unhide"] = "إظهار",
        ["Delete"] = "حذف",
        ["Name"] = "الاسم",
        ["Role"] = "الدور",
        ["Enable"] = "تفعيل",
        ["Disable"] = "تعطيل",

        // ── Badges ────────────────────────────────────────────────────
        ["Badges"] = "الشارات",
        ["Places visited"] = "أماكن تمت زيارتها",
        ["No badges yet — post about a place to earn one."] = "لا توجد شارات بعد — انشر عن مكان لتحصل على شارة.",

        // ── Region names ──────────────────────────────────────────────
        ["Wadi Rum"] = "وادي رم",
        ["Petra"] = "البتراء",
        ["Dana"] = "ضانا",
        ["Ajloun"] = "عجلون",
        ["Dead Sea"] = "البحر الميت",
        ["Wadi Mujib"] = "وادي الموجب",
        ["Aqaba"] = "العقبة",
        ["Jerash"] = "جرش",
        ["Salt"] = "السلط",
        ["Amman"] = "عمّان",
        ["Jordan Trail"] = "درب الأردن",
        ["Other"] = "أخرى",

        // ── Main page ─────────────────────────────────────────────────
        ["Popular regions"] = "مناطق شائعة",
        ["Discover Jordan, one trip at a time"] = "اكتشف الأردن، رحلة تلو الأخرى",
        ["Go to the feed"] = "اذهب إلى المنشورات",
        ["Share a hike"] = "شارك رحلة",
        ["Top hikers"] = "أفضل المتنزهين",
        ["See full leaderboard"] = "عرض كل المتصدرين",
        ["Ready to share your next hike?"] = "جاهز لمشاركة رحلتك القادمة؟",
        ["Explore Jordan, together."] = "استكشف الأردن، معًا.",
        ["How it works"] = "كيف يعمل",
        ["Share your hikes"] = "شارك رحلاتك",
        ["Post trip reports, photos, and stats — distance, elevation and time — from every adventure."]
            = "انشر تقارير الرحلات والصور والإحصائيات — المسافة والارتفاع والوقت — من كل مغامرة.",
        ["Follow the community"] = "تابِع المجتمع",
        ["Connect with hikers and groups, and discover new places through the people who've been there."]
            = "تواصل مع المتنزهين والمجموعات، واكتشف أماكن جديدة من خلال من زاروها.",
        ["Earn badges & rank up"] = "اجمع الشارات وتصدّر",
        ["Collect a badge for every place you visit and climb the community distance leaderboard."]
            = "اجمع شارة لكل مكان تزوره وتسلّق قائمة المتصدرين بالمسافة.",
        ["Browse all"] = "تصفّح الكل",
        ["posts"] = "منشور",

        // ── Hike stats & leaderboard ──────────────────────────────────
        ["Hike stats (optional)"] = "إحصائيات الرحلة (اختياري)",
        ["km"] = "كم",
        ["m elev"] = "م ارتفاع",
        ["min"] = "دقيقة",
        ["time"] = "الوقت",
        ["Hikes"] = "رحلات",
        ["km total"] = "كم إجمالي",
        ["m climbed"] = "م تسلّق",
        ["places"] = "أماكن",
        ["kudos"] = "إعجابات",
        ["Leaderboard"] = "المتصدرون",
        ["Top hikers by distance logged."] = "أكثر المتنزهين مسافةً مسجّلة.",
        ["This month"] = "هذا الشهر",
        ["All time"] = "كل الأوقات",
        ["hikes"] = "رحلات",
        ["m"] = "م",
        ["No logged hikes yet. Add distance to your posts to appear here."] = "لا توجد رحلات مسجّلة بعد. أضف المسافة إلى منشوراتك لتظهر هنا.",

        // ── Discovery & onboarding ────────────────────────────────────
        ["Trending regions"] = "المناطق الرائجة",
        ["See the leaderboard"] = "عرض المتصدرين",
        ["Get started"] = "لنبدأ",
        ["done"] = "منجز",
        ["Dismiss"] = "إخفاء",
        ["Add a profile photo"] = "أضف صورة شخصية",
        ["Write a short bio"] = "اكتب نبذة قصيرة",
        ["Share your first hike"] = "شارك أول رحلة لك",
        ["Follow 3 hikers"] = "تابِع 3 متنزهين",
        ["More from"] = "المزيد من",
        ["Copy link"] = "نسخ الرابط",
        ["Share"] = "مشاركة",

        // ── Notifications ─────────────────────────────────────────────
        ["Notifications"] = "الإشعارات",
        ["No notifications yet."] = "لا توجد إشعارات بعد.",
        ["liked your post"] = "أعجب بمنشورك",
        ["started following you"] = "بدأ بمتابعتك",
        ["commented on your post"] = "علّق على منشورك",

        // ── Shared ────────────────────────────────────────────────────
        ["Back to feed"] = "العودة إلى الرئيسية",
        ["Something went wrong"] = "حدث خطأ ما",
    };
}
