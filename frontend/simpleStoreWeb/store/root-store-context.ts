import { Children, createContext, useContext } from "react";
import store from "./store";
import DataStore from "./store";

export const RootStoreContext = createContext<DataStore | null>(null);

export const useStores = () => {
  const context = useContext(RootStoreContext);
  if (context === null) {
    throw new Error("");
  }
  return context;
};
