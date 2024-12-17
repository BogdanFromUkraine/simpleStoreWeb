import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import App from "./App.jsx";
import { RootStoreContext } from "../store/root-store-context";
import DataStore from "../store/store.js";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import Home from "./Home.jsx";
import Cart from "./Cart.jsx";
import SignInPage from "./SignInPage.jsx";
import SignUpPage from "./SignUpPage.jsx";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "/", element: <Home /> },
      { path: "cart", element: <Cart /> },
      { path: "signIn", element: <SignInPage /> },
      { path: "signUp", element: <SignUpPage /> },
    ],
  },
]);
createRoot(document.getElementById("root")).render(
  <RootStoreContext.Provider value={new DataStore()}>
    <RouterProvider router={router}>
      <StrictMode>
        <App />
      </StrictMode>
    </RouterProvider>
  </RootStoreContext.Provider>
);
