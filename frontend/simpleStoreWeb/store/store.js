import { makeAutoObservable } from "mobx";

class DataStore {
  constructor() {
    makeAutoObservable(this);
  }
}

export default DataStore;
