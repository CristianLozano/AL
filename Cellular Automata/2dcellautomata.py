import random
import numpy as np
w,h = 10,10
automaton = [[0 for x in range(w)] for y in range(h)]
print(automaton)
i = 0

for i in range(h):
    for j in range(w):
        automaton[i][j] = random.randint(0,1)

print(automaton)

#rules = {(0,0,0): 0,(0,0,1): 1,(0,1,0): 1,(0,1,1): 1,(1,0,0): 1,(1,0,1): 1,(1,1,0): 1,(1,1,1): 1}

def apply_2d_rule():
    copied_matrix = automaton.copy()

    for i in range(h):

        for j in range(w):
            neighbor_sum = 0
            if i == 0 and j == 0:
                neighbor_sum += copied_matrix[0][1] + copied_matrix[1][1] + copied_matrix[1][0] + copied_matrix[0][w-1] + copied_matrix [1][w-1] + copied_matrix[h-1][w-1] + copied_matrix[h-1][0] + copied_matrix[h-1][1]
                print(neighbor_sum)
            elif i == 0 and j == w-1:
                neighbor_sum += copied_matrix[0][0] + copied_matrix[0][1] + copied_matrix[0][w-2] + copied_matrix[h - 1][w - 1] + copied_matrix[1][w - 2] + copied_matrix[1][w - 1] + copied_matrix[h - 1][0] + copied_matrix[h - 1][w - 2]
            elif i == h-1 and j == 0:
                neighbor_sum += copied_matrix[0][0] + copied_matrix[0][1] + copied_matrix[0][w-1] + copied_matrix[h-2][0] + copied_matrix[h - 2][1] + copied_matrix[h - 2][w - 1] + copied_matrix[h - 1][1] + copied_matrix[h - 1][w - 1]
            elif i == h - 1 and j == w - 1:
                neighbor_sum += copied_matrix[0][0] + copied_matrix[0][w-2] + copied_matrix[0][w-1] + copied_matrix[h-2][0] + copied_matrix[h-2][w - 2] + copied_matrix[h - 2][w - 1] + copied_matrix[h - 1][0] + copied_matrix[h - 1][w-2]
            elif i == 0:
                neighbor_sum += copied_matrix[i][j-1] + copied_matrix[i][j+1] + copied_matrix[i+1][j-1] + copied_matrix[i+1][j] + copied_matrix[i+1][j + 1] + copied_matrix[h-1][j - 1] + copied_matrix[h - 1][j] + copied_matrix[h - 1][j+1]
            elif j == 0:
                neighbor_sum += copied_matrix[i-1][j] + copied_matrix[i+1][j] + copied_matrix[i-1][j+1] + copied_matrix[i][
                    j + 1] + copied_matrix[i+1][j + 1] + copied_matrix[i - 1][w - 1] + copied_matrix[i][w-1] + \
                                copied_matrix[i + 1][w-1]
            elif j == w - 1:
                neighbor_sum += copied_matrix[i-1][0] + copied_matrix[i][0] + copied_matrix[i+1][0] + copied_matrix[i-1][
                    w - 2] + copied_matrix[i][w - 2] + copied_matrix[i + 1][w - 2] + copied_matrix[i - 1][j] + \
                                copied_matrix[i + 1][j]
            elif i == h - 1:
                neighbor_sum += copied_matrix[0][j-1] + copied_matrix[0][j] + copied_matrix[0][j+1] + copied_matrix[i-1][
                    j - 1] + copied_matrix[i-1][j] + copied_matrix[i - 1][j+1] + copied_matrix[i][j-1] + \
                                copied_matrix[i][j+1]
            else:
                neighbor_sum += copied_matrix[i-1][j-1] + copied_matrix[i-1][j] + copied_matrix[i-1][j+1] + copied_matrix[i][
                    j - 1] + copied_matrix[i][j + 1] + copied_matrix[i+1][j- 1] + copied_matrix[i+1][j] + \
                                copied_matrix[i+1][j+1]

    return copied_matrix

apply_2d_rule()
print(np.matrix(apply_2d_rule()))

#def apply_rule():
#   copy_states = automaton.copy()
  #  j = 0
   # for j in range(len(automaton)):
    #    if j == 0:
     #       neighbors = (copy_states[len(automaton)-1], copy_states[j], copy_states[j+1])
      #  elif j == len(automaton)-1:
       #     neighbors = (copy_states[len(automaton)-2], copy_states[len(automaton)-1], copy_states[0])
        #else:
         #   neighbors = (copy_states[j-1], copy_states[j], copy_states[j+1])
        #automaton[j] = rules.get(neighbors)
    #return automaton

#for k in range(1000):
 #   print(''.join(str(e) for e in apply_rule()))
