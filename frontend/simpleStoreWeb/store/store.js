import { makeAutoObservable } from "mobx";
import GetProducts from "../src/Services/Product/getProducts";

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
}

export default DataStore;
