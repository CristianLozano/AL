from turtle import *
import random
turtle = Turtle()

screensize(400, 400,None)
setworldcoordinates(0, 400, 400, 0)
mode("world")
x = random.randint(0,400)
y = random.randint(0,400)
v1 = (200,0)
v2 = (0,400)
v3 = (400,400)
turtle.up()
turtle.hideturtle()
done = False
i = 0
while True:
    choice = random.randint(1,3)
    if choice == 1:
        x = int((x+v1[0])/2)
        y = int((y+v1[1])/2)
        turtle.setpos(x,y)
        turtle.dot()
    elif choice == 2:
        x = int((x+v2[0])/2)
        y = int((y+v2[1])/2)
        turtle.setpos(x,y)
        turtle.dot()
    elif choice == 3:
        x = int((x+v3[0])/2)
        y = int((y+v3[1])/2)
        turtle.setpos(x,y)
        turtle.dot()

