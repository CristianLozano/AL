import numpy as np
from matplotlib import pyplot as plt

def henon(a,b,x0):

    return a - x0^2 + b*x0
x0 = 3
a = 1
b = 1

while True:
    y = henon(a,b,x0)
    plt.plot([x0,y])
    x0 = y





