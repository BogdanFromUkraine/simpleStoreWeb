import HeroSection from "./HeroSection";
import ImageCarousel from "./AdditionalComponents/ImageCarousel";
import { Shop } from "./Shop";
import ScrollToTop from "react-scroll-to-top";

function Home() {
  return (
    <>
      <HeroSection />
      <ImageCarousel />
      <Shop />
      <ScrollToTop
        smooth
        component={<button className="scroll_button">↑</button>}
        style={{ border: "none", background: "transparent" }} // забираю базовий стиль компонента
      />
    </>
  );
}

export default Home;
