import axios from "axios";
import data from "../../dataJSON/data.json";

export default async function LoginUser(email, password) {
  try {
    const response = await axios.post(
      data.localhost + "/auth/login",
      {
        email: email,
        password: password,
      },
      {
        headers: {
          "Content-Type": "application/json", // Обов'язковий заголовок для JSON
        },
      }
    );

    // Зберігаємо JWT токен у localStorage
    localStorage.setItem("token", await response.data);
    console.log(await response);
    alert("Вхід успішний!");

    window.location.href = "https://localhost:5173/"; // Перенаправлення на захищену сторінку
    return null;
  } catch (error) {
    console.log(error.message);
  }
}
