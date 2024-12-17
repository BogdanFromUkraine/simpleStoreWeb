import React, { useEffect } from "react";
import "../style/snowFall.css";

const Snowfall = () => {
  useEffect(() => {
    const snowContainer = document.querySelector(".snow-container");
    const snowflakeCount = 50; // Кількість сніжинок

    for (let i = 0; i < snowflakeCount; i++) {
      const snowflake = document.createElement("div");
      snowflake.className = "snowflake";
      snowflake.textContent = "❄"; // Символ сніжинки

      // Рандомні стилі для кожної сніжинки
      snowflake.style.left = `${Math.random() * 100}%`;
      snowflake.style.animationDuration = `${3 + Math.random() * 5}s`;
      snowflake.style.fontSize = `${10 + Math.random() * 20}px`;

      snowContainer.appendChild(snowflake);
    }

    return () => {
      snowContainer.innerHTML = ""; // Очищення контейнера при демонтажі
    };
  }, []);

  return <div className="snow-container"></div>;
};

export default Snowfall;
