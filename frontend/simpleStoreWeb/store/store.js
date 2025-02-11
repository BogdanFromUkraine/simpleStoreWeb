import { makeAutoObservable } from "mobx";
import GetProducts from "../src/Services/Product/getProducts";
import AddProductFunc from "../src/Services/Product/addProduct";
import RemoveProduct from "../src/Services/Product/removeProduct";

class DataStore {
  constructor() {
    makeAutoObservable(this);
  }

  products = [];

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
}

export default DataStore;
