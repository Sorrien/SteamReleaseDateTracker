When executed in a folder with "appids.txt" it will take each line of the file (each of which should be steam app id) and check for the release date of the games.

It will print something like this to the console:
Feb:
HELLDIVERS 2 (8th)

Sep:
S.T.A.L.K.E.R. 2: Heart of Chornobyl (5th)
Warhammer 40,000: Space Marine 2 (9th)

Quarters:
Deep Rock Galactic: Survivor (Q1 2024)
Out of Action (Q4 2024)

Only release year is known:
Phantom Fury (2024)

It will also save the data to a JSON file. On subsequent runs it will compare the data and notify if any release dates have changed by printing to the console and saving a notification file.
