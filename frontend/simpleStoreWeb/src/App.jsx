import Header from "./Header";
import Footer from "./Footer";
import "./style/style.css";
import Snowfall from "./AdditionalComponents/SnowFall";
import { Outlet } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function App() {
  return (
    <>
      <Snowfall />
      <Header />
      <Outlet />
      <Footer />
      <ToastContainer />
    </>
  );
}

export default App;
