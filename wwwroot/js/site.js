/* HikeJordan community — lightweight progressive enhancement.
   Everything here is optional; the site is fully functional without JS. */

/* ─── Reset any stale RTL/lang from the previous version of the site ─── */
try {
  localStorage.removeItem("hikeJordanLanguage");
} catch { /* ignore */ }
document.documentElement.lang = "en";
document.documentElement.dir = "ltr";

/* ─── Show / hide password toggles ──────────────────────────────────── */
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

/* ─── Auto-grow textareas ───────────────────────────────────────────── */
function autoGrow(el) {
  el.style.height = "auto";
  el.style.height = Math.min(el.scrollHeight, 420) + "px";
}
document.querySelectorAll("textarea").forEach((area) => {
  autoGrow(area);
  area.addEventListener("input", () => autoGrow(area));
});

/* ─── Live character counters ───────────────────────────────────────── */
document.querySelectorAll("textarea[maxlength]").forEach((area) => {
  const counter = area.parentElement?.querySelector(".char-count");
  if (!counter) return;
  const update = () => {
    const remaining = area.maxLength - area.value.length;
    counter.textContent = `${remaining}`;
    counter.classList.toggle("low", remaining <= 40);
  };
  update();
  area.addEventListener("input", update);
});

/* ─── Highlight the active top-nav link ─────────────────────────────── */
(() => {
  const path = window.location.pathname.toLowerCase();
  document.querySelectorAll(".site-nav .nav-link").forEach((link) => {
    const href = (link.getAttribute("href") || "").toLowerCase();
    if (!href || href === "#") return;
    const isFeed = href === "/" && (path === "/" || path === "");
    const isMatch = href !== "/" && path.startsWith(href);
    if (isFeed || isMatch) link.classList.add("nav-active");
  });
})();
