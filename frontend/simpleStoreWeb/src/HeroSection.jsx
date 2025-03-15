import React from "react";
import "./style/style.css";

const HeroSection = () => {
  return (
    <section className="hero">
      <div className="hero-content">
        <h1>Welcome to WebShop</h1>
        <p>Discover our amazing products!</p>
        <button>
          <a
            href="#section_shop"
            style={{ all: "unset", display: "inline", cursor: "pointer" }}
          >
            Shop Now
          </a>
        </button>
      </div>
    </section>
  );
};

export default HeroSection;
