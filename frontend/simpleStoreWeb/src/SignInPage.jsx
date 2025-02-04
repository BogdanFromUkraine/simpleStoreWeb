import React, { useState } from "react";
import "./style/stylesToAuthorization.css";
import LoginUser from "./Services/Authorization/loginUser";

const SignInPage = () => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    // Тут буде логіка для відправки даних на сервер для входу
    var test = await LoginUser(formData.email, formData.password);
    console.log("Form Data:", formData);
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
};

export default SignInPage;
