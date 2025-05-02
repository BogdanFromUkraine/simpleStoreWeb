# 1. Використовуємо офіційний Node образ для білду
FROM node:20-alpine AS builder

# 2. Робоча директорія всередині контейнера
WORKDIR /app

# 3. Копіюємо package.json та package-lock.json
COPY package*.json ./

# 4. Встановлюємо залежності
RUN npm install

# 5. Копіюємо решту файлів проєкту
COPY . .

# 6. Білдимо фронтенд
RUN npm run build

# 7. Використовуємо lightweight сервер для продакшену
FROM nginx:alpine

# 8. Копіюємо зібраний фронтенд у nginx
COPY --from=builder /app/dist /usr/share/nginx/html

# 9. Копіюємо налаштування nginx (якщо потрібно)
# COPY nginx.conf /etc/nginx/nginx.conf

# 10. Відкриваємо порт
EXPOSE 80

# 11. Запускаємо nginx
CMD ["nginx", "-g", "daemon off;"]
