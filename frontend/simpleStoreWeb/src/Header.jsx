import React, { useEffect, useState } from "react";
import "./style/style.css";
import "./style/header.css";
import { Link } from "react-router-dom";
import getUserRole from "./utils/getUserRole";
import getUserId from "./utils/getUserId";
import logout from "./utils/logout";

const Header = () => {
  const [isAdmin, setIsAdmin] = useState();
  const [isAuth, setIsAuth] = useState(false);

  useEffect(() => {
    async function ifUserIsAdmin() {
      if ((await getUserRole()) == "Admin") {
        setIsAdmin(true);
      }
    }
    async function ifUserAuth() {
      if ((await getUserId()) != null) {
        setIsAuth(true);
      }
    }
    ifUserIsAdmin();
    ifUserAuth();
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
        <Link to="/cart" className="nav-link cart-link">
          Cart
        </Link>

        {isAuth ? (
          <Link to="#" className="nav-link">
            <a onClick={() => setIsAuth(logout())}>Exit</a>
          </Link>
        ) : (
          <>
            <Link to="/signIn" className="nav-link">
              Sign in
            </Link>
            <Link to="/signUp" className="nav-link">
              Sign up
            </Link>
          </>
        )}
      </nav>
    </header>
  );
};

export default Header;
