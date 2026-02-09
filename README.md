### Warehouse API 

API для управления складом металлических рулонов на ASP.NET Core с поддержкой всех CRUD операций, фильтрацией и аналитикой.

### Возможности

- Добавление рулонов с валидацией данных
- Удаление рулонов (остаются в истории)
- Фильтрация по ID, длине, весу с пагинацией
- Статистика за период с расчетом времени хранения
- Конфигурация БД через переменные окружения или файлы
- Swagger UI для тестирования API

### 1. Клонирование

git clone https://github.com/lady-imba/warehouse-api.git
cd warehouse-api/Warehouse.API

### 2. Настройка БД

Способ 1: Через .env файл. Создайте файл .env в папке проекта:
DB_CONNECTION_STRING=Data Source=warehouse.db

Способ 2: Через переменные окружения:
export DB_CONNECTION_STRING="Data Source=warehouse.db"
dotnet run

Способ 3: Через appsettings.json:
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=warehouse.db"
  }
}

### 3. Старт

dotnet run

Проект автоматически:
Прочитает настройки из .env файла (если есть)
Создаст базу данных warehouse.db
Запустит сервер на http://localhost:5249

### 4. Документация

http://localhost:5249/swagger/index.html

### 5. Endpoints

1. POST /api/rolls - Добавить рулон
Проверяет, что длина и вес > 0 и числовые значения

2. DELETE /api/rolls/{id} - Удалить рулон
Меняются removedDate и isRemoved, запись остается в БД

3. GET /api/rolls - Получить рулоны
Без параметров: все рулоны
С параметрами: фильтрация по диапазонам (можно комбинировать)
Параметры:
IdRange.Min/Max - фильтр по ID
LengthRange.Min/Max - фильтр по длине
WeightRange.Min/Max - фильтр по весу
Page, PageSize - пагинация

4. GET /api/rolls/statistics - Статистика
Без параметров: данные за последние 30 дней
С параметрами: статистика за указанный период
Учитываются все рулоны, добавленные в период (включая удаленные)

### 6. Технологии

Backend: ASP.NET Core 8.0 Web API
База данных: SQLite с Entity Framework Core
Документация: Swagger/OpenAPI
Конфигурация: JSON + Environment Variables
