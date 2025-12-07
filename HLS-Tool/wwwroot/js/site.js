// Apply saved theme on load
document.addEventListener("DOMContentLoaded", function () {
    const toggleBtn = document.getElementById("themeToggle");
    if (!toggleBtn) return;

    const darkIcon = "/darkmode.svg";
    const lightIcon = "/lightmode.svg";

    // Create an img element inside the button
    let icon = document.createElement("img");
    icon.src = darkIcon; // default
    icon.alt = "Toggle Theme";
    icon.style.height = "24px";
    icon.style.width = "24px";
    toggleBtn.innerHTML = ""; // clear any content
    toggleBtn.appendChild(icon);

    // Apply saved theme on load
    const isDark = localStorage.getItem("theme") === "dark";
    if (isDark) document.body.classList.add("dark-theme");

    // Set correct icon on load
    icon.src = isDark ? lightIcon : darkIcon;

    // Add click listener
    toggleBtn.addEventListener("click", function () {
        document.body.classList.toggle("dark-theme");

        const nowDark = document.body.classList.contains("dark-theme");
        localStorage.setItem("theme", nowDark ? "dark" : "light");

        // Swap icon
        icon.src = nowDark ? lightIcon : darkIcon;
    });
});