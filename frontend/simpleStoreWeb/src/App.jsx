import { useState } from "react";
import Header from "./Header";
import Footer from "./Footer";
import HeroSection from "./HeroSection";
import ScrollToTop from "react-scroll-to-top";
import Shop from "./Shop";
import ImageCarousel from "./AdditionalComponents/ImageCarousel";
import "./style/style.css";

function App() {
  const [count, setCount] = useState(0);

  return (
    <div>
      <Header />
      <HeroSection />
      <ImageCarousel />
      <Shop />
      <ScrollToTop
        smooth
        component={<button className="scroll_button">↑</button>}
        style={{ border: "none", background: "transparent" }} // забираю базовий стиль компонента
      />
      <Footer />
    </div>
  );
}

export default App;
