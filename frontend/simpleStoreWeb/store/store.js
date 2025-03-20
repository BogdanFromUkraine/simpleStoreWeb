import { makeAutoObservable } from "mobx";
import GetProducts from "../src/Services/Product/getProducts";
import AddProductFunc from "../src/Services/Product/addProduct";
import RemoveProduct from "../src/Services/Product/removeProduct";
import GetUserCart from "../src/Services/Cart/getUserCart";
import RemoveProductFromCart from "../src/Services/Cart/removeProductFromCart";
import AddProductToCart from "../src/Services/Cart/addProductToCart";
import LoginUser from "../src/Services/Authorization/loginUser";
import RegisterUser from "../src/Services/Authorization/registerUser";

class DataStore {
  constructor() {
    makeAutoObservable(this);
  }

  products = [];
  ItemsOfCart = [];
  response = Boolean;

  get_User_Cart = async (userId) => {
    try {
      this.ItemsOfCart = await GetUserCart(userId);
    } catch (error) {}
  };

  get_Products = async () => {
    try {
      this.products = await GetProducts();
    } catch (error) {}
  };

  add_Product = async (name, description, price, stock) => {
    try {
      await AddProductFunc(name, description, price, stock);
      await this.get_Products();
    } catch (error) {}
  };

  remove_Product = async (name) => {
    try {
      await RemoveProduct(name);
      await this.get_Products();
    } catch (error) {}
  };

  remove_Product_From_Cart = async (userId, productId) => {
    try {
      await RemoveProductFromCart(userId, productId);
    } catch (error) {}
  };

  add_Product_To_Cart = async (userId, productId) => {
    try {
      await AddProductToCart(userId, productId);
    } catch (error) {}
  };

  sign_in = async (email, password) => {
    try {
      const res = await LoginUser(email, password);
      this.response = res;
    } catch (error) {}
  };

  sign_up = async (userName, email, password) => {
    try {
      const res = await RegisterUser(userName, email, password);
      this.response = res;
    } catch (error) {}
  };
}

export default DataStore;
