import { useState } from "react";
import styles from "./style/addProduct.module.css";
import { useStores } from "../store/root-store-context.ts";

export default function AddProduct() {
  const [formData, setFormData] = useState({
    name: "",
    price: "",
    description: "",
    stock: "",
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [isAdding, setIsAdding] = useState(true); // Додаємо стан для перемикання режиму

  const { dataStore, add_Product, remove_Product } = useStores();

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!isAdding) return; // Якщо режим "видалення", не виконуємо додавання
    setLoading(true);
    setError(null);

    try {
      await dataStore.add_Product(
        formData.name,
        formData.description,
        formData.price,
        formData.stock
      );

      if (!response.ok) throw new Error("Помилка додавання продукту");
      alert("Продукт успішно додано!");
      setFormData({ name: "", price: "", description: "" });
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (e) => {
    e.preventDefault();
    if (isAdding) return; // Якщо режим "додавання", не виконуємо видалення

    setLoading(true);
    setError(null);

    try {
      //видалення product
      await dataStore.remove_Product(formData.name);

      alert("Продукт успішно видалено!");
      setFormData({ name: "", price: "", description: "", stock: "" });
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.addProductContainer}>
      <div className={styles.addProductForm}>
        <h2>{isAdding ? "Add product" : "Remove product"}</h2>

        {/* Кнопка для перемикання режиму */}
        <button
          className={styles.toggleButton}
          onClick={() => setIsAdding(!isAdding)}
        >
          {isAdding ? "Go to remove" : "Go to add"}
        </button>

        {/* <form onSubmit={isAdding ? handleSubmit : handleDelete}>
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
        </form> */}
        <form onSubmit={isAdding ? handleSubmit : handleDelete}>
          <input
            type="text"
            name="name"
            placeholder="Name of product"
            value={formData.name}
            onChange={handleChange}
            required
          />
          {isAdding && (
            <>
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
            </>
          )}

          {error && <p className={styles.errorMessage}>{error}</p>}
          <button
            type="submit"
            disabled={loading}
            className={styles.submitButton}
          >
            {loading
              ? "Pending..."
              : isAdding
              ? "Add product"
              : "Remove product"}
          </button>
        </form>
      </div>
    </div>
  );
}
