CREATE DATABASE `dbapirecipe`
CHARACTER SET utf8mb4
COLLATE utf8mb4_spanish_ci;


CREATE TABLE `authentications` (
  `id` char(36) PRIMARY KEY,
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

CREATE TABLE `recipes` (
  `id` char(36) PRIMARY KEY,
  `title` varchar(100) NOT NULL,
  `description` text NOT NULL,
  `instruction` text NOT NULL,
  `ingredient` text NOT NULL,
  `preparation` timestamp NOT NULL,
  `cooking` timestamp NOT NULL,
  `estimated` timestamp NOT NULL,
  `difficulty` ENUM ('easy', 'half', 'difficult') NOT NULL,
  `createdAt` timestamp NOT NULL,
  `updatedAt` timestamp NOT NULL,
  `createdBy` char(36) NULL,
  `updatedBy` char(36) NULL
);

CREATE TABLE `likes` (
  `idRecipe` char(36) NOT NULL,
  `idUser` char(36) NOT NULL,
  FOREIGN KEY (`idRecipe`) REFERENCES `recipes` (`id`),
  FOREIGN KEY (`idUser`) REFERENCES `users` (`id`)
  PRIMARY KEY (`idRecipe`, `idUser`)
);

alter table `likes`
	add constraint `checkLikesId`
	check (`idRecipe` <> `idUser`);

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
  `data` BLOB NOT NULL,
  `extension` varchar(5) NOT NULL,
  `createdAt` timestamp NOT NULL,
  `updatedAt` timestamp NOT NULL,
  FOREIGN KEY (`idRecipe`) REFERENCES `recipes` (`id`)
);

CREATE TABLE `videos` (
  `id` char(36) PRIMARY KEY,
  `idRecipe` char(36) NOT NULL,
  `title` varchar(100) NOT NULL,
  `data` LONGBLOB NOT NULL,
  `extension` varchar(5) NOT NULL,
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

-- Inserción en la tabla authentications
INSERT INTO `authentications` (`id`, `username`, `password`, `role`, `status`)
VALUES 
('374949e5-e7c7-4d2a-84b4-42478ef1648a', 'lionos', '1001linux', 'Admin', true),
('8f59ff5a-8bd2-4120-ab7c-8335a855cb4b', 'rocos', '1001linux', 'Other', false);

-- Inserción en la tabla users
INSERT INTO `users` (`id`, `idAuthentication`, `firstName`, `lastName`, `email`, `createdAt`, `updatedAt`)
VALUES 
('09e9f526-6727-48f0-bd9f-5562d9ff9f71', '374949e5-e7c7-4d2a-84b4-42478ef1648a', 'John', 'Doe', 'john.doe@example.com', NOW(), NOW()),
('5f0e139c-f711-431e-b16b-2fd704f53a85', '8f59ff5a-8bd2-4120-ab7c-8335a855cb4b', 'Jane', 'Smith', 'jane.smith@example.com', NOW(), NOW());

