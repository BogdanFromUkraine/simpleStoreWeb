import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import App from "./App.jsx";
import DataStore from "../store/store.js";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import Home from "./Home.jsx";
import { Cart } from "./Cart.jsx";
import { SignInPage } from "./SignInPage.jsx";
import SignUpPage from "./SignUpPage.jsx";
import AddProduct from "./AddProduct.jsx";
import NotificationStore from "../store/notificationStore.js";
import { RootStoreContext } from "../store/root-store-context.ts";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "/", element: <Home /> },
      { path: "cart", element: <Cart /> },
      { path: "signIn", element: <SignInPage /> },
      { path: "signUp", element: <SignUpPage /> },
      { path: "addProduct", element: <AddProduct /> },
    ],
  },
]);

// Створюємо екземпляри store
const dataStore = new DataStore();
const notificationStore = new NotificationStore();

createRoot(document.getElementById("root")).render(
  <RootStoreContext.Provider value={{ dataStore, notificationStore }}>
    <RouterProvider router={router}>
      <StrictMode>
        <App />
      </StrictMode>
    </RouterProvider>
  </RootStoreContext.Provider>
);
