import React, { useEffect, useState } from "react";
import "./style/style.css";
import "./style/header.css";
import { Link } from "react-router-dom";
import getUserRole from "./utils/getUserRole";

const Header = () => {
  const [isAdmin, setIsAdmin] = useState();
  useEffect(() => {
    async function ifUserIsAdmin() {
      if ((await getUserRole()) == "Admin") {
        setIsAdmin(true);
      }
    }
    ifUserIsAdmin();
  }, []);
  return (
    <header className="header">
      <div className="logo">WebShop</div>
      <nav className="nav-links">
        {isAdmin ? (
          <Link to="/addProduct" className="nav-link">
            Add-Delete Product(для адміна)
          </Link>
        ) : null}

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
