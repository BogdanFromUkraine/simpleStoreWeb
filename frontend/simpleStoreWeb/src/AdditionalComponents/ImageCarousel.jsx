import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import "../style/imageCarousel.css";

import img1 from "../img/crew-4Hg8LH9Hoxc-unsplash.jpg";
import img2 from "../img/james-mckinven-tpuAo8gVs58-unsplash.jpg";
import img3 from "../img/luca-bravo-9l_326FISzk-unsplash.jpg";

const ImageCarousel = () => {
  const images = [img1, img2, img3];

  const settings = {
    dots: true, // Показує точки навігації
    infinite: true, // Безкінечна прокрутка
    speed: 500, // Швидкість переходу (мс)
    slidesToShow: 1, // Кількість видимих слайдів
    slidesToScroll: 1, // Кількість слайдів при прокрутці
    autoplay: true, // Автоматична прокрутка
    autoplaySpeed: 3000, // Затримка для автопрокрутки
  };

  return (
    <div className="carousel-container">
      <Slider {...settings}>
        {images.map((img, index) => (
          <div key={index}>
            <img
              src={img}
              alt={`Slide ${index + 1}`}
              className="carousel-image"
            />
          </div>
        ))}
      </Slider>
    </div>
  );
};

export default ImageCarousel;
