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
    if (response.status == 200) {
      return true;
    } else {
      return false;
    }
  } catch (error) {
    console.log(error.message);
  }
}
