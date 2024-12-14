import { useState } from "react";
import Header from "./Header";
import Footer from "./Footer";
import HeroSection from "./HeroSection";
import ProductCard from "./ProductCard";
import Shop from "./Shop";

function App() {
  const [count, setCount] = useState(0);

  return (
    <div>
      <Header />
      <HeroSection />
      <Shop />

      <Footer />
    </div>
  );
}

export default App;
