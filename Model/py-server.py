from concurrent import futures
import time

import grpc

import prediction_pb2
import prediction_pb2_grpc

from sklearn import neighbors
import numpy as np

_ONE_DAY_IN_SECONDS = 60 * 60 * 24


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


class PredictionServer(prediction_pb2_grpc.PredictionServiceServicer):

    def FindNearestHouseIndices(self, request, context):
        print(request)
        results = knc.kneighbors([[request.NumberOfBedrooms,
                                   request.NumberOfBathrooms,
                                   request.NumberOfParkings,
                                   request.Latitude * geo_rate, 
                                   request.Longitude * geo_rate]])
        return prediction_pb2.PredictionResponse(Indices=results[1][0])

port = 51666

def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=1))
    prediction_pb2_grpc.add_PredictionServiceServicer_to_server(PredictionServer(), server)
    server.add_insecure_port('[::]:{}'.format(port))
    server.start()
    print("Prediction Server: {}".format(51666))
    try:
        while True:
            time.sleep(_ONE_DAY_IN_SECONDS)
    except KeyboardInterrupt:
        server.stop(0)


if __name__ == '__main__':
    serve()
