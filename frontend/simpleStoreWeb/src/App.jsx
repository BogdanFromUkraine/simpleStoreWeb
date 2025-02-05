import Header from "./Header";
import Footer from "./Footer";
import "./style/style.css";
import Snowfall from "./AdditionalComponents/SnowFall";
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
