from sklearn import neighbors
import numpy as np

# read csv file into numpy
x = np.genfromtxt("D:\\VSTS\\Repos\\HouseData\\x.csv", dtype=np.float64, delimiter=',', skip_header=1)
y = np.genfromtxt("D:\\VSTS\\Repos\\HouseData\\y.csv", dtype=np.float64, delimiter=',', skip_header=1)

def lnglatWeights(row,multipler):
    return [row[0],row[1],row[2],row[3]*multipler,row[4]*multipler];

geo_rate = 100000000.

x = np.apply_along_axis(lnglatWeights, 1, x,geo_rate )
print(x)
print(y)


knc = neighbors.KNeighborsClassifier(algorithm='auto')

knc.fit(x, y)

# try example:

# 76 Valhalla Street Sunnybank Qld 4109
# 4,3,2,153.064843,-27.5753528

results = knc.kneighbors([[4,3,2,-27.5753528 * geo_rate, 153.064843 * geo_rate]])

print("neighbors for 76 Valhalla Street Sunnybank Qld 4109: ", results[1][0])
