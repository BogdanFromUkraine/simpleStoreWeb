import React, { useState } from "react";
import "./style/stylesToAuthorization.css";
import LoginUser from "./Services/Authorization/loginUser";
import { useStores } from "../store/root-store-context";
import { observer } from "mobx-react-lite";

export const SignInPage = observer(() => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });

  const { dataStore, notificationStore } = useStores();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    // Тут буде логіка для відправки даних на сервер для входу
    var test = await dataStore.sign_in(formData.email, formData.password);
    //логіка toastr
    if (dataStore.response === true) {
      notificationStore.notify("logged in successfully!", "success");
      setTimeout(() => {
        window.location.href = "https://localhost:5173/"; // Перехід після 2 секунд
      }, 5000); // 2000 мс = 2 секунди
    } else {
      notificationStore.notify("something wrong!", "error");
    }
  };

  return (
    <div className="container">
      <div className="auth-container">
        <h2>Sign In</h2>
        <form onSubmit={handleSubmit} className="auth-form">
          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              required
            />
          </div>
          <button type="submit">Sign In</button>
        </form>
      </div>
    </div>
  );
});
