import random

automaton = [0] * 80
i = 0

for i in range(len(automaton)):
    automaton[i] = random.randint(0,1)

rules = {(0,0,0): 0,(0,0,1): 1,(0,1,0): 1,(0,1,1): 1,(1,0,0): 1,(1,0,1): 1,(1,1,0): 1,(1,1,1): 1}
def apply_rule():
    copy_states = automaton.copy()
    j = 0
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
