import sys
import re
from collections import Counter
from collections import OrderedDict
import matplotlib.pyplot as plt

# frequency of letters in both languages

spanish = {"e": 0.1368,
           "a": 0.1253,
           "o": 0.0868,
           "s": 0.0798,
           "r": 0.0687,
           "n": 0.0671,
           "i": 0.0625,
           "d": 0.0586,
           "l": 0.0497,
           "c": 0.0468,
           "t": 0.0463,
           "u": 0.0393,
           "m": 0.0315,
           "p": 0.0251,
           "b": 0.0142,
           "g": 0.0101,
           "v": 0.090,
           "y": 0.090,
           "q": 0.088,
           "h": 0.070,
           "f": 0.069,
           "z": 0.052,
           "j": 0.044,
           "ñ": 0.031,
           "x": 0.022,
           "k": 0.002,
           "w": 0.001
           }

english = {"e": 0.12702,
           "t": 0.09056,
           "a": 0.08167,
           "o": 0.07507,
           "i": 0.06966,
           "n": 0.06749,
           "s": 0.06327,
           "h": 0.06094,
           "r": 0.05987,
           "d": 0.04253,
           "l": 0.04025,
           "c": 0.02782,
           "u": 0.02758,
           "m": 0.02406,
           "w": 0.02360,
           "f": 0.02228,
           "g": 0.02015,
           "y": 0.01974,
           "p": 0.01929,
           "b": 0.01492,
           "v": 0.0978,
           "k": 0.0772,
           "j": 0.0153,
           "x": 0.0150,
           "q": 0.0095,
           "z": 0.0074
           }

# read text from file

input_file = sys.argv[1]

# open file read only

text = open(input_file, "r")

# make it all lower case

string = text.read().lower()

# remove garbage

string = re.sub('[^A-Za-z]+', '', string)

# count letter frequency

letter_count = Counter(string)

# get total amount of letters in text

total = sum(letter_count.values())

# calculate letter frequency

frequency = {k: v / total for total in (sum(letter_count.values()),) for k, v in letter_count.items()}

# get % in 100s
# for key in frequency:
#   frequency[key] *= 100
# print(letter_count)
# print(total)
# print(frequency)

# predict which language is the text in

sorted_freq = OrderedDict(sorted(frequency.items(), key=lambda kv: kv[1], reverse=True))

#print(sorted_freq)


def predict(lang1, lang2, input):

    diff_of_freq1 = {k: abs(lang1.get(k, 0) - input.get(k, 0)) for k in set(lang1) | set(input)}
    score1 = sum(diff_of_freq1.values())
    diff_of_freq2 = {k: abs(lang2.get(k, 0) - input.get(k, 0)) for k in set(lang2) | set(input)}
    score2 = sum(diff_of_freq2.values())
    if score1 <= score2:
        return "spanish"
    else:
        return "english"


print(predict(spanish, english, frequency))

# plot letter frequency in text
plt.title("Letter Frequency")
plt.bar(range(len(frequency)), list(frequency.values()), align='center')
plt.xticks(range(len(frequency)), list(frequency.keys()))
plt.show()

# sacar frecuencia en % del texto? no pero ya esta eso, en lugar de frecuencia, orden?