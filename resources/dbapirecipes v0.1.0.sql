
CREATE TABLE `authentications` (
  `id` varchar(13) PRIMARY KEY NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `role` ENUM ('Admin', 'Other', 'Logged') NOT NULL,
  `status` BOOLEAN NOT NULL
);

CREATE TABLE `users` (
  `id` varchar(13) NOT NULL,
  `idAuthentication` varchar(13) NOT NULL,
  `firstName` varchar(100) NOT NULL,
  `lastName` varchar(100) NOT NULL,
  `email` varchar(50) UNIQUE NOT NULL,
  `createdAt` timestamp NOT NULL,
  `updatedAt` timestamp NOT NULL,
  PRIMARY KEY (`id`),
  FOREIGN KEY (`idAuthentication`) REFERENCES `authentications`(`id`)
);

CREATE TABLE `recipes` (
  `id` varchar(13) PRIMARY KEY NOT NULL,
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
  `createdBy` varchar(13) NULL,
  `updatedBy` varchar(13) NULL
);

CREATE TABLE `likes` (
  `idRecipe` varchar(13) NOT NULL,
  `idUser` varchar(13) NOT NULL,
  PRIMARY KEY (`idRecipe`, `idUser`),
  FOREIGN KEY (`idRecipe`) REFERENCES `recipes` (`id`),
  FOREIGN KEY (`idUser`) REFERENCES `users` (`id`)
);

CREATE TABLE `ratings` (
  `id` varchar(13) PRIMARY KEY NOT NULL,
  `idRecipe` varchar(13) NOT NULL,
  `comment` text NOT NULL,
  `numberLike` BIGINT DEFAULT 0,
  `createdAt` timestamp NOT NULL,
  `updatedAt` timestamp NOT NULL,
  FOREIGN KEY (`idRecipe`) REFERENCES `recipes` (`id`)
);

CREATE TABLE `images` (
  `id` varchar(13) PRIMARY KEY NOT NULL,
  `idRecipe` varchar(13) NOT NULL,
  `data` BLOB NOT NULL,
  `extension` varchar(5) NOT NULL,
  `createdAt` timestamp NOT NULL,
  `updatedAt` timestamp NOT NULL,
  FOREIGN KEY (`idRecipe`) REFERENCES `recipes` (`id`)
);

CREATE TABLE `videos` (
  `id` varchar(13) PRIMARY KEY NOT NULL,
  `idRecipe` varchar(13) NOT NULL,
  `title` varchar(100) NOT NULL,
  `data` LONGBLOB NOT NULL,
  `extension` varchar(5) NOT NULL,
  `description` text,
  `createdAt` timestamp NOT NULL,
  `updatedAt` timestamp NOT NULL,
  FOREIGN KEY (`idRecipe`) REFERENCES `recipes` (`id`)
);

CREATE TABLE `news` (
  `id` varchar(13) PRIMARY KEY NOT NULL,
  `idRecipe` varchar(13) NOT NULL,
  `title` text NOT NULL,
  `subtitle` text NOT NULL,
  `content` text NOT NULL,
  `status` BOOLEAN NOT NULL,
  `createdAt` timestamp NOT NULL,
  `deletedAt` timestamp NOT NULL,
  FOREIGN KEY (`idRecipe`) REFERENCES `recipes` (`id`)
);