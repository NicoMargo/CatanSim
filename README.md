# CatanSim
A simplified simulation of "The Settlers of Catan" using C# and .NET 8, analyzing different house placement strategies with game theory and Monte Carlo simulations.

Project Description for "Models and Simulation" Course at University of Belgrano 2024
Project Title: Simplified Simulation of "The Settlers of Catan"
Objective:
The objective of this project is to create a simplified simulation of the popular board game "The Settlers of Catan" using C# and .NET 8. The simulation will serve as a tool to apply and analyze various game strategies using game theory and Monte Carlo simulations. The goal is to determine the most effective strategy for resource collection in the game.

Project Overview:
In this project, we will develop a graphical user interface (GUI) to represent the Catan game board, which consists of 19 hexagonal tiles. Each tile will be hardcoded with a specific resource and number. Players will place two initial houses on the board according to different strategies. The strategies will be evaluated based on their performance in resource collection after a series of dice rolls.

Strategies Implemented:
Random Placement: Houses are placed randomly on the board.
Best Number Probability: Houses are placed on intersections with the highest probability of producing resources based on the numbers assigned to adjacent tiles.
Best Resources: Houses are placed on intersections adjacent to tiles with the most diverse resources.
Best Resources and Number Probability: Houses are placed on intersections that balance resource diversity and high probability numbers.
Simulation Process:
Initialization: The board is initialized with 19 tiles, each assigned a specific resource (wood, brick, sheep, wheat, ore, or desert) and a number.
House Placement: Each player places two houses on the board according to their strategy.
Dice Rolls: The simulation runs 25 dice rolls. For each roll, resources are collected based on the numbers on the tiles and the locations of the houses.
Resource Collection: Players accumulate resources (wood, brick, sheep, wheat, and ore) based on the outcomes of the dice rolls.
Performance Evaluation: The strategy that reaches 10 units of each resource in the fewest number of dice rolls is considered the most effective.
GUI Features:
Board Display: The board is visually represented with hexagonal tiles.
House Display: Houses are displayed at intersections, colored according to the player's strategy.
Dice Roll Results: A list of dice roll results is displayed in the GUI.
Resource Count: The current resources of each player are displayed, along with their strategy and color.
