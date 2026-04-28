# 1. Використовуємо новий офіційний образ (Quarkus based)
FROM quay.io/keycloak/keycloak:23.0.0

# 2. Копіюємо файл у спеціальну папку для авто-імпорту
COPY ./realm.json /opt/keycloak/data/import/realm.json

# 3. Нові назви змінних для адміна (USER -> ADMIN)
ENV KEYCLOAK_ADMIN=admin
ENV KEYCLOAK_ADMIN_PASSWORD=admin

# 4. Вказуємо команду запуску з прапорцем імпорту
ENTRYPOINT ["/opt/keycloak/bin/kc.sh"]
CMD ["start-dev", "--import-realm"]