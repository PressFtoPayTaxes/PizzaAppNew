INSERT INTO Pizza
VALUES('TomYamPizza', 'TomYamSauce Mocarella Pepper Tomatoes Mushrooms Chicken Shimps', 2250, 25)
INSERT INTO Pizza
VALUES('TomYamPizza', 'TomYamSauce Mocarella Pepper Tomatoes Mushrooms Chicken Shimps', 3350, 30)
INSERT INTO Pizza
VALUES('TomYamPizza', 'TomYamSauce Mocarella Pepper Tomatoes Mushrooms Chicken Shimps', 3950, 35)

INSERT INTO Pizza
VALUES('Salsa', 'Pepperoni Ketchup Mushrooms Mocarella SalsaSauce', 2050, 25)
INSERT INTO Pizza
VALUES('Salsa', 'Pepperoni Ketchup Mushrooms Mocarella SalsaSauce', 2950, 30)
INSERT INTO Pizza
VALUES('Salsa', 'Pepperoni Ketchup Mushrooms Mocarella SalsaSauce', 3750, 35)

INSERT INTO Pizza
VALUES('Cheese chicken', 'CheeseSauce, Chicken, Mocarella, Tomatoes', 2050, 25)
INSERT INTO Pizza
VALUES('Cheese chicken', 'CheeseSauce, Chicken, Mocarella, Tomatoes', 2950, 30)
INSERT INTO Pizza
VALUES('Cheese chicken', 'CheeseSauce, Chicken, Mocarella, Tomatoes', 3750, 35)

UPDATE Pizza
SET FillingList = 'CheeseSauce Chicken Mocarella Tomatoes'
WHERE Name = 'Cheese chicken'