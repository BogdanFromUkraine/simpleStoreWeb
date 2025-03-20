import { Children, createContext, useContext } from "react";
import DataStore from "./store";
import NotificationStore from "./notificationStore";

export const RootStoreContext = createContext({
  dataStore: new DataStore(),
  notificationStore: new NotificationStore(),
});

export const useStores = () => {
  const context = useContext(RootStoreContext);
  if (context === null) {
    throw new Error("");
  }
  return context;
};
