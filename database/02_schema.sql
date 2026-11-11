Create DATABASE music_school;


DROP TABLE IF EXISTS order_items;
DROP TABLE IF EXISTS orders;
DROP TABLE IF EXISTS groups;
DROP TABLE IF EXISTS programs;
DROP TABLE IF EXISTS students;
DROP TABLE IF EXISTS teachers;
DROP TABLE IF EXISTS categories;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS roles;
GO

USE music_school;
GO

CREATE TABLE roles (
    role_id   int IDENTITY(1, 1) PRIMARY KEY,
    role_name nvarchar(50) NOT NULL UNIQUE
);

CREATE TABLE users (
    user_id     int IDENTITY(1, 1) PRIMARY KEY,
    login       nvarchar(50) NOT NULL UNIQUE,
    password    nvarchar(100) NOT NULL,
    last_name   nvarchar(60) NOT NULL,
    first_name  nvarchar(60) NOT NULL,
    middle_name nvarchar(60) NULL,
    role_id     int NOT NULL FOREIGN KEY REFERENCES roles (role_id)
);

CREATE TABLE students (
    student_id    int IDENTITY(1, 1) PRIMARY KEY,
    user_id       int NOT NULL UNIQUE FOREIGN KEY REFERENCES users (user_id),
    birth_date    date NULL
);

CREATE TABLE teachers (
    teacher_id      int IDENTITY(1, 1) PRIMARY KEY,
    user_id         int NOT NULL UNIQUE FOREIGN KEY REFERENCES users (user_id),
    phone           nvarchar(20) NULL,
    specialization  nvarchar(100) NOT NULL -- Исправлена опечатка (было cspecialization)
);

CREATE TABLE categories (
    category_id   int IDENTITY(1, 1) PRIMARY KEY,
    category_name nvarchar(100) NOT NULL UNIQUE
);

CREATE TABLE programs (
    program_id     int IDENTITY(1, 1) PRIMARY KEY,
    category_id    int NOT NULL FOREIGN KEY REFERENCES categories (category_id),
    program_name   nvarchar(150) NOT NULL,
    description    nvarchar(max) NULL,
    base_price     decimal(10, 2) NOT NULL CHECK (base_price > 0)
);

CREATE TABLE groups (
    group_id     int IDENTITY(1, 1) PRIMARY KEY,
    program_id   int NOT NULL FOREIGN KEY REFERENCES programs (program_id),
    teacher_id   int NOT NULL FOREIGN KEY REFERENCES teachers (teacher_id),
    group_name   nvarchar(100) NOT NULL,
    schedule     nvarchar(200) NULL,
    available_seats int NOT NULL DEFAULT 0 CHECK (available_seats >= 0) -- Разрешаем 0 мест
);

CREATE TABLE orders (
    order_id   int IDENTITY(1, 1) PRIMARY KEY,
    order_date date NOT NULL CONSTRAINT df_orders_date DEFAULT (CAST(GETDATE() AS date)),
    student_id int NOT NULL FOREIGN KEY REFERENCES students (student_id)
);

CREATE TABLE order_items (
    order_item_id int IDENTITY(1, 1) PRIMARY KEY,
    order_id      int NOT NULL FOREIGN KEY REFERENCES orders (order_id) ON DELETE CASCADE,
    group_id      int NOT NULL FOREIGN KEY REFERENCES groups (group_id),
    unit_price    decimal(10, 2) NOT NULL CHECK (unit_price >= 0)
);
GO

INSERT INTO roles (role_name) VALUES 
(N'Администратор'), 
(N'Преподаватель'), 
(N'Ученик');

INSERT INTO users (login, password, last_name, first_name, middle_name, role_id) VALUES
(N'admin', N'123', N'Иванов', N'Иван', N'Иванович', 1),
(N'petrova', N'123', N'Петрова', N'Анна', N'Сергеевна', 2),
(N'sidorov', N'123', N'Сидоров', N'Олег', N'Петрович', 2);

INSERT INTO users (login, password, last_name, first_name, middle_name, role_id) VALUES
(N'student1', N'123', N'Смирнов', N'Алексей', N'Дмитриевич', 3),
(N'student2', N'123', N'Кузнецова', N'Мария', N'Андреевна', 3);

INSERT INTO students (user_id, birth_date) VALUES
(4, '2010-05-15'),
(5, '2012-08-20');

INSERT INTO teachers (user_id, phone, specialization) VALUES
(2, '+7(999)111-22-33', N'Фортепиано'),
(3, '+7(999)444-55-66', N'Вокал');

INSERT INTO categories (category_name) VALUES
(N'Струнные инструменты'),
(N'Фортепиано'),
(N'Вокальное искусство');

INSERT INTO programs (category_id, program_name, description, base_price) VALUES
(2, N'Фортепиано для начинающих', N'Базовый курс игры на фортепиано', 5000.00),
(3, N'Академический вокал', N'Постановка голоса, дыхание', 4500.00),
(1, N'Гитара', N'Игра на шестиструнной гитаре', 4000.00);

INSERT INTO groups (program_id, teacher_id, group_name, schedule, available_seats) VALUES
(1, 1, N'Фортепиано (Группа 1)', N'Пн, Ср 16:00', 5),
(2, 2, N'Вокал (Группа А)', N'Вт, Чт 18:00', 8),
(3, 1, N'Гитара (Индивидуально)', N'Сб 12:00', 1);

INSERT INTO orders (order_date, student_id) VALUES
('2026-10-25', 1),
('2026-10-26', 2);

INSERT INTO order_items (order_id, group_id, unit_price) VALUES
(1, 1, 5000.00),
(2, 2, 4500.00);
GO


USE music_school;
SELECT 'roles' AS table_name, COUNT(*) AS rows_count FROM dbo.roles
UNION ALL SELECT 'users', COUNT(*) FROM dbo.users
UNION ALL SELECT 'students', COUNT(*) FROM dbo.students
UNION ALL SELECT 'teachers', COUNT(*) FROM dbo.teachers
UNION ALL SELECT 'categories', COUNT(*) FROM dbo.categories
UNION ALL SELECT 'programs', COUNT(*) FROM dbo.programs
UNION ALL SELECT 'groups', COUNT(*) FROM dbo.groups
UNION ALL SELECT 'orders', COUNT(*) FROM dbo.orders
UNION ALL SELECT 'order_items', COUNT(*) FROM dbo.order_items;

USE music_school;
SELECT * FROM orders;

UPDATE groups SET available_seats = 10;

-- Проверяем заявки
SELECT order_id, order_date, student_id FROM orders;