import random
import numpy as np
w,h = 10,10
automaton = [0] * 100
for i in range(len(automaton)):
        automaton[i] = random.randint(0,1)
num = 31
seed = [0] * 8
seed = [int(x) for x in bin(num)[2:]]
print(seed)
rules = {(0,0,0): seed[0],(0,0,1): seed[1],(0,1,0): seed[2],(0,1,1): seed[3],(1,0,0): seed[4],(1,0,1): seed[5],(1,1,0): seed[6],(1,1,1): seed[7]}


def apply_rule():
   copy_states = automaton.copy()
   for j in range(len(automaton)):
        if j == 0:
            neighbors = (copy_states[len(automaton)-1], copy_states[j], copy_states[j+1])
        elif j == len(automaton)-1:
            neighbors = (copy_states[len(automaton)-2], copy_states[len(automaton)-1], copy_states[0])
        else:
            neighbors = (copy_states[j-1], copy_states[j], copy_states[j+1])
        automaton[j] = rules.get(neighbors)
   return automaton

for k in range(1000):
    print(''.join(str(e) for e in apply_rule()))
