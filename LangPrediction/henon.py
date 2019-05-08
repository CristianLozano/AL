import numpy as np
from matplotlib import pyplot as plt


def henon(a,b,x0):

    return a - x0^2 + b*x0


x0 = 3
a = 1
b = 2
i = 0
points = list()

done = False
while i < 100:
    y = henon(a, b, x0)
    points.append((x0,y))
    x0 = y
    i += 1
print(points)
plt.plot(points)
plt.show()


