import React from "react";
import "./style/style.css";
import "./style/header.css";
import { Link } from "react-router-dom";

const Header = () => {
  return (
    <header className="header">
      <div className="logo">WebShop</div>
      <nav className="nav-links">
        <Link to="/addProduct" className="nav-link">
          Add Product(для адміна)
        </Link>
        <Link to="/" className="nav-link">
          Home
        </Link>
        <a to="/shop" className="nav-link">
          Shop
        </a>
        <Link to="/cart" className="nav-link cart-link">
          Cart
          <span className="cart-count">0</span>
        </Link>
        <Link to="/signIn" className="nav-link">
          Sign in
        </Link>
        <Link to="/signUp" className="nav-link">
          Sign up
        </Link>
      </nav>
    </header>
  );
};

export default Header;
