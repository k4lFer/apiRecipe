CREATE DATABASE `dbapirecipe`
CHARACTER SET utf8mb4
COLLATE utf8mb4_spanish_ci;

use dbapirecipe;

CREATE TABLE `authentications` (
  `id` varchar(36) PRIMARY KEY,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `role` ENUM ('Admin', 'Other', 'Logged') NOT NULL,
  `status` BOOLEAN NOT NULL
);

CREATE TABLE `users` (
  `id` char(36) PRIMARY KEY,
  `idAuthentication` char(36) NOT NULL,
  `firstName` varchar(100) NOT NULL,
  `lastName` varchar(100) NOT NULL,
  `email` varchar(50) UNIQUE NOT NULL,
  `createdAt` timestamp NOT NULL,
  `updatedAt` timestamp NOT NULL,
  FOREIGN KEY (`idAuthentication`) REFERENCES `authentications`(`id`)
);

CREATE TABLE `categories` (
  `id` char(36) PRIMARY KEY,
  `name` varchar(50) NOT NULL,
  `description` text NULL
);

CREATE TABLE `recipes` (
  `id` char(36) PRIMARY KEY,
  `idCategory` char(36) NOT NULL, 
  `title` varchar(100) NOT NULL,
  `description` text NOT NULL,
  `instruction` text NOT NULL,
  `ingredient` text NOT NULL,
  `preparation` timestamp NOT NULL,
  `cooking` timestamp NOT NULL,
  `estimated` timestamp NOT NULL,
  `difficulty` ENUM ('Easy', 'Half', 'Difficult') NOT NULL,
  `createdAt` timestamp NOT NULL,
  `updatedAt` timestamp NOT NULL,
  `createdBy` char(36) NULL,
  `updatedBy` char(36) NULL,
  FOREIGN KEY (`idCategory`) REFERENCES `categories`(`id`)
);

CREATE TABLE `likes` (
  `idRecipe` char(36) NOT NULL,
  `idUser` char(36) NOT NULL,
  PRIMARY KEY (`idRecipe`, `idUser`),
  FOREIGN KEY (`idRecipe`) REFERENCES `recipes` (`id`),
  FOREIGN KEY (`idUser`) REFERENCES `users` (`id`)
);

CREATE TABLE `ratings` (
  `id` char(36) PRIMARY KEY NOT NULL,
  `idRecipe` char(36) NOT NULL,
  `comment` text NOT NULL,
  `numberLike` BIGINT DEFAULT 0,
  `createdAt` timestamp NOT NULL,
  `updatedAt` timestamp NOT NULL,
  FOREIGN KEY (`idRecipe`) REFERENCES `recipes` (`id`)
);

CREATE TABLE `images` (
  `id` char(36) PRIMARY KEY,
  `idRecipe` char(36) NOT NULL,
  `url` char(255) NOT NULL,
  `createdAt` timestamp NOT NULL,
  `updatedAt` timestamp NOT NULL,
  FOREIGN KEY (`idRecipe`) REFERENCES `recipes` (`id`)
);

CREATE TABLE `videos` (
  `id` char(36) PRIMARY KEY,
  `idRecipe` char(36) NOT NULL,
  `title` varchar(100) NOT NULL,
  `url` char(255) NOT NULL,
  `description` text,
  `createdAt` timestamp NOT NULL,
  `updatedAt` timestamp NOT NULL,
  FOREIGN KEY (`idRecipe`) REFERENCES `recipes` (`id`)
);

CREATE TABLE `news` (
  `id` char(36) PRIMARY KEY,
  `idRecipe` char(36) NOT NULL,
  `title` text NOT NULL,
  `subtitle` text NOT NULL,
  `content` text NOT NULL,
  `status` BOOLEAN NOT NULL,
  `createdAt` timestamp NOT NULL,
  `deletedAt` timestamp NOT NULL,
  FOREIGN KEY (`idRecipe`) REFERENCES `recipes` (`id`)
);


INSERT INTO `authentications` (`id`, `username`, `password`, `role`, `status`)
VALUES 
('374949e5-e7c7-4d2a-84b4-42478ef1648a', 'lionos', '1001linux', 'Admin', true),
('8f59ff5a-8bd2-4120-ab7c-8335a855cb4b', 'rocos', '1001linux', 'Other', false);

INSERT INTO `users` (`id`, `idAuthentication`, `firstName`, `lastName`, `email`, `createdAt`, `updatedAt`)
VALUES 
('09e9f526-6727-48f0-bd9f-5562d9ff9f71', '374949e5-e7c7-4d2a-84b4-42478ef1648a', 'John', 'Doe', 'john.doe@example.com', NOW(), NOW()),
('5f0e139c-f711-431e-b16b-2fd704f53a85', '8f59ff5a-8bd2-4120-ab7c-8335a855cb4b', 'Jane', 'Smith', 'jane.smith@example.com', NOW(), NOW());


INSERT INTO categories (id, name, description) VALUES 
('6ffef303-f76b-478a-b9d4-ee912c6c0bcd', 'Postres', 'Recetas de postres deliciosos.');

INSERT INTO recipes (id, idCategory, title, description, instruction, ingredient, preparation, cooking, estimated, difficulty, createdAt, updatedAt) VALUES 
('e63dd814-ef78-489c-a127-806c1df2d22d', '6ffef303-f76b-478a-b9d4-ee912c6c0bcd', 'Tarta de Chocolate', 'Deliciosa tarta de chocolate con capas de crema.', 'Mezclar ingredientes, hornear, decorar.', 'Harina, huevos, chocolate, azúcar, mantequilla.', NOW(), NOW(), NOW(), 'Difficult', NOW(), NOW()),
('459e58c4-c303-4b02-9991-c49780a4a2d6', '6ffef303-f76b-478a-b9d4-ee912c6c0bcd', 'Cheesecake de Fresa', 'Cheesecake cremoso con fresas frescas.', 'Mezclar ingredientes, hornear, refrigerar, decorar con fresas.', 'Harina, huevos, fresas, azúcar, queso crema.', NOW(), NOW(), NOW(), 'Half', NOW(), NOW());

INSERT INTO images (id, idRecipe, url, createdAt, updatedAt) VALUES 
('9c299e59-1e78-4c6f-842f-49250700de59', 'e63dd814-ef78-489c-a127-806c1df2d22d', 'https://example.com/images/tarta_chocolate_1.jpg', NOW(), NOW()),
('045d0e80-21e6-4a39-928f-3cb8e94fc962', 'e63dd814-ef78-489c-a127-806c1df2d22d', 'https://example.com/images/tarta_chocolate_2.jpg', NOW(), NOW()),
('be3eae49-1b7e-4433-bb08-6d517daba31b', '459e58c4-c303-4b02-9991-c49780a4a2d6', 'https://example.com/images/cheesecake_fresa_1.jpg', NOW(), NOW()),
('bc9e5147-0643-449a-b3f9-33ca878974db', '459e58c4-c303-4b02-9991-c49780a4a2d6', 'https://example.com/images/cheesecake_fresa_2.jpg', NOW(), NOW());

INSERT INTO videos (id, idRecipe, title, url, description, createdAt, updatedAt) VALUES 
('84e2923c-48ef-4d57-8855-f0589fe6629f', 'e63dd814-ef78-489c-a127-806c1df2d22d', 'Preparación Tarta de Chocolate', 'https://example.com/videos/tarta_chocolate.mp4', 'Video paso a paso para preparar la tarta de chocolate.', NOW(), NOW()),
('d556a70f-0f9f-4182-9fcf-25aaf551a68c', '459e58c4-c303-4b02-9991-c49780a4a2d6', 'Preparación Cheesecake de Fresa', 'https://example.com/videos/cheesecake_fresa.mp4', 'Video tutorial para preparar cheesecake de fresa.', NOW(), NOW());


-- Inserción en la tabla de categorías
INSERT INTO categories (id, name, description) VALUES 
('b94b3f0c-3a6a-4a89-a840-722d6cfb1e9f', 'Entrantes', 'Recetas de entrantes deliciosos.');

INSERT INTO recipes (id, idCategory, title, description, instruction, ingredient, preparation, cooking, estimated, difficulty, createdAt, updatedAt) VALUES 
('bd763f60-60f7-4b87-b740-5f3061e1f8ed', 'b94b3f0c-3a6a-4a89-a840-722d6cfb1e9f', 'Bruschetta Clásica', 'Pan tostado con tomate, ajo y albahaca.', 'Tostar pan, mezclar tomate, ajo y albahaca, servir.', 'Pan, tomate, ajo, albahaca, aceite de oliva.', NOW(), NOW(), NOW(), 'Easy', NOW(), NOW()),
('928ab6f7-4b3d-4d75-bd7d-eda2c0d92b23', 'b94b3f0c-3a6a-4a89-a840-722d6cfb1e9f', 'Hummus con Pita', 'Delicioso hummus servido con pan de pita.', 'Mezclar ingredientes, servir con pita.', 'Garbanzos, ajo, tahini, limón, pita.', NOW(), NOW(), NOW(), 'Easy', NOW(), NOW());

INSERT INTO images (id, idRecipe, url, createdAt, updatedAt) VALUES 
('4a09d1f8-9d6c-4562-b5b7-72a83d5d6fcd', 'bd763f60-60f7-4b87-b740-5f3061e1f8ed', 'https://example.com/images/bruschetta_clasica.jpg', NOW(), NOW()),
('e4957dbb-3789-4f4c-b6f5-1a9a5e3cfeb8', '928ab6f7-4b3d-4d75-bd7d-eda2c0d92b23', 'https://example.com/images/hummus_pita.jpg', NOW(), NOW());

INSERT INTO videos (id, idRecipe, title, url, description, createdAt, updatedAt) VALUES 
('b49e9f29-cf1c-4a1b-88ea-4c287cd216af', 'bd763f60-60f7-4b87-b740-5f3061e1f8ed', 'Preparación Bruschetta Clásica', 'https://example.com/videos/bruschetta_clasica.mp4', 'Video paso a paso para preparar bruschetta clásica.', NOW(), NOW()),
('d19f2cb3-732e-48e0-92e6-9b7e0342dcf5', '928ab6f7-4b3d-4d75-bd7d-eda2c0d92b23', 'Preparación Hummus con Pita', 'https://example.com/videos/humus_pita.mp4', 'Video tutorial para preparar hummus con pita.', NOW(), NOW());


/*
SELECT p.`id`, p.`name`, p.description 
FROM `tproduct` AS p
ORDER BY p.`name`
LIMIT 5 OFFSET 3;
*/

SELECT 
    r.id AS recipe_id,
    r.title,
    r.description,
    r.instruction,
    r.ingredient,
    r.preparation,
    r.cooking,
    r.estimated,
    r.difficulty,
    r.createdAt,
    r.updatedAt,
    r.createdBy,
    r.updatedBy,
    i.id AS image_id,
    i.url AS image_url,
    v.id AS video_id,
    v.title AS video_title,
    v.url AS video_url,
    v.description AS video_description
FROM recipes r
LEFT JOIN images i ON r.id = i.idRecipe
LEFT JOIN videos v ON r.id = v.idRecipe
WHERE r.idCategory = '6ffef303-f76b-478a-b9d4-ee912c6c0bcd'
ORDER BY r.title
LIMIT 1 OFFSET 0;

