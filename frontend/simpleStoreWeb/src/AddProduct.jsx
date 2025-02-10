import { useState } from "react";
import styles from "./style/addProduct.module.css";
import { useStores } from "../store/root-store-context";

export default function AddProduct() {
  const [formData, setFormData] = useState({
    name: "",
    price: "",
    description: "",
    stock: "",
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const { add_Product } = useStores();

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      await add_Product(
        formData.name,
        formData.description,
        formData.price,
        formData.stock
      );
      //   const response = await fetch("/api/products", {
      //     method: "POST",
      //     headers: { "Content-Type": "application/json" },
      //     body: JSON.stringify(formData),
      //   });

      if (!response.ok) throw new Error("Помилка додавання продукту");
      alert("Продукт успішно додано!");
      setFormData({ name: "", price: "", description: "" });
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.addProductContainer}>
      <div className={styles.addProductForm}>
        <h2>Додати продукт</h2>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            name="name"
            placeholder="Name of product"
            value={formData.name}
            onChange={handleChange}
            required
          />
          <input
            type="number"
            name="price"
            placeholder="Price"
            value={formData.price}
            onChange={handleChange}
            required
          />
          <input
            type="text"
            name="description"
            placeholder="Description"
            value={formData.description}
            onChange={handleChange}
          />
          <input
            type="text"
            name="stock"
            placeholder="Stock"
            value={formData.stock}
            onChange={handleChange}
          />
          {error && <p className={styles.errorMessage}>{error}</p>}
          <button
            type="submit"
            disabled={loading}
            className={styles.submitButton}
          >
            {loading ? "Додається..." : "Add product"}
          </button>
        </form>
      </div>
    </div>
  );
}
