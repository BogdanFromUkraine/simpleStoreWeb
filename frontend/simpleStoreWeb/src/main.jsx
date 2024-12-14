import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import App from "./App.jsx";
import { RootStoreContext } from "../store/root-store-context";
import DataStore from "../store/store.js";

createRoot(document.getElementById("root")).render(
  <RootStoreContext.Provider value={new DataStore()}>
    <StrictMode>
      <App />
    </StrictMode>
  </RootStoreContext.Provider>
);
