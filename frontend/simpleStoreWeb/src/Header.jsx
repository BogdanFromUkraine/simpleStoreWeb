import React from "react";
import "./style/style.css";
import "./style/header.css";

const Header = () => {
  return (
    <header className="header">
      <div className="logo">WebShop</div>
      <nav className="nav-links">
        <a to="/" className="nav-link">
          Home
        </a>
        <a to="/shop" className="nav-link">
          Shop
        </a>
        <a to="/cart" className="nav-link cart-link">
          Cart
          <span className="cart-count">0</span>
        </a>
        <a to="*" className="nav-link">
          Sign in
        </a>
        <a to="*" className="nav-link">
          Sign up
        </a>
      </nav>
    </header>
  );
};

export default Header;
