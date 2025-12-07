// Apply saved theme on load
document.addEventListener("DOMContentLoaded", function () {
    const isDark = localStorage.getItem("theme") === "dark";
    if (isDark) document.body.classList.add("dark-theme");

    const toggleBtn = document.getElementById("themeToggle");
    if (!toggleBtn) return;

    toggleBtn.textContent = isDark ? "☀️ Light Mode" : "🌙 Dark Mode";

    toggleBtn.addEventListener("click", function () {
        document.body.classList.toggle("dark-theme");

        const nowDark = document.body.classList.contains("dark-theme");
        localStorage.setItem("theme", nowDark ? "dark" : "light");

        toggleBtn.textContent = nowDark ? "☀️ Light Mode" : "🌙 Dark Mode";
    });
});