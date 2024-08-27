CREATE DATABASE `dbapirecipe`
CHARACTER SET utf8mb4
COLLATE utf8mb4_spanish_ci;

use dbapirecipe;

CREATE TABLE `authentications` (
  `id` varchar(36) PRIMARY KEY,
  `username` varchar(100) NOT NULL,
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
  `preparation` text NOT NULL,
  `cooking` text NOT NULL,
  `estimated` text NOT NULL,
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


INSERT INTO `authentications` (`id`, `username`, `password`, `role`, `status`) VALUES 
('acf0c71e-a204-4c14-9da9-a3ee4b9445c7', 'hHDzwaN7VXffIMW+R0dFPg==', '$2a$11$XyMVXIkU1HycGa44HkpEduFlJW8XuG3H4yGk52ix8q4cYFk558RWW', 'Admin', true);

INSERT INTO `users` (`id`, `idAuthentication`, `firstName`, `lastName`, `email`, `createdAt`, `updatedAt`) VALUES 
('d0943044-2476-4ba1-bc77-d75b492217ab', 'acf0c71e-a204-4c14-9da9-a3ee4b9445c7', 'John', 'Doe Bash', 'Pv48lM21RyLEJsuI96CItnHLqwDdomzO7q1eGUAxQSI=', NOW(), NOW());

INSERT INTO categories (id, name, description) VALUES 
('6ffef303-f76b-478a-b9d4-ee912c6c0bcd', 'Postres', 'Recetas de postres deliciosos.'),
('b94b3f0c-3a6a-4a89-a840-722d6cfb1e9f', 'Entrantes', 'Recetas de entrantes deliciosos.');

INSERT INTO recipes (id, idCategory, title, description, instruction, ingredient, preparation, cooking, estimated, difficulty, createdAt, updatedAt) VALUES 
('e63dd814-ef78-489c-a127-806c1df2d22d', '6ffef303-f76b-478a-b9d4-ee912c6c0bcd', 'Tarta de Chocolate', 'Deliciosa tarta de chocolate con capas de crema.', 'Mezclar ingredientes, hornear, decorar.', 'Harina, huevos, chocolate, azúcar, mantequilla.', NOW(), NOW(), NOW(), 'Difficult', NOW(), NOW()),
('459e58c4-c303-4b02-9991-c49780a4a2d6', '6ffef303-f76b-478a-b9d4-ee912c6c0bcd', 'Cheesecake de Fresa', 'Cheesecake cremoso con fresas frescas.', 'Mezclar ingredientes, hornear, refrigerar, decorar con fresas.', 'Harina, huevos, fresas, azúcar, queso crema.', NOW(), NOW(), NOW(), 'Half', NOW(), NOW()),
('bd763f60-60f7-4b87-b740-5f3061e1f8ed', 'b94b3f0c-3a6a-4a89-a840-722d6cfb1e9f', 'Bruschetta Clásica', 'Pan tostado con tomate, ajo y albahaca.', 'Tostar pan, mezclar tomate, ajo y albahaca, servir.', 'Pan, tomate, ajo, albahaca, aceite de oliva.', NOW(), NOW(), NOW(), 'Easy', NOW(), NOW()),
('928ab6f7-4b3d-4d75-bd7d-eda2c0d92b23', 'b94b3f0c-3a6a-4a89-a840-722d6cfb1e9f', 'Hummus con Pita', 'Delicioso hummus servido con pan de pita.', 'Mezclar ingredientes, servir con pita.', 'Garbanzos, ajo, tahini, limón, pita.', NOW(), NOW(), NOW(), 'Easy', NOW(), NOW()),
('a1234567-b89c-40d2-e345-f67890abcdef', 'b94b3f0c-3a6a-4a89-a840-722d6cfb1e9f', 'Pasta Alfredo', 'Una rica pasta Alfredo con una salsa cremosa y un toque de parmesano.', 'Cocinar la pasta según las instrucciones del paquete. En una sartén, derretir la mantequilla y añadir la crema. Cocinar a fuego lento y agregar queso parmesano. Mezclar con la pasta.', 'Pasta, mantequilla, crema, queso parmesano, ajo, sal, pimienta.', '10 minutos', '20 minutos', '30 minutos', 'Half', NOW(), NOW()),
('c53205fb-179a-45e6-8cb1-00f841c86607', '6ffef303-f76b-478a-b9d4-ee912c6c0bcd', 'Receta de Tarta de Manzana', 'Una deliciosa tarta de manzana con un toque de canela.', 'Precalentar el horno a 180°C. Mezclar todos los ingredientes y hornear por 40 minutos.', 'Manzanas, harina, azúcar, canela, mantequilla, huevos.', '15 minutos', '40 minutos', '55 minutos', 'Easy', NOW(), NOW());

INSERT INTO images (id, idRecipe, url, createdAt, updatedAt) VALUES 
('9c299e59-1e78-4c6f-842f-49250700de59', 'e63dd814-ef78-489c-a127-806c1df2d22d', 'https://res.cloudinary.com/dnh7ti6pj/image/upload/v1723379671/recipes/rev2m49vym87wisst5tg.jpg', NOW(), NOW()),
('045d0e80-21e6-4a39-928f-3cb8e94fc962', 'e63dd814-ef78-489c-a127-806c1df2d22d', 'https://res.cloudinary.com/dnh7ti6pj/image/upload/v1723379669/recipes/fcpkhdxlqxilk4mpdcxt.jpg', NOW(), NOW()),
('be3eae49-1b7e-4433-bb08-6d517daba31b', '459e58c4-c303-4b02-9991-c49780a4a2d6', 'https://res.cloudinary.com/dnh7ti6pj/image/upload/v1723379668/recipes/siaxcyij4wpv4z4wjpsy.jpg', NOW(), NOW()),
('bc9e5147-0643-449a-b3f9-33ca878974db', '459e58c4-c303-4b02-9991-c49780a4a2d6', 'https://res.cloudinary.com/dnh7ti6pj/image/upload/v1723379667/recipes/npapq3ybdxp6cuy3sfia.jpg', NOW(), NOW()),
('4a09d1f8-9d6c-4562-b5b7-72a83d5d6fcd', 'bd763f60-60f7-4b87-b740-5f3061e1f8ed', 'https://res.cloudinary.com/dnh7ti6pj/image/upload/v1723379667/recipes/tiv64xz5fnwdfymbrjol.jpg', NOW(), NOW()),
('e4957dbb-3789-4f4c-b6f5-1a9a5e3cfeb8', 'a1234567-b89c-40d2-e345-f67890abcdef', 'https://res.cloudinary.com/dnh7ti6pj/image/upload/v1724471461/recipes/snsnjsfjx9mszkdc7ijj.jpg', NOW(), NOW()),
('a1234567-b89c-40d2-e345-f67890abcdef', 'a1234567-b89c-40d2-e345-f67890abcdef', 'https://res.cloudinary.com/dnh7ti6pj/image/upload/v1724471461/recipes/ie6zderil9yn6ypw8qyp.avif', NOW(), NOW()),
('e1068eab-7ddc-41cb-96b9-3506e45a7503', 'c53205fb-179a-45e6-8cb1-00f841c86607', 'https://res.cloudinary.com/dnh7ti6pj/image/upload/v1724462461/recipes/ku1vwszqazxkdy4riyfk.jpg', NOW(), NOW());


INSERT INTO videos (id, idRecipe, title, url, description, createdAt, updatedAt) VALUES 
('84e2923c-48ef-4d57-8855-f0589fe6629f', 'e63dd814-ef78-489c-a127-806c1df2d22d', 'Preparación Tarta de Chocolate', 'https://www.youtube.com/watch?v=ZLhMNd8kgwM', 'Video paso a paso para preparar la tarta de chocolate.', NOW(), NOW()),
('d556a70f-0f9f-4182-9fcf-25aaf551a68c', '459e58c4-c303-4b02-9991-c49780a4a2d6', 'Preparación Cheesecake de Fresa', 'https://www.youtube.com/watch?v=fEkQ1hIp_FE', 'Video tutorial para preparar cheesecake de fresa.', NOW(), NOW()),
('b49e9f29-cf1c-4a1b-88ea-4c287cd216af', 'bd763f60-60f7-4b87-b740-5f3061e1f8ed', 'Preparación Bruschetta Clásica', 'https://www.youtube.com/watch?v=6rC_I879dOY', 'Video paso a paso para preparar bruschetta clásica.', NOW(), NOW()),
('d19f2cb3-732e-48e0-92e6-9b7e0342dcf5', '928ab6f7-4b3d-4d75-bd7d-eda2c0d92b23', 'Preparación Hummus con Pita', 'https://www.youtube.com/watch?v=S2SD5X2hy2k', 'Video tutorial para preparar hummus con pita.', NOW(), NOW()),
('3a76a37a-13c1-4860-9493-bcf8d19453eb', 'c53205fb-179a-45e6-8cb1-00f841c86607', 'Video de Preparación de Tarta de Manzana', 'https://www.youtube.com/watch?v=jhQzux1wGwg', 'Video paso a paso de cómo preparar una deliciosa tarta de manzana.', NOW(), NOW());

INSERT INTO ratings (id, idRecipe, numberLike, createdAt, updatedAt) VALUES 
('fbab1d42-94a3-4f89-a1c8-f3e65f1e5b7d', 'e63dd814-ef78-489c-a127-806c1df2d22d', 10, NOW(), NOW()),
('cc3d9d19-c635-4533-b44d-d9a89ecfced9', '459e58c4-c303-4b02-9991-c49780a4a2d6', 15, NOW(), NOW()),
('b2280fc1-4aeb-4d77-88f2-5dfafdfaad1b', 'bd763f60-60f7-4b87-b740-5f3061e1f8ed', 20, NOW(), NOW()),
('dcd5f249-7756-4a7e-92e7-19c383f0ff17', '928ab6f7-4b3d-4d75-bd7d-eda2c0d92b23', 11, NOW(), NOW());

