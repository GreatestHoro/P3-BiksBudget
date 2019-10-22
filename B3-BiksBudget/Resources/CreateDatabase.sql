CREATE TABLE IF NOT EXISTS `Recipes` (
                                     `id` INT AUTO_INCREMENT,
                                     `recipe` VARCHAR(255) UNIQUE,
                                     PRIMARY KEY(id));
                                     
                                     
CREATE TABLE IF NOT EXISTS `Ingredients` (
                                     `id` INT AUTO_INCREMENT,
                                     `ingridient` VARCHAR(255) UNIQUE,
                                     PRIMARY KEY(id));


CREATE TABLE IF NOT EXISTS `IngridientsInRecipes` (
                                     `id` INT auto_increment,
                                     `recipe` varchar(255),
                                     `ingridient` varchar(255),
                                     primary key(id), 
                                     FOREIGN KEY (recipe) REFERENCES Recipes(recipe), 
                                     foreign key (ingridient) REFERENCES Ingredients(ingridient));


                                     