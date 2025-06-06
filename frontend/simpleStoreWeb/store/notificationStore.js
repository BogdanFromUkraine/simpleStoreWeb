import { makeAutoObservable } from "mobx";
import { toast } from "react-toastify";

class NotificationStore {
  constructor() {
    makeAutoObservable(this);
  }

  notify(message, type = "info") {
    if (type === "success") toast.success(message);
    else if (type === "error") toast.error(message);
    else toast.info(message);
  }
}

export default NotificationStore;
