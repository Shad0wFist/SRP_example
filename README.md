# Simple SRP example

This repo contains Unity sample (for 2021.3.19f1 version) which uses simple SRP implementation based on 3 first chapters of [Catlike Coding](https://catlikecoding.com/unity/tutorials/custom-srp/) tutors.

Examples of basic lit and unlit materials can be found in **Main** scene.

![MaterialExamples](ExampleImages/main_scene.png "Material Examples")

Imported car model is placed in **Car** scene.

![CarScene](ExampleImages/car_scene.png "Car Scene")

1. Implemented damage to the car body using Unity physics - the collision speed of the car with the object is calculated, after which the MeshFilter is changed.
2. New and modified existing materials for car parts, shadows, lighting and other rendering elements for car visualization have been added.
3. A scene has been assembled showing the damage: the environment, the objects on which the car crashes - pillars, a wall and a springboard. Added textures, sky, spark effects when colliding with objects, smoke and tire tracks.
