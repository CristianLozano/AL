import pygame
import random


width = 400
height = 400

pygame.init()
screen = pygame.display.set_mode((width, height))
currentColor = (255,255,255)


x = random.randint(1,400)
y = random.randint(1,400)

screen.fill(currentColor)
black = (0,0,0)

x12 = (200,0)
x34 = (400,400)
x56 = (0,400)
#x78 = (0,400)

done = False
 
while not done:

        step = random.randint(1,6)

        #screen.set_at(point,black)
        #pygame.draw.line(screen, black, (250,250), (350,250))

        if step == 1 or step == 2:
            x = int((x+x12[0])/ 2)
            y = int((y+x12[1])/ 2)
            screen.set_at((x,y),black)
        elif step == 3 or step == 4:
            x = int((x+x34[0])/ 2)
            y = int((y+x34[1])/ 2)
            screen.set_at((x,y),black)
        elif step == 5 or step == 6:
            x = int((x+x56[0])/ 2)
            y = int((y+x56[1])/ 2)
            screen.set_at((x,y),black)
        #elif step == 7 or step == 8:
        #   x = int(2*(x+x78[0])/3)
        #   y = int(2*(y+x78[1])/3)
        #   screen.set_at((x,y),black)


        for event in pygame.event.get():
                if event.type == pygame.QUIT:
                        done = True

        pygame.display.flip()



