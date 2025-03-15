export default async function logout() {
  localStorage.removeItem("token"); // Видаляємо токен
  window.location.href = "https://localhost:5173/"; // Перенаправляємо користувача

  return false;
}
