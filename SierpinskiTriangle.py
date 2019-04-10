import pygame
import math
import random


width = 400
height = 400
points = 3

pygame.init()
screen = pygame.display.set_mode((width, height))
currentColor = (255,255,255)
screen.fill(currentColor)
black = (0,0,0)


# draw a circle
center_x = width/2
center_y = height/2

if width < height:
    radius = width/2
else:
    radius = height/2




x = random.randint(1,width)
y = random.randint(1,height)

positions = []



for i in range(points):
    ang = ((i * math.pi * 2) / points) - (math.pi/2)
    dx = int(math.cos(ang) * radius)
    dy = int(math.sin(ang) * radius)
    #print("dx ", dx, " dy ", dy)
    x1 = center_x + dx
    y1 = center_y + dy
    #print("x1 ", x1, " y1 ", y1)
    positions.append((x1,y1))

print(positions)

#x12 = (400,0)
#x56 = (800,800)
#x34 = (0,800)
#x78 = (0,400)

done = False
 
while not done:

        step = random.randint(1,points)

        #screen.set_at(point,black)

        x = (int(((x+positions[step - 1][0]))/ (points - 1)))#*(points - 2)
        y = (int(((y+positions[step - 1][1]))/ (points - 1)))#*(points - 2)
        screen.set_at((x*(points - 2),y*(points - 2)),black)

        '''
        if step == 1:
            x = int((x+positions[0][0])/ 2)
            y = int((y+positions[0][1])/ 2)
            screen.set_at((x,y),black)    
        elif step == 2:
            x = int((x+positions[1][0])/ 2)
            y = int((y+positions[1][1])/ 2)
            screen.set_at((x,y),black)
        elif step == 3 :
            x = int((x+positions[2][0])/ 2)
            y = int((y+positions[2][1])/ 2)
            screen.set_at((x,y),black)
        elif step == 4 :
            x = int(2*(x+positions[3][0])/2)
            y = int(2*(y+positions[3][1])/2)
            screen.set_at((x,y),black)
        '''

        for event in pygame.event.get():
                if event.type == pygame.QUIT:
                        done = True

        pygame.display.flip()



