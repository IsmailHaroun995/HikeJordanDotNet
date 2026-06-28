const filterControls = document.querySelectorAll(".filter-control");
const hikeCards = document.querySelectorAll(".hike-card");
const resetFilters = document.getElementById("resetFilters");
const categoryChips = document.querySelectorAll(".category-chip");
const filterSummary = document.getElementById("filterSummary");
const noResults = document.getElementById("noResults");
const languageToggle = document.getElementById("languageToggle");

const translations = {
  "Hike Jordan": "هايك جوردن",
  "Trips": "الرحلات",
  "Destinations": "الوجهات",
  "Organizers": "المنظمون",
  "Trust": "الثقة",
  "Admin": "الإدارة",
  "Organizer requests": "طلبات المنظمين",
  "Approve accounts before they can add hikes": "وافق على الحسابات قبل أن تتمكن من إضافة الرحلات",
  "Approve listings before publishing": "وافق على الرحلات قبل نشرها",
  "Account approvals": "موافقات الحسابات",
  "Organizer form requests": "طلبات تسجيل المنظمين",
  "New organizers must be approved here before they can sign in and submit hikes.": "يجب الموافقة على المنظمين الجدد هنا قبل أن يتمكنوا من تسجيل الدخول وإرسال الرحلات.",
  "Regions": "المناطق",
  "Experience": "الخبرة",
  "Approve organizer": "موافقة على المنظم",
  "Needs docs": "تحتاج وثائق",
  "Approve a hike to move it from Submitted into Approved. Publishing would make it visible to visitors.": "وافق على الرحلة لنقلها من مرسلة إلى موافق عليها. النشر يجعلها ظاهرة للزوار.",
  "Publish": "نشر",
  "Published": "منشورة",
  "Rejected": "مرفوضة",
  "Docs requested": "تم طلب الوثائق",
  "Resolved": "تم الحل",
  "Done": "تم",
  "Sign in": "تسجيل الدخول",
  "Sign out": "تسجيل الخروج",
  "Add hike": "إضافة رحلة",
  "Switch language": "تغيير اللغة",
  "Privacy": "الخصوصية",
  "Discover trusted organized hikes in under 60 seconds.": "اكتشف رحلات هايكنج منظمة وموثوقة خلال أقل من 60 ثانية.",

  "Jordan's organized hiking marketplace": "منصة رحلات الهايكنج المنظمة في الأردن",
  "Find the right hike in under 60 seconds.": "اعثر على الرحلة المناسبة خلال أقل من 60 ثانية.",
  "Hike Jordan gathers scattered Instagram, WhatsApp, and Facebook trip posts into one trusted place where travelers can compare routes, organizers, dates, prices, difficulty, and reviews.": "يجمع هايك جوردن رحلات الهايكنج المتفرقة من إنستغرام وواتساب وفيسبوك في مكان موثوق واحد لمقارنة المسارات والمنظمين والتواريخ والأسعار والصعوبة والتقييمات.",
  "Explore hikes": "استكشف الرحلات",
  "Add your trip": "أضف رحلتك",
  "How filters work": "كيف تعمل الفلاتر",
  "Search upcoming hikes": "البحث عن رحلات قادمة",
  "Wadi Rum desert landscape at sunset": "منظر صحراء وادي رم عند الغروب",
  "Marketplace value": "قيمة المنصة",
  "Showing all public hikes. Change region or difficulty to narrow the cards below.": "يتم عرض كل الرحلات العامة. غيّر المنطقة أو مستوى الصعوبة لتضييق النتائج أدناه.",
  "Region": "المنطقة",
  "Anywhere": "أي مكان",
  "Wadi Rum": "وادي رم",
  "Ajloun": "عجلون",
  "Dana": "ضانا",
  "Dead Sea": "البحر الميت",
  "Difficulty": "الصعوبة",
  "Any level": "أي مستوى",
  "Easy": "سهل",
  "Moderate": "متوسط",
  "Hard": "صعب",
  "When": "الموعد",
  "This weekend": "نهاية هذا الأسبوع",
  "Next 7 days": "الأيام السبعة القادمة",
  "Next 30 days": "الأيام الثلاثون القادمة",
  "Reset filters": "إعادة ضبط الفلاتر",
  "Upcoming hikes": "رحلات قادمة",
  "Verified organizers": "منظمون موثقون",
  "Average trip rating": "متوسط تقييم الرحلات",
  "Direct WhatsApp booking": "حجز مباشر عبر واتساب",
  "Search and discovery": "البحث والاكتشاف",
  "Upcoming and featured hikes": "رحلات قادمة ومميزة",
  "Compare routes, organizers, dates, prices, difficulty, and remaining spots — then book directly on WhatsApp.": "قارن المسارات والمنظمين والتواريخ والأسعار والصعوبة والمقاعد المتبقية — ثم احجز مباشرة عبر واتساب.",
  "What kind of trip are you looking for?": "ما نوع الرحلة التي تبحث عنها؟",
  "Featured hike": "رحلة مميزة",
  "Approved hike": "رحلة موافق عليها",
  "Published hike": "رحلة منشورة",
  "Wadi Rum Sunset Ridge": "مسار غروب وادي رم",
  "Golden-hour ridge walk with jeep transfer, Bedouin tea, and stargazing add-on.": "مسار وقت الغروب مع نقل بالجيب، شاي بدوي، وخيار مراقبة النجوم.",
  "8 km": "٨ كم",
  "JD 32": "٣٢ د.أ",
  "Date": "التاريخ",
  "Fri, Jul 3": "الجمعة، ٣ يوليو",
  "Time": "الوقت",
  "2:30 PM": "٢:٣٠ مساء",
  "Spots": "المقاعد",
  "Duration": "المدة",
  "Payment": "طريقة الدفع",
  "Instagram": "إنستغرام",
  "Free": "مجاناً",
  "7 left": "٧ متبقية",
  "0 left": "٠ متبقية",
  "Upcoming": "قادمة",
  "In Progress": "جارية",
  "Completed": "مكتملة",
  "Rate this trip": "قيّم هذه الرحلة",
  "Book on WhatsApp": "احجز عبر واتساب",
  "No approved trips yet": "لا توجد رحلات معتمدة بعد",
  "Hikes appear here once reviewed and approved by our team. Organizers can submit trips for review.": "تظهر الرحلات هنا بعد مراجعتها واعتمادها من فريقنا. يمكن للمنظمين إرسال رحلاتهم للمراجعة.",
  "List your trip": "أضف رحلتك",
  "Organizer": "المنظم",
  "Desert Paths": "مسارات الصحراء",
  "Beginner friendly": "مناسبة للمبتدئين",
  "Ajloun Forest Morning Loop": "جولة صباحية في غابات عجلون",
  "Shaded family-friendly route with guide briefing, breakfast stop, and photo points.": "مسار مظلل مناسب للعائلات مع شرح من المرشد، توقف للإفطار، ونقاط تصوير.",
  "5 km": "٥ كم",
  "JD 18": "١٨ د.أ",
  "Sat, Jul 4": "السبت، ٤ يوليو",
  "8:00 AM": "٨:٠٠ صباحا",
  "14 left": "١٤ متبقية",
  "Green North": "الشمال الأخضر",
  "Experienced hikers": "للمتمرسين",
  "Dana Canyon Descent": "نزول وادي ضانا",
  "Full-day technical descent with licensed guide, safety briefing, and support vehicle.": "نزول تقني ليوم كامل مع مرشد مرخص، شرح سلامة، ومركبة دعم.",
  "14 km": "١٤ كم",
  "JD 45": "٤٥ د.أ",
  "Fri, Jul 10": "الجمعة، ١٠ يوليو",
  "6:30 AM": "٦:٣٠ صباحا",
  "5 left": "٥ متبقية",
  "Jordan Trail Co.": "شركة درب الأردن",
  "No hikes match those filters yet. Try another region or difficulty.": "لا توجد رحلات مطابقة لهذه الفلاتر حاليا. جرّب منطقة أو مستوى صعوبة آخر.",

  "Popular destinations": "وجهات شائعة",
  "Browse by destination.": "تصفح حسب الوجهة.",
  "Filter by region to find hikes near Wadi Rum, Ajloun forests, Dana canyons, or Dead Sea trails.": "فلتر حسب المنطقة للعثور على رحلات قرب وادي رم أو غابات عجلون أو وادي ضانا أو مسارات البحر الميت.",
  "Desert ridges, sunsets, stargazing": "كثبان صحراوية، غروب، ومشاهدة نجوم",
  "Forests, family loops, cool mornings": "غابات، مسارات عائلية، وصباحات باردة",
  "Canyons, long descents, advanced routes": "أودية، نزولات طويلة، ومسارات متقدمة",
  "Dead Sea Trails": "مسارات البحر الميت",
  "Winter routes, viewpoints, dry valleys": "مسارات شتوية، إطلالات، وأودية جافة",
  "Categories": "الفئات",
  "What kind of trip are you looking for?": "ما نوع الرحلة التي تبحث عنها؟",
  "Family hikes": "رحلات عائلية",
  "Sunset trips": "رحلات الغروب",
  "Advanced routes": "مسارات متقدمة",
  "Women-only groups": "مجموعات للسيدات فقط",
  "Overnight camping": "تخييم ليلي",

  "Trust marketplace": "سوق مبني على الثقة",
  "Top organizers": "أفضل المنظمين",
  "No verified organizers yet. Check back soon.": "لا يوجد منظمون موثقون بعد. عد قريباً.",
  "rating": "تقييم",
  "4.9 rating · 126 past trips · verified insurance": "تقييم ٤.٩ · ١٢٦ رحلة سابقة · تأمين موثق",
  "Best for multi-day and advanced routes with strong safety briefings.": "مناسب للرحلات المتقدمة ومتعددة الأيام مع شرح سلامة قوي.",
  "4.7 rating · 84 past trips · fast response": "تقييم ٤.٧ · ٨٤ رحلة سابقة · سرعة رد عالية",
  "Family-friendly Ajloun and Salt routes with beginner pacing.": "مسارات عائلية في عجلون والسلط بوتيرة مناسبة للمبتدئين.",
  "4.8 rating · 97 past trips · local guides": "تقييم ٤.٨ · ٩٧ رحلة سابقة · مرشدون محليون",
  "Wadi Rum desert hikes, sunset routes, and overnight add-ons.": "رحلات صحراء وادي رم، مسارات الغروب، وخيارات المبيت.",
  "What hikers say": "ماذا يقول المتنزهون",
  "Trusted by hikers across Jordan.": "يثق به المتنزهون في جميع أنحاء الأردن.",
  "\"I used to compare five Instagram accounts before choosing. Hike Jordan made the decision obvious.\"": "\"كنت أقارن بين خمسة حسابات على إنستغرام قبل أن أختار. هايك جوردن جعل القرار سهلاً وواضحاً.\"",
  "Rana, Amman": "رنا، عمّان",
  "\"The organizer profile matters. I want to see past trips and reviews before I message on WhatsApp.\"": "\"ملف المنظم يصنع الفرق. أريد أن أرى رحلات سابقة وتقييمات قبل أن أرسل واتساب.\"",
  "Omar, first-time Dana visitor": "عمر، زائر ضانا لأول مرة",
  "\"I listed my trip in minutes and started getting enquiries the same day. Simple and effective.\"": "\"أضفت رحلتي في دقائق وبدأت أتلقى استفسارات في نفس اليوم. بسيط وفعّال.\"",
  "Ahmad, trip organizer in Ajloun": "أحمد، منظم رحلات في عجلون",
  "Why trust us": "لماذا تثق بنا",
  "Every organizer is verified before going live.": "كل منظم يُتحقق منه قبل النشر.",
  "We review each organizer's experience, past trips, and safety record before approving their listing. Joiners can rate and review completed trips so quality stays high.": "نراجع خبرة كل منظم ورحلاته السابقة وسجل السلامة قبل الموافقة على إعلانه. يمكن للمشاركين تقييم الرحلات المكتملة للحفاظ على الجودة.",
  "What we verify": "ما نتحقق منه",
  "Organizer identity and experience level.": "هوية المنظم ومستوى خبرته.",
  "Safety record and past trip history.": "سجل السلامة وتاريخ الرحلات السابقة.",
  "WhatsApp contact and response rate.": "رقم واتساب ومعدل الرد.",
  "Honest reviews from verified joiners.": "تقييمات صادقة من مشاركين موثقين.",
  "For organizers": "للمنظمين",
  "Get approved first, then publish trusted hikes.": "احصل على الموافقة أولا، ثم انشر رحلات موثوقة.",
  "Organizers create a profile and wait for admin approval. After verification, they can sign in, add hikes, and receive bookings directly on WhatsApp.": "ينشئ المنظم ملفا وينتظر موافقة الإدارة. بعد التوثيق يمكنه تسجيل الدخول وإضافة الرحلات واستقبال الحجوزات عبر واتساب.",
  "Organizer sign up": "تسجيل منظم",
  "Organizer sign in": "دخول المنظم",

  "Secure access": "دخول آمن",
  "Sign in to manage Hike Jordan.": "سجّل الدخول لإدارة هايك جوردن.",
  "Admins review supply and trust signals. Approved organizers can add hikes after their profile is verified.": "تراجع الإدارة الرحلات وإشارات الثقة. يمكن للمنظمين الموافق عليهم إضافة رحلات بعد توثيق ملفاتهم.",
  "Demo accounts": "حسابات تجريبية",
  "Admin: admin@hikejordan.test / admin123": "الإدارة: admin@hikejordan.test / admin123",
  "Approved organizer: organizer@hikejordan.test / organizer123": "منظم موافق عليه: organizer@hikejordan.test / organizer123",
  "Pending organizer: pending@hikejordan.test / pending123": "منظم قيد الانتظار: pending@hikejordan.test / pending123",
  "Email": "البريد الإلكتروني",
  "Password": "كلمة المرور",
  "Invalid demo credentials.": "بيانات الدخول التجريبية غير صحيحة.",
  "Organizer onboarding": "انضمام المنظمين",
  "Apply to list hikes.": "قدّم طلبا لإدراج الرحلات.",
  "Organizers cannot publish hikes immediately. The admin reviews the profile, verifies trust signals, then approves the account.": "لا يستطيع المنظمون نشر الرحلات مباشرة. تراجع الإدارة الملف وتتحقق من إشارات الثقة ثم توافق على الحساب.",
  "1. Create organizer profile": "١. إنشاء ملف المنظم",
  "2. Wait for admin approval": "٢. انتظار موافقة الإدارة",
  "3. Add hikes after approval": "٣. إضافة الرحلات بعد الموافقة",
  "Organizer / company name": "اسم المنظم / الشركة",
  "WhatsApp number": "رقم واتساب",
  "Main regions": "المناطق الرئيسية",
  "Experience and safety notes": "الخبرة وملاحظات السلامة",
  "Submit for approval": "إرسال للموافقة",
  "Organizer profile submitted.": "تم إرسال ملف المنظم.",
  "Your account is pending admin approval. After approval, you can sign in and add hikes from the organizer dashboard.": "حسابك بانتظار موافقة الإدارة. بعد الموافقة يمكنك تسجيل الدخول وإضافة الرحلات.",
  "Go to sign in": "اذهب إلى تسجيل الدخول",

  "Organizer submission": "إرسال رحلة من منظم",
  "Add a hike for review.": "أضف رحلة للمراجعة.",
  "Approved organizers submit trip details here. In the MVP, the administrator reviews the listing, approves it, then publishes it with a WhatsApp booking button.": "يرسل المنظمون الموافق عليهم تفاصيل الرحلة هنا. في النسخة الأولى تراجع الإدارة الإعلان وتوافق عليه ثم تنشره مع زر حجز عبر واتساب.",
  "Signed in as": "مسجل الدخول باسم",
  "Account status: Approved": "حالة الحساب: موافق عليه",
  "Account status: Pending approval": "حالة الحساب: بانتظار الموافقة",
  "Hike status after submit: Submitted": "حالة الرحلة بعد الإرسال: مرسلة",
  "What organizers need": "ما يحتاجه المنظمون",
  "A clear title and honest route description.": "عنوان واضح ووصف صادق للمسار.",
  "Date, time, price, capacity, and difficulty.": "التاريخ، الوقت، السعر، السعة، والصعوبة.",
  "Meeting point, required gear, inclusions, and exclusions.": "نقطة اللقاء، المعدات المطلوبة، المشمولات، وغير المشمولات.",
  "A WhatsApp number where visitors can book directly.": "رقم واتساب يمكن للزوار الحجز من خلاله مباشرة.",
  "Admin status after submit": "حالة الإدارة بعد الإرسال",
  "Draft -> Submitted -> Approved -> Published": "مسودة -> مرسلة -> موافق عليها -> منشورة",
  "Your organizer account is still pending approval.": "حساب المنظم الخاص بك ما زال بانتظار الموافقة.",
  "An admin must verify the organizer profile before this account can add hikes. This protects the marketplace from unverified trip supply.": "يجب أن تتحقق الإدارة من ملف المنظم قبل أن يتمكن هذا الحساب من إضافة الرحلات. هذا يحمي السوق من رحلات غير موثقة.",
  "Review signup flow": "راجع مسار التسجيل",
  "Hike submitted for review.": "تم إرسال الرحلة للمراجعة.",
  "The listing is now in the Submitted state. An admin would review the details, verify the organizer, and publish it when approved.": "الإعلان الآن في حالة مرسلة. ستراجع الإدارة التفاصيل وتتحقق من المنظم ثم تنشره عند الموافقة.",
  "Back to homepage": "العودة إلى الصفحة الرئيسية",
  "Organizer name": "اسم المنظم",
  "Hike title": "عنوان الرحلة",
  "Description": "الوصف",
  "Trip basics": "أساسيات الرحلة",
  "Organizer": "المنظم",
  "Schedule and discovery": "الموعد والاكتشاف",
  "Choose region": "اختر المنطقة",
  "Wadi Mujib": "وادي الموجب",
  "Salt": "السلط",
  "Choose difficulty": "اختر الصعوبة",
  "Price": "السعر",
  "Capacity": "السعة",
  "Duration (hours)": "المدة (بالساعات)",
  "Distance (km)": "المسافة (كم)",
  "Meeting point": "نقطة اللقاء",
  "Preparation details": "تفاصيل التجهيز",
  "Required gear": "المعدات المطلوبة",
  "Included items": "العناصر المشمولة",
  "Excluded items": "العناصر غير المشمولة",
  "Submit hike for review": "إرسال الرحلة للمراجعة",

  "Administrator workspace": "مساحة الإدارة",
  "Moderate hikes, organizers, reviews, and marketplace quality.": "راجع الرحلات والمنظمين والتقييمات وجودة السوق.",
  "This dashboard is the control room for the MVP: approve trip supply, verify organizer trust signals, watch WhatsApp conversion, and keep reviews clean.": "هذه اللوحة هي مركز التحكم للنسخة الأولى: الموافقة على الرحلات، توثيق المنظمين، متابعة تحويلات واتساب، والحفاظ على جودة التقييمات.",
  "Add test hike": "أضف رحلة تجريبية",
  "Review queue": "قائمة المراجعة",
  "Submitted hikes": "رحلات مرسلة",
  "3 need admin review today": "٣ تحتاج مراجعة اليوم",
  "Organizer checks": "فحوصات المنظمين",
  "2 waiting for documents": "٢ بانتظار الوثائق",
  "Review flags": "بلاغات التقييمات",
  "1 high priority item": "عنصر واحد عالي الأولوية",
  "WhatsApp clicks": "نقرات واتساب",
  "+24% this week": "+٢٤٪ هذا الأسبوع",
  "Trip moderation": "مراجعة الرحلات",
  "Organizer verification": "توثيق المنظمين",
  "Review moderation": "مراجعة التقييمات",
  "Analytics": "التحليلات",
  "Marketplace settings": "إعدادات السوق",
  "Trip lifecycle": "دورة حياة الرحلة",
  "Submitted hike queue": "قائمة الرحلات المرسلة",
  "Hike": "الرحلة",
  "Level": "المستوى",
  "Status": "الحالة",
  "Action": "الإجراء",
  "Needs image quality check": "تحتاج فحص جودة الصور",
  "Ready to publish": "جاهزة للنشر",
  "Verify safety notes": "تحقق من ملاحظات السلامة",
  "Review": "مراجعة",
  "Approve": "موافقة",
  "Reject": "رفض",
  "Trust operations": "عمليات الثقة",
  "Verified": "موثق",
  "Pending": "قيد الانتظار",
  "4.8": "٤.٨",
  "4.7": "٤.٧",
  "97 trips": "٩٧ رحلة",
  "84 trips": "٨٤ رحلة",
  "126 trips": "١٢٦ رحلة",
  "Local guide documents uploaded": "تم رفع وثائق المرشد المحلي",
  "Waiting for license photo": "بانتظار صورة الترخيص",
  "Insurance proof current": "إثبات التأمين ساري",
  "Verify": "توثيق",
  "Request docs": "طلب وثائق",
  "Moderation flags": "بلاغات المراجعة",
  "Possible duplicate review": "تقييم مكرر محتمل",
  "Same text appeared on two Dana trips": "ظهر النص نفسه على رحلتين في ضانا",
  "Medium": "متوسط",
  "Organizer response dispute": "نزاع حول رد المنظم",
  "Visitor says WhatsApp reply took 3 days": "يقول الزائر إن الرد عبر واتساب استغرق ٣ أيام",
  "Low": "منخفض",
  "Unverified attendance": "حضور غير موثق",
  "Reviewer did not provide trip proof": "لم يقدم المقيّم إثبات حضور",
  "High": "عال",
  "Resolve": "حل",
  "Marketplace analytics": "تحليلات السوق",
  "What admins should track": "ما يجب أن تتابعه الإدارة",
  "Search-to-click rate": "معدل البحث إلى النقر",
  "Measures whether users find a hike quickly.": "يقيس هل يجد المستخدمون رحلة بسرعة.",
  "Organizer response rate": "معدل رد المنظم",
  "Protects trust when booking happens in WhatsApp.": "يحمي الثقة عندما يتم الحجز عبر واتساب.",
  "Approval time": "وقت الموافقة",
  "Keeps new supply from getting stuck in Submitted.": "يمنع بقاء الرحلات الجديدة عالقة في حالة مرسلة.",
  "Flag rate": "معدل البلاغات",
  "Shows review quality and fake-review risk.": "يوضح جودة التقييمات وخطر التقييمات الوهمية.",
  "Configuration": "الإعدادات",
  "Require admin approval before publishing": "طلب موافقة الإدارة قبل النشر",
  "Require WhatsApp number on every hike": "طلب رقم واتساب لكل رحلة",
  "Allow verified reviews only after attendance proof": "السماح بالتقييمات الموثقة فقط بعد إثبات الحضور",
  "Enable paid featured placements": "تفعيل الظهور المميز المدفوع"
};

const placeholders = {
  "Jordan Trail Co.": "شركة درب الأردن",
  "owner@example.com": "owner@example.com",
  "+962 7X XXX XXXX": "+962 7X XXX XXXX",
  "Wadi Rum, Dana, Ajloun": "وادي رم، ضانا، عجلون",
  "Years operating, licenses, safety process, guide experience": "سنوات الخبرة، التراخيص، إجراءات السلامة، خبرة المرشدين",
  "admin@hikejordan.test": "admin@hikejordan.test",
  "admin123": "admin123",
  "Wadi Rum Sunset Ridge": "مسار غروب وادي رم",
  "Describe the route, pace, scenery, and who it is best for.": "صف المسار والوتيرة والمناظر ولمن يناسب.",
  "32": "٣٢",
  "18": "١٨",
  "6": "٦",
  "8": "٨",
  "7th Circle, Amman": "الدوار السابع، عمّان",
  "Hiking shoes, 2L water, hat": "حذاء هايكنج، ٢ لتر ماء، قبعة",
  "Guide, transport, snacks": "مرشد، نقل، وجبات خفيفة",
  "Meals, personal insurance": "وجبات، تأمين شخصي"
};

let currentLanguage = localStorage.getItem("hikeJordanLanguage") || "en";

function normalized(text) {
  return text.replace(/\s+/g, " ").trim();
}

function translateTextNode(node, language) {
  if (!node.nodeValue || !normalized(node.nodeValue)) return;
  if (!node.__sourceText) node.__sourceText = normalized(node.nodeValue);
  const source = node.__sourceText;
  const translated = language === "ar" ? translations[source] : source;
  if (!translated) return;
  const leading = node.nodeValue.match(/^\s*/)?.[0] || "";
  const trailing = node.nodeValue.match(/\s*$/)?.[0] || "";
  node.nodeValue = `${leading}${translated}${trailing}`;
}

function translateAttributes(element, language) {
  ["placeholder", "aria-label", "title", "alt"].forEach((attribute) => {
    if (!element.hasAttribute(attribute)) return;
    const key = `source${attribute}`;
    if (!element.dataset[key]) element.dataset[key] = element.getAttribute(attribute) || "";
    const source = element.dataset[key];
    const translated = language === "ar" ? (placeholders[source] || translations[source]) : source;
    if (translated) element.setAttribute(attribute, translated);
  });
}

function translatePage(language) {
  const walker = document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT, {
    acceptNode(node) {
      const parent = node.parentElement;
      if (!parent || ["SCRIPT", "STYLE", "NOSCRIPT"].includes(parent.tagName)) {
        return NodeFilter.FILTER_REJECT;
      }
      return normalized(node.nodeValue || "") ? NodeFilter.FILTER_ACCEPT : NodeFilter.FILTER_REJECT;
    }
  });

  const textNodes = [];
  while (walker.nextNode()) textNodes.push(walker.currentNode);
  textNodes.forEach((node) => translateTextNode(node, language));
  document.querySelectorAll("[placeholder], [aria-label], [title], img[alt]").forEach((element) => translateAttributes(element, language));
}

function setFilterSummary(visibleCount) {
  if (!filterSummary) return;
  const regionText = document.querySelector('[data-filter="region"]')?.selectedOptions[0]?.text || "Anywhere";
  const difficultyText = document.querySelector('[data-filter="difficulty"]')?.selectedOptions[0]?.text || "Any level";
  if (currentLanguage === "ar") {
    filterSummary.textContent = `يتم عرض ${visibleCount} من ${hikeCards.length} رحلات للمنطقة ${regionText} وبمستوى ${difficultyText}.`;
  } else {
    filterSummary.textContent = `Showing ${visibleCount} of ${hikeCards.length} hikes for ${regionText} and ${difficultyText}.`;
  }
}

function applyFilters() {
  const activeFilters = Array.from(filterControls).reduce((filters, control) => {
    filters[control.dataset.filter] = control.value;
    return filters;
  }, {});

  let visibleCount = 0;

  hikeCards.forEach((card) => {
    const matchesRegion = activeFilters.region === "all" || card.dataset.region === activeFilters.region;
    const matchesDifficulty = activeFilters.difficulty === "all" || card.dataset.difficulty === activeFilters.difficulty;
    const isVisible = matchesRegion && matchesDifficulty;
    card.classList.toggle("is-hidden", !isVisible);
    if (isVisible) visibleCount += 1;
  });

  setFilterSummary(visibleCount);
  if (noResults) noResults.hidden = visibleCount !== 0;
}

filterControls.forEach((control) => {
  control.addEventListener("change", applyFilters);
});

if (resetFilters) {
  resetFilters.addEventListener("click", () => {
    filterControls.forEach((control) => {
      control.value = "all";
    });
    applyFilters();
  });
}

categoryChips.forEach((chip) => {
  chip.addEventListener("click", () => {
    categoryChips.forEach((item) => item.classList.remove("is-active"));
    chip.classList.add("is-active");
    document.getElementById("hikes")?.scrollIntoView({ behavior: "smooth", block: "start" });
  });
});

function setSourceText(element, text) {
  element.childNodes.forEach((node) => {
    if (node.nodeType === Node.TEXT_NODE) {
      node.__sourceText = text;
      node.nodeValue = text;
    }
  });
}

function updateCounter(name, delta) {
  const counter = document.querySelector(`[data-counter="${name}"]`);
  if (!counter) return;
  const value = Number.parseInt(counter.textContent || "0", 10);
  if (Number.isNaN(value)) return;
  counter.textContent = Math.max(0, value + delta).toString();
}

function setStatus(container, status, counterName) {
  const badge = container.querySelector("[data-status], .status-badge");
  if (!badge) return;
  setSourceText(badge, status);
  badge.classList.remove("status-submitted", "status-approved", "status-rejected", "status-published", "status-docs", "status-hidden");
  const statusClass = {
    Approved: "status-approved",
    Published: "status-published",
    Rejected: "status-rejected",
    Hidden: "status-hidden",
    Disabled: "status-rejected",
    "Docs requested": "status-docs",
    Resolved: "status-approved"
  }[status] || "status-submitted";
  badge.classList.add(statusClass);
  container.classList.toggle("is-complete", ["Approved", "Published", "Rejected", "Resolved"].includes(status));
  if (counterName && ["Approved", "Rejected", "Published"].includes(status) && !container.dataset.countedDone) {
    updateCounter(counterName, -1);
    container.dataset.countedDone = "true";
  }
}

document.querySelectorAll("[data-action]").forEach((button) => {
  button.addEventListener("click", () => {
    const action = button.dataset.action;
    const item = button.closest("[data-admin-item]") || button.closest("article") || button.closest("tr");

    if (!item) return;

    if (action === "approve-hike")      setStatus(item, "Approved", "hike-requests");
    if (action === "publish-hike")      setStatus(item, "Published", "hike-requests");
    if (action === "reject-hike")       setStatus(item, "Rejected", "hike-requests");
    if (action === "hide-hike")         setStatus(item, "Hidden");
    if (action === "approve-organizer") setStatus(item, "Approved", "organizer-requests");
    if (action === "reject-organizer")  setStatus(item, "Rejected", "organizer-requests");
    if (action === "disable-organizer") setStatus(item, "Disabled");
    if (action === "enable-organizer")  setStatus(item, "Approved");
    if (action === "verify-card")       setStatus(item, "Approved");
    if (action === "request-docs" || action === "request-docs-card") setStatus(item, "Docs requested");
    if (action === "resolve-flag") {
      setStatus(item, "Resolved");
      updateCounter("review-flags", -1);
    }
  });
});

function applyLanguage(language) {
  currentLanguage = language;
  localStorage.setItem("hikeJordanLanguage", language);
  document.documentElement.lang = language === "ar" ? "ar" : "en";
  document.documentElement.dir = language === "ar" ? "rtl" : "ltr";
  translatePage(language);
  if (languageToggle) languageToggle.textContent = language === "ar" ? "English" : "العربية";
  applyFilters();
}

if (languageToggle) {
  languageToggle.addEventListener("click", () => {
    applyLanguage(currentLanguage === "ar" ? "en" : "ar");
  });
}

applyLanguage(currentLanguage);

/* ─── Scroll reveal ──────────────────────────────────────── */
const revealObserver = new IntersectionObserver(
  (entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        entry.target.classList.add("is-visible");
        revealObserver.unobserve(entry.target);
      }
    });
  },
  { threshold: 0.12, rootMargin: "0px 0px -40px 0px" }
);

document.querySelectorAll(".section-block, .value-strip, .organizer-cta").forEach((el, i) => {
  el.classList.add("reveal");
  el.style.transitionDelay = `${i * 0.04}s`;
  revealObserver.observe(el);
});

/* ─── Sidebar scroll spy ─────────────────────────────────── */
const sidebarLinks = document.querySelectorAll(".admin-sidebar a[href^='#']");
if (sidebarLinks.length) {
  const spyObserver = new IntersectionObserver(
    (entries) => {
      entries.forEach((entry) => {
        if (entry.isIntersecting) {
          sidebarLinks.forEach((link) => link.classList.remove("is-active"));
          const active = document.querySelector(`.admin-sidebar a[href="#${entry.target.id}"]`);
          if (active) active.classList.add("is-active");
        }
      });
    },
    { rootMargin: "-30% 0px -60% 0px" }
  );
  document.querySelectorAll(".admin-panels section[id]").forEach((section) => spyObserver.observe(section));
}

/* ─── Toast notifications ────────────────────────────────── */
function showToast(message, isError = false) {
  let shelf = document.getElementById("toastShelf");
  if (!shelf) {
    shelf = document.createElement("div");
    shelf.id = "toastShelf";
    shelf.className = "toast-shelf";
    document.body.appendChild(shelf);
  }
  const toast = document.createElement("div");
  toast.className = isError ? "toast toast--error" : "toast";
  toast.textContent = message;
  shelf.appendChild(toast);
  setTimeout(() => {
    toast.classList.add("is-leaving");
    toast.addEventListener("animationend", () => toast.remove(), { once: true });
  }, 2600);
}

/* ─── Admin form scroll restore (prevents scroll-to-top on POST) ── */
const SCROLL_KEY = "adminScrollY";

const savedY = sessionStorage.getItem(SCROLL_KEY);
if (savedY !== null) {
  sessionStorage.removeItem(SCROLL_KEY);
  requestAnimationFrame(() => window.scrollTo(0, parseInt(savedY, 10)));
}

document.querySelectorAll(".admin-panels form").forEach((form) => {
  form.addEventListener("submit", () => {
    sessionStorage.setItem(SCROLL_KEY, window.scrollY.toString());
  });
});

/* ─── Star picker (review form) ─────────────────────────── */
const starPicker = document.querySelector(".star-picker");
if (starPicker) {
  const labels = [...starPicker.querySelectorAll(".star-label")];
  labels.forEach((label, i) => {
    label.addEventListener("mouseenter", () =>
      labels.forEach((l, j) => l.querySelector(".star-icon").style.color = j <= i ? "var(--gold)" : "#ccc")
    );
  });
  starPicker.addEventListener("mouseleave", () => {
    const checked = starPicker.querySelector(".star-radio:checked");
    const val = checked ? parseInt(checked.value) - 1 : -1;
    labels.forEach((l, j) => l.querySelector(".star-icon").style.color = j <= val ? "var(--gold)" : "#ccc");
  });
  starPicker.querySelectorAll(".star-radio").forEach((radio, i) => {
    radio.addEventListener("change", () =>
      labels.forEach((l, j) => l.querySelector(".star-icon").style.color = j <= i ? "var(--gold)" : "#ccc")
    );
  });
}

/* ─── Show / hide password toggles ──────────────────────── */
document.querySelectorAll(".password-toggle").forEach((btn) => {
  const input = btn.previousElementSibling;
  if (!input || input.tagName !== "INPUT") return;
  btn.addEventListener("click", (e) => {
    e.preventDefault();
    const isPassword = input.type === "password";
    input.type = isPassword ? "text" : "password";
    btn.textContent = isPassword ? "Hide" : "Show";
    btn.setAttribute("aria-label", isPassword ? "Hide password" : "Show password");
  });
});

/* ─── Animated value counters ────────────────────────────── */
const valueNums = document.querySelectorAll(".value-grid strong");
if (valueNums.length) {
  const countObserver = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (!entry.isIntersecting) return;
      const el = entry.target;
      const target = parseInt(el.textContent, 10);
      if (isNaN(target) || target === 0) return;
      let current = 0;
      const step = Math.max(1, Math.ceil(target / 28));
      const timer = setInterval(() => {
        current = Math.min(current + step, target);
        el.textContent = current;
        if (current >= target) clearInterval(timer);
      }, 30);
      countObserver.unobserve(el);
    });
  }, { threshold: 0.8 });
  valueNums.forEach((el) => countObserver.observe(el));
}
