import { useState } from "react";
import Header from "./Header";
import Footer from "./Footer";
import HeroSection from "./HeroSection";
import ScrollToTop from "react-scroll-to-top";
import Shop from "./Shop";
import ImageCarousel from "./AdditionalComponents/ImageCarousel";
import "./style/style.css";
import Snowfall from "./AdditionalComponents/SnowFall";
import Cart from "./Cart";
import Home from "./Home";
import { Outlet } from "react-router-dom";

function App() {
  return (
    <>
      <Snowfall />
      <Header />
      <Outlet />
      <Footer />
    </>
  );
}

export default App;
