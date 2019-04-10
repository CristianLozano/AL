import pygame
import math
import random

#Parameters of sierpinski, width and height of screen and number of points to iterate.
width = 400
height = 400
points = 10

#Library pygame start, seting of screen and color filler (r,g,b), parameter black for points.
pygame.init()
screen = pygame.display.set_mode((width, height))
currentColor = (255,255,255)
screen.fill(currentColor)
black = (0,0,0)


#Setting center of circle in middle of screen and radius of less distance.
center_x = width/2
center_y = height/2

if width < height:
    radius = width/2
else:
    radius = height/2


#Generation of random start in the screen size.
x = random.randint(1,width)
y = random.randint(1,height)

#Array of positions of points, tuples.
positions = []


#Generation of positions of points in the circle.
for i in range(points):

    #Angle of the circunference, start at x = 0 y = width/2
    ang = ((i * math.pi * 2) / points) - (math.pi/2)
    dx = int(math.cos(ang) * radius)
    dy = int(math.sin(ang) * radius)

    #Real positions of x and y of points.
    x1 = center_x + dx
    y1 = center_y + dy

    #Add position of point to array in tuple.
    positions.append((x1,y1))

#Start of pygame graphing
done = False
 
while not done:

        #Select random start point to move
        step = random.randint(1,points)

        #Define the new point in the screen, depends on the points defined to give the divition.
        x = (int(((x+positions[step - 1][0]))/ (points - 1)))
        y = (int(((y+positions[step - 1][1]))/ (points - 1)))

        #Draw a point in the new point position
        screen.set_at((x*(points - 2),y*(points - 2)),black)

        #Pygame control of window quit/close
        for event in pygame.event.get():
                if event.type == pygame.QUIT:
                        done = True

        #Pygame display refresh, updates every frame.
        pygame.display.flip()
